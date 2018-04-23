using Cassandra;
using Cassandra.Mapping;
using ConnectWithCassandra.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ConnectWithCassandra.DataAccess
{
    public class DataAccess : BaseDataAccess, IDataAccess
    {
        #region Variables
        private IMapper mapper;
        #endregion

        #region Constructors
        public DataAccess()
        {
            MappingConfiguration.Global.Define(new ModelMappings());

            GetSession("simrevamp");
            mapper = new Mapper(session);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Insert Receipt
        /// Insert into receipt_by_receipt_id
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<receipt>> AddReceiptAsync()
        {
            var guid = Guid.NewGuid();

            receipt receiptAdd = new receipt
            {
                businessUnit = "wireless",
                receiptId = guid,
                domain = "wireless",
                receiptType = ReceiptType.add.ToString(),
                user = Environment.UserName,
                dateCreated = TimeUuid.NewId(),
                dateUpdate = null
            };

            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                CqlQueryOptions qo = new CqlQueryOptions().SetConsistencyLevel(ConsistencyLevel.LocalOne);

                await mapper.InsertAsync(receiptAdd, qo);
                sw.Stop();

                File.AppendAllText(@"C:\Guids.txt", $"AddReceiptAsync: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ResultModel<receipt>
                {
                    resultCode = ResultStatus.failure
                };
            }

            return new ResultModel<receipt>
            {
                resultContent = receiptAdd,
                resultCode = ResultStatus.success
            };
        }

        /// <summary>
        /// Insert inventory item
        /// Insert into inventory_by_location_and_upc
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        public async Task<ResultModel<item>> AddInventoryAsync(receipt receipt)
        {
            var srlNum = Guid.NewGuid();
            var itemId = Guid.NewGuid().ToCqlString();

            Stopwatch sw = Stopwatch.StartNew();
            var invCount = await mapper
                                .FirstOrDefaultAsync<int>
                                ("select available_count from inventory_by_location_and_upc where business_unit=? and location=? and upc=?", "wireless", "ye30cy", "upc123");
            sw.Stop();

            File.AppendAllText(@"C:\Guids.txt", $"AddInventoryAsync:GetCount: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


            //insert item
            item item = new item
            {
                businessUnit = "wireless",
                location = "ye30cy",
                upc = "upc123",
                identifierValue = itemId,
                identifierType = "imei",
                serialNumber = srlNum,
                receiptId = receipt.receiptId,
                status = ItemStatus.available.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = null,
                itemAttributes = null,
                availableCount = invCount + 1
            };

            //insert item history
            itemHistory itemHist = new itemHistory
            {
                businessUnit = "wireless",
                location = "ye30cy",
                upc = "upc123",
                histItemId = Guid.NewGuid(),
                identifierValue = itemId,
                identifierType = "imei",
                serialNumber = srlNum,
                receiptId = receipt.receiptId,
                status = ItemStatus.available.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = null,
                itemAttributes = null
            };

            //insert itembyIdentifier
            itemByIdentifier itemById = new itemByIdentifier
            {
                businessUnit = "wireless",
                location = "ye30cy",
                upc = "upc123",
                identifierValue = itemId,
                identifierType = "imei",
                serialNumber = srlNum,
                receiptId = receipt.receiptId,
                status = ItemStatus.available.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = null,
                itemAttributes = null
            };

            sw = Stopwatch.StartNew();

            var batch = mapper.CreateBatch(BatchType.Logged);
            batch.Insert<item>(item);
            batch.Insert<itemHistory>(itemHist);
            batch.Insert<itemByIdentifier>(itemById);
            batch.WithOptions(ac => ac.SetConsistencyLevel(ConsistencyLevel.Quorum));
            await mapper.ExecuteAsync(batch);

            sw.Stop();

            File.AppendAllText(@"C:\Guids.txt", $"AddInventoryAsync:AddItem: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


            return new ResultModel<item>
            {
                resultContent = item,
                resultCode = ResultStatus.success
            };
        }

        /// <summary>
        /// Update an inventory item
        /// </summary>
        /// <param name="oItem"></param>
        /// <returns></returns>
        public async Task<ResultModel<item>> UpdateInventoryAsync(item oItem)
        {
            //update item 
            var updateItem = Cql.New(
                                    $"SET status = {ItemStatus.reserved.ToString()} , date_updated = {TimeUuid.NewId(DateTime.Now)}"
                                     + $" WHERE business_unit={oItem.businessUnit} and location={oItem.location} and upc={oItem.upc}"
                                     + $" and identifier_value = {oItem.identifierValue} and identifier_type= {oItem.identifierType}");


            var updateItemByIdentifer = Cql.New(
                                        $" SET status = {ItemStatus.reserved.ToString()} , date_updated = {TimeUuid.NewId(DateTime.Now)}"
                                        + $" WHERE business_unit={oItem.businessUnit}"
                                        + $" and identifier_value = {oItem.identifierValue} and identifier_type= {oItem.identifierType}");

            item item = new item
            {
                businessUnit = oItem.businessUnit,
                location = oItem.location,
                upc = oItem.upc,
                identifierValue = oItem.identifierValue,
                identifierType = oItem.identifierType,
                serialNumber = oItem.serialNumber,
                receiptId = oItem.receiptId,
                status = ItemStatus.reserved.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = TimeUuid.NewId(),
                itemAttributes = null
            };

            //insert item history
            itemHistory itemHist = new itemHistory
            {
                businessUnit = oItem.businessUnit,
                location = oItem.location,
                upc = oItem.upc,
                histItemId = Guid.NewGuid(),
                identifierValue = oItem.identifierValue,
                identifierType = oItem.identifierType,
                serialNumber = oItem.serialNumber,
                receiptId = oItem.receiptId,
                status = ItemStatus.reserved.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = TimeUuid.NewId(),
                itemAttributes = null
            };

            //insert itembyIdentifier
            itemByIdentifier itemById = new itemByIdentifier
            {
                businessUnit = oItem.businessUnit,
                location = oItem.location,
                upc = oItem.upc,
                identifierValue = oItem.identifierValue,
                identifierType = oItem.identifierType,
                serialNumber = oItem.serialNumber,
                receiptId = oItem.receiptId,
                status = ItemStatus.reserved.ToString(),
                dateCreated = TimeUuid.NewId(),
                dateUpdated = TimeUuid.NewId(),
                itemAttributes = null
            };

            var batch = mapper.CreateBatch(BatchType.Logged);
            batch.Insert<item>(item);
            batch.Insert<itemHistory>(itemHist);
            batch.Insert<itemByIdentifier>(itemById);
            batch.WithOptions(ac => ac.SetConsistencyLevel(ConsistencyLevel.Quorum));
            await mapper.ExecuteAsync(batch);

            return new ResultModel<item>
            {
                resultContent = oItem,
                resultCode = ResultStatus.success
            };
        }

        /// <summary>
        /// Get Item by identifier value and type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultModel<item>> GetItemByIdValueAndTypeAsync(item item)
        {
            Stopwatch sw = Stopwatch.StartNew();

            var resultItem = await mapper.SingleOrDefaultAsync<item>("WHERE business_unit = ? and location = ? and upc = ? "
                                             + " and identifier_value = ? and identifier_type = ?"
                                             , item.businessUnit, item.location, item.upc, item.identifierValue, item.identifierType);

            sw.Stop();

            File.AppendAllText(@"C:\Guids.txt", $"GetItemByIdValueAndTypeAsync: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


            return new ResultModel<item>
            {
                resultContent = resultItem,
                resultCode = ResultStatus.success
            };
        }

        /// <summary>
        /// Create a new reservation
        /// </summary>
        /// <param name="oItem"></param>
        /// <returns></returns>
        public async Task<ResultModel<reservation>> AddReservation(item oItem)
        {
            ResultModel<reservation> results = new ResultModel<reservation>();

            //check if item available
            var resultItem = await GetItemByIdValueAndTypeAsync(oItem);

            if (resultItem != null)
            {
                ItemStatus outStatus = ItemStatus.available;
                Enum.TryParse(resultItem.resultContent.status, out outStatus);

                Stopwatch sw = Stopwatch.StartNew();

                var invCount = await mapper
                               .FirstOrDefaultAsync<int>
                               ("select available_count from inventory_by_location_and_upc where business_unit=? and location=? and upc=?", "wireless", "ye30cy", "upc123");

                sw.Stop();
                File.AppendAllText(@"C:\Guids.txt", $"AddReservation:GetCount: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


                if (outStatus == ItemStatus.available && invCount > 0)
                {
                    //upate inventory status 2 tables

                    item newItem = new item
                    {
                        businessUnit = oItem.businessUnit,
                        location = oItem.location,
                        upc = oItem.upc,
                        identifierValue = oItem.identifierValue,
                        identifierType = oItem.identifierType,
                        serialNumber = oItem.serialNumber,
                        receiptId = oItem.receiptId,
                        status = ItemStatus.reserved.ToString(),
                        dateCreated = TimeUuid.NewId(),
                        dateUpdated = TimeUuid.NewId(),
                        itemAttributes = null,
                        availableCount = invCount - 1
                    };

                    //insert item history
                    itemHistory itemHist = new itemHistory
                    {
                        businessUnit = oItem.businessUnit,
                        location = oItem.location,
                        upc = oItem.upc,
                        histItemId = Guid.NewGuid(),
                        identifierValue = oItem.identifierValue,
                        identifierType = oItem.identifierType,
                        serialNumber = oItem.serialNumber,
                        receiptId = oItem.receiptId,
                        status = ItemStatus.reserved.ToString(),
                        dateCreated = TimeUuid.NewId(),
                        dateUpdated = TimeUuid.NewId(),
                        itemAttributes = null
                    };

                    //insert itembyIdentifier
                    itemByIdentifier itemById = new itemByIdentifier
                    {
                        businessUnit = oItem.businessUnit,
                        location = oItem.location,
                        upc = oItem.upc,
                        identifierValue = oItem.identifierValue,
                        identifierType = oItem.identifierType,
                        serialNumber = oItem.serialNumber,
                        receiptId = oItem.receiptId,
                        status = ItemStatus.reserved.ToString(),
                        dateCreated = TimeUuid.NewId(),
                        dateUpdated = TimeUuid.NewId(),
                        itemAttributes = null
                    };

                    var reservationId = Guid.NewGuid();
                    //create a reservation record
                    reservation newReservation = new reservation
                    {
                        businessUnit = oItem.businessUnit,
                        reservationId = reservationId,
                        reserverId = Guid.NewGuid().ToCqlString(),
                        location = oItem.location,
                        upc = oItem.upc,
                        identifierValue = oItem.identifierValue,
                        identifierType = oItem.identifierType,
                        serialNumber = oItem.serialNumber,
                        receiptId = oItem.receiptId,
                        itemStatus = ItemStatus.reserved.ToString(),
                        dateCreated = TimeUuid.NewId(),
                        dateUpdated = TimeUuid.NewId(),
                        itemAttributes = null,
                        reservationStatus = ReservationStatus.created.ToString()
                    };

                    //create a reservation history

                    reservationHistory newReservationHist = new reservationHistory
                    {
                        businessUnit = oItem.businessUnit,
                        reservationId = reservationId,
                        histReservationId = Guid.NewGuid(),
                        reserverId = Guid.NewGuid().ToCqlString(),
                        location = oItem.location,
                        upc = oItem.upc,
                        identifierValue = oItem.identifierValue,
                        identifierType = oItem.identifierType,
                        serialNumber = oItem.serialNumber,
                        receiptId = oItem.receiptId,
                        itemStatus = ItemStatus.reserved.ToString(),
                        dateCreated = TimeUuid.NewId(),
                        dateUpdated = TimeUuid.NewId(),
                        itemAttributes = null,
                        reservationStatus = ReservationStatus.created.ToString()
                    };

                    //create a logged batch
                    ICqlBatch itembatch = mapper.CreateBatch(BatchType.Logged);
                    itembatch.Update<item>("SET status =? , available_count=?  , date_updated= ? WHERE business_unit =? and location=? and upc=? and identifier_value=? and identifier_type=? IF status=?"
                        , ItemStatus.reserved.ToString(), invCount - 1, TimeUuid.NewId()
                        , oItem.businessUnit, oItem.location, oItem.upc, oItem.identifierValue, oItem.identifierType, ItemStatus.available.ToString());

                    itembatch.Options.SetConsistencyLevel(ConsistencyLevel.Quorum);

                    sw = Stopwatch.StartNew();
                    await mapper.ExecuteAsync(itembatch);
                    sw.Stop();
                    File.AppendAllText(@"C:\Guids.txt", $"AddReservation:itemBatch:Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


                    //ICqlBatch reservationbatch = mapper.CreateBatch(BatchType.Logged);
                    //reservationbatch.InsertIfNotExists(newReservation);

                    //sw = Stopwatch.StartNew();
                    //await mapper.ExecuteConditionalAsync<reservation>(reservationbatch);
                    //sw.Stop();
                    //File.AppendAllText(@"C:\Guids.txt", $"AddReservation:reservationBatch: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");


                    ICqlBatch allBatch = mapper.CreateBatch(BatchType.Logged);
                    allBatch.Insert<reservation>(newReservation);
                    allBatch.Insert<itemByIdentifier>(itemById);
                    allBatch.Insert<itemHistory>(itemHist);
                    allBatch.Insert<reservationHistory>(newReservationHist);
                    allBatch.Options.SetConsistencyLevel(ConsistencyLevel.Quorum);

                    sw = Stopwatch.StartNew();
                    await mapper.ExecuteAsync(allBatch);
                    sw.Stop();
                    File.AppendAllText(@"C:\Guids.txt", $"AddReservation:restOfBatch: Time taken: {sw.Elapsed.Milliseconds}ms\r\n");

                    results.resultContent = newReservation;
                    results.resultCode = ResultStatus.success;
                    return results;
                }
            }

            results.resultCode = ResultStatus.failure;
            return results;
        }

        #endregion
    }
}
