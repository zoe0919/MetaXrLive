using UnityEngine;
using System.Text;
using System.IO;
public class Logger
{
    public static bool s_debugLogEnable = true;
    public static bool s_warningLogEnable = true;
    public static bool s_errorLogEnable = true;
    private static StringBuilder s_logStr = new StringBuilder();
    private static string s_logFileSavePath;
    public static void Init()
    {
        var t = System.DateTime.Now.ToString("yyyyMMddhhmmss");
        s_logFileSavePath = string.Format("{0}/output_{1}.log", Application.persistentDataPath, t);
        Application.logMessageReceived += OnLogCallBack;
    }
    private static void OnLogCallBack(string condition, string stackTrace, LogType type)
    {
        var t = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss ");
        s_logStr.Append(t);
        s_logStr.Append(condition);
        s_logStr.Append("\n");
        s_logStr.Append(stackTrace);
        s_logStr.Append("\n");
        if (s_logStr.Length <= 0) return;
        if (!File.Exists(s_logFileSavePath))
        {
            var fs = File.Create(s_logFileSavePath);
            fs.Close();
        }
        using (var sw = File.AppendText(s_logFileSavePath))
        {
            sw.WriteLine(s_logStr.ToString());
        }
        s_logStr.Remove(0, s_logStr.Length);
    }
    public static void UploadLog(string desc)
    {
        //LogUploader.StartUploadLog(s_logFileSavePath, desc);
    }
    public static void Log(object message, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(message, context);
    }
    public static void LogFormat(string format, params object[] args)
    {
        if (!s_debugLogEnable) return;
        Debug.LogFormat(format, args);
    }
    public static void LogWithColor(object message, string color, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(FmtColor(color, message), context);
    }
    public static void LogRed(object message, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(FmtColor("red", message), context);
    }
    public static void LogGreen(object message, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(FmtColor("green", message), context);
    }
    public static void LogYellow(object message, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(FmtColor("yellow", message), context);
    }
    public static void LogCyan(object message, Object context = null)
    {
        if (!s_debugLogEnable) return;
        Debug.Log(FmtColor("#00ffff", message), context);
    }
    public static void LogFormatWithColor(string format, string color, params object[] args)
    {
        if (!s_debugLogEnable) return;
        Debug.LogFormat((string)FmtColor(color, format), args);
    }
    public static void LogWarning(object message, Object context = null)
    {
        if (!s_warningLogEnable) return;
        Debug.LogWarning(message, context);
    }
    public static void LogError(object message, Object context = null)
    {
        if (!s_errorLogEnable) return;
        Debug.LogError(message, context);
    }
    private static object FmtColor(string color, object obj)
    {
        if (obj is string)
        {
#if !UNITY_EDITOR
            return obj;
#else
            return FmtColor(color, (string)obj);
#endif
        }
        else
        {
#if !UNITY_EDITOR
            return obj;
#else
            return string.Format("<color={0}>{1}</color>", color, obj);
#endif
        }
    }
    private static object FmtColor(string color, string msg)
    {
#if !UNITY_EDITOR
        return msg;
#else
        int p = msg.IndexOf('\n');
        if (p >= 0) p = msg.IndexOf('\n', p + 1);
        if (p < 0 || p >= msg.Length - 1) return string.Format("<color={0}>{1}</color>", color, msg);
        if (p > 2 && msg[p - 1] == '\r') p--;
        return string.Format("<color={0}>{1}</color>{2}", color, msg.Substring(0, p), msg.Substring(p));
#endif
    }
    #region
#if UNITY_EDITOR
    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    static bool OnOpenAsset(int instanceID, int line)
    {
        string stackTrace = GetStackTrace();
        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("Logger:Log"))
        {
            var matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string pathLine = "";
            while (matches.Success)
            {
                pathLine = matches.Groups[1].Value;
                if (!pathLine.Contains("Logger.cs"))
                {
                    int splitIndex = pathLine.LastIndexOf(":");
                    string path = pathLine.Substring(0, splitIndex);
                    line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                    fullPath = fullPath + path;
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                    break;
                }
                matches = matches.NextMatch();
            }
            return true;
        }
        return false;
    }
    static string GetStackTrace()
    {
        var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.NonPublic);
        var consoleInstance = fieldInfo.GetValue(null);
        if (consoleInstance != null)
        {
            if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
            {
                fieldInfo = ConsoleWindowType.GetField("m_ActiveText",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);
                string activeText = fieldInfo.GetValue(consoleInstance).ToString();
                return activeText;
            }
        }
        return null;
    }
#endif
    #endregion
}
