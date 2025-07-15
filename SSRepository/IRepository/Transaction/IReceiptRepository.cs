
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Transaction
{
    public interface IReceiptRepository : ITranBaseRepository
    {
        object SetAccount(TransactionModel model, long FkPartyId);

    }
}
