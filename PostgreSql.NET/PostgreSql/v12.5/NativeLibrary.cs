using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace PostgreSql
{
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PQnoticeReceiver(nint    arg,
                                          pg_result res);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PQnoticeProcessor(nint arg,
                                           nint message);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void pgthreadlock_t(int acquire);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int PGEventProc(PGEventId evtId,
                                    nint    evtInfo,
                                    nint    passThrough);

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void pqsigfunc(int signo);

    /// <summary>
    /// postgresql://[user[:password]@][host][:port][/dbname][?param1=value1&...]
    /// </summary>
    public static partial class NativeLibrary
    {
        //SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining)
        //static NativeLibrary()
        //{
        //    string postgreSQL_RootPath = Environment.GetEnvironmentVariable("POSTGRESQL");

        //    if(!string.IsNullOrEmpty(postgreSQL_RootPath))
        //    {
        //        Program.AddDllDirectory(Path.Combine(postgreSQL_RootPath, "bin"));

        //        //string PATH = Environment.GetEnvironmentVariable("PATH");

        //        //Environment.SetEnvironmentVariable("PATH", Path.Combine(postgreSQL_RootPath, "bin") + ";" + PATH);
        //    }
        //}
        
        public const  string LibraryName = "libpq";
        public static readonly string LibraryPath;

        public static nint Handle;

        public static volatile bool IsLoaded;

        //public const string libsslLibraryName = "libssl-1_1-x64";

        //public static readonly nint libsslHandle;

        //public const string libcryptoLibraryName = "libcrypto-1_1-x64";

        //public static readonly nint libcryptoHandle;

        //public const string libintlLibraryName = "libintl-8";

        //public static readonly nint libintlHandle;

        //public const string libiconvLibraryName = "libiconv-2";

        //public static readonly nint libiconvHandle;

        public static readonly Dictionary<uint, OidKind> OidTypes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static NativeLibrary()
        {
            string operatingSystem      = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win" : "linux";
            string platformArchitecture = RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "x64" : "x86";

            string libraryPath = GetLibraryPath() ?? throw new NullReferenceException("typeof(MultiPorosityLibrary).Assembly.Location is empty.");

#if DEBUG
            Console.WriteLine("libraryPath: " + libraryPath);
#endif

            LibraryPath = Path.Combine(libraryPath, $"runtimes\\{operatingSystem}-{platformArchitecture}\\native");

            OidTypes = new Dictionary<uint, OidKind>();
        }


        [NativeTypeName("#define LIBPQ_HAS_PIPELINING 1")]
        public const int LIBPQ_HAS_PIPELINING = 1;

        [NativeTypeName("#define LIBPQ_HAS_TRACE_FLAGS 1")]
        public const int LIBPQ_HAS_TRACE_FLAGS = 1;

        [NativeTypeName("#define LIBPQ_HAS_SSL_LIBRARY_DETECTION 1")]
        public const int LIBPQ_HAS_SSL_LIBRARY_DETECTION = 1;

        [NativeTypeName("#define PG_COPYRES_ATTRS 0x01")]
        public const int PG_COPYRES_ATTRS = 0x01;

        [NativeTypeName("#define PG_COPYRES_TUPLES 0x02")]
        public const int PG_COPYRES_TUPLES = 0x02;

        [NativeTypeName("#define PG_COPYRES_EVENTS 0x04")]
        public const int PG_COPYRES_EVENTS = 0x04;

        [NativeTypeName("#define PG_COPYRES_NOTICEHOOKS 0x08")]
        public const int PG_COPYRES_NOTICEHOOKS = 0x08;

        [NativeTypeName("#define PQTRACE_SUPPRESS_TIMESTAMPS (1<<0)")]
        public const int PQTRACE_SUPPRESS_TIMESTAMPS = (1 << 0);

        [NativeTypeName("#define PQTRACE_REGRESS_MODE (1<<1)")]
        public const int PQTRACE_REGRESS_MODE = (1 << 1);

        [NativeTypeName("#define PQ_QUERY_PARAM_MAX_LIMIT 65535")]
        public const int PQ_QUERY_PARAM_MAX_LIMIT = 65535;

        [NativeTypeName("#define PQnoPasswordSupplied \"fe_sendauth: no password supplied\\n\"")]
        public static ReadOnlySpan<byte> PQnoPasswordSupplied => new byte[] { 0x66, 0x65, 0x5F, 0x73, 0x65, 0x6E, 0x64, 0x61, 0x75, 0x74, 0x68, 0x3A, 0x20, 0x6E, 0x6F, 0x20, 0x70, 0x61, 0x73, 0x73, 0x77, 0x6F, 0x72, 0x64, 0x20, 0x73, 0x75, 0x70, 0x70, 0x6C, 0x69, 0x65, 0x64, 0x0A, 0x00 };





        public const int PQbackendPIDRVA = 0x000090C0;
        public const int PQbinaryTuplesRVA = 0x0000CD80;
        public const int PQcancelRVA = 0x000065A0;
        public const int PQclearRVA = 0x0000AEE0;
        public const int PQclientEncodingRVA = 0x00009170;
        public const int PQcmdStatusRVA = 0x0000D210;
        public const int PQcmdTuplesRVA = 0x0000D3D0;
        public const int PQconndefaultsRVA = 0x00006400;
        public const int PQconnectPollRVA = 0x00006710;
        public const int PQconnectStartRVA = 0x000085D0;
        public const int PQconnectStartParamsRVA = 0x00007F50;
        public const int PQconnectdbRVA = 0x000086C0;
        public const int PQconnectdbParamsRVA = 0x00008540;
        public const int PQconnectionNeedsPasswordRVA = 0x000090E0;
        public const int PQconnectionUsedPasswordRVA = 0x00009150;
        public const int PQconninfoRVA = 0x00008CA0;
        public const int PQconninfoFreeRVA = 0x00008D60;
        public const int PQconninfoParseRVA = 0x00008BF0;
        public const int PQconsumeInputRVA = 0x0000C390;
        public const int PQcopyResultRVA = 0x0000BA20;
        public const int PQdbRVA = 0x00008DC0;
        public const int PQdescribePortalRVA = 0x0000C720;
        public const int PQdescribePreparedRVA = 0x0000C6D0;
        public const int PQdisplayTuplesRVA = 0x0000FDB0;
        public const int PQdsplenRVA = 0x0000EDF0;
        public const int PQencryptPasswordRVA = 0x000021F0;
        public const int PQencryptPasswordConnRVA = 0x00002250;
        public const int PQendcopyRVA = 0x0000CAC0;
        public const int PQenv2encodingRVA = 0x0000EE00;
        public const int PQerrorMessageRVA = 0x00009080;
        public const int PQescapeByteaRVA = 0x0000D940;
        public const int PQescapeByteaConnRVA = 0x0000D900;
        public const int PQescapeIdentifierRVA = 0x0000D8F0;
        public const int PQescapeLiteralRVA = 0x0000D8E0;
        public const int PQescapeStringRVA = 0x0000D8A0;
        public const int PQescapeStringConnRVA = 0x0000D850;
        public const int PQexecRVA = 0x0000C490;
        public const int PQexecParamsRVA = 0x0000C4E0;
        public const int PQexecPreparedRVA = 0x0000C630;
        public const int PQfformatRVA = 0x0000D090;
        public const int PQfinishRVA = 0x00006490;
        public const int PQfireResultCreateEventsRVA = 0x00016A50;
        public const int PQflushRVA = 0x0000D820;
        public const int PQfmodRVA = 0x0000D1B0;
        public const int PQfnRVA = 0x0000CAF0;
        public const int PQfnameRVA = 0x0000CDA0;
        public const int PQfnumberRVA = 0x0000CE00;
        public const int PQfreeCancelRVA = 0x00006580;
        public const int PQfreeNotifyRVA = 0x0000D840;
        public const int PQfreememRVA = 0x0000D830;
        public const int PQfsizeRVA = 0x0000D150;
        public const int PQftableRVA = 0x0000CFD0;
        public const int PQftablecolRVA = 0x0000D030;
        public const int PQftypeRVA = 0x0000D0F0;
        public const int PQgetCancelRVA = 0x000064C0;
        public const int PQgetCopyDataRVA = 0x0000C900;
        public const int PQgetResultRVA = 0x0000C430;
        public const int PQgetgssctxRVA = 0x000166E0;
        public const int PQgetisnullRVA = 0x0000D690;
        public const int PQgetlengthRVA = 0x0000D5F0;
        public const int PQgetlineRVA = 0x0000C960;
        public const int PQgetlineAsyncRVA = 0x0000C9A0;
        public const int PQgetsslRVA = 0x000166A0;
        public const int PQgetvalueRVA = 0x0000D560;
        public const int PQgssEncInUseRVA = 0x000166F0;
        public const int PQhostRVA = 0x00008E50;
        public const int PQhostaddrRVA = 0x00008EC0;
        public const int PQinitOpenSSLRVA = 0x00016430;
        public const int PQinitSSLRVA = 0x00016420;
        public const int PQinstanceDataRVA = 0x00016950;
        public const int PQisBusyRVA = 0x0000C3E0;
        public const int PQisnonblockingRVA = 0x0000D800;
        public const int PQisthreadsafeRVA = 0x0000D810;
        public const int PQlibVersionRVA = 0x0000DDD0;
        public const int PQmakeEmptyPGresultRVA = 0x0000AC70;
        public const int PQmblenRVA = 0x0000EDE0;
        public const int PQnfieldsRVA = 0x0000CD60;
        public const int PQnotifiesRVA = 0x0000C790;
        public const int PQnparamsRVA = 0x0000D730;
        public const int PQntuplesRVA = 0x0000CD40;
        public const int PQoidStatusRVA = 0x0000D230;
        public const int PQoidValueRVA = 0x0000D350;
        public const int PQoptionsRVA = 0x00008F60;
        public const int PQparameterStatusRVA = 0x00008FD0;
        public const int PQparamtypeRVA = 0x0000D750;
        public const int PQpassRVA = 0x00008E00;
        public const int PQpingRVA = 0x00008710;
        public const int PQpingParamsRVA = 0x00008590;
        public const int PQportRVA = 0x00008F00;
        public const int PQprepareRVA = 0x0000C590;
        public const int PQprintRVA = 0x0000EE50;
        public const int PQprintTuplesRVA = 0x00010180;
        public const int PQprotocolVersionRVA = 0x00009040;
        public const int PQputCopyDataRVA = 0x0000C810;
        public const int PQputCopyEndRVA = 0x0000C8C0;
        public const int PQputlineRVA = 0x0000C9D0;
        public const int PQputnbytesRVA = 0x0000CA10;
        public const int PQregisterEventProcRVA = 0x00016710;
        public const int PQregisterThreadLockRVA = 0x00009420;
        public const int PQrequestCancelRVA = 0x000065F0;
        public const int PQresStatusRVA = 0x0000CC10;
        public const int PQresetRVA = 0x00007E50;
        public const int PQresetPollRVA = 0x00008B30;
        public const int PQresetStartRVA = 0x00007F10;
        public const int PQresultAllocRVA = 0x0000AB20;
        public const int PQresultErrorFieldRVA = 0x0000CD00;
        public const int PQresultErrorMessageRVA = 0x0000CC30;
        public const int PQresultInstanceDataRVA = 0x00016A00;
        public const int PQresultMemorySizeRVA = 0x0000AB80;
        public const int PQresultSetInstanceDataRVA = 0x000169A0;
        public const int PQresultStatusRVA = 0x0000CBF0;
        public const int PQresultVerboseErrorMessageRVA = 0x0000CC60;
        public const int PQsendDescribePortalRVA = 0x0000C780;
        public const int PQsendDescribePreparedRVA = 0x0000C770;
        public const int PQsendPrepareRVA = 0x0000C210;
        public const int PQsendQueryRVA = 0x0000C060;
        public const int PQsendQueryParamsRVA = 0x0000C130;
        public const int PQsendQueryPreparedRVA = 0x0000C290;
        public const int PQserverVersionRVA = 0x00009060;
        public const int PQsetClientEncodingRVA = 0x000091A0;
        public const int PQsetErrorContextVisibilityRVA = 0x000092D0;
        public const int PQsetErrorVerbosityRVA = 0x000092B0;
        public const int PQsetInstanceDataRVA = 0x000168F0;
        public const int PQsetNoticeProcessorRVA = 0x00009380;
        public const int PQsetNoticeReceiverRVA = 0x00009350;
        public const int PQsetResultAttrsRVA = 0x0000AAF0;
        public const int PQsetSingleRowModeRVA = 0x0000C350;
        public const int PQsetdbLoginRVA = 0x00008750;
        public const int PQsetnonblockingRVA = 0x0000D7B0;
        public const int PQsetvalueRVA = 0x0000B820;
        public const int PQsocketRVA = 0x000090A0;
        public const int PQsslAttributeRVA = 0x000166C0;
        public const int PQsslAttributeNamesRVA = 0x000166D0;
        public const int PQsslInUseRVA = 0x00016400;
        public const int PQsslStructRVA = 0x000166B0;
        public const int PQstatusRVA = 0x00008F80;
        public const int PQtraceRVA = 0x000092F0;
        public const int PQtransactionStatusRVA = 0x00008FA0;
        public const int PQttyRVA = 0x00008F40;
        public const int PQunescapeByteaRVA = 0x0000D970;
        public const int PQuntraceRVA = 0x00009320;
        public const int PQuserRVA = 0x00008DE0;
        public const int appendBinaryPQExpBufferRVA = 0x000163B0;
        public const int appendPQExpBufferRVA = 0x000162A0;
        public const int appendPQExpBufferCharRVA = 0x00016370;
        public const int appendPQExpBufferStrRVA = 0x00016310;
        public const int createPQExpBufferRVA = 0x00015E90;
        public const int destroyPQExpBufferRVA = 0x00015F50;
        public const int enlargePQExpBufferRVA = 0x00016050;
        public const int initPQExpBufferRVA = 0x00015F00;
        public const int lo_closeRVA = 0x00010970;
        public const int lo_creatRVA = 0x00011110;
        public const int lo_createRVA = 0x000111E0;
        public const int lo_exportRVA = 0x00011750;
        public const int lo_importRVA = 0x00011730;
        public const int lo_import_with_oidRVA = 0x00011740;
        public const int lo_lseekRVA = 0x00010EB0;
        public const int lo_lseek64RVA = 0x00010FA0;
        public const int lo_openRVA = 0x000108A0;
        public const int lo_readRVA = 0x00010C90;
        public const int lo_tellRVA = 0x000114A0;
        public const int lo_tell64RVA = 0x00011560;
        public const int lo_truncateRVA = 0x00010A30;
        public const int lo_truncate64RVA = 0x00010B50;
        public const int lo_unlinkRVA = 0x00011670;
        public const int lo_writeRVA = 0x00010DA0;
        public const int pg_char_to_encodingRVA = 0x00016B00;
        public const int pg_encoding_to_charRVA = 0x00016C70;
        public const int pg_utf_mblenRVA = 0x000172A0;
        public const int pg_valid_server_encodingRVA = 0x00016C50;
        public const int pg_valid_server_encoding_idRVA = 0x00016AF0;
        //public const int pgresStatusRVA = 0x00031900;
        public const int pqsignalRVA = 0x00016700;
        public const int printfPQExpBufferRVA = 0x000161E0;
        public const int resetPQExpBufferRVA = 0x00015FD0;
        public const int termPQExpBufferRVA = 0x00015F90;

        internal static unsafe void Load()
        {
            if(!IsLoaded)
            {
                Handle = PlatformApi.NativeLibrary.LoadByName(LibraryName, LibraryPath);

                appendBinaryPQExpBuffer = (delegate*<ref PQExpBufferData, sbyte*, ulong, void>)(Handle + appendBinaryPQExpBufferRVA);
                appendPQExpBuffer       = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + appendPQExpBufferRVA);
                appendPQExpBufferChar   = (delegate*<ref PQExpBufferData, sbyte, void>)(Handle + appendPQExpBufferCharRVA);
                appendPQExpBufferStr    = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + appendPQExpBufferStrRVA);
                createPQExpBuffer       = (delegate*<ref PQExpBufferData>)(Handle + createPQExpBufferRVA);
                destroyPQExpBuffer      = (delegate*<ref PQExpBufferData, void>)(Handle + destroyPQExpBufferRVA);
                enlargePQExpBuffer      = (delegate*<ref PQExpBufferData, ulong, int>)(Handle + enlargePQExpBufferRVA);
                initPQExpBuffer         = (delegate*<ref PQExpBufferData, void>)(Handle + initPQExpBufferRVA);
                printfPQExpBuffer       = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + printfPQExpBufferRVA);
                resetPQExpBuffer        = (delegate*<ref PQExpBufferData, void>)(Handle + resetPQExpBufferRVA);
                termPQExpBuffer         = (delegate*<ref PQExpBufferData, void>)(Handle + termPQExpBufferRVA);

                lo_close           = (delegate*<pg_conn, int, int>)(Handle + lo_closeRVA);
                lo_creat           = (delegate*<pg_conn, int, uint>)(Handle + lo_creatRVA);
                lo_create          = (delegate*<pg_conn, uint, uint>)(Handle + lo_createRVA);
                lo_export          = (delegate*<pg_conn, uint, sbyte*, int>)(Handle + lo_exportRVA);
                lo_import          = (delegate*<pg_conn, sbyte*, uint>)(Handle + lo_importRVA);
                lo_import_with_oid = (delegate*<pg_conn, sbyte*, uint, uint>)(Handle + lo_import_with_oidRVA);
                lo_lseek           = (delegate*<pg_conn, int, int, int, int>)(Handle + lo_lseekRVA);
                lo_lseek64         = (delegate*<pg_conn, int, long, int, long>)(Handle + lo_lseek64RVA);
                lo_open            = (delegate*<pg_conn, uint, int, int>)(Handle + lo_openRVA);
                lo_read            = (delegate*<pg_conn, int, sbyte*, ulong, int>)(Handle + lo_readRVA);
                lo_tell            = (delegate*<pg_conn, int, int>)(Handle + lo_tellRVA);
                lo_tell64          = (delegate*<pg_conn, int, long>)(Handle + lo_tell64RVA);
                lo_truncate        = (delegate*<pg_conn, int, ulong, int>)(Handle + lo_truncateRVA);
                lo_truncate64      = (delegate*<pg_conn, int, long, int>)(Handle + lo_truncate64RVA);
                lo_unlink          = (delegate*<pg_conn, uint, int>)(Handle + lo_unlinkRVA);
                lo_write           = (delegate*<pg_conn, int, sbyte*, ulong, int>)(Handle + lo_writeRVA);

                PQbackendPID              = (delegate*<pg_conn, int>)(Handle                                                    + PQbackendPIDRVA);
                PQbinaryTuples            = (delegate*<pg_result, int>)(Handle                                                  + PQbinaryTuplesRVA);
                PQcancel                  = (delegate*<pg_cancel, sbyte*, int, int>)(Handle                                     + PQcancelRVA);
                PQclear                   = (delegate*<pg_result, void>)(Handle                                                 + PQclearRVA);
                PQclientEncoding          = (delegate*<pg_conn, int>)(Handle                                                    + PQclientEncodingRVA);
                PQcmdStatus               = (delegate*<pg_result, sbyte*>)(Handle                                               + PQcmdStatusRVA);
                PQcmdTuples               = (delegate*<pg_result, sbyte*>)(Handle                                               + PQcmdTuplesRVA);
                PQconndefaults            = (delegate*<ref PQconninfoOption>)(Handle                                            + PQconndefaultsRVA);
                PQconnectdb               = (delegate*<sbyte*, pg_conn>)(Handle                                                 + PQconnectdbRVA);
                PQconnectdbParams         = (delegate*<sbyte*[], sbyte*[], int, pg_conn>)(Handle                                + PQconnectdbParamsRVA);
                PQconnectionNeedsPassword = (delegate*<pg_conn, int>)(Handle                                                    + PQconnectionNeedsPasswordRVA);
                PQconnectionUsedPassword  = (delegate*<pg_conn, int>)(Handle                                                    + PQconnectionUsedPasswordRVA);
                PQconnectPoll             = (delegate*<pg_conn, PostgresPollingStatusType>)(Handle                              + PQconnectPollRVA);
                PQconnectStart            = (delegate*<sbyte*, pg_conn>)(Handle                                                 + PQconnectStartRVA);
                PQconnectStartParams      = (delegate*<sbyte*[], sbyte*[], int, pg_conn>)(Handle                                + PQconnectStartParamsRVA);
                PQconninfo                = (delegate*<pg_conn, ref PQconninfoOption>)(Handle                                   + PQconninfoRVA);
                PQconninfoFree            = (delegate*<ref PQconninfoOption, void>)(Handle                                      + PQconninfoFreeRVA);
                PQconninfoParse           = (delegate*<sbyte*, out sbyte*, ref PQconninfoOption>)(Handle                        + PQconninfoParseRVA);
                PQconsumeInput            = (delegate*<pg_conn, int>)(Handle                                                    + PQconsumeInputRVA);
                PQcopyResult              = (delegate*<pg_result, int, pg_result>)(Handle                                       + PQcopyResultRVA);
                PQdb                      = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQdbRVA);
                PQdescribePortal          = (delegate*<pg_conn, sbyte*, pg_result>)(Handle                                      + PQdescribePortalRVA);
                PQdescribePrepared        = (delegate*<pg_conn, sbyte*, pg_result>)(Handle                                      + PQdescribePreparedRVA);
                PQdisplayTuples           = (delegate*<pg_result, ref _iobuf, int, sbyte*, int, int, void>)(Handle              + PQdisplayTuplesRVA);
                PQdsplen                  = (delegate*<sbyte*, int, int>)(Handle                                                + PQdsplenRVA);
                PQencryptPassword         = (delegate*<sbyte*, sbyte*, sbyte*>)(Handle                                          + PQencryptPasswordRVA);
                PQencryptPasswordConn     = (delegate*<pg_conn, sbyte*, sbyte*, sbyte*, sbyte*>)(Handle                         + PQencryptPasswordConnRVA);
                PQendcopy                 = (delegate*<pg_conn, int>)(Handle                                                    + PQendcopyRVA);
                PQenv2encoding            = (delegate*<int>)(Handle                                                             + PQenv2encodingRVA);
                PQerrorMessage            = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQerrorMessageRVA);
                PQescapeBytea             = (delegate*<sbyte*, ulong, ulong*, sbyte*>)(Handle                                   + PQescapeByteaRVA);
                PQescapeByteaConn         = (delegate*<pg_conn, sbyte*, ulong, ulong*, sbyte*>)(Handle                          + PQescapeByteaConnRVA);
                PQescapeIdentifier        = (delegate*<pg_conn, sbyte*, ulong, sbyte*>)(Handle                                  + PQescapeIdentifierRVA);
                PQescapeLiteral           = (delegate*<pg_conn, sbyte*, ulong, sbyte*>)(Handle                                  + PQescapeLiteralRVA);
                PQescapeString            = (delegate*<sbyte*, sbyte*, ulong, ulong>)(Handle                                    + PQescapeStringRVA);
                PQescapeStringConn        = (delegate*<pg_conn, sbyte*, sbyte*, ulong, int*, ulong>)(Handle                     + PQescapeStringConnRVA);
                PQexec                    = (delegate*<pg_conn, sbyte*, pg_result>)(Handle                                      + PQexecRVA);
                PQexecParams              = (delegate*<pg_conn, sbyte*, int, Oid*, sbyte**, int*, int*, int, pg_result>)(Handle + PQexecParamsRVA);
                PQexecPrepared            = (delegate*<pg_conn, sbyte*, int, sbyte*[], int*, int*, int, pg_result>)(Handle      + PQexecPreparedRVA);
                PQfformat                 = (delegate*<pg_result, int, int>)(Handle                                             + PQfformatRVA);
                PQfinish                  = (delegate*<pg_conn, void>)(Handle                                                   + PQfinishRVA);
                PQfireResultCreateEvents  = (delegate*<pg_conn, pg_result, int>)(Handle                                         + PQfireResultCreateEventsRVA);
                PQflush                   = (delegate*<pg_conn, int>)(Handle                                                    + PQflushRVA);
                PQfmod                    = (delegate*<pg_result, int, int>)(Handle                                             + PQfmodRVA);
                PQfn                      = (delegate*<pg_conn, int, int*, int*, int, in PQArgBlock, int, pg_result>)(Handle    + PQfnRVA);
                PQfname                   = (delegate*<pg_result, int, sbyte*>)(Handle                                          + PQfnameRVA);
                PQfnumber                 = (delegate*<pg_result, sbyte*, int>)(Handle                                          + PQfnumberRVA);
                PQfreeCancel              = (delegate*<pg_cancel, void>)(Handle                                                 + PQfreeCancelRVA);
                PQfreeNotify              = (delegate*<ref PGnotify, void>)(Handle                                              + PQfreeNotifyRVA);
                PQfreemem                 = (delegate*<sbyte*, void>)(Handle                                                    + PQfreememRVA);
                PQfsize                   = (delegate*<pg_result, int, int>)(Handle                                             + PQfsizeRVA);
                PQftable                  = (delegate*<pg_result, int, uint>)(Handle                                            + PQftableRVA);
                PQftablecol               = (delegate*<pg_result, int, int>)(Handle                                             + PQftablecolRVA);
                PQftype                   = (delegate*<pg_result, int, uint>)(Handle                                            + PQftypeRVA);
                PQgetCancel               = (delegate*<pg_conn, pg_cancel>)(Handle                                              + PQgetCancelRVA);
                PQgetCopyData             = (delegate*<pg_conn, out sbyte*, int, int>)(Handle                                   + PQgetCopyDataRVA);
                PQgetgssctx               = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQgetgssctxRVA);
                PQgetisnull               = (delegate*<pg_result, int, int, int>)(Handle                                        + PQgetisnullRVA);
                PQgetlength               = (delegate*<pg_result, int, int, int>)(Handle                                        + PQgetlengthRVA);
                PQgetline                 = (delegate*<pg_conn, sbyte*, int, int>)(Handle                                       + PQgetlineRVA);
                PQgetlineAsync            = (delegate*<pg_conn, sbyte*, int, int>)(Handle                                       + PQgetlineAsyncRVA);
                PQgetResult               = (delegate*<pg_conn, pg_result>)(Handle                                              + PQgetResultRVA);
                PQgetssl                  = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQgetsslRVA);
                PQgetvalue                = (delegate*<pg_result, int, int, sbyte*>)(Handle                                     + PQgetvalueRVA);
                PQgssEncInUse             = (delegate*<pg_conn, int>)(Handle                                                    + PQgssEncInUseRVA);
                PQhost                    = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQhostRVA);
                PQhostaddr                = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQhostaddrRVA);
                PQinitOpenSSL             = (delegate*<int, int, void>)(Handle                                                  + PQinitOpenSSLRVA);
                PQinitSSL                 = (delegate*<int, void>)(Handle                                                       + PQinitSSLRVA);
                PQinstanceData            = (delegate*<pg_conn, PGEventProc, sbyte*>)(Handle                                    + PQinstanceDataRVA);
                PQisBusy                  = (delegate*<pg_conn, int>)(Handle                                                    + PQisBusyRVA);
                PQisnonblocking           = (delegate*<pg_conn, int>)(Handle                                                    + PQisnonblockingRVA);
                PQisthreadsafe            = (delegate*<int>)(Handle                                                             + PQisthreadsafeRVA);
                PQlibVersion              = (delegate*<int>)(Handle                                                             + PQlibVersionRVA);
                PQmakeEmptyPGresult       = (delegate*<pg_conn, ExecStatusType, pg_result>)(Handle                              + PQmakeEmptyPGresultRVA);
                PQmblen                   = (delegate*<sbyte*, int, int>)(Handle                                                + PQmblenRVA);
                PQnfields                 = (delegate*<pg_result, int>)(Handle                                                  + PQnfieldsRVA);
                PQnotifies                = (delegate*<pg_conn, ref pgNotify>)(Handle                                           + PQnotifiesRVA);
                PQnparams                 = (delegate*<pg_result, int>)(Handle                                                  + PQnparamsRVA);
                PQntuples                 = (delegate*<pg_result, int>)(Handle                                                  + PQntuplesRVA);
                PQoidStatus               = (delegate*<pg_result, sbyte*>)(Handle                                               + PQoidStatusRVA);
                PQoidValue                = (delegate*<pg_result, uint>)(Handle                                                 + PQoidValueRVA);
                PQoptions                 = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQoptionsRVA);
                PQparameterStatus         = (delegate*<pg_conn, sbyte*, sbyte*>)(Handle                                         + PQparameterStatusRVA);
                PQparamtype               = (delegate*<pg_result, int, uint>)(Handle                                            + PQparamtypeRVA);
                PQpass                    = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQpassRVA);
                PQping                    = (delegate*<sbyte*, PGPing>)(Handle                                                  + PQpingRVA);
                PQpingParams              = (delegate*<sbyte*[], sbyte*[], int, PGPing>)(Handle                                 + PQpingParamsRVA);
                PQport                    = (delegate*<pg_conn, sbyte*>)(Handle                                                 + PQportRVA);
                PQprepare                 = (delegate*<pg_conn, sbyte*, sbyte*, int, uint*, pg_result>)(Handle                  + PQprepareRVA);
                PQprint                   = (delegate*<ref _iobuf, pg_result, ref PQprintOpt, void>)(Handle                     + PQprintRVA);
                PQprintTuples             = (delegate*<pg_result, ref _iobuf, int, int, int, void>)(Handle                      + PQprintTuplesRVA);
                PQprotocolVersion         = (delegate*<pg_conn, int>)(Handle                                                    + PQprotocolVersionRVA);
                PQputCopyData             = (delegate*<pg_conn, sbyte*, int, int>)(Handle                                       + PQputCopyDataRVA);
                PQputCopyEnd              = (delegate*<pg_conn, sbyte*, int>)(Handle                                            + PQputCopyEndRVA);
                PQputline                 = (delegate*<pg_conn, sbyte*, int>)(Handle                                            + PQputlineRVA);
                PQputnbytes               = (delegate*<pg_conn, sbyte*, int, int>)(Handle                                       + PQputnbytesRVA);
                PQregisterEventProc       = (delegate*<pg_conn, PGEventProc, sbyte*, sbyte*, int>)(Handle                       + PQregisterEventProcRVA);
                PQregisterThreadLock      = (delegate*<pgthreadlock_t, pgthreadlock_t>)(Handle                                  + PQregisterThreadLockRVA);
                PQrequestCancel           = (delegate*<pg_conn, int>)(Handle                                                    + PQrequestCancelRVA);
                PQreset                   = (delegate*<pg_conn, void>)(Handle                                                   + PQresetRVA);
                PQresetPoll               = (delegate*<pg_conn, PostgresPollingStatusType>)(Handle                              + PQresetPollRVA);
                PQresetStart              = (delegate*<pg_conn, int>)(Handle                                                    + PQresetStartRVA);
                PQresStatus               = (delegate*<ExecStatusType, sbyte*>)(Handle                                          + PQresStatusRVA);
                PQresultAlloc             = (delegate*<pg_result, ulong, sbyte*>)(Handle                                        + PQresultAllocRVA);
                PQresultErrorField        = (delegate*<pg_result, int, sbyte*>)(Handle                                          + PQresultErrorFieldRVA);
                PQresultErrorMessage      = (delegate*<pg_result, sbyte*>)(Handle                                               + PQresultErrorMessageRVA);
                PQresultInstanceData      = (delegate*<pg_result, PGEventProc, sbyte*>)(Handle                                  + PQresultInstanceDataRVA);
                PQresultMemorySize        = (delegate*<pg_result, ulong>)(Handle                                                + PQresultMemorySizeRVA);
                PQresultSetInstanceData   = (delegate*<pg_result, PGEventProc, sbyte*, int>)(Handle                             + PQresultSetInstanceDataRVA);
                PQresultStatus            = (delegate*<pg_result, ExecStatusType>)(Handle                                       + PQresultStatusRVA);

                PQresultVerboseErrorMessage = (delegate*<pg_result, PGVerbosity, PGContextVisibility, sbyte*>)(Handle + PQresultVerboseErrorMessageRVA);

                PQsendDescribePortal   = (delegate*<pg_conn, sbyte*, int>)(Handle + PQsendDescribePortalRVA);
                PQsendDescribePrepared = (delegate*<pg_conn, sbyte*, int>)(Handle + PQsendDescribePreparedRVA);
                PQsendPrepare          = (delegate*<pg_conn, sbyte*, sbyte*, int, uint*, int>)(Handle + PQsendPrepareRVA);
                PQsendQuery            = (delegate*<pg_conn, sbyte*, int>)(Handle + PQsendQueryRVA);

                PQsendQueryParams = (delegate*<pg_conn, sbyte*, int, uint*, sbyte*[], int*, int*, int, int>)(Handle + PQsendQueryParamsRVA);

                PQsendQueryPrepared = (delegate*<pg_conn, sbyte*, int, sbyte*[], int*, int*, int, int>)(Handle + PQsendQueryPreparedRVA);
                PQserverVersion     = (delegate*<pg_conn, int>)(Handle + PQserverVersionRVA);
                PQsetClientEncoding = (delegate*<pg_conn, sbyte*, int>)(Handle + PQsetClientEncodingRVA);
                PQsetdbLogin        = (delegate*<sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, pg_conn>)(Handle + PQsetdbLoginRVA);

                PQsetErrorContextVisibility = (delegate*<pg_conn, PGContextVisibility, PGContextVisibility>)(Handle + PQsetErrorContextVisibilityRVA);

                PQsetErrorVerbosity  = (delegate*<pg_conn, PGVerbosity, PGVerbosity>)(Handle + PQsetErrorVerbosityRVA);
                PQsetInstanceData    = (delegate*<pg_conn, PGEventProc, sbyte*, int>)(Handle + PQsetInstanceDataRVA);
                PQsetnonblocking     = (delegate*<pg_conn, int, int>)(Handle + PQsetnonblockingRVA);
                PQsetNoticeProcessor = (delegate*<pg_conn, PQnoticeProcessor, sbyte*, PQnoticeProcessor>)(Handle + PQsetNoticeProcessorRVA);
                PQsetNoticeReceiver  = (delegate*<pg_conn, PQnoticeReceiver, sbyte*, PQnoticeReceiver>)(Handle + PQsetNoticeReceiverRVA);
                PQsetResultAttrs     = (delegate*<pg_result, int, ref pgresAttDesc, int>)(Handle + PQsetResultAttrsRVA);
                PQsetSingleRowMode   = (delegate*<pg_conn, int>)(Handle + PQsetSingleRowModeRVA);
                PQsetvalue           = (delegate*<pg_result, int, int, sbyte*, int, int>)(Handle + PQsetvalueRVA);
                PQsocket             = (delegate*<pg_conn, int>)(Handle + PQsocketRVA);
                PQsslAttribute       = (delegate*<pg_conn, sbyte*, sbyte*>)(Handle + PQsslAttributeRVA);
                PQsslAttributeNames  = (delegate*<pg_conn, sbyte*>)(Handle + PQsslAttributeNamesRVA);
                PQsslInUse           = (delegate*<pg_conn, int>)(Handle + PQsslInUseRVA);
                PQsslStruct          = (delegate*<pg_conn, sbyte*, sbyte*>)(Handle + PQsslStructRVA);
                PQstatus             = (delegate*<pg_conn, ConnStatusType>)(Handle + PQstatusRVA);
                PQtrace              = (delegate*<pg_conn, ref _iobuf, void>)(Handle + PQtraceRVA);
                PQtransactionStatus  = (delegate*<pg_conn, PGTransactionStatusType>)(Handle + PQtransactionStatusRVA);
                PQtty                = (delegate*<pg_conn, sbyte*>)(Handle + PQttyRVA);
                PQunescapeBytea      = (delegate*<sbyte*, ulong*, sbyte*>)(Handle + PQunescapeByteaRVA);
                PQuntrace            = (delegate*<pg_conn, void>)(Handle + PQuntraceRVA);
                PQuser               = (delegate*<pg_conn, sbyte*>)(Handle + PQuserRVA);

                pg_char_to_encoding         = (delegate*<sbyte*, int>)(Handle + pg_char_to_encodingRVA);
                pg_encoding_to_char         = (delegate*<int, sbyte*>)(Handle + pg_encoding_to_charRVA);
                pg_utf_mblen                = (delegate*<sbyte*, int>)(Handle + pg_utf_mblenRVA);
                pg_valid_server_encoding    = (delegate*<sbyte*, int>)(Handle + pg_valid_server_encodingRVA);
                pg_valid_server_encoding_id = (delegate*<int, int>)(Handle + pg_valid_server_encoding_idRVA);

                pqsignal    = (delegate*<int, pqsigfunc, pqsigfunc>)(Handle + pqsignalRVA);

                LoadOidTypes();
            }
        }

        internal static void Unload()
        {
            PlatformApi.NativeLibrary.Free(Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string GetNativePackagePath(string nativePackagePath)
        {
            Version lastestVersion = new Version(0, 0, 0, 0);

            Version currentVersion;

            foreach(DirectoryInfo di in new DirectoryInfo(nativePackagePath).GetDirectories())
            {
                currentVersion = new Version(di.Name);

                if(lastestVersion < currentVersion)
                {
                    lastestVersion = currentVersion;
                }
            }

            return Path.Combine(nativePackagePath, lastestVersion.ToString());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static string? GetLibraryPath()
        {
            string fullPath = Assembly.GetExecutingAssembly().Location;

            if(!string.IsNullOrEmpty(fullPath) && !fullPath.Contains(".nuget"))
            {
                int lastIndex = fullPath.LastIndexOf("\\", StringComparison.Ordinal);

                return fullPath.Substring(0, lastIndex);
            }

            string? nugetPackagesEnvironmentVariable = Environment.GetEnvironmentVariable("NUGET_PACKAGES");

            if(!string.IsNullOrEmpty(nugetPackagesEnvironmentVariable))
            {
                string nativePackagePath = Path.Combine(nugetPackagesEnvironmentVariable, "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            //const string dotnetProfileDirectoryName = ".dotnet";
            //const string toolsShimFolderName        = "tools";

            string? userProfile = Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "USERPROFILE" : "HOME");

            if(!string.IsNullOrEmpty(userProfile))
            {
                string nativePackagePath = Path.Combine(userProfile, ".nuget", "packages", "native.MultiPorosity");

                return GetNativePackagePath(nativePackagePath);
            }

            return null;
        }

        private static void LoadOidTypes()
        {
            OidTypes.Add((uint)OidKind.BOOLOID,               OidKind.BOOLOID);
            OidTypes.Add((uint)OidKind.BYTEAOID,              OidKind.BYTEAOID);
            OidTypes.Add((uint)OidKind.CHAROID,               OidKind.CHAROID);
            OidTypes.Add((uint)OidKind.NAMEOID,               OidKind.NAMEOID);
            OidTypes.Add((uint)OidKind.INT8OID,               OidKind.INT8OID);
            OidTypes.Add((uint)OidKind.INT2OID,               OidKind.INT2OID);
            OidTypes.Add((uint)OidKind.INT2VECTOROID,         OidKind.INT2VECTOROID);
            OidTypes.Add((uint)OidKind.INT4OID,               OidKind.INT4OID);
            OidTypes.Add((uint)OidKind.REGPROCOID,            OidKind.REGPROCOID);
            OidTypes.Add((uint)OidKind.TEXTOID,               OidKind.TEXTOID);
            OidTypes.Add((uint)OidKind.OIDOID,                OidKind.OIDOID);
            OidTypes.Add((uint)OidKind.TIDOID,                OidKind.TIDOID);
            OidTypes.Add((uint)OidKind.XIDOID,                OidKind.XIDOID);
            OidTypes.Add((uint)OidKind.CIDOID,                OidKind.CIDOID);
            OidTypes.Add((uint)OidKind.OIDVECTOROID,          OidKind.OIDVECTOROID);
            OidTypes.Add((uint)OidKind.JSONOID,               OidKind.JSONOID);
            OidTypes.Add((uint)OidKind.XMLOID,                OidKind.XMLOID);
            OidTypes.Add((uint)OidKind.PGNODETREEOID,         OidKind.PGNODETREEOID);
            OidTypes.Add((uint)OidKind.PGNDISTINCTOID,        OidKind.PGNDISTINCTOID);
            OidTypes.Add((uint)OidKind.PGDEPENDENCIESOID,     OidKind.PGDEPENDENCIESOID);
            OidTypes.Add((uint)OidKind.PGMCVLISTOID,          OidKind.PGMCVLISTOID);
            OidTypes.Add((uint)OidKind.PGDDLCOMMANDOID,       OidKind.PGDDLCOMMANDOID);
            OidTypes.Add((uint)OidKind.POINTOID,              OidKind.POINTOID);
            OidTypes.Add((uint)OidKind.LSEGOID,               OidKind.LSEGOID);
            OidTypes.Add((uint)OidKind.PATHOID,               OidKind.PATHOID);
            OidTypes.Add((uint)OidKind.BOXOID,                OidKind.BOXOID);
            OidTypes.Add((uint)OidKind.POLYGONOID,            OidKind.POLYGONOID);
            OidTypes.Add((uint)OidKind.LINEOID,               OidKind.LINEOID);
            OidTypes.Add((uint)OidKind.FLOAT4OID,             OidKind.FLOAT4OID);
            OidTypes.Add((uint)OidKind.FLOAT8OID,             OidKind.FLOAT8OID);
            OidTypes.Add((uint)OidKind.UNKNOWNOID,            OidKind.UNKNOWNOID);
            OidTypes.Add((uint)OidKind.CIRCLEOID,             OidKind.CIRCLEOID);
            OidTypes.Add((uint)OidKind.CASHOID,               OidKind.CASHOID);
            OidTypes.Add((uint)OidKind.MACADDROID,            OidKind.MACADDROID);
            OidTypes.Add((uint)OidKind.INETOID,               OidKind.INETOID);
            OidTypes.Add((uint)OidKind.CIDROID,               OidKind.CIDROID);
            OidTypes.Add((uint)OidKind.MACADDR8OID,           OidKind.MACADDR8OID);
            OidTypes.Add((uint)OidKind.ACLITEMOID,            OidKind.ACLITEMOID);
            OidTypes.Add((uint)OidKind.BPCHAROID,             OidKind.BPCHAROID);
            OidTypes.Add((uint)OidKind.VARCHAROID,            OidKind.VARCHAROID);
            OidTypes.Add((uint)OidKind.DATEOID,               OidKind.DATEOID);
            OidTypes.Add((uint)OidKind.TIMEOID,               OidKind.TIMEOID);
            OidTypes.Add((uint)OidKind.TIMESTAMPOID,          OidKind.TIMESTAMPOID);
            OidTypes.Add((uint)OidKind.TIMESTAMPTZOID,        OidKind.TIMESTAMPTZOID);
            OidTypes.Add((uint)OidKind.INTERVALOID,           OidKind.INTERVALOID);
            OidTypes.Add((uint)OidKind.TIMETZOID,             OidKind.TIMETZOID);
            OidTypes.Add((uint)OidKind.BITOID,                OidKind.BITOID);
            OidTypes.Add((uint)OidKind.VARBITOID,             OidKind.VARBITOID);
            OidTypes.Add((uint)OidKind.NUMERICOID,            OidKind.NUMERICOID);
            OidTypes.Add((uint)OidKind.REFCURSOROID,          OidKind.REFCURSOROID);
            OidTypes.Add((uint)OidKind.REGPROCEDUREOID,       OidKind.REGPROCEDUREOID);
            OidTypes.Add((uint)OidKind.REGOPEROID,            OidKind.REGOPEROID);
            OidTypes.Add((uint)OidKind.REGOPERATOROID,        OidKind.REGOPERATOROID);
            OidTypes.Add((uint)OidKind.REGCLASSOID,           OidKind.REGCLASSOID);
            OidTypes.Add((uint)OidKind.REGTYPEOID,            OidKind.REGTYPEOID);
            OidTypes.Add((uint)OidKind.REGROLEOID,            OidKind.REGROLEOID);
            OidTypes.Add((uint)OidKind.REGNAMESPACEOID,       OidKind.REGNAMESPACEOID);
            OidTypes.Add((uint)OidKind.UUIDOID,               OidKind.UUIDOID);
            OidTypes.Add((uint)OidKind.LSNOID,                OidKind.LSNOID);
            OidTypes.Add((uint)OidKind.TSVECTOROID,           OidKind.TSVECTOROID);
            OidTypes.Add((uint)OidKind.GTSVECTOROID,          OidKind.GTSVECTOROID);
            OidTypes.Add((uint)OidKind.TSQUERYOID,            OidKind.TSQUERYOID);
            OidTypes.Add((uint)OidKind.REGCONFIGOID,          OidKind.REGCONFIGOID);
            OidTypes.Add((uint)OidKind.REGDICTIONARYOID,      OidKind.REGDICTIONARYOID);
            OidTypes.Add((uint)OidKind.JSONBOID,              OidKind.JSONBOID);
            OidTypes.Add((uint)OidKind.JSONPATHOID,           OidKind.JSONPATHOID);
            OidTypes.Add((uint)OidKind.TXID_SNAPSHOTOID,      OidKind.TXID_SNAPSHOTOID);
            OidTypes.Add((uint)OidKind.INT4RANGEOID,          OidKind.INT4RANGEOID);
            OidTypes.Add((uint)OidKind.NUMRANGEOID,           OidKind.NUMRANGEOID);
            OidTypes.Add((uint)OidKind.TSRANGEOID,            OidKind.TSRANGEOID);
            OidTypes.Add((uint)OidKind.TSTZRANGEOID,          OidKind.TSTZRANGEOID);
            OidTypes.Add((uint)OidKind.DATERANGEOID,          OidKind.DATERANGEOID);
            OidTypes.Add((uint)OidKind.INT8RANGEOID,          OidKind.INT8RANGEOID);
            OidTypes.Add((uint)OidKind.RECORDOID,             OidKind.RECORDOID);
            OidTypes.Add((uint)OidKind.RECORDARRAYOID,        OidKind.RECORDARRAYOID);
            OidTypes.Add((uint)OidKind.CSTRINGOID,            OidKind.CSTRINGOID);
            OidTypes.Add((uint)OidKind.ANYOID,                OidKind.ANYOID);
            OidTypes.Add((uint)OidKind.ANYARRAYOID,           OidKind.ANYARRAYOID);
            OidTypes.Add((uint)OidKind.VOIDOID,               OidKind.VOIDOID);
            OidTypes.Add((uint)OidKind.TRIGGEROID,            OidKind.TRIGGEROID);
            OidTypes.Add((uint)OidKind.EVTTRIGGEROID,         OidKind.EVTTRIGGEROID);
            OidTypes.Add((uint)OidKind.LANGUAGE_HANDLEROID,   OidKind.LANGUAGE_HANDLEROID);
            OidTypes.Add((uint)OidKind.INTERNALOID,           OidKind.INTERNALOID);
            OidTypes.Add((uint)OidKind.OPAQUEOID,             OidKind.OPAQUEOID);
            OidTypes.Add((uint)OidKind.ANYELEMENTOID,         OidKind.ANYELEMENTOID);
            OidTypes.Add((uint)OidKind.ANYNONARRAYOID,        OidKind.ANYNONARRAYOID);
            OidTypes.Add((uint)OidKind.ANYENUMOID,            OidKind.ANYENUMOID);
            OidTypes.Add((uint)OidKind.FDW_HANDLEROID,        OidKind.FDW_HANDLEROID);
            OidTypes.Add((uint)OidKind.INDEX_AM_HANDLEROID,   OidKind.INDEX_AM_HANDLEROID);
            OidTypes.Add((uint)OidKind.TSM_HANDLEROID,        OidKind.TSM_HANDLEROID);
            OidTypes.Add((uint)OidKind.TABLE_AM_HANDLEROID,   OidKind.TABLE_AM_HANDLEROID);
            OidTypes.Add((uint)OidKind.ANYRANGEOID,           OidKind.ANYRANGEOID);
            OidTypes.Add((uint)OidKind.BOOLARRAYOID,          OidKind.BOOLARRAYOID);
            OidTypes.Add((uint)OidKind.BYTEAARRAYOID,         OidKind.BYTEAARRAYOID);
            OidTypes.Add((uint)OidKind.CHARARRAYOID,          OidKind.CHARARRAYOID);
            OidTypes.Add((uint)OidKind.NAMEARRAYOID,          OidKind.NAMEARRAYOID);
            OidTypes.Add((uint)OidKind.INT8ARRAYOID,          OidKind.INT8ARRAYOID);
            OidTypes.Add((uint)OidKind.INT2ARRAYOID,          OidKind.INT2ARRAYOID);
            OidTypes.Add((uint)OidKind.INT2VECTORARRAYOID,    OidKind.INT2VECTORARRAYOID);
            OidTypes.Add((uint)OidKind.INT4ARRAYOID,          OidKind.INT4ARRAYOID);
            OidTypes.Add((uint)OidKind.REGPROCARRAYOID,       OidKind.REGPROCARRAYOID);
            OidTypes.Add((uint)OidKind.TEXTARRAYOID,          OidKind.TEXTARRAYOID);
            OidTypes.Add((uint)OidKind.OIDARRAYOID,           OidKind.OIDARRAYOID);
            OidTypes.Add((uint)OidKind.TIDARRAYOID,           OidKind.TIDARRAYOID);
            OidTypes.Add((uint)OidKind.XIDARRAYOID,           OidKind.XIDARRAYOID);
            OidTypes.Add((uint)OidKind.CIDARRAYOID,           OidKind.CIDARRAYOID);
            OidTypes.Add((uint)OidKind.OIDVECTORARRAYOID,     OidKind.OIDVECTORARRAYOID);
            OidTypes.Add((uint)OidKind.JSONARRAYOID,          OidKind.JSONARRAYOID);
            OidTypes.Add((uint)OidKind.XMLARRAYOID,           OidKind.XMLARRAYOID);
            OidTypes.Add((uint)OidKind.POINTARRAYOID,         OidKind.POINTARRAYOID);
            OidTypes.Add((uint)OidKind.LSEGARRAYOID,          OidKind.LSEGARRAYOID);
            OidTypes.Add((uint)OidKind.PATHARRAYOID,          OidKind.PATHARRAYOID);
            OidTypes.Add((uint)OidKind.BOXARRAYOID,           OidKind.BOXARRAYOID);
            OidTypes.Add((uint)OidKind.POLYGONARRAYOID,       OidKind.POLYGONARRAYOID);
            OidTypes.Add((uint)OidKind.LINEARRAYOID,          OidKind.LINEARRAYOID);
            OidTypes.Add((uint)OidKind.FLOAT4ARRAYOID,        OidKind.FLOAT4ARRAYOID);
            OidTypes.Add((uint)OidKind.FLOAT8ARRAYOID,        OidKind.FLOAT8ARRAYOID);
            OidTypes.Add((uint)OidKind.CIRCLEARRAYOID,        OidKind.CIRCLEARRAYOID);
            OidTypes.Add((uint)OidKind.MONEYARRAYOID,         OidKind.MONEYARRAYOID);
            OidTypes.Add((uint)OidKind.MACADDRARRAYOID,       OidKind.MACADDRARRAYOID);
            OidTypes.Add((uint)OidKind.INETARRAYOID,          OidKind.INETARRAYOID);
            OidTypes.Add((uint)OidKind.CIDRARRAYOID,          OidKind.CIDRARRAYOID);
            OidTypes.Add((uint)OidKind.MACADDR8ARRAYOID,      OidKind.MACADDR8ARRAYOID);
            OidTypes.Add((uint)OidKind.ACLITEMARRAYOID,       OidKind.ACLITEMARRAYOID);
            OidTypes.Add((uint)OidKind.BPCHARARRAYOID,        OidKind.BPCHARARRAYOID);
            OidTypes.Add((uint)OidKind.VARCHARARRAYOID,       OidKind.VARCHARARRAYOID);
            OidTypes.Add((uint)OidKind.DATEARRAYOID,          OidKind.DATEARRAYOID);
            OidTypes.Add((uint)OidKind.TIMEARRAYOID,          OidKind.TIMEARRAYOID);
            OidTypes.Add((uint)OidKind.TIMESTAMPARRAYOID,     OidKind.TIMESTAMPARRAYOID);
            OidTypes.Add((uint)OidKind.TIMESTAMPTZARRAYOID,   OidKind.TIMESTAMPTZARRAYOID);
            OidTypes.Add((uint)OidKind.INTERVALARRAYOID,      OidKind.INTERVALARRAYOID);
            OidTypes.Add((uint)OidKind.TIMETZARRAYOID,        OidKind.TIMETZARRAYOID);
            OidTypes.Add((uint)OidKind.BITARRAYOID,           OidKind.BITARRAYOID);
            OidTypes.Add((uint)OidKind.VARBITARRAYOID,        OidKind.VARBITARRAYOID);
            OidTypes.Add((uint)OidKind.NUMERICARRAYOID,       OidKind.NUMERICARRAYOID);
            OidTypes.Add((uint)OidKind.REFCURSORARRAYOID,     OidKind.REFCURSORARRAYOID);
            OidTypes.Add((uint)OidKind.REGPROCEDUREARRAYOID,  OidKind.REGPROCEDUREARRAYOID);
            OidTypes.Add((uint)OidKind.REGOPERARRAYOID,       OidKind.REGOPERARRAYOID);
            OidTypes.Add((uint)OidKind.REGOPERATORARRAYOID,   OidKind.REGOPERATORARRAYOID);
            OidTypes.Add((uint)OidKind.REGCLASSARRAYOID,      OidKind.REGCLASSARRAYOID);
            OidTypes.Add((uint)OidKind.REGTYPEARRAYOID,       OidKind.REGTYPEARRAYOID);
            OidTypes.Add((uint)OidKind.REGROLEARRAYOID,       OidKind.REGROLEARRAYOID);
            OidTypes.Add((uint)OidKind.REGNAMESPACEARRAYOID,  OidKind.REGNAMESPACEARRAYOID);
            OidTypes.Add((uint)OidKind.UUIDARRAYOID,          OidKind.UUIDARRAYOID);
            OidTypes.Add((uint)OidKind.PG_LSNARRAYOID,        OidKind.PG_LSNARRAYOID);
            OidTypes.Add((uint)OidKind.TSVECTORARRAYOID,      OidKind.TSVECTORARRAYOID);
            OidTypes.Add((uint)OidKind.GTSVECTORARRAYOID,     OidKind.GTSVECTORARRAYOID);
            OidTypes.Add((uint)OidKind.TSQUERYARRAYOID,       OidKind.TSQUERYARRAYOID);
            OidTypes.Add((uint)OidKind.REGCONFIGARRAYOID,     OidKind.REGCONFIGARRAYOID);
            OidTypes.Add((uint)OidKind.REGDICTIONARYARRAYOID, OidKind.REGDICTIONARYARRAYOID);
            OidTypes.Add((uint)OidKind.JSONBARRAYOID,         OidKind.JSONBARRAYOID);
            OidTypes.Add((uint)OidKind.JSONPATHARRAYOID,      OidKind.JSONPATHARRAYOID);
            OidTypes.Add((uint)OidKind.TXID_SNAPSHOTARRAYOID, OidKind.TXID_SNAPSHOTARRAYOID);
            OidTypes.Add((uint)OidKind.INT4RANGEARRAYOID,     OidKind.INT4RANGEARRAYOID);
            OidTypes.Add((uint)OidKind.NUMRANGEARRAYOID,      OidKind.NUMRANGEARRAYOID);
            OidTypes.Add((uint)OidKind.TSRANGEARRAYOID,       OidKind.TSRANGEARRAYOID);
            OidTypes.Add((uint)OidKind.TSTZRANGEARRAYOID,     OidKind.TSTZRANGEARRAYOID);
            OidTypes.Add((uint)OidKind.DATERANGEARRAYOID,     OidKind.DATERANGEARRAYOID);
            OidTypes.Add((uint)OidKind.INT8RANGEARRAYOID,     OidKind.INT8RANGEARRAYOID);
            OidTypes.Add((uint)OidKind.CSTRINGARRAYOID,       OidKind.CSTRINGARRAYOID);
        }

        public static SQLType GetSqlType(OidKind type)
        {
            switch(type)
            {
                case OidKind.BOOLOID:
                {
                    return SQLType.BOOLEAN;
                }
                case OidKind.CHAROID:
                {
                    return SQLType.CHAR;
                }
                case OidKind.VARCHAROID:
                {
                    return SQLType.VARCHAR;
                }
                case OidKind.BYTEAOID:
                {
                    return SQLType.TINYINT;
                }
                case OidKind.INT2OID:
                {
                    return SQLType.SMALLINT;
                }
                case OidKind.INT4OID:
                {
                    return SQLType.INT;
                }
                case OidKind.INT8OID:
                {
                    return SQLType.BIGINT;
                }
                case OidKind.FLOAT4OID:
                {
                    return SQLType.FLOAT;
                }
                case OidKind.FLOAT8OID:
                {
                    return SQLType.DOUBLE;
                }
                case OidKind.TEXTOID:
                {
                    return SQLType.TEXT;
                }
                case OidKind.TIMEOID:
                {
                    return SQLType.TIME;
                }
                case OidKind.TIMESTAMPOID:
                {
                    return SQLType.TIMESTAMP;
                }
                case OidKind.DATEOID:
                {
                    return SQLType.TEXT;
                }
                case OidKind.VOIDOID:
                {
                    return SQLType.VOID;
                }
                case OidKind.POINTOID:
                {
                    return SQLType.POINT;
                }
                case OidKind.POLYGONOID:
                {
                    return SQLType.POLYGON;
                }
                default:
                {
                    throw new NotSupportedException(type.ToString());
                }
            }
        }

        public static unsafe delegate*<sbyte*, pg_conn>                                                 PQconnectStart;
        public static unsafe delegate*<sbyte*[], sbyte*[], int, pg_conn>                                PQconnectStartParams;
        public static unsafe delegate*<pg_conn, PostgresPollingStatusType>                              PQconnectPoll;
        public static unsafe delegate*<sbyte*, pg_conn>                                                 PQconnectdb;
        public static unsafe delegate*<sbyte*[], sbyte*[], int, pg_conn>                                PQconnectdbParams;
        public static unsafe delegate*<sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, sbyte*, pg_conn> PQsetdbLogin;
        public static unsafe delegate*<pg_conn, void>                                                   PQfinish;
        public static unsafe delegate*<ref PQconninfoOption>                                            PQconndefaults;
        public static unsafe delegate*<sbyte*, out sbyte*, ref PQconninfoOption>                        PQconninfoParse;
        public static unsafe delegate*<pg_conn, ref PQconninfoOption>                                   PQconninfo;
        public static unsafe delegate*<ref PQconninfoOption, void>                                      PQconninfoFree;
        public static unsafe delegate*<pg_conn, int>                                                    PQresetStart;
        public static unsafe delegate*<pg_conn, PostgresPollingStatusType>                              PQresetPoll;
        public static unsafe delegate*<pg_conn, void>                                                   PQreset;
        public static unsafe delegate*<pg_conn, pg_cancel>                                              PQgetCancel;
        public static unsafe delegate*<pg_cancel, void>                                                 PQfreeCancel;
        public static unsafe delegate*<ref PGnotify, void>                                              PQfreeNotify;
        public static unsafe delegate*<pg_cancel, sbyte*, int, int>                                     PQcancel;
        public static unsafe delegate*<pg_conn, int>                                                    PQrequestCancel;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQdb;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQuser;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQpass;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQhost;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQhostaddr;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQport;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQtty;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQoptions;
        public static unsafe delegate*<pg_conn, ConnStatusType>                                         PQstatus;
        public static unsafe delegate*<pg_conn, PGTransactionStatusType>                                PQtransactionStatus;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*>                                         PQparameterStatus;
        public static unsafe delegate*<pg_conn, int>                                                    PQprotocolVersion;
        public static unsafe delegate*<pg_conn, int>                                                    PQserverVersion;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQerrorMessage;
        public static unsafe delegate*<pg_conn, int>                                                    PQsocket;
        public static unsafe delegate*<pg_conn, int>                                                    PQbackendPID;
        public static unsafe delegate*<pg_conn, int>                                                    PQconnectionNeedsPassword;
        public static unsafe delegate*<pg_conn, int>                                                    PQconnectionUsedPassword;
        public static unsafe delegate*<pg_conn, int>                                                    PQclientEncoding;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQsetClientEncoding;
        public static unsafe delegate*<pg_conn, int>                                                    PQsslInUse;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*>                                         PQsslStruct;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*>                                         PQsslAttribute;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQsslAttributeNames;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQgetssl;
        public static unsafe delegate*<int, void>                                                       PQinitSSL;
        public static unsafe delegate*<int, int, void>                                                  PQinitOpenSSL;
        public static unsafe delegate*<pg_conn, int>                                                    PQgssEncInUse;
        public static unsafe delegate*<pg_conn, sbyte*>                                                 PQgetgssctx;
        public static unsafe delegate*<pg_conn, PGVerbosity, PGVerbosity>                               PQsetErrorVerbosity;
        public static unsafe delegate*<pg_conn, PGContextVisibility, PGContextVisibility>               PQsetErrorContextVisibility;
        public static unsafe delegate*<pg_conn, ref _iobuf, void>                                       PQtrace;
        public static unsafe delegate*<pg_conn, void>                                                   PQuntrace;
        public static unsafe delegate*<pg_conn, PQnoticeReceiver, sbyte*, PQnoticeReceiver>             PQsetNoticeReceiver;
        public static unsafe delegate*<pg_conn, PQnoticeProcessor, sbyte*, PQnoticeProcessor>           PQsetNoticeProcessor;
        public static unsafe delegate*<pgthreadlock_t, pgthreadlock_t>                                  PQregisterThreadLock;
        public static unsafe delegate*<pg_conn, sbyte*, pg_result>                                      PQexec;
        public static unsafe delegate*<pg_conn, sbyte*, int, Oid*, sbyte**, int*, int*, int, pg_result> PQexecParams;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*, int, uint*, pg_result>                  PQprepare;
        public static unsafe delegate*<pg_conn, sbyte*, int, sbyte*[], int*, int*, int, pg_result>      PQexecPrepared;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQsendQuery;
        public static unsafe delegate*<pg_conn, sbyte*, int, uint*, sbyte*[], int*, int*, int, int>     PQsendQueryParams;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*, int, uint*, int>                        PQsendPrepare;
        public static unsafe delegate*<pg_conn, sbyte*, int, sbyte*[], int*, int*, int, int>            PQsendQueryPrepared;
        public static unsafe delegate*<pg_conn, int>                                                    PQsetSingleRowMode;
        public static unsafe delegate*<pg_conn, pg_result>                                              PQgetResult;
        public static unsafe delegate*<pg_conn, int>                                                    PQisBusy;
        public static unsafe delegate*<pg_conn, int>                                                    PQconsumeInput;
        public static unsafe delegate*<pg_conn, ref pgNotify>                                           PQnotifies;
        public static unsafe delegate*<pg_conn, sbyte*, int, int>                                       PQputCopyData;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQputCopyEnd;
        public static unsafe delegate*<pg_conn, out sbyte*, int, int>                                   PQgetCopyData;
        public static unsafe delegate*<pg_conn, sbyte*, int, int>                                       PQgetline;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQputline;
        public static unsafe delegate*<pg_conn, sbyte*, int, int>                                       PQgetlineAsync;
        public static unsafe delegate*<pg_conn, sbyte*, int, int>                                       PQputnbytes;
        public static unsafe delegate*<pg_conn, int>                                                    PQendcopy;
        public static unsafe delegate*<pg_conn, int, int>                                               PQsetnonblocking;
        public static unsafe delegate*<pg_conn, int>                                                    PQisnonblocking;
        public static unsafe delegate*<int>                                                             PQisthreadsafe;
        public static unsafe delegate*<sbyte*, PGPing>                                                  PQping;
        public static unsafe delegate*<sbyte*[], sbyte*[], int, PGPing>                                 PQpingParams;
        public static unsafe delegate*<pg_conn, int>                                                    PQflush;
        public static unsafe delegate*<pg_conn, int, int*, int*, int, in PQArgBlock, int, pg_result>    PQfn;
        public static unsafe delegate*<pg_result, ExecStatusType>                                       PQresultStatus;
        public static unsafe delegate*<ExecStatusType, sbyte*>                                          PQresStatus;
        public static unsafe delegate*<pg_result, sbyte*>                                               PQresultErrorMessage;
        public static unsafe delegate*<pg_result, PGVerbosity, PGContextVisibility, sbyte*>             PQresultVerboseErrorMessage;
        public static unsafe delegate*<pg_result, int, sbyte*>                                          PQresultErrorField;
        public static unsafe delegate*<pg_result, int>                                                  PQntuples;
        public static unsafe delegate*<pg_result, int>                                                  PQnfields;
        public static unsafe delegate*<pg_result, int>                                                  PQbinaryTuples;
        public static unsafe delegate*<pg_result, int, sbyte*>                                          PQfname;
        public static unsafe delegate*<pg_result, sbyte*, int>                                          PQfnumber;
        public static unsafe delegate*<pg_result, int, uint>                                            PQftable;
        public static unsafe delegate*<pg_result, int, int>                                             PQftablecol;
        public static unsafe delegate*<pg_result, int, int>                                             PQfformat;
        public static unsafe delegate*<pg_result, int, uint>                                            PQftype;
        public static unsafe delegate*<pg_result, int, int>                                             PQfsize;
        public static unsafe delegate*<pg_result, int, int>                                             PQfmod;
        public static unsafe delegate*<pg_result, sbyte*>                                               PQcmdStatus;
        public static unsafe delegate*<pg_result, sbyte*>                                               PQoidStatus;
        public static unsafe delegate*<pg_result, uint>                                                 PQoidValue;
        public static unsafe delegate*<pg_result, sbyte*>                                               PQcmdTuples;
        public static unsafe delegate*<pg_result, int, int, sbyte*>                                     PQgetvalue;
        public static unsafe delegate*<pg_result, int, int, int>                                        PQgetlength;
        public static unsafe delegate*<pg_result, int, int, int>                                        PQgetisnull;
        public static unsafe delegate*<pg_result, int>                                                  PQnparams;
        public static unsafe delegate*<pg_result, int, uint>                                            PQparamtype;
        public static unsafe delegate*<pg_conn, sbyte*, pg_result>                                      PQdescribePrepared;
        public static unsafe delegate*<pg_conn, sbyte*, pg_result>                                      PQdescribePortal;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQsendDescribePrepared;
        public static unsafe delegate*<pg_conn, sbyte*, int>                                            PQsendDescribePortal;
        public static unsafe delegate*<pg_result, void>                                                 PQclear;
        public static unsafe delegate*<sbyte*, void>                                                    PQfreemem;
        public static unsafe delegate*<pg_conn, ExecStatusType, pg_result>                              PQmakeEmptyPGresult;
        public static unsafe delegate*<pg_result, int, pg_result>                                       PQcopyResult;
        public static unsafe delegate*<pg_result, int, ref pgresAttDesc, int>                           PQsetResultAttrs;
        public static unsafe delegate*<pg_result, ulong, sbyte*>                                        PQresultAlloc;
        public static unsafe delegate*<pg_result, ulong>                                                PQresultMemorySize;
        public static unsafe delegate*<pg_result, int, int, sbyte*, int, int>                           PQsetvalue;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*, ulong, int*, ulong>                     PQescapeStringConn;
        public static unsafe delegate*<pg_conn, sbyte*, ulong, sbyte*>                                  PQescapeLiteral;
        public static unsafe delegate*<pg_conn, sbyte*, ulong, sbyte*>                                  PQescapeIdentifier;
        public static unsafe delegate*<pg_conn, sbyte*, ulong, ulong*, sbyte*>                          PQescapeByteaConn;
        public static unsafe delegate*<sbyte*, ulong*, sbyte*>                                          PQunescapeBytea;
        public static unsafe delegate*<sbyte*, sbyte*, ulong, ulong>                                    PQescapeString;
        public static unsafe delegate*<sbyte*, ulong, ulong*, sbyte*>                                   PQescapeBytea;
        public static unsafe delegate*<ref _iobuf, pg_result, ref PQprintOpt, void>                     PQprint;
        public static unsafe delegate*<pg_result, ref _iobuf, int, sbyte*, int, int, void>              PQdisplayTuples;
        public static unsafe delegate*<pg_result, ref _iobuf, int, int, int, void>                      PQprintTuples;
        public static unsafe delegate*<int>                                                             PQlibVersion;
        public static unsafe delegate*<sbyte*, int, int>                                                PQmblen;
        public static unsafe delegate*<sbyte*, int, int>                                                PQdsplen;
        public static unsafe delegate*<int>                                                             PQenv2encoding;
        public static unsafe delegate*<sbyte*, sbyte*, sbyte*>                                          PQencryptPassword;
        public static unsafe delegate*<pg_conn, sbyte*, sbyte*, sbyte*, sbyte*>                         PQencryptPasswordConn;
        public static unsafe delegate*<pg_conn, PGEventProc, sbyte*, sbyte*, int>                       PQregisterEventProc;
        public static unsafe delegate*<pg_conn, PGEventProc, sbyte*, int>                               PQsetInstanceData;
        public static unsafe delegate*<pg_conn, PGEventProc, sbyte*>                                    PQinstanceData;
        public static unsafe delegate*<pg_result, PGEventProc, sbyte*, int>                             PQresultSetInstanceData;
        public static unsafe delegate*<pg_result, PGEventProc, sbyte*>                                  PQresultInstanceData;
        public static unsafe delegate*<pg_conn, pg_result, int>                                         PQfireResultCreateEvents;

        public static unsafe delegate*<sbyte*, int> pg_char_to_encoding;
        public static unsafe delegate*<int, sbyte*> pg_encoding_to_char;
        public static unsafe delegate*<int, int>    pg_valid_server_encoding_id;
        public static unsafe delegate*<sbyte*, int> pg_utf_mblen;
        public static unsafe delegate*<sbyte*, int> pg_valid_server_encoding;
        
        public static unsafe delegate*<int, pqsigfunc, pqsigfunc> pqsignal;

        public static unsafe delegate*<pg_conn, uint, int, int>          lo_open;
        public static unsafe delegate*<pg_conn, int, int>                lo_close;
        public static unsafe delegate*<pg_conn, int, sbyte*, ulong, int> lo_read;
        public static unsafe delegate*<pg_conn, int, sbyte*, ulong, int> lo_write;
        public static unsafe delegate*<pg_conn, int, int, int, int>      lo_lseek;
        public static unsafe delegate*<pg_conn, int, long, int, long>    lo_lseek64;
        public static unsafe delegate*<pg_conn, int, uint>               lo_creat;
        public static unsafe delegate*<pg_conn, uint, uint>              lo_create;
        public static unsafe delegate*<pg_conn, int, int>                lo_tell;
        public static unsafe delegate*<pg_conn, int, long>               lo_tell64;
        public static unsafe delegate*<pg_conn, int, ulong, int>         lo_truncate;
        public static unsafe delegate*<pg_conn, int, long, int>          lo_truncate64;
        public static unsafe delegate*<pg_conn, uint, int>               lo_unlink;
        public static unsafe delegate*<pg_conn, sbyte*, uint>            lo_import;
        public static unsafe delegate*<pg_conn, sbyte*, uint, uint>      lo_import_with_oid;
        public static unsafe delegate*<pg_conn, uint, sbyte*, int>       lo_export;

        public static unsafe delegate*<ref PQExpBufferData>                      createPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, void>                initPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, void>                destroyPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, void>                termPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, void>                resetPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, ulong, int>          enlargePQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, sbyte*, void>        appendPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, sbyte*, void>        appendPQExpBufferStr;
        public static unsafe delegate*<ref PQExpBufferData, sbyte, void>         appendPQExpBufferChar;
        public static unsafe delegate*<ref PQExpBufferData, sbyte*, ulong, void> appendBinaryPQExpBuffer;
        public static unsafe delegate*<ref PQExpBufferData, sbyte*, void>        printfPQExpBuffer;

        ///// <summary>
        ///// make a new client connection to the backend
        ///// </summary>
        ///// <remarks>
        ///// Asynchronous (non-blocking)
        ///// </remarks>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, EntryPoint = "PQconnectStart", CallingConvention = CallingConvention.Cdecl)]
        //private static extern pg_conn _PQconnectStart([MarshalAs(UnmanagedType.LPStr)] string conninfo);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static async Task<pg_conn> PQconnectStart(string conninfo)
        //{
        //    return await Task.FromResult(_PQconnectStart(conninfo));
        //}

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_conn PQconnectStartParams([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keywords,
        //                                                  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values,
        //                                                  int                                                                             expand_dbname);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PostgresPollingStatusType PQconnectPoll(pg_conn conn);

        ///// <summary>
        ///// Synchronous (blocking)
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_conn PQconnectdb([MarshalAs(UnmanagedType.LPStr)] string conninfo);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_conn PQconnectdbParams([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keywords,
        //                                               [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values,
        //                                               int                                                                             expand_dbname);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_conn PQsetdbLogin([MarshalAs(UnmanagedType.LPStr)] string pghost,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string pgport,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string pgoptions,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string pgtty,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string dbName,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string login,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string pwd);

        ///// <summary>
        ///// close the current connection and free the PGconn data structure
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQfinish(pg_conn conn);

        ///// <summary>
        ///// get info about connection options known to PQconnectdb
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ref _PQconninfoOption PQconndefaults();

        ///// <summary>
        ///// parse connection options in same way as PQconnectdb
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ref _PQconninfoOption PQconninfoParse([MarshalAs(UnmanagedType.LPStr)]     string conninfo,
        //                                                           [MarshalAs(UnmanagedType.LPStr)] out string errmsg);

        ///// <summary>
        ///// return the connection options used by a live connection
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ref _PQconninfoOption PQconninfo(pg_conn conn);

        ///// <summary>
        ///// free the data structure returned by PQconndefaults() or PQconninfoParse()
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQconninfoFree(ref _PQconninfoOption connOptions);

        ///// <summary>
        ///// close the current connection and restablish a new one with the same
        ///// parameters
        ///// </summary>
        ///// <remarks>
        ///// Asynchronous (non-blocking)
        ///// </remarks>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, EntryPoint = "PQresetStart", CallingConvention = CallingConvention.Cdecl)]
        //private static extern int _PQresetStart(pg_conn conn);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static async Task<int> PQresetStart(pg_conn conn)
        //{
        //    return await Task.FromResult(_PQresetStart(conn));
        //}

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PostgresPollingStatusType PQresetPoll(pg_conn conn);

        ///// <summary>
        ///// Synchronous (blocking)
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQreset(pg_conn conn);

        ///// <summary>
        ///// request a cancel structure
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_cancel PQgetCancel(pg_conn conn);

        ///// <summary>
        ///// free a cancel structure
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQfreeCancel(pg_cancel cancel);

        ///// <summary>
        ///// issue a cancel request
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQcancel(pg_cancel cancel,
        //                                  nint    errbuf,
        //                                  int       errbufsize);

        ///// <summary>
        ///// backwards compatible version of PQcancel; not thread-safe
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQrequestCancel(pg_conn conn);

        ///// <summary>
        ///// Accessor functions for PGconn objects
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQdb(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQuser(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQpass(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQhost(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQhostaddr(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQport(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQtty(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern pg_string PQoptions(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ConnStatusType PQstatus(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PGTransactionStatusType PQtransactionStatus(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQparameterStatus(pg_conn                                 conn,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string paramName);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQprotocolVersion(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQserverVersion(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQerrorMessage(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsocket(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQbackendPID(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQconnectionNeedsPassword(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQconnectionUsedPassword(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQclientEncoding(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetClientEncoding(pg_conn                                 conn,
        //                                             [MarshalAs(UnmanagedType.LPStr)] string encoding);

        ///// <summary>
        ///// SSL information functions
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsslInUse(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQsslStruct(pg_conn                                 conn,
        //                                        [MarshalAs(UnmanagedType.LPStr)] string struct_name);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQsslAttribute(pg_conn                                 conn,
        //                                           [MarshalAs(UnmanagedType.LPStr)] string attribute_name);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQsslAttributeNames(pg_conn conn);

        ///// <summary>
        ///// Get the OpenSSL structure associated with a connection. Returns NULL for
        ///// unencrypted connections or if any other TLS library is in use.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQgetssl(pg_conn conn);

        ///// <summary>
        ///// Tell libpq whether it needs to initialize OpenSSL
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQinitSSL(int do_init);

        ///// <summary>
        ///// More detailed way to tell libpq whether it needs to initialize OpenSSL
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQinitOpenSSL(int do_ssl,
        //                                        int do_crypto);

        ///// <summary>
        ///// Return true if GSSAPI encryption is in use
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQgssEncInUse(pg_conn conn);

        ///// <summary>
        ///// Returns GSSAPI context if GSSAPI is in use
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQgetgssctx(pg_conn conn);

        ///// <summary>
        ///// Set verbosity for PQerrorMessage and PQresultErrorMessage
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PGVerbosity PQsetErrorVerbosity(pg_conn     conn,
        //                                                     PGVerbosity verbosity);

        ///// <summary>
        ///// Set CONTEXT visibility for PQerrorMessage and PQresultErrorMessage
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PGContextVisibility PQsetErrorContextVisibility(pg_conn             conn,
        //                                                                     PGContextVisibility show_context);

        ///// <summary>
        ///// Enable/disable tracing
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQtrace(pg_conn    conn,
        //                                  ref _iobuf debug_port);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQuntrace(pg_conn conn);

        ///// <summary>
        ///// Override default notice handling routines
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PQnoticeReceiver PQsetNoticeReceiver(pg_conn                                 conn,
        //                                                          PQnoticeReceiver                        proc,
        //                                                          [MarshalAs(UnmanagedType.LPStr)] string arg);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PQnoticeProcessor PQsetNoticeProcessor(pg_conn                                 conn,
        //                                                            PQnoticeProcessor                       proc,
        //                                                            [MarshalAs(UnmanagedType.LPStr)] string arg);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pgthreadlock_t PQregisterThreadLock(pgthreadlock_t newhandler);

        ///// <summary>
        ///// Simple synchronous query
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQexec(pg_conn                                 conn,
        //                                      [MarshalAs(UnmanagedType.LPStr)] string query);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQexecParams(pg_conn                                                                         conn,
        //                                            [MarshalAs(UnmanagedType.LPStr)] string                                         command,
        //                                            int                                                                             nParams,
        //                                            ref                                                                    uint     paramTypes,
        //                                            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paramValues,
        //                                            ref                                                                    int      paramLengths,
        //                                            ref                                                                    int      paramFormats,
        //                                            int                                                                             resultFormat);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQprepare(pg_conn                                 conn,
        //                                         [MarshalAs(UnmanagedType.LPStr)] string stmtName,
        //                                         [MarshalAs(UnmanagedType.LPStr)] string query,
        //                                         int                                     nParams,
        //                                         ref uint                                paramTypes);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQexecPrepared(pg_conn                                                                         conn,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string                                         stmtName,
        //                                              int                                                                             nParams,
        //                                              [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paramValues,
        //                                              ref                                                                    int      paramLengths,
        //                                              ref                                                                    int      paramFormats,
        //                                              int                                                                             resultFormat);

        ///// <summary>
        ///// Interface for multiple-result or asynchronous queries
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, EntryPoint = "PQsendQuery", CallingConvention = CallingConvention.Cdecl)]
        //private static extern int _PQsendQuery(pg_conn                                 conn,
        //                                       [MarshalAs(UnmanagedType.LPStr)] string query);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static async Task<int> PQsendQuery(pg_conn conn,
        //                                          string  query)
        //{
        //    return await Task.FromResult(_PQsendQuery(conn, query));
        //}

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsendQueryParams(pg_conn                                                                         conn,
        //                                           [MarshalAs(UnmanagedType.LPStr)] string                                         command,
        //                                           int                                                                             nParams,
        //                                           ref                                                                    uint     paramTypes,
        //                                           [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paramValues,
        //                                           ref                                                                    int      paramLengths,
        //                                           ref                                                                    int      paramFormats,
        //                                           int                                                                             resultFormat);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsendPrepare(pg_conn                                 conn,
        //                                       [MarshalAs(UnmanagedType.LPStr)] string stmtName,
        //                                       [MarshalAs(UnmanagedType.LPStr)] string query,
        //                                       int                                     nParams,
        //                                       ref uint                                paramTypes);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsendQueryPrepared(pg_conn                                                                         conn,
        //                                             [MarshalAs(UnmanagedType.LPStr)] string                                         stmtName,
        //                                             int                                                                             nParams,
        //                                             [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paramValues,
        //                                             ref                                                                    int      paramLengths,
        //                                             ref                                                                    int      paramFormats,
        //                                             int                                                                             resultFormat);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetSingleRowMode(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQgetResult(pg_conn conn);

        ///// <summary>
        ///// Routines for managing an asynchronous query
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, EntryPoint = "PQisBusy", CallingConvention = CallingConvention.Cdecl)]
        //private static extern int _PQisBusy(pg_conn conn);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static async Task<int> PQisBusy(pg_conn conn)
        //{
        //    return await Task.FromResult(_PQisBusy(conn));
        //}

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQconsumeInput(pg_conn conn);

        ///// <summary>
        ///// LISTEN/NOTIFY support
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ref pgNotify PQnotifies(pg_conn conn);

        ///// <summary>
        ///// Routines for copy in/out
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQputCopyData(pg_conn                                 conn,
        //                                       [MarshalAs(UnmanagedType.LPStr)] string buffer,
        //                                       int                                     nbytes);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQputCopyEnd(pg_conn                                 conn,
        //                                      [MarshalAs(UnmanagedType.LPStr)] string errormsg);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQgetCopyData(pg_conn    conn,
        //                                       out nint buffer,
        //                                       int        async);

        ///// <summary>
        ///// Deprecated routines for copy in/out
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQgetline(pg_conn                                 conn,
        //                                   [MarshalAs(UnmanagedType.LPStr)] string @string,
        //                                   int                                     length);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQputline(pg_conn                                 conn,
        //                                   [MarshalAs(UnmanagedType.LPStr)] string @string);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, EntryPoint = "PQgetlineAsync", CallingConvention = CallingConvention.Cdecl)]
        //private static extern int _PQgetlineAsync(pg_conn                                 conn,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string buffer,
        //                                          int                                     bufsize);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static async Task<int> PQgetlineAsync(pg_conn conn,
        //                                             string  buffer,
        //                                             int     bufsize)
        //{
        //    return await Task.FromResult(_PQgetlineAsync(conn, buffer, bufsize));
        //}

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQputnbytes(pg_conn                                 conn,
        //                                     [MarshalAs(UnmanagedType.LPStr)] string buffer,
        //                                     int                                     nbytes);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQendcopy(pg_conn conn);

        ///// <summary>
        ///// Set blocking/nonblocking connection to the backend
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetnonblocking(pg_conn conn,
        //                                          int     arg);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQisnonblocking(pg_conn conn);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQisthreadsafe();

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PGPing PQping([MarshalAs(UnmanagedType.LPStr)] string conninfo);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern PGPing PQpingParams([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] keywords,
        //                                         [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] values,
        //                                         int                                                                             expand_dbname);

        ///// <summary>
        ///// Force the write buffer to be written (or at least try)
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQflush(pg_conn conn);

        ///// <summary>
        ///// "Fast path" interface --- not really recommended for application
        ///// use
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQfn(pg_conn       conn,
        //                                    int           fnid,
        //                                    ref int       result_buf,
        //                                    ref int       result_len,
        //                                    int           result_is_int,
        //                                    in PQArgBlock args,
        //                                    int           nargs);

        ///// <summary>
        ///// Accessor functions for pg_result objects
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ExecStatusType PQresultStatus(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresStatus(ExecStatusType status);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresultErrorMessage(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresultVerboseErrorMessage(pg_result           res,
        //                                                        PGVerbosity         verbosity,
        //                                                        PGContextVisibility show_context);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresultErrorField(pg_result res,
        //                                               int       fieldcode);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQntuples(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQnfields(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQbinaryTuples(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQfname(pg_result res,
        //                                       int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQfnumber(pg_result                               res,
        //                                   [MarshalAs(UnmanagedType.LPStr)] string field_name);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint PQftable(pg_result res,
        //                                   int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQftablecol(pg_result res,
        //                                     int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQfformat(pg_result res,
        //                                   int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint PQftype(pg_result res,
        //                                  int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQfsize(pg_result res,
        //                                 int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQfmod(pg_result res,
        //                                int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQcmdStatus(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQoidStatus(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint PQoidValue(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQcmdTuples(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQgetvalue(pg_result res,
        //                                          int       tup_num,
        //                                          int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQgetlength(pg_result res,
        //                                     int       tup_num,
        //                                     int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQgetisnull(pg_result res,
        //                                     int       tup_num,
        //                                     int       field_num);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQnparams(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint PQparamtype(pg_result res,
        //                                      int       param_num);

        ///// <summary>
        ///// Describe prepared statements and portals
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQdescribePrepared(pg_conn                                 conn,
        //                                                  [MarshalAs(UnmanagedType.LPStr)] string stmt);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQdescribePortal(pg_conn                                 conn,
        //                                                [MarshalAs(UnmanagedType.LPStr)] string portal);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsendDescribePrepared(pg_conn                                 conn,
        //                                                [MarshalAs(UnmanagedType.LPStr)] string stmt);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsendDescribePortal(pg_conn                                 conn,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string portal);

        ///// <summary>
        ///// Delete a pg_result
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQclear(pg_result res);

        ///// <summary>
        ///// For freeing other alloc'd results, such as PGnotify structs
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQfreemem(nint ptr);

        ///// <summary>
        ///// Create and manipulate PGresults
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQmakeEmptyPGresult(pg_conn        conn,
        //                                                   ExecStatusType status);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_result PQcopyResult(pg_result src,
        //                                            int       flags);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetResultAttrs(pg_result        res,
        //                                          int              numAttributes,
        //                                          ref pgresAttDesc attDescs);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresultAlloc(pg_result res,
        //                                          ulong     nBytes);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong PQresultMemorySize(pg_result res);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetvalue(pg_result                               res,
        //                                    int                                     tup_num,
        //                                    int                                     field_num,
        //                                    [MarshalAs(UnmanagedType.LPStr)] string value,
        //                                    int                                     len);

        ///// <summary>
        ///// Quoting strings before inclusion in queries.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong PQescapeStringConn(pg_conn                                 conn,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string to,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string from,
        //                                              ulong                                   length,
        //                                              ref int                                 error);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQescapeLiteral(pg_conn                                 conn,
        //                                               [MarshalAs(UnmanagedType.LPStr)] string str,
        //                                               ulong                                   len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQescapeIdentifier(pg_conn                                 conn,
        //                                                  [MarshalAs(UnmanagedType.LPStr)] string str,
        //                                                  ulong                                   len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQescapeByteaConn(pg_conn                                 conn,
        //                                              [MarshalAs(UnmanagedType.LPStr)] string from,
        //                                              ulong                                   from_length,
        //                                              ref ulong                               to_length);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQunescapeBytea([MarshalAs(UnmanagedType.LPStr)] string strtext,
        //                                            ref                              ulong  retbuflen);

        ///// <summary>
        ///// These forms are deprecated!
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern ulong PQescapeString([MarshalAs(UnmanagedType.LPStr)] string to,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string from,
        //                                          ulong                                   length);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQescapeBytea([MarshalAs(UnmanagedType.LPStr)] string from,
        //                                          ulong                                   from_length,
        //                                          ref ulong                               to_length);

        ///// <summary>
        ///// === in fe-print.c ===
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQprint(ref _iobuf      fout,
        //                                  pg_result       res,
        //                                  ref PQprintOpt ps);

        ///// <summary>
        ///// really old printing routines
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQdisplayTuples(pg_result                               res,
        //                                          ref _iobuf                              fp,
        //                                          int                                     fillAlign,
        //                                          [MarshalAs(UnmanagedType.LPStr)] string fieldSep,
        //                                          int                                     printHeader,
        //                                          int                                     quiet);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void PQprintTuples(pg_result  res,
        //                                        ref _iobuf fout,
        //                                        int        printAttName,
        //                                        int        terseOutput,
        //                                        int        width);

        ///// <summary>
        ///// Large-object access routines
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_open(pg_conn conn,
        //                                 uint    lobjId,
        //                                 int     mode);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_close(pg_conn conn,
        //                                  int     fd);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_read(pg_conn conn,
        //                                 int     fd,
        //                                 nint  buf,
        //                                 ulong   len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_write(pg_conn conn,
        //                                  int     fd,
        //                                  nint  buf,
        //                                  ulong   len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_lseek(pg_conn conn,
        //                                  int     fd,
        //                                  int     offset,
        //                                  int     whence);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern long lo_lseek64(pg_conn conn,
        //                                     int     fd,
        //                                     long    offset,
        //                                     int     whence);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint lo_creat(pg_conn conn,
        //                                   int     mode);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint lo_create(pg_conn conn,
        //                                    uint    lobjId);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_tell(pg_conn conn,
        //                                 int     fd);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern long lo_tell64(pg_conn conn,
        //                                    int     fd);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_truncate(pg_conn conn,
        //                                     int     fd,
        //                                     ulong   len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_truncate64(pg_conn conn,
        //                                       int     fd,
        //                                       long    len);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_unlink(pg_conn conn,
        //                                   uint    lobjId);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint lo_import(pg_conn                                 conn,
        //                                    [MarshalAs(UnmanagedType.LPStr)] string filename);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern uint lo_import_with_oid(pg_conn                                 conn,
        //                                             [MarshalAs(UnmanagedType.LPStr)] string filename,
        //                                             uint                                    lobjId);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lo_export(pg_conn                                 conn,
        //                                   uint                                    lobjId,
        //                                   [MarshalAs(UnmanagedType.LPStr)] string filename);

        ///// <summary>
        ///// Get the version of the libpq library in use
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQlibVersion();

        ///// <summary>
        ///// Determine length of multibyte encoded char at *s
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQmblen([MarshalAs(UnmanagedType.LPStr)] string s,
        //                                 int                                     encoding);

        ///// <summary>
        ///// Determine display length of multibyte encoded char at *s
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQdsplen([MarshalAs(UnmanagedType.LPStr)] string s,
        //                                  int                                     encoding);

        ///// <summary>
        ///// Get encoding id from environment variable PGCLIENTENCODING
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQenv2encoding();

        ///// <summary>
        ///// === in fe-auth.c ===
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQencryptPassword([MarshalAs(UnmanagedType.LPStr)] string passwd,
        //                                                 [MarshalAs(UnmanagedType.LPStr)] string user);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string PQencryptPasswordConn(pg_conn                                 conn,
        //                                                     [MarshalAs(UnmanagedType.LPStr)] string passwd,
        //                                                     [MarshalAs(UnmanagedType.LPStr)] string user,
        //                                                     [MarshalAs(UnmanagedType.LPStr)] string algorithm);

        ///// <summary>
        ///// === in encnames.c ===
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int pg_char_to_encoding([MarshalAs(UnmanagedType.LPStr)] string name);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pg_string pg_encoding_to_char(int encoding);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int pg_valid_server_encoding_id(int encoding);

        ///// <summary>
        ///// Fires RESULTCREATE events for an application-created PGresult.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQfireResultCreateEvents(ref pg_conn   conn,
        //                                                  ref pg_result res);

        ///// <summary>
        ///// Gets the PGconn instance data for the provided proc.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQinstanceData(ref pg_conn conn,
        //                                           PGEventProc proc);

        ///// <summary>
        ///// Registers an event proc with the given PGconn.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQregisterEventProc(ref pg_conn conn,
        //                                             PGEventProc proc,
        //                                             nint      name,
        //                                             nint      passThrough);

        ///// <summary>
        ///// Gets the PGresult instance data for the provided proc.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint PQresultInstanceData(ref pg_result result,
        //                                                 PGEventProc   proc);

        ///// <summary>
        ///// Sets the PGresult instance data for the provided proc to data.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQresultSetInstanceData(ref pg_result result,
        //                                                 PGEventProc   proc,
        //                                                 nint        data);

        ///// <summary>
        ///// Sets the PGconn instance data for the provided proc to data.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int PQsetInstanceData(ref pg_conn conn,
        //                                           PGEventProc proc,
        //                                           nint      data);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int pg_utf_mblen(nint arg0);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern pqsigfunc pqsignal(int       signo,
        //                                        pqsigfunc func);

        ///// <summary>
        ///// ------------------------
        ///// createPQExpBuffer
        ///// Create an empty 'PQExpBufferData' 
        ///// &amp;
        ///// return a pointer to it.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern nint createPQExpBuffer();

        ///// <summary>
        ///// ------------------------
        ///// initPQExpBuffer
        ///// Initialize a PQExpBufferData struct (with previously undefined contents)
        ///// to describe an empty string.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void initPQExpBuffer(nint str);

        ///// <summary>
        ///// ------------------------
        ///// To destroy a PQExpBuffer, use either:
        ///// </summary>
        ///// <remarks>
        ///// destroyPQExpBuffer(str);
        ///// free()s both the data buffer and the PQExpBufferData.
        ///// This is the inverse of createPQExpBuffer().termPQExpBuffer(str)
        ///// free()s the data buffer but not the PQExpBufferData itself.
        ///// This is the inverse of initPQExpBuffer().NOTE: some routines build up a string using PQExpBuffer, and then
        ///// release the PQExpBufferData but return the data string itself to their
        ///// caller.  At that point the data string looks like a plain malloc'd
        ///// string.
        ///// </remarks>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void destroyPQExpBuffer(nint str);

        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void termPQExpBuffer(nint str);

        ///// <summary>
        ///// ------------------------
        ///// resetPQExpBuffer
        ///// Reset a PQExpBuffer to empty
        ///// </summary>
        ///// <remarks>
        ///// Note: if possible, a "broken" PQExpBuffer is returned to normal.
        ///// </remarks>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void resetPQExpBuffer(nint str);

        ///// <summary>
        ///// ------------------------
        ///// enlargePQExpBuffer
        ///// Make sure there is enough space for 'needed' more bytes in the buffer
        ///// ('needed' does not include the terminating null).
        ///// </summary>
        ///// <remarks>
        ///// Returns 1 if OK, 0 if failed to enlarge buffer.  (In the latter case
        ///// the buffer is left in "broken" state.)
        ///// </remarks>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int enlargePQExpBuffer(nint str,
        //                                            ulong  needed);

        ///// <summary>
        ///// ------------------------
        ///// printfPQExpBuffer
        ///// Format text data under the control of fmt (an sprintf-like format string)
        ///// and insert it into str.  More space is allocated to str if necessary.
        ///// This is a convenience routine that does the same thing as
        ///// resetPQExpBuffer() followed by appendPQExpBuffer().
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void printfPQExpBuffer(nint str,
        //                                            nint fmt);

        ///// <summary>
        ///// ------------------------
        ///// appendPQExpBuffer
        ///// Format text data under the control of fmt (an sprintf-like format string)
        ///// and append it to whatever is already in str.  More space is allocated
        ///// to str if necessary.  This is sort of like a combination of sprintf and
        ///// strcat.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void appendPQExpBuffer(nint str,
        //                                            nint fmt);

        ///// <summary>
        ///// ------------------------
        ///// appendPQExpBufferStr
        ///// Append the given string to a PQExpBuffer, allocating more space
        ///// if necessary.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void appendPQExpBufferStr(nint str,
        //                                               nint data);

        ///// <summary>
        ///// ------------------------
        ///// appendPQExpBufferChar
        ///// Append a single byte to str.
        ///// Like appendPQExpBuffer(str, "%c", ch) but much faster.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void appendPQExpBufferChar(nint str,
        //                                                sbyte  ch);

        ///// <summary>
        ///// ------------------------
        ///// appendBinaryPQExpBuffer
        ///// Append arbitrary binary data to a PQExpBuffer, allocating more space
        ///// if necessary.
        ///// </summary>
        //[SuppressUnmanagedCodeSecurity]
        //[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        //[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void appendBinaryPQExpBuffer(nint str,
        //                                                  nint data,
        //                                                  ulong  datalen);
    }
}
