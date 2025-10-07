using L2O___D09;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;
using static L2O___D09.ListGenerators;
using static System.Runtime.InteropServices.JavaScript.JSType;


//Use ListGenerators.cs & Customers.xml


//1.Find all products that are out of stock.

//var res = ProductList.Where(p => p.UnitsInStock == 0);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//2.Find all products that are in stock and cost more than 3.00 per unit.

//var res = ProductList.Where(p => p.UnitsInStock != 0 && p.UnitPrice >3);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//3.Returns digits whose name is shorter than their value.

//string[] Arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
//var res = Arr.Select((name, i) => new { Name = name, Value = i })
//    .Where(x => x.Name.Length < x.Value);


//Use ListGenerators.cs & Customers.xml


//1.Get first Product out of Stock 


//var res = ProductList.First(p=> p.UnitsInStock == 0);
//Console.WriteLine(res);


//2. Return the first product whose Price > 1000, unless there is no match, in which case null is returned.

//var res = ProductList.FirstOrDefault(p => p.UnitPrice > 1000);
//Console.WriteLine(res);

//3. Retrieve the second number greater than 5
//
//int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
//;

//var res = Arr.Where(num => num > 5)
//    .ElementAt(1);
//Console.WriteLine(res);


//Use ListGenerators.cs & Customers.xml

//1.Find the unique Category names from Product List
//var res = ProductList.Select(p => p.Category).Distinct();
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//2. Produce a Sequence containing the unique first letter from both product and customer names.
//var res = ProductList.Select(p => p.ProductName[0]).Distinct().Union(CustomerList.Select(p => p.CompanyName[0]).Distinct());
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//3. Create one sequence that contains the common first letter from both product and customer names.
//var res = ProductList.Select(p => p.ProductName[0]).Distinct().Intersect(CustomerList.Select(p => p.CompanyName[0]).Distinct());
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//4. Create one sequence that contains the first letters of product names that are not also first letters of customer names.
//var res = ProductList.Select(p => p.ProductName[0]).Distinct().Except(CustomerList.Select(p => p.CompanyName[0]).Distinct());
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//5. Create one sequence that contains the last Three Characters in each names of all customers and products, including any duplicates
//var res = ProductList.Select(p => p.ProductName.Substring(p.ProductName.Length - 3)).Concat(CustomerList.Select(c=>c.CompanyName.Substring(c.CompanyName.Length-3))); 
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}

//LINQ - Aggregate Operators
//1.Uses Count to get the number of odd numbers in the array
//int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
//var res = Arr.Count(p => p % 2 == 1);
//Console.WriteLine(res);

//Use ListGenerators.cs & Customers.xml
//2.Return a list of customers and how many orders each has.
//var res = CustomerList.Select(c => new { c.CompanyName,Orders = c.Orders.Count()} );
//foreach(var item in res)
//{
//    Console.WriteLine( item);
//}

//3. Return a list of categories and how many products each has
//var res = ProductList.GroupBy(p=>p.Category).Select(g => new {Category=  g.Key,Products= g.Count() });
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//4. Get the total of the numbers in an array.
//int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
//;
//var res = Arr.Sum();
//Console.WriteLine(res);

//5.Get the total number of characters of all words in dictionary_english.txt (Read dictionary_english.txt into Array of String First).
//string[] Arr = File.ReadAllLines("dictionary_english.txt");
//var res  =Arr.Sum(w=> w.Length);
//Console.WriteLine(res);

//Use ListGenerators.cs & Customers.xml
//6.Get the total units in stock for each product category.
//var res = ProductList.GroupBy(c => c.Category).Select(i => new { Category = i.Key, Units = i.Sum(u=>u.UnitsInStock) });
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//7. Get the length of the shortest word in dictionary_english.txt (Read dictionary_english.txt into Array of String First).
//string[] Arr = File.ReadAllLines("dictionary_english.txt");
//var res = Arr.Min(w=> w.Length);
//Console.WriteLine(res);

//Use ListGenerators.cs & Customers.xml
//8. Get the cheapest price among each category's products
//var res = ProductList.Min(p => p.UnitPrice);
//Console.WriteLine(res);

