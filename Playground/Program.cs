using BenScr.Serialization.Binary;
using BenScr.Serialization.Json;
using BenScr.Serialization.Xml;
using BenScr.Serialization;
using System.Diagnostics;

public static class Program
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public static void Main(string[] args)
    {
        const string pathJson = "Preview.json", pathXml = "Preview.xml", pathBin = "Preview.bin";
        Vector2 vec2 = new Vector2 { X = 1.0f, Y = 2.0f };

        Console.WriteLine("Object to save: " + vec2);

        Json.Save(pathJson, vec2);
        Vector2 loadedVec2 = Json.Load<Vector2>(pathJson);
        Console.WriteLine("Json: " + loadedVec2);
        
        Xml.SaveCompressed(pathXml, vec2);
        loadedVec2 = Xml.LoadCompressed<Vector2>(pathXml);
        Console.WriteLine("Xml: " + loadedVec2);

        Binary.Save(pathBin, vec2);
        loadedVec2 = Binary.Load<Vector2>(pathBin);
        Console.WriteLine("Binary: " + loadedVec2);
    }
}