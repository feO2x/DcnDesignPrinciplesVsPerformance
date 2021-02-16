using System.Collections.Generic;

namespace SimpleExample
{
    public static class Program
    {
        public static void Main()
        {
            MakeGoodFriends();
        }

        private static void MakeGoodFriends()
        {
            var myFriends = new List<Person>(2);
            myFriends.CreateAndAddPerson("Walter", 54);
            myFriends.CreateAndAddPerson("Jesse", 27);
        }

        private static void CreateAndAddPerson(this List<Person> list, string name, int age)
        {
            var newPerson = new Person(name, age);
            list.Add(newPerson);
        }
    }

    public class Person
    {
        private readonly string _name;
        private readonly int _age;

        public Person(string name, int age)
        {
            _name = name;
            _age = age;
        }

        public string Name => _name;
        public int Age => _age;
    }
}
