using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
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
            var convertToppings = ConvertToppingsLists(readFile);
            var orderList = OrderStrings(convertToppings);
            var topStrings = Top20Pizzas(orderList);
           

            return readFile;
        }

        private bool Top20Pizzas(List<string> orderList)
        {
            var top20Strings = orderList.GroupBy(x => x).ToArray();

            Console.WriteLine(top20Strings.Count());
            
            return true;
        }

        private List<string> OrderStrings(List<string> convertToppings)
        {
            var twistedLetters = new List<string>();
            foreach (var topping in convertToppings)
            {
                var alphabetizedString = topping.OrderBy(x => x).ToArray();
                var orderedString = String.Join("", alphabetizedString);
            }

            return twistedLetters;
        }

        private List<string> ConvertToppingsLists(List<Pizza> readFile)
        {
            var stringToppings = new List<string>();
            foreach (var file in readFile)
            {
                var stringList = String.Join("", file.Toppings);
                var removeSpace = Regex.Replace(stringList, @"\s+", "");
                stringToppings.Add(removeSpace);
            }

            return stringToppings;
        }
    }

    public class Pizza
    {
        public List<string> Toppings { get; set; }
    }
}