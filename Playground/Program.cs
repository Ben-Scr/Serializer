using BenScr.Serialization.Json;
public static class Program
{
    public static void Main(string[] args)
    {
        string path = "Test.json";
        Json.Save(path, 5);
        int loadedInt = Json.Load<int>(path);
        Console.WriteLine(loadedInt);
    }
}