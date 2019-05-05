//=====================================================
// - FileName:      PoolMgr.cs
// - Author:       Autumn
// - CreateTime:    2019/05/05 15:48:27
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
    [TMonoSingletonAttribute("[Tools]/PoolMgr")]
    public class PoolMgr : TMonoSingleton<PoolMgr>
    {
        private GameObjectPoolGroup m_Group;
        public GameObjectPoolGroup group
        {
            get { return m_Group; }
        }

        public override void OnSingletonInit()
        {
            m_Group = new GameObjectPoolGroup(transform);
        }

        public GameObjectPoolGroup CreatePoolGroup(IGameObjectPoolStrategy strategy = null)
        {
            GameObjectPoolGroup group = new GameObjectPoolGroup(transform, strategy);
            return group;
        }

        public GameObjectPool CreatePool(string poolName, GameObject prefab, int maxCount, int initCount, IGameObjectPoolStrategy strategy = null)
        {
            GameObjectPool pool = new GameObjectPool();
            pool.InitPool(poolName, transform, prefab, maxCount, initCount, strategy);
            return pool;
        }

        public void AddPool(string poolName, GameObject prefab, int maxCount, int initCount)
        {
            m_Group.AddPool(poolName, prefab, maxCount, initCount);
        }

        public void RemovePool(string poolName, bool destroyPrefab)
        {
            m_Group.RemovePool(poolName, destroyPrefab);
        }

        public void RemoveAllPool(bool destroyPrefab)
        {
            m_Group.RemoveAllPool(destroyPrefab);
        }

        public GameObject Allocate(string poolName)
        {
            return m_Group.Allocate(poolName);
        }
        public void Recycle(string poolName, GameObject obj)
        {
            m_Group.Recycle(poolName, obj);
        }
        public void Recycle(GameObject obj)
        {
            m_Group.Recycle(obj.name, obj);
        }
    }
}

