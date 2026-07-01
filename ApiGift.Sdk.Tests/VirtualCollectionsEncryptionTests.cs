using ApiGift.Sdk.V2.Inventory;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace ApiGift.Sdk.Tests;

public sealed class VirtualCollectionsEncryptionTests
{
    private const string InitializationVector = "OFRna73mXaze01xY";

    [Fact]
    public void TryDecryptEncryptedVirtualCollectionsReturnsPlaintext()
    {
        const string secretKey = "merchant-secret";
        const string expectedPlaintext = """{"code":"ABC-123","pin":"9876"}""";
        string encryptedVirtualCollections = Encrypt(
            expectedPlaintext,
            secretKey);

        bool decrypted =
            VirtualCollectionsEncryption.TryDecryptEncryptedVirtualCollections(
                encryptedVirtualCollections,
                secretKey,
                out string plaintext);

        Assert.True(decrypted);
        Assert.Equal(expectedPlaintext, plaintext);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("not-base64")]
    public void TryDecryptEncryptedVirtualCollectionsReturnsFalseForInvalidCiphertext(
        string encryptedVirtualCollections)
    {
        bool decrypted =
            VirtualCollectionsEncryption.TryDecryptEncryptedVirtualCollections(
                encryptedVirtualCollections,
                "merchant-secret",
                out string plaintext);

        Assert.False(decrypted);
        Assert.Equal(string.Empty, plaintext);
    }

    [Fact]
    public void TryDecryptEncryptedVirtualCollectionsReturnsFalseForWrongSecret()
    {
        string encryptedVirtualCollections = Encrypt(
            "plaintext",
            "merchant-secret");

        bool decrypted =
            VirtualCollectionsEncryption.TryDecryptEncryptedVirtualCollections(
                encryptedVirtualCollections,
                "wrong-secret",
                out string plaintext);

        Assert.False(decrypted);
        Assert.Equal(string.Empty, plaintext);
    }

    private static string Encrypt(string plaintext, string secretKey)
    {
        byte[] keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey));
        byte[] ivBytes = Encoding.UTF8.GetBytes(InitializationVector);

        using Aes aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        byte[] cipherBytes = encryptor.TransformFinalBlock(
            plaintextBytes,
            0,
            plaintextBytes.Length);

        return Convert.ToBase64String(cipherBytes);
    }
}
