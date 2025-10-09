// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IFormatExpectation
{
    Type InputType { get; }
    
    object? UnknownInput { get; }

    string ExpectedOutput { get; }

    string? FormatString { get; }

    bool HasDefault { get; }
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T Input { get; }
}

public record SingleSpanFormattableExpectation<T>
    (T Input, string ExpectedOutput, string? FormatString = null, bool HasDefault = false, T? DefaultValue = default)
    : ITypedFormatExpectation<T> where T : ISpanFormattable
{
    public object? UnknownInput => Input;
    public Type InputType => typeof(T);
};

public record SingleNullableSpanFormattableExpectation<T>
    (T? Input, string ExpectedOutput, string? FormatString = null, bool HasDefault = false, T? DefaultValue = null) : ITypedFormatExpectation<T?>
    where T : struct, ISpanFormattable
{
    public object? UnknownInput => Input;
    public Type InputType => typeof(T?);
}

public static class SpanFormattableTestData
{
    public static readonly SingleSpanFormattableExpectation<byte> ByteNoFormatString = new(128, "128");

    public static readonly SingleSpanFormattableExpectation<byte> ByteNo2DpFormatFormatString = new(128, "128.00", "C2");

    public static readonly SingleSpanFormattableExpectation<byte> ByteRightPadding20AsStringFormatString =
        new(128, "128.00", "\"{0,-20}\"");


    public static readonly IFormatExpectation[] AllSpanFormattableExpectations =
    [
        ByteNoFormatString
      , ByteNo2DpFormatFormatString
      , ByteRightPadding20AsStringFormatString
    ];


    public static IStringBearer CreateStringBearerWithValueFor<T>(this ITypedFormatExpectation<T> forTestData, ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.CreateStringBearerFunc(forTestData.InputType)();
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
        {
            isObjectMold.Value = forTestData.Input;
        }
        else
        {
            ((IMoldSupportedValue<T>)createdStringBearer).Value = forTestData.Input;    
        }
        if (forTestData.FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = forTestData.FormatString;
        }
        return createdStringBearer;
    }

    public static IStringBearer CreateStringBearerWithPossibleNullableValueFor<T>(this IFormatExpectation forTestData, ScaffoldingPartEntry scaffoldEntry, bool isNullable)
    {
        if(isNullable)
        {
            return CreateStringBearerWithValueFor((ITypedFormatExpectation<T?>)forTestData, scaffoldEntry);
        }
        return CreateStringBearerWithValueFor((ITypedFormatExpectation<T>)forTestData, scaffoldEntry);
    }


    public static IStringBearer CreateStringBearerWithValueFor(this IFormatExpectation forTestData, ScaffoldingPartEntry scaffoldEntry)
    {
        var input = forTestData.InputType.GetDefaultForUnderlyingNullableOrThis();
        var isNullable = forTestData.InputType.IsNullable();
        
        switch (input)
        {
            case bool : return CreateStringBearerWithPossibleNullableValueFor<bool>(forTestData, scaffoldEntry, isNullable);        
            case byte : return CreateStringBearerWithPossibleNullableValueFor<byte>(forTestData, scaffoldEntry, isNullable);
            case sbyte : return CreateStringBearerWithPossibleNullableValueFor<sbyte>(forTestData, scaffoldEntry, isNullable);       
            case char : return CreateStringBearerWithPossibleNullableValueFor<char>(forTestData, scaffoldEntry, isNullable);         
            case short : return CreateStringBearerWithPossibleNullableValueFor<short>(forTestData, scaffoldEntry, isNullable);       
            case ushort : return CreateStringBearerWithPossibleNullableValueFor<ushort>(forTestData, scaffoldEntry, isNullable);     
            case Half : return CreateStringBearerWithPossibleNullableValueFor<Half>(forTestData, scaffoldEntry, isNullable);    
            case int : return CreateStringBearerWithPossibleNullableValueFor<int>(forTestData, scaffoldEntry, isNullable);           
            case uint : return CreateStringBearerWithPossibleNullableValueFor<uint>(forTestData, scaffoldEntry, isNullable);         
            case nint : return CreateStringBearerWithPossibleNullableValueFor<nint>(forTestData, scaffoldEntry, isNullable);         
            case float : return CreateStringBearerWithPossibleNullableValueFor<float>(forTestData, scaffoldEntry, isNullable);       
            case long : return CreateStringBearerWithPossibleNullableValueFor<long>(forTestData, scaffoldEntry, isNullable);         
            case ulong : return CreateStringBearerWithPossibleNullableValueFor<ulong>(forTestData, scaffoldEntry, isNullable);       
            case double : return CreateStringBearerWithPossibleNullableValueFor<double>(forTestData, scaffoldEntry, isNullable);     
            case decimal : return CreateStringBearerWithPossibleNullableValueFor<decimal>(forTestData, scaffoldEntry, isNullable);   
            case Int128 : return CreateStringBearerWithPossibleNullableValueFor<Int128>(forTestData, scaffoldEntry, isNullable);     
            case UInt128 : return CreateStringBearerWithPossibleNullableValueFor<UInt128>(forTestData, scaffoldEntry, isNullable);   
            case BigInteger : return CreateStringBearerWithPossibleNullableValueFor<BigInteger>(forTestData, scaffoldEntry, isNullable); 
            case Complex : return CreateStringBearerWithPossibleNullableValueFor<Complex>(forTestData, scaffoldEntry, isNullable);    
            case DateTime : return CreateStringBearerWithPossibleNullableValueFor<DateTime>(forTestData, scaffoldEntry, isNullable); 
            case DateOnly : return CreateStringBearerWithPossibleNullableValueFor<DateOnly>(forTestData, scaffoldEntry, isNullable); 
            case TimeSpan : return CreateStringBearerWithPossibleNullableValueFor<TimeSpan>(forTestData, scaffoldEntry, isNullable); 
            case TimeOnly : return CreateStringBearerWithPossibleNullableValueFor<TimeOnly>(forTestData, scaffoldEntry, isNullable); 
            case Rune : return CreateStringBearerWithPossibleNullableValueFor<Rune>(forTestData, scaffoldEntry, isNullable);     
            case Guid : return CreateStringBearerWithPossibleNullableValueFor<Guid>(forTestData, scaffoldEntry, isNullable);         
            case IPNetwork : return CreateStringBearerWithPossibleNullableValueFor<IPNetwork>(forTestData, scaffoldEntry, isNullable);  
            case Enum : return CreateStringBearerWithPossibleNullableValueFor<Enum>(forTestData, scaffoldEntry, isNullable);         
            case Version : return CreateStringBearerWithPossibleNullableValueFor<Version>(forTestData, scaffoldEntry, isNullable);      
            case IPAddress : return CreateStringBearerWithPossibleNullableValueFor<IPAddress>(forTestData, scaffoldEntry, isNullable);  
            case Uri : return CreateStringBearerWithPossibleNullableValueFor<Uri>(forTestData, scaffoldEntry, isNullable);           
        }
        throw new ArgumentException("Unexpected SpanFormattable Type or non SpanFormattable Type - " + input?.GetType().FullName);
    }
    
}
