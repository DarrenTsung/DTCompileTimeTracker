using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DTCompileTimeTracker {
  [InitializeOnLoad]
  public static class CompileTimeTracker {
    public static event Action<CompileTimeKeyframe> KeyframeAdded = delegate {};

    public static IList<CompileTimeKeyframe> GetCompileTimeHistory() {
      return CompileTimeTracker._Data.GetCompileTimeHistory();
    }

    static CompileTimeTracker() {
      EditorApplicationCompilationUtil.StartedCompiling += CompileTimeTracker.HandleEditorStartedCompiling;
      EditorApplicationCompilationUtil.FinishedCompiling += CompileTimeTracker.HandleEditorFinishedCompiling;
    }


    private const string kCompileTimeTrackerKey = "CompileTimeTracker::_data";
    private static CompileTimeTrackerData _data = null;

    private static CompileTimeTrackerData _Data {
        get {
            if (_data == null) {
                _data = new CompileTimeTrackerData(kCompileTimeTrackerKey);
            }
            return _data;
        }
    }

    private static int StoredErrorCount {
      get { return EditorPrefs.GetInt("CompileTimeTracker::StoredErrorCount"); }
      set { EditorPrefs.SetInt("CompileTimeTracker::StoredErrorCount", value); }
    }

    private static void HandleEditorStartedCompiling() {
      CompileTimeTracker._Data.StartTime = TrackingUtil.GetMilliseconds();

      UnityConsoleCountsByType countsByType = UnityEditorConsoleUtil.GetCountsByType();
      CompileTimeTracker.StoredErrorCount = countsByType.errorCount;
    }

    private static void HandleEditorFinishedCompiling() {
      int elapsedTime = TrackingUtil.GetMilliseconds() - CompileTimeTracker._Data.StartTime;

      UnityConsoleCountsByType countsByType = UnityEditorConsoleUtil.GetCountsByType();
      bool hasErrors = (countsByType.errorCount - CompileTimeTracker.StoredErrorCount) > 0;

      CompileTimeKeyframe keyframe = new CompileTimeKeyframe(elapsedTime, hasErrors);
      CompileTimeTracker._Data.AddCompileTimeKeyframe(keyframe);
      CompileTimeTracker.KeyframeAdded.Invoke(keyframe);
    }
  }
}