using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EBankingProject
{
    class FileAccess : User
    {
        public static void GetUserStatementFile(List<User> memoryBuffer)
        {            
            string transactionDateTime = string.Format("{0:yyyy-MM-dd}",
            DateTime.Now);            
            foreach (var userInfo in memoryBuffer)
            {
                string folderPath = $@"C:\Users\Sotiris\Documents\Bootcamp3\ClassrommProjects\EBankingProject\Statement File\{userInfo.Username}";
                string filePath = $@"C:\Users\Sotiris\Documents\Bootcamp3\ClassrommProjects\EBankingProject\Statement File\{userInfo.Username}\statement_{userInfo.Username}_{transactionDateTime}.txt";
                PrintToUserTextFile(userInfo, filePath, folderPath);
            }
        }       

        public static void PrintToUserTextFile(User loginUser, string filePath, string folderPath)
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (!File.Exists(filePath))
                {
                    
                    ostrm = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write, FileShare.Read);
                    writer = new StreamWriter(ostrm);
                }
                else
                {
                    ostrm = new FileStream(filePath, FileMode.Append, System.IO.FileAccess.Write, FileShare.Read);
                    writer = new StreamWriter(ostrm);
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writting");
                Console.WriteLine(e.Message);
                return;
            }

            Console.SetOut(writer);
            Console.WriteLine("");
            Console.WriteLine($"File date: {DateTime.Now}");            
            Console.WriteLine($"UserName: {loginUser.Username}");

            if (loginUser.Transaction == "Withdraw" && loginUser.ID != 1)
            {
                Console.WriteLine("Transaction made by the administrator!");
            }

            Console.WriteLine($"Transaction: {loginUser.Transaction}");

            if(loginUser.Transaction == "Withdraw" && loginUser.ID == 1)
            {
                Console.WriteLine($"Account Holder: {loginUser.DestiationAccountName}");
            }
            if(loginUser.Transaction == "Transfer" && loginUser.SenderAccountName != null)
            {
                Console.WriteLine($"Sender: {loginUser.SenderAccountName}");
            }
            if (loginUser.Transaction == "Transfer" && loginUser.DestiationAccountName != null)
            {
                Console.WriteLine($"Destination account: {loginUser.DestiationAccountName}");
            }

            Console.WriteLine($"Date: {loginUser.TransactionDate}");
            Console.WriteLine($"Transaction amount: {loginUser.TransactionAmount}");
            Console.WriteLine("");
            Console.WriteLine($"Total amount: {loginUser.Balance}");
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");
        }        
    }
}
