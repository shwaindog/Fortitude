#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types;

public interface IConverterRepository : IEnumerable<IConverter>
{
    IEnumerable<IConverter> GetConvertersWithToType(Type toType);
    IEnumerable<IConverter> GetConvertersWithFromType(Type fromType);
    IConverter<TIn, TOut>? GetConverter<TIn, TOut>();
    IConverter? GetConverter(Type fromType, Type toType);
    bool AddConverter(IConverter toAdd);
    bool RemoveConverter(IConverter toRemove);
}

public class ConverterRepository : IConverterRepository
{
    private readonly List<IConverter> registeredConverters = new();

    public ConverterRepository() { }

    public ConverterRepository(params IConverter[] addAll)
    {
        registeredConverters.AddRange(addAll);
    }

    public ConverterRepository(IEnumerable<IConverter> addAll)
    {
        registeredConverters.AddRange(addAll);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IConverter> GetEnumerator() => registeredConverters.GetEnumerator();

    public IEnumerable<IConverter> GetConvertersWithToType(Type toType) => registeredConverters.Where(c => c.ToType == toType);

    public IEnumerable<IConverter> GetConvertersWithFromType(Type fromType) => registeredConverters.Where(c => c.FromType == fromType);

    public IConverter<TIn, TOut>? GetConverter<TIn, TOut>() =>
        registeredConverters
            .FirstOrDefault(c => c.FromType == typeof(TIn) && c.ToType == typeof(TOut)) as IConverter<TIn, TOut>;

    public IConverter? GetConverter(Type fromType, Type toType) =>
        registeredConverters
            .FirstOrDefault(c => c.FromType == fromType && c.ToType == toType);

    public bool AddConverter(IConverter toAdd)
    {
        var checkUnique = GetConverter(toAdd.FromType, toAdd.ToType);
        if (checkUnique != null) return false;
        registeredConverters.Add(toAdd);
        return true;
    }

    public bool RemoveConverter(IConverter toRemove) => registeredConverters.Remove(toRemove);
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

public abstract class ConverterBase<TIn, TOut> : IConverter<TIn, TOut>
{
    public bool AreEquivalent(IConverter? other, bool exactTypes = false)
    {
        var fromTypeSame = FromType == other?.FromType;
        var toTypeSame = ToType == other?.ToType;
        return fromTypeSame && toTypeSame;
    }

    public Type FromType => typeof(TIn);
    public Type ToType => typeof(TOut);
    public object? ConvertBlind(object original, IRecycler? recycler = null) => Convert((TIn)original, recycler);

    public abstract TOut Convert(TIn original, IRecycler? recycler = null);
}
