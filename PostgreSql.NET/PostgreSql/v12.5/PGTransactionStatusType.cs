namespace PostgreSql
{
    public enum PGTransactionStatusType : uint
    {
        /// <summary>
        /// connection idle
        /// </summary>
        PQTRANS_IDLE,

        /// <summary>
        /// command in progress
        /// </summary>
        PQTRANS_ACTIVE,

        /// <summary>
        /// idle, within transaction block
        /// </summary>
        PQTRANS_INTRANS,

        /// <summary>
        /// idle, within failed transaction
        /// </summary>
        PQTRANS_INERROR,

        /// <summary>
        /// cannot determine status
        /// </summary>
        PQTRANS_UNKNOWN,
    }
}