//9. Get the products with the cheapest price in each category (Use Let)
//var res = from p in ProductList
//          group p by p.Category into c
//          let minPrice = c.Min(p => p.UnitPrice)
//          from prod in c
//          where prod.UnitPrice == minPrice
//          select
//                                                     new
//                                                      {
//                                                          Category = c.Key,
//                                                          ProductName = prod.ProductName,
//                                                          Price = prod.UnitPrice
//                                                      };
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//10. Get the length of the longest word in dictionary_english.txt (Read dictionary_english.txt into Array of String First).
//string[] Arr = File.ReadAllLines("dictionary_english.txt");
//var res = Arr.Max(w => w.Length);
//Console.WriteLine(res);

//Use ListGenerators.cs & Customers.xml
//11. Get the most expensive price among each category's products.
//var res = ProductList.Max(p => p.UnitPrice);
//Console.WriteLine(res);

//12. Get the products with the most expensive price in each category.
//var res = from p in ProductList
//          group p by p.Category into c
//          let maxPrice = c.Max(p => p.UnitPrice)
//          from prod in c
//          where prod.UnitPrice == maxPrice
//          select
//                                                     new
//                                                     {
//                                                         Category = c.Key,
//                                                         ProductName = prod.ProductName,
//                                                         Price = prod.UnitPrice
//                                                     };
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//13. Get the average length of the words in dictionary_english.txt (Read dictionary_english.txt into Array of String First).
//string[] Arr = File.ReadAllLines("dictionary_english.txt");
//var res = Arr.Average(w => w.Length);
//Console.WriteLine(res);

//14. Get the average price of each category's products.

//var res = ProductList
//    .GroupBy(p => p.Category)
//    .Select(g => new
//    {
//        Category = g.Key,
//        AveragePrice = g.Average(p => p.UnitPrice)
//    });

//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//LINQ - Ordering Operators

//Use ListGenerators.cs & Customers.xml
//1. Sort a list of products by name
//var res = ProductList.OrderBy(p=>p.ProductName).Select(p => p.ProductName);

//foreach(var item in res)
//{

//    Console.WriteLine( item );
//}

//2. Uses a custom comparer to do a case-insensitive sort of the words in an array.
//string[] Arr = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" }
//;
//Array.Sort(Arr, (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase));
//foreach(var item in Arr)
//{
//    Console.WriteLine( item);
//}

//Use ListGenerators.cs & Customers.xml
//3.Sort a list of products by units in stock from highest to lowest.
//var res = ProductList.OrderByDescending(p => p.UnitsInStock).Select(p => new { p.ProductName, p.UnitsInStock });
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}

////4. Sort a list of digits, first by length of their name, and then alphabetically by the name itself.
//string[] Arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
//var res = Arr.OrderBy(w => w.Length).ThenBy(w => w);
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}

//5.Sort first by word length and then by a case-insensitive sort of the words in an array.
//string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" }
//;
//var res = words.OrderBy(w => w.Length).ThenBy(w=>w,StringComparer.OrdinalIgnoreCase);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//Use ListGenerators.cs & Customers.xml
//6.Sort a list of products, first by category, and then by unit price, from highest to lowest.
//var res = ProductList.OrderBy(p => p.Category).ThenByDescending(p => p.UnitPrice);
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}

//7. Sort first by word length and then by a case-insensitive descending sort of the words in an array.
//string[] Arr = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" }
//;
//var res = Arr.OrderBy(w => w.Length).ThenByDescending(w => w, StringComparer.OrdinalIgnoreCase);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}


//8.Create a list of all digits in the array whose second letter is 'i' that is reversed from the order in the original array.
//string[] Arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
//var res = Arr.Where(w => w[1] == 'i').Reverse();
//foreach (var item in res)
//{
//    Console.WriteLine( item);
//}

//LINQ - Partitioning Operators

//Use ListGenerators.cs & Customers.xml
//1. Get the first 3 orders from customers in Washington
//var res = CustomerList.Where(c => c.City == "Washington").SelectMany(c => c.Orders).Take(3);
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}

//2. Get all but the first 2 orders from customers in Washington.
//var res = CustomerList.Where(c => c.City == "Washington").SelectMany(c => c.Orders).Skip(2);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//3. Return elements starting from the beginning of the array until a number is hit that is less than its position in the array.
//int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
//var res = numbers.TakeWhile((num, index) => num >= index);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//4.Get the elements of the array starting from the first element divisible by 3.

//int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
//;

//var res = numbers.SkipWhile((num => num % 3 != 0));
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//5.Get the elements of the array starting from the first element less than its position.
//int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
//var res = numbers.SkipWhile((num, index) => num >= index);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//LINQ - Projection Operators

