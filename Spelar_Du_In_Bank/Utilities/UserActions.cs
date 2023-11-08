using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
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


        //-This function should be run when the user has navigated to the "Transfer between accounts" option
        //- The user must be able to choose an account to take money from, an account to move the money to and then an amount to be moved between these
        //- This amount must then be moved between these accounts and afterwards the user will be able to see which amount is on these two accounts that were affected
        //- There must be coverage on the accounts you want to transfer money from for the amount you want to transfer 您想要转出资金的账户必须符合您想要转出的金额
        //- It is enough that you can transfer between your own accounts here

        // User's accounts overview: AccoutName, AccountId, Available balance
        public static void OwnTransfer()
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine("Your current accounts in system: ");
                List<Account> accounts = DbHelper.GetAllAccounts(context);

                foreach (var account in accounts)
                {
                    Console.WriteLine($"Account Name\t\t\t\t\t\t\t\t\t\tAccount Id\t\t\t\t\t\t\t\t\t\tAvailable balance");
                    Console.WriteLine($"{account.Name}\t\t{account.Id}\t\t{account.Balance}");
                }

                Console.WriteLine("Transfer from account (please enter Account Id): ");
                string fromAccountId = Console.ReadLine();   //vertify if the account id exist
                Console.WriteLine("Transfer to account (please enter Account Id): ");
                string toAccountId = Console.ReadLine();  //vertify if the account id exist
                Console.WriteLine("Enter transfer amount: ");
                decimal Amount = Convert.ToDecimal(Console.ReadLine());

            }
        }
        public static void WithDraw(BankContext context) 
        { 

        }

        public static void Deposit(BankContext context)
        {

        }
    }
}
