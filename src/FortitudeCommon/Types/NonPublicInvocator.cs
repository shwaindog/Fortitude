#region

using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace FortitudeCommon.Types;

public class NonPublicInvocator
{
    private NonPublicInvocator() { }


    public static T RunStaticMethod<T>(string fullNamespaceTypeName, string strMethod, params object[] aobjParams)
    {
        var flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Static;
        return RunMethod<T>(Type.GetType(fullNamespaceTypeName)!, strMethod, aobjParams, flags);
    }

    public static T RunStaticMethod<T>(Type t, string strMethod, params object[] aobjParams)
    {
        var flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Static;
        return RunMethod<T>(t, strMethod, aobjParams, flags);
    }

    public static T RunInstanceMethod<T>(object objInstance, string strMethod, params object[] aobjParams)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        return RunMethod<T>(objInstance, strMethod, aobjParams, flags);
    }

    public static Action<T> GetInstanceMethodAction<T>(object objInstance, string strMethod)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        return GetInstanceMethodAction<T>(objInstance, strMethod, flags);
    }

    public static Func<T, TU, TV> GetInstanceMethodFunc<T, TU, TV>(object objInstance, string strMethod)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        return GetInstanceMethodFunc<T, TU, TV>(objInstance, strMethod, flags);
    }

    public static T RunInstanceMethodBaseVersion<T>(object objInstance, string strMethod,
        params object[] aobjParams)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        return RunMethodNoVirtual<T>(objInstance.GetType().BaseType!, objInstance, strMethod, aobjParams, flags);
    }

    public static void SetInstanceProperty<T>(object objInstance, string propertyName, T value,
        bool includePubWithNonPubSetters = false)
    {
        var flags = BindingFlags.NonPublic |
                    (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic) |
                    BindingFlags.Instance | BindingFlags.SetProperty
                    | BindingFlags.FlattenHierarchy;
        var t = objInstance.GetType();
        PropertyInfo? propInfo = null;

        while (t != null && t != typeof(object) && (propInfo == null || !propInfo.CanWrite))
        {
            propInfo = t.GetProperty(propertyName, flags);
            t = t.BaseType;
        }

        if (propInfo == null || !propInfo.CanWrite)
            throw new ApplicationException(
                $"Requested property '{propertyName}' could not be found on {objInstance.GetType().Name}");
        propInfo.SetValue(objInstance, value, null);
    }


    public static void SetStaticProperty<T>(Type? t, string propertyName, T value,
        bool includePubWithNonPubSetters = false)
    {
        var flags = BindingFlags.NonPublic |
                    (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic) |
                    BindingFlags.Instance | BindingFlags.SetProperty
                    | BindingFlags.FlattenHierarchy;
        PropertyInfo? propInfo = null;

        while (t != null && t != typeof(object) && (propInfo == null || !propInfo.CanWrite))
        {
            propInfo = t.GetProperty(propertyName, flags);
            t = t.BaseType;
        }

        if (propInfo == null || !propInfo.CanWrite)
        {
            Debug.Assert(t != null, "t != null");
            throw new ApplicationException($"Requested property '{propertyName}' could not be found on {t.Name}");
        }

        propInfo.SetValue(null, value, null);
    }

    public static object? GetInstanceProperty(object objInstance, string propertyName) =>
        GetInstanceProperty(objInstance.GetType(), objInstance, propertyName);

    public static object? GetInstanceProperty(Type t, object objInstance, string propertyName)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty;
        var propInfo = t.GetProperty(propertyName, flags);
        if (propInfo == null)
        {
            if (t != typeof(object)) return GetInstanceProperty(t.BaseType!, objInstance, propertyName);
        }
        else if (propInfo.CanRead)
        {
            return propInfo.GetValue(objInstance, null);
        }

        throw new NotImplementedException("Property does not exist or support get value.");
    }

    public static T GetInstanceField<T>(object objInstance, string fieldName) =>
        GetInstanceField<T>(objInstance.GetType(), objInstance, fieldName)!;

    public static T? GetInstanceField<T>(Type t, object objInstance, string fieldName)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
        var fieldInfo = t.GetField(fieldName, flags);
        if (fieldInfo == null)
        {
            if (t != typeof(object)) return GetInstanceField<T>(t.BaseType!, objInstance, fieldName);
        }
        else
        {
            return (T?)fieldInfo.GetValue(objInstance);
        }

        throw new NotImplementedException("Field does not exist.");
    }

    public static Func<Tc, Tv> GetInstanceFieldExtractor<Tc, Tv>(Type t, string fieldName)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
        var fieldInfo = t.GetField(fieldName, flags);
        return fieldInfo == null ?
            throw new ArgumentException($"Field {fieldName} cannot be found in {t.FullName}") :
            instance => (Tv)fieldInfo.GetValue(instance)!;
    }

    public static void SetAutoPropertyInstanceField<T, U>(object objInstance, Expression<Func<T, U>> expression,
        U value)
    {
        var propertyName = expression.GetPropertyName();
        SetInstanceField(objInstance, $"<{propertyName}>k__BackingField", value);
    }

    public static void SetInstanceField<T>(object objInstance, string fieldName, T value,
        bool includePubWithNonPubSetters = false)
    {
        var flags
            = BindingFlags.NonPublic | (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic)
                                     | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy;
        var t = objInstance.GetType();
        FieldInfo? fieldInfo = null;

        while (t != null && t != typeof(object) && fieldInfo == null)
        {
            fieldInfo = t.GetField(fieldName, flags);
            t = t.BaseType;
        }

        if (fieldInfo == null)
            throw new ApplicationException(
                $"Requested field '{fieldName}' could not be found on {objInstance.GetType().Name}");
        fieldInfo.SetValue(objInstance, value);
    }

    public static FieldInfo SetInstanceFieldFieldInfo<TInstance>(TInstance objInstance, string fieldName,
        bool includePubWithNonPubSetters = false)
    {
        var flags
            = BindingFlags.NonPublic | (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic)
                                     | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy;
        var t = typeof(TInstance);
        FieldInfo? fieldInfo = null;

        while (t != null && t != typeof(object) && fieldInfo == null)
        {
            fieldInfo = t.GetField(fieldName, flags);
            t = t.BaseType;
        }

        if (fieldInfo == null)
            throw new ApplicationException(
                $"Requested field '{fieldName}' could not be found on {objInstance.GetType().Name}");

        return fieldInfo;
    }

    public static void SetStaticField<T>(Type type, string fieldName, T value,
        bool includePubWithNonPubSetters = false)
    {
        var flags = BindingFlags.NonPublic |
                    (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic) | BindingFlags.Static |
                    BindingFlags.SetProperty
                    | BindingFlags.FlattenHierarchy;
        var t = type;
        FieldInfo? fieldInfo = null;

        while (t != null && t != typeof(object) && fieldInfo == null)
        {
            fieldInfo = t.GetField(fieldName, flags);
            t = t.BaseType;
        }

        if (fieldInfo == null)
            throw new ApplicationException($"Requested field '{fieldName}' could not be found on {type.Name}");
        fieldInfo.SetValue(null, value, BindingFlags.SetField, Type.DefaultBinder, null);
    }

    public static void SetStaticField<T>(object objInstance, string fieldName, T value,
        bool includePubWithNonPubSetters = false)
    {
        SetStaticField(objInstance.GetType(), fieldName, value, includePubWithNonPubSetters);
    }

    public static object? GetStaticProperty(string fullTypeName, string propertyName)
    {
        var t = Type.GetType(fullTypeName)!;
        return GetStaticProperty(t, propertyName);
    }

    public static object? GetStaticProperty(Type t, string propertyName, bool includePubWithNonPubSetters = false)
    {
        var flags = (includePubWithNonPubSetters ? BindingFlags.Public : BindingFlags.NonPublic)
                    | BindingFlags.Static | BindingFlags.GetProperty;
        var propInfo = t.GetProperty(propertyName, flags);
        Debug.Assert(propInfo != null, "propInfo != null");
        return propInfo.CanRead ? propInfo.GetValue(t, null) : null;
    }

    public static object? GetStaticPublicField(string fullTypeName, string fieldName)
    {
        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
        var t = Type.GetType(fullTypeName);
        var propInfo = t?.GetField(fieldName, flags);
        Debug.Assert(propInfo != null, "propInfo != null");
        return propInfo.GetValue(t);
    }

    public static object GetInstance(Type t, params object[] constructorParams)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var types = new Type[constructorParams.Length];
        for (var i = 0; i < constructorParams.Length; i++) types[i] = constructorParams[i].GetType();
        var ci = t.GetConstructor(flags, null, types, null);
        if (ci != null)
        {
            var instance = ci.Invoke(constructorParams);
            return instance;
        }

        var typesArrayAsString = string.Empty;
        foreach (var type in types)
        {
            if (typesArrayAsString.Length > 1) typesArrayAsString += ", ";
            typesArrayAsString += type.FullName;
        }

        var foundConstructors = string.Empty;
        foreach (var constructorInfo in t.GetConstructors(flags))
        {
            if (typesArrayAsString.Length > 1) typesArrayAsString += ", ";
            foundConstructors += t.Name + "(";
            foreach (var parameterInfo in constructorInfo.GetParameters())
                foundConstructors += parameterInfo.ParameterType + " " + parameterInfo.Name + ", ";
            foundConstructors += ")\n";
        }

        throw new NotImplementedException("No such constructor can be found for " + t.FullName +
                                          " taking parameters " + typesArrayAsString + "\n" +
                                          "Found constructors for type are,\n" + foundConstructors);
    }

    public static object GetInstance(string fullyQualifiedTypeName, params object[] constructorParams)
    {
        var t = Type.GetType(fullyQualifiedTypeName)!;

        return GetInstance(t, constructorParams);
    }

    public static T GetInstance<T>(params object[] constructorParams) => (T)GetInstance(typeof(T), constructorParams);

    private static T RunMethod<T>(object objInstance, string strMethod, object[] aobjParams, BindingFlags flags)
    {
        Type t;
        if (objInstance != null && !(objInstance is Type))
            t = objInstance.GetType();
        else if (objInstance != null)
            t = (Type)objInstance;
        else
            throw new ArgumentException("Expected objectInstance to be the instance to invoke or the System.Type");
        return RunMethod<T>(t, objInstance, strMethod, aobjParams, flags);
    }

    private static Action<T> GetInstanceMethodAction<T>(object objInstance, string strMethod, BindingFlags flags)
    {
        Type t;
        if (objInstance != null && !(objInstance is Type))
            t = objInstance.GetType();
        else if (objInstance != null)
            t = (Type)objInstance;
        else
            throw new ArgumentException("Expected objectInstance to be the instance to invoke or the System.Type");
        return GetInstanceMethodAction<T>(t, objInstance, strMethod, flags);
    }

    private static Func<T, TU, TV> GetInstanceMethodFunc<T, TU, TV>(object objInstance, string strMethod
        , BindingFlags flags)
    {
        Type t;
        if (objInstance != null && !(objInstance is Type))
            t = objInstance.GetType();
        else if (objInstance != null)
            t = (Type)objInstance;
        else
            throw new ArgumentException("Expected objectInstance to be the instance to invoke or the System.Type");
        return GetInstanceMethodFunc<T, TU, TV>(t, objInstance, strMethod, flags);
    }

    private static T RunMethod<T>(Type t, object? objInstance, string strMethod, object[] objParams,
        BindingFlags flags)
    {
        if (objInstance == null) objInstance = t;
        var paramTypes = new Type[objParams.Length];
        for (var i = 0; i < objParams.Length; i++) paramTypes[i] = objParams[i].GetType();
        MethodInfo? m = null;
        var currentType = t;
        while (currentType != null && currentType != typeof(object) && m == null)
        {
            m = currentType.GetMethod(strMethod, flags, null, paramTypes, null);
            currentType = currentType.BaseType;
        }

        if (m == null)
            throw new ArgumentException("There is no method '" + strMethod + "' for type '" + currentType +
                                        "' or its base methods.");

        var objRet = m.Invoke(objInstance, objParams)!;
        return (T)objRet;
    }

    private static Action<T> GetInstanceMethodAction<T>(Type t, object? objInstance, string strMethod,
        BindingFlags flags)
    {
        if (objInstance == null) objInstance = t;
        var paramTypes = new Type[1];
        paramTypes[0] = typeof(T);
        MethodInfo? m = null;
        var currentType = t;
        while (currentType != null && currentType != typeof(object) && m == null)
        {
            m = currentType.GetMethod(strMethod, flags, null, paramTypes, null);
            currentType = currentType.BaseType;
        }

        if (m == null)
            throw new ArgumentException("There is no method '" + strMethod + "' for type '" + currentType +
                                        "' or its base methods.");

        var callBackDelegate = m.CreateDelegate<Action<T>>(objInstance);
        return callBackDelegate;
    }

    private static Func<T, TU, TV> GetInstanceMethodFunc<T, TU, TV>(Type t, object? objInstance, string strMethod,
        BindingFlags flags)
    {
        if (objInstance == null) objInstance = t;
        var paramTypes = new Type[1];
        paramTypes[0] = typeof(T);
        MethodInfo? m = null;
        var currentType = t;
        while (currentType != null && currentType != typeof(object) && m == null)
        {
            m = currentType.GetMethod(strMethod, flags, null, paramTypes, null);
            currentType = currentType.BaseType;
        }

        if (m == null)
            throw new ArgumentException("There is no method '" + strMethod + "' for type '" + currentType +
                                        "' or its base methods.");

        var callBackDelegate = m.CreateDelegate<Func<T, TU, TV>>(objInstance);
        return callBackDelegate;
    }

    private static T RunMethodNoVirtual<T>(Type t, object? objInstance, string strMethod, object[] aobjParams,
        BindingFlags flags)
    {
        if (objInstance == null) objInstance = t;

        var m = t.GetMethod(strMethod, flags);
        if (m == null) throw new ArgumentException("There is no method '" + strMethod + "' for type '" + t + "'.");

        var paramsAddedInstance = new object[aobjParams.Length + 1];
        paramsAddedInstance[0] = objInstance;
        for (var i = 0; i < aobjParams.Length; i++) paramsAddedInstance[i + 1] = aobjParams[i];

        var objRet = InvokeNonVirtual(m, paramsAddedInstance)!;
        return (T)objRet;
    }

    /// <summary>
    ///     Call a virtual method non-virtually - like Reflection's MethodInfo.Invoke,
    ///     but doesn't do virtual dispatch.
    ///     http://blogs.msdn.com/rmbyers/archive/2008/08/16/invoking-a-virtual-method-non-virtually.aspx
    /// </summary>
    /// <param name="method">The method to invoke</param>
    /// <param name="args">The arguments to pass (including 'this')</param>
    /// <returns>The return value from the call</returns>
    private static object? InvokeNonVirtual(MethodInfo method, object[] args)
    {
        // Reflection doesn't seem to have a way directly (eg. custom binders are 
        // only used for ambiguities).  Using a delegate also always seems to do 
        // virtual dispatch.

        // Use LCG to generate a temporary method that uses a 'call' instruction to
        // invoke the supplied method non-virtually.
        // Doing a non-virtual call on a virtual method outside the class that 
        // defines it will normally generate a VerificationException (PEVerify 
        // says "The 'this' parameter to the call must be the callng method's 
        // 'this' parameter.").  By associating the method with a type ("Program") 
        // in a full-trust assembly, we tell the JIT to skip this verification step.
        // Alternately we might want to associate it with method.DeclaringType - the
        // verification might then pass even if it's not skipped (eg. partial trust).
        var paramTypes = new List<Type>();
        if (!method.IsStatic)
            paramTypes.Add(method.DeclaringType!);
        paramTypes.AddRange(method.GetParameters().Select(p => p.ParameterType));
        Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
        var dm = new DynamicMethod(
            "NonVirtualInvoker", // name
            method.ReturnType, // same return type as method we're calling 
            paramTypes.ToArray(), // same parameter types as method we're calling
            method.DeclaringType); // associates with this full-trust code
        var il = dm.GetILGenerator();
        for (var i = 0; i < paramTypes.Count; i++)
            il.Emit(OpCodes.Ldarg, i); // load all args
        il.EmitCall(OpCodes.Call, method, null); // call the method non-virtually
        il.Emit(OpCodes.Ret); // return what the call returned

        // Call the emitted method, which in turn will call the method requested
        return dm.Invoke(null, args);
    }
}
