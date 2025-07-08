using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Types.StyledTypeStringAppender;

namespace FortitudeCommon.Types;

public enum StringBuildingStyle
{
    None = 0
  , Default
  , Log
  , JsonCompact
  , JsonPretty
}

public static class StringBuildingStyleExtensions
{
    // ReSharper disable UnusedMember.Global
    public static bool IsDefault(this StringBuildingStyle style) => style == StringBuildingStyle.Default;

    public static bool IsDefaultOrLog(this StringBuildingStyle style) => style is StringBuildingStyle.Default or StringBuildingStyle.Log;

    public static bool IsDefaultOrJsonCompact(this StringBuildingStyle style) =>
        style is StringBuildingStyle.Default or StringBuildingStyle.JsonCompact;

    public static bool IsDefaultOrJsonPretty(this StringBuildingStyle style) =>
        style is StringBuildingStyle.Default or StringBuildingStyle.JsonPretty;

    public static bool IsLog(this StringBuildingStyle style) => style == StringBuildingStyle.Log;

    public static bool IsJsonCompact(this StringBuildingStyle style) => style == StringBuildingStyle.JsonCompact;

    public static bool IsJsonPretty(this StringBuildingStyle style) => style == StringBuildingStyle.JsonPretty;
}

public interface IStyledTypeStringAppender : IReusableObject<IStyledTypeStringAppender>
{
    // ReSharper disable UnusedMemberInSuper.Global
    StringBuildingStyle Style { get; }

    int IndentLevel { get; }

    void ClearSetStyle(StringBuildingStyle stringStyle, int indentLevel = 0);

    IStyledTypeStringAppender AddTypeName(string value);
    IStyledTypeStringAppender AddTypeStart();
    IStyledTypeStringAppender AddTypeEnd();


    #region Add Single Fields

    #region Single Fields Always Add

    IStyledTypeStringAppender AddField(string fieldName, IMutableString value);
    IStyledTypeStringAppender AddField(string fieldName, IStyledToStringObject value);
    IStyledTypeStringAppender AddField(string fieldName, bool? value);
    IStyledTypeStringAppender AddField(string fieldName, sbyte? value);
    IStyledTypeStringAppender AddField(string fieldName, byte? value);
    IStyledTypeStringAppender AddField(string fieldName, char? value);
    IStyledTypeStringAppender AddField(string fieldName, short? value);
    IStyledTypeStringAppender AddField(string fieldName, ushort? value);
    IStyledTypeStringAppender AddField(string fieldName, int? value);
    IStyledTypeStringAppender AddField(string fieldName, uint? value);
    IStyledTypeStringAppender AddField(string fieldName, float? value);
    IStyledTypeStringAppender AddField(string fieldName, ulong? value);
    IStyledTypeStringAppender AddField(string fieldName, long? value);
    IStyledTypeStringAppender AddField(string fieldName, double? value);
    IStyledTypeStringAppender AddField(string fieldName, decimal? value);

