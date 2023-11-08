using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Model
{
    internal class Account
    {
        public int Id { get; set; }   //account num
        public int UserId { get; set; }
        public string Name { get; set; } // accunt name : saving account, etc

        public decimal Balance { get; set; }
        public virtual User user { get; set; }  

    }
}
