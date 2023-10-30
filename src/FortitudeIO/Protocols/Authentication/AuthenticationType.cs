namespace FortitudeIO.Protocols.Authentication
{
    public enum AuthenticationType : byte
    {
        None = 0,
        UnencyrptedPassword = 1,
        SymetricKeyEncryptedPassword = 2,
        SingleHashedToken = 3,
        SingleDoubleSessionToken = 4,
        ContinousDoubleSessionToken = 5
    }
}