using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Apache.Thrift;

using System.ServiceModel;
using System.Runtime.Serialization;

using Apache.Thrift.Database;
using Apache.Thrift.Protocol;
using Apache.Thrift.Protocol.Entities;
using Apache.Thrift.Protocol.Utilities;
using Apache.Thrift.Transport;
using Apache.Thrift.Processor;


namespace PostgreSql
{
    [DataContract]
    public class TContractFaultException
    {
        [DataMember]
        public string Error_msg { get; set; }
    }

    public partial class ConnectionProcessor
    {
        [ServiceContract]
        public interface IAsync
        {
            [OperationContract]
            [FaultContract(typeof(TContractFaultException))]
            Task<string> connectAsync(string            user,
                                      string            passwd,
                                      string            dbname,
                                      CancellationToken cancellationToken = default(CancellationToken));

            [OperationContract]
            [FaultContract(typeof(TContractFaultException))]
            Task disconnectAsync(string            session,
                                 CancellationToken cancellationToken = default(CancellationToken));

            [OperationContract]
            [FaultContract(typeof(TContractFaultException))]
            Task<TQueryResult> sql_executeAsync(string            session,
                                                string            query,
                                                bool              column_format,
                                                string            nonce,
                                                int               first_n,
                                                int               at_most_n,
                                                CancellationToken cancellationToken = default(CancellationToken));
        }

        public class Client : TBaseClient, IDisposable, IAsync
        {
            public Client(TProtocol protocol)
                : this(protocol,
                       protocol)
            {
            }

            public Client(TProtocol inputProtocol,
                          TProtocol outputProtocol)
                : base(inputProtocol,
                       outputProtocol)
            {
            }

            public async Task<string> connectAsync(string            user,
                                                   string            passwd,
                                                   string            dbname,
                                                   CancellationToken cancellationToken = default(CancellationToken))
            {
                await OutputProtocol.WriteMessageBeginAsync(new TMessage("connect",
                                                                         TMessageType.Call,
                                                                         SeqId),
                                                            cancellationToken);

                connectArgs args = new connectArgs
                {
                    User   = user,
                    Passwd = passwd,
                    Dbname = dbname
                };

                await args.WriteAsync(OutputProtocol,
                                      cancellationToken);

                await OutputProtocol.WriteMessageEndAsync(cancellationToken);
                await OutputProtocol.Transport.FlushAsync(cancellationToken);

                TMessage msg = await InputProtocol.ReadMessageBeginAsync(cancellationToken);

                if(msg.Type == TMessageType.Exception)
                {
                    TApplicationException x = await TApplicationException.ReadAsync(InputProtocol,
                                                                                    cancellationToken);

                    await InputProtocol.ReadMessageEndAsync(cancellationToken);

                    throw x;
                }

                connectResult result = new connectResult();

                await result.ReadAsync(InputProtocol,
                                       cancellationToken);

                await InputProtocol.ReadMessageEndAsync(cancellationToken);

                if(result.isset.success)
                {
                    return result.Success;
                }

                if(result.isset.e)
                {
                    throw result.E;
                }

                throw new TApplicationException(TApplicationException.ExceptionType.MissingResult,
                                                "connect failed: unknown result");
            }

            public async Task disconnectAsync(string            session,
                                              CancellationToken cancellationToken = default(CancellationToken))
            {
                await OutputProtocol.WriteMessageBeginAsync(new TMessage("disconnect",
                                                                         TMessageType.Call,
                                                                         SeqId),
                                                            cancellationToken);

                disconnectArgs args = new disconnectArgs();
                args.Session = session;

                await args.WriteAsync(OutputProtocol,
                                      cancellationToken);

                await OutputProtocol.WriteMessageEndAsync(cancellationToken);
                await OutputProtocol.Transport.FlushAsync(cancellationToken);

                TMessage msg = await InputProtocol.ReadMessageBeginAsync(cancellationToken);

                if(msg.Type == TMessageType.Exception)
                {
                    TApplicationException x = await TApplicationException.ReadAsync(InputProtocol,
                                                                                    cancellationToken);

                    await InputProtocol.ReadMessageEndAsync(cancellationToken);

                    throw x;
                }

                disconnectResult result = new disconnectResult();

                await result.ReadAsync(InputProtocol,
                                       cancellationToken);

                await InputProtocol.ReadMessageEndAsync(cancellationToken);

                if(result.isset.e)
                {
                    throw result.E;
                }
            }

