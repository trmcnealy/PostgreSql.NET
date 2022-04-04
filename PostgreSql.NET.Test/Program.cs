using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

using Microsoft.Data.Analysis;

using PlatformApi;

using static PostgreSql.NativeLibrary;

namespace PostgreSql.NET.Test
{
    unsafe class Program
    {

        private static readonly Func<string, int, string, string, string, string> connectionString = (host,
                                                                                                      port,
                                                                                                      user,
                                                                                                      passwd,
                                                                                                      dbname) => $"host={host} port={port} user={user} password={passwd} dbname={dbname}";
        static void Main(string[] args)
        {
            const string default_host   = "timothyrmcnealy.com";
            const int    default_port   = 15432;
            const string default_user   = "db_user";
            const string default_passwd = "dbAccess";
            const string default_dbname = "OilGas";


            string connectionStr = connectionString(default_host, default_port, default_user, default_passwd, default_dbname);

            utf8cstring mbString = new(connectionString(default_host, default_port, default_user, default_passwd, default_dbname));

            pg_conn conn = PQconnectdb(mbString);

            if(PQstatus(conn) == ConnStatusType.CONNECTION_BAD)
            {
                throw new Exception(PQerrorMessage(conn).ToString());
            }

            utf8cstring query = new("SELECT \"Id\", \"ClusterPerTreatmentCount\", \"EndDate\", \"LateralLength\", \"MaxProppantConcentration\", \"ProppantMesh\", \"ProppantType\", \"ReservoirName\", \"StartDate\", \"TreatmentCount\", \"WellId\"\nFROM \"CompletionDetails\"\nWHERE \"WellId\" = 3397;");

            pg_result res = PQexec(conn, query);
            
            if(PQresultStatus(res) == ExecStatusType.PGRES_BAD_RESPONSE || PQresultStatus(res) == ExecStatusType.PGRES_NONFATAL_ERROR || PQresultStatus(res) == ExecStatusType.PGRES_FATAL_ERROR)
            {
                throw new Exception(PQresultErrorMessage(res).ToString());
            }

            DataTable df = ResultAsDataTable(res);

            for(int j = 0; j < df.Rows.Count; j++)
            {
                for(int i = 0; i < df.Columns.Count; ++i)
                {
                    Console.Write(df.Rows[j][i]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            PQfinish(conn);

#if DEBUG
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
#endif
        }
       
        
        private const           string      DateTimeFormat         = "yyyy-MM-dd HH:mm:ss";
        private static readonly CultureInfo DateTimeFormatProvider = CultureInfo.CurrentCulture;

        public static DataTable ResultAsDataTable(pg_result results)
        {
            unsafe
            {
                pg_result res = results;

                int nFields = PQnfields(res);
                int nRows   = PQntuples(res);

                string  colName;
                OidKind col_type;
                bool    is_null;

                DataTable dt = new();

                for(int i = 0; i < nFields; ++i)
                {
                    colName  = MultiByteString.GetString(PQfname(res, i));
                    col_type = OidTypes[PQftype(res, i)];

                    switch(col_type)
                    {
                        case OidKind.BOOLOID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(bool)));

                            break;
                        }
                        case OidKind.BYTEAOID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(sbyte)));

                            break;
                        }
                        case OidKind.INT2OID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(short)));

