using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using MoreLinq;
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
        public bool FindTop20(List<Pizza> readFile)
        {
            var convertToppings = ConvertToppingsLists(readFile);
            var orderList = OrderStrings(convertToppings);
            var pizzas = Top20Pizzas(orderList);
            var top20Pizzas = FilterPizzas(pizzas);
            
            return true;
        }

        private List<Pizza> FilterPizzas(List<Pizza> pizzas)
        {
            var distinctPizzas = pizzas.DistinctBy(x=> x.NumberOfOrders).ToList();
            var orderedPizzas = distinctPizzas.MaxBy(x => x.NumberOfOrders).ToList();
            
            Console.WriteLine(pizzas.Count);
            Console.WriteLine(distinctPizzas.Count);

            return distinctPizzas;
        }

        public List<Pizza> Top20Pizzas(List<Pizza> orderList)
        {
            var allTheZas = new List<Pizza>();
            
            var top20Strings = orderList.GroupBy(x => x.ToppingsString)
                .Select(g => new {Value = g.Key, Count = g.Count()})
                .OrderByDescending(x => x.Count);

            var top20Pizzas = from order in orderList
                join item in top20Strings on order.ToppingsString equals item.Value
                where (order.ToppingsString == item.Value)
                select new {Toppings = order.Toppings, NumberOfOrders = item.Count, PizzaName = order.PrettToppingString};

            foreach (var item in top20Pizzas)
            {
                var za = new Pizza
                {
                    Toppings = item.Toppings,
                    PrettToppingString = item.PizzaName,
                    NumberOfOrders = item.NumberOfOrders
                };
                allTheZas.Add(za);
            }

            return allTheZas;
        }

        private List<Pizza> OrderStrings(List<Pizza> convertToppings)
        {

            foreach (var topping in convertToppings)
            {
                var alphabetizedString = topping.ToppingsString.OrderBy(x => x).ToArray();
                var orderedString = String.Join("", alphabetizedString);
                topping.ToppingsString = orderedString;
            }

            return convertToppings;
        }

        private List<Pizza> ConvertToppingsLists(List<Pizza> readFile)
        {
            
            foreach (var file in readFile)
            {
                var stringList = String.Join("", file.Toppings);
                file.PrettToppingString = stringList;
                var removeSpace = Regex.Replace(stringList, @"\s+", "");
                file.ToppingsString = removeSpace;
            }

            return readFile;
        }
    }

    public class Pizza
    {
        public List<string> Toppings { get; set; }
        public string PrettToppingString { get; set; }
        public string ToppingsString { get; set; }
        public int NumberOfOrders { get; set; }
    }
}