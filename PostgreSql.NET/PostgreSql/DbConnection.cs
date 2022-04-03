using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using Apache.Thrift.Database;

using Timer = System.Timers.Timer;
using TRowDescriptor = System.Collections.Generic.List<Apache.Thrift.Database.TColumnType>;
using TTableDescriptor = System.Collections.Generic.Dictionary<string, Apache.Thrift.Database.TColumnType>;

namespace PostgreSql
{
    public sealed class PostgreSqlUser
    {
        public Timer Timer { get; }

        public Guid Uid { get; }

        public string Name { get; }

        public string Password { get; }

        public string DbName { get; }

        public pg_conn Connection { get; }

        public PostgreSqlUser(string              name,
                              string              password,
                              string              dbName,
                              pg_conn             connection,
                              ElapsedEventHandler disconnectUser)
        {
            Uid = Guid.NewGuid();

            Name     = name;
            Password = password;
            DbName   = dbName;

            Connection = connection;

            Timer = new Timer
            {
                Interval = 1 * 60 * 1000
            };

            Timer.Elapsed += disconnectUser;
        }

        ~PostgreSqlUser()
        {
            if(Connection.Handle != 0)
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            NativeLibrary.PQfinish(Connection);
        }
    }

    public class DbConnection
    {
        //private const string hostName = "timothyrmcnealy.com"; //"timothyrmcnealy.com"; //"127.0.0.1"; //"localhost"; //
        //private const int    hostport = 15432;

        private static readonly Func<string, int, string, string, string, string> connectionString = (host,
                                                                                                      port,
                                                                                                      user,
                                                                                                      passwd,
                                                                                                      dbname) => $"host={host} port={port} user={user} password={passwd} dbname={dbname}";
        //$"http://{user}:{passwd}@{host}:{port}/{dbname}";

        private readonly Dictionary<Guid, PostgreSqlUser> _users = new Dictionary<Guid, PostgreSqlUser>();

        public DbConnection()
        {
        }

        private void DisconnectUser(object           sender,
                                    ElapsedEventArgs e)
        {
            if(sender is PostgreSqlUser user)
            {
                user.Timer.Stop();

                _users.Remove(user.Uid);

                NativeLibrary.PQfinish(user.Connection);

                //Logger.LogInformation($"user {user.Name} has been disconnected.");
            }
        }

        public async Task<string> connectAsync(string            hostName,
                                               int               hostport,
                                               string            user,
                                               string            passwd,
                                               string            dbname,
                                               CancellationToken cancellationToken = default)
        {
            //Logger.LogInformation($"user {user} trying to connect to {dbname} with {passwd}");

            pg_conn? conn;

            try
            {
                conn = NativeLibrary.PQconnectdb(connectionString(hostName, hostport, user, passwd, dbname));

                if(NativeLibrary.PQstatus(conn.Value) == ConnStatusType.CONNECTION_BAD)
                {
                    throw new Exception(Marshal.PtrToStringAnsi(NativeLibrary.PQerrorMessage(conn.Value)));
                }
            }
            catch(Exception e)
            {
                await Console.Error.WriteLineAsync(e.ToString());

                return await Task.FromResult(Guid.Empty.ToString());
            }

            PostgreSqlUser postgreSqlUser = new PostgreSqlUser(user, passwd, dbname, conn.Value, DisconnectUser);

            _users.Add(postgreSqlUser.Uid, postgreSqlUser);

            //Logger.LogInformation($"user {postgreSqlUser.Name} connecting to {postgreSqlUser.DbName} with {postgreSqlUser.Password}");

            return await Task.FromResult(postgreSqlUser.Uid.ToString());
        }

        public async Task disconnectAsync(string            session,
                                          CancellationToken cancellationToken = default)
        {
            if(!_users.TryGetValue(Guid.Parse(session), out PostgreSqlUser postgreSqlUser))
            {
                await Task.FromResult(Task.CompletedTask);
            }

            //Logger.LogInformation($"disconnecting {session}");

            NativeLibrary.PQfinish(postgreSqlUser!.Connection);
        }

