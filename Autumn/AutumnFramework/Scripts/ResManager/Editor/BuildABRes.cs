//=====================================================
// - FileName:      BuildABRes.cs
// - Author:       Autumn
// - CreateTime:    2019/04/25 11:27:24
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Autumn
{
    public class BuildABRes : Editor
    {
        [MenuItem("Assets/Autumn/Res/BuildAllAssetBundles")]
        public static void BuildAllAssetBundle()
        {
            string outPath = string.Empty;
            outPath = AutumnPath.ASSETBUNDLE_OUTPUT_PATH;
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            AutoNamedAssets();
            BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();  
            Log.i("AssetBundles Created");
        }

        private static void AutoNamedAssets()
        {
            string resPath = AutumnPath.ASSETBUNDLE_FOLDER_PATH;
            AssetBundleTools.GetAllLocalSubDirs(resPath);
        }
    }
}

