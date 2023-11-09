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
                Console.WriteLine("Enter account name you wish to withdraw from:");

                string name = Console.ReadLine();

                var account = context.Accounts
                    .Where(a => a.Name == name && a.UserId == user.Id)                  
                    .SingleOrDefault();

                if (account == null)
                {
                    Console.WriteLine("Account does not exist");
                    return;
                }

                Console.WriteLine("Enter amount to withdraw:");
                decimal withdrawal = Convert.ToDecimal(Console.ReadLine());

                account.Balance -= withdrawal;
                context.SaveChanges();
                Console.WriteLine($"Withdrew {withdrawal} from {account.Name}");
                Console.WriteLine($"Current balance on {account.Name}: {account.Balance}");
              
                MenuAction.firstMenu();
               
            }



        }
    }
}
