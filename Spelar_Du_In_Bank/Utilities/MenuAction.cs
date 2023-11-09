using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelar_Du_In_Bank.Utilities;
using Spelar_Du_In_Bank.Data;

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

            using (BankContext context = new BankContext()) // Har gjort en jätteenkel inlogg för user. - Sean
            {
                var currentUser = context.Users 
                    .Where(u => u.LastName == userName && u.Pin == pin) // Dubbelkollar att användaren har matat in korekt username och pin - Sean          
                    .SingleOrDefault();

                if (currentUser == null)
                {
                    Console.WriteLine("Incorrect user name or pin!");
                }
                else
                {
                    UserActions.WithdrawMoney(context, currentUser);
                }

            }
        }
    }
}
