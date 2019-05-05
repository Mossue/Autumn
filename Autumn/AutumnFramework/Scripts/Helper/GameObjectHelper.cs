//=====================================================
// - FileName:      GameObjectHelper.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 17:35:36
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autumn
{
    public class GameObjectHelper
    {
        public static GameObject FindGameObject(GameObject root, string path, bool build, bool dontDestroy)
        {
            if (path == null || path.Length == 0)
            {
                return null;
            }

            string[] subPath = path.Split('/');
            if (subPath == null || subPath.Length == 0)
            {
                return null;
            }

            return FindGameObject(null, subPath, 0, build, dontDestroy);
        }

        public static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build, bool dontDestroy)
        {
            GameObject client = null;

            if (root == null)
            {
                client = GameObject.Find(subPath[index]);
            }
            else
            {
                var child = root.transform.Find(subPath[index]);
                if (child != null)
                {
                    client = child.gameObject;
                }
            }

            if (client == null)
            {
                if (build)
                {
                    client = new GameObject(subPath[index]);
                    if (root != null)
                    {
                        client.transform.SetParent(root.transform);
                    }
                    if (dontDestroy && index == 0)
                    {
                        GameObject.DontDestroyOnLoad(client);
                    }
                }
            }

            if (client == null)
            {
                return null;
            }

            if (++index == subPath.Length)
            {
                return client;
            }

            return FindGameObject(client, subPath, index, build, dontDestroy);
        }
    }
}

