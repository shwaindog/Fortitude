// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.Options;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using FortitudeCommon.Types.StyledToString.StyledTypes.ComplexType;
using FortitudeCommon.Types.StyledToString.StyledTypes.StyleFormatting;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;
using FortitudeCommon.Types.StyledToString.StyledTypes.ValueType;

#endregion

namespace FortitudeCommon.Types.StyledToString;

public interface IStyledTypeStringAppender : IReusableObject<IStyledTypeStringAppender>
{
    const string DefaultIndentString   = "  ";
    const string WhenNullString        = "null";
    const string DefaultString         = "";
    const string DefaultStringAsNumber = "0";
    // ReSharper disable UnusedMemberInSuper.Global
    StringBuildingStyle Style { get; }

    StyleOptions Settings { get; }

    int IndentLevel { get; }

    StyledTypeBuilder? CurrentTypeBuilder { get; }

    IStyledTypeStringAppender ClearAndReinitialize
        (StringBuildingStyle stringStyle, int indentLevel = 0, IgnoreWriteFlags ignoreFlags = IgnoreWriteFlags.None);

    IStyledTypeStringAppender Clear(int indentLevel = 0, IgnoreWriteFlags ignoreFlags = IgnoreWriteFlags.None);

    KeyValueCollectionBuilder StartKeyedCollectionType<T>(T toStyle, string? overrideName = null);

    SimpleOrderedCollectionBuilder StartSimpleCollectionType<T>(T toStyle, string? overrideName = null);

    ComplexOrderedCollectionBuilder StartComplexCollectionType<T>(T toStyle, string? overrideName = null);

    ComplexTypeBuilder StartComplexType<T>(T toStyle, string? overrideName = null);

    SimpleValueTypeBuilder StartSimpleValueType<T>(T toStyle, string? overrideName = null);

    ComplexValueTypeBuilder StartComplexValueType<T>(T toStyle, string? overrideName = null);

    // IStyleTypeBuilder AutoType<T>();


    IStringBuilder WriteBuffer { get; }

    bool Equals(string? toCompare);

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public interface IStyleTypeAppenderBuilderAccess : IStyledTypeStringAppender
{
    IgnoreWriteFlags IgnoreWriteFlags { get; }

    new IRecycler Recycler { get; }

    StyledTypeStringAppender ToTypeStringAppender { get; }

    void TypeComplete(IStyleTypeBuilderComponentAccess completeType);

    IStyledTypeStringAppender AddBaseFieldsStart();
    IStyledTypeStringAppender AddBaseFieldsEnd();
}

public class StyledTypeStringAppender : ReusableObject<IStyledTypeStringAppender>, IStyleTypeAppenderBuilderAccess
{
    internal const string Null = "null";

    private static readonly IRecycler AlWaysRecycler = new Recycler();

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    private IStyledTypeFormatting defaultStyledTypeFormatter;

    public static Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    protected int CurrentGraphNodeIndex = -1;

    private TypeAppendSettings initialAppendSettings;

    protected int NextObjVisitedRefId = 1;

    private TypeAppendSettings? nextTypeAppendSettings;

    protected List<GraphNodeVisit> OrderedObjectGraph = new(16);

    protected IStringBuilder? Sb;

    public StyledTypeStringAppender()
    {
        Settings.Style             = StringBuildingStyle.Default;
        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(Settings.Style);
    }

    public StyledTypeStringAppender(StringBuildingStyle withStyle)
    {
        Settings.Style = withStyle;

        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(Settings.Style);
    }

    public StyledTypeStringAppender(StyledTypeStringAppender toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        initialAppendSettings  = toClone.initialAppendSettings;
        nextTypeAppendSettings = toClone.nextTypeAppendSettings;

        Settings.Style             = toClone.Style;
        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(Settings.Style);
    }

    public StyledTypeStringAppender(IStyledTypeStringAppender toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Style             = toClone.Style;
        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(Settings.Style);
    }

    public IStyledTypeStringAppender Initialize(IStringBuilder usingStringBuilder, StringBuildingStyle buildStyle = StringBuildingStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb             = usingStringBuilder;
        Settings.Style = buildStyle;

        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(buildStyle);

        return this;
    }

    public IStyledTypeStringAppender Initialize(StringBuildingStyle buildStyle = StringBuildingStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        Settings.Style = buildStyle;

        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(buildStyle);

        return this;
    }

    protected IStyledTypeFormatting SourceDefaultStyledTypeFormatter(StringBuildingStyle forBuildingStyle)
    {
        switch (forBuildingStyle)
        {
            case StringBuildingStyle.Json | StringBuildingStyle.Compact: return new CompactJsonTypeFormatting();
            case StringBuildingStyle.Json | StringBuildingStyle.Pretty: return new PrettyJsonTypeFormatting();
            default:                                                     return new CompactLogTypeFormatting();
        }
    }

