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
        [SerializeField]
        private List<RoadPoint> m_AvaliableRoad;

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
            return m_AvaliableRoad[ARandom.Range(0, m_AvaliableRoad.Count)];
        }

#if UNITY_EDITOR

        public void OnDrawGizmos() 
        {
            Gizmos.color = m_Color;
            Gizmos.DrawSphere(transform.position, HandleUtility.GetHandleSize(transform.position) * 0.1f);
            if (m_AvaliableRoad != null && m_AvaliableRoad.Count > 0)
            {
                for (int i = 0; i < m_AvaliableRoad.Count; i++)
                {
                    Gizmos.DrawLine(transform.position, m_AvaliableRoad[i].transform.position);
                }
            }
        }


#endif
    }
}

