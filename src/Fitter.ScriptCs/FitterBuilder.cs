using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Fitter.Core;
using ScriptCs.Contracts;

namespace Fitter.ScriptCs {
  public class FitterBuilder : IScriptPackContext {
    public IDictionary<string, string> Build(object spec, int maxDepth = 5) {
      return ConvertToDictionary((ExpandoObject)new Builder().Build(spec, maxDepth));
    }

    private static IDictionary<string, string> ConvertToDictionary(IEnumerable<KeyValuePair<string, object>> result) {
      return result
        .ToDictionary(kvp => kvp.Key, kvp => Util.ConvertToString(kvp.Value));
    }
  }
}