            public async Task<TQueryResult> sql_executeAsync(string            session,
                                                             string            query,
                                                             bool              column_format,
                                                             string            nonce,
                                                             int               first_n,
                                                             int               at_most_n,
                                                             CancellationToken cancellationToken = default(CancellationToken))
            {
                await OutputProtocol.WriteMessageBeginAsync(new TMessage("sql_execute",
                                                                         TMessageType.Call,
                                                                         SeqId),
                                                            cancellationToken);

                sql_executeArgs args = new sql_executeArgs
                {
                    Session       = session,
                    Query         = query,
                    Column_format = column_format,
                    Nonce         = nonce,
                    First_n       = first_n,
                    At_most_n     = at_most_n
                };

                await args.WriteAsync(OutputProtocol,
                                      cancellationToken);

                await OutputProtocol.WriteMessageEndAsync(cancellationToken);
                await OutputProtocol.Transport.FlushAsync(cancellationToken);

                TMessage msg = await InputProtocol.ReadMessageBeginAsync(cancellationToken);

                if(msg.Type == TMessageType.Exception)
                {
                    TApplicationException x = await TApplicationException.ReadAsync(InputProtocol,
                                                                                    cancellationToken);

                    await InputProtocol.ReadMessageEndAsync(cancellationToken);

                    throw x;
                }

                sql_executeResult result = new sql_executeResult();

                await result.ReadAsync(InputProtocol,
                                       cancellationToken);

                await InputProtocol.ReadMessageEndAsync(cancellationToken);

                if(result.isset.success)
                {
                    return result.Success;
                }

                if(result.isset.e)
                {
                    throw result.E;
                }

                throw new TApplicationException(TApplicationException.ExceptionType.MissingResult,
                                                "sql_execute failed: unknown result");
            }
        }

        public class AsyncProcessor : ITAsyncProcessor
        {
            private readonly IAsync _iAsync;

            public AsyncProcessor(IAsync iAsync)
            {
                _iAsync = iAsync ?? throw new ArgumentNullException(nameof(iAsync));

                ProcessMap["connect"]     = connect_ProcessAsync;
                ProcessMap["disconnect"]  = disconnect_ProcessAsync;
                ProcessMap["sql_execute"] = sql_execute_ProcessAsync;
            }

            protected delegate Task ProcessFunction(int               seqid,
                                                    TProtocol         iprot,
                                                    TProtocol         oprot,
                                                    CancellationToken cancellationToken);

            protected Dictionary<string, ProcessFunction> ProcessMap = new Dictionary<string, ProcessFunction>();

            public async Task<bool> ProcessAsync(TProtocol iprot,
                                                 TProtocol oprot)
            {
                return await ProcessAsync(iprot,
                                          oprot,
                                          CancellationToken.None);
            }

            public async Task<bool> ProcessAsync(TProtocol         iprot,
                                                 TProtocol         oprot,
                                                 CancellationToken cancellationToken)
            {
                try
                {
                    TMessage msg = await iprot.ReadMessageBeginAsync(cancellationToken);

                    ProcessMap.TryGetValue(msg.Name,
                                           out ProcessFunction? fn);

                    if(fn == null)
                    {
                        await TProtocolUtil.SkipAsync(iprot,
                                                      TType.Struct,
                                                      cancellationToken);

                        await iprot.ReadMessageEndAsync(cancellationToken);

                        TApplicationException x = new TApplicationException(TApplicationException.ExceptionType.UnknownMethod,
                                                                            "Invalid method name: '" + msg.Name + "'");

                        await oprot.WriteMessageBeginAsync(new TMessage(msg.Name,
                                                                        TMessageType.Exception,
                                                                        msg.SeqID),
                                                           cancellationToken);

                        await x.WriteAsync(oprot,
                                           cancellationToken);

                        await oprot.WriteMessageEndAsync(cancellationToken);
                        await oprot.Transport.FlushAsync(cancellationToken);

                        return true;
                    }

                    await fn(msg.SeqID,
                             iprot,
                             oprot,
                             cancellationToken);
                }
                catch(IOException e)
                {
                    Console.Error.WriteLine(e);
                    return false;
                }

                return true;
            }

