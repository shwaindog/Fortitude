﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace FortitudeCommon.Types;

/// <summary>
///     Provide methods used in reflection
/// </summary>
public static class ReflectionHelper
{
    public static Func<TClass> DefaultCtorFunc<TClass>()
    {
        var typeOf = typeof(TClass);

        var constructorInfo          = typeof(TClass).GetConstructor(Type.EmptyTypes)!;
        var constructorNewExpression = Expression.New(constructorInfo);

        return Expression.Lambda<Func<TClass>>(constructorNewExpression).Compile();
    }

    public static Func<TParam, TClass> CtorBinder<TParam, TClass>()
    {
        var ctorParam = Expression.Parameter(typeof(TParam), "Param");

        var constructorInfo = typeof(TClass).GetConstructor(new[] { typeof(TParam) })!;

        return Expression.Lambda<Func<TParam, TClass>>
            (Expression.New(constructorInfo, ctorParam), ctorParam).Compile();
    }

    public static Func<TParam, TClassBaseType> CtorDerivedBinder<TParam, TClassBaseType>(Type classDerivedType)
    {
        var ctorParam = Expression.Parameter(typeof(TParam), "Param");
        var constructorInfo
            = classDerivedType.GetConstructor(new[] { typeof(TParam) })!;
        return Expression.Lambda<Func<TParam, TClassBaseType>>
            (Expression.New(constructorInfo, ctorParam), ctorParam).Compile();
    }

    public static Func<TParam1, TParam2, TClass> CtorBinder<TParam1, TParam2, TClass>()
    {
        var ctorParam1 = Expression.Parameter(typeof(TParam1), "Param1");
        var ctorParam2 = Expression.Parameter(typeof(TParam2), "Param2");

        var constructorInfo = typeof(TClass).GetConstructor(new[] { typeof(TParam1), typeof(TParam2) })!;

        return Expression.Lambda<Func<TParam1, TParam2, TClass>>
            (Expression.New(constructorInfo, ctorParam1, ctorParam2), ctorParam1, ctorParam2).Compile();
    }

    public static Func<TParam1, TParam2, TClassBaseType> CtorDerivedBinder<TParam1, TParam2, TClassBaseType>(Type classDerivedType)
    {
        var ctorParam1 = Expression.Parameter(typeof(TParam1), "Param1");
        var ctorParam2 = Expression.Parameter(typeof(TParam2), "Param2");

        var constructorInfo = classDerivedType.GetConstructor(new[] { typeof(TParam1), typeof(TParam2) })!;

        return Expression.Lambda<Func<TParam1, TParam2, TClassBaseType>>
            (Expression.New(constructorInfo, ctorParam1, ctorParam2), ctorParam1, ctorParam2).Compile();
    }

    public static Func<TParam1, TParam2, TParam3, TClass> CtorBinder<TParam1, TParam2, TParam3, TClass>()
    {
        var ctorParam1 = Expression.Parameter(typeof(TParam1), "Param1");
        var ctorParam2 = Expression.Parameter(typeof(TParam2), "Param2");
        var ctorParam3 = Expression.Parameter(typeof(TParam3), "Param3");

        var constructorInfo = typeof(TClass).GetConstructor(new[] { typeof(TParam1), typeof(TParam2), typeof(TParam3) })!;

        return Expression.Lambda<Func<TParam1, TParam2, TParam3, TClass>>
            (Expression.New(constructorInfo, ctorParam1, ctorParam2, ctorParam3)
           , ctorParam1, ctorParam2, ctorParam3).Compile();
    }

    public static Func<TParam1, TParam2, TParam3, TParam4, TClass> CtorBinder<TParam1, TParam2, TParam3, TParam4, TClass>()
        where TParam4 : class?
    {
        var ctorParam1 = Expression.Parameter(typeof(TParam1), "Param1");
        var ctorParam2 = Expression.Parameter(typeof(TParam2), "Param2");
        var ctorParam3 = Expression.Parameter(typeof(TParam3), "Param3");
        var ctorParam4 = Expression.Parameter(typeof(TParam4), "Param4");

        var constructorInfo = typeof(TClass).GetConstructor(new[] { typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4) })!;

