using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Utilities
{
    internal static class AdminActions
    {
        public static void DoAdminTasks()
        {
            using (BankContext context = new BankContext())
            {
                Console.Clear();
                Console.WriteLine("Current users in system: ");
                Console.WriteLine("-------------------------------");
                List<User> users = DbHelper.GetAllUsers(context);

                foreach (var user in users)
                {
                    Console.Write($"{user.Id}:{user.FirstName} {user.LastName}");
                    Console.WriteLine("");
                }

                Console.WriteLine($"Total number of users = {users.Count()}");
                Console.WriteLine("");
                Console.WriteLine("[C] to create new user");
                Console.WriteLine("[X] to exit");

                while (true)
                {
                    Console.WriteLine("Enter command: ");
                    string command = Console.ReadLine();

                    switch (command)
                    {
                        case "c":
                            CreateUser(context);
                            break;
                        case "x":
                            MenuAction.firstMenu();
                            return;
                            break;
                        default:
                            Console.WriteLine($"Unkown command: {command} ");
                            break;
                    }
                }
            }
        }

        private static void CreateUser(BankContext context)
        {
            Console.WriteLine("Create user");
            Console.WriteLine("Enter user's first name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter user's last name: ");
            string lastName = Console.ReadLine();
            
            //StringBuilder sb = new StringBuilder(); ??
            Random random = new Random();
            string pin = random.Next(1000, 10000).ToString();

            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Pin = pin,                              
            };

            
            bool success = DbHelper.AddUser(context, newUser);
            if (success)
            {
                Console.WriteLine($"Create username: {firstName} {lastName} with pin: {pin} successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to create user with username {firstName} {lastName}");
            }

            Account newAccount = new Account() /*< -----Skapar en default "Main" bankkonto varje gång en ny användare skapas.*/
            {
                Name = "Main",
                Balance = 0,
                UserId = newUser.Id,
            };

            context.Accounts.Add(newAccount);
            context.SaveChanges();

            context.Accounts.Add(newAccount);
            context.SaveChanges();
        }
    }
}