            public async Task connect_ProcessAsync(int               seqid,
                                                   TProtocol         iprot,
                                                   TProtocol         oprot,
                                                   CancellationToken cancellationToken)
            {
                connectArgs args = new connectArgs();

                await args.ReadAsync(iprot,
                                     cancellationToken);

                await iprot.ReadMessageEndAsync(cancellationToken);
                connectResult result = new connectResult();

                try
                {
                    try
                    {
                        result.Success = await _iAsync.connectAsync(args.User,
                                                                    args.Passwd,
                                                                    args.Dbname,
                                                                    cancellationToken);
                    }
                    catch(TDatabaseException e)
                    {
                        result.E = e;
                    }

                    await oprot.WriteMessageBeginAsync(new TMessage("connect",
                                                                    TMessageType.Reply,
                                                                    seqid),
                                                       cancellationToken);

                    await result.WriteAsync(oprot,
                                            cancellationToken);
                }
                catch(TTransportException)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Error occurred in processor:");
                    Console.Error.WriteLine(ex.ToString());

                    TApplicationException x = new TApplicationException(TApplicationException.ExceptionType.InternalError,
                                                                        " Internal error.");

                    await oprot.WriteMessageBeginAsync(new TMessage("connect",
                                                                    TMessageType.Exception,
                                                                    seqid),
                                                       cancellationToken);

                    await x.WriteAsync(oprot,
                                       cancellationToken);
                }

                await oprot.WriteMessageEndAsync(cancellationToken);
                await oprot.Transport.FlushAsync(cancellationToken);
            }

            public async Task disconnect_ProcessAsync(int               seqid,
                                                      TProtocol         iprot,
                                                      TProtocol         oprot,
                                                      CancellationToken cancellationToken)
            {
                disconnectArgs args = new disconnectArgs();

                await args.ReadAsync(iprot,
                                     cancellationToken);

                await iprot.ReadMessageEndAsync(cancellationToken);
                disconnectResult result = new disconnectResult();

                try
                {
                    try
                    {
                        await _iAsync.disconnectAsync(args.Session,
                                                      cancellationToken);
                    }
                    catch(TDatabaseException e)
                    {
                        result.E = e;
                    }

                    await oprot.WriteMessageBeginAsync(new TMessage("disconnect",
                                                                    TMessageType.Reply,
                                                                    seqid),
                                                       cancellationToken);

                    await result.WriteAsync(oprot,
                                            cancellationToken);
                }
                catch(TTransportException)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Error occurred in processor:");
                    Console.Error.WriteLine(ex.ToString());

                    TApplicationException x = new TApplicationException(TApplicationException.ExceptionType.InternalError,
                                                                        " Internal error.");

                    await oprot.WriteMessageBeginAsync(new TMessage("disconnect",
                                                                    TMessageType.Exception,
                                                                    seqid),
                                                       cancellationToken);

                    await x.WriteAsync(oprot,
                                       cancellationToken);
                }

                await oprot.WriteMessageEndAsync(cancellationToken);
                await oprot.Transport.FlushAsync(cancellationToken);
            }

            public async Task sql_execute_ProcessAsync(int               seqid,
                                                       TProtocol         iprot,
                                                       TProtocol         oprot,
                                                       CancellationToken cancellationToken)
            {
                sql_executeArgs args = new sql_executeArgs();

                await args.ReadAsync(iprot,
                                     cancellationToken);

                await iprot.ReadMessageEndAsync(cancellationToken);
                sql_executeResult result = new sql_executeResult();

                try
                {
                    try
                    {
                        result.Success = await _iAsync.sql_executeAsync(args.Session,
                                                                        args.Query,
                                                                        args.Column_format,
                                                                        args.Nonce,
                                                                        args.First_n,
                                                                        args.At_most_n,
                                                                        cancellationToken);
                    }
                    catch(TDatabaseException e)
                    {
                        result.E = e;
                    }

                    await oprot.WriteMessageBeginAsync(new TMessage("sql_execute",
                                                                    TMessageType.Reply,
                                                                    seqid),
                                                       cancellationToken);

                    await result.WriteAsync(oprot,
                                            cancellationToken);
                }
                catch(TTransportException)
                {
                    throw;
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Error occurred in processor:");
                    Console.Error.WriteLine(ex.ToString());

                    TApplicationException x = new TApplicationException(TApplicationException.ExceptionType.InternalError,
                                                                        " Internal error.");

                    await oprot.WriteMessageBeginAsync(new TMessage("sql_execute",
                                                                    TMessageType.Exception,
                                                                    seqid),
                                                       cancellationToken);

                    await x.WriteAsync(oprot,
                                       cancellationToken);
                }

                await oprot.WriteMessageEndAsync(cancellationToken);
                await oprot.Transport.FlushAsync(cancellationToken);
            }
        }
    }
}
