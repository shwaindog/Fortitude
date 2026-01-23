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
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeCommon.Types.StringsOfPower.WriteMethodType;
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
        (StringStyle style, int indentLevel = 0, FormatFlags initialTypeCreateFlags = DefaultCallerTypeFlags);

    ITheOneString ClearAndReinitialize
        (StyleOptions styleOptions, int indentLevel = 0, FormatFlags initialTypeCreateFlags = DefaultCallerTypeFlags);

    ITheOneString ClearAndReinitialize
        (StyleOptionsValue styleOptions, int indentLevel = 0, FormatFlags initialTypeCreateFlags = DefaultCallerTypeFlags);
    
    ITheOneString ClearKeepSettings(int indentLevel = 0, FormatFlags initialTypeCreateFlags = DefaultCallerTypeFlags);

    ITheOneString ClearAndToDefaultSettings(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags);

    ITheOneString Clear(int indentLevel = 0, FormatFlags initialTypeCreateFlags = DefaultCallerTypeFlags);

    ITheOneString ClearVisitHistory();

    KeyedCollectionMold StartKeyedCollectionType<T>(T toStyle, CreateContext createContext = default);

    ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext = default);

    SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default);

    RawContentMold EnsureRegisteredClassIsReferenceTracked<T>(T toStyle, CreateContext createContext);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle
      , CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionTypeOfNullable<TElement>(Type typeOfToStyle
      , CreateContext createContext = default)
        where TElement : struct;

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object toStyle, CreateContext createContext = default);

    ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default);

    ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default);

    SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default);

    ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default);
    CallContextDisposable  ResolveContextForCallerFlags(FormatFlags contentFlags);

    bool ContinueGivenFormattingFlags(FormatFlags contentFlags);

    bool IsCallerSameInstanceAndMoreDerived<TVisited>(TVisited checkIsLastVisited);

    IStringBuilder WriteBuffer { get; }

    GraphTrackingBuilder InitializedGraphBuilder(IRecycler? recycler = null);

    bool Equals(string? toCompare);

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public interface ISecretStringOfPower : ITheOneString
{
    new IRecycler Recycler { get; }

    new CallerContext CallerContext { get; set; }

    GraphTrackingBuilder? CurrentGraphBuilder { get; }

    void SetCallerFormatFlags(FormatFlags callerContentHandler);
    void SetCallerFormatString(string? formatString);

    StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    void TypeComplete(ITypeMolderDieCast completeType);

    void UpdateVisitWriteMethod(int visitIndex, WriteMethodType newWriteMethod);
    void UpdateVisitAddFormatFlags(int visitIndex, FormatFlags flagsToAdd);
    void UpdateVisitRemoveFormatFlags(int visitIndex, FormatFlags flagsToRemove);

    ITheOneString AddBaseFieldsStart();
}

public readonly record struct StyleFormattingState
    (IStyledTypeFormatting StyleFormatter, IEncodingTransfer StringEncoder, IEncodingTransfer GraphEncoder, IEncodingTransfer ParentGraphEncoder) { }

