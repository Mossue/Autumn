//=====================================================
// - FileName:      EventSystem.cs
// - Author:       Autumn
// - CreateTime:    2019/04/12 15:05:03
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Autumn
{
    /// <summary>
    /// 事件接口
    /// </summary>
    /// <param name="key"></param>
    /// <param name="param"></param>
    public delegate void OnEvent(int key, params object[] param);

    public class AEventSystem : TSingleton<AEventSystem>, ICacheAble
    {
        private bool m_CacheFlag = false;
        public bool cacheFlag
        {
            get
            {
                return m_CacheFlag;
            }
            set
            {
                m_CacheFlag = value;
            }
        }

        private Dictionary<int, ListenerWrap> m_AllListenerMap = new Dictionary<int, ListenerWrap>(50);

        public AEventSystem() { }

        #region 内部结构

        private class ListenerWrap
        {
            private LinkedList<OnEvent> m_EventList;

            public bool Fire(int key, params object[] param)
            {
                if (m_EventList == null) { return false; }

                LinkedListNode<OnEvent> next = m_EventList.First;
                OnEvent call = null;
                LinkedListNode<OnEvent> nextCache = null;

                while (next != null)
                {
                    call = next.Value;
                    nextCache = next.Next;
                    call(key, param);

                    //1.该事件的回调删除了自己OK 2.该事件的回调添加了新回调OK， 3.该事件删除了其它回调(被删除的回调可能有回调，可能没有)
                    next = (next.Next == null) ? nextCache : next.Next;
                }
                return true;
            }

            public bool Add(OnEvent listener)
            {
                if (m_EventList == null)
                {
                    m_EventList = new LinkedList<OnEvent>();
                }

                if (m_EventList.Contains(listener))
                {
                    return false;
                }

                m_EventList.AddLast(listener);
                return true;
            }

            public bool Remove(OnEvent listener)
            {
                if (m_EventList == null) { return false; }

                m_EventList.Remove(listener);
                return true;
            }
        }

        #endregion

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public bool Register<T>(T key, OnEvent fun) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap; 
            if (!m_AllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap = new ListenerWrap();
                m_AllListenerMap.Add(kv, wrap);
            }
            if (wrap.Add(fun))
            {
                return true;
            }

            Log.i("Already had same Event:" + key);
            return false;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        public void UnRegister<T>(T key, OnEvent fun) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if (m_AllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap.Remove(fun);
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Send<T>(T key, params object[] param) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if (m_AllListenerMap.TryGetValue(kv, out wrap))
            {
                return wrap.Fire(kv, param);
            }
            Log.i("UnRegister Events:" + key);
            return false;
        }

        private static object[] s_EmptyParam = new object[0];
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Send<T>(T key) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if (m_AllListenerMap.TryGetValue(kv, out wrap))
            {
                return wrap.Fire(kv, s_EmptyParam);
            }
            Log.i("UnRegister Events:" + key);
            return false;
        }

        public void OnCacheReset() 
        {
            m_AllListenerMap.Clear();
        }
    }
}

