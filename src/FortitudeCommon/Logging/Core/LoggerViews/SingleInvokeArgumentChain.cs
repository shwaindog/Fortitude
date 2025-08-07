using FortitudeCommon.Logging.Core.LogEntries;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ISingleInvokeArgumentChain
{
    void Args<T0>(T0 p0);
    void Args<T0, T1>(T0 p0, T1 p1);
    void Args<T0, T1, T2>(T0 p0, T1 p1, T2 p2);
    void Args<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3);
    void Args<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4);
    void Args<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    void Args<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9
      , T10 p10, T11 p11, T12 p12);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8
      , T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7
      , T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6
      , T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6
      , T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , T20 p20);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19
      , T20 p20, T21 p21);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>(T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18
      , T19 p19, T20 p20, T21 p21, T22 p22);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23);

    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
          , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned ISingleInvokeArgumentChain")]
    ISingleInvokeArgumentChain ArgsAndContinue<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15
      , T16 p16, T17 p17, T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24);

    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0>(T0 p0);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1>(T0 p0, T1 p1);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2>(T0 p0, T1 p1, T2 p2);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3>(T0 p0, T1 p1, T2 p2, T3 p3);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T7 p8);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7
      , T8 p8, T9 p9);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7
      , T8 p8, T9 p9, T10 p10);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5
      , T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
      , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T0 p0, T1 p1, T2 p2, T3 p3
      , T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T0 p0, T1 p1, T2 p2
      , T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T0 p0, T1 p1
      , T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18
      , T19 p19);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(T0 p0
      , T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20, T21 p21);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22>
    (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
      , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender
    <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4
          , T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17, T18 p18, T19 p19, T20 p20
          , T21 p21, T22 p22, T23 p23);
    
    [MustUseReturnValue("Use Args if you do not plan on using the returned IFLogStringAppender")]
    IFLogStringAppender? ArgsThenAppender
        <T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21, T22, T23, T24>
        (T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16, T17 p17
          , T18 p18, T19 p19, T20 p20, T21 p21, T22 p22, T23 p23, T24 p24);
}