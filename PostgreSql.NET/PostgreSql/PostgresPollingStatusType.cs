namespace PostgreSql
{
    public enum PostgresPollingStatusType
    {
        PGRES_POLLING_FAILED = 0,
        PGRES_POLLING_READING,
        PGRES_POLLING_WRITING,
        PGRES_POLLING_OK,
        PGRES_POLLING_ACTIVE,
    }
}
