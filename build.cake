/// <summary>
///     Just a simple build script.
/// </summary>

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
    .IsDependentOn("build");

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
    
// Execution
RunTarget(Target);
