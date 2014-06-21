using System.Text.RegularExpressions;
using Fitter.Core;
using NUnit.Framework;

namespace Fitter.UnitTests.Core {
  [TestFixture]
  public class SpecEntryTest : BaseTestCase {
    [TestCase("avalue", "avalue")]
    [TestCase("", "")]
    [TestCase(null, null)]
    [TestCase(1, "1")]
    public void TestDefaults(object value, string expected) {
      var entry = new SpecEntry("aname", value);
      Assert.That(entry.Name, Is.EqualTo("aname"));
      Assert.That(entry.Value, Is.EqualTo(expected));
    }

    [TestCase("Prop1", "a", "", "")]
    [TestCase("Prop1", null, "", "")]
    [TestCase("Prop1", "a", "Prop1", "Prop1")]
    [TestCase("Prop1", null, "Prop1", "Prop1")]
    [TestCase("Prop1", "a", "<Prop1>", "a")]
    [TestCase("Prop1", "a", "<Prop1><Prop1>", "aa")]
    [TestCase("Prop1", "a", "<Prop1>blah<Prop1>", "ablaha")]
    [TestCase("Prop1", "a", "<prop1>", "a")]
    [TestCase("Prop1", "a", "<PROP1>", "a")]
    [TestCase("Prop1", "a", "<ProP1>", "a")]
    [TestCase("Prop1", "a", "< prop1>", "< prop1>")]
    public void TestApplyTo(string name, string value, string incoming, string expected) {
      var spec = new SpecEntry("", incoming);
      new SpecEntry(name, value).ApplyTo(spec);
      Assert.That(spec.Value, Is.EqualTo(expected));
    }

    [TestCase("", false)]
    [TestCase(null, false)]
    [TestCase("  ", false)]
    [TestCase("<", false)]
    [TestCase(">", false)]
    [TestCase("<>", false)]
    [TestCase("< >", false)]
    [TestCase("<        >", false)]
    [TestCase("<prop><        >", true)]
    [TestCase("<prop><a><b>", true)]
    [TestCase("<<<<kkk<", false)]
    [TestCase("abcde<prop>", true)]
    [TestCase("abcde<prop>jijijij", true)]
    [TestCase("<prop>jijijij", true)]
    public void TestNeedsUpdate(string value, bool expected) {
      Assert.That(new SpecEntry("", value).NeedsUpdate, Is.EqualTo(expected));
    }

    [TestCase("", "<a>", "1", "")]
    [TestCase(null, "<a>", "1", null)]
    [TestCase("<a>", "<a>", "1", "1")]
    [TestCase("<a>a", "<a>", "1", "1a")]
    public void TestUpdateValue(string original, string pattern, string value, string expected) {
      var spec = new SpecEntry("", original);
      spec.UpdateValue(new Regex(pattern), value);
      Assert.That(spec.Value, Is.EqualTo(expected));
    }
  }
}