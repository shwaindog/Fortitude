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

    IStyledTypeFormatting CurrentStyledTypeFormatter { get; }

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
    
    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement> toStyle, CreateContext createContext = default);
    
    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement?> toStyle, CreateContext createContext = default)
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

    StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, bool isKeyName, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    bool RegisterVisitedCheckCanContinue<T>(T guest);
    int  EnsureRegisteredVisited<T>(T guest);

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

    protected List<GraphNodeVisit> OrderedObjectGraph = new(16);

    protected IStringBuilder? Sb;
    private   CallerContext   callerContext;

    private StyleOptions? settings = new(new StyleOptionsValue());

    public TheOneString()
    {
        Settings.Style = StringStyle.CompactLog;
    }

    public TheOneString(StringStyle withStyle)
    {
        Settings.Style = withStyle;
    }

    public TheOneString(TheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        initialAppendSettings  = toClone.initialAppendSettings;
        nextTypeAppendSettings = toClone.nextTypeAppendSettings;

        Settings.Style     = toClone.Style;
        Settings.Formatter = CurrentStyledTypeFormatter;
    }

    public TheOneString(ITheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Style     = toClone.Style;
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


    public static StyleOptions DefaultSettings { get; set; } = new(new StyleOptionsValue());

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

    public GraphTrackingBuilder GraphBuilder { get; set; } = new();

    public ICustomStringFormatter Formatter
    {
        get => Settings.Formatter ??= Settings.Formatter = this.ResolveStyleFormatter();
        set => Settings.Formatter = value;
    }

    public IStyledTypeFormatting CurrentStyledTypeFormatter => (IStyledTypeFormatting)Formatter;

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
        Sb             = usingStringBuilder;
        Settings.Style = buildStyle;

        CallerContext = new CallerContext();

        Settings.Formatter              = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.Reset();
        
        
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

        Settings.Formatter              = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.Reset();
        
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

        Settings.Formatter              = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.Reset();
        
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

        Settings.Formatter              = Settings.Formatter ?? this.ResolveStyleFormatter();
        GraphBuilder.Reset();
        
        Settings.StyledTypeFormatter.GraphBuilder = GraphBuilder;

        return ClearVisitHistory();
    }

    public ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        CallerContext = new CallerContext();
        GraphBuilder.Reset();


        return ClearVisitHistory();
    }

    public ITheOneString ClearVisitHistory()
    {
        ClearObjectVisitedGraph();

        return this;
    }

    void ISecretStringOfPower.TypeComplete(ITypeMolderDieCast completeType)
    {
        if (completeType.DecrementRefCount() == 0) { PopCurrentSettings(); }
    }

    public StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, bool isKeyName, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var type           = obj.GetType();
        var existingRefId  = SourceGraphVisitRefId(obj, type);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;

        return existingRefId > 0 || remainingDepth <= 0
            ? StartComplexContentType(obj).AsValueMatch("", obj, formatString, formatFlags).Complete()
            : StartSimpleContentType(obj).AsValueMatch(obj, formatString, formatFlags).Complete();
    }

    public bool RegisterVisitedCheckCanContinue<T>(T guest)
    {
        var type = guest?.GetType() ?? typeof(T);

        var existingRefId = SourceGraphVisitRefId(guest, type);
        if (existingRefId > 0)
        {
            StartComplexContentType(guest).AsStringOrNull("", "").Complete();
            return false;
        }

        GraphNodeVisit newVisit;
        if (type.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(guest);

            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, storageContainer, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, guest, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);
        }

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return true;
    }

    public int EnsureRegisteredVisited<T>(T guest)
    {
        var type = guest?.GetType() ?? typeof(T);

        var firstVisitedIndex = IndexOfInstanceVisitFromEnd(guest, type);
        if (firstVisitedIndex >= 0) { return firstVisitedIndex; }
        GraphNodeVisit newVisit;
        if (type.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(guest);

            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, storageContainer, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, guest, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);
        }

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return newVisit.ObjVisitIndex;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyedCollectionMold>().InitializeKeyValueCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, keyedCollectionBuilder, actualType);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext)
    {
        var actualType = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var existingRefId  = SourceGraphVisitRefId(keyValueContainerInstance, actualType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(keyValueContainerInstance, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(keyValueContainerInstance, keyedCollectionBuilder, actualType);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, simpleOrderedCollectionBuilder, actualType);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, complexOrderedCollectionBuilder, actualType);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement> toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(Span<TElement>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createContext.FormatFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Span<TElement?> toStyle, CreateContext createContext = default)
        where TElement : struct
    {
        var visitType      = typeof(Span<TElement?>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createContext.FormatFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement> toStyle
      , CreateContext createContext = default)
    {
        var visitType      = typeof(ReadOnlySpan<TElement>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createContext.FormatFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(ReadOnlySpan<TElement?> toStyle
      , CreateContext createContext = default)
        where TElement : struct
    {
        var visitType      = typeof(ReadOnlySpan<TElement?>);
        var actualType     = visitType;
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, 0, createContext.FormatFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var actualType     = collectionInstance.GetType();
        var existingRefId  = SourceGraphVisitRefId(collectionInstance, actualType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(collectionInstance, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(collectionInstance, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetComplexTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexTypeBuilder =
            Recycler.Borrow<ComplexPocoTypeMold>().InitializeComplexTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride
               , remainingDepth, typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, complexTypeBuilder, actualType);
        return complexTypeBuilder;
    }

    public SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var appendSettings = GetValueTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleValueBuilder =
            Recycler.Borrow<SimpleContentTypeMold>().InitializeSimpleValueTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, simpleValueBuilder, actualType);
        return simpleValueBuilder;
    }

    public ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var appendSettings = GetValueTypeAppendSettings(toStyle, actualType, typeFormatter, createContext.FormatFlags);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexContentBuilder =
            Recycler.Borrow<ComplexContentTypeMold>().InitializeComplexValueTypeBuilder
                (actualType, this, appendSettings, createContext.NameOverride, remainingDepth
               , typeFormatter, existingRefId, createContext.FormatFlags);
        TypeStart(toStyle, complexContentBuilder, actualType);
        return complexContentBuilder;
    }

    ITheOneString ISecretStringOfPower.AddBaseFieldsEnd()
    {
        PopCurrentSettings();

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
        nextDieSettings.SkipTypeParts |= formatter.GetNextComplexTypePartFlags(this, forValue, actualType, formatFlags);
        return nextDieSettings;
    }

    public MoldDieCastSettings GetComplexTypeAppendSettings<TElement>(ReadOnlySpan<TElement> forValue, Type actualType
      , IStyledTypeFormatting formatter, FormatFlags formatFlags)
    {
        var nextDieSettings = AppendSettings;
        nextDieSettings.SkipTypeParts |= formatter.GetNextComplexTypePartFlags(this, forValue, actualType, formatFlags);
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

    protected void TypeStart<TElement>(Span<TElement> toStyle, TypeMolder newType, Type typeOfT)
    {
        GraphNodeVisit newVisit;
            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(Span<TElement>), newType.IsComplexType
               , null, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).MoldState
                };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(toStyle, newType, newVisit);
    }

    protected void TypeStart<TElement>(ReadOnlySpan<TElement> toStyle, TypeMolder newType, Type typeOfT)
    {
        GraphNodeVisit newVisit;
            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(ReadOnlySpan<TElement>), newType.IsComplexType
               , null, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).MoldState
                };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(toStyle, newType, newVisit);
    }

    protected void TypeStart<T>(T toStyle, TypeMolder newType, Type typeOfT)
    {
        GraphNodeVisit newVisit;
        if (typeOfT.IsValueType)
        {
            var storageContainer = Recycler.Borrow<RecyclableContainer<T>>().Initialize(toStyle);

            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(T), false, storageContainer, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);

            storageContainer.DecrementRefCount();
        }
        else
        {
            newVisit = new GraphNodeVisit
                (OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(T), newType.IsComplexType
               , typeOfT.IsValueType ? null : toStyle, (CurrentNode?.GraphDepth ?? -1) + 1
               , IndentLevel, Sb!.Length
               , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).MoldState
                };
        }
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(toStyle, newType, newVisit);
    }

    private void StartNewVisit<TElement>(Span<TElement> toStyle, TypeMolder newType, GraphNodeVisit newVisit)
    {
        newType.Start();
        newVisit = newVisit.SetBufferFirstFieldStart(Sb!.Length);

        if (!IsExemptFromCircularRefNodeTracking(newType.TypeBeingBuilt)) OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;
        nextTypeAppendSettings = new MoldDieCastSettings(SkipTypeParts.None);
    }

    private void StartNewVisit<TElement>(ReadOnlySpan<TElement> toStyle, TypeMolder newType, GraphNodeVisit newVisit)
    {
        newType.Start();
        newVisit = newVisit.SetBufferFirstFieldStart(Sb!.Length);

        if (!IsExemptFromCircularRefNodeTracking(newType.TypeBeingBuilt)) OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;
        nextTypeAppendSettings = new MoldDieCastSettings(SkipTypeParts.None);
    }

    private void StartNewVisit<T>(T toStyle, TypeMolder newType, GraphNodeVisit newVisit)
    {
        newType.Start();
        newVisit = newVisit.SetBufferFirstFieldStart(Sb!.Length);

        if (!IsExemptFromCircularRefNodeTracking(newType.TypeBeingBuilt)) OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;
        nextTypeAppendSettings = new MoldDieCastSettings(SkipTypeParts.None);
    }

    public bool IsExemptFromCircularRefNodeTracking(Type typeStarted)
    {
        return typeStarted.IsString() || typeStarted.IsStringBuilder() || typeStarted.IsCharArray()
            || typeStarted.IsCharSequence() || typeStarted.IsArrayOf(typeof(Rune));
    }

    protected void PopCurrentSettings()
    {
        var currentNode = CurrentNode;
        if (currentNode != null)
        {
            OrderedObjectGraph[CurrentGraphNodeIndex] = currentNode.Value.ClearComponentAccess();
            CurrentGraphNodeIndex                     = currentNode.Value.ParentVisitIndex;
            if (CurrentGraphNodeIndex < 0) { OrderedObjectGraph.Clear(); }
        }
    }

    private int SourceGraphVisitRefId<T>(T toStyle, Type type)
    {
        if (type.IsValueType || IsLastVisitedObject(toStyle)) return 0;
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

    private int SourceGraphVisitRefId(object toStyleInstance, Type type)
    {
        if (type.IsValueType) return 0;
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
        var hasVisited = false;
        if (!checkType.IsValueType && toCheck is object checkObj)
        {
            var checkRef = checkExisting.StylingObjInstance;
            hasVisited = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, toCheck) : Equals(checkRef, toCheck);
            if (hasVisited) hasVisited = !IsCallingAsBaseType(checkObj, checkType, checkExisting);
        }
        else if (checkExisting.StylingObjInstance is RecyclableContainer<TVisited> structContainer) { hasVisited = structContainer.Equals(toCheck); }
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
        var refDigitsCount =
            refId switch
            {
                _ when refId < 10   => 1
              , _ when refId < 100  => 2
              , _ when refId < 1000 => 3
              , _                   => 4
            }; //

        Span<char> idSpan = stackalloc char[refDigitsCount + 8];
        idSpan.Append("\"$id\":\"");
        var insert = idSpan[7..];
        if (refId.TryFormat(insert, out _, ""))
        {
            idSpan.Append("\"");
            var shiftBy = idSpan.Length;
            Sb!.InsertAt(idSpan, indexToInsertAt, shiftBy);
            shiftBy += CurrentStyledTypeFormatter.InsertFieldSeparatorAt(Sb!, indexToInsertAt + idSpan.Length, Settings
                                                                       , forThisNode.IndentLevel + 1);
            for (int i = graphNodeIndex; i < OrderedObjectGraph.Count; i++)
            {
                var shiftCharsNode = OrderedObjectGraph[i];
                OrderedObjectGraph[i] = shiftCharsNode.ShiftTypeBufferIndex(shiftBy);
            }
        }
        else
        {
            Debugger.Break();
            Console.Out.WriteLine("Error could not add $ref:" + refId + " to existing object");
        }
    }

    protected bool IsCallingAsBaseType(object objToStyle, Type objAsType, GraphNodeVisit startToLast)
    {
        for (var i = startToLast.ObjVisitIndex; i < OrderedObjectGraph.Count; i++)
        {
            var checkExisting  = OrderedObjectGraph[i];
            var checkRef       = checkExisting.StylingObjInstance;
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

    protected record struct GraphNodeVisit
    {
        public int RefId { get; private init; }

        public bool IsValueTYpe;

        public GraphNodeVisit(int ObjVisitIndex
          , int ParentVisitIndex
          , Type VistedAsType
          , bool isComplexType
          , object? StylingObjInstance
          , int GraphDepth
          , int IndentLevel
          , int CurrentBufferTypeStart
          , int RemainingGraphDepth)
        {
            this.ObjVisitIndex      = ObjVisitIndex;
            this.ParentVisitIndex   = ParentVisitIndex;
            this.VistedAsType       = VistedAsType;
            IsComplexType           = isComplexType;
            this.StylingObjInstance = StylingObjInstance;
            if (StylingObjInstance is IRecyclableObject recyclableObject) { recyclableObject.IncrementRefCount(); }
            this.GraphDepth             = GraphDepth;
            this.IndentLevel            = IndentLevel;
            this.CurrentBufferTypeStart = CurrentBufferTypeStart;
            this.RemainingGraphDepth    = RemainingGraphDepth;
            IsValueTYpe                 = VistedAsType.IsValueType;
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
              , CurrentBufferTypeEnd = CurrentBufferTypeEnd
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
            };
        }

        public int CurrentBufferTypeEnd { get; init; } = -1;

        public int CurrentBufferFirstFieldStart { get; init; }
        public int ObjVisitIndex { get; set; }
        public int ParentVisitIndex { get; set; }
        public Type VistedAsType { get; set; }
        public bool IsComplexType { get; set; }
        public object? StylingObjInstance { get; set; }
        public int GraphDepth { get; set; }
        public int IndentLevel { get; set; }
        public int CurrentBufferTypeStart { get; set; }
        public int RemainingGraphDepth { get; set; }

        public void Reset()
        {
            ObjVisitIndex    = 0;
            ParentVisitIndex = 0;
            VistedAsType     = typeof(GraphNodeVisit);
            IsComplexType    = false;
            if (StylingObjInstance is IRecyclableObject recyclableObject) { recyclableObject.DecrementRefCount(); }
            StylingObjInstance     = null;
            GraphDepth             = 0;
            IndentLevel            = 0;
            CurrentBufferTypeStart = 0;
            RemainingGraphDepth    = 0;
            IsValueTYpe            = false;
        }

        public readonly void Deconstruct(out int ObjVisitIndex
          , out int ParentVisitIndex
          , out Type VistedAsType
          , out bool IsComplexType
          , out object? StylingObjInstance
          , out int GraphDepth
          , out int IndentLevel
          , out int CurrentBufferTypeStart
          , out int RemainingGraphDepth)
        {
            ObjVisitIndex          = this.ObjVisitIndex;
            ParentVisitIndex       = this.ParentVisitIndex;
            VistedAsType           = this.VistedAsType;
            IsComplexType          = this.IsComplexType;
            StylingObjInstance     = this.StylingObjInstance;
            GraphDepth             = this.GraphDepth;
            IndentLevel            = this.IndentLevel;
            CurrentBufferTypeStart = this.CurrentBufferTypeStart;
            RemainingGraphDepth    = this.RemainingGraphDepth;
        }
    }
}
