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
                Console.WriteLine("Current users in system: ");
                List<User> users = DbHelper.GetAllUsers(context);

                foreach (var user in users)
                {
                    Console.Write($"{user.FirstName} {user.LastName}");
                }

                Console.WriteLine($"Total number of users = {users.Count()}");
                Console.WriteLine("c to create new user");
                Console.WriteLine("x to exit");

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

            while (true)
            {
                Console.WriteLine("Create user");
                string firstName = GetNonEmptyInput("Enter user's first name: ");
                if (firstName == null)
                {
                    break;
                }
                string lastName = GetNonEmptyInput("Enter user's last name: ");
                if (lastName == null)
                {
                    break;
                }
                string ssn = GetNonEmptyInput("Enter user's social sequrity number: ");
                if (ssn == null)
                {
                    break;
                }
                Console.WriteLine("Enter user's Email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Enter users's phone number: ");
                string phone = Console.ReadLine();

                //StringBuilder sb = new StringBuilder(); ??
                Random random = new Random();
                string pin = random.Next(100000, 1000000).ToString();   //Changed password to a 6 digit number 

                User newUser = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Pin = pin,
                    Email = email,
                    Phone = phone,
                    SSN = ssn

                };
                bool success = DbHelper.AddUser(context, newUser);
                if (success)
                {
                    Console.WriteLine($"Createusername {firstName} {lastName} with pin {pin} successfully!");
                }
                else
                {
                    Console.WriteLine($"Failed to create user with username {firstName} {lastName}");

                }
            }

        }
        public static string GetNonEmptyInput(string prompt)    //Made a method that forces a user to enter a string, prompt is what you want the method to write, ex enter name etc
                                                                //unless user enters escape key and exit loop. 
        {
            string userInput = "";
            Console.Write(prompt);  //Writes the prompt entered in method 
            userInput = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userInput))  //while loop that prevents user from entering empty loop, 
            {

                Console.WriteLine("this field require an input");
                Console.WriteLine("Or press Escape (Esc) key to exit!");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Reads the key press and stores it to keyInfo, set to true so we dont want to show the keypress in console
                if (keyInfo.Key == ConsoleKey.Escape)   //if esc pressed return null 
                {
                    Console.WriteLine("You pressed Escape key");
                    return null;        //return null so we can use it in whileloop outside
                }
            }
            return userInput;   //this will be stored to the string when we use the methodd..
        }

    }
}
