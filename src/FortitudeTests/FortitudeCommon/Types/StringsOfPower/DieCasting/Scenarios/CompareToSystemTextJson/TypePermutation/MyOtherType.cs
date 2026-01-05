// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class MyOtherTypeClass(string data)
{
    private readonly string data = data;

    public override  string ToString() => data;
    
    protected bool Equals(MyOtherTypeClass other) => data == other.data;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MyOtherTypeClass)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => data.GetHashCode();
}

public readonly struct MyOtherTypeStruct(string data) 
{
    private readonly string data = data;
    
    public override string ToString() => data;
    
    private bool Equals(MyOtherTypeStruct other) => data == other.data;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MyOtherTypeStruct)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => data.GetHashCode();
}
