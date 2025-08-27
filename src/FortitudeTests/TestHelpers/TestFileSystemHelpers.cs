using System.Reflection;

namespace FortitudeTests.TestHelpers;

public static class TestFileSystemHelpers
{

    public static ScopedWorkingDirectory GetTemporaryWorkingDirectoryFor(this Type typeNameSpacePath)
    {
        var testAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var typeSubPath          = typeNameSpacePath.Namespace!.Replace('.', Path.DirectorySeparatorChar);
        var cleanedUpClassName   = typeNameSpacePath.Name.Replace('[', '_').Replace(']', '_').Replace('`', '_');
        var fullPath             = Path.Combine(testAssemblyLocation, typeSubPath, cleanedUpClassName);
        var directoryInfo        = new DirectoryInfo(fullPath);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
        return new ScopedWorkingDirectory(Environment.CurrentDirectory, directoryInfo);
    }  
}

public class ScopedWorkingDirectory : IDisposable
{
    private string originalWorkingDirectory;
    private DirectoryInfo tempWorkingDirectory;

    public ScopedWorkingDirectory(string originalWorkingDirectory, DirectoryInfo tempWorkingDirectory)
    {
        this.originalWorkingDirectory  = originalWorkingDirectory;
        this.tempWorkingDirectory = tempWorkingDirectory;
        Environment.CurrentDirectory   = tempWorkingDirectory.FullName;
    }

    public DirectoryInfo WorkingDirectory
    {
        get => tempWorkingDirectory;
        set => tempWorkingDirectory = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void Dispose()
    {
        Environment.CurrentDirectory = originalWorkingDirectory;
    }
}
