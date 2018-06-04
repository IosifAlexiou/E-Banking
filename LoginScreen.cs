using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBankingProject
{
    class LoginScreen
    {
        public static void LoginValidation()
        {            
            BankAccount dailyBuffer = new BankAccount();
            dailyBuffer.MemoryBuffer = new List<User>();
            int count = 0;

            do
            {
                ++count;               
                Console.WriteLine("Login Screen");
                Console.Write("Please enter your Username: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string username = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please enter your password: ");
                ConsoleKeyInfo key;                
                Console.ForegroundColor = ConsoleColor.Green;
                string password = String.Empty;

                do
                {
                    key = Console.ReadKey(true);
                    
                   if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password = password.Substring(0, (password.Length - 1));
                            Console.Write("\b \b");
                        }
                    }
                }
                // Stops Receving Keys Once Enter is Pressed
                while (key.Key != ConsoleKey.Enter);
                Console.ForegroundColor = ConsoleColor.White;

                password = DataAccess.GenerateSHA256String(password);

                int id = DataAccess.GetUserID(username);
                string usernameDB = DataAccess.GetUsername(id);
                string passwordDB = DataAccess.GetPassword(id);
                                

                if (usernameDB == username && passwordDB == password)
                {
                    dailyBuffer.MemoryBuffer = ApplicationMenu.GetApplicationMenu(id, dailyBuffer.MemoryBuffer);
                    break;
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Password or Username");                    
                    Console.WriteLine(" ");
                    Console.ForegroundColor = ConsoleColor.White;                    
                }
            }
            while (count < 3);

            if(count == 3)
            {
                Application.Exit();
            }            
        }        
    }
}
