using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTCompileTimeTracker {
  public class CompileTimeTrackerData {
    private const int kHistoryKeyframeMaxCount = 100;

    public long StartTime {
      get { return this._startTime; }
      set {
        this._startTime = value;
        this.Save();
      }
    }

    public void AddCompileTimeKeyframe(CompileTimeKeyframe keyframe) {
      this._compileTimeHistory.Add(keyframe);
      this.Save();
    }

    public IList<CompileTimeKeyframe> GetCompileTimeHistory() {
      return this._compileTimeHistory;
    }

    public CompileTimeTrackerData(string editorPrefKey) {
      this._editorPrefKey = editorPrefKey;
      this.Load();
    }


    [SerializeField] private long _startTime;
    [SerializeField] private List<CompileTimeKeyframe> _compileTimeHistory;

    private string _editorPrefKey;

    private void Save() {
      while (this._compileTimeHistory.Count > CompileTimeTrackerData.kHistoryKeyframeMaxCount) {
        this._compileTimeHistory.RemoveAt(0);
      }

      EditorPrefs.SetString(this._editorPrefKey, JsonUtility.ToJson(this));
    }

    private void Load() {
      string serialized = EditorPrefs.GetString(this._editorPrefKey);
      JsonUtility.FromJsonOverwrite(serialized, this);
    }
  }
}