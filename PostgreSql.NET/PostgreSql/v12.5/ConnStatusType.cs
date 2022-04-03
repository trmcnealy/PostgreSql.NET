namespace PostgreSql
{
    public enum ConnStatusType : uint
    {
        CONNECTION_OK,
            
        CONNECTION_BAD,
            
        /// <summary>
        /// Waiting for connection to be made.
        /// </summary>
        CONNECTION_STARTED,
            
        /// <summary>
        /// Connection OK; waiting to send.
        /// </summary>
        CONNECTION_MADE,
            
        /// <summary>
        /// Waiting for a response from the
        /// postmaster.
        /// </summary>
        CONNECTION_AWAITING_RESPONSE,
            
        /// <summary>
        /// Received authentication; waiting for
        /// backend startup.
        /// </summary>
        CONNECTION_AUTH_OK,
            
        /// <summary>
        /// Negotiating environment.
        /// </summary>
        CONNECTION_SETENV,
            
        /// <summary>
        /// Negotiating SSL.
        /// </summary>
        CONNECTION_SSL_STARTUP,
            
        /// <summary>
        /// Internal state: connect() needed
        /// </summary>
        CONNECTION_NEEDED,
            
        /// <summary>
        /// Check if we could make a writable
        /// connection.
        /// </summary>
        CONNECTION_CHECK_WRITABLE,
            
        /// <summary>
        /// Wait for any pending message and consume
        /// them.
        /// </summary>
        CONNECTION_CONSUME,
            
        /// <summary>
        /// Negotiating GSSAPI.
        /// </summary>
        CONNECTION_GSS_STARTUP,
    }
}