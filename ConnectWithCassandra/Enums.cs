/// <summary>
/// Enums to be used accross inventory system.
/// </summary>
namespace ConnectWithCassandra
{
    public enum ItemStatus
    {
        none = 1,
        available = 2,
        reserved = 3,
        sold = 4,
        returned = 5
    }

    public enum ReceiptType
    {
        add = 1,
        delete = 2,
        update = 3
    }

    public enum ReservationStatus
    {
        created = 1,
        locked = 2,
        cancel = 3
    }
}
