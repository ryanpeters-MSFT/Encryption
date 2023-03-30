# Encryption Sample
This sample demonstates basic AES encyption of a C# object and encoding the resulting value as a base-64 encoded value. This approach is a possible option for securely storing a sensitive payload for use in HTTP requests, such as storing in a cookie. 

## Overall Steps
- Define a C# object
- Serialize the object as JSON ([use of BinaryFormatter is considered obsolete](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/binaryformatter-serialization-obsolete))
- Encrypt the JSON string as a byte array
- Convert the encrypted byte array to base-64. 

The resulting base-64 string can be decrypted once it is generated, provided the same symmetric key is used.