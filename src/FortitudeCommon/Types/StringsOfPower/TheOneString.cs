// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

#endregion

namespace FortitudeCommon.Types.StringsOfPower;

public interface ITheOneString : IReusableObject<ITheOneString>
{
    const string DefaultIndentString   = "  ";
    const string WhenNullString        = "null";
    const string DefaultString         = "";
    const string DefaultStringAsNumber = "0";
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
    int EnsureRegisteredVisited(object obj);

    void TypeComplete(ITypeMolderDieCast completeType);

    ITheOneString AddBaseFieldsStart();
    ITheOneString AddBaseFieldsEnd();
}

public class TheOneString : ReusableObject<ITheOneString>, ISecretStringOfPower
{
    internal const string Null = "null";

    private static readonly IRecycler AlWaysRecycler = new Recycler();

    private static readonly ConcurrentDictionary<Type, IStyledTypeFormatting> TypeFormattingOverrides = new();

    public static Func<IStringBuilder> BufferFactory = () => AlWaysRecycler.Borrow<MutableString>();

    protected int CurrentGraphNodeIndex = -1;

    private MoldDieCastSettings initialAppendSettings;

    protected int NextObjVisitedRefId = 1;

    private MoldDieCastSettings? nextTypeAppendSettings;

    protected List<GraphNodeVisit> OrderedObjectGraph = new(16);

    protected IStringBuilder? Sb;

    public TheOneString()
    {
        Settings.Style = StringStyle.Default;
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

    public ITheOneString Initialize(IStringBuilder usingStringBuilder, StringStyle buildStyle = StringStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb             = usingStringBuilder;
        Settings.Style = buildStyle;

        return this;
    }

    public ITheOneString Initialize(StringStyle buildStyle = StringStyle.Default)
    {
        Sb?.DecrementRefCount();
        Sb = SourceStringBuilder();

        Settings.Style = buildStyle;

        return this;
    }

    protected MoldDieCastSettings AppendSettings => nextTypeAppendSettings ?? CurrentTypeAccess?.AppendSettings ?? initialAppendSettings;

    protected ITypeMolderDieCast? CurrentTypeAccess => CurrentNode?.TypeBuilderComponentAccess;

    public bool UseEqualsForVisited
    {
        get => UseReferenceEqualsForVisited!;
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
            ? StartComplexValueType(obj).ValueMatch(obj, formatString).Complete()
            : StartSimpleValueType(obj).ValueMatch(obj, formatString).Complete();
    }

    public bool RegisterVisitedCheckCanContinue(object obj)
    {
        var type = obj.GetType();

        var existingRefId = SourceGraphVisitRefId(obj, type);
        if (existingRefId > 0)
        {
            StartComplexValueType(obj).String("", "").Complete();
            return false;
        }
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, obj, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, null);

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return true;
    }

    public int EnsureRegisteredVisited(object obj)
    {
        var type = obj.GetType();

        var firstVisitedIndex = IndexOfInstanceVisitFromEnd(obj, type);
        if (firstVisitedIndex >= 0)
        {
            return firstVisitedIndex;
        }
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, type, false, obj, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, null);

        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");
        OrderedObjectGraph.Add(newVisit);

        // Not updating as this is an opaque object and just market a circular reference visit CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;

