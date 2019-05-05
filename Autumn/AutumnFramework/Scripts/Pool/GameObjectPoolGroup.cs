//=====================================================
// - FileName:      GameObjectPoolGroup.cs
// - Author:       Autumn
// - CreateTime:    2019/05/05 15:20:09
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
    public class GameObjectPoolGroup
    {
        private Transform m_Parent;
        private IGameObjectPoolStrategy m_Strategy;
        private Dictionary<string, GameObjectPool> m_PoolMap = new Dictionary<string, GameObjectPool>();

        public GameObjectPoolGroup(Transform parant, IGameObjectPoolStrategy strategy = null) 
        {
            m_Parent = parant;
            m_Strategy = strategy;
        }

        public bool HasPool(string name) 
        {
            return m_PoolMap.ContainsKey(name);
        }

        public void AddPool(string poolName, GameObject prefab, int maxCount, int initCount) 
        {
            if (m_PoolMap.ContainsKey(poolName)) 
            {
                Log.e("Allready Init GameObjectPool:{0}", poolName);
                return;
            }
            GameObjectPool cell = new GameObjectPool();
            cell.InitPool(poolName, m_Parent, prefab, maxCount, initCount, m_Strategy);
            m_PoolMap.Add(poolName, cell);
        }

        public void AddPool(GameObjectPool pool) 
        {
            m_PoolMap.Add(pool.poolName,pool);
        }

        public void RemovePool(string PoolName,bool destroyPrefab) 
        {
            GameObjectPool pool = null;
            if (m_PoolMap.TryGetValue(PoolName,out pool)) 
            {
                pool.RemoveAllObject(true, destroyPrefab);
                m_PoolMap.Remove(PoolName);
            }
        }

        public void RemoveAllPool(bool destroyPrefab) 
        {
            foreach (var pool in m_PoolMap) 
            {
                pool.Value.RemoveAllObject(true, destroyPrefab);
            }
            m_PoolMap.Clear();
        }

        public GameObject Allocate(string poolName) 
        {
            GameObjectPool cell = null;
            if (!m_PoolMap.TryGetValue(poolName, out cell)) 
            {
                Log.e("Allocate Not Find Pool:{0}", poolName);
                return null;
            }
            return cell.Allocate();
        }

        public void Recycle(string poolName, GameObject obj) 
        {
            GameObjectPool cell = null;
            if (!m_PoolMap.TryGetValue(poolName, out cell))
            {
                Log.e("Allocate Not Find Pool:{0}", poolName);
                return;
            }
            cell.Recycle(obj);
        }
        public void Recycle(GameObject obj)
        {
            Recycle(obj.name, obj);
        }
    }
}

