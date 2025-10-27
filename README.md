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
```
- Binary
```csharp
string path = "Test.bin";
PersonData data = new PersonData("Name", 20);
Serializer.Binary.Serialize(path, data);
PersonData loadedPerson = Serializer.Binary.Deserialize<PersonData>(path);
```
- XML
```csharp
string path = "Test.xml";
PersonData data = new PersonData("Name", 20);
Serializer.XML.Serialize(path, data);
PersonData loadedPerson = Serializer.XML.Deserialize<PersonData>(path);