        public async Task<TQueryResult> sql_executeAsync(string            session,
                                                         string            query,
                                                         bool              column_format,
                                                         string            nonce,
                                                         int               first_n,
                                                         int               at_most_n,
                                                         CancellationToken cancellationToken = default)
        {
            //Logger.LogInformation($"Sql query: [{query}]");

            if(!_users.TryGetValue(Guid.Parse(session), out PostgreSqlUser postgreSqlUser))
            {
                return await Task.FromResult(new TQueryResult());
            }

            //Logger.LogInformation($"user {postgreSqlUser.Uid} query = {query}");

            pg_result res = NativeLibrary.PQexec(postgreSqlUser.Connection, query);

            int nFields = NativeLibrary.PQnfields(res);

            TQueryResult results = new TQueryResult
            {
                Row_set = new TRowSet
                {
                    Row_desc    = new TRowDescriptor(nFields),
                    Rows        = new List<TRow>(),
                    Columns     = new List<TColumn>(),
                    Is_columnar = column_format
                },
                Execution_time_ms = 0,
                Total_time_ms     = 0,
                Nonce             = nonce,
                Debug             = ""
            };

            for(int i = 0; i < nFields; ++i)
            {
                string     colName  = NativeLibrary.PQfname(res, i);
                SQLType    col_type = NativeLibrary.GetSqlType(NativeLibrary.OidTypes[NativeLibrary.PQftype(res, i)]);
                TDatumType colType  = SqlToThrift.GetDatumType(col_type, out int size);

                //Console.Write($"{colName} ");

                TColumnType projInfo = new TColumnType
                {
                    Col_name = colName,
                    Col_type = new TTypeInfo
                    {
                        Type       = colType,
                        Nullable   = true,
                        Is_array   = false,
                        Encoding   = TEncodingType.NONE,
                        Precision  = 0,
                        Scale      = 0,
                        Comp_param = 0,
                        Size       = -1
                    },
                    Is_reserved_keyword = false,
                    Src_name            = "",
                    Is_system           = false,
                    Is_physical         = false,
                    Col_id              = 0
                };

                results.Row_set.Row_desc.Add(projInfo);

                results.Row_set.Columns.Add(new TColumn
                {
                    Data = new TColumnData
                    {
                        Int_col  = new List<long>(),
                        Real_col = new List<double>(),
                        Str_col  = new List<string>(),
                        Arr_col  = new List<TColumn>()
                    },
                    Nulls = new List<bool>()
                });
            }

            for(int j = 0; j < NativeLibrary.PQntuples(res); j++)
            {
                SQLType col_type;
                bool    is_null;

                for(int i = 0; i < nFields; ++i)
                {
                    //col_text = raw.sqlite3_column_text(stmt, i).utf8_to_string();

                    col_type = NativeLibrary.GetSqlType(NativeLibrary.OidTypes[NativeLibrary.PQftype(res, i)]);

                    is_null = NativeLibrary.PQgetisnull(res, j, i) != 0;

                    //Console.Write($"{NativeLibrary.PQgetvalue(res, j, i)} ");

                    switch(col_type)
                    {
                        case SQLType.BOOLEAN:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(NativeLibrary.PQgetvalue(res, j, i) == "true" ? 1 : 0);
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.MinValue);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.TINYINT:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(sbyte.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.MinValue);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.SMALLINT:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(short.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.MinValue);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.INT:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.MinValue);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.BIGINT:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(long.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Int_col.Add(int.MinValue);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.FLOAT:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Real_col.Add(float.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Real_col.Add(float.NaN);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.DOUBLE:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Real_col.Add(double.Parse(NativeLibrary.PQgetvalue(res, j, i)));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Real_col.Add(double.NaN);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.CHAR:
                        case SQLType.TEXT:
                        case SQLType.VARCHAR:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Str_col.Add(NativeLibrary.PQgetvalue(res, j, i));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Str_col.Add(string.Empty);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        case SQLType.DATE:
                        case SQLType.TIME:
                        {
                            if(!is_null)
                            {
                                results.Row_set.Columns[i].Data.Str_col.Add(NativeLibrary.PQgetvalue(res, j, i));
                                results.Row_set.Columns[i].Nulls.Add(false);
                            }
                            else
                            {
                                results.Row_set.Columns[i].Data.Str_col.Add(string.Empty);
                                results.Row_set.Columns[i].Nulls.Add(true);
                            }

                            break;
                        }
                        default:
                        {
                            throw new NotSupportedException(col_type.ToString());
                        }
                    }

                    //char      GetChar(int ordinal)
                    //DateTime  GetDateTime(int      ordinal)
                    //Decimal   GetDecimal(int       ordinal)
                    //double    GetDouble(int        ordinal)
                    //float     GetFloat(int         ordinal)
                    //Guid      GetGuid(int          ordinal)
                    //short     GetInt16(int         ordinal)
                    //int       GetInt32(int         ordinal)
                    //long      GetInt64(int         ordinal)
                    //string    GetString(int        ordinal)
                    //long      GetBytes(int         ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
                    //long      GetChars(int         ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
                    //Stream    GetStream(int        ordinal)
                    //T         GetFieldValue<T>(int ordinal)
                    //object    GetValue(int         ordinal)
                    //int       GetValues(object[]   values)
                    //DataTable GetSchemaTable()
                }

                //Logger.LogInformation();
            }

            return results;
        }
    }
}
