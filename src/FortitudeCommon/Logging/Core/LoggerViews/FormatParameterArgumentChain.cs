// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.LoggerViews;

public class FormatParameterArgumentChain : RecyclableObject, ISingleInvokeArgumentChain
{
    private IFLogFirstFormatterParameterEntry?      firstParamBuilder;
    private IFLogAdditionalFormatterParameterEntry? continueParamBuilder;

    public FormatParameterArgumentChain Initialize(IFLogFirstFormatterParameterEntry firstFormatterParameter)
    {
        firstParamBuilder = firstFormatterParameter;

        return this;
    }

    private IFLogAdditionalFormatterParameterEntry? FirstOfMoreParam<T0>(T0 p0) =>
        firstParamBuilder != null
            ? firstParamBuilder.WithMatchParams(p0)
            : continueParamBuilder?.AndMatch(p0);

    public void Args<T0>(T0 p0)
    {
        if (firstParamBuilder != null)
            firstParamBuilder.WithOnlyMatchParam(p0);
        else
            continueParamBuilder?.AndFinalMatchParam(p0);
        DecrementRefCount();
    }

    public void Args<T0, T1>(T0 p0, T1 p1)
    {
        var next = AndMatchAll(p0);
        next?.AndFinalMatchParam(p1);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = AndMatchAll(p0, p1);
        next?.AndFinalMatchParam(p2);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = AndMatchAll(p0, p1, p2);
        next?.AndFinalMatchParam(p3);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = AndMatchAll(p0, p1, p2, p3);
        next?.AndFinalMatchParam(p4);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4);
        next?.AndFinalMatchParam(p5);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5);
        next?.AndFinalMatchParam(p6);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6);
        next?.AndFinalMatchParam(p7);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7);
        next?.AndFinalMatchParam(p8);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8);
        next?.AndFinalMatchParam(p9);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        next?.AndFinalMatchParam(p10);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        next?.AndFinalMatchParam(p11);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        next?.AndFinalMatchParam(p12);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        next?.AndFinalMatchParam(p13);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        next?.AndFinalMatchParam(p14);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        next?.AndFinalMatchParam(p15);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        next?.AndFinalMatchParam(p16);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
        next?.AndFinalMatchParam(p17);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17);
        next?.AndFinalMatchParam(p18);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18);
        next?.AndFinalMatchParam(p19);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19);
        next?.AndFinalMatchParam(p20);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20);
        next?.AndFinalMatchParam(p21);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21);
        next?.AndFinalMatchParam(p22);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22);
        next?.AndFinalMatchParam(p23);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23);
        next?.AndFinalMatchParam(p24);
        DecrementRefCount();
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0>(T0 p0)
    {
        var next = FirstOfMoreParam(p0);
        return next;
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1>(T0 p0, T1 p1)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?.AndMatch(p8)?
            .AndMatch(p9);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22);
    }

    protected IFLogAdditionalFormatterParameterEntry? AndMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = FirstOfMoreParam(p0);
        return next?.AndMatch(p1)?.AndMatch(p2)?.AndMatch(p3)?.AndMatch(p4)?.AndMatch(p5)?.AndMatch(p6)?.AndMatch(p7)?
            .AndMatch(p8)?.AndMatch(p9)?.AndMatch(p10)?.AndMatch(p11)?.AndMatch(p12)?.AndMatch(p13)?.AndMatch(p14)?
            .AndMatch(p15)?.AndMatch(p16)?.AndMatch(p17)?.AndMatch(p18)?.AndMatch(p19)?.AndMatch(p20)?.AndMatch(p21)?
            .AndMatch(p22)?.AndMatch(p23);
    }

    public IFLogStringAppender? ArgsThenAppender<T0>(T0 p0)
    {
        if (firstParamBuilder != null) return firstParamBuilder.AfterOnlyParamMatchToStringAppender(p0);
        return continueParamBuilder?.AfterFinalMatchParamToStringAppender(p0);
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1>(T0 p0, T1 p1)
    {
        var next = AndMatchAll(p0);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p1);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = AndMatchAll(p0, p1);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p2);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = AndMatchAll(p0, p1, p2);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p3);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = AndMatchAll(p0, p1, p2, p3);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p4);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next           = AndMatchAll(p0, p1, p2, p3, p4);
        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p5);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p6);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p7);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T7 p8)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p8);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p9);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p10);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p11);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p12);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p13);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p14);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p15);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p16);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p17);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p18);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p19);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p20);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p21);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p22);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p23);
        DecrementRefCount();
        return stringAppender;
    }

    public IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23);

        var stringAppender = next?.AfterFinalMatchParamToStringAppender(p24);
        DecrementRefCount();
        return stringAppender;
    }

    public ISingleInvokeArgumentChain ArgsAndContinue
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        var next = AndMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23);
        continueParamBuilder = next?.AndMatch(p10);
        return this;
    }
}
