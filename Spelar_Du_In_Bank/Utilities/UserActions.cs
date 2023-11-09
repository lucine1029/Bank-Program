using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
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

        //- Metod för att see konton och saldon.
        public static void ViewAccountInfo()
        {
            using (BankContext context = new BankContext())
            {
                int inputid = 2; //Which User ID should it view

                //-Hard coding new accounts for user 1 to test this method.
                //Account newAccount = new Account()
                //{
                //    UserId = inputid,
                //    Name = "Example",
                //    Balance = 0
                //};
                //context.Accounts.Add(newAccount);
                //context.SaveChanges();
                
                // Shows the user name at the top just for clarification
                var user = context.Users
                    .Where(t => t.Id == inputid)
                    .FirstOrDefault();
                Console.WriteLine($"{user.FirstName}");
                
                // Adds all accounts for the user selected.
                Console.WriteLine("Your Accounts and balance:\n");
                List<Account> accounts = context.Accounts
                    .Where(u => u.UserId == inputid)
                    .ToList();

                // Prints every account in the list.
                foreach (var account in accounts)
                {
                    Console.WriteLine($"[{account.Name}] \nBalance: {account.Balance}\n");
                }
            }
        }
    }
}
