using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BaseToolsForUnity
{
    /// <summary>Log</summary>
    public class Log : MonoBehaviour
    {

        /// <summary>Unity Log</summary>
        /// <param name="strLog"></param>
        /// <param name="color"></param>
        /// <param name="ErrorType"></param>
        /// <param name="bAddTime"></param>
        /// <param name="bSave"></param>
        public static void LogAtUnityEditor(string strLog, string color = "white", LogErrorType ErrorType = LogErrorType.Normal, bool bAddTime = true)
        {
            strLog = string.Format("Log : [{0}] {1}", strLog, bAddTime ? DateTime.Now.ToString("yy-MM-dd HH:mm:ss:fff") : "");

            if (!Application.isEditor)
            {
                return;
            }


            string str = UnityColorString(strLog, color);

            switch (ErrorType)
            {
                case LogErrorType.Error:
                    UnityEngine.Debug.LogError(str);
                    break;
                case LogErrorType.Waring:
                    UnityEngine.Debug.LogWarning(str);
                    break;
                case LogErrorType.Normal:
                    UnityEngine.Debug.Log(str);
                    break;
                default:
                    break;
            }
        }


        /// <summary>Unity Log [Normal]</summary>
        /// <param name="strLog"></param>
        /// <param name="bAddTime"></param>
        /// <param name="bSave"></param>
        public static void LogAtUnityEditorNormal(string strLog, bool bAddTime = true)
        {
            LogAtUnityEditor(strLog, "#aaaaaaff", LogErrorType.Normal, bAddTime);
        }


        /// <summary>Unity Log [Warning]</summary>
        /// <param name="strLog"></param>
        /// <param name="bAddTime"></param>
        /// <param name="bSave"></param>
        public static void LogAtUnityEditorWarning(string strLog, bool bAddTime = true)
        {
            LogAtUnityEditor(strLog, "#ffff00ff", LogErrorType.Waring, bAddTime);
        }


        /// <summary>Unity Log [Warning]</summary>
        /// <param name="strLog"></param>
        /// <param name="needLogInvokeSource"></param>
        /// <param name="bAddTime"></param>
        /// <param name="bSave"></param>
        public static void LogAtUnityEditorError(string strLog, bool needLogInvokeSource = false, bool bAddTime = true)
        {
            if (!needLogInvokeSource)
            {
                LogAtUnityEditor(strLog, "#ff0000ff", LogErrorType.Error, bAddTime);
                return;
            }

            StackFrame[] frames = new StackTrace().GetFrames();
            if (frames == null || frames.Length == 0)
                return;
            StringBuilder sb = new(strLog);
            sb.Append("\r\n");
            for (int i = 0; i < frames.Length; i++)
            {
                sb.AppendFormat("调用者[{0}]\r\n", frames[i].GetMethod().ReflectedType.Name);
            }

            LogAtUnityEditor(sb.ToString(), "#ff0000ff", LogErrorType.Error, bAddTime);
        }


        static string UnityColorString(string strLog, string color)
        {
            if (string.IsNullOrEmpty(strLog))
                return null;
            return string.Format("<color={0}>{1}</color>", color, strLog);
        }



    }


    /// <summary>Log平台</summary>
    public enum LogPlatform
    {
        /// <summary>使用Unity的Debug</summary>
        UnityDebug,

        /// <summary>使用控制台的打印</summary>
        Console,
    }



    /// <summary>Log错误类型</summary>
    public enum LogErrorType
    {
        /// <summary>错误</summary>
        Error,

        /// <summary>警告</summary>
        Waring,

        /// <summary>正常</summary>
        Normal,
    }

}
