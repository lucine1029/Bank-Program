using Microsoft.Extensions.Logging;
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
using System.Security.Principal;
using ConsoleTables;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public static void MainMenu()
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
                        UserMenu(user);
                    }
                    else
                    {

                        Console.WriteLine("Invalid username or pin code.");
                        // Asking the user what to do next if log in failed. - Max
                        Console.WriteLine("Do you wanna try again? [1]: Yes\t [2]: No");
                        string tryagainInput = Console.ReadLine();
                        switch (tryagainInput)
                        {
                            case "1":
                                Console.Write("Enter username:");
                                userName = Console.ReadLine();

                                Console.Write("Enter pin code:");
                                pin = Console.ReadLine();
                                break;
                            case "2":
                                break;

                        }


                    }
                }
            }
        }
        public static void UserMenu(User user)
        {
            Console.Clear();
            //when user found. welcome user -> UserMenu-Method.
            Console.WriteLine("_________________________");
            Console.WriteLine($"~Welcome back {user.FirstName}!~");
            Console.WriteLine("-------------------------");
            using (BankContext context = new BankContext())
            {
                //Console.WriteLine();
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
                        //OwnTransfer(context, user);
                        OwnTransfer(context, user);
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

                        MainMenu();
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
                            UserId = user.Id
                        };
                        context.Accounts.Add(newAcc);
                        context.SaveChanges();
                        break;

                    //returning back to "mainMenu"
                    case "m":
                        UserMenu(user);
                        break;
                }
            }
        }
        public static void InsertMoney(BankContext context, User user) // Mojtaba
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
                        Console.Clear();
                    //added a goto function when the input is not valid. /Mojtaba
                    WhichAccToDeposit: Console.Write("Which account do you want to deposit money into?:");
                        string accName = Console.ReadLine();

                        var account = context.Accounts
                       .Where(a => a.Name == accName && a.UserId == user.Id)
                       .SingleOrDefault();

                        if (account != null)
                        {
                            //added while loop so you can enter again if input is not numbers
                            while (true)
                            {
                                Console.WriteLine("How much do you want to deposit?");
                                //used a tryparse if entered input is invalid.
                                if (decimal.TryParse(Console.ReadLine(), out decimal deposit) && deposit > 0)
                                {
                                    account.Balance = account.Balance + deposit;
                                    context.SaveChanges();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("_____________________________________");
                                    Console.ResetColor();
                                    Console.WriteLine($"{deposit:c2} added to {account.Name} account");
                                    Console.WriteLine($"Your new balance is: {account.Balance:C2}");
                                    //added this so the message displays before going to next step. /Mojtaba
                                    Console.WriteLine("Press ENTER to go back");
                                    Console.ReadKey();
                                    Console.Clear();
                                    context.SaveChanges();
                                    break;

                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input! Enter valid number.");
                                    Console.ResetColor();
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This account doesnt exist!");
                            Console.WriteLine("Enter a valid account name.");
                            Console.ResetColor();
                            //added a goto function when the input is not valid. /Mojtaba
                            //So it doesnt go to the very begning.
                            goto WhichAccToDeposit;
                        }
                        break;

                    //returning back to "mainMenu"
                    case "m":
                        UserMenu(user);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input! Enter valid command.");
                        Console.ResetColor();
                        //added this so the error message displays before being ereased. /Mojtbaa
                        int Twomilliseconds = 2000;
                        Thread.Sleep(Twomilliseconds);
                        Console.Clear();
                        break;
                }
            }
        }
        public static void WithdrawMoney(BankContext context, User user) //- Sean
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
                //Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < accounts.Count; i++)
                {

                    Console.WriteLine($"{i + 1}.{accounts[i].Name} Balance:{accounts[i].Balance:C2}");
                    Console.WriteLine("_____________________________________");

                }
                Console.ResetColor();

                Console.WriteLine("Enter account name you wish to withdraw from:");
                Console.WriteLine("[M] to go back to main menu");

                string input = Console.ReadLine();

                if (input.ToLower() == "m")
                {
                    UserMenu(user);
                }

                var account = context.Accounts
                    .Where(a => a.Name.ToLower() == input.ToLower() && a.UserId == user.Id)
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
                Console.WriteLine("[M] to go back to main menu");

                input = Console.ReadLine();

                if (input.ToLower() == "m")
                {
                    UserMenu(user);
                }

                Console.WriteLine("Please enter PIN to continue:");

                string pin = Console.ReadLine();

                while (pin != user.Pin)
                {
                    Console.WriteLine("Invalid pin code! Please try again:");
                    Console.WriteLine("[M] to go back to main menu");
                    if (input.ToLower() == "m")
                    {
                        UserMenu(user);
                    }
                    pin = Console.ReadLine();
                }

                if (pin == user.Pin)
                {
                    Console.WriteLine("Correct PIN code. Withdrawal authorized.");
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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Withdrew {withdrawal:C2} from {account.Name}");
                Console.WriteLine($"Current balance on {account.Name}: {account.Balance:C2}");
                Console.ResetColor();
                Console.WriteLine("Input any key to continue:");
                Console.ReadKey();
                UserMenu(user);
            }

        }

        public static void OwnTransfer(BankContext context, User user) // Jing. Add code to check if valid Account ID is entered.
        {

            Console.Clear();
            Console.WriteLine($"{user.FirstName}'s accounts:");
            PrintAccountinfo.PrintAccount(context, user);   //Newly added 

            Console.WriteLine("_____________________________________");
            Console.WriteLine("[T] to transfer within your accounts");
            Console.WriteLine("[M] to go back to main menu");
            string input = Console.ReadLine().ToLower();

            switch (input)
            {
                case "t":
                //added a goto function when the input is not valid
                WhichAccToTransferFrom: Console.Write("Transfer from account (please enter the Account Name): ");
                    string fromAcc = Console.ReadLine();   //vertify if the account id exist
                    var fromAccount = context.Accounts
                       .Where(a => a.Name == fromAcc && a.UserId == user.Id)
                       .SingleOrDefault();
                    decimal amount;

                    if (fromAccount != null)
                    {
                    WhichAccToTransferTo: Console.Write("Transfer to account (please enter Account Name): ");
                        string toAcc = Console.ReadLine();  //vertify if the account id exist
                        var toAccount = context.Accounts
                           .Where(a => a.Name == toAcc && a.UserId == user.Id)
                        .SingleOrDefault();

                        if (toAccount != null)
                        {
                        HowMuchAmount: Console.WriteLine("Enter transfer amount : "); //vertify if the amount has over the balance
                                                                                      //use a tryparse if enter input is invalid.
                            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0 && amount < fromAccount.Balance)
                            {
                                fromAccount.Balance -= amount;   //balances change saved
                                context.SaveChanges();
                                toAccount.Balance += amount;
                                context.SaveChanges();

                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Your transfer has successed! The current amount of your accounts are: ");
                                PrintAccountinfo.PrintAccount(context, user);
                                Console.WriteLine();
                                Console.WriteLine("Entery any key back to the main menu....");
                                Console.ReadKey();
                                MenuAction.UserMenu(user);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid command, please try again");
                                goto HowMuchAmount;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Account Name, please try again: ");
                            goto WhichAccToTransferTo;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Account Name, please try again: ");
                        goto WhichAccToTransferFrom;
                    }

                //retruning back to mainMenu
                case "m":
                    MenuAction.UserMenu(user);
                    break;

                default:
                    Console.WriteLine("Invalid input! Enter valid command.");
                    Console.ResetColor();
                    int Twomilliseconds = 2000;
                    Thread.Sleep(Twomilliseconds);
                    OwnTransfer(context, user);
                    break;
            }
        }

        public static void AccountInfo(BankContext context, User user) // Mojtaba
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

            }
            Console.ResetColor();
            //Asking if user wants to creat a new account

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
                    UserMenu(user);
                    break;
            }

        }

    }
}