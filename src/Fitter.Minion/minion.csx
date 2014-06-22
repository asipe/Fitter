using System.Diagnostics;

var root = Directory.GetCurrentDirectory();
var thirdparty = Path.Combine(root, "thirdparty");
var config =  new {
    Paths = new {
      RootDir = root,
      DebugDir = Path.Combine(root, "debug"),
      SourceDir = Path.Combine(root, "src"),
      ThirdpartyDir = thirdparty,
      PackagesDir = Path.Combine(thirdparty, "packages"),
      NugetExePath = Path.Combine(thirdparty, @"nuget\nuget.exe"),
      NugetWorkingDir = Path.Combine(root, "nugetworking"),
      NunitConsoleExePath = Path.Combine(thirdparty, @"packages\common\Nunit.Runners\tools\nunit-console.exe")
    }
  };
  
void TryDelete(string dir) {
  if (Directory.Exists(dir))
    Directory.Delete(dir, true);
}

void Clean() {
  TryDelete(config.Paths.DebugDir);
  TryDelete(config.Paths.NugetWorkingDir);  
}

void CleanAll() {
  Clean();
  TryDelete(config.Paths.PackagesDir);
}

void Echo(string message) {
  Console.WriteLine("");
  Console.WriteLine(message);
}

void Bootstrap() {
  Run(config.Paths.NugetExePath, @"install .\src\Fitter.Nuget.Packages\common\packages.config -OutputDirectory .\thirdparty\packages\common -ExcludeVersion");
  Run(config.Paths.NugetExePath, @"install .\src\Fitter.Nuget.Packages\net-4.5\packages.config -OutputDirectory .\thirdparty\packages\net-4.5 -ExcludeVersion");
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
  var libDir = Path.Combine(config.Paths.NugetWorkingDir, @"Fitter.Core\lib");
  var net45Dir = Path.Combine(libDir, "net45");    

  Directory.CreateDirectory(config.Paths.NugetWorkingDir);  
  Directory.CreateDirectory(net45Dir);

  File.Copy(Path.Combine(config.Paths.DebugDir, @"net-4.5\Fitter.Core\Fitter.Core.dll"),
            Path.Combine(net45Dir, "Fitter.Core.dll"));             
            
  File.Copy(Path.Combine(config.Paths.SourceDir, @"Fitter.Nuget.Specs\Fitter.Core.dll.nuspec"),
            Path.Combine(config.Paths.NugetWorkingDir, @"Fitter.Core\Fitter.Core.dll.nuspec"));
            
  Run(config.Paths.NugetExePath, @"pack .\nugetworking\Fitter.Core\Fitter.Core.dll.nuspec -OutputDirectory .\nugetworking\Fitter.Core");
}

void BuildFitterScriptCsNugetPackages() {
  var libDir = Path.Combine(config.Paths.NugetWorkingDir, @"Fitter.ScriptCs\lib");
  var net45Dir = Path.Combine(libDir, "net45");    

  Directory.CreateDirectory(config.Paths.NugetWorkingDir);  
  Directory.CreateDirectory(net45Dir);

  File.Copy(Path.Combine(config.Paths.DebugDir, @"net-4.5\Fitter.ScriptCs\Fitter.ScriptCs.dll"),
            Path.Combine(net45Dir, "Fitter.ScriptCs.dll"));             
            
  File.Copy(Path.Combine(config.Paths.SourceDir, @"Fitter.Nuget.Specs\Fitter.ScriptCs.dll.nuspec"),
            Path.Combine(config.Paths.NugetWorkingDir, @"Fitter.ScriptCs\Fitter.ScriptCs.dll.nuspec"));
            
  Run(config.Paths.NugetExePath, @"pack .\nugetworking\Fitter.ScriptCs\Fitter.ScriptCs.dll.nuspec -OutputDirectory .\nugetworking\Fitter.ScriptCs");
}

void BuildNugetPackages() {
  TryDelete(config.Paths.NugetWorkingDir);
  BuildFitterCoreNugetPackages();
  BuildFitterScriptCsNugetPackages();
}

void RunTests(string name, string assembly, string framework) {
  Console.WriteLine("----------- {0} Unit Tests {1} -----------", name, framework);
  Run(config.Paths.NunitConsoleExePath, string.Format(@"{0} /nologo /framework:{1}", assembly, framework));
  Console.WriteLine("------------------------------------------");
}

void RunUnitTestsVS() {
  RunTests("VS", @".\src\Fitter.UnitTests\bin\debug\Fitter.UnitTests.dll", "net-4.5");
}

void RunAllTests() {
  RunUnitTestsVS();
  RunTests("debug", Path.Combine(config.Paths.DebugDir, @"net-4.5\Fitter.UnitTests\Fitter.UnitTests.dll"), "net-4.5");
}

void PushNugetPackages() {
  Console.WriteLine("------------------------------");
  Console.WriteLine("Push Nuget Packages!!");
  Console.WriteLine("Are You Sure?  Enter YES to Continue");
  if (Console.ReadLine() == "YES") {
    Run(config.Paths.NugetExePath, @"push .\nugetworking\Fitter.Core\Fitter.Core.0.0.0.2.nupkg");
    Run(config.Paths.NugetExePath, @"push .\nugetworking\Fitter.ScriptCs\Fitter.ScriptCs.0.0.0.2.nupkg");
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
