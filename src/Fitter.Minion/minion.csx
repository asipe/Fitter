using System.Diagnostics;
 
var config = Require<FitterBuilder>().Build(new {
  RootDir = Directory.GetCurrentDirectory(),
  DebugDir = @"<rootdir>\debug",
  SourceDir = @"<rootdir>\src",
  ThirdpartyDir = @"<rootdir>\thirdparty",
  PackagesDir = @"<thirdpartydir>\packages",
  NugetExePath = @"<thirdpartydir>\nuget\nuget.exe",
  NugetWorkingDir = @"<rootdir>\nugetworking",
  NunitConsoleExePath = @"<thirdpartydir>\packages\common\Nunit.Runners\tools\nunit-console.exe"
});

void TryDelete(string dir) {
  if (Directory.Exists(dir))
    Directory.Delete(dir, true);
}

void Clean() {
  TryDelete(config["DebugDir"]);
  TryDelete(config["NugetWorkingDir"]);  
}

void CleanAll() {
  Clean();
  TryDelete(config["PackagesDir"]);
}

void Echo(string message) {
  Console.WriteLine("");
  Console.WriteLine(message);
}

void Bootstrap() {
  Run(config["NugetExePath"], @"install .\src\Fitter.Nuget.Packages\common\packages.config -OutputDirectory .\thirdparty\packages\common -ExcludeVersion");
  Run(config["NugetExePath"], @"install .\src\Fitter.Nuget.Packages\net-4.5\packages.config -OutputDirectory .\thirdparty\packages\net-4.5 -ExcludeVersion");
}

void Run(string exePath, string args) {
  var info = new ProcessStartInfo {
    FileName = exePath,
    Arguments = args,
    UseShellExecute = false
  };
  
  using (var p = Process.Start(info)) {
    p.WaitForExit();  

    if (p.ExitCode != 0)
      throw new Exception(string.Format("{0} failed with code {1}", exePath, p.ExitCode));
  }
}

void BuildAll() {
  Run(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe", @".\src\Fitter.Build\Fitter.proj /ds /maxcpucount:6");
}

void BuildFitterCoreNugetPackages() {
  var libDir = Path.Combine(config["NugetWorkingDir"], @"Fitter.Core\lib");
  var net45Dir = Path.Combine(libDir, "net45");    

  Directory.CreateDirectory(config["NugetWorkingDir"]);  
  Directory.CreateDirectory(net45Dir);

  File.Copy(Path.Combine(config["DebugDir"], @"net-4.5\Fitter.Core\Fitter.Core.dll"),
            Path.Combine(net45Dir, "Fitter.Core.dll"));             
            
  File.Copy(Path.Combine(config["SourceDir"], @"Fitter.Nuget.Specs\Fitter.Core.dll.nuspec"),
            Path.Combine(config["NugetWorkingDir"], @"Fitter.Core\Fitter.Core.dll.nuspec"));
            
  Run(config["NugetExePath"], @"pack .\nugetworking\Fitter.Core\Fitter.Core.dll.nuspec -OutputDirectory .\nugetworking\Fitter.Core");
}

void BuildFitterScriptCsNugetPackages() {
  var libDir = Path.Combine(config["NugetWorkingDir"], @"Fitter.ScriptCs\lib");
  var net45Dir = Path.Combine(libDir, "net45");    

  Directory.CreateDirectory(config["NugetWorkingDir"]);  
  Directory.CreateDirectory(net45Dir);

  File.Copy(Path.Combine(config["DebugDir"], @"net-4.5\Fitter.ScriptCs\Fitter.ScriptCs.dll"),
            Path.Combine(net45Dir, "Fitter.ScriptCs.dll"));             
            
  File.Copy(Path.Combine(config["SourceDir"], @"Fitter.Nuget.Specs\Fitter.ScriptCs.dll.nuspec"),
            Path.Combine(config["NugetWorkingDir"], @"Fitter.ScriptCs\Fitter.ScriptCs.dll.nuspec"));
            
  Run(config["NugetExePath"], @"pack .\nugetworking\Fitter.ScriptCs\Fitter.ScriptCs.dll.nuspec -OutputDirectory .\nugetworking\Fitter.ScriptCs");
}

void BuildNugetPackages() {
  TryDelete(config["NugetWorkingDir"]);
  BuildFitterCoreNugetPackages();
  BuildFitterScriptCsNugetPackages();
}

void RunTests(string name, string assembly, string framework) {
  Console.WriteLine("----------- {0} Unit Tests {1} -----------", name, framework);
  Run(config["NunitConsoleExePath"], string.Format(@"{0} /nologo /framework:{1}", assembly, framework));
  Console.WriteLine("------------------------------------------");
}

void RunUnitTestsVS() {
  RunTests("VS", @".\src\Fitter.UnitTests\bin\debug\Fitter.UnitTests.dll", "net-4.5");
}

void RunAllTests() {
  RunUnitTestsVS();
  RunTests("debug", Path.Combine(config["DebugDir"], @"net-4.5\Fitter.UnitTests\Fitter.UnitTests.dll"), "net-4.5");
}

void PushNugetPackages() {
  Console.WriteLine("------------------------------");
  Console.WriteLine("Push Nuget Packages!!");
  Console.WriteLine("Are You Sure?  Enter YES to Continue");
  if (Console.ReadLine() == "YES") {
    Run(config["NugetExePath"], @"push .\nugetworking\Fitter.Core\Fitter.Core.0.0.0.5.nupkg");
    Run(config["NugetExePath"], @"push .\nugetworking\Fitter.ScriptCs\Fitter.ScriptCs.0.0.0.5.nupkg");
  }
  else 
    Console.WriteLine("Operation Cancelled...");
}

void ProcessCommands() {
  var exiting = false;
  
  while (!exiting) {
    Console.WriteLine("");
    Console.Write("Waiting: ");
    
    try {    
      var commands = Console
        .ReadLine()
        .Split(',')
        .Select(s => s.Trim());
        
      foreach (var command in commands) {
        switch (command) {
          case ("exit"):
          case ("x"):
            exiting = true;
            Echo("Goodbye");
            break;
          case ("clean"):
            Clean();
            break;
          case ("clean.all"):
            CleanAll();
            break;
          case ("bootstrap"):
            Bootstrap();
            break;
          case ("build.all"):
            BuildAll();
            break;  
          case ("build.nuget.packages"):
            BuildNugetPackages();
            break;                  
          case ("run.unit.tests.vs"):
            RunUnitTestsVS();
            break;  
          case ("run.all.tests"):
            RunAllTests();
            break; 
          case ("push.nuget.packages"):
            PushNugetPackages();
            break;
          default: 
            throw new Exception("Unknown Command: command");
            break;
        }
      }
    } catch (Exception e) {
      Console.WriteLine("");
      Console.WriteLine(e);
    }
  }
}

ProcessCommands();
