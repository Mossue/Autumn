//=====================================================
// - FileName:      UIManager.cs
// - Author:       Autumn
// - CreateTime:    2019/04/03 10:45:57
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using EventSystem=UnityEngine.EventSystems.EventSystem;

namespace Autumn
{
    [TMonoSingletonAttribute("[UI]/UIManager")]
    public class UIManager : TMonoSingleton<UIManager>
    {
        private Dictionary<UIID, string> m_DicUIPrefabPath;
        private Dictionary<UIID, BasePanel> m_DicAllPanel;
        private Dictionary<UIID, BasePanel> m_DicCurrentShowPanel;

        private Stack<BasePanel> m_StaCurrentPanel;

        public Transform root;
        public Transform fixedRoot;
        public Transform normalRoot;
        public Transform popupRoot;
        public Camera uiCamera;
        #region Init自动生成UIRoot
        private GameObject go;
        private Canvas can;
        private GameObject camObj;
        private GameObject subRoot;
        private Camera cam;

        private void InitRoot()
        {
            go = new GameObject("UIRoot");
            go.layer = LayerMask.NameToLayer("UI");
            go.AddComponent<RectTransform>();

            can = go.AddComponent<Canvas>();
            can.renderMode = RenderMode.ScreenSpaceCamera;
            can.pixelPerfect = true;
            go.AddComponent<GraphicRaycaster>();

            root = go.transform;
            UnityEngine.Object.DontDestroyOnLoad(go);

            camObj = new GameObject("UICamera");
            camObj.layer = LayerMask.NameToLayer("UI");
            camObj.transform.parent = go.transform;
            camObj.transform.localPosition = new Vector3(0, 0, -100f);
            cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Depth;
            cam.orthographic = true;
            cam.farClipPlane = 200f;
            can.worldCamera = cam;
            cam.cullingMask = 1 << 5;
            cam.nearClipPlane = -50f;
            cam.farClipPlane = 50f;
            uiCamera = cam;

            camObj.AddComponent<AudioListener>();
            camObj.AddComponent<GUILayer>();

            CanvasScaler cs = go.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1136f, 640f);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

            subRoot = CreateSubCanvasForRoot(go.transform, 0);
            subRoot.name = "NormalRoot";
            normalRoot = subRoot.transform;
            normalRoot.transform.localScale = Vector3.one;

            subRoot = CreateSubCanvasForRoot(go.transform, 250);
            subRoot.name = "FixedRoot";
            fixedRoot = subRoot.transform;
            fixedRoot.transform.localScale = Vector3.one;

            subRoot = CreateSubCanvasForRoot(go.transform, 500);
            subRoot.name = "PopupRoot";
            popupRoot = subRoot.transform;
            popupRoot.transform.localScale = Vector3.one;

            GameObject esObj = GameObject.Find("EventSystem");
            if (esObj != null)
            {
                GameObject.DestroyImmediate(esObj);
            }

            GameObject eventObj = new GameObject("EventSystem");
            eventObj.layer = LayerMask.NameToLayer("UI");
            eventObj.transform.SetParent(go.transform);
            eventObj.AddComponent<EventSystem>();
            eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        private GameObject CreateSubCanvasForRoot(Transform root, int sort)
        {
            GameObject go = new GameObject("canvas");
            go.transform.parent = root;
            go.layer = LayerMask.NameToLayer("UI");

            RectTransform rect = go.AddComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            return go;
        }
        #endregion

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            InitRoot();
        }