                            break;
                        }
                        case OidKind.INT4OID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(int)));

                            break;
                        }
                        case OidKind.INT8OID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(long)));

                            break;
                        }
                        case OidKind.FLOAT4OID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(float)));

                            break;
                        }
                        case OidKind.FLOAT8OID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(double)));

                            break;
                        }
                        case OidKind.CHAROID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(char)));

                            break;
                        }
                        case OidKind.VARCHAROID:
                        case OidKind.TEXTOID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(string)));

                            break;
                        }
                        case OidKind.TIMEOID:
                        case OidKind.TIMESTAMPOID:
                        case OidKind.DATEOID:
                        {
                            dt.Columns.Add(new DataColumn(colName, typeof(DateTime)));

                            break;
                        }
                        default:
                        {
                            throw new NotSupportedException(col_type.ToString());
                        }
                    }
                }

                DataRow dataRow;

                for(int j = 0; j < nRows; j++)
                {
                    dataRow = dt.NewRow();

                    for(int i = 0; i < nFields; ++i)
                    {
                        col_type = OidTypes[PQftype(res, i)];

                        is_null = PQgetisnull(res, j, i) != 0;

                        if(is_null)
                        {
                            dataRow[i] = DBNull.Value;
                            continue;
                        }

                        sbyte* newValue = PQgetvalue(res, j, i);

                        switch(col_type)
                        {
                            case OidKind.BOOLOID:
                            {
                                dataRow[i] = PQBinaryReader.ReadBoolean(newValue);
                                break;
                            }
                            case OidKind.BYTEAOID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadSByte(newValue));
                                break;
                            }
                            case OidKind.INT2OID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadInt16(newValue));
                                break;
                            }
                            case OidKind.INT4OID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadInt32(newValue));
                                break;
                            }
                            case OidKind.INT8OID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadInt64(newValue));
                                break;
                            }
                            case OidKind.FLOAT4OID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadSingle(newValue));
                                break;
                            }
                            case OidKind.FLOAT8OID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadDouble(newValue));
                                break;
                            }
                            case OidKind.CHAROID:
                            {
                                dataRow[i] = (PQBinaryReader.ReadChar(newValue));
                                break;
                            }
                            case OidKind.VARCHAROID:
                            case OidKind.TEXTOID:
                            {
                                dataRow[i] = PQBinaryReader.ReadString(newValue).ToString();
                                break;
                            }
                            case OidKind.TIMEOID:
                            case OidKind.TIMESTAMPOID:
                            case OidKind.DATEOID:
                            {
                                dataRow[i] = DateTime.ParseExact(PQBinaryReader.ReadString(newValue).ToString(), DateTimeFormat, DateTimeFormatProvider);
                                break;
                            }
                            default:
                            {
                                throw new NotSupportedException(col_type.ToString());
                            }
                        }
                    }

                    dt.Rows.Add(dataRow);
                }

                dt.AcceptChanges();

                return dt;
            }
        }

        
        public static DataFrame ResultAsDataFrame(pg_result results)
        {
            string  colName;
            OidKind col_type;
            bool    is_null;

            pg_result res = results;

            int nFields = PQnfields(res);
            int nRows   = PQntuples(res);

            List<DataFrameColumn> columns = new(nFields);

            for(int i = 0; i < nFields; ++i)
            {
                colName  = MultiByteString.GetString(PQfname(res, i));
                col_type = OidTypes[PQftype(res, i)];

                switch(col_type)
                {
                    case OidKind.BOOLOID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<bool>(colName, nRows));

                        break;
                    }
                    case OidKind.BYTEAOID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<sbyte>(colName, nRows));

                        break;
                    }
                    case OidKind.INT2OID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<short>(colName, nRows));

                        break;
                    }
                    case OidKind.INT4OID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<int>(colName, nRows));

                        break;
                    }
                    case OidKind.INT8OID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<long>(colName, nRows));

                        break;
                    }
                    case OidKind.FLOAT4OID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<float>(colName, nRows));

                        break;
                    }
                    case OidKind.FLOAT8OID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<double>(colName, nRows));

                        break;
                    }
                    case OidKind.CHAROID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<char>(colName, nRows));

                        break;
                    }
                    case OidKind.VARCHAROID:
                    case OidKind.TEXTOID:
                    {
                        columns.Add(new StringDataFrameColumn(colName, nRows));

                        break;
                    }
                    case OidKind.TIMEOID:
                    case OidKind.TIMESTAMPOID:
                    case OidKind.DATEOID:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<DateTime>(colName, nRows));

                        break;
                    }
                    default:
                    {
                        throw new NotSupportedException(col_type.ToString());
                    }
                }
            }

            for(int j = 0; j < nRows; j++)
            {
                for(int i = 0; i < nFields; ++i)
                {
                    col_type = OidTypes[PQftype(res, i)];

                    is_null = PQgetisnull(res, j, i) != 0;

                    switch(col_type)
                    {
                        case OidKind.BOOLOID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = MultiByteString.GetString(PQgetvalue(res, j, i)) == "true";
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.BYTEAOID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = sbyte.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.INT2OID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = short.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.INT4OID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = int.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.INT8OID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = long.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.FLOAT4OID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = float.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.FLOAT8OID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = double.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.CHAROID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = char.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.VARCHAROID:
                        case OidKind.TEXTOID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = MultiByteString.GetString(PQgetvalue(res, j, i));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        case OidKind.TIMEOID:
                        case OidKind.TIMESTAMPOID:
                        case OidKind.DATEOID:
                        {
                            if(!is_null)
                            {
                                columns[i][j] = DateTime.Parse(MultiByteString.GetString(PQgetvalue(res, j, i)));
                            }
                            else
                            {
                                columns[i][j] = null;
                            }

                            break;
                        }
                        default:
                        {
                            throw new NotSupportedException(col_type.ToString());
                        }
                    }
                }
            }

            return new DataFrame(columns);
        }
    }
}
