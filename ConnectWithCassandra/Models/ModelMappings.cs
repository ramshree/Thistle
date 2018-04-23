using Cassandra.Mapping;
using ConnectWithCassandra.Models;
using System;
using System.Collections.Generic;

namespace ConnectWithCassandra.Models
{
    public class ModelMappings : Mappings
    {
        public ModelMappings()
        {
            For<receipt>()
                .KeyspaceName("simrevamp")
                .TableName("receipt_by_receipt_id")
                .PartitionKey(pk1 => pk1.businessUnit, pk2 => pk2.receiptId)
                .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
                .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
                .Column((u) => u.domain, (cm) => cm.WithName("domain"))
                .Column((u) => u.receiptType, (cm) => cm.WithName("receipt_type"))
                .Column((u) => u.user, (cm) => cm.WithName("user"))
                .Column((u) => u.dateCreated, (cm) => cm.WithName("date_created"))
                .Column((u) => u.dateUpdate, (cm) => cm.WithName("date_updated"))
                .ExplicitColumns();

            For<item>()
                .KeyspaceName("simrevamp")
                .TableName("inventory_by_location_and_upc")
                .PartitionKey(pk1 => pk1.businessUnit, pk2 => pk2.location, pk3 => pk3.upc)
                .ClusteringKey(Tuple.Create("identifier_value", SortOrder.Ascending), Tuple.Create("identifier_type", SortOrder.Ascending))
                .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
                .Column(u => u.location, cm => cm.WithName("location"))
                .Column(u => u.upc, cm => cm.WithName("upc"))
                .Column(u => u.availableCount, cm => cm.AsStatic().WithName("available_count"))
                .Column(u => u.identifierValue, cm => cm.WithName("identifier_value"))
                .Column(u => u.identifierType, cm => cm.WithName("identifier_type"))
                .Column(u => u.serialNumber, cm => cm.WithName("serial_number"))
                .Column(u => u.status, cm => cm.WithName("status"))
                .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
                .Column(u => u.dateCreated, cm => cm.WithName("date_created"))
                .Column(u => u.dateUpdated, cm => cm.WithName("date_updated"))
                .Column(u => u.itemAttributes, cm => cm.AsFrozen().WithDbType<Dictionary<string, string>>().WithName("item_attributes"))
                .ExplicitColumns();

            For<itemHistory>()
                .KeyspaceName("simrevamp")
                .TableName("inventory_hist_by_serial_number")
                .PartitionKey(pk1 => pk1.businessUnit, pk2 => pk2.serialNumber)
                .ClusteringKey(ck => ck.histItemId)
                .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
                .Column(u => u.histItemId, cm => cm.WithName("hist_item_id"))
                .Column(u => u.location, cm => cm.WithName("location"))
                .Column(u => u.upc, cm => cm.WithName("upc"))
                .Column(u => u.identifierValue, cm => cm.WithName("identifier_value"))
                .Column(u => u.identifierType, cm => cm.WithName("identifier_type"))
                .Column(u => u.serialNumber, cm => cm.WithName("serial_number"))
                .Column(u => u.status, cm => cm.WithName("status"))
                .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
                .Column(u => u.dateCreated, cm => cm.WithName("date_created"))
                .Column(u => u.dateUpdated, cm => cm.WithName("date_updated"))
                .Column(u => u.itemAttributes, cm => cm.AsFrozen().WithDbType<Dictionary<string, string>>().WithName("item_attributes"))
                .ExplicitColumns();

            For<itemByIdentifier>()
               .KeyspaceName("simrevamp")
               .TableName("inventory_by_identifier_value_and_type")
               .PartitionKey(pk1 => pk1.businessUnit, pk2 => pk2.identifierValue, pk3 => pk3.identifierType)
               .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
               .Column(u => u.location, cm => cm.WithName("location"))
               .Column(u => u.upc, cm => cm.WithName("upc"))
               .Column(u => u.identifierValue, cm => cm.WithName("identifier_value"))
               .Column(u => u.identifierType, cm => cm.WithName("identifier_type"))
               .Column(u => u.serialNumber, cm => cm.WithName("serial_number"))
               .Column(u => u.status, cm => cm.WithName("status"))
               .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
               .Column(u => u.dateCreated, cm => cm.WithName("date_created"))
               .Column(u => u.dateUpdated, cm => cm.WithName("date_updated"))
               .Column(u => u.itemAttributes, cm => cm.AsFrozen().WithDbType<Dictionary<string, string>>().WithName("item_attributes"))
               .ExplicitColumns();

            For<reservation>()
                .KeyspaceName("simrevamp")
                .TableName("reservation_by_location_and_upc")
                .PartitionKey(r => r.businessUnit, r1 => r1.location, r2 => r2.upc)
                .ClusteringKey(c => c.reservationId)
                .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
                .Column(u => u.location, cm => cm.WithName("location"))
                .Column(u => u.upc, cm => cm.WithName("upc"))
                .Column(u => u.reservationId, cm => cm.WithName("reservation_id"))
                .Column(u => u.reserverId, cm => cm.WithName("reserver_id"))
                .Column(u => u.reservationStatus, cm => cm.WithName("reservation_status"))
                .Column(u => u.serialNumber, cm => cm.WithName("serial_number"))
                .Column(u => u.identifierValue, cm => cm.WithName("identifier_value"))
                .Column(u => u.identifierType, cm => cm.WithName("identifier_type"))
                .Column(u => u.itemStatus, cm => cm.WithName("status"))
                .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
                .Column(u => u.dateCreated, cm => cm.WithName("date_created"))
                .Column(u => u.dateUpdated, cm => cm.WithName("date_updated"))
                .Column(u => u.itemAttributes, cm => cm.AsFrozen().WithDbType<Dictionary<string, string>>().WithName("item_attributes"))
                .ExplicitColumns();

            For<reservationHistory>()
                .KeyspaceName("simrevamp")
                .TableName("reservation_hist_by_reservation_id")
                .PartitionKey(r => r.businessUnit, r1 => r1.reservationId)
                .ClusteringKey(c => c.histReservationId)
                .Column(u => u.businessUnit, cm => cm.WithName("business_unit"))
                .Column(u => u.location, cm => cm.WithName("location"))
                .Column(u => u.upc, cm => cm.WithName("upc"))
                .Column(u => u.reservationId, cm => cm.WithName("reservation_id"))
                .Column(u => u.histReservationId, cm => cm.WithName("hist_reservation_id"))
                .Column(u => u.reserverId, cm => cm.WithName("reserver_id"))
                .Column(u => u.reservationStatus, cm => cm.WithName("reservation_status"))
                .Column(u => u.serialNumber, cm => cm.WithName("serial_number"))
                .Column(u => u.identifierValue, cm => cm.WithName("identifier_value"))
                .Column(u => u.identifierType, cm => cm.WithName("identifier_type"))
                .Column(u => u.itemStatus, cm => cm.WithName("status"))
                .Column(u => u.receiptId, cm => cm.WithName("receipt_id"))
                .Column(u => u.dateCreated, cm => cm.WithName("date_created"))
                .Column(u => u.dateUpdated, cm => cm.WithName("date_updated"))
                .Column(u => u.itemAttributes, cm => cm.AsFrozen().WithDbType<Dictionary<string, string>>().WithName("item_attributes"))
                .ExplicitColumns();
        }
    }
}
