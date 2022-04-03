namespace PostgreSql
{
    public enum ExecStatusType : uint
    {
        /// <summary>
        /// empty query string was executed
        /// </summary>
        PGRES_EMPTY_QUERY = 0,

        /// <summary>
        /// a query command that doesn't return
        /// anything was executed properly by the
        /// backend
        /// </summary>
        PGRES_COMMAND_OK,

        /// <summary>
        /// a query command that returns tuples was
        /// executed properly by the backend, pg_result
        /// contains the result tuples
        /// </summary>
        PGRES_TUPLES_OK,

        /// <summary>
        /// Copy Out data transfer in progress
        /// </summary>
        PGRES_COPY_OUT,

        /// <summary>
        /// Copy In data transfer in progress
        /// </summary>
        PGRES_COPY_IN,

        /// <summary>
        /// an unexpected response was recv'd from the
        /// backend
        /// </summary>
        PGRES_BAD_RESPONSE,

        /// <summary>
        /// notice or warning message
        /// </summary>
        PGRES_NONFATAL_ERROR,

        /// <summary>
        /// query failed
        /// </summary>
        PGRES_FATAL_ERROR,

        /// <summary>
        /// Copy In/Out data transfer in progress
        /// </summary>
        PGRES_COPY_BOTH,

        /// <summary>
        /// single tuple from larger resultset
        /// </summary>
        PGRES_SINGLE_TUPLE,
    }
}