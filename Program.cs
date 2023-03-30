using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var dog = new Dog
        {
            Name = "Sadie",
            Breed = "Old mutt",
            Age = 12
        };

        const string phrase = "my-super-cryptic-passphrase";

        var objectEncoded = await EncryptToBase64StringAsync(dog, phrase);

        Console.WriteLine($"Base-64 encoded/encrypted dog: {objectEncoded}");

        var decryptedDog = await DecryptFromBase64StringAsync<Dog>(objectEncoded, phrase);

        Console.WriteLine($"Decrypted dog name: {decryptedDog.Name}");
        Console.WriteLine($"Decrypted dog breed: {decryptedDog.Breed}");
        Console.WriteLine($"Decrypted dog age: {decryptedDog.Age}");
    }

    public static async Task<string> EncryptToBase64StringAsync(object payload, string passphrase)
    {
        // serialize the payload to a JSON string
        var serialized = JsonSerializer.Serialize(payload, payload.GetType());

        // encrypt the byte array
        var encrypted = await CryptographyHelper.EncryptAsync(serialized, passphrase);

        // convert to base64 for use in cookie
        return System.Convert.ToBase64String(encrypted);
    }

    public static async Task<T> DecryptFromBase64StringAsync<T>(string payload, string passphrase)
    {
        // decode the raw base64 encoded string
        var decoded = System.Convert.FromBase64String(payload);

        // decrypt to a string
        var decrypted = await CryptographyHelper.DecryptAsync(decoded, passphrase);

        // deserialize the string
        return JsonSerializer.Deserialize<T>(decrypted);
    }
}