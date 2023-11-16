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
        public static void PrintAccount(BankContext context, User user)
        {
            var accounts = context.Accounts
             .Where(a => a.UserId == user.Id)
             .ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            var table = new ConsoleTable("Account Id", "Account Name", "Available balance");
            foreach (var acc in accounts)
            {
                table.AddRow(acc.Id, acc.Name, acc.Balance.ToString("C2", CultureInfo.CreateSpecificCulture("sv-SE")));
            }
            table.Options.EnableCount = false;
            table.Write();
            Console.ResetColor();
        }

        public static void PrintUserList(BankContext context)
        {
            List<User> users = DbHelper.GetAllUsers(context);
            Console.ForegroundColor = ConsoleColor.Yellow;
            var table = new ConsoleTable("User Id", "User FirstName", "User LastName");
            foreach (var u in users)
            {
                table.AddRow(u.Id, u.FirstName, u.LastName);
            }
            table.Options.EnableCount = false;
            table.Write();
            Console.ResetColor();
        }
    }
}
