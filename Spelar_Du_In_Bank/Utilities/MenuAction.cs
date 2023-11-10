using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelar_Du_In_Bank.Utilities;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public static void MainMenu()   //changed from firstMenu to MainMenu
        {
            Console.WriteLine("Welcome to Bank.");
            Console.WriteLine("Please log in: ");

            Console.WriteLine("Enter user name: ");
            string userName = Console.ReadLine();

            Console.Write("Enter pin code: ");
            string pin = Console.ReadLine();

            if (userName == "admin")
            {
                if (pin != "1234")
                {
                    Console.WriteLine("Wrong password!");
                    return; 
                }

                AdminActions.DoAdminTasks();
                return;
            }
            else
            {
                //Code here for user login *****

                using (BankContext context = new BankContext())
                {
                    //Looking into user-table to find both username and pin. if found we go to "UserMenu"
                    User user = context.Users.SingleOrDefault(u => u.FirstName == userName && u.Pin == pin);

                    if (user != null)
                    {
                        Console.Clear();
                        //when user found. welcome user -> UserMenu-Method.
                        Console.WriteLine("_________________________");
                        Console.WriteLine($"~Welcome back {user.FirstName}!~");
                        Console.WriteLine("-------------------------");
                        UserActions.DoUserTasks();
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or pin code.");
                    }

                }
            }


        }
    }
}
