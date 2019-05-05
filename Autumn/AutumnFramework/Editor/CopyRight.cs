//=====================================================
// - FileName:      #SCRIPTNAME#.cs
// - Author:       #AuthorName#
// - CreateTime:    #CreateTime#
// - Email:         #AuthorEmail#
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using UnityEngine;
using System.Collections;
using System.IO;

public class Copyright : UnityEditor.AssetModificationProcessor
{
    private const string AuthorName = "Autumn";
    private const string AuthorEmail = "543761701@qq.com";

    private const string DateFormat = "yyyy/MM/dd HH:mm:ss";
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = File.ReadAllText(path);
            allText = allText.Replace("#AuthorName#", AuthorName);
            allText = allText.Replace("#AuthorEmail#", AuthorEmail);
            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
            File.WriteAllText(path, allText);
            UnityEditor.AssetDatabase.Refresh();
        }

    }
}