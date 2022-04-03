// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace PostgreSql
{
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
        private const int LIBPQ_HAS_PIPELINING = 1;

        [NativeTypeName("#define LIBPQ_HAS_TRACE_FLAGS 1")]
        private const int LIBPQ_HAS_TRACE_FLAGS = 1;

        [NativeTypeName("#define LIBPQ_HAS_SSL_LIBRARY_DETECTION 1")]
        private const int LIBPQ_HAS_SSL_LIBRARY_DETECTION = 1;

        [NativeTypeName("#define PG_COPYRES_ATTRS 0x01")]
        private const int PG_COPYRES_ATTRS = 0x01;

        [NativeTypeName("#define PG_COPYRES_TUPLES 0x02")]
        private const int PG_COPYRES_TUPLES = 0x02;

        [NativeTypeName("#define PG_COPYRES_EVENTS 0x04")]
        private const int PG_COPYRES_EVENTS = 0x04;

        [NativeTypeName("#define PG_COPYRES_NOTICEHOOKS 0x08")]
        private const int PG_COPYRES_NOTICEHOOKS = 0x08;

        [NativeTypeName("#define PQTRACE_SUPPRESS_TIMESTAMPS (1<<0)")]
        private const int PQTRACE_SUPPRESS_TIMESTAMPS = (1 << 0);

        [NativeTypeName("#define PQTRACE_REGRESS_MODE (1<<1)")]
        private const int PQTRACE_REGRESS_MODE = (1 << 1);

        [NativeTypeName("#define PQ_QUERY_PARAM_MAX_LIMIT 65535")]
        private const int PQ_QUERY_PARAM_MAX_LIMIT = 65535;

        [NativeTypeName("#define PQnoPasswordSupplied \"fe_sendauth: no password supplied\\n\"")]
        public static ReadOnlySpan<byte> PQnoPasswordSupplied => new byte[] { 0x66, 0x65, 0x5F, 0x73, 0x65, 0x6E, 0x64, 0x61, 0x75, 0x74, 0x68, 0x3A, 0x20, 0x6E, 0x6F, 0x20, 0x70, 0x61, 0x73, 0x73, 0x77, 0x6F, 0x72, 0x64, 0x20, 0x73, 0x75, 0x70, 0x70, 0x6C, 0x69, 0x65, 0x64, 0x0A, 0x00 };





        private const int PQbackendPIDRVA = 0x0000B200;
        private const int PQbinaryTuplesRVA = 0x000136E0;
        private const int PQcancelRVA = 0x0000A100;
        private const int PQclearRVA = 0x0000E770;
        private const int PQclientEncodingRVA = 0x0000B360;
        private const int PQcmdStatusRVA = 0x00013DC0;
        private const int PQcmdTuplesRVA = 0x00013FB0;
        private const int PQconndefaultsRVA = 0x000058D0;
        private const int PQconnectPollRVA = 0x00006190;
        private const int PQconnectStartRVA = 0x000037A0;
        private const int PQconnectStartParamsRVA = 0x00003210;
        private const int PQconnectdbRVA = 0x00003750;
        private const int PQconnectdbParamsRVA = 0x000031B0;
        private const int PQconnectionNeedsPasswordRVA = 0x0000B290;
        private const int PQconnectionUsedPasswordRVA = 0x0000B310;
        private const int PQconninfoRVA = 0x0000A820;
        private const int PQconninfoFreeRVA = 0x000042D0;
        private const int PQconninfoParseRVA = 0x0000A6F0;
        private const int PQconsumeInputRVA = 0x00011860;
        private const int PQcopyResultRVA = 0x0000EC20;
        private const int PQdbRVA = 0x0000AAF0;
        private const int PQdefaultSSLKeyPassHook_OpenSSLRVA = 0x00028660;
        private const int PQdescribePortalRVA = 0x000126B0;
        private const int PQdescribePreparedRVA = 0x000124A0;
        private const int PQdisplayTuplesRVA = 0x0001B530;
        private const int PQdsplenRVA = 0x000193C0;
        private const int PQencryptPasswordRVA = 0x00024D20;
        private const int PQencryptPasswordConnRVA = 0x00024DE0;
        private const int PQendcopyRVA = 0x00012DF0;
        private const int PQenterPipelineModeRVA = 0x00012FB0;
        private const int PQenv2encodingRVA = 0x000193F0;
        private const int PQerrorMessageRVA = 0x0000B110;
        private const int PQescapeByteaRVA = 0x00015600;
        private const int PQescapeByteaConnRVA = 0x000150B0;
        private const int PQescapeIdentifierRVA = 0x00015070;
        private const int PQescapeLiteralRVA = 0x00014B90;
        private const int PQescapeStringRVA = 0x00014B40;
        private const int PQescapeStringConnRVA = 0x000147B0;
        private const int PQexecRVA = 0x00011FD0;
        private const int PQexecParamsRVA = 0x00012260;
        private const int PQexecPreparedRVA = 0x000123E0;
        private const int PQexitPipelineModeRVA = 0x00013050;
        private const int PQfformatRVA = 0x00013BC0;
        private const int PQfinishRVA = 0x00003720;
        private const int PQfireResultCreateEventsRVA = 0x00022CE0;
        private const int PQflushRVA = 0x00013320;
        private const int PQfmodRVA = 0x00013D40;
        private const int PQfnRVA = 0x00012E30;
        private const int PQfnameRVA = 0x00013720;
        private const int PQfnumberRVA = 0x000137A0;
        private const int PQfreeCancelRVA = 0x0000A0D0;
        private const int PQfreeNotifyRVA = 0x00014790;
        private const int PQfreememRVA = 0x00014770;
        private const int PQfsizeRVA = 0x00013CC0;
        private const int PQftableRVA = 0x00013AC0;
        private const int PQftablecolRVA = 0x00013B40;
        private const int PQftypeRVA = 0x00013C40;
        private const int PQgetCancelRVA = 0x00009E20;
        private const int PQgetCopyDataRVA = 0x00012BB0;
        private const int PQgetResultRVA = 0x00011980;
        private const int PQgetSSLKeyPassHook_OpenSSLRVA = 0x00028720;
        private const int PQgetgssctxRVA = 0x00020880;
        private const int PQgetisnullRVA = 0x00014480;
        private const int PQgetlengthRVA = 0x000143E0;
        private const int PQgetlineRVA = 0x00012C60;
        private const int PQgetlineAsyncRVA = 0x00012D00;
        private const int PQgetsslRVA = 0x000283F0;
        private const int PQgetvalueRVA = 0x00014290;
        private const int PQgssEncInUseRVA = 0x00020890;
        private const int PQhostRVA = 0x0000AC20;
        private const int PQhostaddrRVA = 0x0000AD80;
        private const int PQinitOpenSSLRVA = 0x00020360;
        private const int PQinitSSLRVA = 0x00020330;
        private const int PQinstanceDataRVA = 0x00022A70;
        private const int PQisBusyRVA = 0x000118F0;
        private const int PQisnonblockingRVA = 0x00014740;
        private const int PQisthreadsafeRVA = 0x00014760;
        private const int PQlibVersionRVA = 0x00017960;
        private const int PQmakeEmptyPGresultRVA = 0x0000E200;
        private const int PQmblenRVA = 0x00019350;
        private const int PQmblenBoundedRVA = 0x00019380;
        private const int PQnfieldsRVA = 0x000136A0;
        private const int PQnotifiesRVA = 0x00012790;
        private const int PQnparamsRVA = 0x00014500;
        private const int PQntuplesRVA = 0x00013660;
        private const int PQoidStatusRVA = 0x00013E00;
        private const int PQoidValueRVA = 0x00013ED0;
        private const int PQoptionsRVA = 0x0000AEB0;
        private const int PQparameterStatusRVA = 0x0000AFA0;
        private const int PQparamtypeRVA = 0x00014540;
        private const int PQpassRVA = 0x0000AB70;
        private const int PQpingRVA = 0x00003850;
        private const int PQpingParamsRVA = 0x000035D0;
        private const int PQpipelineStatusRVA = 0x0000B250;
        private const int PQpipelineSyncRVA = 0x000131A0;
        private const int PQportRVA = 0x0000ADF0;
        private const int PQprepareRVA = 0x00012340;
        private const int PQprintRVA = 0x000197A0;
        private const int PQprintTuplesRVA = 0x0001B9A0;
        private const int PQprotocolVersionRVA = 0x0000B050;
        private const int PQputCopyDataRVA = 0x00012840;
        private const int PQputCopyEndRVA = 0x000129D0;
        private const int PQputlineRVA = 0x00012D50;
        private const int PQputnbytesRVA = 0x00012DA0;
        private const int PQregisterEventProcRVA = 0x00022670;
        private const int PQregisterThreadLockRVA = 0x0000B780;
        private const int PQrequestCancelRVA = 0x0000A590;
        private const int PQresStatusRVA = 0x00013470;
        private const int PQresetRVA = 0x00009C30;
        private const int PQresetPollRVA = 0x00009D50;
        private const int PQresetStartRVA = 0x00009D00;
        private const int PQresultAllocRVA = 0x0000EB60;
        private const int PQresultErrorFieldRVA = 0x000135D0;
        private const int PQresultErrorMessageRVA = 0x000134C0;
        private const int PQresultInstanceDataRVA = 0x00022C10;
        private const int PQresultMemorySizeRVA = 0x0000F9D0;
        private const int PQresultSetInstanceDataRVA = 0x00022B40;
        private const int PQresultStatusRVA = 0x00013430;
        private const int PQresultVerboseErrorMessageRVA = 0x00013510;
        private const int PQsendDescribePortalRVA = 0x00012760;
        private const int PQsendDescribePreparedRVA = 0x00012730;
        private const int PQsendFlushRequestRVA = 0x00013340;
        private const int PQsendPrepareRVA = 0x000110D0;
        private const int PQsendQueryRVA = 0x000103A0;
        private const int PQsendQueryParamsRVA = 0x00010810;
        private const int PQsendQueryPreparedRVA = 0x00011650;
        private const int PQserverVersionRVA = 0x0000B0B0;
        private const int PQsetClientEncodingRVA = 0x0000B3B0;
        private const int PQsetErrorContextVisibilityRVA = 0x0000B580;
        private const int PQsetErrorVerbosityRVA = 0x0000B520;
        private const int PQsetInstanceDataRVA = 0x000229A0;
        private const int PQsetNoticeProcessorRVA = 0x0000B660;
        private const int PQsetNoticeReceiverRVA = 0x0000B5E0;
        private const int PQsetResultAttrsRVA = 0x0000E950;
        private const int PQsetSSLKeyPassHook_OpenSSLRVA = 0x00028730;
        private const int PQsetSingleRowModeRVA = 0x00011780;
        private const int PQsetTraceFlagsRVA = 0x00020970;
        private const int PQsetdbLoginRVA = 0x00005D40;
        private const int PQsetnonblockingRVA = 0x00014640;
        private const int PQsetvalueRVA = 0x0000EFB0;
        private const int PQsocketRVA = 0x0000B190;
        private const int PQsslAttributeRVA = 0x000284B0;
        private const int PQsslAttributeNamesRVA = 0x000284A0;
        private const int PQsslInUseRVA = 0x000202F0;
        private const int PQsslStructRVA = 0x00028430;
        private const int PQstatusRVA = 0x0000AEF0;
        private const int PQtraceRVA = 0x000208A0;
        private const int PQtransactionStatusRVA = 0x0000AF30;
        private const int PQttyRVA = 0x0000AE70;
        private const int PQunescapeByteaRVA = 0x00015650;
        private const int PQuntraceRVA = 0x00020900;
        private const int PQuserRVA = 0x0000AB30;
        private const int appendBinaryPQExpBufferRVA = 0x000234E0;
        private const int appendPQExpBufferRVA = 0x000233F0;
        private const int appendPQExpBufferCharRVA = 0x00023560;
        private const int appendPQExpBufferStrRVA = 0x00023490;
        private const int createPQExpBufferRVA = 0x00022E10;
        private const int destroyPQExpBufferRVA = 0x00022EF0;
        private const int enlargePQExpBufferRVA = 0x00022FF0;
        private const int initPQExpBufferRVA = 0x00022E50;
        private const int lo_closeRVA = 0x00016380;
        private const int lo_creatRVA = 0x00016DA0;
        private const int lo_createRVA = 0x00016E70;
        private const int lo_exportRVA = 0x00017620;
        private const int lo_importRVA = 0x00017260;
        private const int lo_import_with_oidRVA = 0x000175F0;
        private const int lo_lseekRVA = 0x00016A50;
        private const int lo_lseek64RVA = 0x00016B90;
        private const int lo_openRVA = 0x00015AE0;
        private const int lo_readRVA = 0x000167A0;
        private const int lo_tellRVA = 0x00016F80;
        private const int lo_tell64RVA = 0x00017050;
        private const int lo_truncateRVA = 0x00016450;
        private const int lo_truncate64RVA = 0x000165F0;
        private const int lo_unlinkRVA = 0x00017190;
        private const int lo_writeRVA = 0x000168F0;
        private const int pg_char_to_encodingRVA = 0x000297F0;
        private const int pg_encoding_to_charRVA = 0x00029B20;
        private const int pg_utf_mblenRVA = 0x0002BA90;
        private const int pg_valid_server_encodingRVA = 0x000299A0;
        private const int pg_valid_server_encoding_idRVA = 0x00029A10;
        private const int pgresStatusRVA = 0x002171C0;
        private const int pqsignalRVA = 0x00022640;
        private const int printfPQExpBufferRVA = 0x000231F0;
        private const int resetPQExpBufferRVA = 0x00022F90;
        private const int termPQExpBufferRVA = 0x00022F20;


        internal static unsafe void Load()
        {
            if(!IsLoaded)
            {
                Handle = PlatformApi.NativeLibrary.LoadByName(LibraryName, LibraryPath);

                
                //appendBinaryPQExpBuffer = (delegate*<ref PQExpBufferData, sbyte*, ulong, void>)(Handle + appendBinaryPQExpBufferRVA);
                //appendPQExpBuffer       = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + appendPQExpBufferRVA);
                //appendPQExpBufferChar   = (delegate*<ref PQExpBufferData, sbyte, void>)(Handle + appendPQExpBufferCharRVA);
                //appendPQExpBufferStr    = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + appendPQExpBufferStrRVA);
                //createPQExpBuffer       = (delegate*<ref PQExpBufferData>)(Handle + createPQExpBufferRVA);
                //destroyPQExpBuffer      = (delegate*<ref PQExpBufferData, void>)(Handle + destroyPQExpBufferRVA);
                //enlargePQExpBuffer      = (delegate*<ref PQExpBufferData, ulong, int>)(Handle + enlargePQExpBufferRVA);
                //initPQExpBuffer         = (delegate*<ref PQExpBufferData, void>)(Handle + initPQExpBufferRVA);
                //printfPQExpBuffer       = (delegate*<ref PQExpBufferData, sbyte*, void>)(Handle + printfPQExpBufferRVA);
                //resetPQExpBuffer        = (delegate*<ref PQExpBufferData, void>)(Handle + resetPQExpBufferRVA);
                //termPQExpBuffer         = (delegate*<ref PQExpBufferData, void>)(Handle + termPQExpBufferRVA);

                PQconnectStart = (delegate*<nint, pg_conn>)(Handle + PQconnectStartRVA);
                PQconnectStartParams = (delegate*<utf8string*, utf8string*, int, pg_conn>)(Handle + PQconnectStartParamsRVA);
                PQconnectPoll = (delegate*<pg_conn, PostgresPollingStatusType>)(Handle + PQconnectPollRVA);
                PQconnectdb = (delegate*<nint, pg_conn>)(Handle + PQconnectdbRVA);
                PQconnectdbParams = (delegate*<utf8string*, utf8string*, int, pg_conn>)(Handle + PQconnectdbParamsRVA);
                PQsetdbLogin = (delegate*<nint, nint, nint, nint, nint, nint, nint, pg_conn>)(Handle + PQsetdbLoginRVA);
                PQfinish = (delegate*<pg_conn, void>)(Handle + PQfinishRVA);
                PQconndefaults = (delegate*<ref PQconninfoOption>)(Handle + PQconndefaultsRVA);
                PQconninfoParse = (delegate*<nint, utf8string*, ref PQconninfoOption>)(Handle + PQconninfoParseRVA);
                PQconninfo = (delegate*<pg_conn, ref PQconninfoOption>)(Handle + PQconninfoRVA);
                PQconninfoFree = (delegate*<ref PQconninfoOption, void>)(Handle + PQconninfoFreeRVA);
                PQresetStart = (delegate*<pg_conn, int>)(Handle + PQresetStartRVA);
                PQresetPoll = (delegate*<pg_conn, PostgresPollingStatusType>)(Handle + PQresetPollRVA);
                PQreset = (delegate*<pg_conn, void>)(Handle + PQresetRVA);
                PQgetCancel = (delegate*<pg_conn, pg_cancel>)(Handle + PQgetCancelRVA);
                PQfreeCancel = (delegate*<pg_cancel, void>)(Handle + PQfreeCancelRVA);
                PQcancel = (delegate*<pg_cancel, nint, int, int>)(Handle + PQcancelRVA);
                PQrequestCancel = (delegate*<pg_conn, int>)(Handle + PQrequestCancelRVA);
                PQdb = (delegate*<pg_conn, nint>)(Handle + PQdbRVA);
                PQuser = (delegate*<pg_conn, nint>)(Handle + PQuserRVA);
                PQpass = (delegate*<pg_conn, nint>)(Handle + PQpassRVA);
                PQhost = (delegate*<pg_conn, nint>)(Handle + PQhostRVA);
                PQhostaddr = (delegate*<pg_conn, nint>)(Handle + PQhostaddrRVA);
                PQport = (delegate*<pg_conn, nint>)(Handle + PQportRVA);
                PQtty = (delegate*<pg_conn, nint>)(Handle + PQttyRVA);
                PQoptions = (delegate*<pg_conn, nint>)(Handle + PQoptionsRVA);
                PQstatus = (delegate*<pg_conn, ConnStatusType>)(Handle + PQstatusRVA);
                PQtransactionStatus = (delegate*<pg_conn, PGTransactionStatusType>)(Handle + PQtransactionStatusRVA);
                PQparameterStatus = (delegate*<pg_conn, nint, nint>)(Handle + PQparameterStatusRVA);
                PQprotocolVersion = (delegate*<pg_conn, int>)(Handle + PQprotocolVersionRVA);
                PQserverVersion = (delegate*<pg_conn, int>)(Handle + PQserverVersionRVA);
                PQerrorMessage = (delegate*<pg_conn, nint>)(Handle + PQerrorMessageRVA);
                PQsocket = (delegate*<pg_conn, int>)(Handle + PQsocketRVA);
                PQbackendPID = (delegate*<pg_conn, int>)(Handle + PQbackendPIDRVA);
                PQconnectionNeedsPassword = (delegate*<pg_conn, int>)(Handle + PQconnectionNeedsPasswordRVA);
                PQconnectionUsedPassword = (delegate*<pg_conn, int>)(Handle + PQconnectionUsedPasswordRVA);
                PQclientEncoding = (delegate*<pg_conn, int>)(Handle + PQclientEncodingRVA);
                PQsetClientEncoding = (delegate*<pg_conn, nint, int>)(Handle + PQsetClientEncodingRVA);
                PQsslInUse = (delegate*<pg_conn, int>)(Handle + PQsslInUseRVA);
                PQsslStruct = (delegate*<pg_conn, nint, nint>)(Handle + PQsslStructRVA);
                PQsslAttribute = (delegate*<pg_conn, nint, nint>)(Handle + PQsslAttributeRVA);
                PQsslAttributeNames = (delegate*<pg_conn, nint>)(Handle + PQsslAttributeNamesRVA);
                PQgetssl = (delegate*<pg_conn, nint>)(Handle + PQgetsslRVA);
                PQinitSSL = (delegate*<int, void>)(Handle + PQinitSSLRVA);
                PQinitOpenSSL = (delegate*<int, int, void>)(Handle + PQinitOpenSSLRVA);
                PQgssEncInUse = (delegate*<pg_conn, int>)(Handle + PQgssEncInUseRVA);
                PQgetgssctx = (delegate*<pg_conn, nint>)(Handle + PQgetgssctxRVA);
                PQsetErrorVerbosity = (delegate*<pg_conn, PGVerbosity, PGVerbosity>)(Handle + PQsetErrorVerbosityRVA);
                PQsetErrorContextVisibility = (delegate*<pg_conn, PGContextVisibility, PGContextVisibility>)(Handle + PQsetErrorContextVisibilityRVA);
                PQtrace = (delegate*<pg_conn, ref _iobuf, void>)(Handle + PQtraceRVA);
                PQuntrace = (delegate*<pg_conn, void>)(Handle + PQuntraceRVA);
                PQsetNoticeReceiver = (delegate*<pg_conn, PQnoticeReceiver, nint, PQnoticeReceiver>)(Handle + PQsetNoticeReceiverRVA);
                PQsetNoticeProcessor = (delegate*<pg_conn, PQnoticeProcessor, nint, PQnoticeProcessor>)(Handle + PQsetNoticeProcessorRVA);
                PQregisterThreadLock = (delegate*<pgthreadlock_t, pgthreadlock_t>)(Handle + PQregisterThreadLockRVA);
                PQexec = (delegate*<pg_conn, nint, pg_result>)(Handle + PQexecRVA);
                PQexecParams = (delegate*<pg_conn, nint, int, uint*, utf8string*, int*, int*, int, pg_result>)(Handle + PQexecParamsRVA);
                PQprepare = (delegate*<pg_conn, nint, nint, int, uint*, pg_result>)(Handle + PQprepareRVA);
                PQexecPrepared = (delegate*<pg_conn, nint, int, utf8string*, int*, int*, int, pg_result>)(Handle + PQexecPreparedRVA);
                PQsendQuery = (delegate*<pg_conn, nint, int>)(Handle + PQsendQueryRVA);
                PQsendQueryParams = (delegate*<pg_conn, nint, int, uint*, utf8string*, int*, int*, int, int>)(Handle + PQsendQueryParamsRVA);
                PQsendPrepare = (delegate*<pg_conn, nint, nint, int, uint*, int>)(Handle + PQsendPrepareRVA);
                PQsendQueryPrepared = (delegate*<pg_conn, nint, int, utf8string*, int*, int*, int, int>)(Handle + PQsendQueryPreparedRVA);
                PQsetSingleRowMode = (delegate*<pg_conn, int>)(Handle + PQsetSingleRowModeRVA);
                PQgetResult = (delegate*<pg_conn, pg_result>)(Handle + PQgetResultRVA);
                PQisBusy = (delegate*<pg_conn, int>)(Handle + PQisBusyRVA);
                PQconsumeInput = (delegate*<pg_conn, int>)(Handle + PQconsumeInputRVA);
                PQnotifies = (delegate*<pg_conn, pgNotify*>)(Handle + PQnotifiesRVA);
                PQputCopyData = (delegate*<pg_conn, nint, int, int>)(Handle + PQputCopyDataRVA);
                PQputCopyEnd = (delegate*<pg_conn, nint, int>)(Handle + PQputCopyEndRVA);
                PQgetCopyData = (delegate*<pg_conn, utf8string*, int, int>)(Handle + PQgetCopyDataRVA);
                PQgetline = (delegate*<pg_conn, nint, int, int>)(Handle + PQgetlineRVA);
                PQputline = (delegate*<pg_conn, nint, int>)(Handle + PQputlineRVA);
                PQgetlineAsync = (delegate*<pg_conn, nint, int, int>)(Handle + PQgetlineAsyncRVA);
                PQputnbytes = (delegate*<pg_conn, nint, int, int>)(Handle + PQputnbytesRVA);
                PQendcopy = (delegate*<pg_conn, int>)(Handle + PQendcopyRVA);
                PQsetnonblocking = (delegate*<pg_conn, int, int>)(Handle + PQsetnonblockingRVA);
                PQisnonblocking = (delegate*<pg_conn, int>)(Handle + PQisnonblockingRVA);
                PQisthreadsafe = (delegate*<int>)(Handle + PQisthreadsafeRVA);
                PQping = (delegate*<nint, PGPing>)(Handle + PQpingRVA);
                PQpingParams = (delegate*<utf8string*, utf8string*, int, PGPing>)(Handle + PQpingParamsRVA);
                PQflush = (delegate*<pg_conn, int>)(Handle + PQflushRVA);
                PQfn = (delegate*<pg_conn, int, int*, int*, int, in PQArgBlock, int, pg_result>)(Handle + PQfnRVA);
                PQresultStatus = (delegate*<pg_result, ExecStatusType>)(Handle + PQresultStatusRVA);
                PQresStatus = (delegate*<ExecStatusType, nint>)(Handle + PQresStatusRVA);
                PQresultErrorMessage = (delegate*<pg_result, nint>)(Handle + PQresultErrorMessageRVA);
                PQresultVerboseErrorMessage = (delegate*<pg_result, PGVerbosity, PGContextVisibility, nint>)(Handle + PQresultVerboseErrorMessageRVA);
                PQresultErrorField = (delegate*<pg_result, int, nint>)(Handle + PQresultErrorFieldRVA);
                PQntuples = (delegate*<pg_result, int>)(Handle + PQntuplesRVA);
                PQnfields = (delegate*<pg_result, int>)(Handle + PQnfieldsRVA);
                PQbinaryTuples = (delegate*<pg_result, int>)(Handle + PQbinaryTuplesRVA);
                PQfname = (delegate*<pg_result, int, nint>)(Handle + PQfnameRVA);
                PQfnumber = (delegate*<pg_result, nint, int>)(Handle + PQfnumberRVA);
                PQftable = (delegate*<pg_result, int, uint>)(Handle + PQftableRVA);
                PQftablecol = (delegate*<pg_result, int, int>)(Handle + PQftablecolRVA);
                PQfformat = (delegate*<pg_result, int, int>)(Handle + PQfformatRVA);
                PQftype = (delegate*<pg_result, int, uint>)(Handle + PQftypeRVA);
                PQfsize = (delegate*<pg_result, int, int>)(Handle + PQfsizeRVA);
                PQfmod = (delegate*<pg_result, int, int>)(Handle + PQfmodRVA);
                PQcmdStatus = (delegate*<pg_result, nint>)(Handle + PQcmdStatusRVA);
                PQoidStatus = (delegate*<pg_result, nint>)(Handle + PQoidStatusRVA);
                PQoidValue = (delegate*<pg_result, uint>)(Handle + PQoidValueRVA);
                PQcmdTuples = (delegate*<pg_result, nint>)(Handle + PQcmdTuplesRVA);
                PQgetvalue = (delegate*<pg_result, int, int, nint>)(Handle + PQgetvalueRVA);
                PQgetlength = (delegate*<pg_result, int, int, int>)(Handle + PQgetlengthRVA);
                PQgetisnull = (delegate*<pg_result, int, int, int>)(Handle + PQgetisnullRVA);
                PQnparams = (delegate*<pg_result, int>)(Handle + PQnparamsRVA);
                PQparamtype = (delegate*<pg_result, int, uint>)(Handle + PQparamtypeRVA);
                PQdescribePrepared = (delegate*<pg_conn, nint, pg_result>)(Handle + PQdescribePreparedRVA);
                PQdescribePortal = (delegate*<pg_conn, nint, pg_result>)(Handle + PQdescribePortalRVA);
                PQsendDescribePrepared = (delegate*<pg_conn, nint, int>)(Handle + PQsendDescribePreparedRVA);
                PQsendDescribePortal = (delegate*<pg_conn, nint, int>)(Handle + PQsendDescribePortalRVA);
                PQclear = (delegate*<pg_result, void>)(Handle + PQclearRVA);
                PQfreemem = (delegate*<nint, void>)(Handle + PQfreememRVA);
                PQmakeEmptyPGresult = (delegate*<pg_conn, ExecStatusType, pg_result>)(Handle + PQmakeEmptyPGresultRVA);
                PQcopyResult = (delegate*<pg_result, int, pg_result>)(Handle + PQcopyResultRVA);
                PQsetResultAttrs = (delegate*<pg_result, int, ref pgresAttDesc, int>)(Handle + PQsetResultAttrsRVA);
                PQresultAlloc = (delegate*<pg_result, ulong, nint>)(Handle + PQresultAllocRVA);
                PQresultMemorySize = (delegate*<pg_result, ulong>)(Handle + PQresultMemorySizeRVA);
                PQsetvalue = (delegate*<pg_result, int, int, nint, int, int>)(Handle + PQsetvalueRVA);
                PQescapeStringConn = (delegate*<pg_conn, nint, nint, ulong, int*, ulong>)(Handle + PQescapeStringConnRVA);
                PQescapeLiteral = (delegate*<pg_conn, nint, ulong, nint>)(Handle + PQescapeLiteralRVA);
                PQescapeIdentifier = (delegate*<pg_conn, nint, ulong, nint>)(Handle + PQescapeIdentifierRVA);
                PQescapeByteaConn = (delegate*<pg_conn, nint, ulong, out ulong, nint>)(Handle + PQescapeByteaConnRVA);
                PQunescapeBytea = (delegate*<nint, out ulong, nint>)(Handle + PQunescapeByteaRVA);
                PQescapeString = (delegate*<nint, nint, ulong, ulong>)(Handle + PQescapeStringRVA);
                PQescapeBytea = (delegate*<nint, ulong, ref ulong, nint>)(Handle + PQescapeByteaRVA);
                PQprint = (delegate*<ref _iobuf, pg_result, ref PQprintOpt, void>)(Handle + PQprintRVA);
                PQdisplayTuples = (delegate*<pg_result, ref _iobuf, int, nint, int, int, void>)(Handle + PQdisplayTuplesRVA);
                PQprintTuples = (delegate*<pg_result, ref _iobuf, int, int, int, void>)(Handle + PQprintTuplesRVA);
                lo_open = (delegate*<pg_conn, uint, int, int>)(Handle + lo_openRVA);
                lo_close = (delegate*<pg_conn, int, int>)(Handle + lo_closeRVA);
                lo_read = (delegate*<pg_conn, int, nint, ulong, int>)(Handle + lo_readRVA);
                lo_write = (delegate*<pg_conn, int, nint, ulong, int>)(Handle + lo_writeRVA);
                lo_lseek = (delegate*<pg_conn, int, int, int, int>)(Handle + lo_lseekRVA);
                lo_lseek64 = (delegate*<pg_conn, int, long, int, long>)(Handle + lo_lseek64RVA);
                lo_creat = (delegate*<pg_conn, int, uint>)(Handle + lo_creatRVA);
                lo_create = (delegate*<pg_conn, uint, uint>)(Handle + lo_createRVA);
                lo_tell = (delegate*<pg_conn, int, int>)(Handle + lo_tellRVA);
                lo_tell64 = (delegate*<pg_conn, int, long>)(Handle + lo_tell64RVA);
                lo_truncate = (delegate*<pg_conn, int, ulong, int>)(Handle + lo_truncateRVA);
                lo_truncate64 = (delegate*<pg_conn, int, long, int>)(Handle + lo_truncate64RVA);
                lo_unlink = (delegate*<pg_conn, uint, int>)(Handle + lo_unlinkRVA);
                lo_import = (delegate*<pg_conn, nint, uint>)(Handle + lo_importRVA);
                lo_import_with_oid = (delegate*<pg_conn, nint, uint, uint>)(Handle + lo_import_with_oidRVA);
                lo_export = (delegate*<pg_conn, uint, nint, int>)(Handle + lo_exportRVA);
                PQlibVersion = (delegate*<int>)(Handle + PQlibVersionRVA);
                PQmblen = (delegate*<nint, int, int>)(Handle + PQmblenRVA);
                PQdsplen = (delegate*<nint, int, int>)(Handle + PQdsplenRVA);
                PQenv2encoding = (delegate*<int>)(Handle + PQenv2encodingRVA);
                PQencryptPassword = (delegate*<nint, nint, nint>)(Handle + PQencryptPasswordRVA);
                PQencryptPasswordConn = (delegate*<pg_conn, nint, nint, nint, nint>)(Handle + PQencryptPasswordConnRVA);
                pg_char_to_encoding = (delegate*<nint, int>)(Handle + pg_char_to_encodingRVA);
                pg_encoding_to_char = (delegate*<int, nint>)(Handle + pg_encoding_to_charRVA);
                pg_valid_server_encoding_id = (delegate*<int, int>)(Handle + pg_valid_server_encoding_idRVA);
                PQgetSSLKeyPassHook_OpenSSL = (delegate*<PQsslKeyPassHook_OpenSSL_type>)(Handle + PQgetSSLKeyPassHook_OpenSSLRVA);
                PQsetSSLKeyPassHook_OpenSSL = (delegate*<PQsslKeyPassHook_OpenSSL_type, void>)(Handle + PQsetSSLKeyPassHook_OpenSSLRVA);
                PQdefaultSSLKeyPassHook_OpenSSL = (delegate*<nint, int, pg_conn, int>)(Handle + PQdefaultSSLKeyPassHook_OpenSSLRVA);
                PQregisterEventProc = (delegate*<pg_conn, PGEventProc, nint, nint, int>)(Handle + PQregisterEventProcRVA);
                PQsetInstanceData = (delegate*<pg_conn, PGEventProc, nint, int>)(Handle + PQsetInstanceDataRVA);
                PQinstanceData = (delegate*<pg_conn, PGEventProc, nint>)(Handle + PQinstanceDataRVA);
                PQresultSetInstanceData = (delegate*<pg_result, PGEventProc, nint, int>)(Handle + PQresultSetInstanceDataRVA);
                PQresultInstanceData = (delegate*<pg_result, PGEventProc, nint>)(Handle + PQresultInstanceDataRVA);
                PQfireResultCreateEvents = (delegate*<pg_conn, pg_result, int>)(Handle + PQfireResultCreateEventsRVA);


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

        public static unsafe delegate*<nint, pg_conn>                                                      PQconnectStart;
        public static unsafe delegate*<utf8string*, utf8string*, int, pg_conn>                             PQconnectStartParams;
        public static unsafe delegate*<pg_conn, PostgresPollingStatusType>                                 PQconnectPoll;
        public static unsafe delegate*<nint, pg_conn>                                                      PQconnectdb;
        public static unsafe delegate*<utf8string*, utf8string*, int, pg_conn>                             PQconnectdbParams;
        public static unsafe delegate*<nint, nint, nint, nint, nint, nint, nint, pg_conn>                  PQsetdbLogin;
        public static unsafe delegate*<pg_conn, void>                                                      PQfinish;
        public static unsafe delegate*<ref PQconninfoOption>                                               PQconndefaults;
        public static unsafe delegate*<nint, utf8string*, ref PQconninfoOption>                            PQconninfoParse;
        public static unsafe delegate*<pg_conn, ref PQconninfoOption>                                      PQconninfo;
        public static unsafe delegate*<ref PQconninfoOption, void>                                         PQconninfoFree;
        public static unsafe delegate*<pg_conn, int>                                                       PQresetStart;
        public static unsafe delegate*<pg_conn, PostgresPollingStatusType>                                 PQresetPoll;
        public static unsafe delegate*<pg_conn, void>                                                      PQreset;
        public static unsafe delegate*<pg_conn, pg_cancel>                                                 PQgetCancel;
        public static unsafe delegate*<pg_cancel, void>                                                    PQfreeCancel;
        public static unsafe delegate*<pg_cancel, nint, int, int>                                          PQcancel;
        public static unsafe delegate*<pg_conn, int>                                                       PQrequestCancel;
        public static unsafe delegate*<pg_conn, nint>                                                      PQdb;
        public static unsafe delegate*<pg_conn, nint>                                                      PQuser;
        public static unsafe delegate*<pg_conn, nint>                                                      PQpass;
        public static unsafe delegate*<pg_conn, nint>                                                      PQhost;
        public static unsafe delegate*<pg_conn, nint>                                                      PQhostaddr;
        public static unsafe delegate*<pg_conn, nint>                                                      PQport;
        public static unsafe delegate*<pg_conn, nint>                                                      PQtty;
        public static unsafe delegate*<pg_conn, nint>                                                      PQoptions;
        public static unsafe delegate*<pg_conn, ConnStatusType>                                            PQstatus;
        public static unsafe delegate*<pg_conn, PGTransactionStatusType>                                   PQtransactionStatus;
        public static unsafe delegate*<pg_conn, nint, nint>                                                PQparameterStatus;
        public static unsafe delegate*<pg_conn, int>                                                       PQprotocolVersion;
        public static unsafe delegate*<pg_conn, int>                                                       PQserverVersion;
        public static unsafe delegate*<pg_conn, nint>                                                      PQerrorMessage;
        public static unsafe delegate*<pg_conn, int>                                                       PQsocket;
        public static unsafe delegate*<pg_conn, int>                                                       PQbackendPID;
        public static unsafe delegate*<pg_conn, int>                                                       PQconnectionNeedsPassword;
        public static unsafe delegate*<pg_conn, int>                                                       PQconnectionUsedPassword;
        public static unsafe delegate*<pg_conn, int>                                                       PQclientEncoding;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQsetClientEncoding;
        public static unsafe delegate*<pg_conn, int>                                                       PQsslInUse;
        public static unsafe delegate*<pg_conn, nint, nint>                                                PQsslStruct;
        public static unsafe delegate*<pg_conn, nint, nint>                                                PQsslAttribute;
        public static unsafe delegate*<pg_conn, nint>                                                      PQsslAttributeNames;
        public static unsafe delegate*<pg_conn, nint>                                                      PQgetssl;
        public static unsafe delegate*<int, void>                                                          PQinitSSL;
        public static unsafe delegate*<int, int, void>                                                     PQinitOpenSSL;
        public static unsafe delegate*<pg_conn, int>                                                       PQgssEncInUse;
        public static unsafe delegate*<pg_conn, nint>                                                      PQgetgssctx;
        public static unsafe delegate*<pg_conn, PGVerbosity, PGVerbosity>                                  PQsetErrorVerbosity;
        public static unsafe delegate*<pg_conn, PGContextVisibility, PGContextVisibility>                  PQsetErrorContextVisibility;
        public static unsafe delegate*<pg_conn, ref _iobuf, void>                                          PQtrace;
        public static unsafe delegate*<pg_conn, void>                                                      PQuntrace;
        public static unsafe delegate*<pg_conn, PQnoticeReceiver, nint, PQnoticeReceiver>                  PQsetNoticeReceiver;
        public static unsafe delegate*<pg_conn, PQnoticeProcessor, nint, PQnoticeProcessor>                PQsetNoticeProcessor;
        public static unsafe delegate*<pgthreadlock_t, pgthreadlock_t>                                     PQregisterThreadLock;
        public static unsafe delegate*<pg_conn, nint, pg_result>                                           PQexec;
        public static unsafe delegate*<pg_conn, nint, int, uint*, utf8string*, int*, int*, int, pg_result> PQexecParams;
        public static unsafe delegate*<pg_conn, nint, nint, int, uint*, pg_result>                         PQprepare;
        public static unsafe delegate*<pg_conn, nint, int, utf8string*, int*, int*, int, pg_result>        PQexecPrepared;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQsendQuery;
        public static unsafe delegate*<pg_conn, nint, int, uint*, utf8string*, int*, int*, int, int>       PQsendQueryParams;
        public static unsafe delegate*<pg_conn, nint, nint, int, uint*, int>                               PQsendPrepare;
        public static unsafe delegate*<pg_conn, nint, int, utf8string*, int*, int*, int, int>              PQsendQueryPrepared;
        public static unsafe delegate*<pg_conn, int>                                                       PQsetSingleRowMode;
        public static unsafe delegate*<pg_conn, pg_result>                                                 PQgetResult;
        public static unsafe delegate*<pg_conn, int>                                                       PQisBusy;
        public static unsafe delegate*<pg_conn, int>                                                       PQconsumeInput;
        public static unsafe delegate*<pg_conn, pgNotify*>                                                 PQnotifies;
        public static unsafe delegate*<pg_conn, nint, int, int>                                            PQputCopyData;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQputCopyEnd;
        public static unsafe delegate*<pg_conn, utf8string*, int, int>                                     PQgetCopyData;
        public static unsafe delegate*<pg_conn, nint, int, int>                                            PQgetline;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQputline;
        public static unsafe delegate*<pg_conn, nint, int, int>                                            PQgetlineAsync;
        public static unsafe delegate*<pg_conn, nint, int, int>                                            PQputnbytes;
        public static unsafe delegate*<pg_conn, int>                                                       PQendcopy;
        public static unsafe delegate*<pg_conn, int, int>                                                  PQsetnonblocking;
        public static unsafe delegate*<pg_conn, int>                                                       PQisnonblocking;
        public static unsafe delegate*<int>                                                                PQisthreadsafe;
        public static unsafe delegate*<nint, PGPing>                                                       PQping;
        public static unsafe delegate*<utf8string*, utf8string*, int, PGPing>                              PQpingParams;
        public static unsafe delegate*<pg_conn, int>                                                       PQflush;
        public static unsafe delegate*<pg_conn, int, int*, int*, int, in PQArgBlock, int, pg_result>       PQfn;
        public static unsafe delegate*<pg_result, ExecStatusType>                                          PQresultStatus;
        public static unsafe delegate*<ExecStatusType, nint>                                               PQresStatus;
        public static unsafe delegate*<pg_result, nint>                                                    PQresultErrorMessage;
        public static unsafe delegate*<pg_result, PGVerbosity, PGContextVisibility, nint>                  PQresultVerboseErrorMessage;
        public static unsafe delegate*<pg_result, int, nint>                                               PQresultErrorField;
        public static unsafe delegate*<pg_result, int>                                                     PQntuples;
        public static unsafe delegate*<pg_result, int>                                                     PQnfields;
        public static unsafe delegate*<pg_result, int>                                                     PQbinaryTuples;
        public static unsafe delegate*<pg_result, int, nint>                                               PQfname;
        public static unsafe delegate*<pg_result, nint, int>                                               PQfnumber;
        public static unsafe delegate*<pg_result, int, uint>                                               PQftable;
        public static unsafe delegate*<pg_result, int, int>                                                PQftablecol;
        public static unsafe delegate*<pg_result, int, int>                                                PQfformat;
        public static unsafe delegate*<pg_result, int, uint>                                               PQftype;
        public static unsafe delegate*<pg_result, int, int>                                                PQfsize;
        public static unsafe delegate*<pg_result, int, int>                                                PQfmod;
        public static unsafe delegate*<pg_result, nint>                                                    PQcmdStatus;
        public static unsafe delegate*<pg_result, nint>                                                    PQoidStatus;
        public static unsafe delegate*<pg_result, uint>                                                    PQoidValue;
        public static unsafe delegate*<pg_result, nint>                                                    PQcmdTuples;
        public static unsafe delegate*<pg_result, int, int, nint>                                          PQgetvalue;
        public static unsafe delegate*<pg_result, int, int, int>                                           PQgetlength;
        public static unsafe delegate*<pg_result, int, int, int>                                           PQgetisnull;
        public static unsafe delegate*<pg_result, int>                                                     PQnparams;
        public static unsafe delegate*<pg_result, int, uint>                                               PQparamtype;
        public static unsafe delegate*<pg_conn, nint, pg_result>                                           PQdescribePrepared;
        public static unsafe delegate*<pg_conn, nint, pg_result>                                           PQdescribePortal;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQsendDescribePrepared;
        public static unsafe delegate*<pg_conn, nint, int>                                                 PQsendDescribePortal;
        public static unsafe delegate*<pg_result, void>                                                    PQclear;
        public static unsafe delegate*<nint, void>                                                         PQfreemem;
        public static unsafe delegate*<pg_conn, ExecStatusType, pg_result>                                 PQmakeEmptyPGresult;
        public static unsafe delegate*<pg_result, int, pg_result>                                          PQcopyResult;
        public static unsafe delegate*<pg_result, int, ref pgresAttDesc, int>                              PQsetResultAttrs;
        public static unsafe delegate*<pg_result, ulong, nint>                                             PQresultAlloc;
        public static unsafe delegate*<pg_result, ulong>                                                   PQresultMemorySize;
        public static unsafe delegate*<pg_result, int, int, nint, int, int>                                PQsetvalue;
        public static unsafe delegate*<pg_conn, nint, nint, ulong, int*, ulong>                            PQescapeStringConn;
        public static unsafe delegate*<pg_conn, nint, ulong, nint>                                         PQescapeLiteral;
        public static unsafe delegate*<pg_conn, nint, ulong, nint>                                         PQescapeIdentifier;
        public static unsafe delegate*<pg_conn, nint, ulong, out ulong, nint>                              PQescapeByteaConn;
        public static unsafe delegate*<nint, out ulong, nint>                                              PQunescapeBytea;
        public static unsafe delegate*<nint, nint, ulong, ulong>                                           PQescapeString;
        public static unsafe delegate*<nint, ulong, ref ulong, nint>                                       PQescapeBytea;
        public static unsafe delegate*<ref _iobuf, pg_result, ref PQprintOpt, void>                        PQprint;
        public static unsafe delegate*<pg_result, ref _iobuf, int, nint, int, int, void>                   PQdisplayTuples;
        public static unsafe delegate*<pg_result, ref _iobuf, int, int, int, void>                         PQprintTuples;
        public static unsafe delegate*<pg_conn, uint, int, int>                                            lo_open;
        public static unsafe delegate*<pg_conn, int, int>                                                  lo_close;
        public static unsafe delegate*<pg_conn, int, nint, ulong, int>                                     lo_read;
        public static unsafe delegate*<pg_conn, int, nint, ulong, int>                                     lo_write;
        public static unsafe delegate*<pg_conn, int, int, int, int>                                        lo_lseek;
        public static unsafe delegate*<pg_conn, int, long, int, long>                                      lo_lseek64;
        public static unsafe delegate*<pg_conn, int, uint>                                                 lo_creat;
        public static unsafe delegate*<pg_conn, uint, uint>                                                lo_create;
        public static unsafe delegate*<pg_conn, int, int>                                                  lo_tell;
        public static unsafe delegate*<pg_conn, int, long>                                                 lo_tell64;
        public static unsafe delegate*<pg_conn, int, ulong, int>                                           lo_truncate;
        public static unsafe delegate*<pg_conn, int, long, int>                                            lo_truncate64;
        public static unsafe delegate*<pg_conn, uint, int>                                                 lo_unlink;
        public static unsafe delegate*<pg_conn, nint, uint>                                                lo_import;
        public static unsafe delegate*<pg_conn, nint, uint, uint>                                          lo_import_with_oid;
        public static unsafe delegate*<pg_conn, uint, nint, int>                                           lo_export;
        public static unsafe delegate*<int>                                                                PQlibVersion;
        public static unsafe delegate*<nint, int, int>                                                     PQmblen;
        public static unsafe delegate*<nint, int, int>                                                     PQdsplen;
        public static unsafe delegate*<int>                                                                PQenv2encoding;
        public static unsafe delegate*<nint, nint, nint>                                                   PQencryptPassword;
        public static unsafe delegate*<pg_conn, nint, nint, nint, nint>                                    PQencryptPasswordConn;
        public static unsafe delegate*<nint, int>                                                          pg_char_to_encoding;
        public static unsafe delegate*<int, nint>                                                          pg_encoding_to_char;
        public static unsafe delegate*<int, int>                                                           pg_valid_server_encoding_id;
        public static unsafe delegate*<PQsslKeyPassHook_OpenSSL_type>                                      PQgetSSLKeyPassHook_OpenSSL;
        public static unsafe delegate*<PQsslKeyPassHook_OpenSSL_type, void>                                PQsetSSLKeyPassHook_OpenSSL;
        public static unsafe delegate*<nint, int, pg_conn, int>                                            PQdefaultSSLKeyPassHook_OpenSSL;
        public static unsafe delegate*<pg_conn, PGEventProc, nint, nint, int>                              PQregisterEventProc;
        public static unsafe delegate*<pg_conn, PGEventProc, nint, int>                                    PQsetInstanceData;
        public static unsafe delegate*<pg_conn, PGEventProc, nint>                                         PQinstanceData;
        public static unsafe delegate*<pg_result, PGEventProc, nint, int>                                  PQresultSetInstanceData;
        public static unsafe delegate*<pg_result, PGEventProc, nint>                                       PQresultInstanceData;
        public static unsafe delegate*<pg_conn, pg_result, int>                                            PQfireResultCreateEvents;

    }
}
