namespace PostgreSql
{
    public enum PGTransactionStatusType
    {
        PQTRANS_IDLE,
        PQTRANS_ACTIVE,
        PQTRANS_INTRANS,
        PQTRANS_INERROR,
        PQTRANS_UNKNOWN,
    }
}
