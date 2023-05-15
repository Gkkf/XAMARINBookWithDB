using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace KsiazkaXAMARIN
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }

        public Person()
        {

        }

        public Person(string name, string surname, string number, string email)
        {
            Name = name;
            Surname = surname;
            Number = number;
            Email = email;
        }
    }
}
