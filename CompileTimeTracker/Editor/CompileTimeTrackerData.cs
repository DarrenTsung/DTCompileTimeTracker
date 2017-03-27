using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTCompileTimeTracker {
  public class CompileTimeTrackerData {
    private const int kHistoryKeyframeMaxCount = 100;

    public int StartTime {
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


    [SerializeField] private int _startTime;
    [SerializeField] private List<CompileTimeKeyframe> _compileTimeHistory;

    private string _editorPrefKey;

    private void Save() {
      while (this._compileTimeHistory.Count > CompileTimeTrackerData.kHistoryKeyframeMaxCount) {
        this._compileTimeHistory.RemoveAt(0);
      }

      EditorPrefs.SetInt(this._editorPrefKey + "._startTime", this._startTime);
      EditorPrefs.SetString(this._editorPrefKey + "._compileTimeHistory", CompileTimeKeyframe.SerializeList(this._compileTimeHistory));
    }

    private void Load() {
      this._startTime = EditorPrefs.GetInt(this._editorPrefKey + "._startTime");
      string key = this._editorPrefKey + "._compileTimeHistory";
      if (EditorPrefs.HasKey(key)) {
        this._compileTimeHistory = CompileTimeKeyframe.DeserializeList(EditorPrefs.GetString(key));
      } else {
        this._compileTimeHistory = new List<CompileTimeKeyframe>();
      }
    }
  }
}