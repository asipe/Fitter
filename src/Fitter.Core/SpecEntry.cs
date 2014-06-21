using System.Text.RegularExpressions;

namespace Fitter.Core {
  public class SpecEntry {
    public SpecEntry(string name, object value) {
      Name = name;
      Value = value == null ? null : value.ToString();
      mPattern = new Regex(Regex.Escape(string.Concat("<", name.ToLower(), ">")), RegexOptions.IgnoreCase);
    }

    public string Name{get;private set;}
    public string Value{get;private set;}

    public bool NeedsUpdate {
      get {return (Value != null) && _UpdatePattern.IsMatch(Value);}
    }

    public void ApplyTo(SpecEntry spec) {
      if (Value != null)
        spec.UpdateValue(mPattern, Value);
    }

    public void UpdateValue(Regex pattern, string replacementValue) {
      if (Value != null)
        Value = pattern.Replace(Value, replacementValue);
    }

    private static readonly Regex _UpdatePattern = new Regex("<[^< ]+>", RegexOptions.Compiled);
    private readonly Regex mPattern;
  }
}