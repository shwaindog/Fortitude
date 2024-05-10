namespace FortitudeCommon.OSWrapper.Memory;

public unsafe interface IOSDirectMemoryApi
{
    nint MinimumRequiredPageSize { get; }
    void* memcpy(void* dest, void* src, ulong count);
    void* ReserveMemoryRangeInPages(void* preferredAddress, int numberOfPages);
    void* CommitPageMemory(void* preferredAddress, int numberOfPages);
    bool DecommitPageMemory(void* previouslyCommittedAddress, int numberOfPages);
    bool ReleaseReserveMemoryRangeInPages(void* previouslyReservedAddress, int numberOfPages);
    void* MapPageViewOfFile(nint memoryMappedFileHandle, int filePageNumber, int numberOfPages, void* desiredAddress);
    bool ReleaseViewOfFile(void* addressToRelease, int numberOfPages);
    bool FlushPageDataToDisk(void* startAddressToFlush, nint bytesToFlush);
}
