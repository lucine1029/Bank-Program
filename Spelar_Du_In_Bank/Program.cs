using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using Spelar_Du_In_Bank.Utilities;
using System.Runtime.Intrinsics.X86;

namespace Spelar_Du_In_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserActions.DoUserTasks();
            //MenuAction.firstMenu();
        }
    }
}