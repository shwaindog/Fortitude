// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries.FileSystem.File;

[Flags]
public enum FileFlags : ushort
{
    None
  , WriterOpened              = 1
  , Corrupt                   = 2
  , WriterUpdatingHistorical  = 4
  , ClosedForReading          = 16
  , HasExternalIndexFile      = 32
  , HasInternalIndexInHeader  = 64
  , HasExternalAnnotationFile = 128
  , HasOriginalSourceText     = 256
  , HasSubFileHeader          = 512
  , WriteDataCompressed       = 1024
}

public static class FileFlagExtensions
{
    public static bool HasWriterOpenedFlag(this FileFlags flags)           => (flags & FileFlags.WriterOpened) > 0;
    public static bool HasOriginalSourceTextFlag(this FileFlags flags)     => (flags & FileFlags.HasOriginalSourceText) > 0;
    public static bool HasExternalIndexFileFlag(this FileFlags flags)      => (flags & FileFlags.HasExternalIndexFile) > 0;
    public static bool HasInternalIndexInHeaderFlag(this FileFlags flags)  => (flags & FileFlags.HasInternalIndexInHeader) > 0;
    public static bool HasExternalAnnotationFileFlag(this FileFlags flags) => (flags & FileFlags.HasExternalAnnotationFile) > 0;
    public static bool HasSubFileHeaderFileFlag(this FileFlags flags)      => (flags & FileFlags.HasSubFileHeader) > 0;
    public static bool HasWriteDataCompressedFlag(this FileFlags flags)    => (flags & FileFlags.WriteDataCompressed) > 0;

    public static FileFlags Set(this FileFlags flags, FileFlags toSet) => flags | toSet;

    public static FileFlags Unset(this FileFlags flags, FileFlags toUnset) => flags & ~toUnset;
}
