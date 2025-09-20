// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public static class ReusableObjectExtensions
{
    public static T? RecycleNonNull<T>(this T? nullable, IRecycler? recycler) where T : class, IRecyclableObject
    {
        if (nullable != null) recycler?.Recycle(nullable);

        return null;
    }

    public static TReturn? CopyOrClone<TCommon, TReturn>
        (this TCommon? source, TReturn? preExisting, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        where TReturn : class, ITransferState<TCommon>, IRecyclableObject, new() where TCommon : class, IRecyclableObject
    {
        if (preExisting != null && source != null)
        {
            preExisting.CopyFrom(source, copyMergeFlags);
            return preExisting;
        }

        if (source != null)
        {
            var newInstance = source.Recycler?.Borrow<TReturn>() ?? new TReturn();
            newInstance.CopyFrom(source, copyMergeFlags);
            return newInstance;
        }

        preExisting?.DecrementRefCount();
        return null;
    }


    public static TReturn? SyncOrRecycle<TCommon, TReturn>
        (this TCommon? source, TReturn? preExisting, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        where TReturn : class, ITransferState<TCommon>, IRecyclableObject, new() where TCommon : class, IRecyclableObject
    {
        if (source != null) return source.CopyOrClone(preExisting, copyMergeFlags);
        preExisting?.DecrementRefCount();
        return null;
    }

    public static TReturn? CloneOrRecycle<TCommon, TReturn>(this ICloneable<TCommon>? source, TReturn? preExisting)
        where TReturn : class, TCommon where TCommon : class, IRecyclableObject
    {
        preExisting?.DecrementRefCount();
        return source?.Clone() as TReturn;
    }
}
