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
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

#endregion

namespace FortitudeCommon.Types.StringsOfPower;

public interface ITheOneString : IReusableObject<ITheOneString>
{
    // ReSharper disable UnusedMemberInSuper.Global
    StringStyle Style { get; }

    StyleOptions Settings { get; }

    int IndentLevel { get; set; }

    TypeMolder? CurrentTypeBuilder { get; }

    ITheOneString ClearAndReinitialize
        (StyleOptions styleOptions, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString ClearAndReinitialize
        (StyleOptionsValue styleOptions, int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreFlags = SkipTypeParts.None);

    KeyValueCollectionMold StartKeyedCollectionType<T>(T toStyle, string? overrideName = null);

    ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , string? overrideName = null);

    SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, string? overrideName = null);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, string? overrideName = null);

    ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object toStyle, string? overrideName = null);

    ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, string? overrideName = null);

    ComplexTypeMold StartComplexType<T>(T toStyle, string? overrideName = null);

    SimpleValueTypeMold StartSimpleValueType<T>(T toStyle, string? overrideName = null);

    ComplexValueTypeMold StartComplexValueType<T>(T toStyle, string? overrideName = null);

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

    StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, bool isKeyName, string? formatString = null);

    bool RegisterVisitedCheckCanContinue(object obj);
    int  EnsureRegisteredVisited(object obj);

    void TypeComplete(ITypeMolderDieCast completeType);

    ITheOneString AddBaseFieldsStart();
    ITheOneString AddBaseFieldsEnd();
}

