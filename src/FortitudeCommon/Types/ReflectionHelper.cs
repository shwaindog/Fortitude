using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FortitudeCommon.Types
{
    /// <summary>
    /// Provide methods used in reflection
    /// </summary>
    public static class ReflectionHelper
    {
        public static Func<TParam, TClass> CtorBinder<TParam, TClass>()
        {
            ParameterExpression ctorParam = Expression.Parameter(typeof(TParam), "Param");
            return Expression.Lambda<Func<TParam, TClass>>(
                Expression.New(typeof(TClass).GetConstructor(new Type[] { typeof(TParam) }),
                    new Expression[] { ctorParam }),
                new ParameterExpression[] { ctorParam }).Compile();
        }
        /// <summary>
        /// Gets a member from an object
        /// The member name can be composed of other member name (such as "Statistics.Decile.Value")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        /// <param name="ignoreComposedMemberNull">Don't throw an exception if an intermediate is <see langword="null"/>. For example, if looking for "Statistics.Decile.Value", Decile is <see langword="null"/>, then the method will return <see langword="null"/>.</param>
        /// <returns></returns>
        public static T GetProperty<T>(object @object, string memberName, bool ignoreComposedMemberNull)
        {
            if (@object == null)
                return default(T);

            string[] properties = memberName.Split('.');

            object current = @object;

            for (int i = 0; i < properties.Length; i++)
            {
                string prop = properties[i];
                object newCurrent = null;

                if (current == null)
                    if (ignoreComposedMemberNull)
                        return default(T);
                    else
                        throw new NullReferenceException(memberName + " -> " + prop);

                try
                {
                    PropertyInfo propertyInfo = current.GetType().GetProperty(prop);

                    if (propertyInfo == null)
                        throw new NotSupportedException(memberName + " -> " + prop);

                    newCurrent = propertyInfo.GetValue(current, null);
                }
                catch (AmbiguousMatchException)
                {
                    foreach (PropertyInfo pi in current.GetType().GetProperties())
                    {
                        if (pi.Name == prop)
                        {
                            newCurrent = pi.GetValue(current, null);
                            break;
                        }
                    }
                }

                if (newCurrent == null && i < properties.Length - 1)
                    if (ignoreComposedMemberNull)
                        return default(T);
                    else
                        throw new NullReferenceException(memberName + " -> " + prop);

                current = newCurrent;
            }

            if (typeof(T) == typeof(string) && current != null)
                return (T)((object)current.ToString());

            return (T)current;
        }


        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Gets a member from an object
        /// The member name can be composed of other member name (such as "Statistics.Decile.Value")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static T GetProperty<T>(object @object, string memberName)
        {
            return GetProperty<T>(@object, memberName, false);
        }

        /// <summary>
        /// Gets a member from an object
        /// The member name can be composed of other member name (such as "Statistics.Decile.Value")
        /// </summary>
        /// <param name="object"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(object @object, string memberName)
        {
            string[] properties = memberName.Split('.');

            PropertyInfo currentPropertyInfo = null;
            object current = @object;
            for (int i = 0; i < properties.Length; i++)
            {
                string prop = properties[i];
                object currentValue = null;

                if (current == null)
                    throw new NullReferenceException(memberName + " -> " + prop);

                try
                {
                    currentPropertyInfo = current.GetType().GetProperty(prop);

                    if (currentPropertyInfo == null)
                        return null;

                    currentValue = current.GetType().GetProperty(prop).GetValue(current, null);
                }
                catch (AmbiguousMatchException)
                {
                    foreach (PropertyInfo pi in current.GetType().GetProperties())
                    {
                        if (pi.Name == prop)
                        {
                            currentValue = pi.GetValue(current, null);
                            currentPropertyInfo = pi;
                            break;
                        }
                    }
                }

                if (currentValue == null && i < properties.Length - 1)
                    throw new NullReferenceException(memberName + " -> " + prop);

                current = currentValue;
            }

            return currentPropertyInfo;
        }

        /// <summary>
        /// Gets the value of a property of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static T GetProperty<T>(object @object, PropertyInfo property)
        {
            return (T)property.GetValue(@object, null);
        }

        /// <summary>
        /// Return all the properties of a given type of an object 
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="object">Object to look on</param>
        /// <returns>List of objects of type T</returns>
        public static List<T> GetProperties<T>(object @object)
        {
            return GetProperties<T>(@object, false);
        }

        /// <summary>
        /// Return all the fields of a given type of an object
        /// </summary>
        /// <typeparam name="T">Type of the field</typeparam>
        /// <param name="object">Object to look on</param>
        /// <returns>List of objects of type T</returns>
        public static List<T> GetFields<T>(object @object)
        {
            Type fieldsType = typeof(T);
            var fields = new List<T>();

            if (@object == null)
                return fields;

            foreach (FieldInfo pi in @object.GetType().GetFields())
            {
                if (pi.FieldType == fieldsType)
                    fields.Add((T)pi.GetValue(@object));
            }

            return fields;
        }

        /// <summary>
        /// Instantiate an object of the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeToInstantiate"></param>
        /// <returns></returns>
        public static T Instantiate<T>(Type typeToInstantiate)
        {
            return (T)typeToInstantiate.Assembly.CreateInstance(typeToInstantiate.FullName);
        }

        /// <summary>
        /// Instantiate an object of the given type
        /// </summary>
        /// <param name="typeToInstantiate"></param>
        /// <returns></returns>
        public static object Instantiate(Type typeToInstantiate)
        {
            return Instantiate<object>(typeToInstantiate);
        }

        /// <summary>
        /// Get a <see cref="PropertyInfo"/> object from a lambda expression representing the member to return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression);
            return (PropertyInfo)memberExpression.Member;
        }

        public static string GetPropertyName<T, TProp>(this Expression<Func<T, TProp>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression);
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Get a string representation of the lambda expression.
        /// Example: a lambda of o => o.Item.SubItem will return a string of "Item.SubItem"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetCombinedProperty<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            IEnumerable<PropertyInfo> properties = GetProperties(expression);
            properties = properties.Reverse();

            string combinedProperty = "";

            foreach (PropertyInfo propertyInfo in properties)
            {
                combinedProperty += "." + propertyInfo.Name;
            }

            return combinedProperty.Substring(1, combinedProperty.Length - 1);
        }

        /// <summary>
        /// Get a collection of <see cref="PropertyInfo"/> objects from a lambda expression representing the members to return.
        /// Example: if the lambda expression is item.SubItem.SubSubItem, this will return a <see cref="PropertyInfo"/> for SubItem, and another one for SubSubItem.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            MemberExpression memberExpression = GetMemberExpression(expression);
            return GetPropertiesRecursive(memberExpression);
        }

        /// <summary>
        /// Return all the properties of a given type of an object 
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="object">Object to look on</param>
        /// <param name="recursive">Browse the object tree recursively to find all the occurrences of the object</param>
        /// <returns>List of objects of type T</returns>
        public static List<T> GetProperties<T>(object @object, bool recursive)
        {
            return GetProperties<T>(@object, recursive, new List<object>());
        }

        /// <summary>
        /// Get the name of the method which called this method
        /// </summary>
        /// <param name="frameIndex">0 gives the name of this method, 1 its parent, 2 parent of parent etc.</param>
        /// <param name="removeGetSetPrefix">If the calling method is a getter or a setter, setting this to true will remove the get_ set_ prefix</param>
        /// <returns></returns>
        public static string GetCallingMethodName(int frameIndex, bool removeGetSetPrefix)
        {
            StackTrace trace = new StackTrace();
            string methodName = trace.GetFrame(frameIndex).GetMethod().Name;

            if (removeGetSetPrefix)
            {
                if (methodName.StartsWith("set_"))
                    methodName = methodName.Replace("set_", "");
                else if (methodName.StartsWith("get_"))
                    methodName = methodName.Replace("get_", "");
            }

            return methodName;
        }

        public static T GetEnumerationAttribute<T>(object enumeration) where T : Attribute
        {
            Type sourceType = enumeration.GetType();
            IEnumerable<FieldInfo> fields = sourceType.GetFields().Where(field => field.IsLiteral);

            foreach (FieldInfo field in fields)
            {
                object enumValue = field.GetValue(null);
                if (Equals(enumValue, enumeration))
                {
                    var a = (T[])field.GetCustomAttributes(typeof(T), false);

                    if (a != null && a.Length > 0)
                    {
                        return a[0];
                    }

                    return null;
                }
            }

            return null;
        }

        public static string GetEnumerationDescription<TEnum>(TEnum enumValue)
        {
            var a = GetEnumerationAttribute<DescriptionAttribute>(enumValue);
            if (a != null)
                return a.Description;

            return enumValue.ToString();
        }

        /// <summary>
        /// Return all the properties of a given type of an object 
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="object">Object to look on</param>
        /// <param name="recursive">Browse the object tree recursively to find all the occurrences of the object</param>
        /// <param name="alreadyBrowsed">Object already browsed (in order to avoid an infinite loop)</param>
        /// <returns>List of objects of type T</returns>
        private static List<T> GetProperties<T>(object @object, bool recursive, ICollection<object> alreadyBrowsed)
        {
            Type propertiesType = typeof(T);
            var properties = new List<T>();

            if (@object == null)
                return properties;

            if (!recursive)
            {
                foreach (PropertyInfo pi in @object.GetType().GetProperties())
                {
                    if (pi.PropertyType == propertiesType)
                        properties.Add((T)pi.GetValue(@object, null));
                }
            }
            else
            {
                throw new NotImplementedException("Recursivity Not implemented " + alreadyBrowsed.Count);
            }

            return properties;
        }

        private static IEnumerable<PropertyInfo> GetPropertiesRecursive(MemberExpression memberExpression)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();

            propertyInfos.Add((PropertyInfo)memberExpression.Member);

            if (memberExpression.Expression is MemberExpression)
            {
                propertyInfos.AddRange(GetPropertiesRecursive((MemberExpression)memberExpression.Expression));
            }

            return propertyInfos;
        }

        private static MemberExpression GetMemberExpression<T, TU>(Expression<Func<T, TU>> expression)
        {
            return GetMemberExpression(expression, true);
        }

        private static MemberExpression GetMemberExpression<T, TU>(Expression<Func<T, TU>> expression, bool enforceCheck)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (enforceCheck && memberExpression == null)
            {
                throw new ArgumentException("Not a member access", "expression");
            }

            return memberExpression;
        }
    }
}
