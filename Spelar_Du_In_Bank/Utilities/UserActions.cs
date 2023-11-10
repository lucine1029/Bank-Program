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

        public static void DoUserTasks()
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine("Welcome to the user menu: ");


                Console.WriteLine("1. View your accounts and balance");
                Console.WriteLine("2. Transfer between accounts");
                Console.WriteLine("3. Withdraw money");
                Console.WriteLine("4. Deposit money");
                Console.WriteLine("5. Open new account");
                Console.WriteLine("6 Log out");

                while (true)
                {
                    Console.WriteLine("Enter command: ");
                    string command = Console.ReadLine();
                    //List<User> user = DbHelper.GetAllUsers(context)
                    //    .Single(),
                    //    .ToList

                    var user = context.Users.FirstOrDefault();    //need to double check?

                    switch (command)
                    {
                        case "1":

                            break;
                        case "2":
                            OwnTransfer();
                            break;
                        case "3":
                            return;
                        case "4":
                            return;
                        case "5":
                            CreateAccount(context, user);
                            break;
                        case "6":
                            return;
                        default:
                            Console.WriteLine($"Unkown command: {command} ");
                            break;
                    }
                }
            }
        }



        public static void OwnTransfer()
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine("Your current accounts in system: ");
                List<Account> accounts = DbHelper.GetAllAccounts(context);

                foreach (var account in accounts)
                {
                    Console.WriteLine($"Account Name\t\t\t\t\t\tAccount Id\t\t\t\t\t\tAvailable balance");
                    Console.WriteLine($"{account.Name}\t\t{account.Id}\t\t{account.Balance}");
                }

                Console.WriteLine("Transfer from account (please enter the Account Id): ");
                int fromAccountId = int.Parse(Console.ReadLine());   //vertify if the account id exist
                Console.WriteLine("Transfer to account (please enter Account Id): ");
                int toAccountId = int.Parse(Console.ReadLine());  //vertify if the account id exist
                Console.WriteLine("Enter transfer amount : ");

                decimal Amount = Convert.ToDecimal(Console.ReadLine());  //vertify if the amount has over the balance
                var fromAccount = DbHelper.GetAllAccounts(context)
                    .Where(a => a.Id == fromAccountId)
                    .FirstOrDefault();
                if (fromAccount != null)
                {
                    if (fromAccount.Balance >= Amount)
                    {
                        fromAccount.Balance -= Amount;
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("The money you want to transfer has over your balance, please enter E to exit or C to continue: ");
                        string option = Console.ReadLine().ToUpper();   //need to add a loop here

                    }
                }

                var toAccount = DbHelper.GetAllAccounts(context)
                    .Where(a => a.Id == toAccountId)
                    .FirstOrDefault();

                if (toAccount != null)
                {
                    toAccount.Balance += Amount;
                    context.SaveChanges();
                }

                Console.WriteLine("Your transfer has successed! The current amount of your two accounts are: ");
                Console.WriteLine($"Account Name\t\t\t\t\t\ttAccount Id\t\t\t\t\tAvailable balance");
                Console.WriteLine($"{fromAccount.Name}\t\t{fromAccount.Id}\t\t{fromAccount.Balance}");
                Console.WriteLine($"{toAccount.Name}\t\t{toAccount.Id}\t\t{toAccount.Balance}");
            }
        }


        private static void CreateAccount(BankContext context, User user)  //how to handle with UserId
        {
            Console.WriteLine("Create account");
            Console.WriteLine("Enter account's name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the starting balance: ");// need to change to 0 later
            decimal balance = Convert.ToDecimal(Console.ReadLine());

            //StringBuilder sb = new StringBuilder(); ??
            Random random = new Random();
            string pin = random.Next(1000, 10000).ToString();

            Account newAccount = new Account()
            {
                Name = name,
                Balance = balance, 
                user = user    //take in the user
            };
            bool success = DbHelper.AddAccount(context, newAccount);
            if (success)
            {
                Console.WriteLine($"You has just created an new account {name} successfully!");
            }
            else
            {
                Console.WriteLine($"You failed to create an new {name}");
            }

        }


        //public static void WithDraw(BankContext context)
        //{
        //    //var account = DbHelper.GetAllAccounts(context)
        //    //    .Where(a => a.Id == f)

        //}

        //public static void Deposit(BankContext context)
        //{

        //}
    }
}
