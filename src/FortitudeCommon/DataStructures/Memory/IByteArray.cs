// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public interface IByteArray : IReadOnlyList<byte>, IDisposable
{
    int Length { get; }
    byte this[long index] { get; set; }
    new int Count { get; }
    void    SetLength(long newSize);
    void    CopyTo(byte[] array, int arrayIndex);
    void    Flush();
}

public class ObjectByteArrayWrapper : IByteArray
{
    private byte[] backingArray;

    public ObjectByteArrayWrapper(byte[] backingArray) => this.backingArray = backingArray;

    IEnumerator IEnumerable. GetEnumerator() => GetEnumerator();
    public IEnumerator<byte> GetEnumerator() => backingArray.ToList().GetEnumerator();

    public int Count => backingArray.Length;

    byte IReadOnlyList<byte>.this[int index] => this[index];

    public byte this[long index]
    {
        get => backingArray[index];
        set => backingArray[index] = value;
    }

    public void Dispose() { }

    public void Flush() { }

    public void SetLength(long newSize)
    {
        if (newSize == Length) return;
        var replacementArray = new byte[newSize];
        Array.Copy(backingArray, replacementArray, newSize);
        backingArray = replacementArray;
    }

    public int     Length => backingArray.Length;
    int IByteArray.Count  => Count;

    public void CopyTo(byte[] array, int arrayIndex)
    {
        backingArray.CopyTo(array, arrayIndex);
    }
}
