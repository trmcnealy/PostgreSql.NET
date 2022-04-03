using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Apache.Thrift;

using System.ServiceModel;
using System.Runtime.Serialization;
using System.Text;

using Apache.Thrift.Database;
using Apache.Thrift.Protocol;
using Apache.Thrift.Protocol.Entities;
using Apache.Thrift.Protocol.Utilities;
using Apache.Thrift.Transport;
using Apache.Thrift.Processor;

namespace PostgreSql
{
    public partial class ConnectionProcessor
    {
        [DataContract]
        public class connectArgs : TBase
        {
            private string _user;
            private string _passwd;
            private string _dbname;

            [DataMember]
            public string User
            {
                get { return _user; }
                set
                {
                    isset.user = true;
                    _user      = value;
                }
            }

            [DataMember]
            public string Passwd
            {
                get { return _passwd; }
                set
                {
                    isset.passwd = true;
                    _passwd      = value;
                }
            }

            [DataMember]
            public string Dbname
            {
                get { return _dbname; }
                set
                {
                    isset.dbname = true;
                    _dbname      = value;
                }
            }

            [DataMember]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool user;

                [DataMember]
                public bool passwd;

                [DataMember]
                public bool dbname;
            }

            #region XmlSerializer support

            public bool ShouldSerializeUser()
            {
                return isset.user;
            }

            public bool ShouldSerializePasswd()
            {
                return isset.passwd;
            }

            public bool ShouldSerializeDbname()
            {
                return isset.dbname;
            }

            #endregion XmlSerializer support

