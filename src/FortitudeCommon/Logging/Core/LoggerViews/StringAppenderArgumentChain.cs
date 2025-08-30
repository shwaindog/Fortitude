// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LoggerViews;

internal class StringAppenderArgumentChain : RecyclableObject, ISingleInvokeArgumentChain
{
    private IFLogStringAppender stringAppender = null!;

    public StringAppenderArgumentChain Initialize(IFLogStringAppender flogStringAppender)
    {
        stringAppender = flogStringAppender;

        return this;
    }


    public void Args<T0>(T0 p0)
    {
        stringAppender.FinalMatchAppend(p0);
        DecrementRefCount();
    }

    public void Args<T0, T1>(T0 p0, T1 p1)
    {
        var next = AppendMatchAll(p0);
        next.FinalMatchAppend(p1);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = AppendMatchAll(p0, p1);
        next.FinalMatchAppend(p2);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = AppendMatchAll(p0, p1, p2);
        next.FinalMatchAppend(p3);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = AppendMatchAll(p0, p1, p2, p3);
        next.FinalMatchAppend(p4);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4);
        next.FinalMatchAppend(p5);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5);
        next.FinalMatchAppend(p6);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6);
        next.FinalMatchAppend(p7);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7);
        next.FinalMatchAppend(p8);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8);
        next.FinalMatchAppend(p9);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        next.FinalMatchAppend(p10);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        next.FinalMatchAppend(p11);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        next.FinalMatchAppend(p12);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        next.FinalMatchAppend(p13);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7
      , T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        next.FinalMatchAppend(p14);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6
      , T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        next.FinalMatchAppend(p15);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        next.FinalMatchAppend(p16);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
        next.FinalMatchAppend(p17);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17);
        next.FinalMatchAppend(p18);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18);
        next.FinalMatchAppend(p19);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19);
        next.FinalMatchAppend(p20);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , T20 p20, T21 p21)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20);
        next.FinalMatchAppend(p21);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21);
        next.FinalMatchAppend(p22);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16
      , T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22);
        next.FinalMatchAppend(p23);
        DecrementRefCount();
    }

    public void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16
      , T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23);
        next.FinalMatchAppend(p24);
        DecrementRefCount();
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0>(T0 p0) => stringAppender.AppendMatch(p0);

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1>(T0 p0, T1 p1)
    {
        var next = AppendMatchAll(p0, p1);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = AppendMatchAll(p0, p1, p2);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = AppendMatchAll(p0, p1, p2, p3);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T7 p8)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
      , T15 p15)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19, p20);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19, p20, p21);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19, p20, p21, p22);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19, p20, p21, p22, p23);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public IFLogStringAppender ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        var next = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                , p14, p15, p16, p17, p18, p19, p20, p21, p22, p23).AppendMatch(p24);
        DecrementRefCount();
        return next;
    }

    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    public ISingleInvokeArgumentChain ArgsAndContinue
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14
          , T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24)
    {
        stringAppender = AppendMatchAll(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13
                                      , p14, p15, p16, p17, p18, p19, p20, p21, p22, p23).AppendMatch(p24);
        return this;
    }

    protected IFLogStringAppender AppendMatchAll<T0>(T0 p0)
    {
        var next = stringAppender.AppendMatch(p0);
        return next;
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1>(T0 p0, T1 p1)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2>(T0 p0, T1 p1, T2 p2)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8)
    {
        var next = stringAppender.AppendMatch(p0);
        return next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7)
                   .AppendMatch(p8);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13).AppendMatch(p14);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18).AppendMatch(p19);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18).AppendMatch(p19)
                   .AppendMatch(p20);
    }

    protected IFLogStringAppender AppendMatchAll<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20, T21 p21)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18).AppendMatch(p19)
                   .AppendMatch(p20).AppendMatch(p21);
    }

    protected IFLogStringAppender AppendMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
          , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18).AppendMatch(p19)
                   .AppendMatch(p20).AppendMatch(p21).AppendMatch(p22);
    }

    protected IFLogStringAppender AppendMatchAll
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
          , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23)
    {
        var next = stringAppender.AppendMatch(p0);
        next = next.AppendMatch(p1).AppendMatch(p2).AppendMatch(p3).AppendMatch(p4).AppendMatch(p5).AppendMatch(p6).AppendMatch(p7);
        return next.AppendMatch(p8).AppendMatch(p9).AppendMatch(p10).AppendMatch(p11).AppendMatch(p12).AppendMatch(p13)
                   .AppendMatch(p14).AppendMatch(p15).AppendMatch(p16).AppendMatch(p17).AppendMatch(p18).AppendMatch(p19)
                   .AppendMatch(p20).AppendMatch(p21).AppendMatch(p22).AppendMatch(p23);
    }
}
