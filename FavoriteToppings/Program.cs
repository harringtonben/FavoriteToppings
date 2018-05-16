using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var readFile = new FileReader().ReadFile();
            Console.WriteLine(readFile);
        }
    }

    public class FileReader
    {
        public List<List<string>> ReadFile()
        {
            using (var file = File.OpenText("pizzas.json"))
            {
                var serializer = new JsonSerializer();
                var pizzas = serializer.Deserialize(file, typeof(List<List<string>>));
                return (List<List<string>>) pizzas;
            }
        }
    }
}