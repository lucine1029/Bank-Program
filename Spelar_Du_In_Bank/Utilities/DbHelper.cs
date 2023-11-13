﻿using Spelar_Du_In_Bank.Data;
using Spelar_Du_In_Bank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Utilities
{
    internal static class DbHelper
    {
        public static List<User> GetAllUsers(BankContext context)
        {
            List<User> users = context.Users.ToList();
            return users;
        }

        public static bool AddUser(BankContext context, User user)
        {
            context.Users.Add(user);
            
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding user: {e}");
                return false;
            }
            return true;
        }

        //public static List<Account> GetAllAccounts(BankContext context)    //new added
        //{
        //    List<Account> accounts = context.Accounts.ToList();
        //    return accounts;
        //}

        //public static bool AddAccount(BankContext context, Account account)  //new added
        //{
        //    context.Accounts.Add(account);
        //    try
        //    {
        //        context.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error adding account: {e}");
        //        return false;
        //    }
        //    return true;
        //}
    }
}
