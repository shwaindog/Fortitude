// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using KeyedCollectionMold = FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.KeyedCollectionMold;

#endregion

namespace FortitudeCommon.Types.StringsOfPower;

public interface ITheOneString : IReusableObject<ITheOneString>
{
    // ReSharper disable UnusedMemberInSuper.Global
    StringStyle Style { get; }

    StyleOptions Settings { get; }

    CallerContext CallerContext { get; }

    int IndentLevel { get; set; }

    TypeMolder? CurrentTypeBuilder { get; }

    IStyledTypeFormatting CurrentStyledTypeFormatter { get; set; }

    ITheOneString ReInitialize(IStringBuilder usingStringBuilder, StringStyle buildStyle = StringStyle.CompactLog);

    ITheOneString ClearAndReinitialize
        (StringStyle style, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString ClearAndReinitialize
        (StyleOptions styleOptions, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString ClearAndReinitialize
        (StyleOptionsValue styleOptions, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString ClearVisitHistory();

    KeyedCollectionMold StartKeyedCollectionType<T>(T toStyle, CreateContext createContext = default);

    ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext = default);

    SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement> toStyle, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement?> toStyle, CreateContext createContext = default)
        where TElement : struct;

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement> toStyle
      , CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement?> toStyle
      , CreateContext createContext = default)
        where TElement : struct;

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object toStyle, CreateContext createContext = default);

    ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default);

    ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default);

    SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default);

    ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default);

    CallContextDisposable ResolveContextForCallerFlags(FormatFlags contentFlags);

    bool IsLastVisitedObject<TVisited>(TVisited checkIsLastVisited);

    // IStyleTypeBuilder AutoType<T>();


    IStringBuilder WriteBuffer { get; }

    bool Equals(string? toCompare);

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public interface ISecretStringOfPower : ITheOneString
{
    SkipTypeParts SkipTypeParts { get; }

    new IRecycler Recycler { get; }

    new CallerContext CallerContext { get; set; }

    GraphTrackingBuilder GraphBuilder { get; set; }

    void SetCallerFormatFlags(FormatFlags callerContentHandler);
    void SetCallerFormatString(string? formatString);

    StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    bool RegisterVisitedCheckCanContinue<T>(T guest, FormatFlags formatFlags);
    int  EnsureRegisteredVisited<T>(T guest, FormatFlags formatFlags);

    void TypeComplete(ITypeMolderDieCast completeType);

    ITheOneString AddBaseFieldsStart();
    ITheOneString AddBaseFieldsEnd();
}

public readonly record struct StyleFormattingState
    (IStyledTypeFormatting StyleFormatter, IEncodingTransfer StringEncoder, IEncodingTransfer GraphEncoder, IEncodingTransfer ParentGraphEncoder) { }

