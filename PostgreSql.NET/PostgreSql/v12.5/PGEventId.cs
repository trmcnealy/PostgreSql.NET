namespace PostgreSql
{
    /// <summary>
    /// Callback Event Ids
    /// </summary>
    public enum PGEventId : uint
    {
        PGEVT_REGISTER,
            
        PGEVT_CONNRESET,
            
        PGEVT_CONNDESTROY,
            
        PGEVT_RESULTCREATE,
            
        PGEVT_RESULTCOPY,
            
        PGEVT_RESULTDESTROY,
    }
}