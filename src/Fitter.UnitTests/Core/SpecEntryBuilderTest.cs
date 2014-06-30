using Fitter.Core;
using NUnit.Framework;

namespace Fitter.UnitTests.Core {
  [TestFixture]
  public class SpecEntryBuilderTest : BaseTestCase {
    [Test]
    public void TestEmptySpecGivesEmptyEntries() {
      Assert.That(_Builder.Build(new {}), Is.Empty);
    }

    [Test]
    public void TestSpecWithSinglePropertyBuildsSingleEntry() {
      var expected = new[] {new SpecEntry("Name", "Bob")};
      Assert.That(_Builder.Build(new {Name = "Bob"}), Is.EqualTo(expected));
    }

    [Test]
    public void TestSpecWithMultiplePropertiesBuildsEntries() {
      var expected = new[] {
                             new SpecEntry("Root", @"c:\app1\"),
                             new SpecEntry("Source", @"<root>\source"),
                             new SpecEntry("Debug", @"<root>\debug"),
                             new SpecEntry("Build", 3)
                           };
      var spec = new {
                       Root = @"c:\app1\",
                       Source = @"<root>\source",
                       Debug = @"<root>\debug",
                       Build = 3
                     };
      Assert.That(_Builder.Build(spec), Is.EqualTo(expected));
    }

    private static readonly SpecEntryBuilder _Builder = new SpecEntryBuilder();
  }
}