// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;

namespace FortitudeCommon.Types;

public interface IPropertyChanged<in T>
{
    Type OwnerType { get; }

    string PropertyName { get; }

    bool IsPropertyDifferent(T prev, T updated);
}

public interface IPropertyExtractor<in T, out TProp> : IPropertyChanged<T>
{
    Type PropertyType { get; }

    TProp? GetPropertyValue(T? input);
}

public class PropertyExtractor<T, TProp>(Expression<Func<T?, TProp>> propertyGetExpression) : IPropertyExtractor<T, TProp>
{
    private readonly Func<T?, TProp> propertyExtractFunc = propertyGetExpression.Compile();

    public Type OwnerType => typeof(T);

    public string PropertyName { get; } = propertyGetExpression.GetPropertyName();

    public Type PropertyType => typeof(TProp);

    public TProp? GetPropertyValue(T? input) => propertyExtractFunc(input);

    public virtual bool IsPropertyDifferent(T prev, T updated)
    {
        return !Equals(GetPropertyValue(prev), GetPropertyValue(updated));
    }

    protected bool Equals(PropertyExtractor<T, TProp> other) => OwnerType == other.OwnerType && PropertyName == other.PropertyName;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PropertyExtractor<T, TProp>)obj);
    }

    public override int GetHashCode() => OwnerType.GetHashCode() ^ PropertyName.GetHashCode();
}

public interface IPropertySetter<in T, in TProp> : IPropertyChanged<T>
{
    Type PropertyType { get; }

    void SetPropertyValue(T? input, TProp newValue);
}

public class PropertyAccessor<T, TProp>(Expression<Func<T?, TProp>> propertyGetExpression) 
    : PropertyExtractor<T, TProp>(propertyGetExpression), IPropertySetter<T, TProp>
{
    private readonly Action<T, TProp> propertySetterAction = propertyGetExpression.GetProperty().GetSetMethod()!.CreateDelegate<Action<T, TProp>>();

    public void SetPropertyValue(T? input, TProp newValue)
    {
        if (input != null) propertySetterAction(input, newValue);
    }
}