public readonly struct CallContextDisposable
(
    bool shouldSkip
  , ISecretStringOfPower? stringMaster = null
  , StyleOptions? toRestoreOnDispose = null
  , StyleFormattingState? formattingState = null)
    : IDisposable
{
    public bool ShouldSkip => shouldSkip;

    public bool HasFormatChange => toRestoreOnDispose != null;

    public void Dispose()
    {
        if (toRestoreOnDispose != null && stringMaster != null)
        {
            stringMaster.Settings.CopyFrom(toRestoreOnDispose);
            if (formattingState != null)
            {
                stringMaster.CurrentStyledTypeFormatter = formattingState.Value.StyleFormatter;

                stringMaster.GraphBuilder.GraphEncoder                   = formattingState.Value.GraphEncoder;
                stringMaster.GraphBuilder.ParentGraphEncoder             = formattingState.Value.ParentGraphEncoder;
                stringMaster.Settings.StyledTypeFormatter.ContentEncoder = formattingState.Value.StringEncoder;
            }
            ((IRecyclableObject)toRestoreOnDispose).DecrementRefCount();
        }
    }
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    private static readonly IRecycler AlWaysRecycler = new Recycler();

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static readonly Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    protected int CurrentGraphNodeIndex = -1;

    private MoldDieCastSettings initialAppendSettings;

    protected int NextObjVisitedRefId = 1;

    private MoldDieCastSettings? nextTypeAppendSettings;

    protected readonly List<GraphNodeVisit> OrderedObjectGraph = new(16);

    protected IStringBuilder? Sb;
    private   CallerContext   callerContext;

    private StyleOptions?         settings = new(new StyleOptionsValue());
    private GraphTrackingBuilder? graphBuilder;

    public TheOneString()
    {
        if (DefaultSettings == null)
        {
            Settings.Style     = StringStyle.CompactLog;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
        else
        {
            Settings.Values    = DefaultSettings.Values;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
    }

    public TheOneString(StringStyle withStyle)
    {
        if (DefaultSettings == null)
        {
            Settings.Style     = withStyle;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
        else
        {
            Settings.Values    = DefaultSettings.Values;
            Settings.Style     = withStyle;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
    }

    public TheOneString(TheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        initialAppendSettings  = toClone.initialAppendSettings;
        nextTypeAppendSettings = toClone.nextTypeAppendSettings;

        if (DefaultSettings == null)
        {
            Settings.Style     = toClone.Style;
            Settings.Formatter = toClone.Formatter;
        }
        else
        {
            Settings.Values    = DefaultSettings.Values;
            Settings.Formatter = DefaultSettings.Formatter;
        }
    }

    public TheOneString(ITheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Values    = toClone.Settings.Values;
        Settings.Formatter = CurrentStyledTypeFormatter;
    }

    public ITheOneString Initialize(StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        Settings.Style = buildStyle;


        ClearObjectVisitedGraph();
        Settings.Formatter = CurrentStyledTypeFormatter;

        return this;
    }

    protected void ClearObjectVisitedGraph()
    {
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            graphNodeVisit.Reset();
        }
        OrderedObjectGraph.Clear();
    }

    protected MoldDieCastSettings AppendSettings => nextTypeAppendSettings ?? CurrentTypeAccess?.AppendSettings ?? initialAppendSettings;

    protected ITypeMolderDieCast? CurrentTypeAccess => CurrentNode?.TypeBuilderComponentAccess;

    public bool UseEqualsForVisited
    {
        get => UseReferenceEqualsForVisited;
        set => UseReferenceEqualsForVisited = !value;
    }

    public StyleOptions Settings
    {
        get => settings ??= Recycler.Borrow<StyleOptions>();
        set
        {
            if (settings != null! && value != Settings) { ((IRecyclableObject)settings).DecrementRefCount(); }
            settings = value;
            ((IRecyclableObject)settings).IncrementRefCount();
        }
    }


    public static StyleOptions? DefaultSettings { get; set; }

    public bool UseReferenceEqualsForVisited { get; set; }

    public int IndentLevel
    {
        get => Settings.IndentLevel;
        set => Settings.IndentLevel = value;
    }

    public SkipTypeParts SkipTypeParts => AppendSettings.SkipTypeParts;

    public StringStyle Style => Settings.Values.Style;

    public IStringBuilder WriteBuffer => Sb ??= BufferFactory();

    public new IRecycler Recycler => base.Recycler ?? AlWaysRecycler;

    public TypeMolder? CurrentTypeBuilder => CurrentTypeAccess?.TypeMolder;

    public CallerContext CallerContext
    {
        get => callerContext;
        set => callerContext = value;
    }

    public GraphTrackingBuilder GraphBuilder
    {
        get => graphBuilder ??= Recycler.Borrow<GraphTrackingBuilder>().Initialize(Settings);
        set => graphBuilder = value;
    }

    public ICustomStringFormatter Formatter
    {
        get => Settings.Formatter ??= Settings.Formatter = this.ResolveStyleFormatter();
        set => Settings.Formatter = value;
    }

    public IStyledTypeFormatting CurrentStyledTypeFormatter
    {
        get => (IStyledTypeFormatting)Formatter;
        set => Formatter = value;
    }

    public void SetCallerFormatFlags(FormatFlags callerContentHandler)
    {
        callerContext.FormatFlags = callerContentHandler;
    }

    public void SetCallerFormatString(string? formatString)
    {
        callerContext.FormatString = formatString;
    }

    protected GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;

    public ITheOneString ReInitialize(IStringBuilder usingStringBuilder, StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();

        Sb = usingStringBuilder;

        Settings.Style = buildStyle;

        CallerContext = new CallerContext();

        Settings.Formatter = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.StateReset();


        ClearObjectVisitedGraph();
        Settings.Formatter = CurrentStyledTypeFormatter;

        return this;
    }

    public ITheOneString ClearAndReinitialize
        (StringStyle style, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None)
    {
        Settings.Style = style;

        IndentLevel = indentLevel;

        initialAppendSettings = new MoldDieCastSettings(ignoreFlags);
        Sb?.Clear();
        Sb ??= BufferFactory();

        CallerContext = new CallerContext();

        Settings.Formatter = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.StateReset();

        Settings.StyledTypeFormatter.GraphBuilder = GraphBuilder;

        return ClearVisitHistory();
    }

    public ITheOneString ClearAndReinitialize(StyleOptions styleOptions, int indentLevel = 0
      , SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        Settings = styleOptions;

        IndentLevel = indentLevel;

        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        CallerContext = new CallerContext();

        Settings.Formatter = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.StateReset();

        Settings.StyledTypeFormatter.GraphBuilder = GraphBuilder;

        return ClearVisitHistory();
    }

    public ITheOneString ClearAndReinitialize(StyleOptionsValue styleOptionsValue, int indentLevel = 0
      , SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        Settings.Values = styleOptionsValue;

        IndentLevel = indentLevel;

        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        CallerContext = new CallerContext();

        Settings.Formatter = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.StateReset();

        Settings.StyledTypeFormatter.GraphBuilder = GraphBuilder;

        return ClearVisitHistory();
    }

    public ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        CallerContext = new CallerContext();
        GraphBuilder.StateReset();


        return ClearVisitHistory();
    }

    public ITheOneString ClearVisitHistory()
    {
        ClearObjectVisitedGraph();

        return this;
    }

    void ISecretStringOfPower.TypeComplete(ITypeMolderDieCast completeType)
    {
        var finishedAsComplex = completeType.WriteAsComplex;
        if (completeType.DecrementRefCount() == 0)
        {
            PopCurrentSettings(finishedAsComplex);
        }
    }

    public StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var type           = obj.GetType();
        var existingRefId  = SourceGraphVisitRefId(obj, type, formatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;

        return existingRefId > 0 || remainingDepth <= 0
            ? StartComplexContentType(obj).AsValueMatch("", obj, formatString, formatFlags).Complete()
            : StartSimpleContentType(obj).AsValueMatch(obj, formatString, formatFlags).Complete();
    }

    public bool RegisterVisitedCheckCanContinue<T>(T guest, FormatFlags formatFlags)
    {
        var type = guest?.GetType() ?? typeof(T);

        var existingRefId = SourceGraphVisitRefId(guest, type, formatFlags);
        if (existingRefId > 0)
        {
            StartComplexContentType(guest, formatFlags).AsStringOrNull("", "").Complete();
            return false;
        }

        GraphNodeVisit newVisit;
        var fmtState = new FormattingState
            ((CurrentNode?.GraphDepth ?? -1) + 1
           , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, formatFlags
           , IndentLevel, Settings.IndentSize, CurrentStyledTypeFormatter, GraphBuilder.GraphEncoder
           , GraphBuilder.ParentGraphEncoder);
        if (type.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(guest);

            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false
                                        , storageContainer, Sb!.Length, fmtState);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false
                                        , guest, Sb!.Length, fmtState);
        }

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return true;
    }

    public int EnsureRegisteredVisited<T>(T guest, FormatFlags formatFlags)
    {
        var type = guest?.GetType() ?? typeof(T);

        var firstVisitedIndex = IndexOfInstanceVisitFromEnd(guest, type);
        if (firstVisitedIndex >= 0) { return firstVisitedIndex; }
        GraphNodeVisit newVisit;
        var fmtState = new FormattingState
            ((CurrentNode?.GraphDepth ?? -1) + 1
           , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, formatFlags
           , IndentLevel, Settings.IndentSize, CurrentStyledTypeFormatter, GraphBuilder.GraphEncoder
           , GraphBuilder.ParentGraphEncoder);
        if (type.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(guest);

            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false
                                        , storageContainer, Sb!.Length, fmtState);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false
                                        , guest, Sb!.Length, fmtState);
        }

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return newVisit.ObjVisitIndex;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyedCollectionMold>().InitializeKeyValueCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, keyedCollectionBuilder, actualType, createFlags);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType  = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var existingRefId  = SourceGraphVisitRefId(keyValueContainerInstance, actualType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(keyValueContainerInstance, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createFlags);
        TypeStart(keyValueContainerInstance, keyedCollectionBuilder, actualType, createFlags);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, simpleOrderedCollectionBuilder, actualType, createFlags);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, complexOrderedCollectionBuilder, actualType, createFlags);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement> toStyle
      , CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(Span<TElement>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createFlags);
        TypeStart(visitType, explicitOrderedCollectionBuilder, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement?> toStyle
      , CreateContext createContext = default)
        where TElement : struct
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(Span<TElement?>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createFlags);
        TypeStart(visitType, explicitOrderedCollectionBuilder, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement> toStyle
      , CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(ReadOnlySpan<TElement>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createFlags);
        TypeStart(visitType, explicitOrderedCollectionBuilder, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement?> toStyle
      , CreateContext createContext = default)
        where TElement : struct
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(ReadOnlySpan<TElement?>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createFlags);
        TypeStart(visitType, explicitOrderedCollectionBuilder, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType     = collectionInstance.GetType();
        var existingRefId  = SourceGraphVisitRefId(collectionInstance, actualType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(collectionInstance, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(collectionInstance, explicitOrderedCollectionBuilder, actualType, createFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexTypeBuilder =
            Recycler.Borrow<ComplexPocoTypeMold>().InitializeComplexTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, complexTypeBuilder, actualType, createFlags);
        return complexTypeBuilder;
    }

    public SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetValueTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleValueBuilder =
            Recycler.Borrow<SimpleContentTypeMold>().InitializeSimpleValueTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, simpleValueBuilder, actualType, createFlags);
        return simpleValueBuilder;
    }

    public ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags    = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType, createFlags);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetValueTypeAppendSettings(toStyle, actualType, typeFormatter, createFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexContentBuilder =
            Recycler.Borrow<ComplexContentTypeMold>().InitializeComplexValueTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createFlags);
        TypeStart(toStyle, complexContentBuilder, actualType, createFlags);
        return complexContentBuilder;
    }

    ITheOneString ISecretStringOfPower.AddBaseFieldsEnd()
    {
        PopCurrentSettings(OrderedObjectGraph[CurrentGraphNodeIndex].IsComplexType);

        return this;
    }

    ITheOneString ISecretStringOfPower.AddBaseFieldsStart()
    {
        var nextAppender = nextTypeAppendSettings ??
                           new MoldDieCastSettings(SkipTypeParts.All);
        nextAppender.SkipTypeParts = SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd;
        nextTypeAppendSettings     = nextAppender;

        return this;
    }

    public MoldDieCastSettings GetComplexTypeAppendSettings<TElement>(Span<TElement> forValue, Type actualType
      , IStyledTypeFormatting formatter, FormatFlags formatFlags)
    {
        var nextDieSettings = AppendSettings;
        nextDieSettings.SkipTypeParts |= formatter.GetNextComplexTypePartFlags(this, actualType, formatFlags);
        return nextDieSettings;
    }

    public MoldDieCastSettings GetComplexTypeAppendSettings<TElement>(ReadOnlySpan<TElement> forValue, Type actualType
      , IStyledTypeFormatting formatter, FormatFlags formatFlags)
    {
        var nextDieSettings = AppendSettings;
        nextDieSettings.SkipTypeParts |= formatter.GetNextComplexTypePartFlags(this, actualType, formatFlags);
        return nextDieSettings;
    }

    public MoldDieCastSettings GetComplexTypeAppendSettings<T>(T forValue, Type actualType, IStyledTypeFormatting formatter, FormatFlags formatFlags)
    {
        var nextDieSettings = AppendSettings;
        nextDieSettings.SkipTypeParts |= formatter.GetNextComplexTypePartFlags(this, forValue, actualType, formatFlags);
        return nextDieSettings;
    }

    public MoldDieCastSettings GetValueTypeAppendSettings<T>(T forValue, Type actualType, IStyledTypeFormatting formatter, FormatFlags formatFlags)
    {
        var nextDieSettings = AppendSettings;
        nextDieSettings.SkipTypeParts |= formatter.GetNextValueTypePartFlags(this, forValue, actualType, formatFlags);
        return nextDieSettings;
    }

    public CallContextDisposable ResolveContextForCallerFlags(FormatFlags contentFlags)
    {
        var previousStyle = Settings.Style;
        if ((contentFlags & StyleMask) > 0)
        {
            var shouldSkip = false;
            shouldSkip |= Settings.Style.IsLog() && contentFlags.HasExcludeWhenLogStyle();
            shouldSkip |= Settings.Style.IsJson() && contentFlags.HasExcludeWhenJsonStyle();
            shouldSkip |= Settings.Style.IsCompact() && contentFlags.HasExcludeWhenCompactLayout();
            shouldSkip |= Settings.Style.IsPretty() && contentFlags.HasExcludeWhenPrettyLayout();
            if (shouldSkip) return new CallContextDisposable(true);
        }
        if (Settings.IsSame(contentFlags)
         && !(previousStyle.IsJson()
           && contentFlags.HasAsStringContentFlag()
           && GraphBuilder.GraphEncoder.Type != EncodingType.JsonEncoding)
         && GraphBuilder.GraphEncoder.Type == GraphBuilder.ParentGraphEncoder.Type)
            return new CallContextDisposable(false);
        Settings.IfExistsIncrementFormatterRefCount();
        var saveCurrentOptions = Recycler.Borrow<StyleOptions>().Initialize(Settings);

        var existingOptions =
            new CallContextDisposable
                (false, this, saveCurrentOptions
               , new StyleFormattingState
                     (Settings.StyledTypeFormatter
                    , Settings.StyledTypeFormatter.ContentEncoder
                    , Settings.StyledTypeFormatter.GraphBuilder.GraphEncoder
                    , Settings.StyledTypeFormatter.GraphBuilder.ParentGraphEncoder));

        Settings.Style     = contentFlags.UpdateStringStyle(Settings.Style);
        Settings.Formatter = this.ResolveStyleFormatter();

        if (Settings.Style.IsJson()
         && contentFlags.HasAsStringContentFlag()
         && GraphBuilder.GraphEncoder.Type != EncodingType.JsonEncoding)
        {
            GraphBuilder.GraphEncoder = Settings.StyledTypeFormatter.ContentEncoder.Type == EncodingType.JsonEncoding
                ? Settings.StyledTypeFormatter.ContentEncoder
                : this.ResolveStyleEncoder(EncodingType.JsonEncoding);
        }
        else if (Settings.Style == previousStyle && GraphBuilder.ParentGraphEncoder != GraphBuilder.GraphEncoder)
        {
            // no change so parent and child have same encoder
            GraphBuilder.ParentGraphEncoder = GraphBuilder.GraphEncoder;
        }

        return existingOptions;
    }

    protected int NextRefId() => NextObjVisitedRefId++;

    protected void TypeStart(Type visitType, TypeMolder typeMold, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            ((CurrentNode?.GraphDepth ?? -1) + 1
           , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, formatFlags
           , IndentLevel, Settings.IndentSize, CurrentStyledTypeFormatter, GraphBuilder.GraphEncoder
           , GraphBuilder.ParentGraphEncoder);
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, visitType, typeMold.IsComplexType
                                        , null, Sb!.Length, fmtState)
        {
            TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)typeMold).MoldState
        };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    protected void TypeStart<T>(T toStyle, TypeMolder typeMold, Type typeOfT, FormatFlags formatFlags)
    {
        GraphNodeVisit newVisit;
        var fmtState = new FormattingState
            ((CurrentNode?.GraphDepth ?? -1) + 1
           , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, formatFlags
           , IndentLevel, Settings.IndentSize, CurrentStyledTypeFormatter, GraphBuilder.GraphEncoder
           , GraphBuilder.ParentGraphEncoder);
        if (typeOfT.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(toStyle);

            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(T), false
                                        , storageContainer, Sb!.Length, fmtState);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(T), typeMold.IsComplexType
               , typeOfT.IsValueType ? null : toStyle, Sb!.Length, fmtState)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)typeMold).MoldState
                };
        }
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    private void StartNewVisit(TypeMolder newType, GraphNodeVisit newVisit)
    {
        newType.StartTypeOpening();
        newVisit = newVisit.SetBufferFirstFieldStart(Sb!.Length);

        if (!IsExemptFromCircularRefNodeTracking(newType.TypeBeingBuilt)) OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex = newVisit.ObjVisitIndex;
        newType.FinishTypeOpening();
        nextTypeAppendSettings = new MoldDieCastSettings(SkipTypeParts.None);
    }

    public bool IsExemptFromCircularRefNodeTracking(Type typeStarted)
    {
        return typeStarted.IsString() || typeStarted.IsStringBuilder() || typeStarted.IsCharArray()
            || typeStarted.IsCharSequence() || typeStarted.IsArrayOf(typeof(Rune));
    }

    protected void PopCurrentSettings(bool finishedAsComplex)
    {
        var currentNode = CurrentNode;
        if (currentNode != null)
        {
            OrderedObjectGraph[CurrentGraphNodeIndex] = currentNode.Value.MarkContentEndClearComponentAccess(WriteBuffer.Length, finishedAsComplex);
            CurrentGraphNodeIndex                     = currentNode.Value.ParentVisitIndex;
            if (CurrentGraphNodeIndex < 0) { OrderedObjectGraph.Clear(); }
        }
    }

    private int SourceGraphVisitRefId<T>(T toStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || formatFlags.HasNoRevisitCheck() || IsLastVisitedObject(toStyle)) return 0;
        if (toStyle is object objToStyle)
        {
            for (var i = 0; i < OrderedObjectGraph.Count; i++)
            {
                var graphNodeVisit = OrderedObjectGraph[i];
                if (HasVisited(objToStyle, type, graphNodeVisit))
                {
                    if (graphNodeVisit.RefId == 0)
                    {
                        OrderedObjectGraph[i] = graphNodeVisit.SetRefId(NextRefId());
                        InsertRefId(OrderedObjectGraph[i], i);
                    }
                    return OrderedObjectGraph[i].RefId;
                }
            }
        }
        return 0;
    }

    private int SourceGraphVisitRefId(object toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || formatFlags.HasNoRevisitCheck()) return 0;
        for (var i = 0; i < OrderedObjectGraph.Count; i++)
        {
            var graphNodeVisit = OrderedObjectGraph[i];
            if (HasVisited(toStyleInstance, type, graphNodeVisit))
            {
                if (graphNodeVisit.RefId == 0)
                {
                    OrderedObjectGraph[i] = graphNodeVisit.SetRefId(NextRefId());
                    InsertRefId(OrderedObjectGraph[i], i);
                }
                return OrderedObjectGraph[i].RefId;
            }
        }
        return 0;
    }

    private int IndexOfInstanceVisitFromEnd<T>(T toStyle, Type type)
    {
        if (toStyle is object objToStyle)
            for (var i = OrderedObjectGraph.Count - 1; i >= 0; i--)
            {
                var graphNodeVisit = OrderedObjectGraph[i];
                if (HasVisited(objToStyle, type, graphNodeVisit)) { return i; }
            }
        return -1;
    }

    protected bool HasVisited<TVisited>(TVisited toCheck, Type checkType, GraphNodeVisit checkExisting)
    {
        if (checkExisting.CreateWithFlags.HasNoRevisitCheck()) return false;
        var hasVisited = false;
        if (!checkType.IsValueType && toCheck is object checkObj)
        {
            var checkRef = checkExisting.VisitedInstance;
            if (checkRef == null) return false;
            hasVisited = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, toCheck) : Equals(checkRef, toCheck);
            if (hasVisited) hasVisited = !IsCallingAsBaseType(checkObj, checkType, checkExisting);
        }
        else if (checkExisting.VisitedInstance is RecyclableContainer<TVisited> structContainer) { hasVisited = structContainer.Equals(toCheck); }
        return hasVisited;
    }

    public bool IsLastVisitedObject<TVisited>(TVisited checkIsLastVisited)
    {
        if (OrderedObjectGraph.Count == 0 || checkIsLastVisited == null) return false;
        var graphNodeVisit = OrderedObjectGraph[^1];
        return HasVisited(checkIsLastVisited, typeof(TVisited), graphNodeVisit);
    }

    protected void InsertRefId(GraphNodeVisit forThisNode, int graphNodeIndex)
    {
        var indexToInsertAt = forThisNode.CurrentBufferFirstFieldStart;
        var refId           = forThisNode.RefId;
        var fmtState        = forThisNode.FormattingState;
        var formatter       = forThisNode.FormattingState.Formatter;

        var insertGraphBuilder =
            Recycler.Borrow<GraphTrackingBuilder>()
                    .InitializeInsertBuilder(WriteBuffer, fmtState.IndentLevel, fmtState.IndentChars, fmtState.GraphEncoder, fmtState.ParentEncoder);

        var activeMold = forThisNode.TypeBuilderComponentAccess;
        var charsInserted = 
            formatter.InsertInstanceReferenceId
                ( insertGraphBuilder, refId, indexToInsertAt, forThisNode.IsComplexType
                 , fmtState.CreateWithFlags, forThisNode.CurrentBufferTypeEnd, activeMold);

        insertGraphBuilder.DecrementRefCount();
        if (charsInserted == 0) return;
        if(activeMold != null) activeMold.WroteRefId = true;
        for (int i = graphNodeIndex; i < OrderedObjectGraph.Count; i++)
        {
            var shiftCharsNode = OrderedObjectGraph[i];
            OrderedObjectGraph[i] = shiftCharsNode.ShiftTypeBufferIndex(charsInserted);
        }
    }

    protected bool IsCallingAsBaseType(object objToStyle, Type objAsType, GraphNodeVisit startToLast)
    {
        for (var i = startToLast.ObjVisitIndex; i < OrderedObjectGraph.Count; i++)
        {
            var checkExisting  = OrderedObjectGraph[i];
            var checkRef       = checkExisting.VisitedInstance;
            var isSameInstance = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, objToStyle) : Equals(checkRef, objToStyle);
            if (isSameInstance)
            {
                if (checkExisting.VistedAsType == objAsType || !checkExisting.VistedAsType.IsAssignableTo(objAsType)) { return false; }
            }
        }
        return true;
    }

    protected virtual IStringBuilder SourceStringBuilder() => Sb ??= BufferFactory();

    protected virtual void ClearStringBuilder() => Sb = null!;

    public override void StateReset()
    {
        ClearStringBuilder();

        base.StateReset();
    }

    public override TheOneString Clone() => Recycler.Borrow<TheOneString>().CopyFrom(this, CopyMergeFlags.FullReplace);

    public override ITheOneString CopyFrom(ITheOneString source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((TheOneString)source, copyMergeFlags);

    public TheOneString CopyFrom
        (TheOneString source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Settings.Values = source.Settings.Values;
        ClearAndReinitialize(Settings, IndentLevel);
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

    protected record struct FormattingState
    (
        int GraphDepth
      , int RemainingGraphDepth
      , FormatFlags CreateWithFlags
      , int IndentLevel
      , int IndentChars
      , IStyledTypeFormatting Formatter
      , IEncodingTransfer? GraphEncoder
      , IEncodingTransfer? ParentEncoder
    );

    protected record struct GraphNodeVisit
    {
        public int RefId { get; private init; }

        public bool IsValueTYpe;

        public GraphNodeVisit(int ObjVisitIndex
          , int ParentVisitIndex
          , Type VistedAsType
          , bool isComplexType
          , object? VisitedInstance
          , int CurrentBufferTypeStart
          , FormattingState FormattingState
        )
        {
            this.ObjVisitIndex    = ObjVisitIndex;
            this.ParentVisitIndex = ParentVisitIndex;
            this.VistedAsType     = VistedAsType;
            IsValueTYpe           = VistedAsType.IsValueType;
            IsComplexType         = isComplexType;
            this.VisitedInstance  = VisitedInstance;
            if (VisitedInstance is IRecyclableObject recyclableObject) { recyclableObject.IncrementRefCount(); }
            this.CurrentBufferTypeStart = CurrentBufferTypeStart;
            this.FormattingState        = FormattingState;
        }

        public ITypeMolderDieCast? TypeBuilderComponentAccess { get; init; }

        public GraphNodeVisit SetRefId(int newRefId)
        {
            return this with
            {
                RefId = newRefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart
              , CurrentBufferTypeEnd = CurrentBufferTypeEnd
              , IsComplexType  = IsComplexType
            };
        }

        public GraphNodeVisit SetBufferFirstFieldStart(int bufferFirstFieldStart)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = bufferFirstFieldStart
              , IsComplexType  = IsComplexType
            };
        }

        public GraphNodeVisit MarkContentEndClearComponentAccess(int contentEndIndex, bool finishedAsComplex)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = null
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart
              , CurrentBufferTypeEnd = contentEndIndex
              , IsComplexType  = finishedAsComplex
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
              , CurrentBufferTypeEnd = CurrentBufferTypeEnd != -1 ? CurrentBufferTypeEnd + amountToShift : -1
              , IsComplexType  = IsComplexType
            };
        }

        public int CurrentBufferTypeEnd { get; init; } = -1;

        public int CurrentBufferFirstFieldStart { get; init; }
        public int ObjVisitIndex { get; set; }
        public int ParentVisitIndex { get; set; }
        public Type VistedAsType { get; set; }
        public bool IsComplexType { get; set; }
        public object? VisitedInstance { get; set; }
        public int CurrentBufferTypeStart { get; set; }
        public FormattingState FormattingState { get; set; }
        public int GraphDepth => FormattingState.GraphDepth;
        public int IndentLevel => FormattingState.IndentLevel;
        public int RemainingGraphDepth => FormattingState.RemainingGraphDepth;
        public FormatFlags CreateWithFlags => FormattingState.CreateWithFlags;

        public IStyledTypeFormatting? Formatter => FormattingState.Formatter;
        public IEncodingTransfer? GraphEncoder => FormattingState.GraphEncoder;
        public IEncodingTransfer? ParentEncoder => FormattingState.ParentEncoder;

        public void Reset()
        {
            ObjVisitIndex    = 0;
            ParentVisitIndex = 0;
            VistedAsType     = typeof(GraphNodeVisit);
            IsComplexType    = false;
            if (VisitedInstance is IRecyclableObject recyclableObject) { recyclableObject.DecrementRefCount(); }
            VisitedInstance        = null;
            FormattingState        = default;
            CurrentBufferTypeStart = 0;
            IsValueTYpe            = false;
        }

        // ReSharper disable ParameterHidesMember
        // ReSharper disable InconsistentNaming
        public readonly void Deconstruct(out int ObjVisitIndex
              , out int ParentVisitIndex
              , out Type VistedAsType
              , out bool IsComplexType
              , out object? StylingObjInstance
              , out int CurrentBufferTypeStart
              , out FormattingState FormattingState
            )
            // ReSharper restore ParameterHidesMember
            // ReSharper restore InconsistentNaming
        {
            ObjVisitIndex          = this.ObjVisitIndex;
            ParentVisitIndex       = this.ParentVisitIndex;
            VistedAsType           = this.VistedAsType;
            IsComplexType          = this.IsComplexType;
            StylingObjInstance     = this.VisitedInstance;
            CurrentBufferTypeStart = this.CurrentBufferTypeStart;
            FormattingState        = this.FormattingState;
        }
    }
}
