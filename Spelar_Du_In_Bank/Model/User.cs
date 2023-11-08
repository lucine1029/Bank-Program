using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spelar_Du_In_Bank.Model
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pin { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? SSN { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
