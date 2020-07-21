using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
   public interface ITransferDAO
    {
        int MakeTransfer(Transfer transfer);
        Transfer GetTransferDetails(int transferID);
        List<Transfer> TransferHistory(int userID);
    }
}
