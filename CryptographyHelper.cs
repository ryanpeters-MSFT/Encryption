using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class CryptographyHelper
{
    // must be 16 bytes (128-bit)
    private static byte[] initVector = { 23, 45, 3, 56, 12, 128, 5, 34, 56, 124, 65, 23, 7, 34, 12, 49 };

    public static async Task<string> EncryptToStringAsync(object payload, string passphrase)
    {
        // serialize the payload to a JSON string
        var serialized = JsonSerializer.Serialize(payload, payload.GetType());

        // encrypt the byte array
        var encrypted = await EncryptAsync(serialized, passphrase);

        // convert to base64 for use in cookie
        return System.Convert.ToBase64String(encrypted);
    }

    public static async Task<T> DecryptToStringAsync<T>(string payload, string passphrase)
    {
        // decode the raw base64 encoded string
        var decoded = System.Convert.FromBase64String(payload);

        // decrypt to a string
        var decrypted = await DecryptAsync(decoded, passphrase);

        // deserialize the string
        return JsonSerializer.Deserialize<T>(decrypted);
    }

    public static async Task<byte[]> EncryptAsync(string plaintext, string passphrase)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = GetKey(passphrase);
            aes.IV = initVector;

            using (var output = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(plaintext));
                    await cryptoStream.FlushFinalBlockAsync();

                    return output.ToArray();
                }
            }
        }
    }

    public static async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = GetKey(passphrase);
            aes.IV = initVector;

            using (var input = new MemoryStream(encrypted))
            {
                using (var cryptoStream = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var output = new MemoryStream())
                    {
                        await cryptoStream.CopyToAsync(output);

                        return Encoding.Unicode.GetString(output.ToArray());
                    }
                }
            }
        }
    }

    private static byte[] GetKey(string password)
    {
        var emptySalt = Array.Empty<byte>();
        var hashMethod = HashAlgorithmName.SHA512;
        var passwordBytes = Encoding.Unicode.GetBytes(password);

        return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password), emptySalt, 2000, hashMethod, 16);
    }
}