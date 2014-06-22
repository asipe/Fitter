using Fitter.ScriptCs;
using NUnit.Framework;

namespace Fitter.UnitTests.ScriptCs {
  [TestFixture]
  public class ScriptPackTest : BaseTestCase {
    [Test]
    public void TestGetContextGivesNewFitterBuilder() {
      Assert.That(mPack.GetContext().GetType(), Is.EqualTo(typeof(FitterBuilder)));
    }

    [SetUp]
    public void DoSetup() {
      mPack = new ScriptPack();
    }

    private ScriptPack mPack;
  }
}