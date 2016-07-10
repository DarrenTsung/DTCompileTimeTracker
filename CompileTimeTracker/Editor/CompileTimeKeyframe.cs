using System;

namespace DTCompileTimeTracker {
  [Serializable]
  public class CompileTimeKeyframe {
    public DateTime Date {
      get {
        if (this._computedDate == null) {
          if (string.IsNullOrEmpty(this.serializedDate)) {
            this._computedDate = DateTime.MinValue;
          } else {
            this._computedDate = DateTime.Parse(this.serializedDate);
          }
        }

        return this._computedDate.Value;
      }
    }

    public int elapsedCompileTimeInMS;
    public string serializedDate;
    public bool hadErrors;

    public CompileTimeKeyframe(int elapsedCompileTimeInMS, bool hadErrors) {
      this.elapsedCompileTimeInMS = elapsedCompileTimeInMS;
      this.serializedDate = DateTime.Now.ToString();
      this.hadErrors = hadErrors;
    }

    [NonSerialized] private DateTime? _computedDate;
  }
}