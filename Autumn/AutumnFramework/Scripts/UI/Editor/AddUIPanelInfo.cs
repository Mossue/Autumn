//=====================================================
// - FileName:      AddUIPanelInfo.cs
// - Author:       Autumn
// - CreateTime:    2019/04/22 16:07:35
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace Autumn
{
    public class AddUIPanelInfo : EditorWindow
    {
        private string m_Path;
        private string m_UIID;
        private string m_DeleteKey;
        private string[] m_ArrStrs;

        [MenuItem("Autumn/UI/Step1:AddUIPanelInfo")]
        private static void AddUIPanel2AllPanelDic()
        {
            Rect rect = new Rect(0, 0, 500, 550);
            AddUIPanelInfo window = (AddUIPanelInfo)EditorWindow.GetWindowWithRect(typeof(AddUIPanelInfo), rect, true, "增加UI信息(UIID,预制体路径)");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.TextArea(ReadFile(),GUILayout.Width(400),GUILayout.Height(400));
            m_UIID = EditorGUILayout.TextField("请输入UIID", m_UIID);
            m_Path = EditorGUILayout.TextField("请输入UIPath", m_Path);
            if (GUILayout.Button("增加UI字典", GUILayout.Width(200)))
            {
                if (string.IsNullOrEmpty(m_UIID) || string.IsNullOrEmpty(m_Path))
                {
                    this.ShowNotification(new GUIContent("ID或者Path不能为空"));
                }
                else
                {
                    if (ReadFile().Contains(m_UIID) || ReadFile().Contains(m_Path)) 
                    {
                        this.ShowNotification(new GUIContent("ID或者Path已存在！"));
                        return;
                    }
                    CreateOrOpenFile(UIConst.UIDIC_CONFIG_EDITOR_PATH, "UIDicConfig", m_UIID + "|" + m_Path);
                    this.ShowNotification(new GUIContent("增加页面成功！"));
                }
            }
            m_DeleteKey = EditorGUILayout.TextField("请输入要移除的UIID", m_DeleteKey);
            if (GUILayout.Button("移除UI字典", GUILayout.Width(200))) 
            {
                if (string.IsNullOrEmpty(m_DeleteKey)) 
                {
                    this.ShowNotification(new GUIContent("ID不能为空！"));
                    return;
                }
                DeleteKeyInLine(m_DeleteKey);
            }
            if (GUILayout.Button("自动生成代码", GUILayout.Width(200)))
            {
                AutoCreateCSharpCode();
            }
        }

        private void CreateOrOpenFile(string filePath, string fileName, string info)
        {
            StreamWriter sw;
            FileInfo fi = new FileInfo(filePath + "//" + fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!File.Exists(filePath + "//" + fileName))
            {
                sw = fi.CreateText();
                sw.WriteLine(info);
                sw.Close();
                sw.Dispose();
            }
            else
            {
                sw = fi.AppendText();
                sw.WriteLine(info);
                sw.Close();
                sw.Dispose();
            }
        }

        private string ReadFile()
        {
            if (!File.Exists("Assets//Autumn//Config//UIConfig//UIDicConfig")) 
            {
                Log.i("不存在！");
                return string.Empty;
            } 
            string str=string.Empty;
            m_ArrStrs = File.ReadAllLines(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig");
            for (int i = 0; i < m_ArrStrs.Length; i++) 
            {
                str += m_ArrStrs[i];
                str +=System.Environment.NewLine;
            }
                return str;
        }

        private void DeleteKeyInLine(string key) 
        {
            bool isContainKey = false;
            if (!File.Exists(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig"))
            {
                Log.i("不存在！");
                return;
            }
            //string str = string.Empty;
            m_ArrStrs = File.ReadAllLines(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig");
            //先删除包含key的行
            for (int i = 0; i < m_ArrStrs.Length; i++) 
            {
                if (m_ArrStrs[i].Contains(key)) 
                {
                    isContainKey = true;
                    m_ArrStrs[i] = string.Empty;
                }
            }
            if (!isContainKey) 
            {
                this.ShowNotification(new GUIContent("未找到关键字！"));
                return;
            }
            //删除文件
            File.Delete(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig");
            //再重新写入
            StreamWriter sw;
            FileInfo fi = new FileInfo(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig");
            sw = fi.CreateText();
            for(int i=0;i<m_ArrStrs.Length;i++)
            {
                if (m_ArrStrs[i] != string.Empty) 
                {
                    sw.WriteLine(m_ArrStrs[i]);
                }
            }
            sw.Close();
            sw.Dispose();
        }

        private void AutoCreateCSharpCode() 
        {
            if (!File.Exists("Assets//Autumn//Config//UIConfig//UIDicConfig"))
            {
                Log.e("Code create failed!,config files not found!");
                return;
            }
            string code = string.Empty;
            m_ArrStrs = File.ReadAllLines(UIConst.UIDIC_CONFIG_EDITOR_PATH + "//UIDicConfig");
            List<string> lstCode = new List<string>();
            for (int i = 0; i < m_ArrStrs.Length; i++)
            {
                lstCode.Add(Helper.String2ListString(m_ArrStrs[i], "|")[0]);
            }
            code = GenerateCode(lstCode);
            if (string.IsNullOrEmpty(code)) 
            {
                Log.e("Code create failed! no data!");
                this.ShowNotification(new GUIContent("没有数据！请输入数据再生成！"));
                return;
            }
            DirectoryInfo info = new DirectoryInfo(UIConst.UIID_CREATE_PATH);
            if (!info.Exists) 
            {
                Directory.CreateDirectory(UIConst.UIID_CREATE_PATH);
            }
            File.WriteAllText(UIConst.UIID_CREATE_PATH + "/" + "UIID.cs", code);
            Log.i("Code is creating,plz wait!");
        }

        /// <summary>
        /// 自动生成UIID.cs
        /// </summary>
        /// <returns></returns>
        private  string GenerateCode(List<string> strArr) 
        {
            if (strArr == null) 
            {
                return string.Empty;
            }
            StringBuilder code = new StringBuilder();
            code.Append("using System;\nusing UnityEngine;\nusing System.Collections;\nusing System.Collections.Generic;\n");
            code.Append("namespace Autumn\n{\n");
            code.Append("public enum UIID\n{\n");
            for (int i = 0; i < strArr.Count; i++) 
            {
                if (i == 0)
                {
                    code.Append(strArr[i] + "=0,\n");
                }
                else 
                {
                    code.Append(strArr[i] + ",\n");
                }
            }
            code.Append("}\n}");
            return code.ToString();
        }
    }
}

