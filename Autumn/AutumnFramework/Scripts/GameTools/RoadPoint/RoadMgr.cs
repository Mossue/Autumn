//=====================================================
// - FileName:      RoadMgr.cs
// - Author:       Autumn
// - CreateTime:    2019/05/15 17:51:36
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Autumn 
{
    public class RoadMgr : TMonoSingleton<RoadMgr>
    {
        public List<RoadPoint> lstAllRoad = new List<RoadPoint>();
        public RoadPoint startPoint;
        public RoadPoint curRoadPoint;
        public RoadPoint nextRoadPoint;
        private void Start() 
        {
            curRoadPoint = startPoint;
            nextRoadPoint = startPoint.avaliableRoad[0];
            InitAllRoad(startPoint);
        }

        private void InitAllRoad(RoadPoint start) 
        {
            lstAllRoad.Add(start);
            if (start.avaliableRoad.Count > 1)
            {
                for (int i = 0; i < start.avaliableRoad.Count; i++) 
                {
                    InitAllRoad(startPoint.avaliableRoad[i]);
                }
            }
        }

        public void GotoNextRoad() 
        {
            curRoadPoint = curRoadPoint.avaliableRoad[0];
            curRoadPoint.isArrive = true;
            if (!curRoadPoint.EndPoint)
            {
                nextRoadPoint = curRoadPoint.avaliableRoad[0];
            }
            else 
            {
                nextRoadPoint = null;
            }
        }
    }
}

