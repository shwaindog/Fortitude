namespace FortitudeIO.Protocols.Serdes.Binary;

public static class SerdesExtensions
{
    public static IMessageDeserializationRepository? FindConnectedFallbackWithName(this IMessageDeserializationRepository root, string? name)
    {
        var checkRepository = root;
        while (checkRepository != null && name != null)
        {
            if (checkRepository.Name == name) return checkRepository;
            checkRepository = checkRepository.CascadingFallbackDeserializationRepo;
        }

        return null;
    }

    public static IMessageDeserializationRepository GetDeepestConnectedFallback(this IMessageDeserializationRepository root)
    {
        var checkRepository = root;
        var deepestSoFar = root;
        while (checkRepository != null)
        {
            deepestSoFar = checkRepository;
            checkRepository = checkRepository.CascadingFallbackDeserializationRepo;
        }

        return deepestSoFar;
    }

    public static bool IsInConnectedFallback(this IMessageDeserializationRepository root
        , IMessageDeserializationRepository? checkIsInConnectedFallback)
    {
        if (checkIsInConnectedFallback == null) return false;
        var checkRepository = root;
        while (checkRepository != null)
        {
            if (checkRepository == checkIsInConnectedFallback) return true;
            checkRepository = checkRepository.CascadingFallbackDeserializationRepo;
        }

        return false;
    }

    public static void AttachToEndOfConnectedFallbackRepos(this IMessageDeserializationRepository root, IMessageDeserializationRepository attachAtEnd)
    {
        var deepestSoFar = root.GetDeepestConnectedFallback();
        deepestSoFar.CascadingFallbackDeserializationRepo = attachAtEnd;
    }
}
