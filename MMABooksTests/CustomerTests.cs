using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.MarisModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            customers = dbContext.Customer.OrderBy(s => s.Name).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            c = dbContext.Customer.Find(98);
            Assert.IsNotNull(c);
            Assert.AreEqual("Duvall, Billy", c.Name);
            Console.WriteLine(c);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the customers who live in OR
            customers = dbContext.Customer.Where(s => s.StateCode == "OR").OrderBy(s => s.Name).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Erpenbach, Lee", customers[0].Name);
            PrintAll(customers);
        }

        [Test]
        public void GetWithInvoicesTest()
        {
            // get the customer whose id is 20 and all of the invoices for that customer
            c = dbContext.Customer.Include("Invoice").Where(s => s.CustomerID == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual("Chamberland, Sarah", c.Name);
            Assert.AreEqual(3, c.Invoice.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customer.Join(
               dbContext.State,
               c => c.StateCode,
               s => s.StateCode,
               (c, s) => new { c.CustomerID, c.Name, c.StateCode, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
        public void DeleteTest()
        {
            c = dbContext.Customer.Find(531);
            dbContext.Customer.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customer.Find(531));
        }

        [Test]
        public void CreateTest()
        {
            c = new Customer();
            c.Name = "Scooby Doo";
            c.Address = "210 Spooky Street";
            c.City = "Coolsville";
            c.StateCode = "OH";
            c.ZipCode = "1969";
            dbContext.Customer.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customer.Find(c.CustomerID));
        }

        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customer.Find(391);
            c.Name = "Grohl, Dave";
            dbContext.Customer.Update(c);
            dbContext.SaveChanges();
            c = dbContext.Customer.Find(391);
            Assert.AreEqual("Grohl, Dave", c.Name);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
    }
}