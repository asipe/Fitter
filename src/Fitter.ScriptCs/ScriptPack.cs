using ScriptCs.Contracts;

namespace Fitter.ScriptCs {
  public class ScriptPack : IScriptPack {
    public IScriptPackContext GetContext() {
      return new FitterBuilder();
    }

    public void Initialize(IScriptPackSession session) {}
    public void Terminate() {}
  }
}