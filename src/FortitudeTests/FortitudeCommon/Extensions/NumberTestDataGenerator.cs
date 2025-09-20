using System.Numerics;
using System.Text;

namespace FortitudeTests.FortitudeCommon.Extensions;

public static class NumberTestDataGenerator
{
    private static Random random = new Random();

    public static void ReseedRandom(int seed)
    {
        random = new Random(seed);
    }

    public static IEnumerable<T> GenRandomNumberRange<T>(int numberToGenerate
      , double centreFromTotalRange = 0.5d, double maxPercentageFromCentre = 0.5d) where T : struct
    {
        Nullable<T> defaultValue = default;
        foreach (var nullableNum in defaultValue.GenerateNextSequence(centreFromTotalRange, maxPercentageFromCentre,  0.0d
                                                            , numberToGenerate))
        {
            yield return nullableNum!.Value;
        }
    }

    public static IEnumerable<T?> GenRandomNullableNumberRange<T>(int numberToGenerate, double nullChancePercentage = 0.5d
      , double centreFromTotalRange = 0.5d, double maxPercentageFromCentre = 0.5d) where T : struct
    {
        Nullable<T> defaultValue = default;
        foreach (var nullableNum in defaultValue.GenerateNextSequence(centreFromTotalRange, maxPercentageFromCentre,  nullChancePercentage
                                                            , numberToGenerate))
        {
            yield return nullableNum;
        }
    }

