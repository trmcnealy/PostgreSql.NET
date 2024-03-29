﻿namespace PostgreSql
{
    public enum OidKind
    {
        BOOL               = 16,
        BYTEA              = 17,
        CHAR               = 18,
        NAME               = 19,
        INT8               = 20,
        INT2               = 21,
        INT2VECTOR         = 22,
        INT4               = 23,
        REGPROC            = 24,
        TEXT               = 25,
        OID                = 26,
        TID                = 27,
        XID                = 28,
        CID                = 29,
        OIDVECTOR          = 30,
        JSON               = 114,
        XML                = 142,
        PGNODETREE         = 194,
        PGNDISTINCT        = 3361,
        PGDEPENDENCIES     = 3402,
        PGMCVLIST          = 5017,
        PGDDLCOMMAND       = 32,
        POINT              = 600,
        LSEG               = 601,
        PATH               = 602,
        BOX                = 603,
        POLYGON            = 604,
        LINE               = 628,
        FLOAT4             = 700,
        FLOAT8             = 701,
        UNKNOWN            = 705,
        CIRCLE             = 718,
        CASH               = 790,
        MACADDR            = 829,
        INET               = 869,
        CIDR               = 650,
        MACADDR8           = 774,
        ACLITEM            = 1033,
        BPCHAR             = 1042,
        VARCHAR            = 1043,
        DATE               = 1082,
        TIME               = 1083,
        TIMESTAMP          = 1114,
        TIMESTAMPTZ        = 1184,
        INTERVAL           = 1186,
        TIMETZ             = 1266,
        BIT                = 1560,
        VARBIT             = 1562,
        NUMERIC            = 1700,
        REFCURSOR          = 1790,
        REGPROCEDURE       = 2202,
        REGOPER            = 2203,
        REGOPERATOR        = 2204,
        REGCLASS           = 2205,
        REGTYPE            = 2206,
        REGROLE            = 4096,
        REGNAMESPACE       = 4089,
        UUID               = 2950,
        LSN                = 3220,
        TSVECTOR           = 3614,
        GTSVECTOR          = 3642,
        TSQUERY            = 3615,
        REGCONFIG          = 3734,
        REGDICTIONARY      = 3769,
        JSONB              = 3802,
        JSONPATH           = 4072,
        TXID_SNAPSHOT      = 2970,
        INT4RANGE          = 3904,
        NUMRANGE           = 3906,
        TSRANGE            = 3908,
        TSTZRANGE          = 3910,
        DATERANGE          = 3912,
        INT8RANGE          = 3926,
        RECORD             = 2249,
        RECORDARRAY        = 2287,
        CSTRING            = 2275,
        ANY                = 2276,
        ANYARRAY           = 2277,
        VOID               = 2278,
        TRIGGER            = 2279,
        EVTTRIGGER         = 3838,
        LANGUAGE_HANDLER   = 2280,
        INTERNAL           = 2281,
        OPAQUE             = 2282,
        ANYELEMENT         = 2283,
        ANYNONARRAY        = 2776,
        ANYENUM            = 3500,
        FDW_HANDLER        = 3115,
        INDEX_AM_HANDLER   = 325,
        TSM_HANDLER        = 3310,
        TABLE_AM_HANDLER   = 269,
        ANYRANGE           = 3831,
        BOOLARRAY          = 1000,
        BYTEAARRAY         = 1001,
        CHARARRAY          = 1002,
        NAMEARRAY          = 1003,
        INT8ARRAY          = 1016,
        INT2ARRAY          = 1005,
        INT2VECTORARRAY    = 1006,
        INT4ARRAY          = 1007,
        REGPROCARRAY       = 1008,
        TEXTARRAY          = 1009,
        OIDARRAY           = 1028,
        TIDARRAY           = 1010,
        XIDARRAY           = 1011,
        CIDARRAY           = 1012,
        OIDVECTORARRAY     = 1013,
        JSONARRAY          = 199,
        XMLARRAY           = 143,
        POINTARRAY         = 1017,
        LSEGARRAY          = 1018,
        PATHARRAY          = 1019,
        BOXARRAY           = 1020,
        POLYGONARRAY       = 1027,
        LINEARRAY          = 629,
        FLOAT4ARRAY        = 1021,
        FLOAT8ARRAY        = 1022,
        CIRCLEARRAY        = 719,
        MONEYARRAY         = 791,
        MACADDRARRAY       = 1040,
        INETARRAY          = 1041,
        CIDRARRAY          = 651,
        MACADDR8ARRAY      = 775,
        ACLITEMARRAY       = 1034,
        BPCHARARRAY        = 1014,
        VARCHARARRAY       = 1015,
        DATEARRAY          = 1182,
        TIMEARRAY          = 1183,
        TIMESTAMPARRAY     = 1115,
        TIMESTAMPTZARRAY   = 1185,
        INTERVALARRAY      = 1187,
        TIMETZARRAY        = 1270,
        BITARRAY           = 1561,
        VARBITARRAY        = 1563,
        NUMERICARRAY       = 1231,
        REFCURSORARRAY     = 2201,
        REGPROCEDUREARRAY  = 2207,
        REGOPERARRAY       = 2208,
        REGOPERATORARRAY   = 2209,
        REGCLASSARRAY      = 2210,
        REGTYPEARRAY       = 2211,
        REGROLEARRAY       = 4097,
        REGNAMESPACEARRAY  = 4090,
        UUIDARRAY          = 2951,
        PG_LSNARRAY        = 3221,
        TSVECTORARRAY      = 3643,
        GTSVECTORARRAY     = 3644,
        TSQUERYARRAY       = 3645,
        REGCONFIGARRAY     = 3735,
        REGDICTIONARYARRAY = 3770,
        JSONBARRAY         = 3807,
        JSONPATHARRAY      = 4073,
        TXID_SNAPSHOTARRAY = 2949,
        INT4RANGEARRAY     = 3905,
        NUMRANGEARRAY      = 3907,
        TSRANGEARRAY       = 3909,
        TSTZRANGEARRAY     = 3911,
        DATERANGEARRAY     = 3913,
        INT8RANGEARRAY     = 3927,
        CSTRINGARRAY       = 1263
    }
}
