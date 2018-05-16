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
            
        }
    }

    public class FileReader
    {
        public List<Pizza> ReadFile()
        {
            var fileToRead = JsonConvert.DeserializeObject<List<Pizza>>(File.ReadAllText(@"./pizzas.json"));

            return fileToRead;
        }
    }

    public class Pizza
    {
        public List<string> Toppings { get; set; }
    }
}