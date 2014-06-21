using NUnit.Framework;

namespace Fitter.UnitTests {
  [TestFixture]
  public abstract class BaseTestCase {
    static BaseTestCase() {
      _Comparer = new ResultComparer();
    }

    public static ResultComparer _Comparer{get;private set;}
  }
}