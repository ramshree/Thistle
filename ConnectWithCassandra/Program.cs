using System;
using System.IO;
using System.Threading.Tasks;

namespace ConnectWithCassandra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DataAccess.DataAccess da = new DataAccess.DataAccess();

            File.AppendAllText(@"C:\Guids.txt", $"\r\n{DateTime.Now} : Writing to file\r\n");

            Task.Run(async () =>
           {
               var receipt = await da.AddReceiptAsync();
               File.AppendAllText(@"C:\Guids.txt", $"receiptId: {receipt.resultContent.receiptId.ToString()}\r\n");
               var item = await da.AddInventoryAsync(receipt.resultContent);
               File.AppendAllText(@"C:\Guids.txt", $"itemId: {item.resultContent.identifierValue.ToString()}\r\n");

               var reservation = await da.AddReservation(item.resultContent);
               File.AppendAllText(@"C:\Guids.txt", $"reservationId: {reservation.resultContent.reservationId.ToString()}\r\n");

               //await da.UpdateInventoryAsync(item.resultContent);

           }).GetAwaiter().GetResult();

        }
    }
}
