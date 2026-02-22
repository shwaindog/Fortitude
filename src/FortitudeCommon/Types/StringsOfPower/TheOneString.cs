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

    TrackedInstanceMold EnsureRegisteredClassIsReferenceTracked<T>(T toStyle, WrittenAsFlags proposedWriteAs = AsRaw, CreateContext createContext = default);

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
    
    new CallerContext CallerContext { get; set; }

    GraphTrackingBuilder? CurrentGraphBuilder { get; }
    
    GraphInstanceRegistry ActiveGraphRegistry { get; }

    void SetCallerFormatFlags(FormatFlags callerContentHandler);
    void SetCallerFormatString(string? formatString);

    bool        IsExemptFromCircularRefNodeTracking(Type typeStarted);
    
    VisitResult SourceGraphVisitRefIdUpdateGraph(object? toStyleInstance, Type type, FormatFlags formatFlags);

    AppendSummary RegisterVisitedInstanceAndConvert(object obj, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags);

    void TypeComplete(IMoldWriteState completeType);

    void UpdateVisitWriteMethod(VisitId visitId, WrittenAsFlags newWriteMethod);
    void UpdateVisitAddFormatFlags(VisitId visitId, FormatFlags flagsToAdd);
    void UpdateVisitRemoveFormatFlags(VisitId visitId, FormatFlags flagsToRemove);
    void UpdateVisitEncoders(VisitId visitId, IEncodingTransfer contentEncoder, IEncodingTransfer layoutEncoder);

    int  GetNextVisitedReferenceId(int instanceRegistryIndex);
    void RemoveVisitAt(VisitId visitId);
    int  InstanceIdAtVisit(VisitId visitId);
    int  UpdateVisitLength(VisitId visitId, int deltaLength);
    int  GetLatestVisitBufferStartIndex(VisitId visitId);

    void ShiftRegisteredFromCharOffset(int fromChar, int shiftBy);
    
    void PopLastAsStringInstanceRegistry();

    ITheOneString AddBaseFieldsStart();
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
        this.shouldSkip                = shouldSkip;
        contextChangeRequestFlags      = contextChangeCallerFlags;
        this.stringMaster              = stringMaster;
        this.toRestoreOnDispose        = toRestoreOnDispose;
        ((IRecyclableObject?)toRestoreOnDispose)?.IncrementRefCount();
    }

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
            var previousFormatter = stringMaster.CurrentStyledTypeFormatter.ContextCompletePopToPrevious();
            stringMaster.Settings.CopyFrom(toRestoreOnDispose);
            stringMaster.CurrentStyledTypeFormatter = previousFormatter;
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

    private CallerContext callerContext;

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
        
        Settings.Formatter   = CurrentStyledTypeFormatter;

        return this;
    }

    protected IMoldWriteState? CurrentRegisteredTypeAccess => MyActiveGraphRegistry.CurrentNode?.MoldState;


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
    
    public TypeMolder? CurrentTypeBuilder => CurrentRegisteredTypeAccess?.Mold;

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
        var type           = obj.GetType();
        
        
        var existingRefId  = JustSourceGraphVisitResult(obj, type, formatFlags);
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

    public TrackedInstanceMold EnsureRegisteredClassIsReferenceTracked<T>(T toStyle, WrittenAsFlags proposedWriteAs = AsRaw, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        if (visitType.IsValueType) throw new ArgumentException("Expected toStyle to be a class not a value type");
        var actualType = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter     = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, proposedWriteAs, visitResult, createFlags);
        var remainingDepth    = MyActiveGraphRegistry.RemainingDepth - 1;
        writeMethod |= (visitResult.IsARevisit || remainingDepth <= 0) && mergedCreateFlags.DoesNotHaveSuppressOpening()
            ? AsComplex
            : AsRaw;
        var simpleValueBuilder =
            AlwaysRecycler.Borrow<TrackedInstanceMold>().InitializeRawContentTypeBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, simpleValueBuilder, writeMethod, createFlags);
        return simpleValueBuilder;
    }

    KeyedCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, CreateContext createContext)
    {
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsMapCollection, visitResult, createFlags);
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
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var actualType  = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(keyValueContainerInstance, actualType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(keyValueContainerInstance, actualType, typeFormatter, AsMapCollection, visitResult, createFlags);
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
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsSimple | WrittenAsFlags.AsCollection
                                           , visitResult, createFlags);
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
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(visitType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsComplex | WrittenAsFlags.AsCollection
                                           , visitResult, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var complexOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, complexOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance
      , CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var actualType  = collectionInstance.GetType();
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(collectionInstance, actualType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter      = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(collectionInstance, actualType, typeFormatter, AsSimple | WrittenAsFlags.AsCollection
                                         , visitResult, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (collectionInstance, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(collectionInstance, actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(Type typeOfToStyle, CreateContext createContext = default)
    {
        var createFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldMultiGenerationInheritFlags();
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(actualType, typeFormatter, AsSimple | WrittenAsFlags.AsCollection
                                             , VisitResult.VisitNotChecked, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId), writeMethod, mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionTypeOfNullable<TElement>(Type typeOfToStyle
      , CreateContext createContext = default) where TElement : struct
    {
        var createFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldMultiGenerationInheritFlags();
        var actualType    = typeOfToStyle;
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(actualType, actualType, typeFormatter, AsSimple | WrittenAsFlags.AsCollection
                                         , VisitResult.VisitNotChecked, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (NeverEqual, actualType, this, actualType, createContext.NameOverride, remainingDepth
               , MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId), writeMethod, mergedCreateFlags);
        TypeStart(actualType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags) = 
            GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsSimple | WrittenAsFlags.AsCollection
                                                                            , visitResult, createFlags);
        var remainingDepth = MyActiveGraphRegistry.RemainingDepth - 1;
        var explicitOrderedCollectionBuilder =
            AlwaysRecycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (WrapOrReturnSubjectAsObject(toStyle), actualType, this, visitType, createContext.NameOverride, remainingDepth
               , visitResult, writeMethod, mergedCreateFlags);
        TypeStart(toStyle, visitType, explicitOrderedCollectionBuilder, writeMethod, mergedCreateFlags);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexPocoTypeMold StartComplexType<T>(T toStyle, CreateContext createContext = default)
    {
        var callFlags   = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
        var createFlags = callFlags.MoldMultiGenerationInheritFlags();
        var visitType   = typeof(T);
        var actualType  = toStyle?.GetType() ?? visitType;
        var visitResult = !IsExemptFromCircularRefNodeTracking(actualType)
            ? MySourceGraphVisitRefId(toStyle, visitType, callFlags)
            : MyActiveGraphRegistry.VisitCheckNotRequired(MyActiveGraphRegistry.CurrentGraphNodeVisitId);
        var typeFormatter = TypeFormattingOverrides.GetValueOrDefault(actualType, CurrentStyledTypeFormatter);
        var (writeMethod, mergedCreateFlags)
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsComplex, visitResult, createFlags);
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
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
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
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsSimple | AsContent, visitResult, createFlags);
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
        var createFlags = createContext.FormatFlags | CallerContext.FormatFlags.MoldSingleGenerationPassFlags();
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
            = GetFormatterWriteAsFormatFlags(toStyle, actualType, typeFormatter, AsComplex | AsContent, visitResult, createFlags);
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

    public (WrittenAsFlags, FormatFlags) GetFormatterWriteAsFormatFlags(Type forType, IStyledTypeFormatting formatter, WrittenAsFlags proposedWriteType
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
        var fmtState = new FormattingState
            (MyActiveGraphRegistry.CurrentDepth + 1
           , MyActiveGraphRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentStyledTypeFormatter.ContentEncoder
           , CurrentStyledTypeFormatter.LayoutEncoder);

        var newVisit =
            new GraphNodeVisit
                (typeMold.MoldVisit.VisitId, MyActiveGraphRegistry.CurrentGraphNodeVisitId
               , visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , null, IndentLevel, CallerContext, fmtState, formatFlags,  Sb!.Length
               , typeMold.MoldVisit.LastRevisitCount + 1)
                {
                    MoldState = ((ITypeBuilderComponentSource)typeMold).MoldState
                };
        CallerContext.Clear();
        if (newVisit.NodeVisitId.VisitIndex != MyActiveGraphRegistry.Count)
            throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartMoldRegisterVisit(typeMold, newVisit, formatFlags);
    }

    protected void TypeStart<T>(T toStyle, Type visitType, TypeMolder typeMold, WrittenAsFlags writeMethod, FormatFlags formatFlags)
    {
        var fmtState = new FormattingState
            (MyActiveGraphRegistry.CurrentDepth + 1
           , MyActiveGraphRegistry.RemainingDepth - 1, formatFlags
           , Settings.IndentSize, CurrentStyledTypeFormatter, CurrentStyledTypeFormatter.ContentEncoder
           , CurrentStyledTypeFormatter.LayoutEncoder);

        var wrapped = WrapOrReturnSubjectAsObject(toStyle);

        var newVisit =
            new GraphNodeVisit
                (typeMold.MoldVisit.VisitId
               , MyActiveGraphRegistry.CurrentGraphNodeVisitId
               , toStyle?.GetType() ?? visitType, visitType
               , ((ITypeBuilderComponentSource)typeMold).MoldState, writeMethod
               , wrapped, IndentLevel, CallerContext, fmtState, formatFlags
               , Sb!.Length, typeMold.MoldVisit.LastRevisitCount + 1);

        CallerContext.Clear();    
        if (newVisit.NodeVisitId.RegistryId >= -1 && newVisit.NodeVisitId.VisitIndex != MyActiveGraphRegistry.Count)
            throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartMoldRegisterVisit(typeMold, newVisit, formatFlags);
    }

    private void StartMoldRegisterVisit(TypeMolder startingMold, GraphNodeVisit newVisit, FormatFlags formatFlags)
    {
        var visitId = startingMold.MoldVisit.VisitId;
        if (MyActiveGraphRegistry.RegistryId == visitId.RegistryId && visitId.VisitIndex == MyActiveGraphRegistry.Count)
        {
            MyActiveGraphRegistry.Add(newVisit);
            MyActiveGraphRegistry.CurrentGraphNodeIndex = newVisit.NodeVisitId.VisitIndex;
        }
        startingMold.StartTypeOpening(formatFlags);
        if (MyActiveGraphRegistry.RegistryId == visitId.RegistryId && visitId.VisitIndex != MyActiveGraphRegistry.Count)
            MyActiveGraphRegistry[newVisit.NodeVisitId.VisitIndex] = 
                MyActiveGraphRegistry[newVisit.NodeVisitId.VisitIndex].SetBufferFirstFieldStart(Sb!.Length, CurrentStyledTypeFormatter.Gb.IndentLevel);
        
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
        var (visitRegId, visitIndex)= completeVisitDetails.VisitId;
        transferBetweenMolds.CopyFrom(completeType.Sf.Gb);
        // Console.Out.WriteLine("Completing " + completeType);
        if (completeType.DecrementRefCount() == 0 && visitRegId == asStringEnteredCount && visitIndex >= 0)
        {
            PopCurrentSettings(completeVisitDetails);
            return;
        } else if ( visitRegId >= -1 && visitRegId <= asStringEnteredCount)
        {
            var reg         = GetInstanceRegistry(visitRegId);
            if (reg != null && visitIndex < reg.Count)
            {
                var currentNode = reg[visitIndex];
                // Console.Out.WriteLine("Wiping other registry Type visit " + completeVisitDetails.VisitId);
                reg[visitIndex] =
                    currentNode.MarkContentEndClearComponentAccess(WriteBuffer.Length, currentNode.WrittenAs);

                // return;
            }
        }
        // Console.Out.WriteLine("NOT WIPING !!!!!!!!!!!! Type visit " + completeVisitDetails.VisitId);
    }

    protected void PopCurrentSettings(VisitResult visitResult)
    {
        var (visitRegId, visitIndex)= visitResult.VisitId;
        if (MyActiveGraphRegistry.RegistryId != visitRegId 
         || visitIndex >= MyActiveGraphRegistry.Count)
        {
            Debugger.Break();
        }
        var currentNode       = MyActiveGraphRegistry[visitIndex];
        MyActiveGraphRegistry[visitIndex] =
        currentNode.MarkContentEndClearComponentAccess(WriteBuffer.Length, currentNode.WrittenAs);
        // Console.Out.WriteLine("Wiping Type visit " + visitResult.VisitId);
        MyActiveGraphRegistry.TryCurrentGraphNodeChecked(visitResult.RequesterVisitId);
        if (MyActiveGraphRegistry.CurrentGraphNodeIndex < 0)
        {
            MyActiveGraphRegistry.ClearObjectVisitedGraph();
        }
        else
        {
            var poppedToNode      = MyActiveGraphRegistry[MyActiveGraphRegistry.CurrentGraphNodeIndex];
            if (poppedToNode.MoldState != null && poppedToNode.MoldState?.Sf.Gb != null)
            {
                poppedToNode.MoldState?.Sf.Gb.CopyFrom(transferBetweenMolds);
            }
        }
    }

    VisitResult ISecretStringOfPower.SourceGraphVisitRefIdUpdateGraph(object? toStyleInstance, Type type, FormatFlags formatFlags)
    {
        var visitResult        = JustSourceGraphVisitResult(toStyleInstance, type, formatFlags);
        if (visitResult.NoVisitCheckDone) return visitResult;
        if (visitResult.IsARevisit && !MyActiveGraphRegistry[visitResult.FirstInstanceMatchVisitIndex].HasInsertedInstanceId)
        {
            InsertRefId(MyActiveGraphRegistry[visitResult.FirstInstanceMatchVisitIndex], visitResult.FirstInstanceMatchVisitIndex);
        }
        return visitResult;
    }

    VisitResult JustSourceGraphVisitResult(object? toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return VisitResult.VisitCheckNotRequired;
        var registrySearchResult = MyActiveGraphRegistry.SourceGraphVisitRefId(MyActiveGraphRegistry.CurrentGraphNodeVisitId, toStyleInstance, type, formatFlags);
        var updatedResult        = UpdateIfRevisitIgnored(registrySearchResult, type, formatFlags);
        return updatedResult;
    }

    private VisitResult MySourceGraphVisitRefId<T>(T toStyleInstance, Type type, FormatFlags formatFlags)
    {
        if (type.IsValueType || toStyleInstance == null) return VisitResult.VisitCheckNotRequired;
        return ((ISecretStringOfPower)this).SourceGraphVisitRefIdUpdateGraph(toStyleInstance, type, formatFlags);
    }
    
    protected void InsertRefId(GraphNodeVisit forThisNode, int graphNodeIndex)
    {
        var typeOpenIndex = forThisNode.TypeOpenBufferIndex;
        var indexToInsertAt = forThisNode.FirstFieldBufferIndex;
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

        var activeMold = forThisNode.MoldState;
        
        var (charsInserted, deltaIndentLevel) =
            formatter.InsertInstanceReferenceId
                (insertGraphBuilder, refId, forThisNode.ActualType, typeOpenIndex, forThisNode.WrittenAs, indexToInsertAt
               , forThisNode.CurrentFormatFlags, forThisNode.BufferLength, activeMold);
        MyActiveGraphRegistry[graphNodeIndex] = forThisNode.SetHasInsertedRefId(true);
        insertGraphBuilder.DecrementRefCount();
        formatter.DecrementRefCount();
        contentEncoder.DecrementRefCount();
        layoutEncoder.DecrementRefCount();
        
        if (!ReferenceEquals(contentEncoder, combinedEncoder)) { combinedEncoder.DecrementRefCount(); }
        if (charsInserted == 0) return;
        if (activeMold != null) activeMold.WroteRefId = true;
        ShiftRegisteredFromIndex(graphNodeIndex, charsInserted);
        if (forThisNode.BufferLength < 0 && deltaIndentLevel > 0)
        {
            Settings.IndentLevel += deltaIndentLevel;
            IncrementIndentForAllDescendantsOf(graphNodeIndex, deltaIndentLevel, false);
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

    public void ShiftRegisteredFromCharOffset(int fromChar, int shiftBy)
    {
        var firstIndexGreaterOrEqualTo = -1;
        for (int i = 0; i < MyActiveGraphRegistry.Count; i++)
        {
            var shiftCharsNode = MyActiveGraphRegistry[i];
            if (shiftCharsNode.TypeOpenBufferIndex >= fromChar)
            {
                firstIndexGreaterOrEqualTo = i;
                break;
            }
        }
        if (firstIndexGreaterOrEqualTo >= 0 && firstIndexGreaterOrEqualTo < MyActiveGraphRegistry.Count)
        {
            ShiftRegisteredFromIndex(firstIndexGreaterOrEqualTo, shiftBy);
        }
    }

    public void ShiftRegisteredFromIndex(int fromIndex, int shiftBy)
    {
        for (int i = fromIndex; i < MyActiveGraphRegistry.Count; i++)
        {
            var shiftCharsNode = MyActiveGraphRegistry[i];
            MyActiveGraphRegistry[i] = shiftCharsNode.ShiftTypeBufferIndex(shiftBy);
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

    public void UpdateVisitWriteMethod(VisitId visitId, WrittenAsFlags newWriteMethod)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return;
        forRegistry.UpdateVisitWriteMethod(visitId.VisitIndex, newWriteMethod);
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

    public int  UpdateVisitLength(VisitId visitId, int deltaLength)
    {
        var forRegistry = GetInstanceRegistry(visitId.RegistryId);
        if (forRegistry == null || forRegistry.Count < visitId.VisitIndex) return -1;
        return forRegistry.UpdateVisitLength(visitId.VisitIndex, deltaLength);
    }

    public int GetNextVisitedReferenceId(int instanceRegistryIndex)
    {
        if ( instanceRegistryIndex >= 0
         && AsStringInstanceVisitRegistry != null
         && AsStringInstanceVisitRegistry.Count > instanceRegistryIndex
         && Settings.InstanceMarkingAsStringIndependentNumbering)
        {
            return AsStringInstanceVisitRegistry[instanceRegistryIndex].ThisRegistryNextRefId++;
        }
        return RootGraphInstanceVisitRegistry.ThisRegistryNextRefId++;
    }

    public int  GetLatestVisitBufferStartIndex(VisitId visitId)
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
