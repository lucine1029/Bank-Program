using ConsoleTables;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class PrintAccountinfo
    {
        public static int PrintAccount(BankContext context, User user)  // print out the list of accounts for the user in a table - Jing 
        {
            //List the accounts that have the same UserId as the user parameter
            var accounts = context.Accounts    
             .Where(a => a.UserId == user.Id)
             .ToList();
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Declares a variable named table with three column headers
            var table = new ConsoleTable("Account Id", "Account Name", "Available balance");
            foreach (var acc in accounts)
            {
                //add a new row to the table, using the properties of the acc object as the values.
                table.AddRow(acc.Id, acc.Name, acc.Balance.ToString("C2", CultureInfo.CreateSpecificCulture("sv-SE")));
            }
            table.Options.EnableCount = false; // The table will not display the number of rows at the bottom
            table.Write();
            Console.ResetColor();
            int accountCount = accounts.Count; //Return the number of accounts for the user
            return accountCount;
            
        }

        public static void PrintUserList(BankContext context)  // print out the list of all users in the form of a table - Jing 
        {
            //List all the users
            List<User> users = DbHelper.GetAllUsers(context);
            Console.ForegroundColor = ConsoleColor.Yellow;

            // Declares a variable named table with three column headers
            var table = new ConsoleTable("User Id", "FirstName", "LastName");
            foreach (var u in users)
            {
                //add a new row to the table, using the properties of the acc object as the values.
                table.AddRow(u.Id, u.FirstName, u.LastName);
            }
            table.Options.EnableCount = false;
            table.Write();
            Console.ResetColor();
        }
    }
}
