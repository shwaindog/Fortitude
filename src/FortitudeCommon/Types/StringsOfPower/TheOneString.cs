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
using static FortitudeCommon.Types.StringsOfPower.DieCasting.WrittenAsFlags;
using KeyedCollectionMold = FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.KeyedCollectionMold;

#endregion

namespace FortitudeCommon.Types.StringsOfPower;

public interface ITheOneString : IReusableObject<ITheOneString>
{
    // ReSharper disable UnusedMemberInSuper.Global
    StringStyle Style { get; }

    StyleOptions Settings { get; }

    CallerContext CallerContext { get; }
    CallerContext NextCallContext { get; }

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

    ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default);

    TrackedInstanceMold GetTrackedInstanceMold<T>(T toStyle, FormatFlags innerContentFormatFlags
      , WrittenAsFlags proposedWriteAs = AsRaw, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle
      , CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionTypeOfNullable<TElement>(Type typeOfToStyle
      , CreateContext createContext = default)
        where TElement : struct;

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object toStyle
      , CreateContext createContext = default);

    ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default);

    SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default);

    ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default);
    CallContextDisposable  ResolveContextForCallerFlags(FormatFlags contentFlags);

    bool ContinueGivenFormattingFlags(FormatFlags contentFlags);

    bool IsCallerSameInstanceAndMoreDerived<TVisited>(TVisited checkIsLastVisited);

    bool IsCallerSameAsLastRegisteredVisit<TVisited>(TVisited checkIsLastVisited);

    IStringBuilder WriteBuffer { get; }

    GraphTrackingBuilder InitializedGraphBuilder(IRecycler? recycler = null);

    bool Equals(string? toCompare);

    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public interface ISecretStringOfPower : ITheOneString
{
    new IRecycler Recycler { get; }

    new CallerContext NextCallContext { get; set; }

    GraphTrackingBuilder? CurrentGraphBuilder { get; }

    GraphInstanceRegistry ActiveGraphRegistry { get; }

    void SetCallerFormatFlags(FormatFlags callerContentHandler);
    void SetCallerFormatString(string? formatString);
    void SetCallerWriteAs(WrittenAsFlags writeAs);

    bool IsExemptFromCircularRefNodeTracking(Type typeStarted);

    VisitResult SourceGraphVisitRefIdUpdateGraph(object? toStyleInstance, Type type, Type nextMoldType, FormatFlags formatFlags);
    VisitResult JustSourceGraphVisitResult(object? toStyleInstance, Type type, Type nextMoldType, FormatFlags formatFlags);

    AppendSummary RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    void TypeComplete(IMoldWriteState completeType);

    void SetBufferFirstFieldStartAndIndentLevel(VisitId visitId, int firstFieldStart, int indentLevel);
    void SetBufferFirstFieldStartAndWrittenAs(VisitId visitId, int firstFieldStart, WrittenAsFlags writtenAsFlags);
    void UpdateVisitWriteMethod(VisitId visitId, WrittenAsFlags newWriteMethod);
    void UpdateVisitAddFormatFlags(VisitId visitId, FormatFlags flagsToAdd);
    void UpdateVisitRemoveFormatFlags(VisitId visitId, FormatFlags flagsToRemove);
    void UpdateVisitEncoders(VisitId visitId, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder);
    void UpdateVisitFormatter(VisitId visitId, IStyledTypeFormatting updatedFormatter);

    int  GetNextVisitedReferenceId(int instanceRegistryIndex);
    void RemoveVisitAt(VisitId visitId);
    int  InstanceIdAtVisit(VisitId visitId);
    int  UpdateVisitLinesAndLength(VisitId visitId, int deltaLength, int deltaLineNumber = 0);
    int  GetLatestVisitBufferStartIndex(VisitId visitId);

    void ShiftRegisteredFromCharOffset(int fromChar, int shiftByStart, int shiftEnd);

    void PopLastAsStringInstanceRegistry();

    ITheOneString AddBaseFieldsStart(IMoldWriteState moldWriteState);
}