            public connectArgs()
            {
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 1:
                                if(field.Type == TType.String)
                                {
                                    User = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 2:
                                if(field.Type == TType.String)
                                {
                                    Passwd = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 3:
                                if(field.Type == TType.String)
                                {
                                    Dbname = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("connect_args");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(User != null && isset.user)
                    {
                        field.Name = "user";
                        field.Type = TType.String;
                        field.ID   = 1;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(User, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(Passwd != null && isset.passwd)
                    {
                        field.Name = "passwd";
                        field.Type = TType.String;
                        field.ID   = 2;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Passwd, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(Dbname != null && isset.dbname)
                    {
                        field.Name = "dbname";
                        field.Type = TType.String;
                        field.ID   = 3;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Dbname, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                connectArgs? other = that as connectArgs;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.user   == other.isset.user)   && ((!isset.user)   || (System.Object.Equals(User,   other.User))))   &&
                       ((isset.passwd == other.isset.passwd) && ((!isset.passwd) || (System.Object.Equals(Passwd, other.Passwd)))) &&
                       ((isset.dbname == other.isset.dbname) && ((!isset.dbname) || (System.Object.Equals(Dbname, other.Dbname))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.user)
                        hashcode = (hashcode * 397) + User.GetHashCode();

                    if(isset.passwd)
                        hashcode = (hashcode * 397) + Passwd.GetHashCode();

                    if(isset.dbname)
                        hashcode = (hashcode * 397) + Dbname.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("connect_args(");
                bool          first = true;

                if(User != null && isset.user)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("User: ");
                    sb.Append(User);
                }

                if(Passwd != null && isset.passwd)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Passwd: ");
                    sb.Append(Passwd);
                }

                if(Dbname != null && isset.dbname)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Dbname: ");
                    sb.Append(Dbname);
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        [DataContract]
        public class connectResult : TBase
        {
            private string             _success;
            private TDatabaseException _e;

            [DataMember]
            public string Success
            {
                get { return _success; }
                set
                {
                    isset.success = true;
                    _success      = value;
                }
            }

            [DataMember]
            public TDatabaseException E
            {
                get { return _e; }
                set
                {
                    isset.e = true;
                    _e      = value;
                }
            }

            [DataMember]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool success;

                [DataMember]
                public bool e;
            }

            #region XmlSerializer support

            public bool ShouldSerializeSuccess()
            {
                return isset.success;
            }

            public bool ShouldSerializeE()
            {
                return isset.e;
            }

            #endregion XmlSerializer support

            public connectResult()
            {
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 0:
                                if(field.Type == TType.String)
                                {
                                    Success = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 1:
                                if(field.Type == TType.Struct)
                                {
                                    E = new TDatabaseException();

                                    await E.ReadAsync(iprot, cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("connect_result");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(isset.success)
                    {
                        if(Success != null)
                        {
                            field.Name = "Success";
                            field.Type = TType.String;
                            field.ID   = 0;

                            await oprot.WriteFieldBeginAsync(field, cancellationToken);

                            await oprot.WriteStringAsync(Success, cancellationToken);

                            await oprot.WriteFieldEndAsync(cancellationToken);
                        }
                    }
                    else if(isset.e)
                    {
                        if(E != null)
                        {
                            field.Name = "E";
                            field.Type = TType.Struct;
                            field.ID   = 1;

                            await oprot.WriteFieldBeginAsync(field, cancellationToken);

                            await E.WriteAsync(oprot, cancellationToken);

                            await oprot.WriteFieldEndAsync(cancellationToken);
                        }
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                connectResult? other = that as connectResult;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.success == other.isset.success) && ((!isset.success) || (System.Object.Equals(Success, other.Success)))) &&
                       ((isset.e       == other.isset.e)       && ((!isset.e)       || (System.Object.Equals(E,       other.E))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.success)
                        hashcode = (hashcode * 397) + Success.GetHashCode();

                    if(isset.e)
                        hashcode = (hashcode * 397) + E.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("connect_result(");
                bool          first = true;

                if(Success != null && isset.success)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Success: ");
                    sb.Append(Success);
                }

                if(E != null && isset.e)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("E: ");
                    sb.Append(E == null ? "<null>" : E.ToString());
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        [DataContract]
        public class disconnectArgs : TBase
        {
            private string _session;

            [DataMember]
            public string Session
            {
                get { return _session; }
                set
                {
                    isset.session = true;
                    _session      = value;
                }
            }

            [DataMember]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool session;
            }

            #region XmlSerializer support

            public bool ShouldSerializeSession()
            {
                return isset.session;
            }

            #endregion XmlSerializer support

            public disconnectArgs()
            {
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 1:
                                if(field.Type == TType.String)
                                {
                                    Session = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("disconnect_args");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(Session != null && isset.session)
                    {
                        field.Name = "session";
                        field.Type = TType.String;
                        field.ID   = 1;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Session, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                disconnectArgs? other = that as disconnectArgs;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.session == other.isset.session) && ((!isset.session) || (System.Object.Equals(Session, other.Session))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.session)
                        hashcode = (hashcode * 397) + Session.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("disconnect_args(");
                bool          first = true;

                if(Session != null && isset.session)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Session: ");
                    sb.Append(Session);
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        [DataContract]
        public class disconnectResult : TBase
        {
            private TDatabaseException _e;

            [DataMember]
            public TDatabaseException E
            {
                get { return _e; }
                set
                {
                    isset.e = true;
                    _e      = value;
                }
            }

            [DataMember]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool e;
            }

            #region XmlSerializer support

            public bool ShouldSerializeE()
            {
                return isset.e;
            }

            #endregion XmlSerializer support

            public disconnectResult()
            {
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 1:
                                if(field.Type == TType.Struct)
                                {
                                    E = new TDatabaseException();

                                    await E.ReadAsync(iprot, cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("disconnect_result");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(isset.e)
                    {
                        if(E != null)
                        {
                            field.Name = "E";
                            field.Type = TType.Struct;
                            field.ID   = 1;

                            await oprot.WriteFieldBeginAsync(field, cancellationToken);

                            await E.WriteAsync(oprot, cancellationToken);

                            await oprot.WriteFieldEndAsync(cancellationToken);
                        }
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                disconnectResult? other = that as disconnectResult;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.e == other.isset.e) && ((!isset.e) || (System.Object.Equals(E, other.E))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.e)
                        hashcode = (hashcode * 397) + E.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("disconnect_result(");
                bool          first = true;

                if(E != null && isset.e)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("E: ");
                    sb.Append(E == null ? "<null>" : E.ToString());
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        [DataContract(Namespace = "")]
        public class sql_executeArgs : TBase
        {
            private string _session;
            private string _query;
            private bool   _column_format;
            private string _nonce;
            private int    _first_n;
            private int    _at_most_n;

            [DataMember(Order = 0)]
            public string Session
            {
                get { return _session; }
                set
                {
                    isset.session = true;
                    _session      = value;
                }
            }

            [DataMember(Order = 0)]
            public string Query
            {
                get { return _query; }
                set
                {
                    isset.query = true;
                    _query      = value;
                }
            }

            [DataMember(Order = 0)]
            public bool Column_format
            {
                get { return _column_format; }
                set
                {
                    isset.column_format = true;
                    _column_format      = value;
                }
            }

            [DataMember(Order = 0)]
            public string Nonce
            {
                get { return _nonce; }
                set
                {
                    isset.nonce = true;
                    _nonce      = value;
                }
            }

            [DataMember(Order = 0)]
            public int First_n
            {
                get { return _first_n; }
                set
                {
                    isset.first_n = true;
                    _first_n      = value;
                }
            }

            [DataMember(Order = 0)]
            public int At_most_n
            {
                get { return _at_most_n; }
                set
                {
                    isset.at_most_n = true;
                    _at_most_n      = value;
                }
            }

            [DataMember(Order = 1)]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool session;

                [DataMember]
                public bool query;

                [DataMember]
                public bool column_format;

                [DataMember]
                public bool nonce;

                [DataMember]
                public bool first_n;

                [DataMember]
                public bool at_most_n;
            }

            #region XmlSerializer support

            public bool ShouldSerializeSession()
            {
                return isset.session;
            }

            public bool ShouldSerializeQuery()
            {
                return isset.query;
            }

            public bool ShouldSerializeColumn_format()
            {
                return isset.column_format;
            }

            public bool ShouldSerializeNonce()
            {
                return isset.nonce;
            }

            public bool ShouldSerializeFirst_n()
            {
                return isset.first_n;
            }

            public bool ShouldSerializeAt_most_n()
            {
                return isset.at_most_n;
            }

            #endregion XmlSerializer support

            public sql_executeArgs()
            {
                _first_n        = -1;
                isset.first_n   = true;
                _at_most_n      = -1;
                isset.at_most_n = true;
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 1:
                                if(field.Type == TType.String)
                                {
                                    Session = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 2:
                                if(field.Type == TType.String)
                                {
                                    Query = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 3:
                                if(field.Type == TType.Bool)
                                {
                                    Column_format = await iprot.ReadBoolAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 4:
                                if(field.Type == TType.String)
                                {
                                    Nonce = await iprot.ReadStringAsync(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 5:
                                if(field.Type == TType.I32)
                                {
                                    First_n = await iprot.ReadI32Async(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 6:
                                if(field.Type == TType.I32)
                                {
                                    At_most_n = await iprot.ReadI32Async(cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("sql_execute_args");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(Session != null && isset.session)
                    {
                        field.Name = "session";
                        field.Type = TType.String;
                        field.ID   = 1;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Session, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(Query != null && isset.query)
                    {
                        field.Name = "query";
                        field.Type = TType.String;
                        field.ID   = 2;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Query, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(isset.column_format)
                    {
                        field.Name = "column_format";
                        field.Type = TType.Bool;
                        field.ID   = 3;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteBoolAsync(Column_format, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(Nonce != null && isset.nonce)
                    {
                        field.Name = "nonce";
                        field.Type = TType.String;
                        field.ID   = 4;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteStringAsync(Nonce, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(isset.first_n)
                    {
                        field.Name = "first_n";
                        field.Type = TType.I32;
                        field.ID   = 5;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteI32Async(First_n, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    if(isset.at_most_n)
                    {
                        field.Name = "at_most_n";
                        field.Type = TType.I32;
                        field.ID   = 6;

                        await oprot.WriteFieldBeginAsync(field, cancellationToken);

                        await oprot.WriteI32Async(At_most_n, cancellationToken);

                        await oprot.WriteFieldEndAsync(cancellationToken);
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                sql_executeArgs? other = that as sql_executeArgs;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.session       == other.isset.session)       && ((!isset.session)       || (System.Object.Equals(Session,       other.Session))))       &&
                       ((isset.query         == other.isset.query)         && ((!isset.query)         || (System.Object.Equals(Query,         other.Query))))         &&
                       ((isset.column_format == other.isset.column_format) && ((!isset.column_format) || (System.Object.Equals(Column_format, other.Column_format)))) &&
                       ((isset.nonce         == other.isset.nonce)         && ((!isset.nonce)         || (System.Object.Equals(Nonce,         other.Nonce))))         &&
                       ((isset.first_n       == other.isset.first_n)       && ((!isset.first_n)       || (System.Object.Equals(First_n,       other.First_n))))       &&
                       ((isset.at_most_n     == other.isset.at_most_n)     && ((!isset.at_most_n)     || (System.Object.Equals(At_most_n,     other.At_most_n))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.session)
                        hashcode = (hashcode * 397) + Session.GetHashCode();

                    if(isset.query)
                        hashcode = (hashcode * 397) + Query.GetHashCode();

                    if(isset.column_format)
                        hashcode = (hashcode * 397) + Column_format.GetHashCode();

                    if(isset.nonce)
                        hashcode = (hashcode * 397) + Nonce.GetHashCode();

                    if(isset.first_n)
                        hashcode = (hashcode * 397) + First_n.GetHashCode();

                    if(isset.at_most_n)
                        hashcode = (hashcode * 397) + At_most_n.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("sql_execute_args(");
                bool          first = true;

                if(Session != null && isset.session)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Session: ");
                    sb.Append(Session);
                }

                if(Query != null && isset.query)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Query: ");
                    sb.Append(Query);
                }

                if(isset.column_format)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Column_format: ");
                    sb.Append(Column_format);
                }

                if(Nonce != null && isset.nonce)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Nonce: ");
                    sb.Append(Nonce);
                }

                if(isset.first_n)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("First_n: ");
                    sb.Append(First_n);
                }

                if(isset.at_most_n)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("At_most_n: ");
                    sb.Append(At_most_n);
                }

                sb.Append(")");

                return sb.ToString();
            }
        }

        [DataContract(Namespace = "")]
        public class sql_executeResult : TBase
        {
            private TQueryResult       _success;
            private TDatabaseException _e;

            [DataMember(Order = 0)]
            public TQueryResult Success
            {
                get { return _success; }
                set
                {
                    isset.success = true;
                    _success      = value;
                }
            }

            [DataMember]
            public TDatabaseException E
            {
                get { return _e; }
                set
                {
                    isset.e = true;
                    _e      = value;
                }
            }

            [DataMember]
            public Isset isset;

            [DataContract]
            public struct Isset
            {
                [DataMember]
                public bool success;

                [DataMember]
                public bool e;
            }

            #region XmlSerializer support

            public bool ShouldSerializeSuccess()
            {
                return isset.success;
            }

            public bool ShouldSerializeE()
            {
                return isset.e;
            }

            #endregion XmlSerializer support

            public sql_executeResult()
            {
            }

            public async Task ReadAsync(TProtocol         iprot,
                                        CancellationToken cancellationToken)
            {
                iprot.IncrementRecursionDepth();

                try
                {
                    TField field;
                    await iprot.ReadStructBeginAsync(cancellationToken);

                    while(true)
                    {
                        field = await iprot.ReadFieldBeginAsync(cancellationToken);

                        if(field.Type == TType.Stop)
                        {
                            break;
                        }

                        switch(field.ID)
                        {
                            case 0:
                                if(field.Type == TType.Struct)
                                {
                                    Success = new TQueryResult();

                                    await Success.ReadAsync(iprot, cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            case 1:
                                if(field.Type == TType.Struct)
                                {
                                    E = new TDatabaseException();

                                    await E.ReadAsync(iprot, cancellationToken);
                                }
                                else
                                {
                                    await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
                                }

                                break;
                            default:
                                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);

                                break;
                        }

                        await iprot.ReadFieldEndAsync(cancellationToken);
                    }

                    await iprot.ReadStructEndAsync(cancellationToken);
                }
                finally
                {
                    iprot.DecrementRecursionDepth();
                }
            }

            public async Task WriteAsync(TProtocol         oprot,
                                         CancellationToken cancellationToken)
            {
                oprot.IncrementRecursionDepth();

                try
                {
                    TStruct struc = new TStruct("sql_execute_result");

                    await oprot.WriteStructBeginAsync(struc, cancellationToken);

                    TField field = new TField();

                    if(isset.success)
                    {
                        if(Success != null)
                        {
                            field.Name = "Success";
                            field.Type = TType.Struct;
                            field.ID   = 0;

                            await oprot.WriteFieldBeginAsync(field, cancellationToken);

                            await Success.WriteAsync(oprot, cancellationToken);

                            await oprot.WriteFieldEndAsync(cancellationToken);
                        }
                    }
                    else if(isset.e)
                    {
                        if(E != null)
                        {
                            field.Name = "E";
                            field.Type = TType.Struct;
                            field.ID   = 1;

                            await oprot.WriteFieldBeginAsync(field, cancellationToken);

                            await E.WriteAsync(oprot, cancellationToken);

                            await oprot.WriteFieldEndAsync(cancellationToken);
                        }
                    }

                    await oprot.WriteFieldStopAsync(cancellationToken);
                    await oprot.WriteStructEndAsync(cancellationToken);
                }
                finally
                {
                    oprot.DecrementRecursionDepth();
                }
            }

            public override bool Equals(object? that)
            {
                sql_executeResult? other = that as sql_executeResult;

                if(other == null)
                    return false;

                if(ReferenceEquals(this, other))
                    return true;

                return ((isset.success == other.isset.success) && ((!isset.success) || (System.Object.Equals(Success, other.Success)))) &&
                       ((isset.e       == other.isset.e)       && ((!isset.e)       || (Object.Equals(E, other.E))));
            }

            public override int GetHashCode()
            {
                int hashcode = 157;

                unchecked
                {
                    if(isset.success)
                        hashcode = (hashcode * 397) + Success.GetHashCode();

                    if(isset.e)
                        hashcode = (hashcode * 397) + E.GetHashCode();
                }

                return hashcode;
            }

            public override string ToString()
            {
                StringBuilder sb    = new StringBuilder("sql_execute_result(");
                bool          first = true;

                if(Success != null && isset.success)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("Success: ");
                    sb.Append(Success == null ? "<null>" : Success.ToString());
                }

                if(E != null && isset.e)
                {
                    if(!first)
                    {
                        sb.Append(", ");
                    }

                    first = false;
                    sb.Append("E: ");
                    sb.Append(E == null ? "<null>" : E.ToString());
                }

                sb.Append(")");

                return sb.ToString();
            }
        }
    }
}
