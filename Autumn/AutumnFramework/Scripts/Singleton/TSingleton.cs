//=====================================================
// - FileName:      TSingleton.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 11:13:11
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
    public class TSingleton<T> : ASingleton where T:TSingleton<T>,new ()
    {
        protected static T m_instance;
        protected static Object  s_lock = new Object();

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
                            m_instance = new T();
                            m_instance.OnSingletonInit();
                        }
                    }
                }
                return m_instance;
            }
        }

        public static T ResetInstance() 
        {
            m_instance = new T();
            m_instance.OnSingletonInit();
            return m_instance;
        }

        public virtual void OnSingletonInit() 
        {
        }

    }
}

