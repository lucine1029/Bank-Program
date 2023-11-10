﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spelar_Du_In_Bank.Utilities;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public static void firstMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Bank!");
            Console.WriteLine("Please login");

            Console.Write("Enter username:");
            string userName = Console.ReadLine();

            Console.Write("Enter pin code:");
            string pin = Console.ReadLine();

            if (userName == "admin")
            {
                if (pin != "1234")
                {
                    Console.WriteLine("Wrong password!");
                    return;
                }
                AdminActions.DoAdminTasks();
                //AdminFunctions.DoAdminTasks();
                return;
            }
            else
            {
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
                        UserMenu(user);
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or pin code.");
                    }

                }

            }
        }
        public static void UserMenu(User user)
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Choose one of the options:");
                Console.ResetColor();
                Console.WriteLine("1. Se dina konton och saldo");
                Console.WriteLine("2. Överföring mellan konton");
                Console.WriteLine("3. Ta ut pengar");
                Console.WriteLine("4. Sätt in pengar");
                Console.WriteLine("5. Öppna nytt konto");
                Console.WriteLine("6. Logga ut");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        //Se saldo method.
                        AccountInfo(context, user);
                        break;
                    case "2":
                        //Överföring method.
                        OwnTransfer();
                        break;
                    case "3":
                        //Se Withdraw method.
                        WithdrawMoney(context, user);
                        break;
                    case "4":
                        //Se Deposit method.
                        InsertMoney(context, user);
                        break;
                    case "5":
                        //calling createNewAcc method.
                        CreateNewAccount(context, user);
                        break;
                    case "6":
                        //Logout method.
                        //going back to mainmenu
                        firstMenu();
                        break;
                }
            }

        }
        public static void CreateNewAccount(BankContext context, User user)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;

                //Listing the existing accounts.
                Console.WriteLine($"{user.FirstName}s current accounts");
                Console.WriteLine("");
                var accounts = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.Accounts)
                    .SingleOrDefault()
                    .Accounts
                    .ToList();

                for (int i = 0; i < accounts.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{accounts[i].Name} Balance:{accounts[i].Balance:C2}");
                    Console.WriteLine("_____________________________________");
                    Console.ResetColor();
                }

                //Asking if user wants to creat a new account
                Console.WriteLine("_____________________________________");
                Console.WriteLine("[C] to create new Account");
                Console.WriteLine("[M] to go back to main menu");
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "c":
                        Console.Write("Enter account name:");
                        string accName = Console.ReadLine();

                        //creating new acc with 0 balance.
                        Account newAcc = new Account()
                        {
                            Name = accName,
                            Balance = 0,
                            user = user
                        };
                        context.Accounts.Add(newAcc);
                        context.SaveChanges();
                        break;

                    //returning back to "mainMenu"
                    case "m":
                        firstMenu();
                        break;
                }
            }
        }
        public static void InsertMoney(BankContext context, User user)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;

                //Listing the existing accounts.
                Console.WriteLine($"{user.FirstName}s current accounts");
                Console.WriteLine("");
                var accounts = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.Accounts)
                    .SingleOrDefault()
                    .Accounts
                    .ToList();

                for (int i = 0; i < accounts.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{accounts[i].Name} Balance:{accounts[i].Balance:C2}");
                    Console.WriteLine("_____________________________________");
                    Console.ResetColor();
                }

                //Asking if user wants to creat a new account
                Console.WriteLine("_____________________________________");
                Console.WriteLine("[D] to deposit money into your account");
                Console.WriteLine("[M] to go back to main menu");
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "d":
                        Console.Write("Which account do you want to deposit money into?:");
                        string accName = Console.ReadLine();

                        var account = context.Accounts
                       .Where(a => a.Name == accName && a.UserId == user.Id)
                       .SingleOrDefault();

                        if (account != null)
                        {
                            Console.WriteLine("How much do you want to deposit?");
                            decimal deposit = decimal.Parse(Console.ReadLine());

                            account.Balance = account.Balance + deposit;
                            context.SaveChanges();
                            Console.WriteLine($"{deposit} added to {account.Name}");
                            Console.WriteLine($"Balance: {account.Balance}");
                            context.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("This account doesnt exist!");
                        }
                        break;

                    //returning back to "mainMenu"
                    case "m":
                        firstMenu();
                        break;
                }
            }
        }
        public static void WithdrawMoney(BankContext context, User user) //Ta ut pengar metoden -Sean
        {
            while (true)
            {

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

                while (withdrawal <= 0)
                {
                    Console.WriteLine("Invalid input:");
                    Console.WriteLine("Enter amount to withdraw:");
                    withdrawal = Convert.ToDecimal(Console.ReadLine());
                }

                if (account.Balance <= 0)
                {
                    Console.WriteLine("Withdrawal failed. Insufficient funds in account:");
                    Console.WriteLine("Input any key to continue");
                    Console.ReadKey();
                    continue;
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
        public static void OwnTransfer()
        {
            using (BankContext context = new BankContext())
            {
                Console.Clear();
                Console.WriteLine("Your current accounts in system: ");
                List<Account> accounts = DbHelper.GetAllAccounts(context);

                foreach (var account in accounts)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Account Id\tAccount Name\tAvailable balance");
                    Console.ResetColor();
                    Console.WriteLine($"{account.Id}\t\t{account.Name}\t\t{account.Balance}");
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your transfer has successed! The current amount of your two accounts are: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Account Id\tAccount Name\tAvailable balance");
                    Console.WriteLine($"{fromAccount.Id} = \t\t{fromAccount.Name}\t\t{fromAccount.Balance}");
                    Console.WriteLine($"{toAccount.Id} = \t\t{toAccount.Name}\t\t{toAccount.Balance}");
                    Console.ResetColor();
                }

               
            }
        }
        public static void AccountInfo(BankContext context, User user)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Listing the existing accounts.
            Console.WriteLine($"{user.FirstName}s current accounts");
            Console.WriteLine("");
            var accounts = context.Users
                .Where(u => u.Id == user.Id)
                .Include(u => u.Accounts)
                .SingleOrDefault()
                .Accounts
                .ToList();

            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{accounts[i].Name} Balance:{accounts[i].Balance:C2}");
                Console.WriteLine("_____________________________________");
                Console.ResetColor();
            }

            //Asking if user wants to creat a new account
            Console.WriteLine("_____________________________________");
            Console.WriteLine("[S] to show full information Account");
            Console.WriteLine("[M] to go back to main menu");
            string input = Console.ReadLine().ToLower();

            switch (input)
            {
                case "s":
                    Console.WriteLine("Under construction...");
                    break;

                //returning back to "mainMenu"
                case "m":
                    firstMenu();
                    break;
            }

        }

    }
}   