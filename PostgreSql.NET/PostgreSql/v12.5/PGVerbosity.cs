namespace PostgreSql
{
    public enum PGVerbosity : uint
    {
        /// <summary>
        /// single-line error messages
        /// </summary>
        PQERRORS_TERSE,

        /// <summary>
        /// recommended style
        /// </summary>
        PQERRORS_DEFAULT,

        /// <summary>
        /// all the facts, ma'am
        /// </summary>
        PQERRORS_VERBOSE,

        /// <summary>
        /// only error severity and SQLSTATE code
        /// </summary>
        PQERRORS_SQLSTATE,
    }
}