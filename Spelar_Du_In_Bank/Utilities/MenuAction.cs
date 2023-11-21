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
using static System.Collections.Specialized.BitVector32;
using System.ComponentModel.Design;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public static void Start()     //first method being called in program.cs 1
        {
            Console.Title = "Spelar du in?";
            MainMeny(); //It main purpoise is to change main title and call this method 
        }
        public static void MainMeny() //Main meny method, this is what will be shown when entering console starts
        {
            Console.Clear();
            string prompt =
@" .oooooo..o oooooooooo.   ooooo         oooooooooo.                        oooo                              
d8P'    `Y8 `888'   `Y8b  `888'         `888'   `Y8b                       `888                              
Y88bo.       888      888  888           888     888  .oooo.   ooo. .oo.    888  oooo   .ooooo.  ooo. .oo.   
 `""Y8888o.   888      888  888           888oooo888' `P  )88b  `888P""Y88b   888 .8P'   d88' `88b `888P""Y88b  
     `""Y88b  888      888  888  8888888  888    `88b  .oP""888   888   888   888888.    888ooo888  888   888  
oo     .d8P  888     d88'  888           888    .88P d8(  888   888   888   888 `88b.  888    .o  888   888  
8""""88888P'  o888bood8P'   o888o         o888bood8P'  `Y888""""8o o888o o888o o888o o888o `Y8bod8P' o888o o888o 
                                                        ";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(prompt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ResetColor();
            string[] options = { "Admin", "User", "About", "Exit" };   //Meny options
            Console.ForegroundColor = ConsoleColor.Black;
            //MenuHelper mainMeny = new MenuHelper(prompt, options);
            int selectedIndex = MenuHelper.RunMeny(options, false, true, 1, 13);     //Run method that registers arrowkeys and displays the options. 


            switch (selectedIndex)
            {
                case 0:
                    RunAdminChoice();
                    break;
                case 1:
                    RunUserChoice();
                    break;
                case 2:
                    DisplayAboutInfo();
                    break;
                case 3:
                    ExitProgram();
                    break;
            }
        }
        public static void ExitProgram() //Exit the game
        {
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        public static void DisplayAboutInfo() //Displays about info 
        {
            Console.Clear();
            Console.WriteLine("Made by:\nJonny Touma\nSean Ortega Schelin\nJing Zhang\nMohtaba Mobasheri\nMax Samuelsson");
            Console.WriteLine("Press any key to return to main meny");
            Console.ReadKey(true);
            MainMeny();
        }
        public static void RunAdminChoice()
        {
            Console.Clear();
            string prompt = (" \t\t\t\t\t\tWelcome Admin");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(prompt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ResetColor();
            string[] options = { "Login", "Return" };
            //MenuHelper loginMeny = new MenuHelper(prompt, options);
            int selectIndex = MenuHelper.RunMeny(options, false, true, 1, 6);

            switch (selectIndex)
            {
                case 0:
                    LoginMenu();
                    break;
                case 1:
                    MainMeny();
                    break;
            }

        }
        public static void RunUserChoice()
        {
            Console.Clear();
            string prompt = (" \t\t\t\t\t\tWelcome User");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(prompt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ResetColor();
            string[] options = { "Login", "Return" };
            //MenuHelper userLogin = new MenuHelper(prompt, options);
            int selectIndex = MenuHelper.RunMeny(options, false, true, 1, 6);

            switch (selectIndex)
            {
                case 0:
                    LoginMenu();
                    break;
                case 1:
                    MainMeny();
                    break;
            }
        }
        public static void LoginMenu()
        {
            Console.Clear();

            Console.WriteLine("To login press [any key] or press [escape key] to return");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Reads the key press and stores it to keyInfo, set to true so we dont want to show the keypress in console
            if (keyInfo.Key == ConsoleKey.Escape)   //if esc pressed return null 
            {
                Console.WriteLine("You pressed Escape key");
                Thread.Sleep(700);
                //MenuAction menuAction = new MenuAction();
                //menuAction.RunMainMenu();
                MainMeny();
            }
            Console.CursorVisible = true;
            Console.Write("Enter username:");
            string userName = Console.ReadLine();

            Console.Write("Enter pin code:");
            string pin = Console.ReadLine();

            CancellationTokenSource cts = new CancellationTokenSource();    //Create a cancellationtoken source, witch is used to create a cancellationtoken
            Thread loadingThread = new Thread(() => MenuHelper.LoadingScreen(cts.Token));   //create a new thread and start it, use lamda expression to call on method.
            loadingThread.Start();  //Start thread
            if (userName == "admin")
            {
                if (pin != "1234")
                {
                    Console.WriteLine("Wrong password!");
                    return;
                }
                cts.Cancel();   //cansell thread
                loadingThread.Join();    //Block the main thread and let it join with the main thread. whitout this there is a chance for spillower in main prompt.
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
                        cts.Cancel();
                        loadingThread.Join();
                        RunUserMenu(user);
                    }

                    else
                    {
                        cts.Cancel();
                        loadingThread.Join();
                        int attempts;
                        for (attempts = 3; attempts > 0; attempts--) // For loop that substracts attempts variable by 1 after every failed login attempts. -Sean 14/11/23
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid username or pin code.");
                            // Asking the user what to do next if log in failed. - Max
                            //Console.WriteLine("Would you like to try again? [1]: Yes\t [2]: No");
                            Console.WriteLine($"{attempts} attempts left");
                            //string tryagainInput = Console.ReadLine();
                            Console.WriteLine("Would you like to try again");
                            string[] options = { "Yes", "no" };
                            int selectIndex = MenuHelper.RunMeny(options, false, true, 1, 6);

                            switch (selectIndex)
                            {
                                case (0):
                                    Console.CursorVisible = true;
                                    Console.Write("Enter username:");
                                    userName = Console.ReadLine();

                                    Console.Write("Enter pin code:");
                                    pin = Console.ReadLine();


                                    user = context.Users.SingleOrDefault(u => u.FirstName == userName && u.Pin == pin);

                                    if (user != null)
                                    {
                                        RunUserMenu(user);
                                    }
                                    break;
                                case (1):
                                    MainMeny();
                                    break;

                                default:
                                    Console.WriteLine("Invalid input");
                                    attempts++; //I don't think the user's login attempts should decrease if they press the wrong key. Only if they input a wrong username and/or password. This prevents the attempts variable from changing if they press a wrong key
                                    break;

                            }
                        }
                        if (attempts == 0)
                        {
                            Console.WriteLine("Maximum number of attempts reached. The program will now close.");
                            Environment.Exit(1);
                        }

                    }

                }

            }

        }
        public static void RunUserMenu(User user)
        {
            using (BankContext context = new BankContext())
            {
                Console.Clear();
                string prompt = ($"\t\t\t\t\t\tWelcome back {user.FirstName}!~");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(prompt);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                Console.ResetColor();
                string[] options = {
                "Accounts & Balance",
                "Account transfer",
                "Withdrawal",
                "Insert money",
                "Open new account",
                "Logout" };
                //MenuHelper userLogin = new MenuHelper(prompt, options);
                int selectIndex = MenuHelper.RunMeny(options, false, false, 0, 6);

                switch (selectIndex)
                {
                    case 0:
                        AccountInfo(context, user);
                        break;
                    case 1:
                        OwnTransfer(context, user);
                        break;
                    case 2:
                        WithdrawMoney(context, user);
                        break;
                    case 3:
                        InsertMoney(context, user);
                        break;
                    case 4:
                        CreateNewAccount(context, user);
                        break;
                    case 5:
                        MainMeny();
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

                Console.WriteLine($"{user.FirstName}s current accounts");
                //Listing the existing accounts.
                PrintAccountinfo.PrintAccount(context, user);
                var accounts = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.Accounts)
                    .Single()
                    .Accounts
                    .ToList();

                //Asking if user wants to creat a new account

                //Console.WriteLine("_____________________________________");
                string[] options = { "Create new account", "Main menu" };
                int selectedIndex = MenuHelper.RunMeny(options, true, true, 1, 1);
                //Console.WriteLine("[C] to create new Account");
                //Console.WriteLine("[M] to go back to main menu");
                //string input = Console.ReadLine().ToLower();

                switch (selectedIndex)
                {
                    case (0):
                        Console.Clear();
                        PrintAccountinfo.PrintAccount(context, user);
                        Console.CursorVisible = true;
                        string accName = AdminActions.GetNonEmptyInput("Enter account name:");
                        if (accName == null)
                        {
                            RunUserMenu(user);
                        }
                        //creating new acc with 0 balance.
                        Account newAcc = new Account()
                        {
                            Name = accName,
                            Balance = 0,
                            UserId = user.Id
                        };
                        context.Accounts.Add(newAcc);
                        context.SaveChanges();
                        Console.WriteLine($"Account:[{accName}] was created successfully!");
                        Console.WriteLine("Press ENTER to go back");
                        Console.ReadKey();
                        break;

                    //returning back to "mainMenu"
                    case (1):
                        RunUserMenu(user);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input! Enter valid command.");
                        Console.ResetColor();
                        //added this so the error message displays before being ereased. /Mojtbaa
                        Thread.Sleep(1200);
                        Console.Clear();
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
                Console.WriteLine($"{user.FirstName}s current accounts");
                Console.WriteLine("");
                //Listing the existing accounts with "printAccountInfo".
                PrintAccountinfo.PrintAccount(context, user);
                var accounts = context.Users
                    .Where(u => u.Id == user.Id)
                    .Include(u => u.Accounts)
                    .Single()
                    .Accounts
                    .ToList();


                //Console.WriteLine("_____________________________________");
                string[] options = { "Deposit money", "Main menu" };
                int selectedIndex = MenuHelper.RunMeny(options, true, true, 1, 1);
                //Console.WriteLine("[D] to deposit money into your account");
                //Console.WriteLine("[M] to go back to main menu");
                //string input = Console.ReadLine().ToLower();

                switch (selectedIndex)
                {
                    case (0):
                        Console.CursorVisible = true;
                        Console.Clear();
                        PrintAccountinfo.PrintAccount(context, user);
                    //added a goto function when the input is not valid. /Mojtaba
                    WhichAccToDeposit: Console.Write("Enter account ID you wish to deposit into: ");
                        if (int.TryParse(Console.ReadLine(), out int accId))
                        {
                            var account = context.Accounts
                                .Where(a => a.Id == accId && a.UserId == user.Id)
                                .SingleOrDefault();

                            if (account != null)
                            {
                                //added while loop so you can enter again if input is not numbers
                                while (true)
                                {     //added a goto function when the input is not valid. /Mojtaba
                                HowmuchDeposit: Console.Write("How much do you want to deposit? ");
                                    //used a tryparse if entered input is invalid.
                                    if (decimal.TryParse(Console.ReadLine(), out decimal deposit) && deposit > 0)
                                    {
                                        account.Balance = account.Balance + deposit;
                                        context.SaveChanges();
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine("_____________________________________");
                                        Console.ResetColor();

                                        Console.WriteLine($"{deposit:c2} were added to {account.Name} account");
                                        Console.WriteLine($"Your new balance is: {account.Balance:C2}");
                                        Console.WriteLine("Press ENTER to go back");
                                        Console.CursorVisible = false;
                                        //added "ReadKey" so the message displays before going to next step.
                                        //otherwise the message will not show. /Mojtaba
                                        Console.ReadKey();
                                        Console.Clear();
                                        context.SaveChanges();
                                        break;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        PrintAccountinfo.PrintAccount(context, user);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid input! Enter valid number.");
                                        Console.ResetColor();
                                        //added a goto function when the input is not valid
                                        //So it doesnt go to the very begning. /Mojtaba
                                        goto HowmuchDeposit;
                                    }
                                }
                            }
                            else
                            {
                                Console.Clear();
                                PrintAccountinfo.PrintAccount(context, user);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("This account doesnt exist!");
                                Console.WriteLine("Enter a valid account name.");
                                Console.ResetColor();
                                //added a goto function when the input is not valid
                                //So it doesnt go to the very begning. /Mojtaba
                                goto WhichAccToDeposit;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            PrintAccountinfo.PrintAccount(context, user);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This account doesnt exist!");
                            Console.WriteLine("Enter a valid account name.");
                            Console.ResetColor();
                            goto WhichAccToDeposit;
                        }
                        break;
                    //returning back to "mainMenu"
                    case (1):
                        MenuAction action = new MenuAction();
                        RunUserMenu(user);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input! Enter valid command.");
                        Console.ResetColor();
                        //added this so the error message displays before being ereased. /Mojtbaa
                        Thread.Sleep(1300);
                        Console.Clear();
                        break;
                }
            }
        }
        public static void WithdrawMoney(BankContext context, User user) //- Sean. 
        {

        StartOfWithdrawal: Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Listing the existing accounts.
            Console.WriteLine($"{user.FirstName}s current accounts");
            Console.WriteLine("");

            PrintAccountinfo.PrintAccount(context, user);
            Console.ResetColor();

            //Console.WriteLine("_____________________________________");

            //Console.WriteLine("[W] to withdraw money frpm your account");
            //Console.WriteLine("[M] to go back to main menu");
            string[] options = { "Withdraw money", "Main menu" };
            int selectedIndex = MenuHelper.RunMeny(options, true, true, 1, 1);

            string withdrawInput = "";


            switch (selectedIndex)
            {
                case (0):
                    Console.Clear();
                    PrintAccountinfo.PrintAccount(context, user);
                    Console.CursorVisible = true;
                    Console.WriteLine("Please enter account ID you want to withdraw from: \nInput [M] to return to main menu:");
                    withdrawInput = Console.ReadLine();
                    int intInput;

                    if (withdrawInput.ToLower() == "m")
                    {
                        RunUserMenu(user);
                    }
                    try
                    {
                        intInput = Convert.ToInt32(withdrawInput);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. \nPress enter to start over:");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        goto StartOfWithdrawal;
                    }

                    var account = context.Accounts // LINQ query that searches for bank account with corresponding account ID number
                .Where(a => a.Id == intInput && a.UserId == user.Id)
                .SingleOrDefault();

                    if (account == null) // if statement if searched account doesnt exist.
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Account does not exist");
                        Console.WriteLine("Input any key to continue:");
                        Console.ResetColor();
                        Console.ReadKey();
                        goto StartOfWithdrawal;
                    }

                    Console.WriteLine();
                AmountToWithdraw: Console.WriteLine("Enter amount to withdraw or [M] to return to main menu:");
                    decimal withdrawal = 0;
                    bool isNumber = false;

                    if (withdrawInput.ToLower() == "m")
                    {
                        RunUserMenu(user);
                    }

                    while (isNumber == false)
                    {
                        try
                        {
                            withdrawInput = Console.ReadLine();
                            if (withdrawInput.ToLower() == "m")
                            {
                                RunUserMenu(user);
                            }
                            withdrawal = Convert.ToDecimal(withdrawInput);
                            isNumber = true;
                        }
                        catch (FormatException)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter numbers and not letters:");
                            Console.ResetColor();
                            goto AmountToWithdraw;
                        }
                    }

                    while (withdrawal <= 0) // Decimal check here. 
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input");
                        Console.ResetColor();
                        goto AmountToWithdraw;

                    }


                    Console.WriteLine("Please enter PIN to continue:");

                    string pin = Console.ReadLine();

                    while (pin != user.Pin)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid pin code! Please try again or [M] to return to main menu:");
                        Console.ResetColor();
                        pin = Console.ReadLine();
                        if (pin.ToLower() == "m")
                        {
                            RunUserMenu(user);
                        }

                    }
                    Console.Clear();
                    if (pin == user.Pin)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Correct PIN code. Withdrawal authorized.");
                        Console.ResetColor();
                    }

                    if (account.Balance < withdrawal)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Withdrawal failed. Insufficient funds in account:");
                        Console.ResetColor();
                        Console.WriteLine("Input any key to continue");
                        Console.ReadKey();
                        goto StartOfWithdrawal;
                    }

                    account.Balance -= withdrawal;

                    context.SaveChanges();
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    PrintAccountinfo.PrintAccount(context, user);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Withdrew {withdrawal:C2} from {account.Name}");
                    Console.WriteLine($"Current balance on {account.Name}: {account.Balance:C2}");
                    Console.ResetColor();
                    Console.WriteLine("Press enter to return to menu:");
                    Console.ReadKey();
                    RunUserMenu(user);
                    break;

                case (1):
                    RunUserMenu(user);
                    break;

                default:
                    WithdrawMoney(context, user);
                    break;
            }

        }
        public static void OwnTransfer(BankContext context, User user) // Jing.
        {

            Console.Clear();
            Console.WriteLine($"{user.FirstName}'s accounts:");
            //Newly added 
            int returnAccountNum = PrintAccountinfo.PrintAccount(context, user);

            //Console.WriteLine("_____________________________________");
            string[] options = { "Transfer whitin accounts", "Main menu" };
            int selectedIndex = MenuHelper.RunMeny(options, true, true, 1, 1);

            //Console.WriteLine("[T] to transfer within your accounts");
            //Console.WriteLine("[M] to go back to main menu");
            //string input = Console.ReadLine().ToLower();
            MenuAction action = new MenuAction();
            switch (selectedIndex)
            {
                case (0):
                    //added a goto function when the input is not valid
                    if (returnAccountNum != 1)
                    {
                        Console.Clear();
                        PrintAccountinfo.PrintAccount(context, user);
                        Console.CursorVisible = true;
                        WhichAccToTransferFrom: Console.Write("Please enter ID of account you wish to transfer FROM: ");
                        string fromAcc = Console.ReadLine();   //vertify if the account name exist
                        int fromAccId;
                        try
                        {
                            fromAccId = Convert.ToInt32(fromAcc);
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input, please press [Enter] to try again!");
                            Console.ReadKey();
                            Console.ResetColor();
                            goto WhichAccToTransferFrom;
                        }
                        var fromAccount = context.Accounts
                           .Where(a => a.Id == fromAccId && a.UserId == user.Id)
                           .SingleOrDefault();
                        decimal amount;

                        if (fromAccount != null)
                        {
                        WhichAccToTransferTo: Console.Write("Please enter ID  of account you wish to transfer TO: ");
                            string toAcc = Console.ReadLine();  //vertify if the account name exist
                            int toAccId;
                            try
                            {
                                toAccId = Convert.ToInt32(toAcc);
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input, please press [Enter] to try again!");
                                Console.ReadKey();
                                Console.ResetColor();
                                goto WhichAccToTransferTo;
                            }
                            var toAccount = context.Accounts
                               .Where(a => a.Id == toAccId && a.UserId == user.Id)
                            .SingleOrDefault();

                            if (toAccount != null)
                            {
                            HowMuchAmount: Console.Write("Enter transfer amount: "); //vertify if the amount has over the balance
                                                                                          //use a tryparse if enter input is invalid.
                                if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0 && amount < fromAccount.Balance)
                                {
                                    fromAccount.Balance -= amount;   //balances change saved
                                    context.SaveChanges();
                                    toAccount.Balance += amount;
                                    context.SaveChanges();

                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    //Console.WriteLine("Your transfer has successed! The current amount of your accounts are: ");
                                    Console.WriteLine("Your transfer was successful!");
                                    Console.WriteLine($"You transfered {amount:c2} from {fromAccount.Name} account to {toAccount.Name} account");
                                    Console.WriteLine($"Your new balance: {fromAccount.Name} account: {fromAccount.Balance:c2} & {toAccount.Name} account: {toAccount.Balance:c2}");
                                    //PrintAccountinfo.PrintAccount(context, user);
                                    Console.WriteLine();
                                    Console.WriteLine("Enter any key back to the main menu....");
                                    Console.ReadKey();

                                    RunUserMenu(user);
                                    break;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid command, please try again");
                                    Console.ResetColor();
                                    goto HowMuchAmount;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid Account Name, please try again");
                                Console.ResetColor();
                                goto WhichAccToTransferTo;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Account Name, please try again");
                            Console.ResetColor();
                            goto WhichAccToTransferFrom;
                        }
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sorry, you only have 1 account. Please create an new account first!");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("Entery any key back to the main menu....");
                        Console.ReadKey();

                        RunUserMenu(user);
                    }
                    break;

                //retruning back to mainMenu
                case (1):

                    RunUserMenu(user);
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input! Enter valid command.");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    OwnTransfer(context, user);
                    break;
            }
        }
        public static void AccountInfo(BankContext context, User user) // Max
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            //MenuAction action = new MenuAction();

            //Listing the existing accounts.
            Console.WriteLine($"{user.FirstName}s current accounts");
            Console.WriteLine("");
            // Changed to the Method PrintAccount and replaced the old code.
            PrintAccountinfo.PrintAccount(context, user);
            Console.ResetColor();

            //Asking if user wants to creat a new account
            string[] options = { "Account information", "Main menu" };
            int selectedIndex = MenuHelper.RunMeny(options, true, true, 1, 1);


            switch (selectedIndex)
            {
                case 0:
                    var userInfo = context.Users
                       .Where(i => i.Id == user.Id)
                       .Select(i => new { i.FirstName, i.LastName, i.Email, i.Phone, i.SSN, AccountCount = i.Accounts.Count() })
                       .FirstOrDefault();
                    Console.Clear();
                    // 2 Methods for coloring console texts. -Max
                    static void MakeYellow()
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    static void MakeWhite()
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    MakeYellow();
                    Console.WriteLine("Your information: ");

                    MakeYellow();
                    Console.Write($"Full Name: ");
                    MakeWhite();
                    Console.WriteLine($"{userInfo.FirstName} {userInfo.LastName}");

                    MakeYellow();
                    Console.Write($"Email: ");
                    MakeWhite();
                    Console.WriteLine($"{userInfo.Email}");

                    MakeYellow();
                    Console.Write($"Phone: ");
                    MakeWhite();
                    Console.WriteLine($"{userInfo.Phone}");

                    MakeYellow();
                    Console.Write($"SSN: ");
                    MakeWhite();
                    Console.WriteLine($"{userInfo.SSN}");
                    MakeYellow();
                    Console.WriteLine($"You currently have {userInfo.AccountCount} Accounts");
                    PrintAccountinfo.PrintAccount(context, user);
                    Console.ResetColor();
                    // Asks if user wants to go back - Max
                    Console.Write("\nEnter [M] to go back to main menu: ");
                    do
                    {
                        string gotoMenu = Console.ReadLine().ToLower();
                        if (gotoMenu == "m")
                        {

                            RunUserMenu(user);
                        }
                        else
                        {
                            Console.WriteLine("Not a valid input... Enter [M] to go back to Main Menu");
                        }
                    } while (true);
                    break;

                case 1:

                    RunUserMenu(user);
                    break;
            }
        }
    }
}