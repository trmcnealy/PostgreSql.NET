using System;
using System.Collections.Generic;

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

            MultiByteString mbString = new(connectionString(default_host, default_port, default_user, default_passwd, default_dbname));

            pg_conn conn = PQconnectdb(mbString.GetPointer());

            if(PQstatus(conn) == ConnStatusType.CONNECTION_BAD)
            {
                throw new Exception(MultiByteString.GetString(PQerrorMessage(conn)));
            }

            MultiByteString query = new("SELECT * \nFROM \"WellListView\";");

            pg_result res = PQexec(conn, query.GetPointer());
            
            if(PQresultStatus(res) == ExecStatusType.PGRES_BAD_RESPONSE || PQresultStatus(res) == ExecStatusType.PGRES_NONFATAL_ERROR || PQresultStatus(res) == ExecStatusType.PGRES_FATAL_ERROR)
            {
                throw new Exception(MultiByteString.GetString(PQresultErrorMessage(res)));
            }

            DataFrame df = ResultAsDataFrame(res);

            for(int j = 0; j < 10; j++)
            {
                for(int i = 0; i < df.Columns.Count; ++i)
                {
                    Console.Write(df[j,i]);
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
