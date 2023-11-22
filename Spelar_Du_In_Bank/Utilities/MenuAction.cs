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
using System.Drawing;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuAction
    {
        public void Start()     //first method being called in program.cs 1
        {
            Console.Title = "Spelar du in?";
            RunMainMenu(); //It main purpoise is to change main title and call this method 
        }
        public void RunMainMenu() //Main meny method, this is what will be shown when entering console starts
        {
            string prompt =
@" .oooooo..o oooooooooo.   ooooo         oooooooooo.                        oooo                              
d8P'    `Y8 `888'   `Y8b  `888'         `888'   `Y8b                       `888                              
Y88bo.       888      888  888           888     888  .oooo.   ooo. .oo.    888  oooo   .ooooo.  ooo. .oo.   
 `""Y8888o.   888      888  888           888oooo888' `P  )88b  `888P""Y88b   888 .8P'   d88' `88b `888P""Y88b  
     `""Y88b  888      888  888  8888888  888    `88b  .oP""888   888   888   888888.    888ooo888  888   888  
oo     .d8P  888     d88'  888           888    .88P d8(  888   888   888   888 `88b.  888    .o  888   888  
8""""88888P'  o888bood8P'   o888o         o888bood8P'  `Y888""""8o o888o o888o o888o o888o `Y8bod8P' o888o o888o 
                                                        ";


            string[] options = { "Admin", "User", "About", "Exit" };   //Meny options
            Console.ForegroundColor = ConsoleColor.Black;
            MenuHelper mainMeny = new MenuHelper(prompt, options);
            int selectedIndex = mainMeny.Run();     //Run method that registers arrowkeys and displays the options. 


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
        public void ExitProgram() //Exit the game
        {
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        public void DisplayAboutInfo() //Displays about info 
        {
            Console.Clear();
            Console.WriteLine("Made by:\nJonny Touma\nSean Ortega Schelin\nJing Zhang\nMohtaba Mobasheri\nMax Samuelsson");
            Console.WriteLine("Press any key to return to main meny");
            Console.ReadKey(true);
            RunMainMenu();
        }
        public void RunAdminChoice()
        {
            string prompt = (" \t\t\t\t\t\tWelcome Admin");
            string[] options = { "Login", "Return" };
            MenuHelper loginMeny = new MenuHelper(prompt, options);
            int selectIndex = loginMeny.Run();

            switch (selectIndex)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    RunMainMenu();
                    break;
            }

        }
        public void RunUserChoice()   //OBS!!!! method with switch, might not be used 
        {
            string prompt = (" \t\t\t\t\t\tWelcome User");
            string[] options = { "Login", "Return" };
            MenuHelper userLogin = new MenuHelper(prompt, options);
            int selectIndex = userLogin.Run();

            switch (selectIndex)
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    RunMainMenu();
                    break;

            }
        }
        public static void MainMenu()
        {
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Press [any key] to login or press [escape] key to return");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Reads the key press and stores it to keyInfo, set to true so we dont want to show the keypress in console
            if (keyInfo.Key == ConsoleKey.Escape)   //if esc pressed return null 
            {
                Console.WriteLine("You pressed Escape key");
                Thread.Sleep(700);
                MenuAction menuAction = new MenuAction();
                menuAction.RunMainMenu();

            }
            Console.Write("Enter username:");
            string userName = Console.ReadLine();

            Console.Write("Enter pin code:");
            string pin = Console.ReadLine();

            if (userName == "admin")
            {                               
                int attempts = 3;
                if (pin == "1234")
                {
                    AdminActions.DoAdminTasks();
                }
                else if (pin != "1234")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid admin PIN code!");
                    Console.ResetColor();
                    
                    for (attempts = 3; attempts > 0; attempts--) // For loop that substracts attempts variable by 1 after every failed login attempts. -Sean 14/11/23
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        // Asking the user what to do next if log in failed. - Max
                        Console.WriteLine("Would you like to try again? [1]: Yes\t [2]: No");
                        Console.WriteLine($"{attempts} attempts left");
                        string tryagainInput = Console.ReadLine();
                        MenuAction action = new MenuAction();
                        Console.ResetColor();
                        switch (tryagainInput)
                        {
                            case "1":
                              
                                Console.Write("Enter pin code:");
                                pin = Console.ReadLine();

                                if (pin == "1234")
                                {
                                    Console.WriteLine("Correct admin PIN");
                                    AdminActions.DoAdminTasks();
                                }
                                
                                break;
                            case "2":
                                action.RunMainMenu();
                                break;

                            default:
                                Console.WriteLine("Invalid input");
                                attempts++; //I don't think the user's login attempts should decrease if they press the wrong key. Only if they input a wrong username and/or password. This prevents the attempts variable from changing if they press a wrong key
                                break;

                        }
                        
                    }
                }                              
            } 

            
            else
            {
                using (BankContext context = new BankContext())
                {
                    //Looking into user-table to find both username and pin. if found we go to "UserMenu"
                    User user = context.Users.SingleOrDefault(u => u.FirstName == userName && u.Pin == pin);

                    if (user != null)
                    {
                        MenuAction action = new MenuAction();
                        action.RunUserMenu(user);
                    }
                    else
                    {
                        int attempts;
                        for (attempts = 3; attempts > 0; attempts--) // For loop that substracts attempts variable by 1 after every failed login attempts. -Sean 14/11/23
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid username or pin code.");
                            Console.WriteLine("If you are an administrator, please restart the program and attempt login again.");
                            // Asking the user what to do next if log in failed. - Max
                            Console.WriteLine("Would you like to try again? [1]: Yes\t [2]: No");
                            Console.WriteLine($"{attempts} attempts left");
                            string tryagainInput = Console.ReadLine();
                            MenuAction action = new MenuAction();
                            Console.ResetColor();
                            switch (tryagainInput)
                            {
                                case "1":

                                    Console.Write("Enter username:");
                                    userName = Console.ReadLine();


                                    Console.Write("Enter pin code:");
                                    pin = Console.ReadLine();

                                    user = context.Users.SingleOrDefault(u => u.FirstName == userName && u.Pin == pin);

                                    if (user != null)
                                    {
                                        action.RunUserMenu(user);
                                    }
                                    break;
                                case "2":
                                    action.RunMainMenu();
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

        } // Login page and function
        public void RunUserMenu(User user)   //OBS!!!! method with switch, might not be used 
        {
            using (BankContext context = new BankContext())
            {
                string prompt = ($"\t\t\t\t\t\tWelcome back {user.FirstName}!~");
                string[] options = { "Accounts & Balance",
                "Account transfer",
                "Withdrawal",
                "Insert money",
                "Open new account",
                "Logout" };
                MenuHelper userLogin = new MenuHelper(prompt, options);
                int selectIndex = userLogin.RunVertical();

                switch (selectIndex)
                {
                    case 0:
                        UserActions.AccountInfo(context, user);
                        break;
                    case 1:
                        UserActions.OwnTransfer(context, user);
                        break;
                    case 2:
                        UserActions.WithdrawMoney(context, user);
                        break;
                    case 3:
                        UserActions.InsertMoney(context, user);
                        break;
                    case 4:
                        UserActions.CreateNewAccount(context, user);
                        break;
                    case 5:
                        RunMainMenu();
                        break;
                }
            }
        }               
    }
}
