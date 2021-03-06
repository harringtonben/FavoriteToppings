﻿using System;
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

            foreach (var item in findTop20)
            {
                Console.WriteLine($"a pizza with the following toppings ({item.PrettToppingString}) was ordered {item.NumberOfOrders} times.");
            }
        }
    }

    //This class reads the file and parses it into a list of the class of Pizza and returns it to the Main method.
    public class FileReader
    {
        public List<Pizza> ReadFile()
        {
            var fileToRead = JsonConvert.DeserializeObject<List<Pizza>>(File.ReadAllText(@"./pizzas.json"));

            return fileToRead;
        }
    }
    
    //This class holds all of the logic to return the top 20 pizzas in the List
    public class TallyList
    {
        //This method invokes all of the logic and filters the top 20 pizza orders
        public List<Pizza> FindTop20(List<Pizza> readFile)
        {
            var top20Pizzas = new List<Pizza>();
            var convertToppings = ConvertToppingsLists(readFile);
            var orderList = OrderStrings(convertToppings);
            var pizzas = Top20Pizzas(orderList);
            var orderPizzas = FilterPizzas(pizzas);

            foreach (var pizza in orderPizzas)
            {
                if (top20Pizzas.Count < 20)
                    top20Pizzas.Add(pizza);
            }
            
            return top20Pizzas;
        }

        //This method filters the pizzas by number of Orders from largest to smallest and removes all duplicates
        private List<Pizza> FilterPizzas(List<Pizza> pizzas)
        {
            var distinctPizzas = pizzas.DistinctBy(x=> x.NumberOfOrders).ToList();
            var orderedPizzas = distinctPizzas.OrderByDescending(x => x.NumberOfOrders).ToList();
            
            return orderedPizzas;
        }

        //This method first counts the number of similar topping orders and tallies them and then returns that number to a propery in the list
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

        //This method alphabetizes each combined string in order for the strings to be compared and counted
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

        //This method joins the list of toppings in each Pizza in the List and saves them to a new property
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