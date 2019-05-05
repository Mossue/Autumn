//=====================================================
// - FileName:      BeforeCloseUnity.cs
// - Author:       Autumn
// - CreateTime:    2019/04/15 14:35:42
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using UnityEditor;
using System.IO;
using System;
namespace Autumn
{
    /// <summary>
    /// 暂时没用
    /// </summary>
    public class BeforeCloseUnity
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            CheckFolder();
        }
        static bool Quit()
        {
            EditorUtility.DisplayDialog("关闭Unity", "确认关闭Unity前保存了吗？", "确认");
            return false;
        }

        static void CheckFolder()
        {
            if (!Directory.Exists("Assets/ABRes")) 
            {
                 Directory.CreateDirectory("Assets/ABRes");
            }
        }
    }

}



