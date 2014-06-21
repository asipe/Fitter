# Fitter

Fitter is a simple micro framework for building sets of paths to files and directories

### Install

nuget package page: https://nuget.org/packages/Fitter.Core/

install via package manager: Install-Package Fitter.Core

### Usage


```csharp
using System;
using Fitter.Core;

namespace Fitter.Sample {
  internal class Program {
    private static void Main(string[] args) {
      var spec = new {
                       Root = @"z:\project1",
                       Source = @"<root>\source",
                       Debug = @"<root>\debug",
                       Temp = @"<working>\temp",
                       Working = @"<debug>\working"
                     };
      var paths = new Builder().Build(spec);

      Console.WriteLine("paths.Root: {0}", paths.Root);       // z:\project1
      Console.WriteLine("paths.Source: {0}", paths.Source);   // z:\project1\source
      Console.WriteLine("paths.Debug: {0}", paths.Debug);     // z:\project1\debug
      Console.WriteLine("paths.Working: {0}", paths.Working); // z:\project1\debug\working
      Console.WriteLine("paths.Temp: {0}", paths.Temp);       // z:\project1\debug\working\temp
    }
  }
}
```

src\Samples contains a solution with some samples

### License

Fitter is licensed under the MIT license

     The MIT License (MIT)

     Copyright (c) 2014 Andy Sipe

     Permission is hereby granted, free of charge, to any person obtaining a copy
     of this software and associated documentation files (the "Software"), to deal
     in the Software without restriction, including without limitation the rights
     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
     copies of the Software, and to permit persons to whom the Software is
     furnished to do so, subject to the following conditions:

     The above copyright notice and this permission notice shall be included in all
     copies or substantial portions of the Software.

     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
     SOFTWARE.
