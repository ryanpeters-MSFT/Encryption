using System.Text.Json;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var dog = new Dog
        {
            Name = "Sadie",
            Breed = "Old mutt",
            Age = 12,

            FavoriteFood = new Food
            {
                Name = "Purina",
                Cost = 49.99m
            }
        };

        const string phrase = "my-super-cryptic-passphrase";

        var encryptedDog = await EncryptToBase64StringAsync(dog, phrase);

        Console.WriteLine($"Base-64 encoded/encrypted dog: {encryptedDog}");

        var decryptedDog = await DecryptFromBase64StringAsync<Dog>(encryptedDog, phrase);

        Console.WriteLine($"Dog.Name: {decryptedDog.Name}");
        Console.WriteLine($"Dog.Breed: {decryptedDog.Breed}");
        Console.WriteLine($"Dog.Age: {decryptedDog.Age}");
        Console.WriteLine($"Dog.FavoriteFood.Name: {decryptedDog.FavoriteFood.Name}");
        Console.WriteLine($"Dog.FavoriteFood.Cost: {decryptedDog.FavoriteFood.Cost}");
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