//=====================================================
// - FileName:      PoolStrategy.cs
// - Author:       Autumn
// - CreateTime:    2019/05/05 14:23:12
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
    public interface IGameObjectPoolStrategy
    {
        void ProcessContainer(GameObject container);
        void OnAllocate(GameObject result);

        void OnRecycle(GameObject result);
    }

    public class DefaultPoolStrategy : TSingleton<DefaultPoolStrategy>, IGameObjectPoolStrategy
    {
        public void ProcessContainer(GameObject container)
        {
            container.SetActive(false);
        }

        public void OnAllocate(GameObject result)
        {

        }

        public void OnRecycle(GameObject result)
        {

        }
    }
}

