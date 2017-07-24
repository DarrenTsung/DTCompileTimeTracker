using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DTCompileTimeTracker {
  public struct UnityConsoleCountsByType {
    public int errorCount;
    public int warningCount;
    public int logCount;
  }

  public static class UnityEditorConsoleUtil {
    private static MethodInfo _clearMethod;
    private static MethodInfo _getCountMethod;
    private static MethodInfo _getCountsByTypeMethod;

    static UnityEditorConsoleUtil() {
      Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
      Type logEntriesType;
#if UNITY_2017_1_OR_NEWER
      logEntriesType = assembly.GetType("UnityEditor.LogEntries");
#else 
      logEntriesType  = assembly.GetType("UnityEditorInternal.LogEntries");
#endif

      UnityEditorConsoleUtil._clearMethod = logEntriesType.GetMethod("Clear");
      UnityEditorConsoleUtil._getCountMethod = logEntriesType.GetMethod("GetCount");
      UnityEditorConsoleUtil._getCountsByTypeMethod = logEntriesType.GetMethod("GetCountsByType");
    }

    public static void Clear() {
      if (UnityEditorConsoleUtil._clearMethod == null) {
        Debug.LogError("Failed to find LogEntries.Clear method info!");
        return;
      }

      UnityEditorConsoleUtil._clearMethod.Invoke(null, null);
    }

    public static int GetCount() {
      if (UnityEditorConsoleUtil._getCountMethod == null) {
        Debug.LogError("Failed to find LogEntries.GetCount method info!");
        return 0;
      }

      return (int)UnityEditorConsoleUtil._getCountMethod.Invoke(null, null);
    }

    public static UnityConsoleCountsByType GetCountsByType() {
      UnityConsoleCountsByType countsByType = new UnityConsoleCountsByType();

      if (UnityEditorConsoleUtil._getCountsByTypeMethod == null) {
        Debug.LogError("Failed to find LogEntries.GetCountsByType method info!");
        return countsByType;
      }

      object[] arguments = new object[] { 0, 0, 0 };
      UnityEditorConsoleUtil._getCountsByTypeMethod.Invoke(null, arguments);

      countsByType.errorCount = (int)arguments[0];
      countsByType.warningCount = (int)arguments[1];
      countsByType.logCount = (int)arguments[2];

      return countsByType;
    }
  }
}
