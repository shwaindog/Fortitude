// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
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
    IRecycler Recycler { get; }
    
    new CallerContext CallerContext { get; set; }

    GraphTrackingBuilder? CurrentGraphBuilder { get; }
    
    GraphInstanceRegistry ActiveGraphRegistry { get; }

    void SetCallerFormatFlags(FormatFlags callerContentHandler);
    void SetCallerFormatString(string? formatString);

    bool        IsExemptFromCircularRefNodeTracking(Type typeStarted);
    
    VisitResult SourceGraphVisitRefId(object? toStyleInstance, Type type, FormatFlags formatFlags);

    StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    void TypeComplete(ITypeMolderDieCast completeType);

    void UpdateVisitWriteMethod(int registryId, int visitIndex, WriteMethodType newWriteMethod);
    void UpdateVisitAddFormatFlags(int registryId, int visitIndex, FormatFlags flagsToAdd);
    void UpdateVisitRemoveFormatFlags(int registryId, int visitIndex, FormatFlags flagsToRemove);
    void UpdateVisitEncoders(int registryId, int visitIndex, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder);

    int  GetNextVisitedReferenceId(int instanceRegistryIndex);
    void RemoveVisitAt(int registryId, int visitIndex);
    int  InstanceIdAtVisit(int registryId, int visitIndex);
    // void  UpdateTypeMold(int registryId, int visitIndex);
    void PopLastAsStringInstanceRegistry();

    ITheOneString AddBaseFieldsStart();
}