        return newVisit.ObjVisitIndex;
    }

    KeyValueCollectionMold ITheOneString.StartKeyedCollectionType<T>(T toStyle, string? overrideName)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        if (type == typeof(object))
        {
            type = toStyle?.GetType() ?? type;
        }
        var keyedCollectionBuilder =
            Recycler.Borrow<KeyValueCollectionMold>()
                    .InitializeKeyValueCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, type);
        return keyedCollectionBuilder;
    }

    public ExplicitKeyedCollectionMold<TKey, TValue> StartExplicitKeyedCollectionType<TKey, TValue>(object keyValueContainerInstance
      , string? overrideName = null)
    {
        var type = keyValueContainerInstance.GetType();
        if (!type.IsKeyedCollection())
        {
            throw new ArgumentException("Expected keyValueContainerInstance to be a keyed collection type");
        }

        var appendSettings = AppendSettings;
        var existingRefId  = SourceGraphVisitRefId(keyValueContainerInstance, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ExplicitKeyedCollectionMold<TKey, TValue>>()
                    .InitializeExplicitKeyValueCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(keyValueContainerInstance, keyedCollectionBuilder, type);
        return keyedCollectionBuilder;
    }

    public SimpleOrderedCollectionMold StartSimpleCollectionType<T>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        if (!type.IsValueType && !type.IsAnyTypeHoldingChars())
        {
            type = toStyle?.GetType() ?? type;
        }
        var simpleOrderedCollectionBuilder =
            Recycler.Borrow<SimpleOrderedCollectionMold>()
                    .InitializeSimpleOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleOrderedCollectionBuilder, type);
        return simpleOrderedCollectionBuilder;
    }

    public ComplexOrderedCollectionMold StartComplexCollectionType<T>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        if (type == typeof(object))
        {
            type = toStyle?.GetType() ?? type;
        }
        var complexOrderedCollectionBuilder =
            Recycler.Borrow<ComplexOrderedCollectionMold>()
                    .InitializeComplexOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, complexOrderedCollectionBuilder, type);
        return complexOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<T, TElement>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        if (type == typeof(object))
        {
            type = toStyle?.GetType() ?? type;
        }
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>()
                    .InitializeExplicitOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, explicitOrderedCollectionBuilder, type);
        return explicitOrderedCollectionBuilder;
    }

    public ExplicitOrderedCollectionMold<TElement> StartExplicitCollectionType<TElement>(object collectionInstance, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = collectionInstance.GetType();
        var existingRefId  = SourceGraphVisitRefId(collectionInstance, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var explicitOrderedCollectionBuilder =
            Recycler.Borrow<ExplicitOrderedCollectionMold<TElement>>()
                    .InitializeExplicitOrderedCollectionBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(collectionInstance, explicitOrderedCollectionBuilder, type);
        return explicitOrderedCollectionBuilder;
    }

    public ComplexTypeMold StartComplexType<T>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var complexTypeBuilder =
            Recycler.Borrow<ComplexTypeMold>()
                    .InitializeComplexTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, complexTypeBuilder, type);
        return complexTypeBuilder;
    }

    public SimpleValueTypeMold StartSimpleValueType<T>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var simpleValueBuilder =
            Recycler.Borrow<SimpleValueTypeMold>()
                    .InitializeSimpleValueTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, simpleValueBuilder, type);
        return simpleValueBuilder;
    }

    public ComplexValueTypeMold StartComplexValueType<T>(T toStyle, string? overrideName = null)
    {
        var appendSettings = AppendSettings;
        var type           = typeof(T);
        var existingRefId  = SourceGraphVisitRefId(toStyle, type);
        var typeFormatter  = TypeFormattingOverrides.GetValueOrDefault(type, Settings.StyledTypeFormatter);
        var remainingDepth = (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1;
        var keyedCollectionBuilder =
            Recycler.Borrow<ComplexValueTypeMold>()
                    .InitializeComplexValueTypeBuilder
                        (type, this, appendSettings, overrideName ?? type.Name, remainingDepth, typeFormatter, existingRefId);
        TypeStart(toStyle, keyedCollectionBuilder, type);
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

    public ITheOneString Clear(int indentLevel = 0, SkipTypeParts ignoreWrite = SkipTypeParts.None)
    {
        initialAppendSettings = new MoldDieCastSettings(ignoreWrite);
        Sb?.Clear();
        Sb ??= BufferFactory();

        return this;
    }

    protected int NextRefId() => NextObjVisitedRefId++;

    protected void TypeStart<T>(T toStyle, TypeMolder newType, Type typeOfT)
    {
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, typeOfT, newType.IsComplexType
                                        , typeOfT.IsValueType ? null : toStyle, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, null)
        {
            TypeBuilderComponentAccess = ((ITypeBuilderComponentSource)newType).ComponentAccess
        };
        if (newVisit.ObjVisitIndex != OrderedObjectGraph.Count) throw new ArgumentException("ObjVisitIndex to be the size of OrderedObjectGraph");

        StartNewVisit(newType, newVisit);
    }

    protected void ObjectStart(object toStyle, TypeMolder newType, Type toStyleType)
    {
        var newVisit = new GraphNodeVisit(OrderedObjectGraph.Count, CurrentGraphNodeIndex, toStyleType, newType.IsComplexType
                                        , toStyle, (CurrentNode?.GraphDepth ?? -1) + 1
                                        , IndentLevel, Sb!.Length
                                        , (CurrentNode?.RemainingGraphDepth ?? Settings.DefaultGraphMaxDepth) - 1, null)
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
        OrderedObjectGraph.Add(newVisit);

        CurrentGraphNodeIndex  = newVisit.ObjVisitIndex;
        nextTypeAppendSettings = new MoldDieCastSettings(SkipTypeParts.None);
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
                if (HasVisited(objToStyle, type, graphNodeVisit))
                {
                    return i;
                }
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

        var idSpan = stackalloc char[refDigitsCount + 8].ResetMemory();
        idSpan.Append("\"$id\":\"");
        var insert = idSpan[7..];
        if (refId.TryFormat(insert, out var written, ""))
        {
            idSpan.Append("\"");
            var shiftBy = idSpan.Length;
            Sb!.InsertAt(idSpan, indexToInsertAt);
            shiftBy += Settings.StyledTypeFormatter.InsertFieldSeparatorAt(Sb!, indexToInsertAt + idSpan.Length, Settings, forThisNode.IndentLevel + 1);
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
      , int OriginalBufferTypeStart
      , int RemainingGraphDepth
      , MutableString? BackReferenceNames
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

        public GraphNodeVisit SetBufferTypeENd(int bufferIndex)
        {
            return this with
            {
                RefId = RefId
              , TypeBuilderComponentAccess = TypeBuilderComponentAccess
              , CurrentBufferTypeStart = CurrentBufferTypeStart
              , CurrentBufferFirstFieldStart = CurrentBufferFirstFieldStart
              , CurrentBufferTypeEnd = bufferIndex
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

        public int CurrentBufferTypeStart { get; init; } = OriginalBufferTypeStart;

        public int CurrentBufferTypeEnd { get; init; } = -1;

        public int CurrentBufferFirstFieldStart { get; init; }
    }
}
