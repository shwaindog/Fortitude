using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T7 p8);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9);
    void Args<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10);
}