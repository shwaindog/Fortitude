// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.DataStructures.MemoryPools;

public interface IReusableObject : IRecyclableObject, ITransferState, ICloneable { }

public interface IReusableObject<T> : IReusableObject, ITransferState<IReusableObject<T>>, ICloneable<T>, ITransferState<T>
    where T : class { }

public abstract class ReusableObject<T> : RecyclableObject, IReusableObject<T> where T : class
{
    protected readonly int InstanceNum = InstanceCounter<T>.NextInstanceNum;

    ITransferState ITransferState.CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) => (ITransferState)CopyFrom((T)source, copyMergeFlags);

    IReusableObject<T> ITransferState<IReusableObject<T>>.CopyFrom(IReusableObject<T> source, CopyMergeFlags copyMergeFlags) =>
        (IReusableObject<T>)CopyFrom((T)source, copyMergeFlags);

    object ICloneable.Clone() => Clone();

    public abstract T Clone();

    public abstract T CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}
