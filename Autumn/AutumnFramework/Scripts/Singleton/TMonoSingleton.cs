//=====================================================
// - FileName:      TMonoSingleton.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 17:37:48
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
    public abstract class TMonoSingleton <T>:MonoSingleton,ASingleton where T:TMonoSingleton<T>
    {
        private static T m_instance = null;
        private static object s_lock = new object();

        public static T Ins 
        {
            get 
            {
                if (m_instance == null)
                {
                    lock (s_lock) 
                    {
                        if (m_instance == null) 
                        {
                            m_instance = CreateMonoSingleton<T>();
                        }
                    }
                }
                return m_instance;
            }
        }

        public virtual void OnSingletonInit() { }

    }
}

