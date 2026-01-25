using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Types.Mutable.CopyMergeFlags;
using static FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions.IFormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public interface IFormattingOptions : ICloneable<IFormattingOptions>
{
    public const string DefaultNullString = "null";

    public const string Empty = "";
    public const string Cma   = ",";
    public const string Spc   = " ";

    protected const string DefaultTrueString             = "true";
    protected const string DefaultFalseString            = "false";
    protected const string DefaultMainItemSeparator      = Cma;
    protected const string DefaultAlternateItemSeparator = Empty;
    protected const string DefaultMainItemPadding        = Spc;
    protected const string DefaultAlternateItemPadding   = Empty;

    protected const bool DefaultEnumAsNumber = false;

    string MainItemSeparator { get; set; }
    string AlternateItemSeparator { get; set; }
    string MainItemPadding { get; set; }
    string AlternateItemPadding { get; set; }

    bool NullWritesEmpty { get; set; }

    bool NullWritesNullString { get; set; }

    bool EmptyCollectionWritesNull { get; set; }

    bool IgnoreEmptyCollection { get; set; }

    string NullString { get; set; }

    string True { get; set; }

    string False { get; set; }

    bool EnumsDefaultAsNumber { get; set; }
}

public class FormattingOptions : ExplicitRecyclableObject, IFormattingOptions, ITransferState<IFormattingOptions>
{
    private static int globalInstanceId;

    protected int InstanceId = Interlocked.Increment(ref globalInstanceId);

    protected ICustomStringFormatter? Stringformatter;

    private bool writeNullString = true;

    protected const char TokenClose = '}';
    protected const char TokenOpen  = '{';

    public FormattingOptions() { }

    public FormattingOptions(IFormattingOptions toClone)
    {
        MainItemSeparator         = toClone.MainItemSeparator;
        NullWritesNullString      = toClone.NullWritesNullString;
        EmptyCollectionWritesNull = toClone.EmptyCollectionWritesNull;
        IgnoreEmptyCollection     = toClone.IgnoreEmptyCollection;

        NullString = toClone.NullString;

        True  = toClone.True;
        False = toClone.False;
    }

    public string MainItemSeparator { get; set; } = DefaultMainItemSeparator;
    public string AlternateItemSeparator { get; set; } = DefaultAlternateItemSeparator;
    public string MainItemPadding { get; set; } = DefaultMainItemPadding;
    public string AlternateItemPadding { get; set; } = DefaultAlternateItemPadding;

    public bool NullWritesEmpty
    {
        get => !writeNullString;
        set => writeNullString = !value;
    }

    public bool NullWritesNullString
    {
        get => writeNullString;
        set => writeNullString = value;
    }

    public bool EmptyCollectionWritesNull { get; set; }

    public bool IgnoreEmptyCollection { get; set; }

    public string NullString { get; set; } = DefaultNullString;

    public string True { get; set; } = DefaultTrueString;
    public string False { get; set; } = DefaultFalseString;

    public bool EnumsDefaultAsNumber { get; set; }

    object ICloneable.Clone() => Clone();

    public virtual IFormattingOptions Clone() =>
        ((ITransferState<IFormattingOptions>)(AlwaysRecycler).Borrow<FormattingOptions>())
        .CopyFrom(this, FullReplace);

    ITransferState ITransferState.CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags)
    {
        return (ITransferState)(((ITransferState<IFormattingOptions>)this).CopyFrom((IFormattingOptions)source, copyMergeFlags));
    }

    IFormattingOptions ITransferState<IFormattingOptions>.CopyFrom(IFormattingOptions source, CopyMergeFlags copyMergeFlags)
    {
        MainItemSeparator         = source.MainItemSeparator;
        NullWritesNullString      = source.NullWritesNullString;
        EmptyCollectionWritesNull = source.EmptyCollectionWritesNull;
        IgnoreEmptyCollection     = source.IgnoreEmptyCollection;

        NullString = source.NullString;

        True  = source.True;
        False = source.False;

        return this;
    }

    public override string ToString() => $"{{ {GetType().Name}: {InstanceId}, }}";
}
