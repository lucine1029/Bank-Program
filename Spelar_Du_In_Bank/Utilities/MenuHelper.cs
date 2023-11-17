using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Utilities
{
    internal class MenuHelper
    {
        private int SelectedIndex;  //Fields for storing data 
        private string[] Options;
        private string Prompt;

        public MenuHelper(string propmt, string[] options)    //Konstruktor 
        {
            Prompt = propmt;
            Options = options;
            SelectedIndex = 0;
        }
        public void DisplayOptions()    //Method
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Prompt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("\nWelcome to the Spelar du in bank, what would you like to do\r\n(Use arrow keys to navigate and enter to select option)\n");

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string prefix;


                if (i == SelectedIndex)
                {
                    prefix = "";

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                {
                    prefix = " ";

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.Write($"{prefix}{currentOption}");
            }
            Console.ResetColor();

        }
        public int Run()    //Run displays options, and registers what keys been pressed 3.
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();   //Calls meny that display options I/E displays main prompt and meny options. 

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Registers key info 
                keyPressed = keyInfo.Key;   //Update selectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = 0;  //Set to max so it always resets when left key reaches array position -1 it resets to 0.
                    }
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = Options.Length - 1; //Set to max so it always resets when left key reaches array of its lenght and resets to -1.
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter); //While loop aslong keypress is not enter. 
            {

            }
            return SelectedIndex;
        }
        public int RunVertical()    //Run displays options, and registers what keys been pressed 3.
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptionsVertical();   //Calls meny that display options I/E displays main prompt and meny options. 

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Registers key info 
                keyPressed = keyInfo.Key;   //Update selectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = 0;  //Set to max so it always resets when left key reaches array position -1 it resets to 0.
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = Options.Length - 1; //Set to max so it always resets when left key reaches array of its lenght and resets to -1.
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter); //While loop aslong keypress is not enter. 
            {

            }
            return SelectedIndex;
        }

        public void DisplayOptionsVertical()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Prompt);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("---------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("\nWelcome to the Spelar du in bank, what would you like to do\r\n(Use up and down arrows to navigate and enter to select option)\n");

            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string prefix;


                if (i == SelectedIndex)
                {
                    prefix = "";

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                {
                    prefix = " ";

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix}{currentOption}");
            }
            Console.ResetColor();
        }
        public static int MenyStuffTest()
        {
           
            int selectedIndex = 0;
            string[] options = { "Account information", "Main meny" };
            ConsoleKey keyPressed;
            do
            {   //Jag ändrade den så att den alltid ligger längst ner i fönstret. /Mojtaba
                Console.SetCursorPosition(1, Console.WindowHeight - 1);
                for (int i = 0; i < options.Length; i++)
                {
                    string currentOption = options[i];
                    string prefix;


                    if (i == selectedIndex)
                    {
                        prefix = "";

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        prefix = " ";

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write($"{prefix}{currentOption}");
                }
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //Registers key info 
                keyPressed = keyInfo.Key;   //Update selectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    selectedIndex--;
                    if (selectedIndex == -1)
                    {
                        selectedIndex = 0;  //Set to max so it always resets when left key reaches array position -1 it resets to 0.
                    }
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    selectedIndex++;
                    if (selectedIndex == options.Length)
                    {
                        selectedIndex = options.Length - 1; //Set to max so it always resets when left key reaches array of its lenght and resets to -1.
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter); //While loop aslong keypress is not enter. 
            {

            }
            return selectedIndex;
        }
       
    }
}
