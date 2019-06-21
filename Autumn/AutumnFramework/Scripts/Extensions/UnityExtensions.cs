//=====================================================
// - FileName:      UnityExtensions.cs
// - Author:       Autumn
// - CreateTime:    2019/05/06 09:48:23
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
    public static class UnityExtensions
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Set outputList and random
            List<T> outputList = new List<T>();

            while (inputList.Count > 0)
            {
                //Select an index and item
                int rdIndex = ARandom.Range(0, inputList.Count);
                T remove = inputList[rdIndex];

                //remove it from copyList and add it to output
                inputList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

        public static Color HexToColor(string hex, bool alpha = false)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            if (alpha)
            {
                byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                float a = cc / 255f;
                return new Color(r, g, b, a);
            }
            else
                return new Color(r, g, b);
        }

        public static T CreateHandler<T>(string className) where T : class
        {
            if (string.IsNullOrEmpty(className))
            {
                return null;
            }

            Type type = Type.GetType(className);
            if (type == null)
            {
                Log.e("Not Find Handler Class:" + className);
                return null;
            }

            try
            {
                object obj = Activator.CreateInstance(type, true);

                return obj as T;
            }
            catch (Exception e)
            {
                Log.e(e);
            }
            return null;
        }

        //public static void DoBuzz(float time)
        //{
        //    if (PlayerPrefs.GetInt(Define.BUZZ_STATE, 1) > 0)
        //    {
        //        AndroidVibration.Vibrate((long)(time * 1000));
        //    }
        //}

        public static Coroutine CallWithDelay(this MonoBehaviour obj, System.Action call, float delay)
        {
            return obj.StartCoroutine(doCallWithDelay(call, delay));
        }

        static IEnumerator doCallWithDelay(System.Action call, float delay)
        {
            if (delay <= 0)
                yield return null;
            else
            {
                float start = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup < start + delay)
                {
                    yield return null;
                }
            }

            if (call != null)
                call.Invoke();
        }

        static public void SetLocalPos(this GameObject obj, Vector3 pos)
        {
            obj.transform.localPosition = pos;
        }
        static public void SetPos(this GameObject obj, Vector3 pos)
        {
            obj.transform.position = pos;
        }
        static public void SetAngle(this GameObject obj, Vector3 angle)
        {
            obj.transform.localEulerAngles = angle;
        }
        /// 设置X，只改变X c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetX(this Transform trans, float x)
        {
            trans.position = new Vector3(x, trans.position.y, trans.position.z);
        }
        /// <summary>
        /// 设置Y，只改变Y c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetY(this Transform trans, float y)
        {
            trans.position = new Vector3(trans.position.x, y, trans.position.z);
        }
        /// <summary>
        /// 设置Z，只改变Z c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetZ(this Transform trans, float z)
        {
            trans.position = new Vector3(trans.position.x, trans.position.y, z);
        }
        /// <summary>
        /// 设置LocalX，只改变LocalX c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetLocalX(this Transform trans, float x)
        {
            trans.localPosition = new Vector3(x, trans.localPosition.y, trans.localPosition.z);
        }
        /// <summary>
        /// 设置LocalY，只改变LocalY c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetLocalY(this Transform trans, float y)
        {
            trans.localPosition = new Vector3(trans.localPosition.x, y, trans.localPosition.z);
        }
        /// <summary>
        /// 设置LocalZ，只改变LocalZ c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetLocalZ(this Transform trans, float z)
        {
            trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, z);
        }
        /// <summary>
        /// 在游戏中，朝向的时候只朝向某个坐标，不低头 c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="target"></param>
        static public void LookAtXZ(this Transform trans, Vector3 target)
        {
            trans.LookAt(new Vector3(target.x, trans.position.y, target.z));
        }
        /// <summary>
        /// 在游戏中，朝向的时候只朝向x坐标
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="target"></param>
        static public void LookAtX(this Transform trans, Vector3 target)
        {
            trans.LookAt(new Vector3(target.x, trans.position.y, trans.position.z));
        }
        /// <summary>
        /// 遍历go c
        /// </summary>
        /// <param name="go"></param>
        /// <param name="handle"></param>
        static public void IterateGameObject(this GameObject go, Action<GameObject> handle)
        {
            Queue q = new Queue();
            q.Enqueue(go);
            while (q.Count != 0)
            {
                GameObject tmpGo = (GameObject)q.Dequeue();
                foreach (Transform t in tmpGo.transform)
                {
                    q.Enqueue(t.gameObject);
                }
                if (handle != null)
                {
                    handle(tmpGo);
                }
            }
        }
        /// <summary>
        /// 设置go层级关系 c
        /// </summary>
        /// <param name="go"></param>
        /// <param name="layer"></param>
        static public void SetAllLayer(this GameObject go, int layer)
        {
            IterateGameObject(go, (g) =>
            {
                g.layer = layer;
            });
        }
        /// <summary>
        /// 重置某个物体的三围等 c
        /// </summary>
        /// <param name="go"></param>
        static public void Reset(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        static public void ResetLocalAngle(this GameObject go)
        {
            go.transform.localEulerAngles = Vector3.zero;
        }

        static public void ResetLocalPos(this GameObject go)
        {
            go.transform.localPosition = Vector3.zero;
        }

        static public T AddMissingComponent<T>(this GameObject go) where T : Component
        {
#if UNITY_FLASH
		object comp = go.GetComponent<T>();
#else
            T comp = go.GetComponent<T>();
#endif
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
#if UNITY_FLASH
		return (T)comp;
#else
            return comp;
#endif
        }

        static public void SampleAnim(this Animator animator, string stateName, float normalizedTime, int layer = -1)
        {
            animator.StopPlayback();
            animator.Play(stateName, layer, normalizedTime);
            animator.StartPlayback();
        }

        static public void SampleAnim(this Animation anim, string stateName, float normalizedTime)
        {
            anim.Play(stateName);
            anim[stateName].speed = 1;
            anim[stateName].normalizedTime = normalizedTime;
            anim[stateName].speed = 0;
        }

        static public void PlayAnim(this Animation anim, string stateName)
        {
            anim.Play(stateName);
            anim[stateName].speed = 1;
        }

        static public void SetRigidBodiesKinematic(this GameObject obj, bool state)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i].gameObject.tag == "StayKinematic")
                    bodies[i].isKinematic = true;
                else
                    bodies[i].isKinematic = state;
            }
        }

        static public void SetColliderEnable(this GameObject obj, bool state)
        {
            var cols = obj.GetComponentsInChildren<Collider>();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i].enabled = state;
            }
        }

        static public void AddRigidBodiesForce(this GameObject obj, Vector3 force, ForceMode mode = ForceMode.Force)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].AddForce(force, mode);
        }

        static public void SetRigidBodiesDrag(this GameObject obj, float dragVal = 0)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].drag = dragVal;
        }

        static public void SetRigidBodiesAngularDrag(this GameObject obj, float dragVal = 0.05f)
        {
            var bodies = obj.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < bodies.Length; i++)
                bodies[i].angularDrag = dragVal;
        }

        static public List<Transform> GetChildTrsList(this Transform trsRoot)
        {
            List<Transform> parts = new List<Transform>();
            for (int i = 0; i < trsRoot.childCount; i++)
                parts.Add(trsRoot.GetChild(i));
            return parts;
        }

        //获取网格的size
        static public Vector3 GetMeshSize(this GameObject objRoot)
        {
            var mesh = objRoot.GetComponent<Renderer>();
            return mesh == null ? Vector3.zero : mesh.bounds.size;
        }
        //获取网格的中心
        static public Vector3 GetMeshCenter(this GameObject objRoot)
        {
            Vector3 boundsCenter = Vector3.zero;
            var meshes = objRoot.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < meshes.Length; j++)
            {
                boundsCenter += meshes[j].bounds.center;
            }
            boundsCenter /= meshes.Length * 1.0f;
            return boundsCenter;
        }

        //public static Sprite FindSprite(Qarth.ResLoader loader, string assetName, string spriteName)
        //{
        //    Sprite result = null;
        //    result = loader.LoadSync(spriteName) as Sprite;
        //    if (result == null)
        //    {
        //        var data = loader.LoadSync(assetName) as Qarth.SpritesData;
        //        if (data != null)
        //        {
        //            GameplayMgr.S.GameSprHandler.SetData(new Qarth.SpritesData[] { data });
        //            return GameplayMgr.S.GameSprHandler.FindSprite(spriteName) as Sprite;
        //        }
        //    }
        //    return result;
        //}

        public static bool CheckIsNewDay()
        {
            string lastSignDayKey = "lastsignstr--12354371";
            string timeString = PlayerPrefs.GetString(lastSignDayKey, "");
            DateTime lastSignDate;
            if (!string.IsNullOrEmpty(timeString))
            {
                if (DateTime.TryParse(timeString, out lastSignDate))
                {
                    DateTime today = DateTime.Today;
                    TimeSpan pass = today - lastSignDate;

                    if (pass.Days >= 1)
                    {
                        PlayerPrefs.SetString(lastSignDayKey, DateTime.Today.ToShortDateString());
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetString(lastSignDayKey, DateTime.Today.ToShortDateString());
                return true;
            }
        }

        public static void ScreenPosition2UIPosition(Camera sceneCamera, Camera uiCamera, Vector3 posInScreen, Transform uiTarget)
        {
            Vector3 viewportPos = sceneCamera.ScreenToViewportPoint(posInScreen);
            Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
            uiTarget.position = worldPos;
            Vector3 localPos = uiTarget.localPosition;
            localPos.z = 0f;
            uiTarget.localPosition = localPos;
        }

        public static void ScenePosition2UIPosition(Camera sceneCamera, Camera uiCamera, Vector3 posInScene, Transform uiTarget)
        {
            Vector3 viewportPos = sceneCamera.WorldToViewportPoint(posInScene);
            Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
            uiTarget.position = worldPos;
            Vector3 localPos = uiTarget.localPosition;
            localPos.z = 0f;
            uiTarget.localPosition = localPos;
        }

        public static Vector3 ScreenPosition2UIPositionVec(Camera sceneCamera, Camera uiCamera, Vector3 posInScene)
        {
            Vector3 viewportPos = sceneCamera.ScreenToViewportPoint(posInScene);
            Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
            return worldPos;
        }

        public static bool IsAfNonOrganic()
        {
#if UNITY_EDITOR
            return true;
#else
            string attr = PlayerPrefs.GetString("AppsFlyerAttr", "");
            return attr.Trim().ToLower().Equals("non-organic");
#endif
        }

        public static string GetMaskImg(string name)
        {
            return string.Format("{0}_h", name);
        }

        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
        {
            return Mathf.Atan2(
                       Vector3.Dot(n, Vector3.Cross(v1, v2)),
                       Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }

        public static string FormatRemainTime(long timestamp)
        {
            long day = timestamp / 86400;
            long hours = (timestamp % 86400) / 3600;
            long minute = (timestamp % 3600) / 60;
            long second = (timestamp % 60);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours
                , minute, second);
        }

        /// <summary>
        /// 设置LocalY，只改变LocalY c
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="x"></param>
        static public void SetLocalRotY(this Transform trans, float y)
        {
            trans.localEulerAngles = new Vector3(trans.localEulerAngles.x, y, trans.localEulerAngles.z);
        }
    }
}

