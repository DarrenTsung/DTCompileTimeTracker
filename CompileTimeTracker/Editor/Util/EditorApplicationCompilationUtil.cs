using System;
using UnityEditor;
using UnityEngine;

namespace DTCompileTimeTracker {
  [InitializeOnLoad]
  public static class EditorApplicationCompilationUtil {
    public static event Action StartedCompiling = delegate {};
    public static event Action FinishedCompiling = delegate {};

    static EditorApplicationCompilationUtil() {
      EditorApplication.update += EditorApplicationCompilationUtil.OnEditorUpdate;
    }


    private static bool StoredCompilingState {
      get { return EditorPrefs.GetBool("EditorApplicationCompilationUtil::StoredCompilingState"); }
      set { EditorPrefs.SetBool("EditorApplicationCompilationUtil::StoredCompilingState", value); }
    }

    private static void OnEditorUpdate() {
      if (EditorApplication.isCompiling && EditorApplicationCompilationUtil.StoredCompilingState == false) {
        EditorApplicationCompilationUtil.StoredCompilingState = true;
        EditorApplicationCompilationUtil.StartedCompiling.Invoke();
      }

      if (!EditorApplication.isCompiling && EditorApplicationCompilationUtil.StoredCompilingState == true) {
        EditorApplicationCompilationUtil.StoredCompilingState = false;
        EditorApplicationCompilationUtil.FinishedCompiling.Invoke();
      }
    }
  }
}