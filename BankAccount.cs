using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBankingProject
{
    class BankAccount
    {
        public List<User> MemoryBuffer { get; set; }

        public static List<User> GetUserListOfDaysActivity(User loginUser, List<User> memoryBuffer)
        {
            memoryBuffer = DataAccess.GetListOfUserAccountInfo(loginUser, memoryBuffer);
            
            return memoryBuffer;
        }        
    }
}
