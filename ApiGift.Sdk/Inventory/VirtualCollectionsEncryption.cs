using System.Security.Cryptography;
using System.Text;

namespace ApiGift.Sdk.V2.Inventory;

/// <summary>
/// Provides helpers for encrypted virtual collection payloads.
/// </summary>
public static class VirtualCollectionsEncryption
{
    private const string InitializationVector = "OFRna73mXaze01xY";

    /// <summary>
    /// Attempts to decrypt encrypted virtual collection data.
    /// </summary>
    public static bool TryDecryptEncryptedVirtualCollections(
        string encryptedVirtualCollections,
        string secretKey,
        out string plaintext)
    {
        plaintext = string.Empty;
        if (string.IsNullOrWhiteSpace(encryptedVirtualCollections)
            || string.IsNullOrWhiteSpace(secretKey))
        {
            return false;
        }

        try
        {
            byte[] cipherBytes = Convert.FromBase64String(encryptedVirtualCollections.Trim());
            byte[] keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(secretKey));
            byte[] ivBytes = Encoding.UTF8.GetBytes(InitializationVector);

            using Aes aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] plaintextBytes = decryptor.TransformFinalBlock(
                cipherBytes,
                0,
                cipherBytes.Length);

            plaintext = Encoding.UTF8.GetString(plaintextBytes);
            return !string.IsNullOrWhiteSpace(plaintext);
        }
        catch (Exception exception)
            when (exception is FormatException
                or CryptographicException
                or ArgumentException)
        {
            plaintext = string.Empty;
            return false;
        }
    }
}