public readonly struct CallContextDisposable : IDisposable
{
    private readonly bool                 shouldSkip;
    private readonly ISecretStringOfPower stringMaster;
    private readonly StyleOptions?        toRestoreOnDispose;
    private readonly FormatFlags          contextChangeRequestFlags;

    public CallContextDisposable(ISecretStringOfPower stringMaster
      , bool shouldSkip
      , FormatFlags contextChangeCallerFlags
      , StyleOptions? toRestoreOnDispose = null)
    {
        this.shouldSkip           = shouldSkip;
        contextChangeRequestFlags = contextChangeCallerFlags;
        this.stringMaster         = stringMaster;
        this.toRestoreOnDispose   = toRestoreOnDispose;
        ((IRecyclableObject?)toRestoreOnDispose)?.IncrementRefCount();
    }

    public bool ShouldSkip => shouldSkip;


    public bool HasFormatChange
    {
        [DebuggerStepThrough] get => toRestoreOnDispose != null || contextChangeRequestFlags.HasAsStringContentFlag();
    }

    public void Dispose()
    {
        if (toRestoreOnDispose != null)
        {
            var previousFormatter = stringMaster.CurrentStyledTypeFormatter.ContextCompletePopToPrevious();
            stringMaster.Settings.CopyFrom(toRestoreOnDispose);
            stringMaster.CurrentStyledTypeFormatter = previousFormatter;
            ((IRecyclableObject)toRestoreOnDispose).DecrementRefCount();
        }
        if (contextChangeRequestFlags.HasAsStringContentFlag()) { stringMaster.PopLastAsStringInstanceRegistry(); }
    }
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    #pragma warning disable CA2211
    public static int PropertyNameDefaultBufferSize = 64;
    #pragma warning restore CA2211

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static readonly Func<IStringBuilder> BufferFactory = () => StaticAlwaysRecycler.Borrow<MutableString>();

    protected readonly GraphInstanceRegistry RootGraphInstanceVisitRegistry;

    private sbyte asStringEnteredCount = -1;

    protected ReusableList<GraphInstanceRegistry>? AsStringInstanceVisitRegistry;


    private FormatFlags nextTypeCreateFlags = DefaultCallerTypeFlags;


    protected IStringBuilder?      Sb;
    private   GraphTrackingBuilder transferBetweenMolds = new GraphTrackingBuilder();

    private CallerContext nextCallContext;

    internal static readonly object NeverEqual = float.NaN;

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

        Settings.Formatter = CurrentStyledTypeFormatter;

        return this;
    }

    protected IMoldWriteState? CurrentRegisteredTypeAccess => MyActiveGraphRegistry.CurrentNode?.MoldState;


    public StyleOptions Settings
    {
        [DebuggerStepThrough] get => settings ??= AlwaysRecycler.Borrow<StyleOptions>();
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

    public TypeMolder? CurrentTypeBuilder => CurrentRegisteredTypeAccess?.Mold;

    public CallerContext CallerContext => 
        (unregisteredVisitMolds?.Any() ?? false) 
            ? unregisteredVisitMolds[^1].Caller
            : (MyActiveGraphRegistry.CurrentNode?.MoldState?.Caller ?? new CallerContext());

    private List<TypeMolder>? unregisteredVisitMolds;

    public CallerContext NextCallContext
    {
        get => nextCallContext;
        set => nextCallContext = value;
    }

    public GraphTrackingBuilder CurrentGraphBuilder
    {
        get => CurrentStyledTypeFormatter.GraphBuilder ??= AlwaysRecycler.Borrow<GraphTrackingBuilder>();
        set => CurrentStyledTypeFormatter.GraphBuilder = value;
    }
    public GraphTrackingBuilder InitializedGraphBuilder(IRecycler? recycler = null) => (recycler ?? AlwaysRecycler).Borrow<GraphTrackingBuilder>();

    public ICustomStringFormatter Formatter
    {
        [DebuggerStepThrough] get => Settings.Formatter ??= this.ResolveStyleFormatter();
        set => Settings.Formatter = value;
    }

    public IStyledTypeFormatting CurrentStyledTypeFormatter
    {
        [DebuggerStepThrough] get => (IStyledTypeFormatting)Formatter;
        set => Formatter = value;
    }

    public void SetCallerFormatFlags(FormatFlags callerContentHandler)
    {
        nextCallContext.FormatFlags = callerContentHandler;
    }

    public void SetCallerFormatString(string? formatString)
    {
        nextCallContext.FormatString = formatString;
    }

    public void SetCallerWriteAs(WrittenAsFlags writeAs)
    {
        nextCallContext.WriteAs = writeAs;
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

        unregisteredVisitMolds?.Clear();
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

        NextCallContext = NextCallContext.Clear();

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

        NextCallContext = NextCallContext.Clear();

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
                (this, false, contentFlags, Settings);

        Settings = nextContextOptions;

        Settings.Style = contentFlags.UpdateStringStyle(nextContextOptions);
        if (contentFlags.HasAsStringContentFlag())
        {
            if (nextContextOptions.AsStringSeparateRestartedIndentation) nextContextOptions.IndentLevel = 0;
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
            newContentEncoder                   = newContentEncoder.WithAttachedLayoutEncoder(nextContextFormatter.ContentEncoder);
            nextContextFormatter.ContentEncoder = newContentEncoder;
        }
        else if (Settings.Style == previousStyle &&
                 CurrentStyledTypeFormatter.LayoutEncoder.Type != CurrentStyledTypeFormatter.LayoutEncoder.LayoutEncoder.Type)
        {
            nextContextFormatter.AddedContextOnThisCall = false;
            var newContentEncoder = this.ResolveStyleEncoder(EncodingType.JsonEncoding);
            newContentEncoder                   = newContentEncoder.WithAttachedLayoutEncoder(nextContextFormatter.ContentEncoder);
            nextContextFormatter.ContentEncoder = newContentEncoder;
        }

        Settings.Formatter = nextContextFormatter;
        // above increments RefCount so we need to decrement here so it is recycled
        nextContextFormatter.DecrementRefCount();

        return existingOptions;
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

    public AppendSummary RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var type = obj.GetType();


        var existingRefId  = ((ISecretStringOfPower)(this)).JustSourceGraphVisitResult(obj, type, typeof(TrackedInstanceMold), formatFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;

        if (formatFlags.HasAsValueContentFlag())
        {
            return existingRefId.IsARevisit || remainingDepth <= 0
                ? StartComplexContentType(obj).AsValueMatch("", obj, formatString, formatFlags).Complete()
                : StartSimpleContentType(obj).AsValueMatch(obj, formatString, formatFlags).Complete();
        }
        return existingRefId.IsARevisit || remainingDepth <= 0
            ? StartComplexContentType(obj).AsStringMatch("", obj, formatString, formatFlags).Complete()
            : StartSimpleContentType(obj).AsStringMatch(obj, formatString, formatFlags).Complete();
    }

    protected object WrapOrReturnSubjectAsObject<T>(T subject)
    {
        var type = subject?.GetType() ?? typeof(T);
        if (type.IsValueType) { return AlwaysRecycler.Borrow<RecyclableContainer<T>>().Initialize(subject); }
        return subject!;
    }

    public TrackedInstanceMold GetTrackedInstanceMold<T>(T toStyle, FormatFlags innerContentFormatFlags
      , WrittenAsFlags proposedWriteAs = AsRaw, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | proposedWriteAs;
        var visitType   = typeof(T);
        if (visitType.IsValueType) throw new ArgumentException("Expected toStyle to be a class not a value type");
        var actualType = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(TrackedInstanceMold), callFlags).WitReusedCount(-1)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var simpleValueBuilder =
            AlwaysRecycler.Borrow<TrackedInstanceMold>().InitializeRawContentTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags, innerContentFormatFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, mergedCreateFlags);
        return simpleValueBuilder;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsMapCollection;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(KeyedCollectionMold), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            AlwaysRecycler.Borrow<KeyedCollectionMold>().InitializeKeyValueCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , CreateContext createContext)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsMapCollection;
        var actualType  = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(keyValueContainerInstance, actualType, typeof(ExplicitKeyedCollectionMold<TKey, TValue>), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(keyValueContainerInstance, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var keyedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (keyValueContainerInstance, actualType, this, actualType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(keyValueContainerInstance, actualType, keyedCollectionBuilder, writeMethod, mergedCreateFlags);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsSimple | WrittenAsFlags.AsCollection;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;

        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(SimpleOrderedCollectionMold), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var simpleOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsComplex | WrittenAsFlags.AsCollection;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;

        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(ComplexOrderedCollectionMold), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsSimple | WrittenAsFlags.AsCollection;
        var actualType  = collectionInstance.GetType();
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(collectionInstance, actualType, typeof(ExplicitOrderedCollectionMold<TElement>), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(collectionInstance, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (collectionInstance, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(collectionInstance, actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle, CreateContext createContext = default)
    {
        var callFlags     = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs   = NextCallContext.WriteAs | AsSimple | WrittenAsFlags.AsCollection;
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(actualType, typeFormatter, callWriteAs, VisitResult.VisitNotChecked, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId), writeMethod, NextCallContext
               , mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionTypeOfNullable<TElement>(Type typeOfToStyle
      , CreateContext createContext = default) where TElement : struct
    {
        var callFlags     = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs   = NextCallContext.WriteAs | AsSimple | WrittenAsFlags.AsCollection;
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(actualType, actualType, typeFormatter, callWriteAs, VisitResult.VisitNotChecked, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId), writeMethod, NextCallContext
               , mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsSimple | WrittenAsFlags.AsCollection;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(ExplicitOrderedCollectionMold<TElement>), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) =
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsComplex;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, typeof(ComplexPocoTypeMold), callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexTypeBuilder =
            AlwaysRecycler.Borrow<ComplexPocoTypeMold>().InitializeComplexTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride
               , remainingDepth, visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexTypeBuilder, writeMethod, mergedCreateFlags);
        return complexTypeBuilder;
    }

    public SimpleContentTypeMold StartSimpleContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsSimple | AsContent;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        // Content types always start out with an empty visit and only when the value being added is known usually the next call. Will the visit
        // be recorded.  This is because AsString content can be tracked on a separate registry and we don't want to update the currently
        // active graph if this is the case.  
        // So not doing ...
        // var visitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
        //     ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
        //     : VisitResult.Empty;
        var visitResult   = MyActiveGraphRegistry.VisitNotChecked(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, callFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var simpleValueBuilder =
            AlwaysRecycler.Borrow<SimpleContentTypeMold>().InitializeSimpleValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, mergedCreateFlags);
        return simpleValueBuilder;
    }

    public ComplexContentTypeMold StartComplexContentType<T>(T toStyle, CreateContext createContext = default)
    {
        var createFlags = createContext.FormatFlags | NextCallContext.FormatFlags;
        var callWriteAs = NextCallContext.WriteAs | AsComplex | AsContent;
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        // Content types always start out with an empty visit and only when the value being added is known usually the next call. Will the visit
        // be recorded.  This is because AsString content can be tracked on a separate registry and we don't want to update the currently
        // active graph if this is the case. 
        // So not doing ...
        // var visitResult = !IsExemptFromCircularRefNodeTracking(actualType, createFlags)
        //     ? MySourceGraphVisitRefId(toStyle, visitType, createFlags)
        //     : VisitResult.Empty;
        var visitResult   = MyActiveGraphRegistry.VisitNotChecked(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, callWriteAs, visitResult, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexContentBuilder =
            AlwaysRecycler.Borrow<ComplexContentTypeMold>().InitializeComplexValueTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, NextCallContext, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexContentBuilder, writeMethod, mergedCreateFlags);
        return complexContentBuilder;
    }

    ITheOneString ISecretStringOfPower.AddBaseFieldsStart(IMoldWriteState moldWriteState)
    {
        nextTypeCreateFlags = SuppressOpening | LogSuppressTypeNames | SuppressClosing;
        NextCallContext     = NextCallContext.Merge(moldWriteState.Caller);

        return this;
    }

    public (WrittenAsFlags, FormatFlags) GetFormatterWriteAsFormatFlags(Type forType, IStyledTypeFormatting formatter
      , WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var nextCreateFlags =
            formatter.ResolveMoldWriteAsFormatFlags
                (this, forType, proposedWriteType, visitResult, nextTypeCreateFlags | formatFlags);
        return nextCreateFlags;
    }

    public (WrittenAsFlags, FormatFlags) GetFormatterWriteAsFormatFlags<T>(T forValue, Type actualType, IStyledTypeFormatting formatter
      , WrittenAsFlags proposedWriteType
      , VisitResult visitResult, FormatFlags formatFlags)
    {
        var nextCreateWriteAsFormatFlags
            = formatter.ResolveMoldWriteAsFormatFlags
                (this, forValue, actualType, proposedWriteType, visitResult, nextTypeCreateFlags | formatFlags);
        return nextCreateWriteAsFormatFlags;
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
    
    protected void TypeStart(Type visitType, TypeMolder typeMold, WrittenAsFlags writeMethod, FormatFlags formatFlags)
    {
        TypeStart(null, visitType, visitType, typeMold, writeMethod, formatFlags);
    }

    protected void TypeStart<T>(T toStyle, Type visitType, TypeMolder typeMold, WrittenAsFlags writeMethod, FormatFlags formatFlags)
    {
        var wrapped          = WrapOrReturnSubjectAsObject(toStyle);
        TypeStart(wrapped, toStyle?.GetType() ?? typeof(T), visitType, typeMold, writeMethod, formatFlags);
    }

    protected void TypeStart(object? wrappedOrInstance, Type actualType, Type visitType, TypeMolder typeMold, WrittenAsFlags writeMethod, FormatFlags formatFlags)
    {
        var incrementDepthBy = 1;
        var thisVisitId      = typeMold.MoldVisit.VisitId;
        var requesterVisitId = MyActiveGraphRegistry.CurrentGraphNodeVisitId;
        var lastFieldsStart  = -1;
        var moldWriteState   = ((ITypeBuilderComponentSource)typeMold).MoldState;
        var typeStart        = Sb!.Length;
        if (thisVisitId == requesterVisitId)
        {
            var lastVisit = GetNodeVisit(requesterVisitId);
            typeStart = lastVisit.TypeOpenBufferIndex;
            if (lastVisit.FirstFieldBufferIndex > typeStart) { lastFieldsStart = lastVisit.FirstFieldBufferIndex; }

            requesterVisitId = lastVisit.ParentVisitId;
            var lastMoldState = lastVisit.MoldState;
            if (lastMoldState != null)
            {
                var lastMold   = lastMoldState.Mold;
                var lastMoldType = lastMold.GetType();
                if (!(lastMoldType == typeof(TrackedInstanceMold) 
                 || lastMoldType == typeof(SimpleContentTypeMold)
                    || lastMoldType == typeof(ComplexContentTypeMold)))
                {
                    writeMethod = lastMoldState.WroteTypeOpen
                        ? lastMoldState.CreateWriteMethod
                        : lastMoldState.CurrentWriteMethod;
                    moldWriteState.CurrentWriteMethod = writeMethod;
                }
                moldWriteState.WroteTypeName = lastMoldState.WroteTypeName;
                var thisVisitResult   = moldWriteState.MoldGraphVisit;
                var isBaseClass       = thisVisitResult.IsBaseOfInitial;
                var updateVisitResult = lastMoldState.MoldGraphVisit.IncrementUsedCount();
                moldWriteState.MoldWrittenFlags |= lastMoldState.MoldWrittenFlags;
                moldWriteState.MoldGraphVisit =
                    isBaseClass
                        ? updateVisitResult.WitRequesterVisitIdSetTo(thisVisitId)
                        : updateVisitResult;
            }
            incrementDepthBy = 0;
        }
        var fmtState = new FormattingState
            (MyActiveGraphRegistry.CurrentDepth + incrementDepthBy
           , MyActiveGraphRegistry.RemainingDepth - incrementDepthBy, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentStyledTypeFormatter.ContentEncoder
           , CurrentStyledTypeFormatter.LayoutEncoder);

        var parentLineNumber = GetCurrentNodeLineNumber(requesterVisitId.VisitIndex, Settings.NewLineStyle, typeStart);
        var newVisit =
            new GraphNodeVisit
                (thisVisitId, requesterVisitId
               , actualType, visitType
               , moldWriteState, writeMethod
               , wrappedOrInstance, IndentLevel, fmtState, formatFlags
               , typeStart, typeMold.MoldVisit.LastRevisitCount
               , ParentLineNumber: parentLineNumber
               , WrittenFlags: moldWriteState.MoldWrittenFlags);

        var totalNodes = MyActiveGraphRegistry.Count;
        if (newVisit.NodeVisitId.RegistryId >= -1
         && !(newVisit.NodeVisitId.VisitIndex == totalNodes
           || newVisit.NodeVisitId.VisitIndex == totalNodes - 1))
            throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartMoldRegisterVisit(typeMold, newVisit, formatFlags, lastFieldsStart);
    }

    public int GetCurrentNodeLineNumber(int fromParentIndex, string newLine, int typeStart)
    {
        if (Settings.Style.IsPretty() && fromParentIndex >= 0)
        {
            var maybeCurrentNode = MyActiveGraphRegistry[fromParentIndex];
            var fromIndex        = maybeCurrentNode.TypeOpenBufferIndex;
            var endIndex         = typeStart;
            var countNewLines    = WriteBuffer.CountOccurenceOf(newLine, fromIndex, endIndex);
            return countNewLines;
        }
        return 0;
    }

    private void StartMoldRegisterVisit(TypeMolder startingMold, GraphNodeVisit newVisit, FormatFlags formatFlags, int lastFieldsStartOffset)
    {
        NextCallContext = NextCallContext.Clear();
        var visitId       = startingMold.MoldVisit.VisitId;
        var validRegistry = MyActiveGraphRegistry.RegistryId == visitId.RegistryId;
        var isNewVisit    = visitId.VisitIndex == MyActiveGraphRegistry.Count;
        var registerVisit = validRegistry && isNewVisit;
        if (registerVisit) { MyActiveGraphRegistry.Add(newVisit); }
        startingMold.StartTypeOpening(formatFlags);
        var isExistingRegistered = visitId.VisitIndex < MyActiveGraphRegistry.Count;
        var updateVisit          = validRegistry && isExistingRegistered;
        if (updateVisit)
        {
            var moldWriteState = ((ITypeBuilderComponentSource)startingMold).MoldState;
            var updatedVisit   = MyActiveGraphRegistry[newVisit.NodeVisitId.VisitIndex];
            updatedVisit = updatedVisit.UpdateMoldWriteState(moldWriteState);
            updatedVisit = updatedVisit.UpdateVisitWriteType(newVisit.WrittenAs);
            updatedVisit = updatedVisit.UpdateVisitReplaceFormatFlags(newVisit.CurrentFormatFlags);
            if (lastFieldsStartOffset >= 0)
            {
                MyActiveGraphRegistry[newVisit.NodeVisitId.VisitIndex] =
                    updatedVisit.SetBufferFirstFieldStartAndIndentLevel(lastFieldsStartOffset, CurrentStyledTypeFormatter.Gb.IndentLevel);
            }
            else
            {
                MyActiveGraphRegistry[newVisit.NodeVisitId.VisitIndex] =
                    updatedVisit.SetBufferFirstFieldStartAndIndentLevel(Sb!.Length, CurrentStyledTypeFormatter.Gb.IndentLevel);
            }
            MyActiveGraphRegistry.CurrentGraphNodeIndex = updatedVisit.NodeVisitId.VisitIndex;
        }
        else
        {
            unregisteredVisitMolds ??= new List<TypeMolder>();
            unregisteredVisitMolds.Add(startingMold);
        }

        startingMold.FinishTypeOpening(formatFlags);
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

    public VisitResult UpdateIfRevisitIgnored(VisitResult fullResult, Type typeStarted, FormatFlags formatFlags)
    {
        var shouldIgnore = formatFlags.HasNoRevisitCheck() && fullResult.LastRevisitCount < 64 // todo update this from settings
                        || (typeStarted.IsSpanFormattableCached() && !Settings.InstanceTrackingIncludeSpanFormattableClasses);
        return fullResult.WithIsARevisitSetTo(!shouldIgnore & fullResult.InstanceId > 0 && !fullResult.IsBaseOfInitial);
    }

    void ISecretStringOfPower.TypeComplete(IMoldWriteState completeType)
    {
        var completeVisitDetails = completeType.MoldGraphVisit;
        var (visitRegId, visitIndex) = completeVisitDetails.VisitId;
        transferBetweenMolds.SetHistory(completeType.Sf.Gb);
        var lineCount    = 0;
        var currentIndex = WriteBuffer.Length;
        if (completeType.Style.IsPretty())
        {
            var startAt = completeType.Mold.StartIndex;
            lineCount = WriteBuffer.CountOccurenceOf(completeType.Settings.NewLineStyle, startAt, currentIndex);
        }
        if (completeType.DecrementRefCount() == 0 && visitRegId == asStringEnteredCount && visitIndex >= 0)
        {
            PopCurrentSettings(completeVisitDetails, lineCount);
            return;
        }
        else if (visitRegId >= -1 && visitRegId <= asStringEnteredCount)
        {
            var reg = GetInstanceRegistry(visitRegId);
            if (reg != null && visitIndex < reg.Count)
            {
                var currentNode = reg[visitIndex];
                reg[visitIndex] =
                    currentNode.MarkContentEndClearComponentAccess(currentIndex, lineCount);
            }
        } else if (unregisteredVisitMolds?.Any() ?? false)
        {
            var last = unregisteredVisitMolds![^1];
            if (last.MoldVisit.VisitId == completeVisitDetails.VisitId)
            {
                unregisteredVisitMolds.RemoveAt(unregisteredVisitMolds.Count - 1);
            }
        }
        MyActiveGraphRegistry.TryCurrentGraphNodeChecked(completeVisitDetails.RequesterVisitId);
    }

    protected void PopCurrentSettings(VisitResult visitResult, int lineCount)
    {
        var (visitRegId, visitIndex) = visitResult.VisitId;
        if (MyActiveGraphRegistry.RegistryId != visitRegId
         || visitIndex >= MyActiveGraphRegistry.Count) { Debugger.Break(); }
        var currentNode = MyActiveGraphRegistry[visitIndex];
        MyActiveGraphRegistry[visitIndex] =
            currentNode.MarkContentEndClearComponentAccess(WriteBuffer.Length, lineCount);
        MyActiveGraphRegistry.TryCurrentGraphNodeChecked(visitResult.RequesterVisitId);
        if (MyActiveGraphRegistry.CurrentGraphNodeIndex < 0) { MyActiveGraphRegistry.ClearObjectVisitedGraph(); }
        else
        {
            var poppedToNode = MyActiveGraphRegistry[MyActiveGraphRegistry.CurrentGraphNodeIndex];
            if (poppedToNode.MoldState is { Sf.Gb: not null }) { poppedToNode.MoldState?.Sf.Gb.SetHistory(transferBetweenMolds); }
        }
    }

    VisitResult ISecretStringOfPower.SourceGraphVisitRefIdUpdateGraph(object? toStyleInstance, Type type, Type nextMoldType, FormatFlags formatFlags)
    {
        var visitResult = ((ISecretStringOfPower)(this)).JustSourceGraphVisitResult(toStyleInstance, type, nextMoldType, formatFlags);
        if (visitResult.NoVisitCheckDone) return visitResult;
        if (visitResult.IsARevisit && !MyActiveGraphRegistry[visitResult.FirstInstanceMatchVisitIndex].HasInsertedInstanceId)
        {
            InsertRefId(MyActiveGraphRegistry[visitResult.FirstInstanceMatchVisitIndex], visitResult.FirstInstanceMatchVisitIndex);
        }
        return visitResult;
    }

    VisitResult ISecretStringOfPower.JustSourceGraphVisitResult(object? toStyleInstance, Type type, Type nextMoldType, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return VisitResult.VisitCheckNotRequired;

        var shouldResuseLastVisitSlot = ShouldReuseLastVisitIndex(toStyleInstance, type, nextMoldType);
        if (shouldResuseLastVisitSlot)
        {
            var prevVisited   = MyActiveGraphRegistry[^1];
            var prevMoldState = prevVisited.MoldState;
            if (prevMoldState != null) { return prevMoldState.MoldGraphVisit.IncrementUsedCount(); }
        }
        var registrySearchResult
            = MyActiveGraphRegistry.SourceGraphVisitRefId(MyActiveGraphRegistry.CurrentGraphNodeVisitId, toStyleInstance, type, formatFlags);
        var updatedResult = UpdateIfRevisitIgnored(registrySearchResult, type, formatFlags);
        return updatedResult;
    }

    private bool ShouldReuseLastVisitIndex(object? toStyleInstance, Type type, Type nextMoldType)
    {
        if (type.IsValueType) return false;
        if (MyActiveGraphRegistry.Count == 0) return false;
        var prevVisited  = MyActiveGraphRegistry[^1];
        var prevInstance = prevVisited.VisitedInstance;
        if (!ReferenceEquals(toStyleInstance, prevInstance)) return false;
        var prevMoldState = prevVisited.MoldState;
        if (prevMoldState == null) return false;
        var prevMoldType    = prevMoldState.Mold.GetType();
        var prevVisitResult = prevMoldState.MoldGraphVisit;
        var isPrevPassThroughMold =
            prevMoldType == typeof(SimpleContentTypeMold)
         || prevMoldType == typeof(ComplexContentTypeMold)
         || prevMoldType == typeof(TrackedInstanceMold);
        
        if (nextMoldType == typeof(TrackedInstanceMold) && !isPrevPassThroughMold) return false;
        var prevCompareType = prevVisitResult.ReusedCount < 1
            ? prevMoldState.TypeBeingBuilt
            : prevMoldState.TypeBeingVisitedAs;
        var isSameAsPrev     = prevCompareType == type;
        var isBaseOfPrev     = prevCompareType != type && prevCompareType.IsAssignableTo(type);
        var isNotMoreDerived = prevCompareType != type && type.IsAssignableTo(prevMoldType);
        var isBaseSeekingPrevMold =
            prevMoldType == typeof(ComplexPocoTypeMold)
         || prevMoldType == typeof(ComplexOrderedCollectionMold)
         || prevMoldType == typeof(KeyedCollectionMold);

        if ( !prevMoldState.WroteTypeClose 
          && !isNotMoreDerived
             && (!isSameAsPrev || isPrevPassThroughMold)   
               && ((!isBaseSeekingPrevMold && prevVisitResult.ReusedCount <= 0)  
                || (isBaseOfPrev & isBaseSeekingPrevMold)))
        {
            return true;
        }
        return false;
    }

    private VisitResult MySourceGraphVisitRefId<T>(T toStyleInstance, Type type, Type nextMoldType, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return VisitResult.VisitCheckNotRequired;
        return ((ISecretStringOfPower)this).SourceGraphVisitRefIdUpdateGraph(toStyleInstance, type, nextMoldType, formatFlags);
    }

    protected void InsertRefId(GraphNodeVisit forThisNode, int graphNodeIndex)
    {
        var typeOpenIndex   = forThisNode.TypeOpenBufferIndex;
        var indexToInsertAt = forThisNode.FirstFieldBufferIndex;
        // note: empty types might have a CurrentBufferFirstFieldStart greater than CurrentBufferTypeEnd as empty padding is removed
        var refId           = forThisNode.RefId;
        var moldFlags       = forThisNode.WrittenFlags;
        var fmtState        = forThisNode.FormattingState;
        var formatter       = forThisNode.FormattingState.Formatter.Clone();
        var contentEncoder  = forThisNode.ContentEncoder.Clone();
        var layoutEncoder   = forThisNode.LayoutEncoder.Clone();
        var combinedEncoder = contentEncoder.WithAttachedLayoutEncoder(layoutEncoder);
        formatter.ContentEncoder = combinedEncoder;

        var insertGraphBuilder =
            AlwaysRecycler.Borrow<GraphTrackingBuilder>()
                          .InitializeInsertBuilder(formatter, WriteBuffer, forThisNode.IndentLevel, fmtState.IndentChars);

        var activeMold = forThisNode.MoldState;

        var insertInfo =
            formatter.InsertInstanceReferenceId
                (insertGraphBuilder, refId, forThisNode.ActualType, typeOpenIndex, forThisNode.WrittenAs, indexToInsertAt
               , forThisNode.CurrentFormatFlags, moldFlags, forThisNode.BufferLength, activeMold);

        forThisNode                           = MyActiveGraphRegistry[graphNodeIndex]; // could be updated in InsertInstanceReferenceId
        MyActiveGraphRegistry[graphNodeIndex] = forThisNode.SetHasInsertedRefId(true);
        insertGraphBuilder.DecrementRefCount();
        formatter.DecrementRefCount();
        contentEncoder.DecrementRefCount();
        layoutEncoder.DecrementRefCount();

        if (!ReferenceEquals(contentEncoder, combinedEncoder)) { combinedEncoder.DecrementRefCount(); }
        if (insertInfo.PrefixInserted == 0) return;
        if (activeMold != null) activeMold.WroteRefId = true;
        ShiftRegisteredFromIndex(graphNodeIndex, insertInfo);
        UpdateClosedParentLengths(forThisNode, insertInfo.TotalCharsAdded, insertInfo.PrefixNewLines + insertInfo.SuffixNewLines);
        if (forThisNode.BufferLength < 0 && insertInfo.DeltaIndentLevel > 0)
        {
            Settings.IndentLevel += insertInfo.DeltaIndentLevel;
        }
    }

    public void IncrementIndentForAllDescendantsOf(int visitId, int deltaIncreaseIndentLevel, bool foundFirstUnfinishedChild = true)
    {
        var totalVisits = MyActiveGraphRegistry.Count;
        for (var i = visitId + 1; i < totalVisits; i++)
        {
            var checkVisit = MyActiveGraphRegistry[i];
            if (checkVisit.ParentVisitId.VisitIndex == visitId)
            {
                if (!foundFirstUnfinishedChild && checkVisit.BufferLength < 0)
                {
                    checkVisit.MoldState?.IncrementCloseDepthDecrementBy(deltaIncreaseIndentLevel);
                    foundFirstUnfinishedChild = true;
                }
                MyActiveGraphRegistry[i] = checkVisit.UpdateIndentLevel(checkVisit.IndentLevel + deltaIncreaseIndentLevel);
                IncrementIndentForAllDescendantsOf(i, deltaIncreaseIndentLevel);
            }
        }
    }

    public void ShiftRegisteredFromCharOffset(int fromChar, int shiftByStart, int shiftEnd)
    {
        var firstIndexGreaterOrEqualTo = -1;
        for (int i = 0; i < MyActiveGraphRegistry.Count; i++)
        {
            var shiftCharsNode = MyActiveGraphRegistry[i];
            if (shiftCharsNode.TypeOpenBufferIndex >= fromChar || shiftCharsNode.FirstFieldBufferIndex >= fromChar)
            {
                firstIndexGreaterOrEqualTo = i;
                break;
            }
        }
        if (firstIndexGreaterOrEqualTo >= 0 && firstIndexGreaterOrEqualTo < MyActiveGraphRegistry.Count)
        {
            ShiftRegisteredFromIndex(firstIndexGreaterOrEqualTo
                                   , new InsertInfo(shiftByStart, shiftEnd, 0, 0
                                                  , shiftByStart + shiftEnd, 0, 0));
        }
    }

    public void ShiftRegisteredFromIndex(int insertedVisitIndex, InsertInfo insertInfo)
    {
        var foundFirstOutside = false;

        var insertedIdVisitId = new VisitId(MyActiveGraphRegistry.RegistryId, insertedVisitIndex);
        var preInsertedNode   = MyActiveGraphRegistry[insertedVisitIndex];

        var toAdd = insertInfo.PrefixInserted;

        var totalInsertedNewLines = insertInfo.PrefixNewLines + insertInfo.SuffixNewLines;

        for (int i = insertedVisitIndex + 1; i < MyActiveGraphRegistry.Count; i++)
        {
            var shiftCharsNode = MyActiveGraphRegistry[i];
            var isInside       = !foundFirstOutside;
            if (isInside)
            {
                foundFirstOutside = !HasAncestor(shiftCharsNode, insertedIdVisitId);
                if (foundFirstOutside)
                {
                    toAdd    = insertInfo.TotalCharsAdded;
                    isInside = false;
                }
            }

            if (!isInside) { MyActiveGraphRegistry[i] = shiftCharsNode.ShiftTypeBufferIndex(toAdd, 0, 0); }
            else if (IsChildOf(shiftCharsNode, insertedIdVisitId))
            {
                var nodeLineNumber = shiftCharsNode.ParentLineNumber;
                var offsetShift    = nodeLineNumber * insertInfo.NewLineIndentedBy;
                if (IsChildless(shiftCharsNode))
                {
                    MyActiveGraphRegistry[i] =
                        shiftCharsNode
                            .ShiftTypeBufferIndex(toAdd + offsetShift, insertInfo.DeltaIndentLevel, insertInfo.NewLineIndentedBy);
                }
                else
                {
                    ShiftInnerChildFromIndex(shiftCharsNode, insertInfo, toAdd + offsetShift);
                }
                if (insertInfo.PrefixNewLines > 0)
                    MyActiveGraphRegistry[shiftCharsNode.NodeVisitId.VisitIndex] = shiftCharsNode.AddParentNewLines(insertInfo.PrefixNewLines);
            }
        }

        MyActiveGraphRegistry[insertedVisitIndex] =
            preInsertedNode.ShiftTypeBufferIndex
                (0, insertInfo.DeltaIndentLevel, insertInfo.NewLineIndentedBy
               , insertInfo.PrefixInserted, insertInfo.SuffixInserted
               , totalInsertedNewLines);
    }

    public int ShiftInnerChildFromIndex(GraphNodeVisit child, InsertInfo insertInfo, int deltaShift)
    {
        var childVisitId = child.NodeVisitId;

        GraphNodeVisit descendantNode;

        for (int i = childVisitId.VisitIndex + 1; i < MyActiveGraphRegistry.Count; i++)
        {
            descendantNode = MyActiveGraphRegistry[i];
            var foundFirstOutside = !HasAncestor(descendantNode, childVisitId);
            if (foundFirstOutside) break;

            var nodeLineNumber  = descendantNode.ParentLineNumber;
            var deltaLineNumber = nodeLineNumber;
            var offsetShift     = deltaLineNumber * insertInfo.NewLineIndentedBy;

            var parentAndOffset = deltaShift + offsetShift;
            if (IsChildOf(descendantNode, childVisitId))
            {
                if (IsChildless(descendantNode))
                {
                    MyActiveGraphRegistry[i] =
                        descendantNode.ShiftTypeBufferIndex(parentAndOffset, insertInfo.DeltaIndentLevel, insertInfo.NewLineIndentedBy);
                }
                else { ShiftInnerChildFromIndex(descendantNode, insertInfo, parentAndOffset); }
            }
        }
        descendantNode = MyActiveGraphRegistry[childVisitId.VisitIndex];
        var bufferWidth = descendantNode.BufferLength;

        MyActiveGraphRegistry[childVisitId.VisitIndex] = descendantNode =
            descendantNode.ShiftTypeBufferIndex(deltaShift, insertInfo.DeltaIndentLevel, insertInfo.NewLineIndentedBy);

        var deltaWidthChange = descendantNode.BufferLength - bufferWidth;
        return deltaWidthChange;
    }

    public bool HasAncestor(GraphNodeVisit originalUpdatedLength, VisitId ancestorVisitId)
    {
        var ancestorNode = originalUpdatedLength;
        var countDown    = 1000;
        while (ancestorNode.NodeVisitId is { RegistryId: >= -1, VisitIndex: >= 0 }
            && ancestorNode.BufferLength >= 0 && countDown-- > 0)
        {
            if (ancestorNode.NodeVisitId == ancestorVisitId) { return true; }
            ancestorNode = GetNodeVisit(ancestorNode.ParentVisitId);
        }
        return false;
    }

    public bool IsSiblingOf(GraphNodeVisit originalUpdatedLength, VisitId checkIsSiblingOf)
    {
        return originalUpdatedLength.ParentVisitId == GetNodeVisit(checkIsSiblingOf).ParentVisitId;
    }

    public bool IsChildOf(GraphNodeVisit originalUpdatedLength, VisitId parentVisitId) => originalUpdatedLength.ParentVisitId == parentVisitId;

    public bool IsChildless(GraphNodeVisit originalUpdatedLength)
    {
        var visitId    = originalUpdatedLength.NodeVisitId;
        var registry   = GetInstanceRegistry(visitId.RegistryId);
        var visitIndex = visitId.VisitIndex;
        if (registry == null) return true;

        if (visitIndex + 1 < registry.Count)
        {
            var checkNexRegistered = registry[visitIndex + 1];
            var nextParentId       = checkNexRegistered.ParentVisitId;
            if (visitId == nextParentId) return false;
        }
        else
        {
            if (asStringEnteredCount > visitId.RegistryId)
            {
                registry = GetInstanceRegistry(visitId.RegistryId + 1)!;
                if (registry.Count > 0)
                {
                    var checkNexRegistered = registry[0];
                    var nextParentId       = checkNexRegistered.ParentVisitId;
                    if (visitId == nextParentId) return false;
                }
            }
        }
        return true;
    }

    public void UpdateClosedParentLengths(GraphNodeVisit originalUpdatedLength, int shiftBy, int addedNewLines)
    {
        var updatedChild = originalUpdatedLength;
        if (originalUpdatedLength.ParentVisitId is not { RegistryId: >= -1, VisitIndex: >= 0 }) return;
        var ancestorNode = GetNodeVisit(originalUpdatedLength.ParentVisitId);
        while (ancestorNode.NodeVisitId is { RegistryId: >= -1, VisitIndex: >= 0 })
        {
            if (ancestorNode.BufferLength >= 0) { UpdateVisitLinesAndLength(ancestorNode.NodeVisitId, shiftBy, addedNewLines); }
            var reg             = GetInstanceRegistry(ancestorNode.NodeVisitId.RegistryId)!;
            var ancestorVisitId = ancestorNode.NodeVisitId;
            for (int i = updatedChild.NodeVisitId.VisitIndex + 1; i < reg.Count; i++)
            {
                var familyNode        = reg[i];
                var foundFirstOutside = !HasAncestor(familyNode, ancestorVisitId);
                if (foundFirstOutside) break;

                if (IsChildOf(familyNode, ancestorVisitId)) { reg.UpdateAddParentNewLines(familyNode.NodeVisitId, addedNewLines); }
            }
            if (ancestorNode.ParentVisitId is { RegistryId: >= -1, VisitIndex: >= 0 })
            {
                updatedChild = ancestorNode;
                ancestorNode = GetNodeVisit(ancestorNode.ParentVisitId);
            }
            else
                break;
        }
    }

    public bool IsCallerSameAsLastRegisteredVisit<TVisited>(TVisited checkIsLastVisited)
    {
        if (MyActiveGraphRegistry.Count == 0 && asStringEnteredCount == -1
         || checkIsLastVisited == null)
            return false;
        var graphNodeVisit = MyActiveGraphRegistry.Count > 0
            ? MyActiveGraphRegistry.CurrentNode!.Value
            : GetInstanceRegistry(asStringEnteredCount - 1)!.CurrentNode!.Value;
        var wasLastSameObjectAndOrMoreDerivedRef = MyActiveGraphRegistry.WasVisitOnSameInstance(checkIsLastVisited, graphNodeVisit);
        return wasLastSameObjectAndOrMoreDerivedRef;
    }

    public bool IsCallerSameInstanceAndMoreDerived<TVisited>(TVisited checkIsLastVisited)
    {
        if (MyActiveGraphRegistry.Count == 0
         || checkIsLastVisited == null
         || (MyActiveGraphRegistry.CurrentNode is not { ParentVisitId.VisitIndex: >= 0 }))
            return false;
        var graphNodeVisit = MyActiveGraphRegistry[MyActiveGraphRegistry.CurrentNode.Value.ParentVisitId.VisitIndex];
        var wasLastSameObjectAndOrMoreDerivedRef = MyActiveGraphRegistry.WasVisitOnSameInstance(checkIsLastVisited, graphNodeVisit)
                                                && MyActiveGraphRegistry.WasVisitOnAMoreDerivedType(typeof(TVisited), graphNodeVisit);
        return wasLastSameObjectAndOrMoreDerivedRef;
    }

    public GraphNodeVisit GetNodeVisit(VisitId visitId)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) throw new ArgumentException("Visit does not exist");
        return forRegistry[visitId.VisitIndex];
    }

    public void UpdateVisitWriteMethod(VisitId visitId, WrittenAsFlags newWriteMethod)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitWriteMethod(visitId.VisitIndex, newWriteMethod);
    }

    public void SetBufferFirstFieldStartAndIndentLevel(VisitId visitId, int firstFieldStart, int indentLevel)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.SetBufferFirstFieldStartAndIndentLevel(visitId.VisitIndex, firstFieldStart, indentLevel);
    }

    public void SetBufferFirstFieldStartAndWrittenAs(VisitId visitId, int firstFieldStart, WrittenAsFlags writtenAsFlags)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.SetBufferFirstFieldStartAndWrittenAs(visitId.VisitIndex, firstFieldStart, writtenAsFlags);
    }

    public void UpdateVisitAddFormatFlags(VisitId visitId, FormatFlags flagsToAdd)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitAddFormatFlags(visitId.VisitIndex, flagsToAdd);
    }

    public void UpdateVisitRemoveFormatFlags(VisitId visitId, FormatFlags flagsToRemove)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitRemoveFormatFlags(visitId.VisitIndex, flagsToRemove);
    }

    public void UpdateVisitEncoders(VisitId visitId, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitEncoders(visitId.VisitIndex, contentEncoder, layoutEncoder);
    }

    public void UpdateVisitFormatter(VisitId visitId, IStyledTypeFormatting updatedFormatter)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitFormatter(visitId.VisitIndex, updatedFormatter);
    }

    public int InstanceIdAtVisit(VisitId visitId)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return -1;
        return forRegistry.InstanceIdAtVisit(visitId.VisitIndex);
    }

    public void RemoveVisitAt(VisitId visitId)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.RemoveComparisonInstanceAtVisit(visitId.VisitIndex);
    }

    public int UpdateVisitLinesAndLength(VisitId visitId, int deltaLength, int addedNewLines)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return -1;
        return forRegistry.UpdateVisitLinesAndLength(visitId, deltaLength, addedNewLines);
    }

    public void UpdateAddParentNewLines(VisitId visitId, int deltaParentNewLines)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateAddParentNewLines(visitId, deltaParentNewLines);
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

    public int GetLatestVisitBufferStartIndex(VisitId visitId)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return -1;
        return forRegistry.TypeBufferStartIndexAtVisit(visitId.VisitIndex);
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

    GraphInstanceRegistry ISecretStringOfPower.ActiveGraphRegistry =>
        GetInstanceRegistry(asStringEnteredCount)
     ?? throw new NullReferenceException("Must not still point to a registry that has been removed");

    protected GraphInstanceRegistry MyActiveGraphRegistry
    {
        [DebuggerStepThrough] get => ((ISecretStringOfPower)this).ActiveGraphRegistry;
    }

    protected GraphInstanceRegistry? GetInstanceRegistry(int registryId)
    {
        if (registryId < -1) return null;
        if (registryId == -1) return RootGraphInstanceVisitRegistry;
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