    protected TypeAppendSettings AppendSettings => nextTypeAppendSettings ?? CurrentTypeAccess?.AppendSettings ?? initialAppendSettings;

    protected IStyleTypeBuilderComponentAccess? CurrentTypeAccess => CurrentNode?.TypeBuilderComponentAccess;

    public bool UseEqualsForVisited
    {
        get => UseReferenceEqualsForVisited!;
        set => UseReferenceEqualsForVisited = !value;
    }

    public StyleOptions Settings { get; set; } = new(new StyleOptionsValue());


    public static StyleOptions DefaultSettings { get; set; } = new(new StyleOptionsValue());

    public bool UseReferenceEqualsForVisited { get; set; }

    public int IndentLevel => AppendSettings.IndentLvl;

    public IgnoreWriteFlags IgnoreWriteFlags => AppendSettings.IgnoreWriteFlags;

    public StringBuildingStyle Style => Settings.Values.Style;

    public IStringBuilder WriteBuffer => Sb ??= BufferFactory();

    public new IRecycler Recycler => base.Recycler ?? AlWaysRecycler;

    public StyledTypeBuilder? CurrentTypeBuilder => CurrentTypeAccess?.StyleTypeBuilder;

    protected GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;

    public IStyledTypeStringAppender ClearAndReinitialize(StringBuildingStyle stringStyle, int indentLevel = 0
      , IgnoreWriteFlags ignoreWrite = IgnoreWriteFlags.None)
    {
        Settings.Style = stringStyle;

        defaultStyledTypeFormatter = SourceDefaultStyledTypeFormatter(stringStyle);
        initialAppendSettings      = new TypeAppendSettings((ushort)indentLevel, ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        return this;
    }

    void IStyleTypeAppenderBuilderAccess.TypeComplete(IStyleTypeBuilderComponentAccess completeType)
    {
        PopCurrentSettings();
        ((IRecyclableObject)completeType).DecrementRefCount();
    }

    KeyValueCollectionBuilder IStyledTypeStringAppender.StartKeyedCollectionType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyValueCollectionBuilder>()
                    .InitializeKeyValueCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, type);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionBuilder StartSimpleCollectionType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionBuilder>()
                    .InitializeSimpleOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleOrderedCollectionBuilder, type);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionBuilder StartComplexCollectionType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionBuilder>()
                    .InitializeComplexOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, complexOrderedCollectionBuilder, type);
        return complexOrderedCollectionBuilder;
    }

    public ComplexTypeBuilder StartComplexType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var complexTypeBuilder =
            Recycler.Borrow<ComplexTypeBuilder>()
                    .InitializeComplexTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, complexTypeBuilder, type);
        return complexTypeBuilder;
    }

    public SimpleValueTypeBuilder StartSimpleValueType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var simpleValueBuilder =
            Recycler.Borrow<SimpleValueTypeBuilder>()
                    .InitializeSimpleValueTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleValueBuilder, type);
        return simpleValueBuilder;
    }

    public ComplexValueTypeBuilder StartComplexValueType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, defaultStyledTypeFormatter);
        var keyedCollectionBuilder =
            Recycler.Borrow<ComplexValueTypeBuilder>()
                    .InitializeComplexValueTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, type);
        return keyedCollectionBuilder;
    }

    StyledTypeStringAppender IStyleTypeAppenderBuilderAccess.ToTypeStringAppender => this;

    IStyledTypeStringAppender IStyleTypeAppenderBuilderAccess.AddBaseFieldsEnd()
    {
        PopCurrentSettings();

        return this;
    }

    IStyledTypeStringAppender IStyleTypeAppenderBuilderAccess.AddBaseFieldsStart()
    {
        var nextAppender = nextTypeAppendSettings ??
                           new TypeAppendSettings(CurrentTypeAccess?.IndentLevel ?? 0, IgnoreWriteFlags.All);
        nextAppender.IgnoreWriteFlags = IgnoreWriteFlags.TypeStart | IgnoreWriteFlags.TypeName | IgnoreWriteFlags.TypeEnd;
        nextTypeAppendSettings        = nextAppender;

        return this;
    }

    public IStyledTypeStringAppender Clear(int indentLevel = 0, IgnoreWriteFlags ignoreWrite = IgnoreWriteFlags.None)
    {
        initialAppendSettings = new TypeAppendSettings((ushort)indentLevel, ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        return this;
    }

    protected int NextRefId() => NextObjVisitedRefId++;

    protected void TypeStart<T>(T toStyle, StyledTypeBuilder newType, Type typeOfT)
    {
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeOfT
                                        , typeOfT.IsValueType ? null : toStyle, AppendSettings.IndentLvl, Sb!.Length)
        {
            TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).ComponentAccess
        };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;
        nextTypeAppendSettings = new TypeAppendSettings((ushort)newVisit.IndentLevel, IgnoreWriteFlags.None);

        newType.Start();
    }


    protected void PopCurrentSettings()
    {
        var currentNode = CurrentNode;
        if (currentNode != null)
        {
            OrderedObjectGraph[CurrentGraphNodeIndex] = currentNode.Value.ClearComponentAccess();
            CurrentGraphNodeIndex                     = currentNode.Value.ParentVisitIndex;
            if (CurrentGraphNodeIndex < 0)
            {
                OrderedObjectGraph.Clear();
            }
        }
    }

    private int SourceGraphVisitRefId<T>(T toStyle, Type type)
    {
        if (toStyle is object objToStyle)
            for (var i = 0; i < OrderedObjectGraph.Count; i++)
            {
                var graphNodeVisit = OrderedObjectGraph[i];
                if (HasVisited(objToStyle, type, graphNodeVisit))
                {
                    if (graphNodeVisit.RefId == 0)
                    {
                        OrderedObjectGraph[i] = graphNodeVisit.SetRefId(NextRefId());
                        InsertRefId(graphNodeVisit);
                    }
                    return OrderedObjectGraph[i].RefId;
                }
            }
        return 0;
    }

    protected bool HasVisited(object objToStyle, Type objAsType, GraphNodeVisit checkExisting)
    {
        var checkRef               = checkExisting.StylingObjInstance;
        var hasVisited             = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, objToStyle) : Equals(checkRef, objToStyle);
        if (hasVisited) hasVisited = !IsCallingAsBaseType(objToStyle, objAsType, checkExisting);
        return hasVisited;
    }

    protected void InsertRefId(GraphNodeVisit forThisNode) { }

    protected bool IsCallingAsBaseType(object objToStyle, Type objAsType, GraphNodeVisit startToLast)
    {
        for (var i = startToLast.ObjVisitIndex; i < OrderedObjectGraph.Count; i++)
        {
            var checkExisting  = OrderedObjectGraph[i];
            var checkRef       = checkExisting.StylingObjInstance;
            var isSameInstance = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, objToStyle) : Equals(checkRef, objToStyle);
            if (isSameInstance)
            {
                if (checkExisting.VistedAsType == objAsType || !checkExisting.VistedAsType.IsAssignableTo(objAsType))
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected virtual IStringBuilder SourceStringBuilder() => Sb ??= BufferFactory();

    protected virtual void ClearStringBuilder() => Sb = null!;

    protected IStringBuilder RemoveLastWhiteSpacedCommaIfFound()
    {
        var sb = WriteBuffer;
        if (sb[^2] == ',' && sb[^1] == ' ')
        {
            sb.Length -= 2;
            sb.Append(" ");
            return sb;
        }
        for (var i = sb.Length - 1; i > 0 && sb[i] is ' ' or '\r' or '\n' or ','; i--)
            if (sb[i] == ',')
            {
                sb.Remove(i, 1);
                break;
            }
        return sb;
    }

    public override void StateReset()
    {
        ClearStringBuilder();

        base.StateReset();
    }

    public override StyledTypeStringAppender Clone() => Recycler.Borrow<StyledTypeStringAppender>().CopyFrom(this, CopyMergeFlags.FullReplace);

    public override IStyledTypeStringAppender CopyFrom(IStyledTypeStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((StyledTypeStringAppender)source, copyMergeFlags);

    public StyledTypeStringAppender CopyFrom
        (StyledTypeStringAppender source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClearAndReinitialize(source.Style, IndentLevel);
        Sb!.Append(source.WriteBuffer);

        initialAppendSettings  = source.initialAppendSettings;
        nextTypeAppendSettings = source.nextTypeAppendSettings;

        return this;
    }

    public bool Equals(string? toCompare)
    {
        var sb = WriteBuffer;
        if (toCompare == null) return false;
        if (sb.Length != toCompare.Length) return false;
        for (var i = 0; i < sb.Length; i++)
            if (sb[i] != toCompare[i])
                return false;
        return true;
    }

    public override string ToString() => WriteBuffer.ToString();

    protected record struct GraphNodeVisit
    (
        int ObjVisitIndex
      , int ParentVisitIndex
      , Type VistedAsType
      , object? StylingObjInstance
      , int IndentLevel
      , int OriginalBufferTypeStart)
    {
        public int RefId { get; private init; }

        public bool IsValueTYpe = VistedAsType.IsValueType;

        public IStyleTypeBuilderComponentAccess? TypeBuilderComponentAccess { get; init; }

        public GraphNodeVisit SetRefId(int newRefId)
        {
            return this with
            {
                RefId = newRefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart
            };
        }

        public GraphNodeVisit ClearComponentAccess()
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = null
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart
            };
        }

        public GraphNodeVisit ShiftTypeBufferIndex(int amountToShift)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart + amountToShift
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart + amountToShift
            };
        }

        public int CurrentBufferTypeStart { get; init; } = OriginalBufferTypeStart;

        public int CurrentBufferFirstFieldStart { get; init; }
    }
}
