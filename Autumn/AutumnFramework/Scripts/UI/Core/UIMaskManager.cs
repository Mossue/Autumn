//=====================================================
// - FileName:      UIMaskManager.cs
// - Author:       Autumn
// - CreateTime:    2019/04/22 14:25:25
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
    [TMonoSingletonAttribute("[UI]/UIMaskManager")]
    public class UIMaskManager : TMonoSingleton<UIMaskManager>
    {
        private Transform m_Root;
        private Transform m_PopupRoot;
        private Camera m_UICamera;
        private float m_OriginalUICameraDepth;
        private GameObject m_TopPanel;
        private GameObject m_MaskPanel;

        private void Awake() 
        {
            m_PopupRoot = UIManager.Ins.popupRoot;
            m_TopPanel = UIManager.Ins.root.gameObject;
            //加载mask
            LoadMask();
            m_UICamera = UIManager.Ins.uiCamera;
            if (m_UICamera != null)
            {
                m_OriginalUICameraDepth = m_UICamera.depth;
            }
            else 
            {
                Log.e("UICamera is null! plz Check");
            }
        }

        private void  LoadMask() 
        {
            m_MaskPanel = Instantiate(Resources.Load(UIConst.UIMASK_PATH)) as GameObject;
            m_MaskPanel.name = "UIMask";
            m_MaskPanel.transform.SetParent(m_PopupRoot,false);
            m_MaskPanel.SetActive(false);
        }
        /// <summary>
        /// 设置遮罩状态
        /// </summary>
        /// <param name="goDisplayPanel"></param>
        /// <param name="lucenyType"></param>
        public void SetMaskWindow(GameObject goDisplayPanel, UIFormLucenyType lucenyType = UIFormLucenyType.Lucency)
        {
            //顶层窗体下移
            m_TopPanel.transform.SetAsLastSibling();
            //启用遮罩窗体以及设置透明度
            switch (lucenyType)
            {
                //完全透明，不能穿透
                case UIFormLucenyType.Lucency:
                    m_MaskPanel.SetActive(true);
                    Color newColor1 = new Color(UIConst.UIMASK_LUCENCY_COLOR_RGB, UIConst.UIMASK_LUCENCY_COLOR_RGB, UIConst.UIMASK_LUCENCY_COLOR_RGB, UIConst.UIMASK_LUCENCY_COLOR_RGB_A);
                    m_MaskPanel.GetComponent<Image>().color = newColor1;
                    break;
                //半透明，不能穿透
                case UIFormLucenyType.Translucence:
                    m_MaskPanel.SetActive(true);
                    Color newColor2 = new Color(UIConst.UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConst.UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConst.UIMASK_TRANS_LUCENCY_COLOR_RGB, UIConst.UIMASK_TRANS_LUCENCY_COLOR_RGB_A);
                    m_MaskPanel.GetComponent<Image>().color = newColor2;
                    break;
                //低透明，不能穿透
                case UIFormLucenyType.ImPenetrable:
                    m_MaskPanel.SetActive(true);
                    Color newColor3 = new Color(UIConst.UIMASK_IMPENETRABLE_COLOR_RGB, UIConst.UIMASK_IMPENETRABLE_COLOR_RGB, UIConst.UIMASK_IMPENETRABLE_COLOR_RGB, UIConst.UIMASK_IMPENETRABLE_COLOR_RGB_A);
                    m_MaskPanel.GetComponent<Image>().color = newColor3;
                    break;
                //可以穿透
                case UIFormLucenyType.Pentrate:
                    if (m_MaskPanel.activeInHierarchy)
                    {
                        m_MaskPanel.SetActive(false);
                    }
                    break;

                default:
                    break;
            }

            //遮罩窗体下移
            m_MaskPanel.transform.SetAsLastSibling();
            //显示窗体的下移
            goDisplayPanel.transform.SetAsLastSibling();
            //增加当前UI摄像机的层深（保证当前摄像机为最前显示）
            if (m_UICamera != null)
            {
                m_UICamera.depth = m_UICamera.depth + 100;    //增加层深
            }

        }

        /// <summary>
        /// 取消遮罩状态
        /// </summary>
        public void CancelMaskWindow()
        {
            //顶层窗体上移
            m_TopPanel.transform.SetAsFirstSibling();
            //禁用遮罩窗体
            if (m_MaskPanel.activeInHierarchy)
            {
                //隐藏
                m_MaskPanel.SetActive(false);
            }

            //恢复当前UI摄像机的层深 
            if (m_UICamera != null)
            {
                m_UICamera.depth = m_OriginalUICameraDepth;  //恢复层深
            }
        }

    }
}

