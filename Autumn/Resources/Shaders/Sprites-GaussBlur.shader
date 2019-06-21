// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/GaussBlur"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _BlurRadius ("BlurRadius", Range(1, 15)) = 5
        _TextureSize ("TextureSize", Float) = 256
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
		#pragma multi_compile _ PIXELSNAP_ON
		#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
		#include "UnitySprites.cginc"
        #include "UnityCG.cginc"

		struct Input
		{
			float2 uv_MainTex;
			fixed4 color;
		};

			int _BlurRadius;
			float _TextureSize;

        	//这一步其实可以用计算出来的常量来替代，不需要在循环中每一步计算
			float GetGaussWeight(float x, float y, float sigma)
			{
				float sigma2 = pow(sigma, 2.0f);
				float left = 1 / (2 * sigma2 * 3.1415926f);
				float right = exp(-(x*x+y*y)/(2*sigma2));
				return left * right;
			}

			fixed4 GaussBlur(float2 uv)
			{
				//因为高斯函数中3σ以外的点的权重已经很小了，因此σ取半径r/3的值
				float sigma = (float)_BlurRadius / 3.0f;
				float4 col = float4(0, 0, 0, 0);
				for (int x = - _BlurRadius; x <= _BlurRadius; ++x)
				{
					for (int y = - _BlurRadius; y <= _BlurRadius; ++y)
					{
						//获取周围像素的颜色
						//因为uv是0-1的一个值，而像素坐标是整形，我们要取材质对应位置上的颜色，需要将整形的像素坐标
						//转为uv上的坐标值
						float4 color = tex2D(_MainTex, uv + float2(x / _TextureSize, y / _TextureSize));
						//获取此像素的权重
						float weight = GetGaussWeight(x, y, sigma);
						//计算此点的最终颜色
						col += color * weight;
						
					}
				}
				return col * _Color;
			}
		
		void vert (inout appdata_full v, out Input o)
		{
			v.vertex.xy *= _Flip.xy;

			#if defined(PIXELSNAP_ON)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color * _RendererColor;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = SampleSpriteTexture (IN.uv_MainTex) * IN.color;
			o.Albedo = GaussBlur(IN.uv_MainTex) * c.a;
			o.Alpha = c.a;
		}
		ENDCG
	}

Fallback "Transparent/VertexLit"
}
