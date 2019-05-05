//=====================================================
// - FileName:      IRefCount.cs
// - Author:       Autumn
// - CreateTime:    2019/05/05 10:38:07
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
    public interface IRefCount
    {
        int refCount
        {
            get;
        }
        void AddRef();
        void SubRef();
    }
    /// <summary>
    /// 资源引用计数
    /// </summary>
    public class RefCount : IRefCount
    {
        private int m_RefCount = 0;

        public int refCount 
        {
            get 
            {
                return m_RefCount;
            }
        }

        public void AddRef() 
        {
            ++m_RefCount;
        }

        public void SubRef() 
        {
            --m_RefCount;
            if (m_RefCount == 0) 
            {
                OnZeroRef();
            }
        }

        protected virtual void OnZeroRef() 
        {
        }
    }
}

