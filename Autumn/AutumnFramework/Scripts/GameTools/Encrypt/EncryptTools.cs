//=====================================================
// - FileName:      EncryptTools.cs
// - Author:       Autumn
// - CreateTime:    2019/05/13 14:47:47
// - Email:         543761701@qq.com
// - Description:   
// -  (C) Copyright 2019, webeye,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

namespace Autumn
{
    public static class EncryptTools
    {
        private const string m_KeyValue = "12321211312321321312321312341231";
        public static string Encrypt(string value, string keyValue = m_KeyValue) 
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyValue);
            //加密格式
            RijndaelManaged encryption = new RijndaelManaged();
            encryption.Key = keyArray;
            encryption.Mode = CipherMode.ECB;
            encryption.Padding = PaddingMode.PKCS7;
            //生成加密锁
            ICryptoTransform cTransform = encryption.CreateEncryptor();
            byte[] encryptArray = UTF8Encoding.UTF8.GetBytes(value);
            byte[] resultArray = cTransform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrpt(string value, string keyValue = m_KeyValue)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(keyValue);
            RijndaelManaged decrption = new RijndaelManaged();
            decrption.Key = keyArray;
            decrption.Mode = CipherMode.ECB;
            decrption.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = decrption.CreateDecryptor();
            byte[] encryptArray = Convert.FromBase64String(value);
            byte[] resultArray = cTransform.TransformFinalBlock(encryptArray, 0, encryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}

