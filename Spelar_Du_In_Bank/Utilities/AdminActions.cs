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
                Console.WriteLine("-------------------------------");
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
                            MenuAction.MainMenu();
                            return;
                            break;
                        default:
                            Console.WriteLine($"Unknown command: {command} ");
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
                Console.Write("Enter user's Email: ");
                string email = Console.ReadLine();
                Console.Write("Enter users's phone number: ");
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

                Account newAccount = new Account()
                {
                    Name = "Main",
                    Balance = 0,
                    UserId = newUser.Id,
                };

                context.Accounts.Add(newAccount);

                context.SaveChanges();

                if (success)                
                {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Created username [{firstName}] {lastName} with pin {pin} successfully!");
                    //added this so the message displays before going to next step. /Mojtaba
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Write your down your pin and store it somewhere safe!");
                    Console.ResetColor();
                    Console.WriteLine("");
                    Console.WriteLine("Press ENTER to go back!");
                    Console.ReadKey();
                    DoAdminTasks();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to create user with username {firstName} {lastName}");
                    Console.ResetColor();

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
