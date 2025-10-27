# SerializerCS
A C# Serializing library

## Usage
- Serialzing/Deserializing data into JSON, XML and Binary format
- File Management

## Example Syntax
- Json
```csharp
string path = "Test.json";
PersonData data = new PersonData("Name", 20);
Serializer.Json.Serialize(path, data);
PersonData loadedPerson = Serializer.Json.Deserialize<PersonData>(path);

// Structure
public static void Serialize<T>(string path, T obj, JsonSerializerOptions? options = null)
public static T Deserialize<T>(string path, T defaultValue = default(T), JsonSerializerOptions? options = null)
```
- Binary
```csharp
string path = "Test.bin";
PersonData data = new PersonData("Name", 20);
Serializer.Binary.Serialize(path, data);
PersonData loadedPerson = Serializer.Binary.Deserialize<PersonData>(path);

//Structure
public static void SerializeUnmanagedBlock<T>(string path, ReadOnlySpan<T> span, CompressionLevel level = CompressionLevel.Fastest) where T : unmanaged
public static ReadOnlySpan<T> DeserializeUnmanagedBlock<T>(string path) where T : unmanaged

public static void Serialize<T>(string path, T item, CompressionLevel compressionLevel = CompressionLevel.Fastest, JsonSerializerOptions? options = null)
public static T Deserialize<T>(string path, JsonSerializerOptions? options = null)
```
- XML
```csharp
string path = "Test.xml";
PersonData data = new PersonData("Name", 20);
Serializer.XML.Serialize(path, data);
PersonData loadedPerson = Serializer.XML.Deserialize<PersonData>(path);

// Structure
public static void Serialize<T>(string path, T obj)
public static T Deserialize<T>(string path)
```