    public static IEnumerable<T?> GenerateNextSequence<T>(this T? dummyMatch, double centreFromTotalRange, double maxPercentageFromCentre
      ,double nullValuesPercentageFromCentre, int seqLength = 1)
        where T : struct
    {
        switch (dummyMatch)
        {
            case byte:
                var bCentre              = (byte)(byte.MaxValue * centreFromTotalRange);
                var bRangeFromCentre     = (byte)(byte.MaxValue * maxPercentageFromCentre);
                var bNullRangeFromCentre = (byte)(byte.MaxValue * nullValuesPercentageFromCentre);
                var bMinValue            = Math.Clamp(bCentre - bRangeFromCentre, byte.MinValue, byte.MaxValue);
                var bMaxValue            = Math.Clamp(bCentre + bRangeFromCentre, byte.MinValue, byte.MaxValue);
                if (bMaxValue <= bMinValue)
                {
                    if (bMaxValue > 0) bMinValue             = bMaxValue - 1;
                    if (bMinValue < byte.MaxValue) bMaxValue = bMinValue + 1;
                }
                var bNullMinValue = Math.Clamp(bCentre - bNullRangeFromCentre, byte.MinValue, byte.MaxValue);
                var bNullMaxValue = Math.Clamp(bCentre + bNullRangeFromCentre, byte.MinValue, byte.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (byte)random.Next(bMinValue, bMaxValue);
                    if (nextValue > bNullMinValue && nextValue < bNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case sbyte:
                var sbCentre              = (sbyte)((byte.MaxValue * centreFromTotalRange) + sbyte.MinValue);
                var sbRangeFromCentre     = (byte)(byte.MaxValue * maxPercentageFromCentre);
                var sbNullRangeFromCentre = (byte)(byte.MaxValue * nullValuesPercentageFromCentre);
                var sbMinValue            = Math.Clamp(sbCentre - sbRangeFromCentre, sbyte.MinValue, sbyte.MaxValue);
                var sbMaxValue            = Math.Clamp(sbCentre + sbRangeFromCentre, sbyte.MinValue, sbyte.MaxValue);
                if (sbMaxValue <= sbMinValue)
                {
                    if (sbMaxValue > sbyte.MinValue) sbMinValue = sbMaxValue - 1;
                    if (sbMinValue < sbyte.MaxValue) sbMaxValue = sbMinValue + 1;
                }
                var sbNullMinValue = Math.Clamp(sbCentre - sbNullRangeFromCentre, sbyte.MinValue, sbyte.MaxValue);
                var sbNullMaxValue = Math.Clamp(sbCentre + sbNullRangeFromCentre, sbyte.MinValue, sbyte.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (sbyte)random.Next(sbMinValue, sbMaxValue);
                    if (nextValue > sbNullMinValue && nextValue < sbNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case char:
                var utf16Max             = 0X00010000;
                var utf16Min             = 0X00000000;
                var cCentre              = (char)((utf16Max * centreFromTotalRange) + utf16Min);
                var cRangeFromCentre     = (char)(utf16Max * maxPercentageFromCentre);
                var cNullRangeFromCentre = (char)(utf16Max * nullValuesPercentageFromCentre);
                var cMinValue            = Math.Clamp(cCentre - cRangeFromCentre, utf16Min, utf16Max);
                var cMaxValue            = Math.Clamp(cCentre + cRangeFromCentre, utf16Min, utf16Max);
                if (cMaxValue <= cMinValue)
                {
                    if (cMaxValue > utf16Min) cMinValue = cMaxValue - 1;
                    if (cMinValue < utf16Max) cMaxValue = cMinValue + 1;
                }
                var cNullMinValue = Math.Clamp(cCentre - cNullRangeFromCentre, utf16Min, utf16Max);
                var cNullMaxValue = Math.Clamp(cCentre + cNullRangeFromCentre, utf16Min, utf16Max);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (char)random.Next(cMinValue, cMaxValue);
                    if (nextValue > cNullMinValue && nextValue < cNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case Rune:
                var utf32Max             = 0X0010FFFF;
                var utf32Min             = 0X00010000;
                var utf32Range           = utf32Max - utf32Min;
                var rCentre              = (int)((utf32Range * centreFromTotalRange) + utf32Min);
                var rRangeFromCentre     = (int)(utf32Range * maxPercentageFromCentre);
                var rNullRangeFromCentre = (char)(utf32Range * nullValuesPercentageFromCentre);
                var rMinValue            = Math.Clamp(rCentre - rRangeFromCentre, utf32Min, utf32Max);
                var rMaxValue            = Math.Clamp(rCentre + rRangeFromCentre, utf32Min, utf32Max);
                if (rMaxValue <= rMinValue)
                {
                    if (rMaxValue > utf32Min) rMinValue = rMaxValue - 1;
                    if (rMinValue < utf32Max) rMaxValue = rMinValue + 1;
                }
                var rNullMinValue = Math.Clamp(rCentre - rNullRangeFromCentre, utf32Min, utf32Max);
                var rNullMaxValue = Math.Clamp(rCentre + rNullRangeFromCentre, utf32Min, utf32Max);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = random.Next(rMinValue, rMaxValue);
                    if (nextValue > rNullMinValue && nextValue < rNullMaxValue) yield return null;
                    yield return (T)(ValueType)new Rune(nextValue);
                }
                yield break;
            case short:
                var sCentre              = (short)((ushort.MaxValue * centreFromTotalRange) + short.MinValue);
                var sRangeFromCentre     = (ushort)(ushort.MaxValue * maxPercentageFromCentre);
                var sNullRangeFromCentre = (ushort)(ushort.MaxValue * nullValuesPercentageFromCentre);
                var sMinValue            = Math.Clamp(sCentre - sRangeFromCentre, short.MinValue, short.MaxValue);
                var sMaxValue            = Math.Clamp(sCentre + sRangeFromCentre, short.MinValue, short.MaxValue);
                if (sMaxValue <= sMinValue)
                {
                    if (sMaxValue > short.MinValue) sMinValue = sMaxValue - 1;
                    if (sMinValue < short.MaxValue) sMaxValue = sMinValue + 1;
                }
                var sNullMinValue = Math.Clamp(sCentre - sNullRangeFromCentre, short.MinValue, short.MaxValue);
                var sNullMaxValue = Math.Clamp(sCentre + sNullRangeFromCentre, short.MinValue, short.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (short)random.Next(sMinValue, sMaxValue);
                    if (nextValue > sNullMinValue && nextValue < sNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case ushort:
                var usCentre             = (ushort)(ushort.MaxValue * centreFromTotalRange);
                var usRangeFromCentre    = (ushort)(ushort.MaxValue * maxPercentageFromCentre);
                var usNullRangeFromCentre = (ushort)(ushort.MaxValue * nullValuesPercentageFromCentre);
                var usMinValue           = Math.Clamp(usCentre - usRangeFromCentre, ushort.MinValue, ushort.MaxValue);
                var usMaxValue           = Math.Clamp(usCentre + usRangeFromCentre, ushort.MinValue, ushort.MaxValue);
                if (usMaxValue <= usMinValue)
                {
                    if (usMaxValue > ushort.MinValue) usMinValue = usMaxValue - 1;
                    if (usMinValue < ushort.MaxValue) usMaxValue = usMinValue + 1;
                }
                var usNullMinValue = Math.Clamp(usCentre - usNullRangeFromCentre, short.MinValue, short.MaxValue);
                var usNullMaxValue = Math.Clamp(usCentre + usNullRangeFromCentre, short.MinValue, short.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (ushort)random.Next(usMinValue, usMaxValue);
                    if (nextValue > usNullMinValue && nextValue < usNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case Half:
                var minusHalfMaxValue    = (Half.MaxValue * -Half.One);
                var hCentre              = (Half)((((double)Half.MaxValue * 2.0d) * centreFromTotalRange) + (double)minusHalfMaxValue);
                var hRangeFromCentre     = (Half)((((double)Half.MaxValue * 2.0d) * maxPercentageFromCentre));
                var hNullRangeFromCentre = (Half)((((double)Half.MaxValue * 2.0d) * nullValuesPercentageFromCentre));
                var hMinValue            = (Half)Math.Clamp((float)(hCentre - hRangeFromCentre), (float)minusHalfMaxValue, (float)Half.MaxValue);
                var hMaxValue            = (Half)Math.Clamp((float)(hCentre + hRangeFromCentre), (float)minusHalfMaxValue, (float)Half.MaxValue);
                if (hMaxValue <= hMinValue)
                {
                    if (hMaxValue > minusHalfMaxValue + Half.One) hMinValue = hMaxValue - Half.One;
                    if (hMinValue < (Half.MaxValue - Half.One)) hMaxValue   = hMinValue + Half.One;
                }
                var hNullMinValue = (Half)Math.Clamp((float)(hCentre - hNullRangeFromCentre), (float)minusHalfMaxValue, (float)Half.MaxValue);
                var hNullMaxValue = (Half)Math.Clamp((float)(hCentre + hNullRangeFromCentre), (float)minusHalfMaxValue, (float)Half.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (Half)(random.NextDouble() * ((double)hMaxValue - (double)hMinValue) + (double)hMinValue);
                    if (nextValue > hNullMinValue && nextValue < hNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case int:
                var iCentre              = (int)((uint.MaxValue * centreFromTotalRange) + int.MinValue);
                var iRangeFromCentre     = (uint)(uint.MaxValue * maxPercentageFromCentre);
                var iNullRangeFromCentre = (uint)(uint.MaxValue * nullValuesPercentageFromCentre);
                var iMinValue            = Math.Clamp((int)(iCentre - iRangeFromCentre), int.MinValue, int.MaxValue);
                var iMaxValue            = Math.Clamp((int)(iCentre + iRangeFromCentre), int.MinValue, int.MaxValue);
                if (iMaxValue <= iMinValue)
                {
                    if (iMaxValue > int.MinValue) iMinValue = iMaxValue - 1;
                    if (iMinValue < int.MaxValue) iMaxValue = iMinValue + 1;
                }
                var iNullMinValue = Math.Clamp(iCentre - iNullRangeFromCentre, int.MinValue, int.MaxValue);
                var iNullMaxValue = Math.Clamp(iCentre + iNullRangeFromCentre, int.MinValue, int.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = random.Next(iMinValue, iMaxValue);
                    if (nextValue > iNullMinValue && nextValue < iNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case uint:
                var uiCentre             = (uint)(uint.MaxValue * centreFromTotalRange);
                var uiRangeFromCentre    = (uint)(uint.MaxValue * maxPercentageFromCentre);
                var uiNullRangeFromCentre = (uint)(uint.MaxValue * nullValuesPercentageFromCentre);
                var uiMinValue           = Math.Clamp(uiCentre - uiRangeFromCentre, uint.MinValue, uint.MaxValue);
                var uiMaxValue           = Math.Clamp(uiCentre + uiRangeFromCentre, uint.MinValue, uint.MaxValue);
                if (uiMaxValue <= uiMinValue)
                {
                    if (uiMaxValue > uint.MinValue) uiMinValue = uiMaxValue - 1;
                    if (uiMinValue < uint.MaxValue) uiMaxValue = uiMinValue + 1;
                }
                var uiNullMinValue = Math.Clamp(uiCentre - uiNullRangeFromCentre, uint.MinValue, uint.MaxValue);
                var uiNullMaxValue = Math.Clamp(uiCentre + uiNullRangeFromCentre, uint.MinValue, uint.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (uint)(random.NextDouble() * (uiMaxValue - uiMinValue) + uiMinValue);
                    if (nextValue > uiNullMinValue && nextValue < uiNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case float:
                var minusFloatMaxValue    = (float.MaxValue * -1f);
                var fCentre               = (float)(((float.MaxValue * 2.0d) * centreFromTotalRange) + minusFloatMaxValue);
                var fRangeFromCentre      = (float)(((float.MaxValue * 2.0d) * maxPercentageFromCentre));
                var fNullRangeFromCentre = (float)(((float.MaxValue * 2.0d) * nullValuesPercentageFromCentre));
                var fMinValue             = Math.Clamp((fCentre - fRangeFromCentre), minusFloatMaxValue, float.MaxValue);
                var fMaxValue             = Math.Clamp((fCentre + fRangeFromCentre), minusFloatMaxValue, float.MaxValue);
                if (fMaxValue <= fMinValue)
                {
                    if (fMaxValue > minusFloatMaxValue + 1f) fMinValue = fMaxValue - 1f;
                    if (fMinValue < (float.MaxValue - 1f)) fMaxValue    = fMinValue + 1f;
                }
                var fNullMinValue = Math.Clamp((fCentre - fNullRangeFromCentre), minusFloatMaxValue, float.MaxValue);
                var fNullMaxValue = Math.Clamp((fCentre + fNullRangeFromCentre), minusFloatMaxValue, float.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (float)(random.NextDouble() * (fMaxValue - fMinValue) + fMinValue);
                    if (nextValue > fNullMinValue && nextValue < fNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case long:
                var lCentre               = (long)((ulong.MaxValue * centreFromTotalRange) + long.MinValue);
                var lRangeFromCentre      = (ulong)(ulong.MaxValue * maxPercentageFromCentre);
                var lNullRangeFromCentre = (ulong)(ulong.MaxValue * nullValuesPercentageFromCentre);
                var lMinValue             = Math.Clamp((long)((decimal)lCentre - lRangeFromCentre), long.MinValue, long.MaxValue);
                var lMaxValue             = Math.Clamp((long)((decimal)lCentre + lRangeFromCentre), long.MinValue, long.MaxValue);
                if (lMaxValue <= lMinValue)
                {
                    if (lMaxValue > long.MinValue) lMinValue = lMaxValue - 1;
                    if (lMinValue < long.MaxValue) lMaxValue = lMinValue + 1;
                }
                var lNullMinValue = Math.Clamp((long)((decimal)lCentre - lNullRangeFromCentre), long.MinValue, long.MaxValue);
                var lNullMaxValue = Math.Clamp((long)((decimal)lCentre + lNullRangeFromCentre), long.MinValue, long.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (long)(random.NextDouble() * (lMaxValue - lMinValue) + lMinValue);
                    if (nextValue > lNullMinValue && nextValue < lNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case ulong:
                var ulCentre             = (ulong)(ulong.MaxValue * centreFromTotalRange);
                var ulRangeFromCentre    = (ulong)(ulong.MaxValue * maxPercentageFromCentre);
                var ulNullRangeFromCentre = (ulong)(ulong.MaxValue * nullValuesPercentageFromCentre);
                var ulMinValue           = Math.Clamp(ulCentre - ulRangeFromCentre, ulong.MinValue, ulong.MaxValue);
                var ulMaxValue           = Math.Clamp(ulCentre + ulRangeFromCentre, ulong.MinValue, ulong.MaxValue);
                if (ulMaxValue <= ulMinValue)
                {
                    if (ulMaxValue > ulong.MinValue) ulMinValue = ulMaxValue - 1;
                    if (ulMinValue < ulong.MaxValue) ulMaxValue = ulMinValue + 1;
                }
                var ulNullMinValue = Math.Clamp(ulCentre - ulNullRangeFromCentre, ulong.MinValue, ulong.MaxValue);
                var ulNullMaxValue = Math.Clamp(ulCentre + ulNullRangeFromCentre, ulong.MinValue, ulong.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (ulong)(random.NextDouble() * (ulMaxValue - ulMinValue) + ulMinValue);
                    if (nextValue > ulNullMinValue && nextValue < ulNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case double:
                var minusDoubleMaxValue  = (double.MaxValue * -1d);
                var dCentre              = ((double.MaxValue * centreFromTotalRange) + minusDoubleMaxValue/2);
                var dRangeFromCentre     = double.MaxValue * maxPercentageFromCentre;
                var dNullRangeFromCentre = double.MaxValue * nullValuesPercentageFromCentre;
                var dMinValue            = Math.Clamp((dCentre - dRangeFromCentre), minusDoubleMaxValue, double.MaxValue);
                var dMaxValue            = Math.Clamp((dCentre + dRangeFromCentre), minusDoubleMaxValue, double.MaxValue);
                if (dMaxValue <= dMinValue)
                {
                    if (dMaxValue > minusDoubleMaxValue + 1d) dMinValue = dMaxValue - 1d;
                    if (dMinValue < (double.MaxValue - 1d)) dMaxValue   = dMinValue + 1d;
                }
                var dNullMinValue = Math.Clamp((dCentre - dNullRangeFromCentre), minusDoubleMaxValue, double.MaxValue);
                var dNullMaxValue = Math.Clamp((dCentre + dNullRangeFromCentre), minusDoubleMaxValue, double.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (random.NextDouble() * (dMaxValue - dMinValue) + dMinValue);
                    if (nextValue > dNullMinValue && nextValue < dNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case decimal:
                var minusDecimalMaxValue = (decimal.MaxValue * decimal.MinusOne);
                var mCentre              = ((decimal.MaxValue * (decimal)centreFromTotalRange) + minusDecimalMaxValue/2);
                var mRangeFromCentre     = decimal.MaxValue * (decimal)maxPercentageFromCentre;
                var mNullRangeFromCentre = decimal.MaxValue * (decimal)nullValuesPercentageFromCentre;
                var mMinValue            = Math.Clamp((mCentre - mRangeFromCentre), minusDecimalMaxValue, decimal.MaxValue);
                var mMaxValue            = Math.Clamp((mCentre + mRangeFromCentre), minusDecimalMaxValue, decimal.MaxValue);
                if (mMaxValue <= mMinValue)
                {
                    if (mMaxValue > minusDecimalMaxValue + 1m) mMinValue = mMaxValue - 1m;
                    if (mMinValue < (decimal.MaxValue - 1m)) mMaxValue   = mMinValue + 1m;
                }
                var mNullMinValue = Math.Clamp((mCentre - mNullRangeFromCentre), minusDecimalMaxValue, decimal.MaxValue);
                var mNullMaxValue = Math.Clamp((mCentre - mNullRangeFromCentre), minusDecimalMaxValue, decimal.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = ((decimal)random.NextDouble() * (mMaxValue - mMinValue) + mMinValue);
                    if (nextValue > mNullMinValue && nextValue < mNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case Int128:
                var vlCentre             = ((Int128)((double)Int128.MaxValue * 2.0 * centreFromTotalRange) + Int128.MinValue);
                var vlRangeFromCentre    = (Int128.MaxValue * (BigInteger)(2_000_000_000 * maxPercentageFromCentre)/(1_000_000_000));
                var vlNullRangeFromCentre = (Int128.MaxValue * (BigInteger)(2_000_000_000 * nullValuesPercentageFromCentre)/(1_000_000_000));
                var vlDistanceToMin      = (vlCentre - Int128.MinValue);
                var vlDistanceToMax      = Int128.MaxValue - vlCentre;
                vlRangeFromCentre = vlRangeFromCentre > vlDistanceToMin ? vlDistanceToMin : vlRangeFromCentre;
                vlRangeFromCentre = vlRangeFromCentre > vlDistanceToMax ? vlDistanceToMax : vlRangeFromCentre;
                var vlMinValue        = (Int128)(vlCentre - vlRangeFromCentre);
                var vlMaxValue        = (Int128)(vlCentre + vlRangeFromCentre);
                if (vlMaxValue <= vlMinValue)
                {
                    if (vlMaxValue > Int128.MinValue) vlMinValue = vlMaxValue - 1;
                    if (vlMinValue < Int128.MaxValue) vlMaxValue = vlMinValue + 1;
                }
                vlNullRangeFromCentre = vlNullRangeFromCentre > vlDistanceToMin ? vlDistanceToMin : vlNullRangeFromCentre;
                vlNullRangeFromCentre = vlNullRangeFromCentre > vlDistanceToMax ? vlDistanceToMax : vlNullRangeFromCentre;
                var vlNullMinValue = (Int128)(vlCentre - vlNullRangeFromCentre);
                var vlNullMaxValue = (Int128)(vlCentre + vlNullRangeFromCentre);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (Int128)(random.NextDouble() * (double)(vlMaxValue - vlMinValue) + (double)vlMinValue);
                    if (nextValue > vlNullMinValue && nextValue < vlNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case UInt128:
                var vulCentre              = (UInt128)((double)UInt128.MaxValue * centreFromTotalRange);
                var vulRangeFromCentre     = (UInt128)(UInt128.MaxValue * (BigInteger)(2_000_000_000 * maxPercentageFromCentre)/(2_000_000_000));
                var vulNullRangeFromCentre = (UInt128)(UInt128.MaxValue * (BigInteger)(2_000_000_000 * nullValuesPercentageFromCentre)/(2_000_000_000));
                var vulDistanceToMin       = (vulCentre - UInt128.MinValue);
                var vulDistanceToMax       = UInt128.MaxValue - vulCentre;
                vulRangeFromCentre = vulRangeFromCentre > vulDistanceToMin ? vulDistanceToMin : vulRangeFromCentre;
                vulRangeFromCentre = vulRangeFromCentre > vulDistanceToMax ? vulDistanceToMax : vulRangeFromCentre;
                var vulMinValue        = vulCentre - vulRangeFromCentre;
                var vulMaxValue        = vulCentre + vulRangeFromCentre;
                if (vulMaxValue <= vulMinValue)
                {
                    if (vulMaxValue > UInt128.MinValue) vulMinValue = vulMaxValue - 1;
                    if (vulMinValue < UInt128.MaxValue) vulMaxValue = vulMinValue + 1;
                }
                vulNullRangeFromCentre = vulNullRangeFromCentre > vulDistanceToMin ? vulDistanceToMin : vulNullRangeFromCentre;
                vulNullRangeFromCentre = vulNullRangeFromCentre > vulDistanceToMax ? vulDistanceToMax : vulNullRangeFromCentre;
                var vulNullMinValue = (vulCentre - vulNullRangeFromCentre);
                var vulNullMaxValue = (vulCentre + vulNullRangeFromCentre);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (UInt128)(random.NextDouble() * (double)(vulMaxValue - vulMinValue) + (double)vulMinValue);
                    if (nextValue > vulNullMinValue && nextValue < vulNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case BigInteger:
                var myBigIntMax = (BigInteger)decimal.MaxValue * 1_000;
                var myBigIntMin = (BigInteger)decimal.MaxValue * -1_000;
                
                var biCentre               = (BigInteger)((double)myBigIntMax * centreFromTotalRange);
                var biRangeFromCentre      = myBigIntMax * (BigInteger)(2 * maxPercentageFromCentre);
                var biNullRangeFromCentre = myBigIntMax * (BigInteger)(2 * nullValuesPercentageFromCentre);
                var biDistanceToMin        = (biCentre - myBigIntMin);
                var biDistanceToMax        = myBigIntMax - biCentre;
                biRangeFromCentre = biRangeFromCentre > biDistanceToMin ? biDistanceToMin : biRangeFromCentre;
                biRangeFromCentre = biRangeFromCentre > biDistanceToMax ? biDistanceToMax : biRangeFromCentre;
                var biMinValue        = biCentre - biRangeFromCentre;
                var biMaxValue        = biCentre + biRangeFromCentre;
                if (biMaxValue <= biMinValue)
                {
                    if (biMaxValue > myBigIntMin) biMinValue = biMaxValue - 1;
                    if (biMinValue < myBigIntMax) biMaxValue = biMinValue + 1;
                }
                biNullRangeFromCentre = biNullRangeFromCentre > biDistanceToMin ? biDistanceToMin : biNullRangeFromCentre;
                biNullRangeFromCentre = biNullRangeFromCentre > biDistanceToMax ? biDistanceToMax : biNullRangeFromCentre;
                var bilNullMinValue = (biCentre - biNullRangeFromCentre);
                var bilNullMaxValue = (biCentre + biNullRangeFromCentre);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextValue = (BigInteger)(random.NextDouble() * (double)(biMaxValue - biMinValue) + (double)biMinValue);
                    if (nextValue > bilNullMinValue && nextValue < bilNullMaxValue) yield return null;
                    yield return (T)(ValueType)nextValue;
                }
                yield break;
            case Complex:
                var minusComplexDoubleMaxValue  = (double.MaxValue * -1d);
                var cmplxCentre              = ((double.MaxValue * centreFromTotalRange) + minusComplexDoubleMaxValue/2);
                var cmplxRangeFromCentre     = double.MaxValue * maxPercentageFromCentre;
                var cmplxNullRangeFromCentre = double.MaxValue * nullValuesPercentageFromCentre;
                var cmplxMinValue            = Math.Clamp((cmplxCentre - cmplxRangeFromCentre), minusComplexDoubleMaxValue, double.MaxValue);
                var cmplxMaxValue            = Math.Clamp((cmplxCentre + cmplxRangeFromCentre), minusComplexDoubleMaxValue, double.MaxValue);
                if (cmplxMaxValue <= cmplxMinValue)
                {
                    if (cmplxMaxValue > minusComplexDoubleMaxValue + 1d) cmplxMinValue = cmplxMaxValue - 1d;
                    if (cmplxMinValue < (double.MaxValue - 1d)) cmplxMaxValue   = cmplxMinValue + 1d;
                }
                var cmplxNullMinValue = Math.Clamp((cmplxCentre - cmplxNullRangeFromCentre), minusComplexDoubleMaxValue, double.MaxValue);
                var cmplxNullMaxValue = Math.Clamp((cmplxCentre + cmplxNullRangeFromCentre), minusComplexDoubleMaxValue, double.MaxValue);
                for (int i = 0; i < seqLength; i++)
                {
                    var nextRealValue = (random.NextDouble() * (cmplxMaxValue - cmplxMinValue) + cmplxMinValue);
                    if (nextRealValue > cmplxNullMinValue && nextRealValue < cmplxNullMaxValue) yield return null;
                    var nextImaginaryValue = (random.NextDouble() * (cmplxMaxValue - cmplxMinValue) + cmplxMinValue);
                    yield return (T)(ValueType)new Complex(nextRealValue, nextImaginaryValue);
                }
                yield break;
        }
    }
}