        return Expression.Lambda<Func<TParam1, TParam2, TParam3, TParam4, TClass>>
                             (Expression.New(constructorInfo, ctorParam1, ctorParam2, ctorParam3, ctorParam4)
                            , ctorParam1, ctorParam2, ctorParam3, ctorParam4)
                         .Compile();
    }

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="memberName"></param>
    /// <param name="ignoreComposedMemberNull">
    ///     Don't throw an exception if an intermediate is <see langword="null" />. For
    ///     example, if looking for "Statistics.Decile.Value", Decile is <see langword="null" />, then the method will return
    ///     <see langword="null" />.
    /// </param>
    /// <returns></returns>
    public static T? GetProperty<T>(object? obj, string memberName, bool ignoreComposedMemberNull)
    {
        if (obj == null)
            return default;

        var properties = memberName.Split('.');

        var current = obj;

        for (var i = 0; i < properties.Length; i++)
        {
            var prop = properties[i];

            object? newCurrent = null;

            if (current == null)
                if (ignoreComposedMemberNull)
                    return default;
                else
                    throw new NullReferenceException(memberName + " -> " + prop);

            try
            {
                var propertyInfo = current.GetType().GetProperty(prop);

                if (propertyInfo == null)
                    throw new NotSupportedException(memberName + " -> " + prop);

                newCurrent = propertyInfo.GetValue(current, null);
            }
            catch (AmbiguousMatchException)
            {
                foreach (var pi in current.GetType().GetProperties())
                    if (pi.Name == prop)
                    {
                        newCurrent = pi.GetValue(current, null);
                        break;
                    }
            }

            if (newCurrent == null && i < properties.Length - 1)
                if (ignoreComposedMemberNull)
                    return default;
                else
                    throw new NullReferenceException(memberName + " -> " + prop);

            current = newCurrent;
        }

        if (typeof(T) == typeof(string) && current != null)
            return (T)(object)current.ToString()!;

        return (T?)current;
    }


    public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        var currentType = toCheck;
        while (currentType != null && currentType != typeof(object))
        {
            var cur = currentType.IsGenericType ? currentType.GetGenericTypeDefinition() : currentType;
            if (generic == cur) return true;
            currentType = currentType.BaseType;
        }

        return false;
    }

    public static bool ImplementsInterface<TInterface>(this Type type) => type.GetInterfaces().Contains(typeof(TInterface));

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static T? GetProperty<T>(object obj, string memberName) => GetProperty<T>(obj, memberName, false);

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty(object obj, string memberName)
    {
        var properties = memberName.Split('.');

        PropertyInfo? currentPropertyInfo = null;

        var current = obj;

        for (var i = 0; i < properties.Length; i++)
        {
            var prop = properties[i];

            object? currentValue = null;

            if (current == null)
                throw new NullReferenceException(memberName + " -> " + prop);

            try
            {
                currentPropertyInfo = current.GetType().GetProperty(prop);

                if (currentPropertyInfo == null)
                    return null;

                currentValue = current.GetType().GetProperty(prop)!.GetValue(current, null);
            }
            catch (AmbiguousMatchException)
            {
                foreach (var pi in current.GetType().GetProperties())
                    if (pi.Name == prop)
                    {
                        currentValue        = pi.GetValue(current, null);
                        currentPropertyInfo = pi;
                        break;
                    }
            }

            if (currentValue == null && i < properties.Length - 1)
                throw new NullReferenceException(memberName + " -> " + prop);

            current = currentValue;
        }

        return currentPropertyInfo;
    }

    /// <summary>
    ///     Gets the value of a property of an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static T? GetProperty<T>(object obj, PropertyInfo property) => (T?)property.GetValue(obj, null);

    /// <summary>
    ///     Return all the properties of a given type of an object
    /// </summary>
    /// <typeparam name="T">Type of the property</typeparam>
    /// <param name="object">Object to look on</param>
    /// <returns>List of objects of type T</returns>
    public static List<T> GetProperties<T>(object @object) => GetProperties<T>(@object, false);

    /// <summary>
    ///     Return all the fields of a given type of an object
    /// </summary>
    /// <typeparam name="T">Type of the field</typeparam>
    /// <param name="object">Object to look on</param>
    /// <returns>List of objects of type T</returns>
    public static List<T> GetFields<T>(object @object)
    {
        var fieldsType = typeof(T);
        var fields     = new List<T>();

        if (@object == null)
            return fields;

        foreach (var pi in @object.GetType().GetFields())
            if (pi.FieldType == fieldsType)
                fields.Add((T)pi.GetValue(@object)!);

        return fields;
    }

    /// <summary>
    ///     Instantiate an object of the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="typeToInstantiate"></param>
    /// <returns></returns>
    public static T? Instantiate<T>(Type typeToInstantiate) => (T?)typeToInstantiate.Assembly.CreateInstance(typeToInstantiate.FullName!);


    /// <summary>
    ///     Instantiate an object of a generic Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterlessGenericType"></param>
    /// <returns></returns>
    public static object InstantiateGenericType
    (Type parameterlessGenericType,
        Type[] genericArgs, params object[] constructorArgs)
    {
        var fullConfiguredType = parameterlessGenericType.MakeGenericType(genericArgs);
        return Activator.CreateInstance(fullConfiguredType, BindingFlags.CreateInstance,
                                        null, constructorArgs, CultureInfo.CurrentCulture)!;
    }

    public static Delegate CreateEmptyConstructorFactoryAsFuncType(Type withParameterlessConstructor)
    {
        var typeConstant    = Expression.Constant(withParameterlessConstructor);
        var constructor     = withParameterlessConstructor.GetConstructor(Array.Empty<Type>());
        var callConstructor = Expression.New(constructor!);
        var funcType        = Expression.GetFuncType(withParameterlessConstructor);
        return Expression.Lambda(funcType, callConstructor).Compile();
    }

    /// <summary>
    ///     Instantiate an object of the given type
    /// </summary>
    /// <param name="typeToInstantiate"></param>
    /// <returns></returns>
    public static object? Instantiate(Type typeToInstantiate) => Instantiate<object>(typeToInstantiate);

    /// <summary>
    ///     Get a <see cref="PropertyInfo" /> object from a lambda expression representing the member to return
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static PropertyInfo GetProperty<T, TResult>(this Expression<Func<T, TResult>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return (PropertyInfo)memberExpression.Member;
    }

    public static string GetPropertyName<T, TProp>(this Expression<Func<T, TProp>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return memberExpression.Member.Name;
    }

    /// <summary>
    ///     Get a string representation of the lambda expression.
    ///     Example: a lambda of o => o.Item.SubItem will return a string of "Item.SubItem"
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static string GetCombinedProperty<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        var properties = GetProperties(expression);
        properties = properties.Reverse();

        var combinedProperty = "";

        foreach (var propertyInfo in properties) combinedProperty += "." + propertyInfo.Name;

        return combinedProperty.Substring(1, combinedProperty.Length - 1);
    }

    /// <summary>
    ///     Get a collection of <see cref="PropertyInfo" /> objects from a lambda expression representing the members to
    ///     return.
    ///     Example: if the lambda expression is item.SubItem.SubSubItem, this will return a <see cref="PropertyInfo" /> for
    ///     SubItem, and another one for SubSubItem.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetProperties<T, TResult>(Expression<Func<T, TResult>> expression)
    {
        var memberExpression = GetMemberExpression(expression);
        return GetPropertiesRecursive(memberExpression);
    }

    /// <summary>
    ///     Return all the properties of a given type of an object
    /// </summary>
    /// <typeparam name="T">Type of the property</typeparam>
    /// <param name="object">Object to look on</param>
    /// <param name="recursive">Browse the object tree recursively to find all the occurrences of the object</param>
    /// <returns>List of objects of type T</returns>
    public static List<T> GetProperties<T>(object @object, bool recursive) => GetProperties<T>(@object, recursive, new List<object>());

    /// <summary>
    ///     Get the name of the method which called this method
    /// </summary>
    /// <param name="frameIndex">0 gives the name of this method, 1 its parent, 2 parent of parent etc.</param>
    /// <param name="removeGetSetPrefix">
    ///     If the calling method is a getter or a setter, setting this to true will remove the
    ///     get_ set_ prefix
    /// </param>
    /// <returns></returns>
    public static string GetCallingMethodName(int frameIndex, bool removeGetSetPrefix)
    {
        var trace      = new StackTrace();
        var methodName = trace.GetFrame(frameIndex)!.GetMethod()!.Name;

        if (removeGetSetPrefix)
        {
            if (methodName.StartsWith("set_"))
                methodName = methodName.Replace("set_", "");
            else if (methodName.StartsWith("get_"))
                methodName = methodName.Replace("get_", "");
        }

        return methodName;
    }

    public static T? GetEnumerationAttribute<T>(object enumeration) where T : Attribute
    {
        var sourceType = enumeration.GetType();
        var fields     = sourceType.GetFields().Where(field => field.IsLiteral);

        foreach (var field in fields)
        {
            var enumValue = field.GetValue(null);
            if (Equals(enumValue, enumeration))
            {
                var a = (T[])field.GetCustomAttributes(typeof(T), false);

                if (a != null && a.Length > 0) return a[0];

                return null;
            }
        }

        return null;
    }

    public static string GetEnumerationDescription<TEnum>(TEnum enumValue) where TEnum : notnull
    {
        var a = GetEnumerationAttribute<DescriptionAttribute>(enumValue);
        if (a != null)
            return a.Description;

        return enumValue.ToString()!;
    }

    /// <summary>
    ///     Return all the properties of a given type of an object
    /// </summary>
    /// <typeparam name="T">Type of the property</typeparam>
    /// <param name="object">Object to look on</param>
    /// <param name="recursive">Browse the object tree recursively to find all the occurrences of the object</param>
    /// <param name="alreadyBrowsed">Object already browsed (in order to avoid an infinite loop)</param>
    /// <returns>List of objects of type T</returns>
    private static List<T> GetProperties<T>(object @object, bool recursive, ICollection<object> alreadyBrowsed)
    {
        var propertiesType = typeof(T);
        var properties     = new List<T>();

        if (!recursive)
        {
            foreach (var pi in @object.GetType().GetProperties())
                if (pi.PropertyType == propertiesType)
                    properties.Add((T)pi.GetValue(@object, null)!);
        }
        else
        {
            throw new NotImplementedException("Recursivity Not implemented " + alreadyBrowsed.Count);
        }

        return properties;
    }

    private static IEnumerable<PropertyInfo> GetPropertiesRecursive(MemberExpression memberExpression)
    {
        var propertyInfos = new List<PropertyInfo>();

        propertyInfos.Add((PropertyInfo)memberExpression.Member);

        if (memberExpression.Expression is MemberExpression)
            propertyInfos.AddRange(GetPropertiesRecursive((MemberExpression)memberExpression.Expression));

        return propertyInfos;
    }

    private static MemberExpression GetMemberExpression<T, TU>(Expression<Func<T, TU>> expression) => GetMemberExpression(expression, true);

    private static MemberExpression GetMemberExpression<T, TU>(Expression<Func<T, TU>> expression, bool enforceCheck)
    {
        MemberExpression? memberExpression = null;
        if (expression.Body.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.Body.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression.Body as MemberExpression;
        }

        if (enforceCheck && memberExpression == null) throw new ArgumentException("Not a member access", "expression");

        return memberExpression!;
    }
}
