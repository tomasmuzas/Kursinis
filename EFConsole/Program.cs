using System;
using System.Linq;

namespace EFConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseContext();
            var profile = db.Profiles.First();
            Console.WriteLine($"Name: {profile.Name} Surname: {profile.Surname} Email: {profile.Email}");
        }
    }
}
