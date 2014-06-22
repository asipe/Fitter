using System;
using Fitter.Core;

namespace Fitter.Sample {
  internal class Program {
    private static void Main(string[] args) {
      var spec = new {
                       Root = @"z:\project1\<version>",
                       Source = @"<root>\source",
                       Debug = @"<root>\debug",
                       Temp = @"<working>\temp\<cycle>",
                       Working = @"<debug>\working",
                       Cycle = 1,
                       Version = new Version(10, 20)
                     };
      var paths = new Builder().Build(spec);

      Console.WriteLine("paths.Root: {0}", paths.Root);
      Console.WriteLine("paths.Source: {0}", paths.Source);
      Console.WriteLine("paths.Debug: {0}", paths.Debug);
      Console.WriteLine("paths.Working: {0}", paths.Working);
      Console.WriteLine("paths.Temp: {0}", paths.Temp);
      Console.WriteLine("paths.Cycle: {0}", paths.Cycle);
      Console.WriteLine("paths.Version: {0}", paths.Version);
    }
  }
}