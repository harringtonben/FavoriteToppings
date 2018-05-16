using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var readFile = new FileReader().ReadFile();

            var findTop20 = new TallyList().FindTop20(readFile);
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
    
    public class TallyList
    {
        public List<Pizza> FindTop20(List<Pizza> readFile)
        {
            var singleIngredientPizza = findSingleIngredientPies(readFile);
            var twoIngredientPizza = findTwoIngredientPies(readFile);

            return readFile;
        }

        public List<Pizza> findSingleIngredientPies(List<Pizza> pizzas)
        {
            var singleIngPies = new List<Pizza>();
            foreach (var pizza in pizzas)
            {
                if(pizza.Toppings.Count == 1)
                    singleIngPies.Add(pizza);
            }

            return singleIngPies;
        }
        
        public List<Pizza> findTwoIngredientPies(List<Pizza> pizzas)
        {
            var doubleIngPies = new List<Pizza>();
            foreach (var pizza in pizzas)
            {
                if(pizza.Toppings.Count == 2)
                    doubleIngPies.Add(pizza);
            }

            return doubleIngPies;
        }
    }

    public class Pizza
    {
        public List<string> Toppings { get; set; }
    }
}