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
            p = dbContext.Product.Find("OR");
            Assert.IsNotNull(p);
            Assert.AreEqual("Ore", p.ProductCode);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Product.Where(s => s.ProductCode.StartsWith("A")).OrderBy(s => s.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("Alabama", products[0].ProductCode);
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
            p = dbContext.Product.Find("HI");
            dbContext.Product.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Product.Find("HI"));
        }

        [Test]
        public void CreateTest()
        {

        }

        [Test]
        public void UpdateTest()
        {

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