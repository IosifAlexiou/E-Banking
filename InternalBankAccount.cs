using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace EBankingProject
{
    class InternalBankAccount
    {
        public static List<User> Transfer(User loginUser, List<User> memoryBuffer)
        {
            User thirdParty = new User(); 
            loginUser.Balance = DataAccess.GetUserBalance(loginUser.ID);
            loginUser.Transaction = "Transfer";

            bool validation = false;                                   

            do
            {
                Console.WriteLine("");
                Console.Write("Insert destination's account username: ");
                Console.ForegroundColor = ConsoleColor.Green;
                thirdParty.Username = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;

                validation = ValidateDestinationUsername(thirdParty.Username, loginUser.ID, memoryBuffer);

                if (loginUser.Username == thirdParty.Username)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Transaction!");
                    Console.WriteLine("Same account");
                    Console.ForegroundColor = ConsoleColor.White;
                    validation = false;
                }

            } while (validation == false);

            do
            {
                Console.WriteLine("");
                Console.Write("Your current balance: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(loginUser.Balance + "   ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter amount: ");
                Console.ForegroundColor = ConsoleColor.Green;

                loginUser.TransactionAmount = Convert.ToDecimal(Console.ReadLine());
                validation = ValidateAmount(loginUser.ID, loginUser.TransactionAmount, memoryBuffer);
                Console.ForegroundColor = ConsoleColor.White;

            } while (validation == false);

            thirdParty.ID = DataAccess.GetUserID(thirdParty.Username);

            DisplayTransferInfo(loginUser, thirdParty.Username);

            Console.WriteLine("");
            Console.Write("Confirm transaction and proceed?   Y/N: ");
            string choice = Console.ReadLine();
            if (choice == "Y" || choice == "y")
            {
                DataAccess.AddToBalance(thirdParty.ID, loginUser.TransactionAmount);
                DataAccess.SubtractToBalance(loginUser.ID, loginUser.TransactionAmount);

                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Transaction Successful!");
                
                loginUser.DestiationAccountName = thirdParty.Username;
                memoryBuffer = BankAccount.GetUserListOfDaysActivity(loginUser, memoryBuffer);
                
                thirdParty.Transaction = "Transfer";
                thirdParty.SenderAccountName = loginUser.Username;
                thirdParty.TransactionAmount = loginUser.TransactionAmount;
                memoryBuffer = BankAccount.GetUserListOfDaysActivity(thirdParty, memoryBuffer);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Environment.NewLine + "Transaction was canceled");
                Console.ForegroundColor = ConsoleColor.White;

                ApplicationMenu.GetApplicationMenu(loginUser.ID, memoryBuffer);
            }           

            return memoryBuffer;
        }        

        public static List<User> Withdraw(string username, List<User> memoryBuffer, User loginUser)
        {
            User member = new User();
            member.Username = username;
            member.ID = DataAccess.GetUserID(member.Username);            
            member.Balance = DataAccess.GetUserBalance(member.ID);
            member.Transaction = "Withdraw";
            bool validation = false;                      

            do
            {
                Console.WriteLine("");
                Console.Write("Your current balance: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(member.Balance + "    ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Enter amount: ");
                Console.ForegroundColor = ConsoleColor.Green;

                member.TransactionAmount = Convert.ToDecimal(Console.ReadLine());
                validation = ValidateAmount(member.ID, member.TransactionAmount, memoryBuffer);
                Console.ForegroundColor = ConsoleColor.White;

            } while (validation == false);

            DisplayWithdrawInfo(member);

            Console.WriteLine("");
            Console.Write("Confirm transaction and proceed?   Y/N: ");
            string choice = Console.ReadLine();

            if (choice == "Y" || choice == "y")
            {
                DataAccess.SubtractToBalance(member.ID, member.TransactionAmount);

                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Transaction Successful!");

                //member = DataAccess.GetUserAccount(member);
                memoryBuffer = BankAccount.GetUserListOfDaysActivity(member, memoryBuffer);

                loginUser = DataAccess.GetUserAccount(loginUser);
                loginUser.Transaction = "Withdraw";
                loginUser.TransactionAmount = member.TransactionAmount;
                loginUser.TransactionDate = DateTime.Now;
                loginUser.DestiationAccountName = member.Username;
                memoryBuffer.Add(loginUser);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Environment.NewLine + "Transaction was canceled");
                Console.ForegroundColor = ConsoleColor.White;                
                ApplicationMenu.GetApplicationMenu(loginUser.ID, memoryBuffer);                
            }

            return memoryBuffer;
        }

        public static List<User> Deposit (User loginUser, List<User> memoryBuffer)
        {
            bool validation = false;
            Console.Clear();
            Console.WriteLine("");           

            do
            {
                Console.Write("Enter amount: ");
                Console.ForegroundColor = ConsoleColor.Green;
                loginUser.TransactionAmount = Convert.ToDecimal(Console.ReadLine());
                Console.ForegroundColor = ConsoleColor.White;

                if (loginUser.TransactionAmount > 400)
                {
                    Console.WriteLine("Invalid amount!");
                    Console.WriteLine("The amount must be between 0-400");
                    Console.WriteLine("");
                }
                else
                {
                    validation = true;
                }


            } while (validation == false);            

            loginUser.Transaction = "Deposit";
            DisplayDepositInfo(loginUser);
            Console.WriteLine("");
            Console.Write("Confirm transaction and proceed?   Y/N: ");
            string choice = Console.ReadLine();

            if (choice == "Y" || choice == "y")
            {
                DataAccess.AddToBalance(loginUser.ID, loginUser.TransactionAmount);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Transaction Successful!");                
                memoryBuffer = DataAccess.GetListOfUserAccountInfo(loginUser, memoryBuffer);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Environment.NewLine + "Transaction was canceled");
                Console.ForegroundColor = ConsoleColor.White;
                ApplicationMenu.GetApplicationMenu(loginUser.ID, memoryBuffer);
            }

            return memoryBuffer;
        }

        public static void RegisterNewUser(User loginUser)
        {


        }

        public static bool ValidateDestinationUsername(string userToDeposit, int id, List<User> memoryBuffer)
        {
            bool validation = false;
            int destinationMemberID = DataAccess.GetUserID(userToDeposit);
            string username = DataAccess.GetUsername(destinationMemberID);
            if( username != null)
            {                
                validation = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Environment.NewLine + "Invalid username!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Do you want try again?  Y/N: ");
                string answer = Console.ReadLine();
                if (answer == "N" || answer == "n")
                {
                    Console.Clear();
                    ApplicationMenu.GetApplicationMenu(id, memoryBuffer);
                }                
            }

            return validation;
        }

        public static bool ValidateAmount(int id, decimal transactionAmount, List<User> memoryBuffer)
        {
            bool validation = false;
            decimal balance = DataAccess.GetUserBalance(id);
            if( transactionAmount <= balance && transactionAmount >= 20)
            {
                validation = true;
            }
            else
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid amount!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Do you want try again?  Y/N: ");
                string answer = Console.ReadLine();
                if (answer == "N" || answer == "n")
                {
                    ApplicationMenu.GetApplicationMenu(id, memoryBuffer);
                }
            }

            return validation;
        }        

        public static void DisplayUserAccountInfo(User loginUser)
        {
            //loginUser = DataAccess.GetUserAccount(loginUser);
            loginUser.Balance = DataAccess.GetUserBalance(loginUser.ID);
            loginUser.TransactionDate = DataAccess.GetTransactionDate(loginUser.ID);

            Console.WriteLine("Account info:");            
            Console.Write(Environment.NewLine + "User ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.ID);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Latest Transaction Date: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.TransactionDate);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Balance: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(loginUser.Balance);
            Console.ForegroundColor = ConsoleColor.White;
        }       

        public static void DisplayTransferInfo(User loginUser, string userTotransfer)
        {
            Console.WriteLine("");
            Console.WriteLine("Transaction details");
            Console.Write("Account Username:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Transaction:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Transaction);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Destination Account: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(userTotransfer);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Amount to be tranfered: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(loginUser.TransactionAmount);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");        
        }

        public static void DisplayDepositInfo(User loginUser)
        {
            Console.WriteLine("");
            Console.WriteLine("Transaction details");
            Console.Write("Account Username:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Transaction:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Transaction);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Amount to be tranfered: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(loginUser.TransactionAmount);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        public static void DisplayWithdrawInfo(User loginUser)
        {
            Console.WriteLine("");
            Console.WriteLine("Transaction details");
            Console.Write("Account Username:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Username);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Transaction:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(loginUser.Transaction);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Environment.NewLine + "Amount to be tranfered: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(loginUser.TransactionAmount);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        } 
        
        public static decimal AmountInputValidation()
        {
            Console.Write("Enter amount: ");
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleKeyInfo buttonInfo;
            do
            {

                buttonInfo = Console.ReadKey();
                if (buttonInfo.KeyChar >= '0' && buttonInfo.KeyChar <= '9')
                {
                    continue;
                }
                else if (buttonInfo.Key == ConsoleKey.Backspace)
                {
                    Console.Write(" " + "\b");
                }
                else
                {
                    Console.Write("\b \b");
                }

            } while (buttonInfo.Key != ConsoleKey.Enter);

            decimal amount = Convert.ToDecimal(Console.ReadLine());
            return amount;
        }
    }
}
