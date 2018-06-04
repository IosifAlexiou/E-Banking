using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBankingProject
{
    class ApplicationMenu
    {
        public static List<User> GetApplicationMenu (int id, List<User> memoryBuffer)
        {
            Console.Clear();
            if (id == 1)
            {
                Console.WriteLine("Administrator's Menu");
                Console.WriteLine("");
                Console.WriteLine("1. View Cooperative Internal Bank Account");
                Console.WriteLine("2. View Member's Bank Account");
                Console.WriteLine("3. Transfer to Member's Account");
                Console.WriteLine("4. Withdraw from Member's Account");
                Console.WriteLine("5. Deposit to User's Account");
                Console.WriteLine("6. Send Today's Statement");
                Console.WriteLine("7. Exit Application");
                memoryBuffer = GetAdminMenuControls(id, memoryBuffer);
            }

            else
            {
                Console.WriteLine("Member's Menu");
                Console.WriteLine("");
                Console.WriteLine("1. View Member's Bank Account");
                Console.WriteLine("2. Transfer to anohter Bank Account");
                Console.WriteLine("3. Deposit to User's Account");
                Console.WriteLine("4. Send Today's Statement");
                Console.WriteLine("5. Exit Application");
                memoryBuffer = GetMemberMenuControls(id, memoryBuffer);
            }

            return memoryBuffer;
        }

        public static List<User> GetAdminMenuControls(int id, List<User> memoryBuffer)
        {
            User member = new User();
            User loginUser = new User();
            loginUser.ID = id;
            loginUser.Username = DataAccess.GetUsername(loginUser.ID);
            
            bool validation = false;
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Please choose the appropriate number of the action you want to take.");
            ConsoleKeyInfo buttonInfo;
            buttonInfo = Console.ReadKey();

            switch (buttonInfo.KeyChar)
            {
                case '1':

                    Console.Clear();                    
                    InternalBankAccount.DisplayUserAccountInfo(loginUser);
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");                    
                    Console.WriteLine();
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '2':                   

                    do
                    {
                        Console.Clear();                        
                        Console.Write(Environment.NewLine + "Insert Member's Account Username: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        member.Username = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        validation = InternalBankAccount.ValidateDestinationUsername(member.Username, id, memoryBuffer);

                    } while (validation == false);

                    member.ID = DataAccess.GetUserID(member.Username);                    
                    Console.Clear();
                    InternalBankAccount.DisplayUserAccountInfo(member);                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '3':

                    Console.Clear();
                    memoryBuffer = InternalBankAccount.Transfer(loginUser, memoryBuffer);                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    return memoryBuffer;                   

                case '4':

                    Console.Clear();                                       

                    do
                    {
                        Console.Write("Insert Member's Account Username: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        member.Username = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        validation = InternalBankAccount.ValidateDestinationUsername(member.Username, id, memoryBuffer);

                    } while (validation == false);

                    memoryBuffer = InternalBankAccount.Withdraw(member.Username, memoryBuffer, loginUser);                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '5':

                    memoryBuffer = InternalBankAccount.Deposit(loginUser, memoryBuffer);
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '6':

                    GetStatementFileMenu(loginUser, memoryBuffer);
                    break;

                case '7':

                    Environment.Exit(0);
                    break;

                default:

                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;
            }
            return memoryBuffer;
        }

        public static List<User>  GetMemberMenuControls(int id, List<User> memoryBuffer)
        {
            User loginUser = new User();
            loginUser.ID = id;
            loginUser.Username = DataAccess.GetUsername(loginUser.ID);            
            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Please choose the appropriate number of the action you want to take.");
            ConsoleKeyInfo buttonInfo;
            buttonInfo = Console.ReadKey();

            switch (buttonInfo.KeyChar)
            {
                case '1':

                    Console.Clear();
                    Console.WriteLine("");
                    InternalBankAccount.DisplayUserAccountInfo(loginUser);                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '2':

                    Console.Clear();
                    InternalBankAccount.Transfer(loginUser, memoryBuffer);                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '3':

                    memoryBuffer = InternalBankAccount.Deposit(loginUser, memoryBuffer);
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;

                case '4':

                    GetStatementFileMenu(loginUser, memoryBuffer);
                    break;

                case '5':

                    Environment.Exit(0);
                    break;

                default:

                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice!");
                    Console.ForegroundColor = ConsoleColor.White;                    
                    Console.WriteLine(Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(id, memoryBuffer);
                    break;                   
            }

            return memoryBuffer;
        }

        public static void GetStatementFileMenu (User loginUser, List<User> memoryBuffer)
        {
            Console.Clear();            
            Console.WriteLine(Environment.NewLine + "1. Create statement file and  go to Login Screen" +
                              Environment.NewLine + "2. Create statement file and  Exit Application"  + Environment.NewLine +
                              Environment.NewLine + "Please choose the appropriate number of the action you want to take: ");
            string buttonInfo = Console.ReadLine();

            switch (buttonInfo)
            {
                case "1":

                    FileAccess.GetUserStatementFile(memoryBuffer);
                    Console.WriteLine("Statement file created successfully!");                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to return to Login Screen...");
                    Console.ReadKey();
                    Console.Clear();
                    LoginScreen.LoginValidation();
                    if(loginUser.Username != null)
                    {
                        Console.Clear();
                        GetApplicationMenu(loginUser.ID, memoryBuffer);
                    }
                    else
                    {
                        Application.Exit();
                    }
                    break;

                case "2":

                    FileAccess.GetUserStatementFile(memoryBuffer);
                    Console.WriteLine("Statement file created successfully!");                    
                    Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Press any key to Exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                    
                    break;

                default:

                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice!");
                    Console.ForegroundColor = ConsoleColor.White;                    
                    Console.WriteLine(Environment.NewLine + "Press any key to return to Main Menu...");
                    Console.ReadKey();
                    Console.Clear();
                    GetApplicationMenu(loginUser.ID, memoryBuffer);
                    break;
            }
        }
    }
}