    IStyledTypeStringAppender AddField<T>(string fieldName, T? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddField(string fieldName, string? value);
    IStyledTypeStringAppender AddField(string fieldName, string? value, int startIndex, int length);

    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddField(string fieldName, object? value);

    #endregion Single Fields Always Add

    #region Single Fields Condition Non Default

    IStyledTypeStringAppender AddNonDefaultField(string fieldName, bool value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, sbyte value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, byte value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, char value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, short value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, ushort value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, int value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, uint value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, float value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, long value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, ulong value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, double value);
    IStyledTypeStringAppender AddNonDefaultField(string fieldName, decimal value);
    IStyledTypeStringAppender AddNonDefaultField<T>(string fieldName, T value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    #endregion Single Fields Condition Non Default

    #region Single Fields Condition Non Null

    IStyledTypeStringAppender AddNonNullField(string fieldName, bool? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, sbyte? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, byte? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, char? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, short? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, ushort? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, int? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, uint? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, float? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, long? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, ulong? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, double? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, decimal? value);
    IStyledTypeStringAppender AddNonNullField<T>(string fieldName, T? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullOrEmptyField(string fieldName, string? value);
    IStyledTypeStringAppender AddNonNullOrEmptyField(string fieldName, string? value, int startIndex, int length);
    IStyledTypeStringAppender AddNonNullOrEmptyField(string fieldName, IMutableString? value);
    IStyledTypeStringAppender AddNonNullField(string fieldName, IStyledToStringObject? value);

    #endregion Single Fields Condition Non Null

    #endregion Add Single Fields


    #region Add Collection Fields

    #region Collection Always Add

    IStyledTypeStringAppender AddCollectionField(string fieldName, bool[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, bool?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, sbyte[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, sbyte?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, byte[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, byte?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, char[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, char?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, short[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, short?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, ushort[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, ushort?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, int[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, int?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, uint[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, uint?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, float[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, float?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, ulong[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, ulong?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, long[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, long?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, double[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, double?[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, decimal[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, decimal?[]? value);

    IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddCollectionField(string fieldName, string[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IStyledToStringObject[]? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IMutableString[]? value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddCollectionField(string fieldName, object?[]? value);

    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<bool>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<bool?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<sbyte>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<sbyte?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<byte>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<byte?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<short>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<short?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ushort>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ushort?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<int>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<int?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<uint>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<uint?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<float>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<float?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ulong>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ulong?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<long>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<long?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<double>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<double?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<decimal>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<decimal?>? value);

    IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<string?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char[]>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<IMutableString?>? value);
    IStyledTypeStringAppender AddCollectionField(string fieldName, IEnumerable<IStyledToStringObject?>? value);


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<object?> value);

    #endregion Collection Always Add


    #region Collection Condition Non Null

    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, bool[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, bool?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, sbyte[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, sbyte?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, byte[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, byte?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, char[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, char?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, short[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, short?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ushort[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ushort?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, int[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, int?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, uint[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, uint?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, float[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, float?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ulong[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ulong?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, long[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, long?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, double[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, double?[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, decimal[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, decimal?[]? value);

    IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, string[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IStyledToStringObject[]? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IMutableString[]? value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, object?[]? value);

    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<bool>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<bool?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<sbyte>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<sbyte?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<byte>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<byte?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<short>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<short?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ushort>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ushort?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<int>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<int?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<uint>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<uint?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<float>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<float?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ulong>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ulong?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<long>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<long?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<double>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<double?>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<decimal>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<decimal?>? value);

    IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<string>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char[]>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<IMutableString>? value);
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IEnumerable<IStyledToStringObject>? value);
    

    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<object?> value);

    #endregion Collection Condition Non Null

    #region Collection Condition Populated

    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, bool[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, bool?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, sbyte[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, sbyte?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, byte[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, byte?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, char[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, char?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, short[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, short?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ushort[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ushort?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, int[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, int?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, uint[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, uint?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, float[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, float?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ulong[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ulong?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, long[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, long?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, double[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, double?[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, decimal[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, decimal?[] value);

    IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, T[] value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, T?[] value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, string[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IStyledToStringObject[] value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IMutableString[] value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, object?[] value);

    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<bool> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<bool?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<byte> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<byte?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<short> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<short?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ushort> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ushort?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<int> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<int?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<uint> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<uint?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<float> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<float?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ulong> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ulong?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<long> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<long?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<double> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<double?> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<decimal> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<decimal?> value);

    IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T> value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T?> value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<string> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char[]> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<IMutableString> value);
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IEnumerable<IStyledToStringObject> value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<object?> value);

    #endregion Collection Condition Populated

    #region Collection Condition Not Null and Populated

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, bool[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, bool?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, sbyte[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, sbyte?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, byte[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, byte?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, char[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, char?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, short[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, short?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ushort[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ushort?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, int[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, int?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, uint[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, uint?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, float[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, float?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ulong[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ulong?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, long[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, long?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, double[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, double?[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, decimal[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, decimal?[]? value);

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, string[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IStyledToStringObject[]? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IMutableString[]? value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, object?[]? value);

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<bool>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<bool?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<byte>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<byte?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<char>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<char?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<short>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<short?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ushort>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ushort?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<int>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<int?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<uint>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<uint?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<float>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<float?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ulong>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ulong?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<long>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<long?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<double>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<double?>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<decimal>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<decimal?>? value);

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct;

    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<string>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<IMutableString>? value);
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IEnumerable<IStyledToStringObject>? value);
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<object?> value);

    #endregion Collection Condition Not Null and Populated

    #endregion Add Collection Fields

    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    IStyledTypeStringAppender AddNonNullField(string fieldName, object? value);

    StringBuilder BackingStringBuilder { get; }

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public class StyledTypeStringAppender : ReusableObject<IStyledTypeStringAppender>, IStyledTypeStringAppender
{
    internal const string Indent = "  ";
    internal const string Null   = "null";

    private readonly StringBuilder sb = new();

    private StringBuildingStyle style;

    private int indent;

    public StyledTypeStringAppender() => style = StringBuildingStyle.Default;

    public StyledTypeStringAppender(StyledTypeStringAppender toClone)
    {
        indent = toClone.IndentLevel;
        style  = toClone.Style;
    }

    public int IndentLevel => indent;

    public StringBuildingStyle Style => style;

    public StringBuilder BackingStringBuilder => sb;

    public void ClearSetStyle(StringBuildingStyle stringStyle, int indentLevel = 0)
    {
        style  = stringStyle;
        indent = indentLevel;
        sb.Clear();
    }

    public IStyledTypeStringAppender AddTypeName(string value)
    {
        if (style.IsLog())
        {
            sb.Append(value);
        }
        return this;
    }

    public IStyledTypeStringAppender AddTypeStart()
    {
        sb.Append("{");
        indent++;
        return this;
    }

    public IStyledTypeStringAppender AddTypeEnd()
    {
        for (var i = sb.Length - 1; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
        {
            if (sb[i] == ',')
            {
                sb.Remove(i, 1);
                break;
            }
        }
        sb.Append("}");
        indent--;
        if (indent > 0 && style.IsJsonPretty())
        {
            sb.Append(",\n");
            AddToTypeLevelIndents();
        }
        return this;
    }

    #region Single Fields

    #region Always Add Single Fields

    public IStyledTypeStringAppender AddField(string fieldName, byte? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, sbyte? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, bool? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, char? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, short? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, ushort? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, int? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, uint? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, float? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, long? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, ulong? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, double? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, decimal? value) => AddFieldName(fieldName).AppendOrNull(value).AddGoToNext(this);

    public IStyledTypeStringAppender AddField<T>
        (string fieldName, T? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct
    {
        AddFieldName(fieldName);
        this.AppendOrNull(value, structToString);
        return sb.AddGoToNext(this);
    }

    public IStyledTypeStringAppender AddField(string fieldName, IStyledToStringObject? value) => AddFieldName(fieldName).AddNullOrValue(value, this);

    public IStyledTypeStringAppender AddField(string fieldName, IMutableString? value) => AddFieldName(fieldName).AddNullOrValue(value, this);

    public IStyledTypeStringAppender AddField(string fieldName, string? value) => AddFieldName(fieldName).Append(value ?? "null").AddGoToNext(this);

    public IStyledTypeStringAppender AddField(string fieldName, string? value, int startIndex, int length) =>
        AddFieldName(fieldName).AddNullOrValue(value, startIndex, length, this);


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddField(string fieldName, object? value) => AddFieldName(fieldName).AddNullOrValue(value, this);

    #endregion Always Add Single Fields

    #region Conditional Add Non Default Single Fields

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, bool value) => value ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, byte value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, sbyte value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, char value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, short value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, ushort value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, int value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, uint value) => value != 0u ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, float value) => value != 0f ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, long value) => value != 0L ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, ulong value) => value != 0ul ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, double value) => value != 0 ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField(string fieldName, decimal value) => value != 0m ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonDefaultField<T>
        (string fieldName, T value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        !Equals(value, null) ? AddField(fieldName, value, structToString) : this;

    #endregion Conditional Add Non Default Single Fields


    #region Conditional Add Non Null Single Fields

    public IStyledTypeStringAppender AddNonNullField(string fieldName, bool? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, byte? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, sbyte? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, char? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, short? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, ushort? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, int? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, uint? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, float? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, long? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, ulong? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, double? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, decimal? value) => value != null ? AddField(fieldName, value.Value) : this;

    public IStyledTypeStringAppender AddNonNullField<T>(string fieldName, T? value, Action<T, IStyledTypeStringAppender> structToString)
        where T : struct =>
        !Equals(value, null) ? AddField(fieldName, value.Value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullField(string fieldName, IStyledToStringObject? value) =>
        value != null ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullOrEmptyField(string fieldName, IMutableString? value) =>
        value != null ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullOrEmptyField(string fieldName, string? value) => value != null ? AddField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullOrEmptyField
        (string fieldName, string? value, int startIndex, int length) =>
        value != null ? AddField(fieldName, value) : this;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddNonNullField(string fieldName, object? value) => value != null ? AddField(fieldName, value) : this;

    #endregion Conditional Add Non Null Single Fields

    #endregion Single Fields

    #region Collection Fields

    #region Always Add Collection Fields

    #region Always Add Array Fields

    public IStyledTypeStringAppender AddCollectionField(string fieldName, bool[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, bool?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, sbyte[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, sbyte?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, byte[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, byte?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, char[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, char?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, short[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, short?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, ushort[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, ushort?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, int[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, int?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, uint[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, uint?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, float[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, float?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, long[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, long?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, ulong[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, ulong?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, double[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, double?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, decimal[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, decimal?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                structToString(value[i], this);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                this.AppendOrNull(value[i], structToString);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, string?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.Append(value[i] ?? Null);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IMutableString?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IStyledToStringObject?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                this.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddCollectionField(string fieldName, object?[]? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    #endregion Always Add Array Fields

    #region Always Add List Fields

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<bool>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<bool?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<sbyte>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<sbyte?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<byte>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<byte?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<short>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<short?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ushort>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ushort?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<int>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<int?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<uint>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<uint?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<float>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<float?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<long>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<long?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ulong>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<ulong?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<double>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<double?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<decimal>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<decimal?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                structToString(value[i], this);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                this.AppendOrNull(value[i], structToString);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<char[]>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<string?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.Append(value[i] ?? Null);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<IMutableString?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                this.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    public IStyledTypeStringAppender AddCollectionField(string fieldName, IEnumerable<IStyledToStringObject?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            foreach (var stso in value)
            {
                this.AppendOrNull(stso);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddCollectionField(string fieldName, IReadOnlyList<object?>? value)
    {
        StartCollection(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                sb.AppendOrNull(value[i]);
                GoToNextCollectionItemStart();
            }
        }
        else
        {
            sb.Append(Null);
        }
        return EndCollection();
    }

    #endregion Always Add List Fields

    #endregion Always Add Collection Fields

    #region Condition Add Array Fields

    #region Condition Non Null Array Fields

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, bool[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, bool?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, sbyte[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, sbyte?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, byte[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, byte?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, char[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, char?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, short[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, short?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ushort[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ushort?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, int[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, int?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, uint[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, uint?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, float[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, float?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, long[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, long?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ulong[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, ulong?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, double[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, double?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, decimal[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, decimal?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, string[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IMutableString[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IStyledToStringObject[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, object?[]? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    #endregion Condition Non Null Array Fields

    #region Condition Populated Array Fields

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, bool[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, bool?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, sbyte[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, sbyte?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, byte[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, byte?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, char[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, char?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, short[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, short?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ushort[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ushort?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, int[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, int?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, uint[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, uint?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, float[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, float?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, long[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, long?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ulong[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, ulong?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, double[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, double?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, decimal[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, decimal?[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, T[] value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value.Any() ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, T?[] value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value.Any() ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, string[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IMutableString[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IStyledToStringObject[] value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, object?[] value) =>
        value.Any() ? AddPopulatedCollectionField(fieldName, value) : this;

    #endregion Condition Populated Array Fields

    #region Condition Non Null and Populated Array Fields

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, bool[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, bool?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, sbyte[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, sbyte?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, byte[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, byte?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, char[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, char?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, short[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, short?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ushort[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ushort?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, int[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, int?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, uint[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, uint?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, float[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, float?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ulong[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, ulong?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, long[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, long?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, double[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, double?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, decimal[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, decimal?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, T[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, T?[]? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, string[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IStyledToStringObject[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IMutableString[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, object?[]? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    #endregion Condition Non Null and Populated Array Fields

    #endregion Condition Add Array Fields


    #region Condition List Fields

    #region Condition Non Null List Fields

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<bool>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<bool?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<sbyte>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<sbyte?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<byte>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<byte?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<short>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<short?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ushort>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ushort?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<int>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<int?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<uint>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<uint?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<float>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<float?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<long>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<long?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ulong>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<ulong?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<double>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<double?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<decimal>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<decimal?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<char[]>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<string>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<IMutableString>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IEnumerable<IStyledToStringObject>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddNonNullCollectionField(string fieldName, IReadOnlyList<object?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    #endregion Condition Non Null List Fields

    #region Condition Add Populated List Fields

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<bool> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<bool?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<byte> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<byte?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<short> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<short?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ushort> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ushort?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<int> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<int?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<uint> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<uint?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<float> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<float?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<long> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<long?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ulong> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<ulong?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<double> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<double?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<decimal> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<decimal?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T> value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value.Any() ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T?> value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value.Any() ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<char[]> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<string> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<IMutableString> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IEnumerable<IStyledToStringObject> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddPopulatedCollectionField(string fieldName, IReadOnlyList<object?> value) =>
        value.Any() ? AddCollectionField(fieldName, value) : this;

    #endregion Condition Add Populated List Fields

    #region Condition Non Null And Populated List Fields

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<bool>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<bool?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<sbyte?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<byte>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<byte?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<char>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<char?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<short>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<short?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ushort>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ushort?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<int>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<int?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<uint>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<uint?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<float>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<float?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ulong>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<ulong?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<long>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<long?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<double>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<double?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<decimal>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<decimal?>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField<T>
        (string fieldName, IReadOnlyList<T?>? value, Action<T, IStyledTypeStringAppender> structToString) where T : struct =>
        value != null ? AddPopulatedCollectionField(fieldName, value, structToString) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<string>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<IMutableString>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<IStyledToStringObject>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;

    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IEnumerable<IStyledToStringObject>? value) =>
        value != null ? AddPopulatedCollectionField(fieldName, value) : this;
    
    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public IStyledTypeStringAppender AddNonNullAndPopulatedCollectionField(string fieldName, IReadOnlyList<object?>? value) =>
        value != null ? AddCollectionField(fieldName, value) : this;

    #endregion Condition Non Null List Fields

    #endregion Condition List Fields

    #endregion Collection Fields

    public override StyledTypeStringAppender Clone() =>
        Recycler?.Borrow<StyledTypeStringAppender>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new StyledTypeStringAppender(this);

    public override StyledTypeStringAppender CopyFrom(IStyledTypeStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClearSetStyle(source.Style, IndentLevel);

        return this;
    }

    protected StringBuilder AddFieldName(string fieldName)
    {
        if (style.IsJsonPretty() || style.IsJsonCompact())
        {
            sb.Append(Indent).Append("\"").Append(fieldName).Append("\"").Append(": ");
        }
        else if (style.IsJsonCompact())
        {
            sb.Append("\"").Append(fieldName).Append("\"").Append(": ");
        }
        else
        {
            sb.Append(fieldName).Append(": ");
        }

        return sb;
    }

    protected StringBuilder RemoveLastWhiteSpacedCommaIfFound()
    {
        if (sb[^2] == ',' && sb[^1] == ' ')
        {
            sb.Length -= 2;
            sb.Append(" ");
            return sb;
        }
        for (var i = sb.Length - 1; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
        {
            if (sb[i] == ',')
            {
                sb.Remove(i, 1);
                break;
            }
        }
        return sb;
    }

    internal IStyledTypeStringAppender AddToTypeLevelIndents()
    {
        sb.AddIndents(indent - 1);
        return this;
    }

    internal void StartCollection(string fieldName)
    {
        AddFieldName(fieldName).Append("[");
        if (style.IsJsonPretty())
        {
            indent++;
            sb.Append("\n").AddIndents(indent);
        }
    }

    internal void GoToNextCollectionItemStart()
    {
        if (style.IsJsonPretty())
        {
            sb.Append(",\n").AddIndents(indent);
        }
        else
        {
            sb.Append(", ");
        }
    }

    internal IStyledTypeStringAppender EndCollection()
    {
        RemoveLastWhiteSpacedCommaIfFound();
        sb.Append("]");
        return sb.AddGoToNext(this);
    }

    public override string ToString() => sb.ToString();
}

public static class StyledTypeStringAppenderExtensions
{
    public static StringBuilder AddIndents(this StringBuilder sb, int indentLevel)
    {
        for (int i = 0; i < indentLevel; i++)
        {
            sb.Append(Indent);
        }
        return sb;
    }

    public static StyledTypeStringAppender AddGoToNext(this StringBuilder sb, StyledTypeStringAppender returnStyledAppender)
    {
        if (returnStyledAppender.Style.IsJsonPretty())
        {
            sb.Append(",\n");
            returnStyledAppender.AddToTypeLevelIndents();
        }
        else
        {
            sb.Append(", ");
        }
        return returnStyledAppender;
    }

    public static StringBuilder AppendOrNull(this StringBuilder sb, bool? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, sbyte? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, byte? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, char? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, short? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, ushort? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, int? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, uint? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, float? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, long? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, ulong? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, double? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, decimal? value) => value != null ? sb.Append(value.Value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, IMutableString? value) => value != null ? sb.Append(value) : sb.Append(Null);
    public static StringBuilder AppendOrNull(this StringBuilder sb, object? value) => value != null ? sb.Append(value) : sb.Append(Null);

    public static void AppendOrNull(this StyledTypeStringAppender stsa, IStyledToStringObject? value)
    {
        if (value != null)
            value.ToString(stsa);
        else
            stsa.BackingStringBuilder.Append(Null);
    }

    public static StyledTypeStringAppender AppendOrNull<T>
        (this StyledTypeStringAppender returnStyledAppender, T? value, Action<T, IStyledTypeStringAppender> styledToStringAction)
        where T : struct
    {
        if (value != null)
        {
            styledToStringAction(value.Value, returnStyledAppender);
        }
        else
        {
            returnStyledAppender.BackingStringBuilder.Append("null");
        }
        return returnStyledAppender;
    }

    public static StyledTypeStringAppender AddNullOrValue
        (this StringBuilder sb, IStyledToStringObject? value, StyledTypeStringAppender returnStyledAppender)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else
        {
            value.ToString(returnStyledAppender);
        }
        return sb.AddGoToNext(returnStyledAppender);
    }

    public static StyledTypeStringAppender AddNullOrValue(this StringBuilder sb, IMutableString? value, StyledTypeStringAppender returnStyledAppender)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else
        {
            sb.Append(value);
        }
        return sb.AddGoToNext(returnStyledAppender);
    }

    public static StyledTypeStringAppender AddNullOrValue
        (this StringBuilder sb, string? value, int startIndex, int length, StyledTypeStringAppender returnStyledAppender)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else
        {
            sb.Append(value, startIndex, length);
        }
        return sb.AddGoToNext(returnStyledAppender);
    }

    public static StyledTypeStringAppender AddNullOrValue(this StringBuilder sb, object? value, StyledTypeStringAppender returnStyledAppender)
    {
        if (value == null)
        {
            sb.Append("null");
        }
        else
        {
            sb.Append(value);
        }
        return sb.AddGoToNext(returnStyledAppender);
    }
}
