namespace NRZ.Shared.Enums
{
    public enum PaymentRequestStatus
    {
        NEW = 1,
        PENDING,
        AUTHORIZED,
        ORDERED,
        PAID,
        EXPIRED,
        CANCELED,
        SUSPENDED,
        ACCEPTED,
        REJECTED,
        ERROR
    }
}
