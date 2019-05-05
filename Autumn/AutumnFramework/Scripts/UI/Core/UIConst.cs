//=====================================================
// - FileName:      UIID.cs
// - Author:       Autumn
// - CreateTime:    2019/04/22 14:59:53
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
    public class UIConst 
    {
        //编辑器用的路径配置
        public const string UIDIC_CONFIG_EDITOR_PATH = "Assets//Autumn//Config//UIConfig";
        public const string UIID_CREATE_PATH = "Assets/Autumn/AutumnFramework/Scripts/UI/Core";
        //UI路径常量
        public const string UIMASK_PATH = "UIBase/UIMask";
        public const string UIDIC_CONFIG_PATH = "Assets/Autumn/Config/UIConfig/UIDicConfig";

        //遮罩管理器中，透明度常量
        public const float UIMASK_LUCENCY_COLOR_RGB = 255 / 255F;
        public const float UIMASK_LUCENCY_COLOR_RGB_A = 0F / 255F;

        public const float UIMASK_TRANS_LUCENCY_COLOR_RGB = 220 / 255F;
        public const float UIMASK_TRANS_LUCENCY_COLOR_RGB_A = 50F / 255F;

        public const float UIMASK_IMPENETRABLE_COLOR_RGB = 50 / 255F;
        public const float UIMASK_IMPENETRABLE_COLOR_RGB_A = 200F / 255F;
    }
}