        public void Awake()
        {
            m_DicAllPanel = new Dictionary<UIID, BasePanel>();
            m_DicCurrentShowPanel = new Dictionary<UIID, BasePanel>();
            m_DicUIPrefabPath = new Dictionary<UIID, string>();
            m_StaCurrentPanel = new Stack<BasePanel>();

            AutoReadAllPanelDic();
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="panelName"></param>
        public void ShowPanel(UIID panelName) 
        {
            BasePanel basePanel = null;
            basePanel = LoadPanelToAllPanelCache(panelName);
            if (basePanel == null) return;

            if (basePanel.IsClearStack) 
            {
                ClearStackArray();
            }

            switch (basePanel.showMode) 
            {
                case UIShowMode.Normal:
                    LoadPanelToCurrentCache(panelName);
                    break;
                case UIShowMode.ReverseChange:
                    PushPanel2Stack(panelName);
                    break;
                case UIShowMode.HideOther:
                    EnterPanelAndHideOther(panelName);
                    break;
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="panelName"></param>
        public void ClosePanel(UIID panelName) 
        {
            BasePanel basePanel;
            m_DicAllPanel.TryGetValue(panelName, out basePanel);
            if (basePanel == null) return;

            switch (basePanel.showMode)
            {
                case UIShowMode.Normal:
                    ExitPanel(panelName);
                    break;
                case UIShowMode.ReverseChange:
                    PopUIPanel();
                    break;
                case UIShowMode.HideOther:
                    ExitPanelAndDisplayOther(panelName);
                    break;
            }
        }

        #region 私有加载方法

        private BasePanel LoadPanelToAllPanelCache(UIID panelName)
        {
            BasePanel returnPanel = null;
            m_DicAllPanel.TryGetValue(panelName, out returnPanel);
            if (returnPanel == null) 
            {
                returnPanel = LoadPanel(panelName);
            }
            return returnPanel;
        } 

        private BasePanel LoadPanel(UIID panelName) 
        {
            string uiPath = null;
            GameObject cloneUIPrefabs = null;
            BasePanel basePanel = null;

            m_DicUIPrefabPath.TryGetValue(panelName,out uiPath);
            if (!string.IsNullOrEmpty(uiPath)) 
            {
                //Test  需要重新改成ab
                Log.i("加载"+panelName);
                cloneUIPrefabs = Instantiate(Resources.Load(uiPath) )as GameObject;//通过名称路径加载预设的克隆体
                cloneUIPrefabs.name = panelName.ToString();
            }

            if (root != null && cloneUIPrefabs != null)
            {
                basePanel = cloneUIPrefabs.GetComponent<BasePanel>();
                if (basePanel == null)
                {
                    Log.e("UIPrefab is null! Plz check UIPrefab!");
                    return null;
                }
                switch (basePanel.formType)
                {
                    case UIFormType.Normal:
                        cloneUIPrefabs.transform.SetParent(normalRoot, false);
                        break;
                    case UIFormType.Fixed:
                        cloneUIPrefabs.transform.SetParent(fixedRoot, false);
                        break;
                    case UIFormType.PopUp:
                        cloneUIPrefabs.transform.SetParent(popupRoot, false);
                        break;
                    case UIFormType.None:
                        break;
                }
                cloneUIPrefabs.SetActive(false);
                m_DicAllPanel.Add(panelName, basePanel);
                return basePanel;
            }
            else 
            {
                Log.e("root or cloneUIPrefab is null！Plz check panelName="+panelName);
            }
            return null;
        }

        private void LoadPanelToCurrentCache(UIID panelName) 
        {
            BasePanel basePanel;
            BasePanel basePanelFromAllCache;

            m_DicCurrentShowPanel.TryGetValue(panelName, out basePanel);
            if (basePanel != null) return;

            m_DicAllPanel.TryGetValue(panelName, out basePanelFromAllCache);
            if (basePanelFromAllCache != null) 
            {
                m_DicCurrentShowPanel.Add(panelName, basePanelFromAllCache);
                basePanelFromAllCache.Display();
            }

        }

        /// <summary>
        /// 弹窗入栈
        /// </summary>
        /// <param name="panelName"></param>
        private void PushPanel2Stack(UIID panelName) 
        {
            BasePanel basePanel;

            if (m_StaCurrentPanel.Count > 0) 
            {
                BasePanel topPanel = m_StaCurrentPanel.Peek();
                topPanel.Freeze();
            }
            m_DicAllPanel.TryGetValue(panelName, out basePanel);
            if (basePanel != null)
            {
                basePanel.Display();
                m_StaCurrentPanel.Push(basePanel);
            }
            else 
            {
                Log.i("basePanel is null ,plz check panelName={0}",panelName);
            }
        }

        /// <summary>
        /// 弹窗出栈
        /// </summary>
        private void PopUIPanel() 
        {
            if (m_StaCurrentPanel.Count > 2) 
            {
                //出栈
                BasePanel topPanel = m_StaCurrentPanel.Pop();
                topPanel.Hide();
                BasePanel nextPanel = m_StaCurrentPanel.Peek();
                nextPanel.ReDisplay();
            }
            else if (m_StaCurrentPanel.Count == 1) 
            {
                //出栈
                BasePanel topPanel = m_StaCurrentPanel.Pop();
                topPanel.Hide();
            }
        }

        private bool ClearStackArray() 
        {
            if (m_StaCurrentPanel != null && m_StaCurrentPanel.Count >= 1) 
            {
                m_StaCurrentPanel.Clear();
                return true;
            }
            return false;
        }

        /// <summary>
        /// HideOther属性的显示UI方法
        /// </summary>
        /// <param name="panelName"></param>
        private void EnterPanelAndHideOther(UIID panelName) 
        {
            BasePanel basePanel;
            BasePanel basePanelFromAll;
            m_DicCurrentShowPanel.TryGetValue(panelName, out basePanel);
            if (basePanel != null) return;

            foreach (BasePanel panel in m_DicCurrentShowPanel.Values) 
            {
                panel.Hide();
            }
            foreach (BasePanel panel in m_StaCurrentPanel) 
            {
                panel.Hide();
            }

            m_DicAllPanel.TryGetValue(panelName, out basePanelFromAll);
            if (basePanelFromAll != null) 
            {
                m_DicCurrentShowPanel.Add(panelName, basePanelFromAll);
                basePanelFromAll.Display();
            }
        }

        /// <summary>
        /// HideOther属性的关闭UI方法
        /// </summary>
        /// <param name="panelName"></param>
        private void ExitPanelAndDisplayOther(UIID panelName) 
        {
            BasePanel basePanel;
            m_DicCurrentShowPanel.TryGetValue(panelName, out basePanel);
            if (basePanel == null) return;

            basePanel.Hide();
            m_DicCurrentShowPanel.Remove(panelName);

            foreach (BasePanel panel in m_DicCurrentShowPanel.Values) 
            {
                panel.ReDisplay();
            }
            foreach (BasePanel panel in m_StaCurrentPanel) 
            {
                panel.ReDisplay();
            }

        }

        private void ExitPanel(UIID panelName) 
        {
            BasePanel basePanel;

            m_DicCurrentShowPanel.TryGetValue(panelName, out basePanel);
            if (basePanel == null) return;

            basePanel.Hide();
            m_DicCurrentShowPanel.Remove(panelName);
        }

        #endregion

        /// <summary>
        /// 自动读取UI信息字典
        /// </summary>
        private void AutoReadAllPanelDic() 
        {
            string[] strArr;
            if (!File.Exists(UIConst.UIDIC_CONFIG_PATH))
            {
                Log.i("配置文件不存在！");
                return;
            }
            strArr = File.ReadAllLines(UIConst.UIDIC_CONFIG_PATH);
            for (int i = 0; i < strArr.Length; i++)
            {
                UIID id = (UIID)i;
                m_DicUIPrefabPath.Add(id, Helper.String2ListString(strArr[i],"|")[1]);
            }
        }
    }
}
