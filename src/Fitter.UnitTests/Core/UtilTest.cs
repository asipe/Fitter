using Fitter.Core;
using NUnit.Framework;

namespace Fitter.UnitTests.Core {
  [TestFixture]
  public class UtilTest : BaseTestCase {
    [TestCase("", "")]
    [TestCase(null, null)]
    [TestCase("a", "a")]
    [TestCase(1, "1")]
    public void TestConvertToString(object value, string expected) {
      Assert.That(Util.ConvertToString(value), Is.EqualTo(expected));
    }
  }
}