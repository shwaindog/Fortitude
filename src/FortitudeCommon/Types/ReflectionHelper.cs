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
    public static Func<TParam, TClass> CtorBinder<TParam, TClass>()
    {
        var ctorParam = Expression.Parameter(typeof(TParam), "Param");
        return Expression.Lambda<Func<TParam, TClass>>(
            Expression.New(typeof(TClass).GetConstructor(new[] { typeof(TParam) })!, ctorParam), ctorParam).Compile();
    }

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="object"></param>
    /// <param name="memberName"></param>
    /// <param name="ignoreComposedMemberNull">
    ///     Don't throw an exception if an intermediate is <see langword="null" />. For
    ///     example, if looking for "Statistics.Decile.Value", Decile is <see langword="null" />, then the method will return
    ///     <see langword="null" />.
    /// </param>
    /// <returns></returns>
    public static T? GetProperty<T>(object? @object, string memberName, bool ignoreComposedMemberNull)
    {
        if (@object == null)
            return default;

        var properties = memberName.Split('.');

        var current = @object;

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

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="object"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static T? GetProperty<T>(object @object, string memberName) => GetProperty<T>(@object, memberName, false);

    /// <summary>
    ///     Gets a member from an object
    ///     The member name can be composed of other member name (such as "Statistics.Decile.Value")
    /// </summary>
    /// <param name="object"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty(object @object, string memberName)
    {
        var properties = memberName.Split('.');

        PropertyInfo? currentPropertyInfo = null;
        var current = @object;
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
                        currentValue = pi.GetValue(current, null);
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
    /// <param name="object"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static T? GetProperty<T>(object @object, PropertyInfo property) => (T?)property.GetValue(@object, null);

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
        var fields = new List<T>();

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
    public static T? Instantiate<T>(Type typeToInstantiate) =>
        (T?)typeToInstantiate.Assembly.CreateInstance(typeToInstantiate.FullName!);


    /// <summary>
    ///     Instantiate an object of a generic Type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterlessGenericType"></param>
    /// <returns></returns>
    public static object InstantiateGenericType(Type parameterlessGenericType,
        Type[] genericArgs, params object[] constructorArgs)
    {
        var fullConfiguredType = parameterlessGenericType.MakeGenericType(genericArgs);
        return Activator.CreateInstance(fullConfiguredType, BindingFlags.CreateInstance,
            null, constructorArgs, CultureInfo.CurrentCulture)!;
    }

    public static Delegate CreateEmptyConstructorFactoryAsFuncType(Type withParameterlessConstructor)
    {
        var typeConstant = Expression.Constant(withParameterlessConstructor);
        var constructor = withParameterlessConstructor.GetConstructor(Array.Empty<Type>());
        var callConstructor = Expression.New(constructor);
        return Expression.Lambda(callConstructor).Compile();
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
    public static List<T> GetProperties<T>(object @object, bool recursive) =>
        GetProperties<T>(@object, recursive, new List<object>());

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
        var trace = new StackTrace();
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
        var fields = sourceType.GetFields().Where(field => field.IsLiteral);

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
        var properties = new List<T>();

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

    private static MemberExpression GetMemberExpression<T, TU>(Expression<Func<T, TU>> expression) =>
        GetMemberExpression(expression, true);

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
