//=====================================================
// - FileName:      AStarAlgorithm.cs
// - Author:       Autumn
// - CreateTime:    2019/05/21 17:19:19
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
    public class AStarAlgorithm:TSingleton<AStarAlgorithm>
    {
        private const int mGridWidth = 20;
        private const int mGridHeight = 10;

        //使用二维数组存储点网格    
        public AStarPoint[,] mPointGrid = new AStarPoint[mGridWidth, mGridHeight];
        //存储路径方格子
        public List<AStarPoint> mPathPosList = new List<AStarPoint>();

        public AStarAlgorithm()
        {
            InitPoint();
        }

        //在网格上设置点的信息
        private void InitPoint()
        {
            for (int i = 0; i < mGridWidth; i++)
            {
                for (int j = 0; j < mGridHeight; j++)
                {
                    mPointGrid[i, j] = new AStarPoint(i, j);
                }
            }

            //设置障碍物
            mPointGrid[4, 2].isObstacle = true;
            mPointGrid[4, 3].isObstacle = true;
            mPointGrid[4, 4].isObstacle = true;
            mPointGrid[4, 5].isObstacle = true;
            mPointGrid[4, 6].isObstacle = true;

            //显示障碍物
            for (int x = 0; x < mGridWidth; x++)
            {
                for (int y = 0; y < mGridHeight; y++)
                {
                    if (mPointGrid[x, y].isObstacle)
                    {
                        CreatePath(x, y, Color.blue);
                    }
                }
            }
        }

        public void ClearGrid()
        {
            for (int x = 0; x < mGridWidth; x++)
            {
                for (int y = 0; y < mGridHeight; y++)
                {
                    if (!mPointGrid[x, y].isObstacle)
                    {
                        if (mPointGrid[x, y].aStarObject != null)
                        {
                            GameObject.Destroy(mPointGrid[x, y].aStarObject);
                            mPointGrid[x, y].aStarObject = null;

                            //重新设置父节点
                            mPointGrid[x, y].parentPoint = null;
                        }
                    }
                }
            }
        }

        //寻路
        public List<AStarPoint> FindPath(AStarPoint mStartPoint, AStarPoint mEndPoint)
        {
            if (mEndPoint.isObstacle || mStartPoint.pointPosition == mEndPoint.pointPosition)
            {
                return null;
            }

            //开启列表
            List<AStarPoint> openPointList = new List<AStarPoint>();
            //关闭列表
            List<AStarPoint> closePointList = new List<AStarPoint>();

            openPointList.Add(mStartPoint);

            while (openPointList.Count > 0)
            {
                //寻找开启列表中最小预算值的表格
                AStarPoint minFPoint = FindPointWithMinF(openPointList);
                //将当前表格从开启列表移除 在关闭列表添加
                openPointList.Remove(minFPoint);
                closePointList.Add(minFPoint);
                //找到当前点周围的全部点
                List<AStarPoint> surroundPoints = FindSurroundPoint(minFPoint);
                //在周围的点中，将关闭列表里的点移除掉
                SurroundPointsFilter(surroundPoints, closePointList);
                //寻路逻辑
                foreach (var surroundPoint in surroundPoints)
                {
                    if (openPointList.Contains(surroundPoint))
                    {
                        //计算下新路径下的G值（H值不变的，比较G相当于比较F值）
                        float newPathG = CalcG(surroundPoint, minFPoint);
                        if (newPathG < surroundPoint.aStarG)
                        {
                            surroundPoint.aStarG = newPathG;
                            surroundPoint.aStarF = surroundPoint.aStarG + surroundPoint.aStarH;
                            surroundPoint.parentPoint = minFPoint;
                        }
                    }
                    else
                    {
                        //将点之间的
                        surroundPoint.parentPoint = minFPoint;
                        CalcF(surroundPoint, mEndPoint);
                        openPointList.Add(surroundPoint);
                    }
                }

                //如果开始列表中包含了终点，说明找到路径
                if (openPointList.IndexOf(mEndPoint) > -1)
                {
                    break;
                }
            }

            return ShowPath(mStartPoint, mEndPoint);
        }

        private List<AStarPoint> ShowPath(AStarPoint start, AStarPoint end)
        {
            mPathPosList.Clear();

            AStarPoint temp = end;
            while (true)
            {
                mPathPosList.Add(temp);

                Color c = Color.white;
                if (temp == start)
                {
                    c = Color.green;
                }
                else if (temp == end)
                {
                    c = Color.red;
                }
                CreatePath(temp.pointPositionX, temp.pointPositionY, c);

                if (temp.parentPoint == null)
                    break;
                temp = temp.parentPoint;
            }

            return mPathPosList;
        }

        private void CreatePath(int x, int y, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(x, y, 0);
            go.GetComponent<Renderer>().material.color = color;
            go.transform.SetParent(GameObject.Find("Path").transform);

            if (mPointGrid[x, y].aStarObject != null)
            {
                GameObject.Destroy(mPointGrid[x, y].aStarObject);
            }
            mPointGrid[x, y].aStarObject = go;
        }

        //寻找预计值最小的格子
        private AStarPoint FindPointWithMinF(List<AStarPoint> openPointList)
        {
            float f = float.MaxValue;
            AStarPoint temp = null;
            foreach (AStarPoint p in openPointList)
            {
                if (p.aStarF < f)
                {
                    temp = p;
                    f = p.aStarF;
                }
            }
            return temp;
        }

        //寻找周围的全部点
        private List<AStarPoint> FindSurroundPoint(AStarPoint point)
        {
            List<AStarPoint> list = new List<AStarPoint>();

            ////////////判断周围的八个点是否在网格内/////////////
            AStarPoint up = null, down = null, left = null, right = null;
            AStarPoint lu = null, ru = null, ld = null, rd = null;
            if (point.pointPositionY < mGridHeight - 1)
            {
                up = mPointGrid[point.pointPositionX, point.pointPositionY + 1];
            }
            if (point.pointPositionY > 0)
            {
                down = mPointGrid[point.pointPositionX, point.pointPositionY - 1];
            }
            if (point.pointPositionX > 0)
            {
                left = mPointGrid[point.pointPositionX - 1, point.pointPositionY];
            }
            if (point.pointPositionX < mGridWidth - 1)
            {
                right = mPointGrid[point.pointPositionX + 1, point.pointPositionY];
            }
            if (up != null && left != null)
            {
                lu = mPointGrid[point.pointPositionX - 1, point.pointPositionY + 1];
            }
            if (up != null && right != null)
            {
                ru = mPointGrid[point.pointPositionX + 1, point.pointPositionY + 1];
            }
            if (down != null && left != null)
            {
                ld = mPointGrid[point.pointPositionX - 1, point.pointPositionY - 1];
            }
            if (down != null && right != null)
            {
                rd = mPointGrid[point.pointPositionX + 1, point.pointPositionY - 1];
            }


            /////////////将可以经过的表格添加到开启列表中/////////////
            if (down != null && down.isObstacle == false)
            {
                list.Add(down);
            }
            if (up != null && up.isObstacle == false)
            {
                list.Add(up);
            }
            if (left != null && left.isObstacle == false)
            {
                list.Add(left);
            }
            if (right != null && right.isObstacle == false)
            {
                list.Add(right);
            }
            if (lu != null && lu.isObstacle == false && left.isObstacle == false && up.isObstacle == false)
            {
                list.Add(lu);
            }
            if (ld != null && ld.isObstacle == false && left.isObstacle == false && down.isObstacle == false)
            {
                list.Add(ld);
            }
            if (ru != null && ru.isObstacle == false && right.isObstacle == false && up.isObstacle == false)
            {
                list.Add(ru);
            }
            if (rd != null && rd.isObstacle == false && right.isObstacle == false && down.isObstacle == false)
            {
                list.Add(rd);
            }

            return list;
        }

        //将关闭带你从周围点列表中关闭
        private void SurroundPointsFilter(List<AStarPoint> surroundPoints, List<AStarPoint> closePoints)
        {
            foreach (var closePoint in closePoints)
            {
                if (surroundPoints.Contains(closePoint))
                {
                    Log.i("将关闭列表的点移除");
                    surroundPoints.Remove(closePoint);
                }
            }
        }

        //计算最小预算值点G值
        private float CalcG(AStarPoint surround, AStarPoint minFPoint)
        {
            return Vector3.Distance(surround.pointPosition, minFPoint.pointPosition) + minFPoint.aStarG;
        }

        //计算该点到终点的F值
        private void CalcF(AStarPoint now, AStarPoint end)
        {
            //F = G + H
            float h = Mathf.Abs(end.pointPositionX - now.pointPositionX) + Mathf.Abs(end.pointPositionY - now.pointPositionY);
            float g = 0;
            if (now.parentPoint == null)
            {
                g = 0;
            }
            else
            {
                g = Vector2.Distance(new Vector2(now.pointPositionX, now.pointPositionY), new Vector2(now.parentPoint.pointPositionX, now.parentPoint.pointPositionY)) + now.parentPoint.aStarG;
            }
            float f = g + h;
            now.aStarF = f;
            now.aStarG = g;
            now.aStarH = h;
        }
    }
}

