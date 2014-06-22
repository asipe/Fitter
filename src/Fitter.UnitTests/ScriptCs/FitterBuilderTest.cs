using System.Collections.Generic;
using Fitter.ScriptCs;
using NUnit.Framework;

namespace Fitter.UnitTests.ScriptCs {
  [TestFixture]
  public class FitterBuilderTest : BaseTestCase {
    [Test]
    public void TestBuildNull() {
      Assert.That(mBuilder.Build(null), Is.Empty);
    }

    [Test]
    public void TestBuildEmpty() {
      Assert.That(mBuilder.Build(new {}), Is.Empty);
    }

    [Test]
    public void TestBuildSingleSpecEntry() {
      Assert.That(mBuilder.Build(new {A = "1"}),
                  Is.EquivalentTo(new Dictionary<string, string> {{"A", "1"}}));
    }

    [Test]
    public void TestBuildMultipleSpecEntry() {
      Assert.That(mBuilder.Build(new {A = "1", B = "<a><a><d><c>", C = (string)null, D = 3}),
                  Is.EquivalentTo(new Dictionary<string, string> {
                                                                   {"A", "1"},
                                                                   {"B", "113<c>"},
                                                                   {"C", null},
                                                                   {"D", "3"}
                                                                 }));
    }

    [SetUp]
    public void DoSetup() {
      mBuilder = new FitterBuilder();
    }

    private FitterBuilder mBuilder;
  }
}