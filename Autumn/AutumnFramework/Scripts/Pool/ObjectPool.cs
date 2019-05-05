//=====================================================
// - FileName:      ObjectPool.cs
// - Author:       Autumn
// - CreateTime:    2019/05/05 10:51:57
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
    //主动提供缓存管理的类型
    public interface ICacheType 
    {
        void Recycle2Cache();
    }

    public interface ICacheAble 
    {
        void OnCacheReset();
        bool cacheFlag
        {
            get;
            set;
        }
    }

    public interface CountObserverAble 
    {
        int currentCount
        {
            get;
        }
    }

    public class ObjectPool<T>:TSingleton<ObjectPool<T>>,CountObserverAble where T:ICacheAble, new ()
    {
        private int m_MaxCount = 0;
        private Stack<T> m_CacheStack;
        private int m_CreateCount = 0;
        private int m_MaxCreateCount = 0;

        public int currentCount
        {
            get
            {
                if (m_CacheStack == null)
                {
                    return 0;
                }

                return m_CacheStack.Count;
            }
        }

        public int maxCreateCount
        {
            get { return m_MaxCreateCount; }
            set { m_MaxCreateCount = value; }
        }

        public int maxCacheCount
        {
            get { return m_MaxCount; }
            set
            {
                m_MaxCount = value;

                if (m_CacheStack != null)
                {
                    if (m_MaxCount > 0)
                    {
                        if (m_MaxCount < m_CacheStack.Count)
                        {
                            int removeCount = m_MaxCount - m_CacheStack.Count;
                            while (removeCount > 0)
                            {
                                m_CacheStack.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

        public void Init(int maxCount, int initCount) 
        {
            if (maxCount > 0) 
            {
                initCount = Mathf.Min(maxCount, initCount);
            }

            m_MaxCount = maxCount;

            if (currentCount < initCount) 
            {
                for (int i = currentCount; i < initCount; i++) 
                {
                    
                }
            }
        }

        public T Allocate() 
        {
            T result;
            if (m_CacheStack == null || m_CacheStack.Count == 0)
            {
                if (maxCacheCount == 0 || (m_MaxCreateCount > 0 && m_CreateCount < m_MaxCreateCount))
                {
                    m_CreateCount++;
                    result = new T();
                }
                else
                {
                    return default(T);
                }
            }
            else 
            {
                result = m_CacheStack.Pop();
            }
            result.cacheFlag = false;
            return result;
        }

        public bool Recycle(T t) 
        {
            if (t == null || t.cacheFlag) 
            {
                return false;
            }
            if (m_CacheStack == null) 
            {
                m_CacheStack = new Stack<T>();
            }
            else if (m_MaxCount > 0) 
            {
                if (m_CacheStack.Count >= m_MaxCount) 
                {
                    t.OnCacheReset();
                    return false;
                }
            }
            t.cacheFlag = true;
            t.OnCacheReset();
            m_CacheStack.Push(t);
            return true;
        }

    }
}

