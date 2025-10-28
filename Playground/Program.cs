using BenScr.Serialization.Binary;
using BenScr.Serialization.Json;
using BenScr.Serialization.Xml;
public static class Program
{
    public static void Main(string[] args)
    {
        const string path = "Test.txt";

        Json.Save(path, 5);
        int loadedInt = Json.Load<int>(path);
        Console.WriteLine(loadedInt);

        Xml.Save(path, 5);
        loadedInt = Xml.Load<int>(path);
        Console.WriteLine(loadedInt);

        Binary.Save(path, 5);
        loadedInt = Binary.Load<int>(path);
        Console.WriteLine(loadedInt);
    }
}