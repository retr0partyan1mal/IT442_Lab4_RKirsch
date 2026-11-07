using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class StateTests
    {
        // ignore this warning about making dbContext nullable.
        // if you add the ?, you'll get a warning wherever you use dbContext
        MMABooksContext dbContext;
        State? s;
        List<State>? states;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetStateData()");
        }

        [Test]
        public void GetAllTest()
        {
            states = dbContext.State.OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(53, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            s = dbContext.State.Find("OR");
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Console.WriteLine(s);
        }

        [Test]
        public void GetUsingWhere()
        {
            states = dbContext.State.Where(s => s.StateName.StartsWith("A")).OrderBy(s => s.StateName).ToList();
            Assert.AreEqual(4, states.Count);
            Assert.AreEqual("Alabama", states[0].StateName);
            PrintAll(states);
        }

        [Test]
        public void GetWithCustomersTest()
        {
            s = dbContext.State.Include("Customer").Where(s => s.StateCode == "OR").SingleOrDefault();
            Assert.IsNotNull(s);
            Assert.AreEqual("Ore", s.StateName);
            Assert.AreEqual(5, s.Customer.Count);
            Console.WriteLine(s);
        }

        [Test]
        public void DeleteTest()
        {
            s = dbContext.State.Find("HI");
            dbContext.State.Remove(s);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.State.Find("HI"));
        }

        [Test]
        public void CreateTest()
        {
            // The following code was needed or else the test would not pass.
            s = dbContext.State.Find("HI");
            if (s != null)
            {
                dbContext.State.Remove(s);
                dbContext.SaveChanges();
            }

            s = new State();
            s.StateCode = "HI";
            s.StateName = "Hawaii";
            dbContext.State.Add(s);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.State.Find("HI"));
        }

        [Test]
        public void UpdateTest()
        {
            s = dbContext.State.Find("OR");
            s.StateName = "Oregon";
            dbContext.State.Update(s);
            dbContext.SaveChanges();
            s = dbContext.State.Find("OR");
            Assert.AreEqual("Oregon", s.StateName);
        }

        public void PrintAll(List<State> states)
        {
            foreach (State s in states)
            {
                Console.WriteLine(s);
            }
        }
    }
}