public readonly struct CallContextDisposable(bool shouldSkip, ITheOneString? stringMaster = null, StyleOptions? toRestoreOnDispose = null)
    : IDisposable
{
    public bool ShouldSkip => shouldSkip;
    
    public bool HasFormatChange => toRestoreOnDispose != null;

    public void Dispose()
    {
        if (toRestoreOnDispose != null && stringMaster != null)
        {
            stringMaster.Settings.Values = toRestoreOnDispose.Values;
            ((IRecyclableObject)toRestoreOnDispose).DecrementRefCount();
        }
    }
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    internal const string Null = "null";

    private static readonly IRecycler AlWaysRecycler = new Recycler();

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static readonly Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    protected int CurrentGraphNodeIndex = -1;

    private MoldDieCastSettings initialAppendSettings;

    protected int NextObjVisitedRefId = 1;

    private MoldDieCastSettings? nextTypeAppendSettings;

    protected List<GraphNodeVisit> OrderedObjectGraph = new(16);

    protected IStringBuilder? Sb;

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

        Settings.Style = toClone.Style;
    }

    public TheOneString(ITheOneString toClone)
    {
        Sb = BufferFactory();
        Sb.Append(toClone.WriteBuffer);

        Settings.Style = toClone.Style;
    }

    public ITheOneString Initialize(IStringBuilder usingStringBuilder, StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();
        Sb             = usingStringBuilder;
        Settings.Style = buildStyle;
        OrderedObjectGraph.Clear();

        return this;
    }

    public ITheOneString Initialize(StringStyle buildStyle = StringStyle.CompactLog)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        Settings.Style = buildStyle;
        OrderedObjectGraph.Clear();

        return this;
    }

    protected MoldDieCastSettings AppendSettings => nextTypeAppendSettings ?? CurrentTypeAccess?.AppendSettings ?? initialAppendSettings;

    protected ITypeMolderDieCast? CurrentTypeAccess => CurrentNode?.TypeBuilderComponentAccess;

    public bool UseEqualsForVisited
    {
        get => UseReferenceEqualsForVisited;
        set => UseReferenceEqualsForVisited = !value;
    }

    public StyleOptions Settings { get; set; } = new(new StyleOptionsValue());


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

    protected GraphNodeVisit? CurrentNode =>
        CurrentGraphNodeIndex >= 0 && CurrentGraphNodeIndex < OrderedObjectGraph.Count
            ? OrderedObjectGraph[CurrentGraphNodeIndex]
            : null;

    public ITheOneString ClearAndReinitialize(StyleOptions styleOptions, int indentLevel = 0
      , SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        Settings = styleOptions;

        IndentLevel = indentLevel;

        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();
        OrderedObjectGraph.Clear();

        return this;
    }

    public ITheOneString ClearAndReinitialize(StyleOptionsValue styleOptionsValue, int indentLevel = 0
      , SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        Settings.Values = styleOptionsValue;

        IndentLevel = indentLevel;

        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();
        OrderedObjectGraph.Clear();

        return this;
    }

    void ISecretStringOfPower.TypeComplete(ITypeMolderDieCast completeType)
    {
        PopCurrentSettings();
        ((IRecyclableObject)completeType).DecrementRefCount();
    }

    public StateExtractStringRange RegisterVisitedInstanceAndConvert(object obj, bool isKeyName, string? formatString = null)
    {
        var type           = obj.GetType();
        var existingRefId  = SourceGraphVisitRefId(obj, type);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;

        return existingRefId > 0 || remainingDepth <= 0
            ? StartComplexValueType(obj).AsValueMatch("", obj, formatString).Complete()
            : StartSimpleValueType(obj).AsValueMatch("", obj, formatString).Complete();
    }

    public bool RegisterVisitedCheckCanContinue(object obj)
    {
        var type = obj.GetType();

        var existingRefId = SourceGraphVisitRefId(obj, type);
        if (existingRefId > 0)
        {
            StartComplexValueType(obj).AsStringOrNull("", "").Complete();
            return false;
        }
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, obj, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return true;
    }

    public int EnsureRegisteredVisited(object obj)
    {
        var type = obj.GetType();

        var firstVisitedIndex = IndexOfInstanceVisitFromEnd(obj, type);
        if (firstVisitedIndex >= 0) { return firstVisitedIndex; }
        var newVisit = new GraphNodeVisit
            (OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, obj, (CurrentNode?.GraphDepth ?? -1) + 1
           , IndentLevel, Sb!.Length
           , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1);

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return newVisit.ObjVisitIndex;
    }

    KeyValueCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, string? overrideName)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettings(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyValueCollectionMold>().InitializeKeyValueCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, actualType);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , string? overrideName = null)
    {
        var actualType = keyValueContainerInstance.GetType();
        if (!actualType.IsKeyedCollection()) { throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type"); }

        var appendSettings = GetAppendSettings(keyValueContainerInstance, actualType);
        var existingRefId  = SourceGraphVisitRefId(keyValueContainerInstance, actualType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>().InitializeExplicitKeyValueCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(keyValueContainerInstance, keyedCollectionBuilder, actualType);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettings(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionMold>().InitializeSimpleOrderedCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleOrderedCollectionBuilder, actualType);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettings(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionMold>().InitializeComplexOrderedCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, complexOrderedCollectionBuilder, actualType);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettings(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance, string? overrideName = null)
    {
        var actualType     = collectionInstance.GetType();
        var appendSettings = GetAppendSettings(collectionInstance, actualType);
        var existingRefId  = SourceGraphVisitRefId(collectionInstance, actualType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>().InitializeExplicitOrderedCollectionBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(collectionInstance, explicitOrderedCollectionBuilder, actualType);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexTypeMold StartComplexType<T>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettings(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexTypeBuilder =
            Recycler.Borrow<ComplexTypeMold>().InitializeComplexTypeBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, complexTypeBuilder, actualType);
        return complexTypeBuilder;
    }

    public SimpleValueTypeMold StartSimpleValueType<T>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettingsSuppressSpanFormattable(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(visitType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleValueBuilder =
            Recycler.Borrow<SimpleValueTypeMold>().InitializeSimpleValueTypeBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleValueBuilder, actualType);
        return simpleValueBuilder;
    }

    public ComplexValueTypeMold StartComplexValueType<T>(T toStyle, string? overrideName = null)
    {
        var visitType      = typeof(T);
        var actualType     = toStyle?.GetType() ?? visitType;
        var appendSettings = GetAppendSettingsSuppressSpanFormattable(toStyle, actualType);
        var existingRefId  = SourceGraphVisitRefId(toStyle, visitType);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(actualType, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ComplexValueTypeMold>().InitializeComplexValueTypeBuilder
                (actualType, this, appendSettings, overrideName ?? actualType.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, actualType);
        return keyedCollectionBuilder;
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

    public MoldDieCastSettings GetAppendSettings<T>(T forValue, Type actualType)
    {
        var nextDieSettings = AppendSettings;
        return nextDieSettings;
    }

    public MoldDieCastSettings GetAppendSettingsSuppressSpanFormattable<T>(T forValue, Type actualType)
    {
        var nextDieSettings = GetAppendSettings(forValue, actualType);
        if (forValue is ISpanFormattable)
        {
            nextDieSettings.SkipTypeParts = SkipTypeParts.TypeStart | SkipTypeParts.TypeName | SkipTypeParts.TypeEnd;
        }
        return nextDieSettings;
    }

    public ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        OrderedObjectGraph.Clear();

        return this;
    }

    public CallContextDisposable ResolveContextForContentFlags(FieldContentHandling contentFlags)
    {
        if ((contentFlags & ExcludeMask) > 0)
        {
            var shouldSkip = false;
            shouldSkip |= Settings.Style.IsLog() && contentFlags.HasExcludeWhenLogStyleFlag();
            shouldSkip |= Settings.Style.IsJson() && contentFlags.HasExcludeWhenJsonStyleFlag();
            shouldSkip |= Settings.Style.IsCompact() && contentFlags.HasExcludeWhenCompactFlag();
            shouldSkip |= Settings.Style.IsPretty() && contentFlags.HasExcludeWhenPrettyFlag();
            if(shouldSkip) return new CallContextDisposable(true);
        }
        if(Settings.IsSame(contentFlags)) return new CallContextDisposable(false);
        Settings.IfExistsIncrementFormatterRefCount();
        var saveCurrentOptions = Recycler.Borrow<StyleOptions>().Initialize(Settings.Values);
        var existingOptions = new CallContextDisposable(false, this, saveCurrentOptions);

        Settings.Style     = contentFlags.UpdateStringStyle(Settings.Style);
        Settings.Formatter = Recycler.ResolveStyleFormatter(Settings);

        return existingOptions;
    }

    protected int NextRefId() => NextObjVisitedRefId++;

    protected void TypeStart<T>(T toStyle, TypeMolder newType, Type typeOfT)
    {
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeof(T), newType.IsComplexType
                                        , typeOfT.IsValueType ? null : toStyle, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1)
        {
            TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).ComponentAccess
        };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(newType, newVisit);
    }

    private void StartNewVisit(TypeMolder newType, GraphNodeVisit newVisit)
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
        if (toStyle is object objToStyle)
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
        return 0;
    }

    private int SourceGraphVisitRefId(object toStyleInstance, Type type)
    {
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

    protected bool HasVisited(object objToStyle, Type objAsType, GraphNodeVisit checkExisting)
    {
        var checkRef               = checkExisting.StylingObjInstance;
        var hasVisited             = UseReferenceEqualsForVisited ? ReferenceEquals(checkRef, objToStyle) : Equals(checkRef, objToStyle);
        if (hasVisited) hasVisited = !IsCallingAsBaseType(objToStyle, objAsType, checkExisting);
        return hasVisited;
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
            shiftBy += Settings.StyledTypeFormatter.InsertFieldSeparatorAt(Sb!, indexToInsertAt + idSpan.Length, Settings
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
    (
        int ObjVisitIndex
      , int ParentVisitIndex
      , Type VistedAsType
      , bool isComplexType
      , object? StylingObjInstance
      , int GraphDepth
      , int IndentLevel
      , int CurrentBufferTypeStart
      , int RemainingGraphDepth
    )
    {
        public int RefId { get; private init; }

        public bool IsValueTYpe = VistedAsType.IsValueType;

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
    }
}