public readonly struct CallContextDisposable : IDisposable
{
    private readonly bool                   shouldSkip;
    private readonly ISecretStringOfPower  stringMaster;
    private readonly StyleOptions?          toRestoreOnDispose;
    private readonly IStyledTypeFormatting? expectedPostCallFormatter;
    private readonly FormatFlags           contextChangeRequestFlags;

    public CallContextDisposable(ISecretStringOfPower stringMaster
      , bool shouldSkip
      , FormatFlags contextChangeCallerFlags
      , StyleOptions? toRestoreOnDispose = null
      , IStyledTypeFormatting? expectedPostCallFormatter = null)
    {
        this.shouldSkip                = shouldSkip;
        contextChangeRequestFlags      = contextChangeCallerFlags;
        this.stringMaster              = stringMaster;
        this.toRestoreOnDispose        = toRestoreOnDispose;
        this.expectedPostCallFormatter = expectedPostCallFormatter;
        ((IRecyclableObject?)toRestoreOnDispose)?.IncrementRefCount();
    }

    private bool ShouldPopFormatter => expectedPostCallFormatter != null;
    public bool ShouldSkip => shouldSkip;

    
    public bool HasFormatChange
    {
        [DebuggerStepThrough]
        get => toRestoreOnDispose != null || contextChangeRequestFlags.HasAsStringContentFlag();
    }

    public void Dispose()
    {
        if (toRestoreOnDispose != null)
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
        if (contextChangeRequestFlags.HasAsStringContentFlag())
        {
            stringMaster.PopLastAsStringInstanceRegistry();
        }
    }
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    public static int PropertyNameDefaultBufferSize = 64;

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static readonly Func<IStringBuilder> BufferFactory = () => StaticAlwaysRecycler.Borrow<MutableString>();

    protected readonly GraphInstanceRegistry RootGraphInstanceVisitRegistry;

    private int asStringEnteredCount = -1;

    protected ReusableList<GraphInstanceRegistry>? AsStringInstanceVisitRegistry;


    private FormatFlags nextTypeCreateFlags = DefaultCallerTypeFlags;


    protected IStringBuilder? Sb;

    private CallerContext callerContext;

    private static readonly object NeverEqual = float.NaN;

    private StyleOptions? settings = new(new StyleOptionsValue());

    public TheOneString()
    {
        if (DefaultSettings == null!)
        {
            Settings.Style     = StringStyle.CompactLog;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
        else
        {
            Settings.Values    = DefaultSettings.Values;
            Settings.Formatter = CurrentStyledTypeFormatter;
        }
        RootGraphInstanceVisitRegistry = AlwaysRecycler.Borrow<GraphInstanceRegistry>().Initialize(this);
    }

    public TheOneString(StringStyle withStyle)
    {
        if (DefaultSettings == null!)
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
        RootGraphInstanceVisitRegistry = AlwaysRecycler.Borrow<GraphInstanceRegistry>().Initialize(this);
    }

    public TheOneString(TheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.Sb);
        nextTypeCreateFlags = toClone.nextTypeCreateFlags;

        Settings.Values    = DefaultSettings.Values;
        Settings.Formatter = DefaultSettings.Formatter;

        RootGraphInstanceVisitRegistry = AlwaysRecycler.Borrow<GraphInstanceRegistry>().Initialize(this);
    }

    public TheOneString(ITheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Values    = toClone.Settings.Values;
        Settings.Formatter = CurrentStyledTypeFormatter;

        RootGraphInstanceVisitRegistry = AlwaysRecycler.Borrow<GraphInstanceRegistry>().Initialize(this);
    }

    public ITheOneString Initialize(StringStyle buildStyle = StringStyle.CompactLog)
    {
        if (Sb?.Length != 0)
        {
            Sb?.DecrementRefCount();
            Sb = SourceStringBuilder();
        }

        Settings.Style = buildStyle;


        RootGraphInstanceVisitRegistry.ClearObjectVisitedGraph();

        AsStringInstanceVisitRegistry?.Clear();
        AsStringInstanceVisitRegistry?.DecrementRefCount();
        AsStringInstanceVisitRegistry = null;
        asStringEnteredCount          = -1;
        
        Settings.Formatter   = CurrentStyledTypeFormatter;

        return this;
    }

    protected ITypeMolderDieCast? CurrentTypeAccess => MyActiveGraphRegistry.CurrentNode?.TypeBuilderComponentAccess;


    public StyleOptions Settings
    {
        get => settings ??= AlwaysRecycler.Borrow<StyleOptions>();
        set
        {
            if (ReferenceEquals(settings, value)) return;
            if (settings != null!) { ((IRecyclableObject)settings).DecrementRefCount(); }
            settings = value;
            ((IRecyclableObject)settings).IncrementRefCount();
        }
    }

    public static StyleOptions DefaultSettings { get; set; } = new();

    public int IndentLevel
    {
        get => Settings.IndentLevel;
        set => Settings.IndentLevel = value;
    }

    public override IRecycler Recycler => base.Recycler ?? StaticAlwaysRecycler;

    public StringStyle Style => Settings.Values.Style;

    public IStringBuilder WriteBuffer => Sb ??= BufferFactory();

    public TypeMolder? CurrentTypeBuilder => CurrentTypeAccess?.TypeMolder;

    public CallerContext CallerContext
    {
        get => callerContext;
        set => callerContext = value;
    }

    public GraphTrackingBuilder CurrentGraphBuilder
    {
        get => CurrentStyledTypeFormatter.GraphBuilder ??= AlwaysRecycler.Borrow<GraphTrackingBuilder>();
        set => CurrentStyledTypeFormatter.GraphBuilder = value;
    }
    public GraphTrackingBuilder InitializedGraphBuilder(IRecycler? recycler = null) => (recycler ?? AlwaysRecycler).Borrow<GraphTrackingBuilder>();

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
        if (!ReferenceEquals(usingStringBuilder, Sb))
        {
            Sb?.DecrementRefCount();
            Sb = usingStringBuilder;
            usingStringBuilder.IncrementRefCount();
        }

        Settings.Values = DefaultSettings.Values;
        Settings.Style  = buildStyle;

        return ClearSettings();
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
        Sb?.Clear();
        Sb ??= BufferFactory();

        AsStringInstanceVisitRegistry?.Clear();
        AsStringInstanceVisitRegistry?.DecrementRefCount();
        AsStringInstanceVisitRegistry = null;
        asStringEnteredCount          = -1;
        
        return ClearSettings(indentLevel, initialTypeFlags);
    }

    protected ITheOneString FreeStringBuilder(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        Settings.Values = DefaultSettings.Values;

        Sb?.DecrementRefCount();
        Sb = null!;

        return ClearSettings(indentLevel, initialTypeFlags);
    }

    protected ITheOneString ClearSettings(int indentLevel = 0, FormatFlags initialTypeFlags = DefaultCallerTypeFlags)
    {
        nextTypeCreateFlags = initialTypeFlags;

        IndentLevel = indentLevel;

        CallerContext = CallerContext.Clear();

        var checkFormatter = CurrentStyledTypeFormatter;
        CurrentGraphBuilder.StateReset();
        checkFormatter.Initialize(this);


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
        checkFormatter.Initialize(this);


        // Console.Out.WriteLine($"Using formatter {Formatter}");
        AsStringInstanceVisitRegistry?.Clear();
        AsStringInstanceVisitRegistry?.DecrementRefCount();
        AsStringInstanceVisitRegistry = null;
        asStringEnteredCount          = -1;
        return ClearVisitHistory();
    }

    public ITheOneString ClearVisitHistory()
    {
        MyActiveGraphRegistry.ClearObjectVisitedGraph();

        // Console.Out.WriteLine($"Using formatter {Formatter}");
        return this;
    }

    void ISecretStringOfPower.TypeComplete(ITypeMolderDieCast completeType)
    {
        var completeVisitDetails = completeType.MoldGraphVisit;
        var visitRegId           = completeVisitDetails.RegistryId;
        var visitIndex =  completeVisitDetails.CurrentVisitIndex;
        if (completeType.DecrementRefCount() == 0 && visitRegId == asStringEnteredCount && visitIndex >= 0)
        {
            PopCurrentSettings(completeVisitDetails);
        }
    }

    public StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var type           = obj.GetType();
        var existingRefId  = MySourceGraphVisitRefId(obj, type, formatFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;

        return existingRefId.IsARevisit || remainingDepth <= 0
            ? StartComplexContentType(obj).AsValueMatch("", obj, formatString, formatFlags).Complete()
            : StartSimpleContentType(obj).AsValueMatch(obj, formatString, formatFlags).Complete();
    }

    protected object WrapOrReturnSubjectAsObject<T>(T subject)
    {
        var type = subject?.GetType() ?? typeof(T);
        if (type.IsValueType) { return AlwaysRecycler.Borrow<RecyclableContainer<T>>().Initialize(subject); }
        return subject!;
    }

    public RawContentMold EnsureRegisteredClassIsReferenceTracked<T>(T toStyle, CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        if (visitType.IsValueType) throw new ArgumentException("Expected toStyle to be a class not a value type");
        var actualType = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter     = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, RawContent, visitResult, createFlags);
        var remainingDepth    = MyActiveGraphRegistry.RemainingDepth - 1;
        var writeMethod = (visitResult.IsARevisit || remainingDepth <= 0) && mergedCreateFlags.DoesNotHaveSuppressOpening()
            ? MoldComplexType
            : RawContent;
        var simpleValueBuilder =
            AlwaysRecycler.Borrow<RawContentMold>().InitializeRawContentTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, createFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, createFlags);
        return simpleValueBuilder;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldKeyedCollectionType
                                                               , visitResult, createFlags);
        var writeMethod    = MoldKeyedCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            AlwaysRecycler.Borrow<KeyedCollectionMold>().InitializeKeyValueCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType  = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(keyValueContainerInstance, actualType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(keyValueContainerInstance, actualType, typeFormatter, MoldExplicitKeyedCollectionType
                                                               , visitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (keyValueContainerInstance, actualType, this, actualType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, mergedCreateFlags);
        TypeStart(keyValueContainerInstance, actualType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldSimpleCollectionType
                                                               , visitResult, createFlags);
        var writeMethod    = MoldSimpleCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var simpleOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexCollectionType, visitResult
                                                               , createFlags);
        var writeMethod    = MoldSimpleCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle, CreateContext createContext = default)
    {
        var createFlags   = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(actualType, typeFormatter, MoldExplicitCollectionType, VisitResult.VisitNotChecked, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , VisitResult.VisitNotChecked, writeMethod, mergedCreateFlags);
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
                                                               , VisitResult.VisitNotChecked, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , VisitResult.VisitNotChecked, writeMethod, mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldExplicitCollectionType
                                                               , visitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var actualType  = collectionInstance.GetType();
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(collectionInstance, actualType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags = GetFormatterContentHandlingFlags(collectionInstance, actualType, typeFormatter, MoldExplicitCollectionType
                                                               , visitResult, createFlags);
        var writeMethod    = MoldExplicitCollectionType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (collectionInstance, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(collectionInstance, actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
            : VisitResult.VisitNotChecked;
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexType, visitResult, createFlags);
        var writeMethod    = MoldComplexType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexTypeBuilder =
            AlwaysRecycler.Borrow<ComplexPocoTypeMold>().InitializeComplexTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexTypeBuilder, writeMethod, mergedCreateFlags);
        return complexTypeBuilder;
    }

    public SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        // Content types always start out with an empty visit and only when the value being added is known usually the next call. Will the visit
        // be recorded.  This is because AsString content can be tracked on a separate registry and we don't want to update the currently
        // active graph if this is the case.  
        // So not doing ...
        // var visitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
        //     ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
        //     : VisitResult.Empty;
        var visitResult   = VisitResult.VisitNotChecked;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldSimpleContentType, visitResult, createFlags);
        var writeMethod    = MoldSimpleContentType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var simpleValueBuilder =
            AlwaysRecycler.Borrow<SimpleContentTypeMold>().InitializeSimpleValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, mergedCreateFlags);
        return simpleValueBuilder;
    }

    public ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        // Content types always start out with an empty visit and only when the value being added is known usually the next call. Will the visit
        // be recorded.  This is because AsString content can be tracked on a separate registry and we don't want to update the currently
        // active graph if this is the case. 
        // So not doing ...
        // var visitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
        //     ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
        //     : VisitResult.Empty;
        var visitResult   = VisitResult.VisitNotChecked;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var mergedCreateFlags
            = GetFormatterContentHandlingFlags(toStyle, actualType, typeFormatter, MoldComplexContentType, visitResult, createFlags);
        var writeMethod    = MoldComplexContentType;
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexContentBuilder =
            AlwaysRecycler.Borrow<ComplexContentTypeMold>().InitializeComplexValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
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


    protected void TypeStart(Type visitType, TypeMolder typeMold, WriteMethodType writeMethod, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            (MyActiveGraphRegistry.CurrentDepth + 1
           , MyActiveGraphRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentStyledTypeFormatter.ContentEncoder
           , CurrentStyledTypeFormatter.LayoutEncoder);

        var newVisit =
            new GraphNodeVisit
                (asStringEnteredCount, MyActiveGraphRegistry.NextFreeSlot, MyActiveGraphRegistry.CurrentGraphNodeIndex
               , visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , null, Sb!.Length, IndentLevel, CallerContext, fmtState, formatFlags
               , typeMold.MoldVisit.LastRevisitCount + 1)
                {
                    TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)typeMold).MoldState
                };
        if (newVisit.ObjVisitIndex != MyActiveGraphRegistry.Count)
            throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    protected void TypeStart<T>(T toStyle, Type visitType, TypeMolder typeMold, WriteMethodType writeMethod, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            (MyActiveGraphRegistry.CurrentDepth + 1
           , MyActiveGraphRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentStyledTypeFormatter.ContentEncoder
           , CurrentStyledTypeFormatter.LayoutEncoder);

        var wrapped = WrapOrReturnSubjectAsObject(toStyle);

        var newVisit =
            new GraphNodeVisit
                (asStringEnteredCount, MyActiveGraphRegistry.NextFreeSlot, MyActiveGraphRegistry.CurrentGraphNodeIndex
               , toStyle?.GetType() ?? visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , wrapped, Sb!.Length, IndentLevel, CallerContext, fmtState, formatFlags
               , typeMold.MoldVisit.LastRevisitCount + 1);

        if (newVisit.ObjVisitIndex != MyActiveGraphRegistry.Count)
            throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(typeMold, newVisit);
    }

    private void StartNewVisit(TypeMolder newType, GraphNodeVisit newVisit)
    {
        if (!newType.MoldVisit.NoVisitCheckDone) { MyActiveGraphRegistry.Add(newVisit); }
        MyActiveGraphRegistry.CurrentGraphNodeIndex = newVisit.ObjVisitIndex;
        newType.StartTypeOpening();
        if (!newType.MoldVisit.NoVisitCheckDone)
            MyActiveGraphRegistry[newVisit.ObjVisitIndex] = newVisit.SetBufferFirstFieldStart(Sb!.Length, CurrentStyledTypeFormatter.Gb.IndentLevel);
        
        newType.FinishTypeOpening();
        nextTypeCreateFlags = DefaultCallerTypeFlags;
    }

    public bool IsExemptFromCircularRefNodeTracking(Type typeStarted)
    {
        return typeStarted.IsValueType
            || (typeStarted.IsString() && !Settings.InstanceTrackingIncludeStringInstances)
            || (typeStarted.IsStringBuilder() && !Settings.InstanceTrackingIncludeStringBuilderInstances) 
            || (typeStarted.IsCharSequence() && !Settings.InstanceTrackingIncludeCharSequenceInstances)
            || (typeStarted.IsCharArray() && !Settings.InstanceTrackingIncludeCharArrayInstances)
            || (typeStarted.IsArrayOf(typeof(Rune)) && !Settings.InstanceTrackingIncludeCharArrayInstances);
    }

    public CallContextDisposable ResolveContextForCallerFlags(FormatFlags contentFlags)
    {
        var previousStyle  = Settings.Style;
        var shouldContinue = ContinueGivenFormattingFlags(contentFlags);
        if (!shouldContinue) return new CallContextDisposable(this, true, contentFlags & ~AsStringContent);
        if (contentFlags.HasAsStringContentFlag()) RunAsStringInstanceTrackingChecks();
        if (Settings.IsSame(contentFlags)
         && !(previousStyle.IsJson()
           && contentFlags.HasAsStringContentFlag()
           && CurrentStyledTypeFormatter.LayoutEncoder.Type != EncodingType.JsonEncoding)
         && CurrentStyledTypeFormatter.LayoutEncoder.Type == CurrentStyledTypeFormatter.LayoutEncoder.LayoutEncoder.Type)
        {
            CurrentStyledTypeFormatter.AddedContextOnThisCall = false;
            return new CallContextDisposable(this, false, contentFlags);
        }
        Settings.IfExistsIncrementFormatterRefCount();
        var nextContextOptions = AlwaysRecycler.Borrow<StyleOptions>().Initialize(Settings);

        var oldFormatter = Settings.StyledTypeFormatter;
        var existingOptions =
            new CallContextDisposable
                (this, false, contentFlags, Settings, oldFormatter);

        Settings = nextContextOptions;

        Settings.Style = contentFlags.UpdateStringStyle(nextContextOptions);
        if (contentFlags.HasAsStringContentFlag())
        {
            if( nextContextOptions.AsStringSeparateRestartedIndentation) nextContextOptions.IndentLevel = 0;
        }
        var nextContextFormatter = CurrentStyledTypeFormatter.ContextStartPushToNext(nextContextOptions);
        nextContextFormatter.Initialize(this);
        nextContextFormatter.AddedContextOnThisCall = true;
        nextContextFormatter.PreviousContext        = oldFormatter;

        if (Settings.Style.IsJson()
         && contentFlags.HasAsStringContentFlag()
         && CurrentStyledTypeFormatter.LayoutEncoder.Type != EncodingType.JsonEncoding)
        {
            var newContentEncoder = this.ResolveStyleEncoder(EncodingType.JsonEncoding);
            newContentEncoder                   = newContentEncoder.WithAttachedLayoutEncoder(oldFormatter.ContentEncoder);
            nextContextFormatter.ContentEncoder = newContentEncoder;
        }
        else if (Settings.Style == previousStyle &&
                 CurrentStyledTypeFormatter.LayoutEncoder.Type != CurrentStyledTypeFormatter.LayoutEncoder.LayoutEncoder.Type)
        {
            nextContextFormatter.AddedContextOnThisCall = false;
            var newContentEncoder = this.ResolveStyleEncoder(EncodingType.JsonEncoding);
            newContentEncoder                   = newContentEncoder.WithAttachedLayoutEncoder(oldFormatter.ContentEncoder);
            nextContextFormatter.ContentEncoder = newContentEncoder;
        }

        Settings.Formatter = nextContextFormatter;
        // above increments RefCount so we need to decrement here so it is recycled
        nextContextFormatter.DecrementRefCount();

        return existingOptions;
    }

    public VisitResult UpdateIfRevisitIgnored(VisitResult fullResult, Type typeStarted, FormatFlags formatFlags)
    {
        var shouldIgnore = formatFlags.HasNoRevisitCheck() && fullResult.LastRevisitCount < 64 // todo update this from settings
                        || (typeStarted.IsSpanFormattableCached() && !Settings.InstanceTrackingIncludeSpanFormattableClasses);
        return fullResult.WithIsARevisitSetTo(!shouldIgnore & fullResult.InstanceId > 0 && !fullResult.IsBaseOfInitial);
    }

    protected void PopCurrentSettings(VisitResult visitIndex)
    {
        if (MyActiveGraphRegistry.RegistryId != visitIndex.RegistryId 
         || visitIndex.CurrentVisitIndex >= MyActiveGraphRegistry.Count)
        {
            Debugger.Break();
        }
        var currentActiveNode = MyActiveGraphRegistry.CurrentGraphNodeIndex;
        var currentNode = MyActiveGraphRegistry[visitIndex.CurrentVisitIndex];
        MyActiveGraphRegistry[currentNode.ObjVisitIndex] =
            currentNode.MarkContentEndClearComponentAccess(WriteBuffer.Length, currentNode.WriteMethod);
        if (visitIndex.CurrentVisitIndex == currentActiveNode)
        {
            MyActiveGraphRegistry.CurrentGraphNodeIndex = currentNode.ParentVisitIndex;    
        }
        if (MyActiveGraphRegistry.CurrentGraphNodeIndex < 0) { MyActiveGraphRegistry.ClearObjectVisitedGraph(); }
    }

    VisitResult ISecretStringOfPower.SourceGraphVisitRefId(object? toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return new VisitResult();
        var registrySearchResult = MyActiveGraphRegistry.SourceGraphVisitRefId(toStyleInstance, type, formatFlags);
        var updatedResult        = UpdateIfRevisitIgnored(registrySearchResult, type, formatFlags);
        if (updatedResult.IsARevisit && !MyActiveGraphRegistry[registrySearchResult.FirstInstanceMatchVisitIndex].HasInsertedInstanceId)
        {
            InsertRefId(MyActiveGraphRegistry[registrySearchResult.FirstInstanceMatchVisitIndex], registrySearchResult.FirstInstanceMatchVisitIndex);
        }
        return registrySearchResult;
    }

    private VisitResult MySourceGraphVisitRefId<T>(T toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return new VisitResult();
        return ((ISecretStringOfPower)this).SourceGraphVisitRefId(toStyleInstance, type, formatFlags);
    }


    protected void InsertRefId(GraphNodeVisit forThisNode, int graphNodeIndex)
    {
        var indexToInsertAt = forThisNode.CurrentBufferExpectedFirstFieldStart;
        // note: empty types might have a CurrentBufferFirstFieldStart greater than CurrentBufferTypeEnd as empty padding is removed
        var refId           = forThisNode.RefId;
        var fmtState        = forThisNode.FormattingState;
        var formatter       = forThisNode.FormattingState.Formatter.Clone();
        var contentEncoder  = forThisNode.ContentEncoder.Clone();
        var layoutEncoder   = forThisNode.LayoutEncoder.Clone();
        var combinedEncoder = contentEncoder.WithAttachedLayoutEncoder(layoutEncoder);
        formatter.ContentEncoder = combinedEncoder;

        var insertGraphBuilder =
            AlwaysRecycler.Borrow<GraphTrackingBuilder>()
                          .InitializeInsertBuilder(formatter, WriteBuffer, forThisNode.IndentLevel, fmtState.IndentChars);

        var activeMold = forThisNode.TypeBuilderComponentAccess;
        var charsInserted =
            formatter.InsertInstanceReferenceId
                (insertGraphBuilder, forThisNode.ActualType, refId, indexToInsertAt, forThisNode.WriteMethod
               , forThisNode.CurrentFormatFlags, forThisNode.CurrentBufferTypeEnd, activeMold);
        MyActiveGraphRegistry[graphNodeIndex] = forThisNode.SetHasInsertedRefId(true);
        insertGraphBuilder.DecrementRefCount();
        formatter.DecrementRefCount();
        contentEncoder.DecrementRefCount();
        layoutEncoder.DecrementRefCount();
        
        if (!ReferenceEquals(contentEncoder, combinedEncoder)) { combinedEncoder.DecrementRefCount(); }
        if (charsInserted == 0) return;
        if (activeMold != null) activeMold.WroteRefId = true;
        for (int i = graphNodeIndex; i < MyActiveGraphRegistry.Count; i++)
        {
            var shiftCharsNode = MyActiveGraphRegistry[i];
            MyActiveGraphRegistry[i] = shiftCharsNode.ShiftTypeBufferIndex(charsInserted);
        }
    }

    public bool IsCallerSameInstanceAndMoreDerived<TVisited>(TVisited checkIsLastVisited)
    {
        if (MyActiveGraphRegistry.Count == 0
         || checkIsLastVisited == null
         || (MyActiveGraphRegistry.CurrentNode is not { ParentVisitIndex: >= 0 }))
            return false;
        var graphNodeVisit = MyActiveGraphRegistry[MyActiveGraphRegistry.CurrentNode.Value.ParentVisitIndex];
        var wasLastSameObjectAndOrMoreDerivedRef = MyActiveGraphRegistry.WasVisitOnSameInstance(checkIsLastVisited, graphNodeVisit)
                                                && MyActiveGraphRegistry.WasVisitOnAMoreDerivedType(typeof(TVisited), graphNodeVisit);
        return wasLastSameObjectAndOrMoreDerivedRef;
    }

    public void UpdateVisitWriteMethod(int registryId, int visitIndex, WriteMethodType newWriteMethod)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return;
        forRegistry.UpdateVisitWriteMethod(visitIndex, newWriteMethod);
    }

    public void UpdateVisitAddFormatFlags(int registryId, int visitIndex, FormatFlags flagsToAdd)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return;
        forRegistry.UpdateVisitAddFormatFlags(visitIndex, flagsToAdd);
    }

    public void UpdateVisitRemoveFormatFlags(int registryId, int visitIndex, FormatFlags flagsToRemove)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return;
        forRegistry.UpdateVisitRemoveFormatFlags(visitIndex, flagsToRemove);
    }

    public void UpdateVisitEncoders(int registryId, int visitIndex, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return;
        forRegistry.UpdateVisitEncoders(visitIndex, contentEncoder, layoutEncoder);
    }

    public int InstanceIdAtVisit(int registryId, int visitIndex)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return -1;
        return forRegistry.InstanceIdAtVisit(visitIndex);
    }

    public void RemoveVisitAt(int registryId, int visitIndex)
    {
        var forRegistry = GetInstanceRegistry(registryId);
        if (forRegistry == null || forRegistry.Count < visitIndex) return;
        forRegistry.RemoveVisitAt(visitIndex);
    }

    public int GetNextVisitedReferenceId(int instanceRegistryIndex)
    {
        if (instanceRegistryIndex >= 0
         && AsStringInstanceVisitRegistry != null
         && AsStringInstanceVisitRegistry.Count > instanceRegistryIndex
         && Settings.InstanceMarkingAsStringIndependentNumbering)
        {
            return AsStringInstanceVisitRegistry[instanceRegistryIndex].ThisRegistryNextRefId++;
        }
        return RootGraphInstanceVisitRegistry.ThisRegistryNextRefId++;
    }

    protected void RunAsStringInstanceTrackingChecks()
    {
        if (Settings.InstanceTrackingAllAsStringHaveLocalTracking)
        {
            asStringEnteredCount++;
            AsStringInstanceVisitRegistry ??= AlwaysRecycler.Borrow<ReusableList<GraphInstanceRegistry>>();
            if (AsStringInstanceVisitRegistry.Count != asStringEnteredCount)
            {
                Debugger.Break();
                Console.Out.WriteLine("Error should never start out greater than the new string we just entered!");
            }
            var newRegistry = AlwaysRecycler.Borrow<GraphInstanceRegistry>().Initialize(this, asStringEnteredCount);
            AsStringInstanceVisitRegistry.Add(newRegistry);
        }
    }

    public void PopLastAsStringInstanceRegistry()
    {
        if (asStringEnteredCount >= 0 && AsStringInstanceVisitRegistry != null)
        {
            var lastRegistry = AsStringInstanceVisitRegistry![^1];
            AsStringInstanceVisitRegistry.RemoveAt(AsStringInstanceVisitRegistry.Count - 1);
            lastRegistry.DecrementRefCount();
            asStringEnteredCount--;
        }
        if (AsStringInstanceVisitRegistry is { Count: 0 })
        {
            AsStringInstanceVisitRegistry.DecrementRefCount();
            AsStringInstanceVisitRegistry = null;
        }
    }

    GraphInstanceRegistry ISecretStringOfPower.ActiveGraphRegistry => 
        GetInstanceRegistry(asStringEnteredCount) 
     ?? throw new NullReferenceException("Must not still point to a registry that has been removed");
    
    protected GraphInstanceRegistry MyActiveGraphRegistry
    {
        [DebuggerStepThrough]
        get => ((ISecretStringOfPower)this).ActiveGraphRegistry;
    }

    protected GraphInstanceRegistry? GetInstanceRegistry(int registryId)
    {
        if (registryId < 0) return RootGraphInstanceVisitRegistry;
        if (AsStringInstanceVisitRegistry != null && AsStringInstanceVisitRegistry.Count > registryId)
            return AsStringInstanceVisitRegistry[registryId];
        return null;
    }
    
    protected virtual IStringBuilder SourceStringBuilder() => Sb ??= BufferFactory();

    public override void StateReset()
    {
        RootGraphInstanceVisitRegistry.ClearObjectVisitedGraph();
        AsStringInstanceVisitRegistry?.Clear();
        AsStringInstanceVisitRegistry?.DecrementRefCount();
        AsStringInstanceVisitRegistry = null;
        asStringEnteredCount          = -1;
        FreeStringBuilder();

        base.StateReset();
    }

    public override TheOneString Clone() => AlwaysRecycler.Borrow<TheOneString>().CopyFrom(this, CopyMergeFlags.FullReplace);

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
