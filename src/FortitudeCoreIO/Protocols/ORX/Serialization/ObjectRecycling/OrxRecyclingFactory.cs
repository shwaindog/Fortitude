#region

using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;

public class OrxRecyclingFactory : IRecycler
{
    private static readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(OrxRecyclingFactory));

    private readonly IDictionary<Type, DisassemblerAndPoolFactoryContainer> poolFactoryMap =
        new Dictionary<Type, DisassemblerAndPoolFactoryContainer>(128);

    private readonly IOrxRecyclingDisassemblerLookup recyclingDisassemblerLookup =
        new OrxRecyclingDisassemblerLookup();

    public T Borrow<T>() where T : class, new()
    {
        if (poolFactoryMap.TryGetValue(typeof(T), out var poolFactoryContainer))
        {
            var reborrowing = (T)poolFactoryContainer.PooledFactory.Borrow();
            if (reborrowing is IRecycleableObject checkRecyclerStillAdded) checkRecyclerStillAdded.Recycler = this;
            return reborrowing;
        }

        poolFactoryContainer = CreateNewPoolFactoryContainer<T>();
        var checkingOut = (T)poolFactoryContainer.PooledFactory.Borrow();
        if (checkingOut is IRecycleableObject recycleableObject) recycleableObject.Recycler = this;
        return checkingOut;
    }

    public void Recycle(object trash)
    {
        if (poolFactoryMap.TryGetValue(trash.GetType(), out var poolFactoryContainer))
        {
            poolFactoryContainer.RecyclingDisassembler.ReturnReferencePropertiesToPool(trash, this);
            poolFactoryContainer.PooledFactory.ReturnBorrowed(trash);
            return;
        }

        logger.Debug("Returning item without recycling factory having created it.");
    }

    private DisassemblerAndPoolFactoryContainer CreateNewPoolFactoryContainer<T>() where T : class, new()
    {
        var typeOfT = typeof(T);
        var ctor = typeOfT.GetConstructor(Type.EmptyTypes);
        if (ctor == null)
            throw new MissingMethodException(
                $"There is no constructor without defined parameters for object of type {typeof(T).FullName}");
        var name = typeOfT.Name;
        if (name.Contains("`"))
        {
            name = name.Substring(0, name.IndexOf("`"));
            foreach (var genericTypeArgument in typeOfT.GenericTypeArguments) name += "_" + genericTypeArgument.Name;
        }

        var dynamic = new DynamicMethod("dynamicFactoryOf" + name,
            typeOfT,
            typeOfT.GetGenericArguments(),
            GetType());
        var il = dynamic.GetILGenerator();

        il.DeclareLocal(typeOfT);
        il.Emit(OpCodes.Newobj, ctor);
        il.Emit(OpCodes.Stloc_0);
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ret);

        var func = (Func<T>)dynamic.CreateDelegate(typeof(Func<T>));

        var poolFactoryContainer = new DisassemblerAndPoolFactoryContainer(
            new GarbageAndLockFreePooledFactory<T>(func),
            recyclingDisassemblerLookup.GetOrCreateRecyclingDisassembler(typeof(T))!);
        poolFactoryMap.Add(typeof(T), poolFactoryContainer);
        return poolFactoryContainer;
    }

    private class DisassemblerAndPoolFactoryContainer
    {
        public DisassemblerAndPoolFactoryContainer(IPooledFactory pooledFactory,
            IOrxRecyclingDisassembler recyclingDisassembler)
        {
            PooledFactory = pooledFactory;
            RecyclingDisassembler = recyclingDisassembler;
        }

        public IPooledFactory PooledFactory { get; }

        public IOrxRecyclingDisassembler RecyclingDisassembler { get; }
    }
}
