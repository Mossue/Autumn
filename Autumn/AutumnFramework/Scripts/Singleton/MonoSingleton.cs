//=====================================================
// - FileName:      MonoSingleton.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 17:18:51
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
    public class MonoSingleton : MonoBehaviour
    {
        private static bool m_IsApplicationQuit = false;

        public static bool isApplicationQuit
        {
            get { return m_IsApplicationQuit; }
            set { m_IsApplicationQuit = value; }
        }

        public static K CreateMonoSingleton<K>() where K : MonoBehaviour, ASingleton
        {
            if (m_IsApplicationQuit)
            {
                return null;
            }

            K instance = null;

            if (instance == null && !m_IsApplicationQuit)
            {
                instance = GameObject.FindObjectOfType(typeof(K)) as K;
                if (instance == null)
                {
                    System.Reflection.MemberInfo info = typeof(K);
                    object[] attributes = info.GetCustomAttributes(true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        TMonoSingletonAttribute defineAttri = attributes[i] as TMonoSingletonAttribute;
                        if (defineAttri == null)
                        {
                            continue;
                        }
                        instance = CreateComponentOnGameObject<K>(defineAttri.AbsolutePath, true);
                        break;
                    }

                    if (instance == null)
                    {
                        GameObject obj = new GameObject("Singleton of " + typeof(K).Name);
                        UnityEngine.Object.DontDestroyOnLoad(obj);
                        instance = obj.AddComponent<K>();
                    }
                }

                instance.OnSingletonInit();
            }

            return instance;
        }

        protected static K CreateComponentOnGameObject<K>(string path, bool dontDestroy) where K : MonoBehaviour
        {
            GameObject obj = GameObjectHelper.FindGameObject(null, path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of " + typeof(K).Name);
                if (dontDestroy)
                {
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }

            return obj.AddComponent<K>();
        }
    }
}

