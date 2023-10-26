using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Museum
{
    class User
    {
        public static User.Roles role;
        public enum Roles
        {
            Admin,
            Guide,
            Client
        }
        public static int Id;
        public static string Surname;
        public static string Name;


        public User(int id, string surname, string name, int? i)
        {
            Id = id;
            Surname = surname;
            Name = name;
            if (i == 1)
                role = Roles.Admin;
            else if (i == 2)
                role = Roles.Guide;
            else
                role = Roles.Client;
        }
    }
}
