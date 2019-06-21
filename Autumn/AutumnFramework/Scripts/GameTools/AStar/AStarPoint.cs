//=====================================================
// - FileName:      AStarPoint.cs
// - Author:       Autumn
// - CreateTime:    2019/05/21 17:21:12
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
    /// <summary>
    /// 存储寻路点信息
    /// </summary>
    public class AStarPoint
    {
        //父“格子”
        public AStarPoint parentPoint { get; set; }
        //格子显示对象
        public GameObject aStarObject { get; set; }

        public float aStarF { get; set; }
        public float aStarG { get; set; }
        public float aStarH { get; set; }
        //点的位置
        public Vector2 pointPosition { get; set; }
        public int pointPositionX { get; set; }
        public int pointPositionY { get; set; }
        //该点是否处于障碍物
        public bool isObstacle { get; set; }

        public AStarPoint(int positionX, int positionY)
        {
            this.pointPositionX = positionX;
            this.pointPositionY = positionY;
            this.pointPosition = new Vector2(pointPositionX, pointPositionY);
            this.parentPoint = null;
        }
    }
}