public readonly struct CallContextDisposable : IDisposable
{
    private readonly bool                   shouldSkip;
    private readonly ISecretStringOfPower?  stringMaster;
    private readonly StyleOptions?          toRestoreOnDispose;
    private readonly IStyledTypeFormatting? expectedPostCallFormatter;

    public CallContextDisposable(bool shouldSkip
      , ISecretStringOfPower? stringMaster = null
      , StyleOptions? toRestoreOnDispose = null
      , IStyledTypeFormatting? expectedPostCallFormatter = null)
    {
        this.shouldSkip                = shouldSkip;
        this.stringMaster              = stringMaster;
        this.toRestoreOnDispose        = toRestoreOnDispose;
        this.expectedPostCallFormatter = expectedPostCallFormatter;
        ((IRecyclableObject?)toRestoreOnDispose)?.IncrementRefCount();
    }

    private bool ShouldPopFormatter => expectedPostCallFormatter != null;
    public bool ShouldSkip => shouldSkip;

    public bool HasFormatChange => toRestoreOnDispose != null;

    public void Dispose()
    {
        if (toRestoreOnDispose != null && stringMaster != null)
        {
            IStyledTypeFormatting? previousFormatter = null;
            if (ShouldPopFormatter)
            {
                previousFormatter = stringMaster.CurrentStyledTypeFormatter.ContextCompletePopToPrevious();

                if (!ReferenceEquals(previousFormatter, expectedPostCallFormatter)) { Debugger.Break(); }
            }
            stringMaster.Settings.CopyFrom(toRestoreOnDispose);
            if (previousFormatter != null) stringMaster.CurrentStyledTypeFormatter = previousFormatter;
            ((IRecyclableObject)toRestoreOnDispose).DecrementRefCount();
        }
    }
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    public static int PropertyNameDefaultBufferSize = 64;

    private static readonly IRecycler AlWaysRecycler = new Recycler();

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static readonly Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    protected GraphInstanceRegistry InstanceVisitRegistry;

    private FormatFlags nextTypeCreateFlags = DefaultCallerTypeFlags;


    protected IStringBuilder? Sb;

    private CallerContext callerContext;

    private static readonly object NeverEqual = float.NaN;

    private StyleOptions? settings = new(new StyleOptionsValue());

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
        InstanceVisitRegistry = new GraphInstanceRegistry(this);
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
        InstanceVisitRegistry = new GraphInstanceRegistry(this);
    }

    public TheOneString(TheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        nextTypeCreateFlags = toClone.nextTypeCreateFlags;

        Settings.Values    = DefaultSettings.Values;
        Settings.Formatter = DefaultSettings.Formatter;
        
        InstanceVisitRegistry = new GraphInstanceRegistry(this);
    }

    public TheOneString(ITheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Values    = toClone.Settings.Values;
        Settings.Formatter = CurrentStyledTypeFormatter;

        InstanceVisitRegistry = new GraphInstanceRegistry(this);
    }

    public ITheOneString Initialize(StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        Settings.Style = buildStyle;


        InstanceVisitRegistry.ClearObjectVisitedGraph();
        Settings.Formatter = CurrentStyledTypeFormatter;

        return this;
    }

    protected ITypeMolderDieCast? CurrentTypeAccess => InstanceVisitRegistry.CurrentNode?.TypeBuilderComponentAccess;


    public StyleOptions Settings
    {
        get => settings ??= Recycler.Borrow<StyleOptions>();
        set
        {
            if (ReferenceEquals(settings, value)) return;
            if (settings != null!) { ((IRecyclableObject)settings).DecrementRefCount(); }
            settings = value;
            ((IRecyclableObject)settings).IncrementRefCount();
        }
    }

    public static StyleOptions DefaultSettings { get; set; } = new ();

    public int IndentLevel
    {
        get => Settings.IndentLevel;
        set => Settings.IndentLevel = value;
    }

    public StringStyle Style => Settings.Values.Style;

    public IStringBuilder WriteBuffer => Sb ??= BufferFactory();

    public new IRecycler Recycler => base.Recycler ?? AlWaysRecycler;

    public TypeMolder? CurrentTypeBuilder => CurrentTypeAccess?.TypeMolder;

    public CallerContext CallerContext
    {
        get => callerContext;
        set => callerContext = value;
    }

    public GraphTrackingBuilder CurrentGraphBuilder
    {
        get => CurrentStyledTypeFormatter.GraphBuilder ?? Recycler.Borrow<GraphTrackingBuilder>().Initialize(Settings, WriteBuffer);
        set => CurrentStyledTypeFormatter.GraphBuilder = value;
    }
    public GraphTrackingBuilder InitializedGraphBuilder(IRecycler? recycler = null) => (recycler ?? Recycler).Borrow<GraphTrackingBuilder>();

    public ICustomStringFormatter Formatter
    {
        get => Settings.Formatter ??= this.ResolveStyleFormatter();
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

    public ITheOneString ReInitialize(IStringBuilder usingStringBuilder, StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();

        Sb = usingStringBuilder;

        Settings.Values = DefaultSettings.Values;
        Settings.Style = buildStyle;
        
        return Clear();
    }

    public ITheOneString ClearAndReinitialize
        (StringStyle style, int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        Settings.Values = DefaultSettings.Values;
        Settings.Style  = style;
        
        return Clear(indentLevel, initialTypeFlags);
    }

    public ITheOneString ClearAndReinitialize(StyleOptions styleOptions, int indentLevel = 0
      , FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        Settings = styleOptions;

        return Clear(indentLevel, initialTypeFlags);
    }

    public ITheOneString ClearAndReinitialize(StyleOptionsValue styleOptionsValue, int indentLevel = 0
      , FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        Settings.Values = styleOptionsValue;

        return Clear(indentLevel, initialTypeFlags);
    }

    public ITheOneString Clear(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        nextTypeCreateFlags = initialTypeFlags;
        Sb?.Clear();

        Sb ??= BufferFactory();

        IndentLevel = indentLevel;

        CallerContext = CallerContext.Clear();

        var checkFormatter = CurrentStyledTypeFormatter;
        CurrentGraphBuilder.StateReset();
        checkFormatter.Initialize(CurrentGraphBuilder, Settings, Sb);


        // Console.Out.WriteLine($"Using formatter {Formatter}");
        return ClearVisitHistory();
    }

    public ITheOneString ClearAndToDefaultSettings(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        Settings.Values = DefaultSettings.Values;
        
        return Clear(indentLevel, initialTypeFlags);
    }

    public ITheOneString ClearKeepSettings(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        nextTypeCreateFlags = initialTypeFlags;
        Sb?.Clear();

        Sb ??= BufferFactory();

        IndentLevel = indentLevel;

        CallerContext = CallerContext.Clear();

        var checkFormatter = CurrentStyledTypeFormatter;
        CurrentGraphBuilder.StateReset();
        checkFormatter.Initialize(CurrentGraphBuilder, Settings, Sb);


        // Console.Out.WriteLine($"Using formatter {Formatter}");
        return ClearVisitHistory();
    }

    public ITheOneString ClearVisitHistory()
    {
        InstanceVisitRegistry.ClearObjectVisitedGraph();

        // Console.Out.WriteLine($"Using formatter {Formatter}");
        return this;
    }

    void ISecretStringOfPower.TypeComplete(ITypeMolderDieCast completeType)
    {
        var finishedAsComplex = completeType.SupportsMultipleFields;
        if (completeType.DecrementRefCount() == 0) { PopCurrentSettings(finishedAsComplex); }
    }

    public StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var type           = obj.GetType();
        var existingRefId  = SourceGraphVisitRefId(obj, type, formatFlags);
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;

        return existingRefId.HasExistingInstanceId || remainingDepth <= 0
            ? StartComplexContentType(obj).AsValueMatch("", obj, formatString, formatFlags).Complete()
            : StartSimpleContentType(obj).AsValueMatch(obj, formatString, formatFlags).Complete();
    }

    protected object WrapOrReturnSubjectAsObject<T>(T subject)
    {
        var type = subject?.GetType() ?? typeof(T);
        if (type.IsValueType) { return Recycler.Borrow<RecyclableContainer<T>>().Initialize(subject); }
        return subject!;
    }

    public RawContentMold EnsureRegisteredClassIsReferenceTracked<T>(T toStyle, CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        if (visitType.IsValueType) throw new ArgumentException("Expected toStyle to be a class not a value type");
        var actualType = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter     = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, RawContent, graphVisitResult, createFlags);
        var remainingDepth    = InstanceVisitRegistry.RemainingDepth - 1;
        var writeMethod = (graphVisitResult.HasExistingInstanceId || remainingDepth <= 0) && mergedCreateFlags.DoesNotHaveSuppressOpening()
            ? MoldComplexType
            : RawContent;
        var simpleValueBuilder =
            Recycler.Borrow<RawContentMold>().InitializeRawContentTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, createFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, createFlags);
        return simpleValueBuilder;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldKeyedCollectionType
                                                               , graphVisitResult, createFlags);
        var writeMethod    = MoldKeyedCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyedCollectionMold>().InitializeKeyValueCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride
               , remainingDepth, graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType  = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(keyValueContainerInstance, actualType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(keyValueContainerInstance, actualType, typeFormatter, MoldExplicitKeyedCollectionType
                                                               , graphVisitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (keyValueContainerInstance, actualType, this, createContext.NameOverride
               , remainingDepth, graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(keyValueContainerInstance, actualType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldSimpleCollectionType
                                                               , graphVisitResult, createFlags);
        var writeMethod    = MoldSimpleCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexCollectionType, graphVisitResult
                                                               , createFlags);
        var writeMethod    = MoldSimpleCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle, CreateContext createContext = default)
    {
        var createFlags   = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(actualType, typeFormatter, MoldExplicitCollectionType, VisitResult.Empty, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, createContext.NameOverride, remainingDepth
               , VisitResult.Empty, writeMethod, mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionTypeOfNullable<TElement>(Type typeOfToStyle
      , CreateContext createContext = default)
        where TElement : struct
    {
        var createFlags   = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(actualType, actualType, typeFormatter, MoldExplicitCollectionType
                                                               , VisitResult.Empty, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, createContext.NameOverride, remainingDepth
               , VisitResult.Empty, writeMethod, mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldExplicitCollectionType
                                                               , graphVisitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType  = collectionInstance.GetType();
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(collectionInstance, actualType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(collectionInstance, actualType, typeFormatter, MoldExplicitCollectionType
                                                               , graphVisitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (collectionInstance, actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(collectionInstance, actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter     = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexType, graphVisitResult, createFlags);
        var writeMethod       = MoldComplexType;
        var remainingDepth    = InstanceVisitRegistry.RemainingDepth - 1;
        var complexTypeBuilder =
            Recycler.Borrow<ComplexPocoTypeMold>().InitializeComplexTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride
               , remainingDepth, graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexTypeBuilder, writeMethod, mergedCreateFlags);
        return complexTypeBuilder;
    }

    public SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldSimpleContentType, graphVisitResult, createFlags);
        var writeMethod    = MoldSimpleContentType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var simpleValueBuilder =
            Recycler.Borrow<SimpleContentTypeMold>().InitializeSimpleValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, mergedCreateFlags);
        return simpleValueBuilder;
    }

    public ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var graphVisitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
            ? SourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.Empty;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexContentType, graphVisitResult, createFlags);
        var writeMethod    = MoldComplexContentType;
        var remainingDepth = InstanceVisitRegistry.RemainingDepth - 1;
        var complexContentBuilder =
            Recycler.Borrow<ComplexContentTypeMold>().InitializeComplexValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, createContext.NameOverride, remainingDepth
               , graphVisitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexContentBuilder, writeMethod, mergedCreateFlags);
        return complexContentBuilder;
    }

    ITheOneString ISecretStringOfPower.AddBaseFieldsStart()
    {
        nextTypeCreateFlags = SuppressOpening | LogSuppressTypeNames | SuppressClosing;

        return this;
    }

    public FormatFlags GetFormatterContentHandlingFlags(Type forType, IStyledTypeFormatting formatter, WriteMethodType proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var nextCreateFlags =
            formatter.GetFormatterContentHandlingFlags
                (this, forType, proposedWriteType, visitResult, nextTypeCreateFlags | formatFlags);
        return nextCreateFlags;
    }

    public FormatFlags GetFormatterContentHandlingFlags<T>(T forValue, Type actualType, IStyledTypeFormatting formatter
      , WriteMethodType proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var nextCreateFlags
            = formatter.GetFormatterContentHandlingFlags
                (this, forValue, actualType, proposedWriteType, visitResult, nextTypeCreateFlags | formatFlags);
        return nextCreateFlags;
    }

    public bool ContinueGivenFormattingFlags(FormatFlags contentFlags)
    {
        var shouldSkip = false;
        if ((contentFlags & StyleMask) > 0)
        {
            shouldSkip |= Settings.Style.IsLog() && contentFlags.HasExcludeWhenLogStyle();
            shouldSkip |= Settings.Style.IsJson() && contentFlags.HasExcludeWhenJsonStyle();
            shouldSkip |= Settings.Style.IsCompact() && contentFlags.HasExcludeWhenCompactLayout();
            shouldSkip |= Settings.Style.IsPretty() && contentFlags.HasExcludeWhenPrettyLayout();
        }
        return !shouldSkip;
    }

    public CallContextDisposable ResolveContextForCallerFlags(FormatFlags contentFlags)
    {
        var previousStyle  = Settings.Style;
        var shouldContinue = ContinueGivenFormattingFlags(contentFlags);
        if (!shouldContinue) return new CallContextDisposable(true);
        if (Settings.IsSame(contentFlags)
         && !(previousStyle.IsJson()
           && contentFlags.HasAsStringContentFlag()
           && CurrentGraphBuilder.GraphEncoder.Type != EncodingType.JsonEncoding)
         && CurrentGraphBuilder.GraphEncoder.Type == CurrentGraphBuilder.ParentGraphEncoder.Type)
        {
            CurrentStyledTypeFormatter.AddedContextOnThisCall = false;
            return new CallContextDisposable(false);
        }
        Settings.IfExistsIncrementFormatterRefCount();
        var nextContextOptions = Recycler.Borrow<StyleOptions>().Initialize(Settings);

        var oldFormatter = Settings.StyledTypeFormatter;
        var existingOptions =
            new CallContextDisposable
                (false, this, Settings, oldFormatter);

        Settings = nextContextOptions;

        Settings.Style = contentFlags.UpdateStringStyle(Settings.Style);
        var nextContextFormatter = CurrentStyledTypeFormatter.ContextStartPushToNext();

        Settings.Formatter = nextContextFormatter;

        nextContextFormatter.AddedContextOnThisCall = true;
        nextContextFormatter.PreviousContext        = oldFormatter;

        if (Settings.Style.IsJson()
         && contentFlags.HasAsStringContentFlag()
         && CurrentGraphBuilder.GraphEncoder.Type != EncodingType.JsonEncoding)
        {
            var gb = nextContextFormatter.GraphBuilder!;

            gb.GraphEncoder = Settings.StyledTypeFormatter.ContentEncoder.Type == EncodingType.JsonEncoding
                ? nextContextFormatter.ContentEncoder
                : this.ResolveStyleEncoder(EncodingType.JsonEncoding);
        }
        else if (Settings.Style == previousStyle && CurrentGraphBuilder.ParentGraphEncoder != CurrentGraphBuilder.GraphEncoder)
        {
            nextContextFormatter.AddedContextOnThisCall = false;
            var gb = CurrentStyledTypeFormatter.GraphBuilder!;
            gb.ParentGraphEncoder = gb.GraphEncoder;
        }

        return existingOptions;
    }


    protected void TypeStart(Type visitType, TypeMolder typeMold, WriteMethodType writeMethod, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            (InstanceVisitRegistry.CurrentDepth + 1
           , InstanceVisitRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentGraphBuilder.GraphEncoder
           , CurrentGraphBuilder.ParentGraphEncoder);

        var newVisit =
            new GraphNodeVisit
                (InstanceVisitRegistry.Count, InstanceVisitRegistry.CurrentGraphNodeIndex
               , visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , null, Sb!.Length, IndentLevel, CallerContext, fmtState, formatFlags
               , typeMold.MoldVisit.LastRevisitCount + 1)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)typeMold).MoldState
                };
        if (newVisit.ObjVisitIndex != InstanceVisitRegistry.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    protected void TypeStart<T>(T toStyle, Type visitType, TypeMolder typeMold, WriteMethodType writeMethod, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            (InstanceVisitRegistry.CurrentDepth + 1
           , InstanceVisitRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentGraphBuilder.GraphEncoder
           , CurrentGraphBuilder.ParentGraphEncoder);

        var wrapped = WrapOrReturnSubjectAsObject(toStyle);

        var newVisit =
            new GraphNodeVisit
                (InstanceVisitRegistry.Count, InstanceVisitRegistry.CurrentGraphNodeIndex
               , toStyle?.GetType() ?? visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , wrapped, Sb!.Length, IndentLevel, CallerContext, fmtState, formatFlags
               , typeMold.MoldVisit.LastRevisitCount + 1);

        if (newVisit.ObjVisitIndex != InstanceVisitRegistry.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    private void StartNewVisit(TypeMolder newType, GraphNodeVisit newVisit)
    {
        newType.StartTypeOpening();
        newVisit = newVisit.SetBufferFirstFieldStart(Sb!.Length, IndentLevel);

        if (!newType.MoldVisit.IsEmpty) { InstanceVisitRegistry.Add(newVisit); }

        InstanceVisitRegistry.CurrentGraphNodeIndex = newVisit.ObjVisitIndex;
        newType.FinishTypeOpening();
        nextTypeCreateFlags = DefaultCallerTypeFlags;
    }

    public bool IsExemptFromCircularRefNodeTracking(Type typeStarted, FormatFlags formatFlags)
    {
        return typeStarted.IsValueType
            || formatFlags.HasNoRevisitCheck()
            || (Settings.InstanceTrackingAllAsStringClassesAreExempt && formatFlags.HasAsStringContentFlag())   
            || typeStarted.IsString()
            || typeStarted.IsStringBuilder()
            || typeStarted.IsCharArray()
            || typeStarted.IsCharSequence()
            || (typeStarted.IsSpanFormattableCached()
             && !Settings.InstanceTrackingIncludeSpanFormattableClasses)
            || typeStarted.IsArrayOf(typeof(Rune));
    }

    protected void PopCurrentSettings(bool finishedAsComplex)
    {
        var currentNode = InstanceVisitRegistry.CurrentNode;
        if (currentNode != null)
        {
            InstanceVisitRegistry[InstanceVisitRegistry.CurrentGraphNodeIndex] =
                currentNode.Value
                           .MarkContentEndClearComponentAccess(WriteBuffer.Length, currentNode.Value.WriteMethod);
            InstanceVisitRegistry.CurrentGraphNodeIndex = currentNode.Value.ParentVisitIndex;
            if (InstanceVisitRegistry.CurrentGraphNodeIndex < 0) { InstanceVisitRegistry.ClearObjectVisitedGraph(); }
        }
    }

    private VisitResult SourceGraphVisitRefId<T>(T toStyle, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyle == null || formatFlags.HasNoRevisitCheck()) return new VisitResult();
        return SourceGraphVisitRefId((object)toStyle, type, formatFlags);
    }

    private VisitResult SourceGraphVisitRefId(object toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || formatFlags.HasNoRevisitCheck()) return new VisitResult();
        var registrySearchResult = InstanceVisitRegistry.SourceGraphVisitRefId(toStyleInstance, type, formatFlags);
        if (registrySearchResult.NewlyAssigned)
        {
            InsertRefId(InstanceVisitRegistry[registrySearchResult.FirstVisitIndex], registrySearchResult.FirstVisitIndex);
        }
        return registrySearchResult;
    }


    protected void InsertRefId(GraphNodeVisit forThisNode, int graphNodeIndex)
    {
        var indexToInsertAt = forThisNode.CurrentBufferExpectedFirstFieldStart;
        // note: empty types might have a CurrentBufferFirstFieldStart greater than CurrentBufferTypeEnd as empty padding is removed
        var refId     = forThisNode.RefId;
        var fmtState  = forThisNode.FormattingState;
        var formatter = forThisNode.FormattingState.Formatter;

        var insertGraphBuilder =
            Recycler.Borrow<GraphTrackingBuilder>()
                    .InitializeInsertBuilder(WriteBuffer, forThisNode.IndentLevel, fmtState.IndentChars, fmtState.GraphEncoder
                                           , fmtState.ParentEncoder);

        var activeMold = forThisNode.TypeBuilderComponentAccess;
        var charsInserted =
            formatter.InsertInstanceReferenceId
                (insertGraphBuilder, forThisNode.ActualType, refId, indexToInsertAt, forThisNode.WriteMethod
               , forThisNode.CurrentFormatFlags, forThisNode.CurrentBufferTypeEnd, activeMold);

        insertGraphBuilder.DecrementRefCount();
        if (charsInserted == 0) return;
        if (activeMold != null) activeMold.WroteRefId = true;
        for (int i = graphNodeIndex; i < InstanceVisitRegistry.Count; i++)
        {
            var shiftCharsNode = InstanceVisitRegistry[i];
            InstanceVisitRegistry[i] = shiftCharsNode.ShiftTypeBufferIndex(charsInserted);
        }
    }

    public bool IsCallerSameInstanceAndMoreDerived<TVisited>(TVisited checkIsLastVisited)
    {
        if (InstanceVisitRegistry.Count == 0
         || checkIsLastVisited == null
         || (InstanceVisitRegistry.CurrentNode is not { ParentVisitIndex: >= 0 }))
            return false;
        var graphNodeVisit = InstanceVisitRegistry[InstanceVisitRegistry.CurrentNode.Value.ParentVisitIndex];
        var wasLastSameObjectAndOrMoreDerivedRef = InstanceVisitRegistry.WasVisitOnSameInstance(checkIsLastVisited, graphNodeVisit)
                                                && InstanceVisitRegistry.WasVisitOnAMoreDerivedType(typeof(TVisited), graphNodeVisit);
        return wasLastSameObjectAndOrMoreDerivedRef;
    }

    public void UpdateVisitWriteMethod(int visitIndex, WriteMethodType newWriteMethod)
    {
        if (InstanceVisitRegistry.Count < visitIndex) return;
        InstanceVisitRegistry.UpdateVisitWriteMethod(visitIndex, newWriteMethod);
    }

    public void UpdateVisitAddFormatFlags(int visitIndex, FormatFlags flagsToAdd)
    {
        if (InstanceVisitRegistry.Count < visitIndex) return;
        InstanceVisitRegistry.UpdateVisitAddFormatFlags(visitIndex, flagsToAdd);
    }

    public void UpdateVisitRemoveFormatFlags(int visitIndex, FormatFlags flagsToRemove)
    {
        if (InstanceVisitRegistry.Count < visitIndex) return;
        InstanceVisitRegistry.UpdateVisitRemoveFormatFlags(visitIndex, flagsToRemove);
    }


    protected virtual IStringBuilder SourceStringBuilder() => Sb ??= BufferFactory();

    public override void StateReset()
    {
        ClearAndToDefaultSettings();

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

        nextTypeCreateFlags = DefaultCallerTypeFlags;

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
}
