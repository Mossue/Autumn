//=====================================================
// - FileName:      AssetBundleBuilder.cs
// - Author:       Autumn
// - CreateTime:    2019/04/25 14:19:30
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Autumn
{
    public class AssetBundleTools : MonoBehaviour
    {
        private static Dictionary<string, string> m_DicAbres=new Dictionary<string,string>();
        public static List<string> GetAllLocalSubDirs(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
                return null;
            string fullRootPath = System.IO.Path.GetFullPath(rootPath);
            if (string.IsNullOrEmpty(fullRootPath))
                return null;

            string[] dirs = System.IO.Directory.GetDirectories(fullRootPath);
            if ((dirs == null) || (dirs.Length <= 0))
                return null;
            List<string> ret = new List<string>();

            for (int i = 0; i < dirs.Length; ++i)
            {
                string dir = GetLocalPath(dirs[i]);
                ret.Add(dir);
                if (DirExistResource(dir))//自动生成ab名称
                {
                    AddBundleName(dir);
                }
            }
            for (int i = 0; i < dirs.Length; ++i)
            {
                string dir = dirs[i];
                List<string> list = GetAllLocalSubDirs(dir);
                if (list != null)
                    ret.AddRange(list);
            }

            return ret;
        }
        // 获得根据Assets目录的局部目录
        private static string GetLocalPath(string path)
        {
            return GetAssetRelativePath(path);
        }
        private static string GetAssetRelativePath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
                return string.Empty;
            fullPath = fullPath.Replace("\\", "/");
            int index = fullPath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);
            if (index < 0)
                return fullPath;
            string ret = fullPath.Substring(index);
            return ret;
        }
        // 根据目录判断是否有资源文件
        private static bool DirExistResource(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            string fullPath = Path.GetFullPath(path);
            if (string.IsNullOrEmpty(fullPath))
                return false;

            string[] files = System.IO.Directory.GetFiles(fullPath);
            if ((files == null) || (files.Length <= 0))
                return false;
            for (int i = 0; i < files.Length; ++i)
            {
                string ext = System.IO.Path.GetExtension(files[i]);
                if (string.IsNullOrEmpty(ext))
                    continue;
                for (int j = 0; j < ResourceExts.Length; ++j)
                {
                    if (string.Compare(ext, ResourceExts[j], true) == 0)
                    {
                        if ((ResourceExts[j] == ".fbx") || (ResourceExts[j] == ".controller"))
                        {
                            // ingore xxx@idle.fbx
                            string name = Path.GetFileNameWithoutExtension(files[i]);
                            if (name.IndexOf('@') >= 0)
                                return false;
                        }
                        else
                            if (ResourceExts[j] == ".unity")
                            {
                                if (!IsVaildSceneResource(files[i]))
                                    return false;
                            }
                        return true;
                    }
                }
            }

            return false;
        }

        private static void AddBundleName(string path) 
        {
            if (Directory.Exists(path)) 
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Length; i++) 
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                    AssetImporter assetImp = AssetImporter.GetAtPath(path + "/" + files[i].Name);
                    //string abName=path.Replace("/","_");
                    Log.i(path + "/" + files[i].Name);
                    assetImp.SetAssetBundleNameAndVariant(path, "Autumn");
                    if (m_DicAbres.ContainsKey(files[i].Name))
                    {
                        Log.e("Allready exits Res: {0},plz rename it", files[i].Name);
                    }
                    else 
                    {
                        m_DicAbres.Add(files[i].Name, path);
                    }
                }
            }
        }

        private static void CheckHasSameNameInPath() 
        {
            if (Directory.Exists(AutumnPath.ASSETBUNDLE_FOLDER_PATH))
            {
                DirectoryInfo direction = new DirectoryInfo(AutumnPath.ASSETBUNDLE_FOLDER_PATH);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".meta"))
                    {
                        continue;
                    }
                }
            }
        }

        // 支持的资源文件格式
        private static readonly string[] ResourceExts = {".prefab", ".fbx",
							 ".png", ".jpg", ".dds", ".gif", ".psd", ".tga", ".bmp",
							 ".txt", ".bytes", ".xml", ".csv", ".json",
							".controller", ".shader", ".anim", ".unity", ".mat",
							".wav", ".mp3", ".ogg",
							".ttf",
							 ".shadervariants", ".asset"};

        private static bool IsVaildSceneResource(string fileName)
        {
            bool ret = false;

            if (string.IsNullOrEmpty(fileName))
                return ret;

            string localFileName = GetLocalPath(fileName);
            if (string.IsNullOrEmpty(localFileName))
                return ret;

            var scenes = EditorBuildSettings.scenes;
            if (scenes == null)
                return ret;

            var iter = scenes.GetEnumerator();
            while (iter.MoveNext())
            {
                EditorBuildSettingsScene scene = iter.Current as EditorBuildSettingsScene;
                if ((scene != null) && scene.enabled)
                {
                    if (string.Compare(scene.path, localFileName, true) == 0)
                    {
                        ret = true;
                        break;
                    }
                }
            }

            return ret;
        }
    }
}

