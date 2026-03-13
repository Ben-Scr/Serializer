# BenScr.Serialization
A Performant C# `Net 9.0` Serialization library that combines Json, Xml and Binary

## Features
- Serialization and Deserialization of Json, Xml and Binary
- Conversion of one format to another fx. Json -> Binary

## How to use
### Include
```csharp
using BenScr.Serializer;
```
---
### Set a path and create an object
```csharp
string path = "Test.dat";
PersonData person = new("Name", 20);
```
---
### Json
```csharp
Json.Save(path, person);
PersonData loadedPerson = Json.Load<PersonData>(path);
string json = Json.Serialize(person);
```
---
### Binary
```csharp
Binary.Save(path, person);
PersonData loadedPerson = Binary.Load<PersonData>(path);
byte[] binary = Binary.Serialize(person);
```
---
### Xml
```csharp
Xml.Save(path, data);
PersonData loadedPerson = XML.Load<PersonData>(path);
byte[] xml = Xml.Serialize(person);
```
---
### Converting files from one format to any other
```csharp
// ConvertTo() creates a new file containing the same data,
// but converted into a different format.
// By default, the output path is "path.[extension]"—in this case, "path.xml".
// You can override the output path by specifying it as the fourth parameter.
FileConverter.ConvertTo(path: "path.json", original: Format.Json, to: Format.Xml);
```

## External Libaries
- MessagePack: https://github.com/MessagePack-CSharp/MessagePack-CSharp
