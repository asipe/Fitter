using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Fitter.Core;

namespace Fitter.UnitTests {
  public class ResultComparer : IEqualityComparer<object> {
    private sealed class PropertyValue {
      public PropertyValue(string name, string value) {
        Name = name;
        Value = value;
      }

      public string Name{get;private set;}
      public string Value{get;private set;}

      public override bool Equals(object obj) {
        if (ReferenceEquals(this, obj))
          return true;
        if (obj == null)
          return false;
        if (GetType() != obj.GetType())
          return false;

        var o = (PropertyValue)obj;
        return Equals(Name, o.Name) &&
               Equals(Value, o.Value);
      }

      public override int GetHashCode() {
        return 0;
      }
    }

    public new bool Equals(object anonymous, object expando) {
      return GetProperties(anonymous).SequenceEqual(GetValues(expando));
    }

    public int GetHashCode(object obj) {
      return 0;
    }

    private static IEnumerable<PropertyValue> GetProperties(object actual) {
      return actual
        .GetType()
        .GetProperties()
        .Select(pi => new PropertyValue(pi.Name, Util.ConvertToString(pi.GetValue(actual, null))))
        .OrderBy(pv => pv.Name)
        .ThenBy(pv => pv.Value);
    }

    private static IEnumerable<PropertyValue> GetValues(object actual) {
      return ((ExpandoObject)actual)
        .Select(kvp => new PropertyValue(kvp.Key, Util.ConvertToString(kvp.Value)))
        .OrderBy(pv => pv.Name)
        .ThenBy(pv => pv.Value);
    }
  }
}