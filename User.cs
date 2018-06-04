using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBankingProject
{
    class User
    {
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (value.Length < 3 || value.Length > 20)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid character length!");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    username = value;
                }
            }
        }

        public decimal Balance { get; set; }        
        public string Password { get; set; }
        public int ID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Transaction { get; set; }
        public decimal TransactionAmount { get; set; }
        public string DestiationAccountName { get; set; }
        public  string SenderAccountName { get; set; }
    }
}
