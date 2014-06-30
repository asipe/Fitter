using System.Linq;

namespace Fitter.Core {
  public class SpecEntryBuilder {
    public SpecEntry[] Build(object spec) {
      return spec
        .GetType()
        .GetProperties()
        .Select(p => new SpecEntry(p.Name, p.GetValue(spec, null)))
        .ToArray();
    }
  }
}