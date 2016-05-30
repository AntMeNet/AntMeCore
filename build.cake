/// <summary>
///     Just a simple build script.
/// </summary>

#tool "xunit.runner.console"
#tool "ReportUnit"

// *********************
//      ARGUMENTS
// *********************
var Target = Argument("target", "default");
var Configuration = Argument("configuration", "release");

// *********************
//      VARIABLES
// *********************
var Solution = File("AntMeCore.sln");
var Tests = new List<FilePath>();

var BuildVerbosity = Verbosity.Minimal;

// *********************
//      TASKS
// *********************

/// <summary>
///     Task to build the solution. Using MSBuild on Windows and MDToolBuild on OSX / Linux
/// </summary>
Task("build")
    .WithCriteria(() => 
    {
        var canBuild = Solution != null && FileExists(Solution);
        
        if(!canBuild)
            Information("Build skipped. To run a build set Solution variable before.");
        
        return canBuild;
    })
    .Does(() =>
    {
        DotNetBuild(Solution, cfg => 
        {
            cfg.Configuration = Configuration;
            cfg.Verbosity = BuildVerbosity;
        }); 
    });

/// <summary>
///     Task to clean all obj and bin directories as well as the ./output folder.
///     Commonly called right before build.
/// </summary>
Task("clean")
    .Does(() =>
    {
        CleanDirectories("./output");
        CleanDirectories("./bin");
        CleanDirectories(string.Format("./src/**/obj/{0}", Configuration));
        CleanDirectories(string.Format("./src/**/bin/{0}", Configuration));
        CleanDirectories(string.Format("./tests/**/obj/{0}", Configuration));
        CleanDirectories(string.Format("./tests/**/bin/{0}", Configuration));
    });

/// <summary>
///     The default task with a predefined flow.
/// </summary>
Task("default")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .IsDependentOn("build")
    .IsDependentOn("test");

/// <summary>
///     Task to rebuild. Nothing else than a clean followed by build.
/// </summary>
Task("rebuild")
    .IsDependentOn("clean")
    .IsDependentOn("build");

/// <summary>
///     Task to restore NuGet packages on solution level for all containing projects.
/// </summary>    
Task("restore")
    .WithCriteria(() => 
    {
        var canRestore = Solution != null && FileExists(Solution);
        
        if(!canRestore)
            Information("Restore skipped. To restore packages set Solution variable.");
        
        return canRestore;
    })
    .Does(() => NuGetRestore(Solution));
    
/// <summary>
///     Task to run unit tests.
/// </summary>
Task("test")
    .WithCriteria(() => 
    {
        // Find Tests by convention
        var testLibs = GetFiles(string.Format("./tests/*/bin/{0}/**/*.Tests.dll", Configuration)).ToArray();
        Tests.AddRange(testLibs);
    
        var canTest = Tests != null && Tests.Any();
        
        if(!canTest)
            Information("Test skipped. To run tests add some dlls to Tests variable.");
        
        return canTest;
    })
    .Does(() => 
    {
        CreateDirectory("./output/xunit");
    
        XUnit2(Tests, new XUnit2Settings
        {
            OutputDirectory = "./output/xunit",
            XmlReport = true,
            Parallelism = ParallelismOption.All
        });
    }).Finally(() => 
    {
        ReportUnit("./output/xunit");
    });
    
// Execution
RunTarget(Target);
