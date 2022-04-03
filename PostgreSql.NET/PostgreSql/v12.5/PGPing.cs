namespace PostgreSql
{
    /// <summary>
    /// PGPing - The ordering of this enum should not be altered because the
    /// values are exposed externally via pg_isready.
    /// </summary>
    public enum PGPing : uint
    {
        /// <summary>
        /// server is accepting connections
        /// </summary>
        PQPING_OK,

        /// <summary>
        /// server is alive but rejecting connections
        /// </summary>
        PQPING_REJECT,

        /// <summary>
        /// could not establish connection
        /// </summary>
        PQPING_NO_RESPONSE,

        /// <summary>
        /// connection not attempted (bad params)
        /// </summary>
        PQPING_NO_ATTEMPT,
    }
}