using System.Collections;
using Fitter.Core;
using NUnit.Framework;

namespace Fitter.UnitTests.Core {
  [TestFixture]
  public class BuilderTest : BaseTestCase {
    [TestCaseSource("GetTestCases")]
    public void TestEmptySpecGivesEmptyResult(object spec, int maxDepth, object expected) {
      Assert.That(mBuilder.Build(spec, maxDepth), Is.EqualTo(expected).Using(_Comparer));
    }

    [SetUp]
    public void DoSetup() {
      mBuilder = new Builder();
    }

    public IEnumerable GetTestCases() {
      yield return new TestCaseData(new {},
                                    5,
                                    new {})
        .SetName("EmptySpec");

      yield return new TestCaseData(null,
                                    5,
                                    new {})
        .SetName("NullSpec");

      yield return new TestCaseData(new {Root = (string)null},
                                    5,
                                    new {Root = (string)null})
        .SetName("NullSpec");

      yield return new TestCaseData(new {Root = @"c:\app1"},
                                    5,
                                    new {Root = @"c:\app1"})
        .SetName("SingleSpecWithNoPlaceholders");

      yield return new TestCaseData(new {Root = @"<drive>\app1"},
                                    5,
                                    new {Root = @"<drive>\app1"})
        .SetName("SingleSpecWithMissingPlaceholders");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source"
                                        })
        .SetName("MultipleSpecsWithNoPlaceholders");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<root>\debug",
                                          Source = @"<root>\source"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source"
                                        })
        .SetName("MultipleSpecsWithSpecPlaceholders");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<Root>\debug",
                                          Source = @"<rOOt>\source"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source"
                                        })
        .SetName("MultipleSpecsWithSpecPlaceholdersCaseInsensitive");

      yield return new TestCaseData(new {
                                          Debug = @"<root>\debug",
                                          Source = @"<root>\source",
                                          Root = @"c:\app1"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source"
                                        })
        .SetName("MultipleSpecsWithSpecPlaceholdersSpecWithPlaceholderComesAfter");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<root>\debug",
                                          Source = @"<root>\source",
                                          ThirdParty = @"<root>\thirdparty",
                                          NugetExePath = @"<thirdparty>\nuget\nuget.exe",
                                          Deploy = @"<root>\deploy",
                                          Deploy4_5 = @"<deploy>\net-4.5"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source",
                                          ThirdParty = @"c:\app1\thirdparty",
                                          NugetExePath = @"c:\app1\thirdparty\nuget\nuget.exe",
                                          Deploy = @"c:\app1\deploy",
                                          Deploy4_5 = @"c:\app1\deploy\net-4.5"
                                        })
        .SetName("MultipleSpecsWithComplexSpecPlaceholders1");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<root>\debug",
                                          Deploy4_5 = @"<deploy>\net-4.5",
                                          Source = @"<root>\source",
                                          NugetExePath = @"<thirdparty>\nuget\nuget.exe",
                                          ThirdParty = @"<root>\thirdparty",
                                          Deploy = @"<root>\deploy"
                                        },
                                    5,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source",
                                          ThirdParty = @"c:\app1\thirdparty",
                                          NugetExePath = @"c:\app1\thirdparty\nuget\nuget.exe",
                                          Deploy = @"c:\app1\deploy",
                                          Deploy4_5 = @"c:\app1\deploy\net-4.5"
                                        })
        .SetName("MultipleSpecsWithComplexSpecPlaceholders2");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<root>\debug",
                                          Deploy4_5 = @"<deploy>\net-4.5",
                                          Source = @"<root>\source",
                                          NugetExePath = @"<thirdparty>\nuget\nuget.exe",
                                          ThirdParty = @"<root>\thirdparty",
                                          Deploy = @"<root>\deploy"
                                        },
                                    1,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source",
                                          ThirdParty = @"c:\app1\thirdparty",
                                          NugetExePath = @"<root>\thirdparty\nuget\nuget.exe",
                                          Deploy = @"c:\app1\deploy",
                                          Deploy4_5 = @"<root>\deploy\net-4.5"
                                        })
        .SetName("MultipleSpecsWithComplexSpecPlaceholders3");

      yield return new TestCaseData(new {
                                          Root = @"c:\app1",
                                          Debug = @"<root>\debug",
                                          Deploy4_5 = @"<deploy>\net-4.5",
                                          Source = @"<root>\source",
                                          NugetExePath = @"<thirdparty>\nuget\nuget.exe",
                                          ThirdParty = @"<root>\thirdparty",
                                          Deploy = @"<root>\deploy"
                                        },
                                    2,
                                    new {
                                          Root = @"c:\app1",
                                          Debug = @"c:\app1\debug",
                                          Source = @"c:\app1\source",
                                          ThirdParty = @"c:\app1\thirdparty",
                                          NugetExePath = @"c:\app1\thirdparty\nuget\nuget.exe",
                                          Deploy = @"c:\app1\deploy",
                                          Deploy4_5 = @"c:\app1\deploy\net-4.5"
                                        })
        .SetName("MultipleSpecsWithComplexSpecPlaceholders4");

      yield return new TestCaseData(new {
                                          A = "1",
                                          B = "2",
                                          C = "3",
                                          D = "4",
                                          AB = "<A><B>",
                                          ABC = "<A><B><C>",
                                          ABCD = "<A><B><C><D>",
                                          All = "<A><B><C><D>",
                                          Reversed = "<D><C><B><A>",
                                          Duplicated = "<A><A><B><B><A><A><ABCD><ABCD>",
                                          Funky = "<A>AABCD<ABCD><ABC><Reversed><Duplicated>"
                                        },
                                    1,
                                    new {
                                          A = "1",
                                          B = "2",
                                          C = "3",
                                          D = "4",
                                          AB = "12",
                                          ABC = "123",
                                          ABCD = "1234",
                                          All = "1234",
                                          Reversed = "4321",
                                          Duplicated = "11221112341234",
                                          Funky = "1AABCD1234123432111221112341234"
                                        })
        .SetName("TestCrazy");

      yield return new TestCaseData(new {
                                          B = "2",
                                          ABCD = "<A><B><C><D>",
                                          Reversed = "<D><C><B><A>",
                                          Funky = "<A>AABCD<ABCD><ABC><Reversed><Duplicated>",
                                          A = "1",
                                          Duplicated = "<A><A><B><B><A><A><ABCD><ABCD>",
                                          All = "<A><B><C><D>",
                                          C = "3",
                                          D = "4",
                                          AB = "<A><B>",
                                          ABC = "<A><B><C>",
                                          IsNull = (string)null
                                        },
                                    10,
                                    new {
                                          A = "1",
                                          B = "2",
                                          C = "3",
                                          D = "4",
                                          AB = "12",
                                          ABC = "123",
                                          ABCD = "1234",
                                          All = "1234",
                                          Reversed = "4321",
                                          Duplicated = "11221112341234",
                                          Funky = "1AABCD1234123432111221112341234",
                                          IsNull = (string)null
                                        })
        .SetName("TestCrazyMixed");
    }

    private Builder mBuilder;
  }
}