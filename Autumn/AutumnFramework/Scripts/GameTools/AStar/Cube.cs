//=====================================================
// - FileName:      Cube.cs
// - Author:       Autumn
// - CreateTime:    2019/05/21 17:29:49
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
    public class Cube : MonoBehaviour
    {
        public delegate void VoidDelegate(int x, int y);
        public VoidDelegate FindPath;

        private void OnMouseDown()
        {
            if (FindPath != null)
            {
                FindPath((int)this.transform.position.x, (int)this.transform.position.y);
            }
        }
    }
}

