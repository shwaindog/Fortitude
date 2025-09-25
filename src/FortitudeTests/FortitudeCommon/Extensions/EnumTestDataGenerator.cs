namespace FortitudeTests.FortitudeCommon.Extensions;

public class EnumTestDataGenerator
{
    private static Random random = new Random();

    public static void ReseedRandom(int seed)
    {
        random = new Random(seed);
    }
    
    public static IEnumerable<TEnum> GenRandomEnumValues<TEnum>(int numberToGenerate, double chanceOfZero = 0.1d, double chanceOfUnspecifiedValue = 0.0d)
        where TEnum : struct, Enum
    {
        foreach (var nullableEnum in GenRandomNullableEnumValues<TEnum>(numberToGenerate, 0.0d, chanceOfZero, chanceOfUnspecifiedValue))
        {
            yield return nullableEnum!.Value;
        }
    }

    public static IEnumerable<TEnum?> GenRandomNullableEnumValues<TEnum>(int numberToGenerate, double chanceOfNullPercentage = 0.0d
      , double chanceOfZero = 0.1d, double chanceOfUnspecifiedValue = 0.0d)
        where TEnum : struct, Enum
    {
        var enumType   = typeof(TEnum);
        var enumValues = (TEnum[])Enum.GetValues(enumType);

        for (int i = 0; i < numberToGenerate; i++)
        {
            if (chanceOfNullPercentage > 0d)
            {
                var rollForNull = random.NextDouble();
                if (chanceOfNullPercentage > rollForNull)
                {
                    yield return null;
                    continue;
                }
            }
            if (chanceOfZero > 0d)
            {
                var rollForZero = random.NextDouble();
                if (chanceOfZero > rollForZero)
                {
                    yield return (TEnum)(ValueType)0;
                    continue;
                }
            }
            if (chanceOfUnspecifiedValue > 0d)
            {
                var rollForZero = random.NextDouble();
                if (chanceOfUnspecifiedValue > rollForZero)
                {
                    yield return (TEnum)(ValueType)random.NextInt64();
                    continue;
                }
            }
            var someIndex = random.Next(0, enumValues.Length * 2) % enumValues.Length;
            yield return enumValues[someIndex];
        }
    }
    
    public static IEnumerable<TEnum> GenRandomEnumMultiFlagValues<TEnum>(int numberToGenerate
      , int minFlagsPerEnum = 1, int maxFlagsPerEnum = 10,  double chanceOfZero = 0.1d, double chanceOfUnspecifiedValue = 0.0d)
        where TEnum : struct, Enum
    {
        foreach (var nullableEnum in GenRandomNullableEnumMultiFlagValues<TEnum>
                     (numberToGenerate,minFlagsPerEnum, maxFlagsPerEnum, 0.0d, chanceOfZero, chanceOfUnspecifiedValue))
        {
            yield return nullableEnum!.Value;
        }
    }

    public static IEnumerable<TEnum?> GenRandomNullableEnumMultiFlagValues<TEnum>(int numberToGenerate
       , int minFlagsPerEnum = 1, int maxFlagsPerEnum = 10,  double chanceOfNullPercentage = 0.0d
      , double chanceOfZero = 0.1d, double chanceOfUnspecifiedValue = 0.0d)
        where TEnum : struct, Enum, IConvertible
    {
        var enumType = typeof(TEnum);
        var enumValues = (TEnum[])Enum.GetValues(enumType);
        
        for (int i = 0; i < numberToGenerate; i++)
        {
            if (chanceOfNullPercentage > 0d)
            {
                var rollForNull = random.NextDouble();
                if (chanceOfNullPercentage > rollForNull)
                {
                    yield return null;
                    continue;
                }
            }
            if (chanceOfZero > 0d)
            {
                var rollForZero = random.NextDouble();
                if (chanceOfZero > rollForZero)
                {
                    yield return (TEnum)(ValueType)0;
                    continue;
                }
            }
            if (chanceOfUnspecifiedValue > 0d)
            {
                var rollForZero = random.NextDouble();
                if (chanceOfUnspecifiedValue > rollForZero)
                {
                    yield return (TEnum)(ValueType)random.NextInt64();
                    continue;
                }
            }
            var  numOfFlags = random.Next(minFlagsPerEnum, maxFlagsPerEnum);
            long enumSoFar  = 0;
            for (int j = 0; j < numOfFlags; j++)
            {
                var  someIndex  = random.Next(0, enumValues.Length*2) % enumValues.Length;
                enumSoFar |= enumValues[someIndex].ToInt64(null);
            }
            yield return (TEnum)(ValueType)enumSoFar;
        }
    }
}
