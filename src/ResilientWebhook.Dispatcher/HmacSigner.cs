using System.Security.Cryptography;
using System.Text;

namespace ResilientWebhook.Dispatcher;

public static class HmacSigner
{
    public static string ComputeSignature(string payload, string secret)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static bool VerifySignature(string payload, string secret, string expectedSignature)
    {
        var actualSignature = ComputeSignature(payload, secret);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(actualSignature),
            Encoding.UTF8.GetBytes(expectedSignature.ToLowerInvariant())
        );
    }
}
