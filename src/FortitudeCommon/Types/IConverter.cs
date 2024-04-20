#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types;

public interface IConverterRepository : IEnumerable<IConverter>
{
    IEnumerable<IConverter> GetConvertersWithToType(Type toType);
    IEnumerable<IConverter> GetConvertersWithFromType(Type fromType);
    IConverter<TIn, TOut> GetConverter<TIn, TOut>();
    IConverter GetConverter(Type fromType, Type toType);
    bool AddConverter(IConverter toAdd);
    bool RemoveConverter(IConverter toRemove);
}

public interface IConverter : IInterfacesComparable<IConverter>
{
    Type FromType { get; }
    Type ToType { get; }
    object? ConvertBlind(object original, IRecycler? recycler = null);
}

public interface IConverter<in TIn, out TOut> : IConverter
{
    TOut Convert(TIn original, IRecycler? recycler = null);
}
