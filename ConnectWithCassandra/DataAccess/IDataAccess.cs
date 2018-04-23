using System.Threading.Tasks;
using ConnectWithCassandra.Models;

namespace ConnectWithCassandra.DataAccess
{
    public interface IDataAccess
    {
        Task<ResultModel<receipt>> AddReceiptAsync();

        Task<ResultModel<item>> AddInventoryAsync(receipt receipt);

        Task<ResultModel<item>> UpdateInventoryAsync(item item);

        Task<ResultModel<item>> GetItemByIdValueAndTypeAsync(item item);

        Task<ResultModel<reservation>> AddReservation(item item);
    }
}
