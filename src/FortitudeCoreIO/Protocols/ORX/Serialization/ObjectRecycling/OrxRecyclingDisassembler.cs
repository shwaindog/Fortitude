#region

using System.Reflection;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;

public class OrxRecyclingDisassembler<T> : IOrxRecyclingDisassembler where T : class
{
    private readonly IPropertyRecycler[] serializers;

    public OrxRecyclingDisassembler(IOrxRecyclingDisassemblerLookup recyclingDisassemblerLookup)
    {
        recyclingDisassemblerLookup.SetRecyclingDisassembler(typeof(T), this);
        var all = new List<IPropertyRecycler>();
        foreach (var kv in OrxMandatoryField.FindAll(typeof(T)))
        {
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
            {
                if (pi.PropertyType.IsClass)
                {
                    if (pi.PropertyType != typeof(string))
                    {
                        var customAttributes = (OrxMandatoryField?)pi
                            .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add((IPropertyRecycler)Activator.CreateInstance(
                            typeof(ReferencePropertyRecycler<>).MakeGenericType(
                                typeof(T), pi.PropertyType), pi, mapping)!);
                    }
                }
                else
                {
                    all.Add((IPropertyRecycler)Activator.CreateInstance(typeof(ValuePropertyRecycler<>).MakeGenericType(
                        typeof(T),
                        pi.PropertyType), pi)!);
                }
            }
            else
            {
                if (pi.PropertyType.IsArray && pi.PropertyType.GetElementType() != typeof(string))
                {
                    var customAttributes = (OrxMandatoryField?)pi
                        .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                    var mapping = customAttributes?.Mapping;
                    all.Add((IPropertyRecycler)Activator.CreateInstance(
                        typeof(ArrayReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GetElementType()!), pi, mapping)!);
                }
                else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    var customAttributes = (OrxMandatoryField?)pi
                        .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                    var mapping = customAttributes?.Mapping;
                    all.Add((IPropertyRecycler)Activator.CreateInstance(
                        typeof(ListReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GenericTypeArguments[0]), pi, mapping)!);
                }
            }
        }

