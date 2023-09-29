using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling
{
    public class OrxRecyclingFactory : IRecycler
    {
        private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(OrxRecyclingFactory)); 

        private readonly IDictionary<Type, DisassemblerAndPoolFactoryContainer> poolFactoryMap = 
            new Dictionary<Type, DisassemblerAndPoolFactoryContainer>(128);
        private readonly IOrxRecyclingDisassemblerLookup recyclingDisassemblerLookup = 
            new OrxRecyclingDisassemblerLookup();

        public T Borrow<T>() where T : class
        {
            if (poolFactoryMap.TryGetValue(typeof(T), out var poolFactoryContainer))
            {
                var reborrowing = (T)poolFactoryContainer.PooledFactory.Borrow();
                if (reborrowing is IRecycleableObject checkRecyclerStillAdded)
                {
                    checkRecyclerStillAdded.Recycler = this;
                }
                return reborrowing;
            }
            poolFactoryContainer = CreateNewPoolFactoryContainer<T>();
            var checkingOut = (T)poolFactoryContainer.PooledFactory.Borrow();
            if (checkingOut is IRecycleableObject recycleableObject)
            {
                recycleableObject.Recycler = this;
            }
            return checkingOut;
        }

        private DisassemblerAndPoolFactoryContainer CreateNewPoolFactoryContainer<T>() where T : class
        {
            var typeOfT = typeof(T);
            var ctor = typeOfT.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new MissingMethodException("There is no constructor without defined parameters for this object");
            }
            string name = typeOfT.Name;
            if (name.Contains("`"))
            {
                name = name.Substring(0, name.IndexOf("`"));
                foreach (var genericTypeArgument in typeOfT.GenericTypeArguments)
                {
                    name += "_" + genericTypeArgument.Name;
                }
            }
            DynamicMethod dynamic = new DynamicMethod("dynamicFactoryOf" + name,
                typeOfT,
                typeOfT.GetGenericArguments(),
                GetType());
            ILGenerator il = dynamic.GetILGenerator();

            il.DeclareLocal(typeOfT);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var func = (Func<T>)dynamic.CreateDelegate(typeof(Func<T>));

            var poolFactoryContainer = new DisassemblerAndPoolFactoryContainer(
                new GarbageAndLockFreePooledFactory<T>(func),
                recyclingDisassemblerLookup.GetOrCreateRecyclingDisassembler(typeof(T)));
            poolFactoryMap.Add(typeof(T), poolFactoryContainer);
            return poolFactoryContainer;
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
}