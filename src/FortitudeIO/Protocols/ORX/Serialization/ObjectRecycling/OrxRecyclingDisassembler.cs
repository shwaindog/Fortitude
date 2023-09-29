using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

namespace FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling
{
    public class OrxRecyclingDisassembler<T> : IOrxRecyclingDisassembler where T : class
    {
        private readonly IPropertyRecycler[] serializers;

        public OrxRecyclingDisassembler(IOrxRecyclingDisassemblerLookup recyclingDisassemblerLookup)
        {
            recyclingDisassemblerLookup.SetRecyclingDisassembler(typeof(T), this);
            List<IPropertyRecycler> all = new List<IPropertyRecycler>();
            foreach (var kv in OrxMandatoryField.FindAll(typeof(T)))
            {
                PropertyInfo pi = kv.Value;
                if (!pi.PropertyType.IsArray && !ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                {
                    if (pi.PropertyType.IsClass)
                    {
                        if (pi.PropertyType != typeof(string))
                        {
                            var customAttributes = (OrxMandatoryField) pi
                                .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                            var mapping = customAttributes?.Mapping;
                            all.Add(Activator.CreateInstance(typeof(ReferencePropertyRecycler<>).MakeGenericType(
                                typeof(T), pi.PropertyType), pi, mapping) as IPropertyRecycler);
                        }
                    }
                    else
                    {
                        all.Add(Activator.CreateInstance(typeof(ValuePropertyRecycler<>).MakeGenericType(typeof(T),
                            pi.PropertyType), pi) as IPropertyRecycler);
                    }
                }
                else
                {
                    if (pi.PropertyType.IsArray && pi.PropertyType.GetElementType() != typeof(string))
                    {
                        var customAttributes = (OrxMandatoryField)pi
                            .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add(Activator.CreateInstance(typeof(ArrayReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GetElementType()), pi, mapping) as IPropertyRecycler);
                    } else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                    {
                        var customAttributes = (OrxMandatoryField)pi
                            .GetCustomAttributes(typeof(OrxMandatoryField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add(Activator.CreateInstance(typeof(ListReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GenericTypeArguments[0]), pi, mapping) as IPropertyRecycler);
                    }
                }
            }
            foreach (var kv in OrxOptionalField.FindAll(typeof(T)))
            {
                PropertyInfo pi = kv.Value;
                if (!pi.PropertyType.IsArray)
                {
                    if (pi.PropertyType.IsClass)
                    {
                        var customAttributes = (OrxOptionalField)pi
                            .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add(Activator.CreateInstance(typeof(ReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType), pi, mapping) as IPropertyRecycler);
                    }
                    else
                    {
                        all.Add(Activator.CreateInstance(typeof(ValuePropertyRecycler<>).MakeGenericType(typeof(T),
                            pi.PropertyType), pi) as IPropertyRecycler);
                    }
                }
                else
                {
                    if (pi.PropertyType.IsArray && pi.PropertyType.GetElementType() != typeof(string))
                    {
                        var customAttributes = (OrxOptionalField)pi
                            .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add(Activator.CreateInstance(typeof(ArrayReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GetElementType()), pi, mapping) as IPropertyRecycler);
                    }
                    else if (ReflectionHelper.IsSubclassOfRawGeneric(typeof(List<>), pi.PropertyType))
                    {
                        var customAttributes = (OrxOptionalField)pi
                            .GetCustomAttributes(typeof(OrxOptionalField)).FirstOrDefault();
                        var mapping = customAttributes?.Mapping;
                        all.Add(Activator.CreateInstance(typeof(ListReferencePropertyRecycler<>).MakeGenericType(
                            typeof(T), pi.PropertyType.GenericTypeArguments[0]), pi, mapping) as IPropertyRecycler);
                    }
                }
            }
            serializers = all.ToArray();
        }

        public void ReturnReferencePropertiesToPool(T toBeRecycled, IRecycler recyclator)
        {
            foreach (var propertyRecycler in serializers)
            {
                propertyRecycler.StripObject(toBeRecycled, recyclator);
            }
        }

        public void ReturnReferencePropertiesToPool(object toBeRecycled, IRecycler recyclator)
        {
            ReturnReferencePropertiesToPool((T)toBeRecycled, recyclator);
        }

        private interface IPropertyRecycler
        {
            void StripObject(object disintegrate, IRecycler recyclingFactory);
        }

        private class ReferencePropertyRecycler<Tp> : IPropertyRecycler where Tp : class
        {
            private readonly IDispatchToRecycler<Tp>[] possibleRecyclers;

            public ReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type> mapping)
            {
                propertyGet = (Func<T, Tp>) Delegate.CreateDelegate(typeof(Func<T, Tp>), property.GetGetMethod(true));
                propertySet = (Action<T, Tp>) Delegate.CreateDelegate(typeof(Action<T, Tp>), property.GetSetMethod(true));

                var listOfRecyclers = new List<IDispatchToRecycler<Tp>>();

                if (mapping != null)
                {
                    foreach (var keyValuePair in mapping)
                    {
                        listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(Tp), keyValuePair.Value)) as IDispatchToRecycler<Tp>);
                    }
                }
                else
                {
                    listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(Tp), typeof(Tp))) as IDispatchToRecycler<Tp>);
                }

                possibleRecyclers = listOfRecyclers.ToArray();
            }

            private readonly Func<T, Tp> propertyGet;
            private readonly Action<T, Tp> propertySet;
            

            private void StripObject(T disintegrate, IRecycler recyclingFactory)
            {
                Tp property = propertyGet(disintegrate);
                propertySet(disintegrate, null);
                if (property != null)
                {
                    foreach (var dispatchToRecycler in possibleRecyclers)
                    {
                        if (dispatchToRecycler.ExactTypeMatch(disintegrate, property))
                        {
                            dispatchToRecycler.DispatchToRecyler(property, recyclingFactory);
                        }
                    }
                }
            }

            public void StripObject(object disintegrate, IRecycler recyclingFactory)
            {
                StripObject((T) disintegrate, recyclingFactory);
            }

            private interface IDispatchToRecycler<in Tpp> where Tpp: class
            {
                bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
                void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
            }

            private class DispatchToRecycler<To> : IDispatchToRecycler<Tp> where To : class, Tp
            {
                public bool ExactTypeMatch(T disintegrate, Tp declaringPropertyType)
                {
                    return declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);
                }

                public void DispatchToRecyler(Tp declaringPropertyType, IRecycler recyclingFactory)
                {
                    recyclingFactory.Recycle((To)declaringPropertyType);
                }
            }
        }

        private class ArrayReferencePropertyRecycler<Tp> : IPropertyRecycler where Tp : class
        {
            private readonly IDispatchToRecycler<Tp>[] possibleRecyclers;

            public ArrayReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type> mapping)
            {
                propertyGet = (Func<T, Tp[]>) Delegate.CreateDelegate(typeof(Func<T, Tp[]>), property.GetGetMethod(true));
                propertySet = (Action<T, Tp[]>) Delegate.CreateDelegate(typeof(Action<T, Tp[]>), property.GetSetMethod(true));

                var listOfRecyclers = new List<IDispatchToRecycler<Tp>>();

                if (mapping != null)
                {
                    foreach (var keyValuePair in mapping)
                    {
                        listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(Tp), keyValuePair.Value)) as IDispatchToRecycler<Tp>);
                    }
                }
                else
                {
                    listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(Tp), typeof(Tp))) as IDispatchToRecycler<Tp>);
                }

                possibleRecyclers = listOfRecyclers.ToArray();
            }

            private readonly Func<T, Tp[]> propertyGet;
            private readonly Action<T, Tp[]> propertySet;
            

            private void StripObject(T disintegrate, IRecycler recyclingFactory)
            {
                Tp[] propertyArray = propertyGet(disintegrate);
                propertySet(disintegrate, null);
                if (propertyArray != null)
                {
                    foreach (var element in propertyArray)
                    {
                        foreach (var dispatchToRecycler in possibleRecyclers)
                        {
                            if (dispatchToRecycler.ExactTypeMatch(disintegrate, element))
                            {
                                dispatchToRecycler.DispatchToRecyler(element, recyclingFactory);
                            }
                        }
                    }
                }
            }

            public void StripObject(object disintegrate, IRecycler recyclingFactory)
            {
                StripObject((T) disintegrate, recyclingFactory);
            }

            private interface IDispatchToRecycler<in Tpp> where Tpp: class
            {
                bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
                void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
            }

            private class DispatchToRecycler<To> : IDispatchToRecycler<Tp> where To : class, Tp
            {
                public bool ExactTypeMatch(T disintegrate, Tp declaringPropertyType)
                {
                    return declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);
                }

                public void DispatchToRecyler(Tp declaringPropertyType, IRecycler recyclingFactory)
                {
                    recyclingFactory.Recycle((To)declaringPropertyType);
                }
            }
        }

        private class ListReferencePropertyRecycler<Tp> : IPropertyRecycler where Tp : class
        {
            private readonly IDispatchToRecycler<Tp>[] possibleRecyclers;

            public ListReferencePropertyRecycler(PropertyInfo property, IDictionary<ushort, Type> mapping)
            {
                propertyGet = (Func<T, List<Tp>>) Delegate.CreateDelegate(typeof(Func<T, List<Tp>>), property.GetGetMethod(true));
                propertySet = (Action<T, List<Tp>>) Delegate.CreateDelegate(typeof(Action<T, List<Tp>>), property.GetSetMethod(true));

                var listOfRecyclers = new List<IDispatchToRecycler<Tp>>();

                if (mapping != null)
                {
                    foreach (var keyValuePair in mapping)
                    {
                        listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                            typeof(T), typeof(Tp), keyValuePair.Value)) as IDispatchToRecycler<Tp>);
                    }
                }
                else
                {
                    listOfRecyclers.Add(Activator.CreateInstance(typeof(DispatchToRecycler<>).MakeGenericType(
                        typeof(T), typeof(Tp), typeof(Tp))) as IDispatchToRecycler<Tp>);
                }

                possibleRecyclers = listOfRecyclers.ToArray();
            }

            private readonly Func<T, List<Tp>> propertyGet;
            private readonly Action<T, List<Tp>> propertySet;
            

            private void StripObject(T disintegrate, IRecycler recyclingFactory)
            {
                List<Tp> propertyList = propertyGet(disintegrate);
                propertySet(disintegrate, null);
                if (propertyList != null)
                {
                    if (typeof(Tp) != typeof(string))
                    {
                        foreach (var element in propertyList)
                        {
                            foreach (var dispatchToRecycler in possibleRecyclers)
                            {
                                if (dispatchToRecycler.ExactTypeMatch(disintegrate, element))
                                {
                                    dispatchToRecycler.DispatchToRecyler(element, recyclingFactory);
                                }
                            }
                        }
                    }
                    propertyList.Clear();
                    recyclingFactory.Recycle(propertyList);
                }
            }

            public void StripObject(object disintegrate, IRecycler recyclingFactory)
            {
                StripObject((T) disintegrate, recyclingFactory);
            }

            private interface IDispatchToRecycler<in Tpp> where Tpp: class
            {
                bool ExactTypeMatch(T disintegrate, Tpp declaringPropertyType);
                void DispatchToRecyler(Tpp declaringPropertyType, IRecycler recyclingFactory);
            }

            private class DispatchToRecycler<To> : IDispatchToRecycler<Tp> where To : class, Tp
            {
                public bool ExactTypeMatch(T disintegrate, Tp declaringPropertyType)
                {
                    return declaringPropertyType != null && declaringPropertyType.GetType() == typeof(To);
                }

                public void DispatchToRecyler(Tp declaringPropertyType, IRecycler recyclingFactory)
                {
                    recyclingFactory.Recycle((To)declaringPropertyType);
                }
            }
        }

        private class ValuePropertyRecycler<Tp> : IPropertyRecycler where Tp : struct 
        {
            public ValuePropertyRecycler(PropertyInfo property)
            {
                propertySet = (Action<T, Tp>) Delegate.CreateDelegate(typeof(Action<T, Tp>), property.GetSetMethod(true));
            }
            
            private readonly Action<T, Tp> propertySet;

            private void StripObject(T disintegrate, IRecycler recyclingFactory)
            {
                propertySet(disintegrate, default(Tp));
            }

            public void StripObject(object disintegrate, IRecycler recyclingFactory)
            {
                StripObject((T)disintegrate, recyclingFactory);
            }

            public bool ExactTypeMatch(object disintegrate)
            {
                return true;
            }
        }
    }
}
