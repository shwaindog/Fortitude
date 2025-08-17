using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.OSWrapper.FileSystem;


public interface IFileSystemController
{
    bool DirExists(string path);
    bool EnsureDirExists(string path);
    
    bool FileExists(string path);
    
    IStream OpenOrCreateAppendWriteStream(string path, FileShare share = FileShare.ReadWrite);
    
    IStream OpenOrCreateWriteSeekingStream(string path, FileShare share = FileShare.ReadWrite);

    IPagedMemoryMappedFile OpenOrCreateMemoryMappedFile(string path, int newFileInitialSize);
}

public class FileSystemController : IFileSystemController
{
    
    private static IFileSystemController? instance;
    

    public static IFileSystemController Instance
    {
        get => instance ??= new FileSystemController();
        set => instance = value;
    }

    public bool DirExists(string? path) => Directory.Exists(path);

    public bool EnsureDirExists(string path)
    {
        var directoryName = Path.GetDirectoryName(path);
        if (directoryName != null && !DirExists(directoryName))
        {
            new DirectoryInfo(directoryName).Create();
            return true;
        }
        return false;
    }

    public bool FileExists(string path) => File.Exists(path);

    public IStream OpenOrCreateAppendWriteStream(string path, FileShare share = FileShare.ReadWrite)
    {
        var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, share);
        return new StreamWrapper(fileStream);
    }

    public IStream OpenOrCreateWriteSeekingStream(string path, FileShare share = FileShare.ReadWrite)
    {
        var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, share);
        return new StreamWrapper(fileStream);
    }

    public IPagedMemoryMappedFile OpenOrCreateMemoryMappedFile(string path, int newFileInitialSize)
    {
        return new PagedMemoryMappedFile(path, newFileInitialSize);
    }
}
