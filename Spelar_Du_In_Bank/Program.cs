using Spelar_Du_In_Bank.Utilities;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BankBootstrap
{
    internal class Program
    {
        static void Main(string[] args)
        {          
            {
               //calling the menu method.
               MenuAction.MainMenu();
            }
        }
    }
}