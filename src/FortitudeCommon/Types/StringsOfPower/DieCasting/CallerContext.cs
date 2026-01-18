// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public struct CallerContext : IStructTransferState<CallerContext>, IEquatable<CallerContext>
{
    // private          uint            packedDepthDelta;
    
    private IStringBuilder? callerFieldName;

    public CallerContext() { }

    public FormatFlags FormatFlags { get; set; }

    public string? FormatString { get; set; }

    public IStringBuilder CallerFieldName => callerFieldName ??= TheOneString.PropertyNameDefaultBufferSize.SourceCharArrayStringBuilder();

    public bool IsFieldNameKey { get; set; }

    public bool IsIndexAccessedValue => FieldNameRetrieveIndex >= 0;

    public int FieldNameRetrieveIndex { get; set; } = -1; 
    
    
    public Type? CallerType { get; set; } 
    
    public int SetFieldName(ReadOnlySpan<char> fieldName) => CallerFieldName.Clear().Append(fieldName).Length;

    public CallerContext Clear()
    {
        FormatFlags = FormatFlags.DefaultCallerTypeFlags;
        FormatString = null;
        callerFieldName?.DecrementRefCount();
        callerFieldName = null;
        IsFieldNameKey = false;
        FieldNameRetrieveIndex = -1;
        CallerType = null;

        return this;
    }


    public CallerContext CopyFrom(CallerContext? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source == null)
        {
            Clear();
            
            return this;
        }
        return CopyFrom(source.Value, copyMergeFlags);
    }

    public CallerContext CopyFrom(CallerContext source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FormatFlags  = source.FormatFlags;
        FormatString = source.FormatString;
        if (source.callerFieldName == null)
        {
            if (callerFieldName != null)
            {
                callerFieldName.DecrementRefCount();
                callerFieldName = null;
            }
        } else if (source.callerFieldName.Length > 0)
        {
            if (callerFieldName != null && callerFieldName.Capacity < source.callerFieldName.Length)
            {
                callerFieldName.DecrementRefCount();
                callerFieldName = null;
            }
            callerFieldName ??= source.callerFieldName.Capacity.SourceCharArrayStringBuilder();
            callerFieldName.Clear();
            callerFieldName.Append(source.callerFieldName);
        }
        else
        {
            callerFieldName?.DecrementRefCount();
            callerFieldName = null;
        }
        
        IsFieldNameKey         = source.IsFieldNameKey;
        FieldNameRetrieveIndex = -1;
        CallerType             = source.CallerType;

        return this;
    }

    //
    // public sbyte InitiatorDepthDelta
    // {
    //     get => unchecked ((sbyte)(byte)(packedDepthDelta & 0xFF));
    //     set => packedDepthDelta = packedDepthDelta & 0xFFFFFF00 | unchecked((byte)value);
    // }
    //
    // public sbyte InitiatorChildDepthDelta
    // {
    //     get => unchecked ((sbyte)(byte)((packedDepthDelta >> 8) & 0xFF));
    //     set => packedDepthDelta = packedDepthDelta & 0xFFFF00FF | ((uint)unchecked((byte)value) << 8);
    // }
    //
    // public sbyte InitiatorDescendantDepthDelta
    // {
    //     get => unchecked ((sbyte)(byte)((packedDepthDelta >> 16) & 0xFF));
    //     set => packedDepthDelta = packedDepthDelta & 0xFF00FFFF | ((uint)unchecked((byte)value) << 16);
    // }
    //
    // public byte CappedMinRemainingDepth
    // {
    //     get => unchecked ((byte)((packedDepthDelta >> 24) & 0xFF));
    //     set => packedDepthDelta = packedDepthDelta & 0x00FFFFFF | ((uint)value << 24);
    // }
    //
    // public static implicit operator CallerContext(string? formatString)             => new() { FormatString = formatString };
    // public static implicit operator CallerContext(FormatFlags formatFlags) => new() { FormatFlags  = formatFlags };
    //
    // public static implicit operator CallerContext((string?, FormatFlags) formatStringAndFlags) => 
    //     new()
    //     {
    //         FormatString = formatStringAndFlags.Item1
    //       , FormatFlags  = formatStringAndFlags.Item2
    //     };
    public bool Equals(CallerContext other) => 
        Equals(callerFieldName, other.callerFieldName) 
     && FormatFlags == other.FormatFlags 
     && FormatString == other.FormatString 
     && IsFieldNameKey == other.IsFieldNameKey 
     && FieldNameRetrieveIndex == other.FieldNameRetrieveIndex 
     && CallerType == other.CallerType;

    public override bool Equals(object? obj) => obj is CallerContext other && Equals(other);

    public override int GetHashCode() => 
        HashCode.Combine(callerFieldName, FormatFlags, FormatString
                       , IsFieldNameKey, FieldNameRetrieveIndex, CallerType);
}