//Use ListGenerators.cs & Customers.xml
//1. Return a sequence of just the names of a list of products.
//var res =ProductList.Select(p=>p.ProductName) ;
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//2. Produce a sequence of the uppercase and lowercase versions of each word in the original array (Anonymous Types).
//string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };
//var res = words.Select(w => new { upper = w.ToUpper(), lower = w.ToLower() });
//foreach(var item in res)
//{
//    Console.WriteLine(item);
//}
//Use ListGenerators.cs & Customers.xml
//3.Produce a sequence containing some properties of Products, including UnitPrice which is renamed to Price in the resulting type.
//var res = ProductList.Select(p => new{ p.ProductID,p.ProductName,Price = p.UnitPrice});
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//4. Determine if the value of ints in an array match their position in the array.
//int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }
//;
//var res = Arr.Select((value, index) =>value == index);
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//5. Returns all pairs of numbers from both arrays such that the number from numbersA is less than the number from numbersB.
//int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 }
//;
//int[] numbersB = { 1, 3, 5, 7, 8 };
//Result
//Pairs where a < b:
//0 is less than 1
//0 is less than 3
//0 is less than 5
//0 is less than 7
//0 is less than 8
//2 is less than 3
//2 is less than 5
//2 is less than 7
//2 is less than 8
//4 is less than 5
//4 is less than 7
//4 is less than 8
//5 is less than 7
//5 is less than 8
//6 is less than 7
//6 is less than 8

//var res = from a in numbersA
//          from b in numbersB
//          where a < b
//          select new { A = a, B = b };
//foreach (var item in res)
//{
//    Console.WriteLine($"{item.A}<{item.B}");
//}


//Use ListGenerators.cs & Customers.xml
//6.Select all orders where the order total is less than 500.00.
//var res = CustomerList.SelectMany(o => o.Orders).Where(o=>o.Total<500);

//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}


//7. Select all orders where the order was made in 1998 or later.
//var res = CustomerList.SelectMany(o => o.Orders).Where(o => o.OrderDate.Year >= 1998);

//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}


//LINQ - Quantifiers

//1. Determine if any of the words in dictionary_english.txt (Read dictionary_english.txt into Array of String First) contain the substring 'ei'.
//string[] Arr = File.ReadAllLines("dictionary_english.txt");
//var res = Arr.Any(w => w.Contains("ei"));
//Console.WriteLine(res);


//Use ListGenerators.cs & Customers.xml
//2. Return a grouped a list of products only for categories that have at least one product that is out of stock.
//var res = from p in ProductList
//          group p by p.Category into g
//          where g.Any(p => p.UnitsInStock == 0)
//          select g.Key;
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}

//3. Return a grouped a list of products only for categories that have all of their products in stock.
//var res = from p in ProductList
//          group p by p.Category into g
//          where g.All(p => p.UnitsInStock != 0)
//          select g.Key;
//foreach (var item in res)
//{
//    Console.WriteLine(item);
//}
//LINQ - Grouping Operators

//1. Use group by to partition a list of numbers by their remainder when divided by 5
//Output: 
//int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

//var res =
//    from n in numbers
//    group n by n % 5 into g
//    select g;

//foreach (var group in res)
//{
//    Console.WriteLine($"Numbers with a remainder of {group.Key} when divided by 5:");
//    foreach (var num in group)
//    {
//        Console.WriteLine(num);
//    }
//}
//Numbers with a remainder of 0 when divided by 5:
//0
//5
//10
//Numbers with a remainder of 1 when divided by 5:
//1
//6
//11
//Numbers with a remainder of 2 when divided by 5:
//7
//2
//12
//Numbers with a remainder of 3 when divided by 5:
//3
//8
//13
//Numbers with a remainder of 4 when divided by 5:
//4
//9
//14

//2.Uses group by to partition a list of words by their first letter.
//Use dictionary_english.txt for Input
//string[] words = File.ReadAllLines("dictionary_english.txt");
//var res = from w in words
//          group w by w[0] into g  
//          orderby g.Key           
//          select g;




//3. Consider this Array as an Input 


//Use Group By with a custom comparer that matches words that are consists of the same Characters Together
//Result
//...
//from 
//form 
//...
//salt
//last 
//...
//earn 
//near

//string[] Arr = { "from   ", " salt", " earn ", "  last   ", " near ", " form  " };
//var res = from word in Arr
//          let cleaned = new string(word.Trim().OrderBy(c => c).ToArray())
//          group word by cleaned into g
//          select g;
