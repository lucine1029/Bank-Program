using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelar_Du_In_Bank.Utilities;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public static void firstMenu()
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
                    return;  //?
                }

                AdminActions.DoAdminTasks();
                return;
            }


            //Code here for user login *****
        }
    }
}
