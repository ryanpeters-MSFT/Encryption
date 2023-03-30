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

        var objectEncoded = await CryptographyHelper.EncryptToStringAsync(dog, phrase);

        Console.WriteLine($"Base-64 encoded/encrypted dog: {objectEncoded}");

        var decryptedDog = await CryptographyHelper.DecryptToStringAsync<Dog>(objectEncoded, phrase);

        Console.WriteLine($"Decrypted dog name: {decryptedDog.Name}");
        Console.WriteLine($"Decrypted dog breed: {decryptedDog.Breed}");
        Console.WriteLine($"Decrypted dog age: {decryptedDog.Age}");
    }
}