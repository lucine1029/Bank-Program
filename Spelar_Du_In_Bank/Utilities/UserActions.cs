using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Utilities
{
    internal static class UserActions
    {
        /*
          Se dina konton och saldo	-Max
          Överföring mellan konton	-Jing
          Ta ut pengar	-Sean
          Sätt in pengar	-Muhtaba
          Öppna nytt konto	-Jonny
         */

        public static void WithdrawMoney(BankContext context, User user) //Ta ut pengar metoden -Sean
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter account name you wish to withdraw from:");
                Console.WriteLine("Input 'r' to return to menu:");

                string input = Console.ReadLine();

                if (input.ToLower() == "r")
                {
                    MenuAction.firstMenu();
                }

                var account = context.Accounts
                    .Where(a => a.Name == input && a.UserId == user.Id)                  
                    .SingleOrDefault();

                if (account == null)
                {
                    Console.Clear();
                    Console.WriteLine("Account does not exist");
                    Console.WriteLine("Input any key to continue:");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Enter amount to withdraw:");
                Console.WriteLine("Input 'r' to return to menu:");
                
                input = Console.ReadLine();

                if (input.ToLower() == "r")
                {
                    MenuAction.firstMenu();
                }

                decimal withdrawal = Convert.ToDecimal(input);
               
                while (withdrawal < 0)
                {
                    Console.WriteLine("Invalid input:");
                    Console.WriteLine("Enter amount to withdraw:");
                    withdrawal = Convert.ToDecimal(Console.ReadLine());
                }

                account.Balance -= withdrawal;
                context.SaveChanges();
                Console.WriteLine($"Withdrew {withdrawal} from {account.Name}");
                Console.WriteLine($"Current balance on {account.Name}: {account.Balance}");

                Console.WriteLine("Input any key to continue:");
                Console.ReadKey();
                MenuAction.firstMenu();
               
            }



        }
    }
}
