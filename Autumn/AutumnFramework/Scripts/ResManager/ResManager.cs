//=====================================================
// - FileName:      ResManager.cs
// - Author:       Autumn
// - CreateTime:    2019/04/26 16:17:38
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autumn
{
    public class ResManager : TMonoSingleton<ResManager>
    {
        public void LoadSync(string name) 
        {
            //AssetBundle ab = AssetBundle.LoadFromFile(AutumnPath.ASSETBUNDLE_OUTPUT_PATH+"/"+name);
            //GameObject go = Instantiate(ab.LoadAsset(name)) as GameObject;
        }
    }
}

