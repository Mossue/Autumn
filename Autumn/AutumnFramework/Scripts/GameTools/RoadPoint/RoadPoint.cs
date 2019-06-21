//=====================================================
// - FileName:      RoadPoint.cs
// - Author:       Autumn
// - CreateTime:    2019/05/09 16:05:04
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Autumn
{
    public class RoadPoint : MonoBehaviour
    {
        [SerializeField]
        private Color m_Color = Color.red;
        [SerializeField]
        private bool m_StartPoint;
        [SerializeField]
        private bool m_EndPoint;

        public List<RoadPoint> avaliableRoad;
        public Transform leftRoad;
        public Transform rightRoad;
        public bool isArrive = false;

        public bool StartPoint
        {
            get { return m_StartPoint; }
        }
        public bool EndPoint
        {
            get { return m_EndPoint; }
        }

        public RoadPoint GetNextRoadPoint() 
        {
            return avaliableRoad[0];
        }

#if UNITY_EDITOR

        public void OnDrawGizmos()
        {
            Gizmos.color = m_Color;
            Gizmos.DrawSphere(transform.position, HandleUtility.GetHandleSize(transform.position) * 0.2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(leftRoad.position, HandleUtility.GetHandleSize(transform.position) * 0.1f);
            Gizmos.DrawSphere(rightRoad.position, HandleUtility.GetHandleSize(transform.position) * 0.1f);

            if (avaliableRoad != null && avaliableRoad.Count > 0)
            {
                for (int i = 0; i < avaliableRoad.Count; i++)
                {
                    Gizmos.DrawLine(leftRoad.position, avaliableRoad[i].leftRoad.position);
                    Gizmos.DrawLine(rightRoad.position, avaliableRoad[i].rightRoad.position);
                }
            }
        }
#endif
    }
}

