using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Person> persons = new List<Person>() { 
                new Person{Name = "1",Age = 1},
                new Person{Name="2",Age=2}
            };

            persons.Where(c => c.Age == 1 && c.Name == "1");
        }
    }

    public class FuncHelper
    {
        public Func<Person, bool> GetPredicate(string name)
        {
            return (t1) => { return t1.GetType().GetProperty("Name").GetValue(t1).ToString() == name; };
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class QueryPara
    {
        public string FieldName { get; set; }
        public object Value { get; set; }
    }
}
