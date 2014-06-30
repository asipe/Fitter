# Fitter

Fitter is a simple micro framework for building sets of paths to files and directories

[![Fitter.Core on NuGet](http://img.shields.io/nuget/v/Fitter.Core.svg?style=flat)](https://www.nuget.org/packages/Fitter.Core)
[![Fitter.Core tag](http://img.shields.io/github/tag/asipe/fitter.svg?style=flat)](https://github.com/asipe/Fitter/tags)
[![Fitter.Core license](http://img.shields.io/badge/license-mit-blue.svg?style=flat)](https://raw.githubusercontent.com/asipe/Fitter/master/LICENSE)

### Install

nuget package (Fitter.Core): https://nuget.org/packages/Fitter.Core/

nuget package (Fitter.ScriptCs): https://nuget.org/packages/Fitter.ScriptCs/

install via package manager: Install-Package Fitter.Core

### Usage

```csharp
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

/* outputs 

paths.Root: z:\project1\10.20
paths.Source: z:\project1\10.20\source
paths.Debug: z:\project1\10.20\debug
paths.Working: z:\project1\10.20\debug\working
paths.Temp: z:\project1\10.20\debug\working\temp\1
paths.Cycle: 1
paths.Version: 10.20

*/
```

ScriptCs support is a bit different because at this time Roslyn doesn't support dynamic.

scriptcs -install Fitter.ScriptCs 

```csharp
var spec = new {
                 Root = @"z:\project1\<version>",
                 Source = @"<root>\source",
                 Debug = @"<root>\debug",
                 Temp = @"<working>\temp\<cycle>",
                 Working = @"<debug>\working",
                 Cycle = 1,
                 Version = new Version(10, 20)
               };
               
var paths = Require<FitterBuilder>().Build(spec);

Console.WriteLine("paths['Root']: {0}", paths["Root"]);
Console.WriteLine("paths['Source']: {0}", paths["Source"]);
Console.WriteLine("paths['Debug']: {0}", paths["Debug"]);
Console.WriteLine("paths['Working']: {0}", paths["Working"]);
Console.WriteLine("paths['Temp']: {0}", paths["Temp"]);
Console.WriteLine("paths['Cycle']: {0}", paths["Cycle"]);
Console.WriteLine("paths['Version']: {0}", paths["Version"]);
```
src\Samples contains some samples

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
