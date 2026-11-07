using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Product.OrderBy(s => s.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Product.Find("JAVP");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's Java Programming", p.Description);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Product.Where(s => s.UnitPrice == 56.50m).OrderBy(s => s.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Product.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = dbContext.Product.Find("SQ12");
            dbContext.Product.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Product.Find("SQ12"));
        }

        [Test]
        public void CreateTest()
        {
            p = new Product();
            p.ProductCode = "A320";
            p.Description = "Frosted Monster Cookie";
            p.OnHandQuantity = 400;
            p.UnitPrice = 3.50m;
            dbContext.Product.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Product.Find("A320"));
        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Product.Find("ADC4");
            p.Description = "Murach's ADO.NET 4 with C# 2026";
            dbContext.Product.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Product.Find("ADC4");
            Assert.AreEqual("Murach's ADO.NET 4 with C# 2026", p.Description);
        }

        public void PrintAll(List<Product> products)
        {
            foreach (Product p in products)
            {
                Console.WriteLine(p);
            }
        }
    }
}