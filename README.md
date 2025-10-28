# Serializer
A Performant C# `Net 9.0` Serialization library that combines Json, Xml and Binary

## Features
- Serialization and Deserialization of Json, Xml and Binary

## How to use
```csharp
using BenScr.Serialization.Json;
using BenScr.Serialization.Xml;
using BenScr.Serialization.Binary;
```

- Json
```csharp
string path = "Test.json";
PersonData data = new PersonData("Name", 20);
Json.Save(path, data);
PersonData loadedPerson = Json.Load<PersonData>(path);
```
- Binary
```csharp
string path = "Test.bin";
PersonData data = new PersonData("Name", 20);
Binary.Save(path, data);
PersonData loadedPerson = Binary.Load<PersonData>(path);
```
- Xml
```csharp
string path = "Test.xml";
PersonData data = new PersonData("Name", 20);
XML.Save(path, data);
PersonData loadedPerson = XML.Load<PersonData>(path);
```
## Support
[![Ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/benscr)</p>
