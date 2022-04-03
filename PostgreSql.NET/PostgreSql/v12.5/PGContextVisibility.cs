namespace PostgreSql
{
    public enum PGContextVisibility : uint
    {
        /// <summary>
        /// never show CONTEXT field
        /// </summary>
        PQSHOW_CONTEXT_NEVER,

        /// <summary>
        /// show CONTEXT for errors only (default)
        /// </summary>
        PQSHOW_CONTEXT_ERRORS,

        /// <summary>
        /// always show CONTEXT field
        /// </summary>
        PQSHOW_CONTEXT_ALWAYS,
    }
}