        foreach (var kv in OrxOptionalField.FindAll(typeof(T)))
        {
            var pi = kv.Value;
            if (!pi.PropertyType.IsArray)
            {
                if (pi.PropertyType.IsClass)
                {
                    var customAttributes = (OrxOptionalField?)pi
                        .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                    var mapping = customAttributes?.Mapping;
                    all.Add((IPropertyRecycler)Activator.CreateInstance(
                        typeof(ReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType), pi, mapping)!);
                }
                else
                {
                    all.Add((IPropertyRecycler)Activator.CreateInstance(typeof(ValuePropertyRecycler<>).MakeGenericType(
                        typeof(T),
                        pi.PropertyType), pi)!);
                }
            }
            else
            {
                if (pi.PropertyType.IsArray && pi.PropertyType.GetElementType() != typeof(string))
                {
                    var customAttributes = (OrxOptionalField?)pi
                        .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                    var mapping = customAttributes?.Mapping;
                    all.Add((IPropertyRecycler)Activator.CreateInstance(
                        typeof(ArrayReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GetElementType()!), pi, mapping)!);
                }
                else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    var customAttributes = (OrxOptionalField?)pi
                        .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                    var mapping = customAttributes?.Mapping;
                    all.Add((IPropertyRecycler)Activator.CreateInstance(
                        typeof(ListReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GenericTypeArguments[0]), pi, mapping)!);
                }
            }
        }

        serializers = all.ToArray();
    }

    public void ReturnReferencePropertiesToPool(object toBeRecycled, IRecycler recyclator)
    {
        ReturnReferencePropertiesToPool((T)toBeRecycled, recyclator);
    }

    public void ReturnReferencePropertiesToPool(T toBeRecycled, IRecycler recyclator)
    {
        foreach (var propertyRecycler in serializers) propertyRecycler.StripObject(toBeRecycled, recyclator);
    }

    private interface IPropertyRecycler
    {
        void StripObject(object disintegrate, IRecycler recyclingFactory);
    }

    private class ReferencePropertyRecycler<TP> : IPropertyRecycler where TP : class
    {
        private readonly IDispatchToRecycler<TP>[] possibleRecyclers;

        private readonly Func<T, TP?> propertyGet;
        private readonly Action<T, TP?> propertySet;

        public ReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type>? mapping)
        {
            propertyGet = (Func<T, TP?>)Delegate.CreateDelegate(typeof(Func<T, TP?>), property.GetGetMethod(true)!);
            propertySet = (Action<T, TP?>)Delegate.CreateDelegate(typeof(Action<T, TP?>), property.GetSetMethod(true)!);

            var listOfRecyclers = new List<IDispatchToRecycler<TP>>();

            if (mapping != null)
                foreach (var keyValuePair in mapping)
                    listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                        typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(TP), keyValuePair.Value))!);
            else
                listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                    typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(TP), typeof(TP)))!);

            possibleRecyclers = listOfRecyclers.ToArray();
        }

        public void StripObject(object disintegrate, IRecycler recyclingFactory)
        {
            StripObject((T)disintegrate, recyclingFactory);
        }


        private void StripObject(T disintegrate, IRecycler recyclingFactory)
        {
            var property = propertyGet(disintegrate);
            propertySet(disintegrate, null);
            if (property != null)
                foreach (var dispatchToRecycler in possibleRecyclers)
                    if (dispatchToRecycler.ExactTypeMatch(disintegrate, property))
                        dispatchToRecycler.DispatchToRecyler(property, recyclingFactory);
        }

        private interface IDispatchToRecycler<in Tpp> where Tpp : class
        {
            bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
            void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
        }

        private class DispatchToRecycler<To> : IDispatchToRecycler<TP> where To : class, TP
        {
            public bool ExactTypeMatch(T disintegrate, TP declaringPropertyType) =>
                declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);

            public void DispatchToRecyler(TP declaringPropertyType, IRecycler recyclingFactory)
            {
                recyclingFactory.Recycle((To)declaringPropertyType);
            }
        }
    }

    private class ArrayReferencePropertyRecycler<TP> : IPropertyRecycler where TP : class
    {
        private readonly IDispatchToRecycler<TP>[] possibleRecyclers;

        private readonly Func<T, TP[]?> propertyGet;
        private readonly Action<T, TP[]?> propertySet;

        public ArrayReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type>? mapping)
        {
            propertyGet = (Func<T, TP[]?>)Delegate.CreateDelegate(typeof(Func<T, TP[]?>), property.GetGetMethod(true)!);
            propertySet = (Action<T, TP[]?>)Delegate.CreateDelegate(typeof(Action<T, TP[]?>)
                , property.GetSetMethod(true)!);

            var listOfRecyclers = new List<IDispatchToRecycler<TP>>();

            if (mapping != null)
                foreach (var keyValuePair in mapping)
                    listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                        typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(TP), keyValuePair.Value))!);
            else
                listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                    typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(TP), typeof(TP)))!);

            possibleRecyclers = listOfRecyclers.ToArray();
        }

        public void StripObject(object disintegrate, IRecycler recyclingFactory)
        {
            StripObject((T)disintegrate, recyclingFactory);
        }


        private void StripObject(T disintegrate, IRecycler recyclingFactory)
        {
            var propertyArray = propertyGet(disintegrate);
            propertySet(disintegrate, null);
            if (propertyArray != null)
                foreach (var element in propertyArray)
                foreach (var dispatchToRecycler in possibleRecyclers)
                    if (dispatchToRecycler.ExactTypeMatch(disintegrate, element))
                        dispatchToRecycler.DispatchToRecyler(element, recyclingFactory);
        }

        private interface IDispatchToRecycler<in Tpp> where Tpp : class
        {
            bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
            void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
        }

        private class DispatchToRecycler<To> : IDispatchToRecycler<TP> where To : class, TP
        {
            public bool ExactTypeMatch(T disintegrate, TP declaringPropertyType) =>
                declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);

            public void DispatchToRecyler(TP declaringPropertyType, IRecycler recyclingFactory)
            {
                recyclingFactory.Recycle((To)declaringPropertyType);
            }
        }
    }

    private class ListReferencePropertyRecycler<TP> : IPropertyRecycler where TP : class
    {
        private readonly IDispatchToRecycler<TP>[] possibleRecyclers;

        private readonly Func<T, List<TP>?> propertyGet;
        private readonly Action<T, List<TP>?> propertySet;

        public ListReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type>? mapping)
        {
            propertyGet = (Func<T, List<TP>?>)Delegate.CreateDelegate(typeof(Func<T, List<TP>?>)
                , property.GetGetMethod(true)!);
            propertySet = (Action<T, List<TP>?>)Delegate.CreateDelegate(typeof(Action<T, List<TP>?>)
                , property.GetSetMethod(true)!);

            var listOfRecyclers = new List<IDispatchToRecycler<TP>>();

            if (mapping != null)
                foreach (var keyValuePair in mapping)
                    listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                        typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(TP), keyValuePair.Value))!);
            else
                listOfRecyclers.Add((IDispatchToRecycler<TP>)Activator.CreateInstance(
                    typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(TP), typeof(TP)))!);

            possibleRecyclers = listOfRecyclers.ToArray();
        }

        public void StripObject(object disintegrate, IRecycler recyclingFactory)
        {
            StripObject((T)disintegrate, recyclingFactory);
        }


        private void StripObject(T disintegrate, IRecycler recyclingFactory)
        {
            var propertyList = propertyGet(disintegrate);
            propertySet(disintegrate, null);
            if (propertyList != null)
            {
                if (typeof(TP) != typeof(string))
                    foreach (var element in propertyList)
                    foreach (var dispatchToRecycler in possibleRecyclers)
                        if (dispatchToRecycler.ExactTypeMatch(disintegrate, element))
                            dispatchToRecycler.DispatchToRecyler(element, recyclingFactory);
                propertyList.Clear();
                recyclingFactory.Recycle(propertyList);
            }
        }

        private interface IDispatchToRecycler<in Tpp> where Tpp : class
        {
            bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
            void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
        }

        private class DispatchToRecycler<To> : IDispatchToRecycler<TP> where To : class, TP
        {
            public bool ExactTypeMatch(T disintegrate, TP declaringPropertyType) =>
                declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);

            public void DispatchToRecyler(TP declaringPropertyType, IRecycler recyclingFactory)
            {
                recyclingFactory.Recycle((To)declaringPropertyType);
            }
        }
    }

    private class ValuePropertyRecycler<TP> : IPropertyRecycler where TP : struct
    {
        private readonly Action<T, TP> propertySet;

        public ValuePropertyRecycler(PropertyInfo property) =>
            propertySet = (Action<T, TP>)Delegate.CreateDelegate(typeof(Action<T, TP>), property.GetSetMethod(true)!);

        public void StripObject(object disintegrate, IRecycler recyclingFactory)
        {
            StripObject((T)disintegrate, recyclingFactory);
        }

        private void StripObject(T disintegrate, IRecycler recyclingFactory)
        {
            propertySet(disintegrate, default);
        }

        public bool ExactTypeMatch(object disintegrate) => true;
    }
}
