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

    }
}