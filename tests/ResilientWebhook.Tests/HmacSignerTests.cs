using ResilientWebhook.Dispatcher;
using Xunit;

namespace ResilientWebhook.Tests;

public class HmacSignerTests
{
    [Fact]
    public void ComputeSignature_ValidPayload_ReturnsDeterministicHash()
    {
        var payload = "{\"event\":\"order.created\",\"amount\":99.50}";
        var secret = "whsec_super_secret_test_key_12345";

        var signature1 = HmacSigner.ComputeSignature(payload, secret);
        var signature2 = HmacSigner.ComputeSignature(payload, secret);

        Assert.Equal(signature1, signature2);
        Assert.NotEmpty(signature1);
    }

    [Fact]
    public void VerifySignature_MatchingSignature_ReturnsTrue()
    {
        var payload = "{\"event\":\"payment.succeeded\"}";
        var secret = "whsec_secure_key_abcdef";

        var signature = HmacSigner.ComputeSignature(payload, secret);
        var isValid = HmacSigner.VerifySignature(payload, secret, signature);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifySignature_TamperedPayload_ReturnsFalse()
    {
        var payload = "{\"event\":\"payment.succeeded\"}";
        var tamperedPayload = "{\"event\":\"payment.succeeded\",\"amount\":1000000}";
        var secret = "whsec_secure_key_abcdef";

        var signature = HmacSigner.ComputeSignature(payload, secret);
        var isValid = HmacSigner.VerifySignature(tamperedPayload, secret, signature);

        Assert.False(isValid);
    }
}
