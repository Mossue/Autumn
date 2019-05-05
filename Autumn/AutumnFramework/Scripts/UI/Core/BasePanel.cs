//=====================================================
// - FileName:      BasePanel.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 10:17:44
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autumn
{
    /// <summary>
    /// UI界面基类
    /// </summary>
    public class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 是否清空“栈集合”
        /// </summary>
        public bool IsClearStack = false;

        public UIFormType formType = UIFormType.Normal;

        public UIShowMode showMode = UIShowMode.Normal;

        public UIFormLucenyType lucenyType = UIFormLucenyType.Lucency;

        /// <summary>
        /// 显示状态
        /// </summary>
        public virtual void Display() 
        {
            this.gameObject.SetActive(true);
            if (formType == UIFormType.PopUp) 
            {
                UIMaskManager.Ins.SetMaskWindow(this.gameObject,lucenyType);
            }
        }

        /// <summary>
        /// 隐藏状态
        /// </summary>
        public virtual void Hide() 
        {
            this.gameObject.SetActive(false);
            if (formType == UIFormType.PopUp)
            {
                UIMaskManager.Ins.CancelMaskWindow();
            }
        }

        /// <summary>
        /// 重新显示状态
        /// </summary>
        public virtual void ReDisplay() 
        {
            this.gameObject.SetActive(true);
            if (formType == UIFormType.PopUp)
            {
                UIMaskManager.Ins.SetMaskWindow(this.gameObject, lucenyType);
            }
        }

        /// <summary>
        /// 冻结状态
        /// </summary>
        public virtual void Freeze() 
        {
            this.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// UI类型
    /// </summary>
    public class UIType 
    {
        /// <summary>
        /// 是否清空“栈集合”
        /// </summary>
        public bool IsClearStack = false;

        public UIFormType formType = UIFormType.Normal;

        public UIShowMode showMode = UIShowMode.Normal;

        public UIFormLucenyType lucenyType = UIFormLucenyType.Lucency;

    }
}

