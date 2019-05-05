//=====================================================
// - FileName:      UIDefine.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 10:22:06
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
    /// UI窗体位置类型
    /// </summary>
    public enum UIFormType
    {
        Normal,//普通
        Fixed,//固定
        PopUp,//弹窗
        None,
    }

    /// <summary>
    /// UI显示模式
    /// </summary>
    public enum UIShowMode
    {
        Normal,
        HideOther,//隐藏其他
        ReverseChange,//反向切换
    }

    /// <summary>
    /// UI窗体透明度类型
    /// </summary>
    public enum UIFormLucenyType
    {
        /// <summary>
        /// 完全透明，不能穿透
        /// </summary>
        Lucency, 
        /// <summary>
        /// 半透明，不能穿透
        /// </summary>
        Translucence,
        /// <summary>
        /// 低透明度，不能穿透
        /// </summary>
        ImPenetrable, 
        /// <summary>
        /// 可以穿透
        /// </summary>
        Pentrate,
    }
}
