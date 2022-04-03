namespace PostgreSql
{
    public enum PostgresPollingStatusType : uint
    {
        PGRES_POLLING_FAILED = 0,

        /// <summary>
        /// These two indicate that one may
        /// </summary>
        PGRES_POLLING_READING,

        /// <summary>
        /// use select before polling again.
        /// </summary>
        PGRES_POLLING_WRITING,

        PGRES_POLLING_OK,

        /// <summary>
        /// unused; keep for awhile for backwards
        /// compatibility
        /// </summary>
        PGRES_POLLING_ACTIVE,
    }
}