using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EBankingProject
{
    class DataAccess : User
    {
        static DataEBankingDataContext EBankingDB = new DataEBankingDataContext();

        public static string GetUsername(int id)
        {
            string usernameDB = "";
            try
            {
                usernameDB = (from u in EBankingDB.users
                              where u.id.Equals(id)
                              select u.username).FirstOrDefault();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return usernameDB;                       
        }

        public static string GetPassword(int id)
        {
            string passwordDB = "";

            try
            {
                passwordDB = (from p in EBankingDB.users
                              where p.id.Equals(id)
                              select p.password).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return passwordDB;
        }

        public static int GetUserID(string loginUsername)
        {
            int ID = 0;
            try
            {
                ID = (from i in EBankingDB.users
                      where i.username.Equals(loginUsername)
                      select i.id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return ID;
        }

        public static int GetUserIDFromAccountTable(int id)
        {
            int userId = 0;
            try
            {
                userId = (from u in EBankingDB.accounts
                          where u.id.Equals(id)
                          select u.user_id).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return userId;
        }

        public static DateTime GetTransactionDate(int userID)
        {
            DateTime transactionDate = new DateTime();
            try
            {
                transactionDate = (from t in EBankingDB.accounts
                                   where t.id.Equals(userID)
                                   select t.transaction_date).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return transactionDate;
        }

        //Set Balance property for the object User
        public static decimal GetUserBalance(int id)
        {
            decimal balance = 0;
            try
            {
                balance = (from b in EBankingDB.accounts
                           where b.user_id.Equals(id)
                           select b.amount).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            

            return balance;
        }
        
        public static void AddToBalance(int userId, decimal amountToTransfer)
        {
            try
            {
                var balance = EBankingDB.accounts.Single(b => b.user_id == userId);
                balance.amount = balance.amount + amountToTransfer;

                var dateTime = EBankingDB.accounts.Single(d => d.user_id == userId);
                dateTime.transaction_date = DateTime.Now;

                EBankingDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        public static void SubtractToBalance(int userId, decimal amountToSubtract)
        {
            try
            {
                var balance = EBankingDB.accounts.Single(b => b.user_id == userId);
                balance.amount = balance.amount - amountToSubtract;

                var dateTime = EBankingDB.accounts.Single(d => d.user_id == userId);
                dateTime.transaction_date = DateTime.Now;

                EBankingDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        public static List<User> GetListOfUserAccountInfo(User loginUser, List<User> listOfAccountInfo)
        {
            loginUser = GetUserAccount(loginUser);
            listOfAccountInfo.Add(loginUser);

            return listOfAccountInfo;
        }
        
        public static User GetUserAccount(User loginUser)
        {
            try
            {
                loginUser.Username = GetUsername(loginUser.ID);
                loginUser.Balance = GetUserBalance(loginUser.ID);
                loginUser.TransactionDate = GetTransactionDate(loginUser.ID);                              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }           

            return loginUser;
        }       

        public static string GenerateSHA256String(string loginPassword)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(loginPassword);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static void ReturnPasswordToDB(string loginusername)
        {
            try
            {
                var password = EBankingDB.users.Single(u => u.username == loginusername);
                password.password = GenerateSHA256String(password.password);
                Console.WriteLine(password.password);
                EBankingDB.SubmitChanges();
                Console.WriteLine(password.password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
