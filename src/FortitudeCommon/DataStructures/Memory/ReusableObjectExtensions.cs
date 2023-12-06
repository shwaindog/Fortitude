#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Memory;

public static class ReusableObjectExtensions
{
    public static T? RecycleNonNull<T>(this T? nullable, IRecycler? recycler) where T : class
    {
        if (nullable != null) recycler?.Recycle(nullable);

        return null;
    }

    public static TReturn? CopyOrClone<TCommon, TReturn>(this IReusableObject<TCommon>? source, TReturn? preExisting
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        where TReturn : class, IReusableObject<TCommon>, new() where TCommon : class, IReusableObject<TCommon>
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


    public static TReturn? SyncOrRecycle<TCommon, TReturn>(this IReusableObject<TCommon>? source, TReturn? preExisting
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        where TReturn : class, TCommon, new() where TCommon : class, IReusableObject<TCommon>
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
