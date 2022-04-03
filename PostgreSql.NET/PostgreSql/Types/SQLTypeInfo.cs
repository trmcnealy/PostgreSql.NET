using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PostgreSql
{
    [UnsafeValueType]
    [StructLayout(LayoutKind.Explicit,
#if X86
                  Size = (sizeof(uint)) + sizeof(uint)
#else
                  Size = (sizeof(uint) * 2) + sizeof(ulong)
#endif
                 )]
    public struct VarlenDatum
    {
        [FieldOffset(0)]
        public uint length;

#if X86
        [FieldOffset(sizeof(uint))]
#else
        [FieldOffset(sizeof(uint) * 2)]
#endif
        public nint pointer;

        public bool is_null
        {
            get { return pointer == 0; }
        }

        //public VarlenDatum()
        //{
        //    length  = 0;
        //    pointer = null;
        //    is_null = true;
        //}

        public VarlenDatum(uint l,
                           nint p)
        {
            length  = l;
            pointer = p;
        }
    }

    [StructLayout(LayoutKind.Explicit,
#if X86
                  Size = sizeof(uint) * 2
#else
                  Size = sizeof(ulong) * 2
#endif
                 )]
    public struct Datum
    {
        [FieldOffset(0)]
        public bool boolval;

        [FieldOffset(0)]
        public sbyte tinyintval;

        [FieldOffset(0)]
        public short smallintval;

        [FieldOffset(0)]
        public int intval;

        [FieldOffset(0)]
        public long bigintval;

        [FieldOffset(0)]
        public float floatval;

        [FieldOffset(0)]
        public double doubleval;

        [FieldOffset(0)]
        public VarlenDatum arrayval;

        [FieldOffset(0)]
        public unsafe sbyte* stringval;
    }

    public class SQLTypeInfo : IEquatable<SQLTypeInfo>
    {
        public static bool IS_INTEGER(SQLType sqlType)
        {
            return sqlType == SQLType.INT || sqlType == SQLType.SMALLINT || sqlType == SQLType.BIGINT || sqlType == SQLType.TINYINT;
        }

        public static bool IS_NUMBER(SQLType sqlType)
        {
            return sqlType == SQLType.INT      ||
                   sqlType == SQLType.SMALLINT ||
                   sqlType == SQLType.DOUBLE   ||
                   sqlType == SQLType.FLOAT    ||
                   sqlType == SQLType.BIGINT   ||
                   sqlType == SQLType.NUMERIC  ||
                   sqlType == SQLType.DECIMAL  ||
                   sqlType == SQLType.TINYINT;
        }

        public static bool IS_STRING(SQLType sqlType)
        {
            return sqlType == SQLType.TEXT || sqlType == SQLType.VARCHAR || sqlType == SQLType.CHAR;
        }

        public static bool IS_GEO(SQLType sqlType)
        {
            return sqlType == SQLType.POINT || sqlType == SQLType.LINESTRING || sqlType == SQLType.POLYGON || sqlType == SQLType.MULTIPOLYGON;
        }

        public static bool IS_INTERVAL(SQLType sqlType)
        {
            return sqlType == SQLType.INTERVAL_DAY_TIME || sqlType == SQLType.INTERVAL_YEAR_MONTH;
        }

        public static bool IS_DECIMAL(SQLType sqlType)
        {
            return sqlType == SQLType.NUMERIC || sqlType == SQLType.DECIMAL;
        }

        public static bool IS_GEO_POLY(SQLType sqlType)
        {
            return sqlType == SQLType.POLYGON || sqlType == SQLType.MULTIPOLYGON;
        }

        public static bool IS_DATETIME(SQLType sqlType)
        {
            return sqlType == SQLType.TIME || sqlType == SQLType.TIMESTAMP || sqlType == SQLType.DATE;
        }

        public static int TRANSIENT_DICT(int id)
        {
            return -id;
        }

        public SQLTypeInfo(SQLType      t,
                           int          d,
                           int          s,
                           bool         n,
                           EncodingType c,
                           int          p,
                           SQLType      st)
        {
            _type       = t;
            subtype     = st;
            dimension   = d;
            scale       = s;
            notnull     = n;
            compression = c;
            comp_param  = p;
            size        = get_storage_size();
        }

        public SQLTypeInfo(SQLType t,
                           int     d,
                           int     s,
                           bool    n)
        {
            _type       = t;
            subtype     = SQLType.NULLT;
            dimension   = d;
            scale       = s;
            notnull     = n;
            compression = EncodingType.ENCODING_NONE;
            comp_param  = 0;
            size        = get_storage_size();
        }

        public SQLTypeInfo(SQLType t,
                           int     d,
                           int     s)
            : this(t, d, s, false)
        {
        }

        public SQLTypeInfo(SQLType t,
                           bool    n)
        {
            _type       = t;
            subtype     = SQLType.NULLT;
            dimension   = 0;
            scale       = 0;
            notnull     = n;
            compression = EncodingType.ENCODING_NONE;
            comp_param  = 0;
            size        = get_storage_size();
        }

        public SQLTypeInfo(SQLType t)
            : this(t, false)
        {
        }

        public SQLTypeInfo(SQLType      t,
                           bool         n,
                           EncodingType c)
        {
            _type       = t;
            subtype     = SQLType.NULLT;
            dimension   = 0;
            scale       = 0;
            notnull     = n;
            compression = c;
            comp_param  = 0;
            size        = get_storage_size();
        }

        public SQLTypeInfo()
        {
            _type       = SQLType.NULLT;
            subtype     = SQLType.NULLT;
            dimension   = 0;
            scale       = 0;
            notnull     = false;
            compression = EncodingType.ENCODING_NONE;
            comp_param  = 0;
            size        = 0;
        }

        public SQLType get_type()
        {
            return _type;
        }

        public SQLType get_subtype()
        {
            return subtype;
        }

        public int get_dimension()
        {
            return dimension;
        }

        public int get_precision()
        {
            return dimension;
        }

        public int get_input_srid()
        {
            return dimension;
        }

        public int get_scale()
        {
            return scale;
        }

        public int get_output_srid()
        {
            return scale;
        }

        public bool get_notnull()
        {
            return notnull;
        }

        public EncodingType get_compression()
        {
            return compression;
        }

        public int get_comp_param()
        {
            return comp_param;
        }

        public int get_size()
        {
            return size;
        }

        public int get_logical_size()
        {
            if(compression == EncodingType.ENCODING_FIXED || compression == EncodingType.ENCODING_DATE_IN_DAYS)
            {
                SQLTypeInfo ti = new(_type, dimension, scale, notnull, EncodingType.ENCODING_NONE, 0, subtype);

                return ti.get_size();
            }

            if(compression == EncodingType.ENCODING_DICT)
            {
                return 4;
            }

            return get_size();
        }

        public int get_physical_cols()
        {
            switch(_type)
            {
                case SQLType.POINT:        return 1; // coords
                case SQLType.LINESTRING:   return 2; // coords, bounds
                case SQLType.POLYGON:      return 4; // coords, ring_sizes, bounds, render_group
                case SQLType.MULTIPOLYGON: return 5; // coords, ring_sizes, poly_rings, bounds, render_group
                default:                   break;
            }

            return 0;
        }

        public int get_physical_coord_cols()
        {
            // @TODO dmitri/simon rename this function?
            // It needs to return the number of extra columns
            // which need to go through the executor, as opposed
            // to those which are only needed by CPU for poly
            // cache building or what-not. For now, we just omit
            // the Render Group column. If we add Bounding Box
            // or something this may require rethinking. Perhaps
            // these two functions need to return an array of
            // offsets rather than just a number to loop over,
            // so that executor and non-executor columns can
            // be mixed.
            // NOTE(adb): In binding to extension functions, we need to know some pretty specific
            // type info about each of the physical coords cols for each geo type. I added checks
            // there to ensure the physical coords col for the geo type match what we expect. If
            // these values are ever changed, corresponding values in
            // ExtensionFunctionsBinding.cpp::compute_narrowing_conv_scores and
            // ExtensionFunctionsBinding.cpp::compute_widening_conv_scores will also need to be
            // changed.
            switch(_type)
            {
                case SQLType.POINT:        return 1;
                case SQLType.LINESTRING:   return 1; // omit bounds
                case SQLType.POLYGON:      return 2; // omit bounds, render group
                case SQLType.MULTIPOLYGON: return 3; // omit bounds, render group
                default:                   break;
            }

            return 0;
        }

        public bool has_bounds()
        {
            switch(_type)
            {
                case SQLType.LINESTRING:
                case SQLType.POLYGON:
                case SQLType.MULTIPOLYGON: return true;
                default: break;
            }

            return false;
        }

        public bool has_render_group()
        {
            switch(_type)
            {
                case SQLType.POLYGON:
                case SQLType.MULTIPOLYGON: return true;
                default: break;
            }

            return false;
        }

        public void set_type(SQLType t)
        {
            _type = t;
        }

        public void set_subtype(SQLType st)
        {
            subtype = st;
        }

        public void set_dimension(int d)
        {
            dimension = d;
        }

        public void set_precision(int d)
        {
            dimension = d;
        }

        public void set_input_srid(int d)
        {
            dimension = d;
        }

        public void set_scale(int s)
        {
            scale = s;
        }

        public void set_output_srid(int s)
        {
            scale = s;
        }

        public void set_notnull(bool n)
        {
            notnull = n;
        }

        public void set_size(int s)
        {
            size = s;
        }

        public void set_fixed_size()
        {
            size = get_storage_size();
        }

        public void set_compression(EncodingType c)
        {
            compression = c;
        }

        public void set_comp_param(int p)
        {
            comp_param = p;
        }

        public string get_type_name()
        {
            if(IS_GEO(_type))
            {
                string srid_string = "";

                if(get_output_srid() > 0)
                {
                    srid_string = ", " + Convert.ToString(get_output_srid());
                }

                //CHECK_LT((int)subtype, SQLTypes.SQLTYPE_LAST);
                return type_name[(int)subtype] + "(" + type_name[(int)_type] + srid_string + ")";
            }

            string ps = "";

            if(_type == SQLType.DECIMAL || _type == SQLType.NUMERIC || subtype == SQLType.DECIMAL || subtype == SQLType.NUMERIC)
            {
                ps = "(" + Convert.ToString(dimension) + "," + Convert.ToString(scale) + ")";
            }
            else if(_type == SQLType.TIMESTAMP)
            {
                ps = "(" + Convert.ToString(dimension) + ")";
            }

            if(_type == SQLType.ARRAY)
            {
                SQLTypeInfo? elem_ti   = get_elem_type();
                string?      num_elems = size > 0 ? Convert.ToString(size / elem_ti.get_size()) : "";

                //CHECK_LT((int)subtype, SQLTypes.SQLTYPE_LAST);
                return type_name[(int)subtype] + ps + "[" + num_elems + "]";
            }

            return type_name[(int)_type] + ps;
        }

        public string get_compression_name()
        {
            return comp_name[(int)compression];
        }

        public string to_string()
        {
            return string.Join("(",
                               type_name[(int)_type],
                               ", ",
                               get_dimension(),
                               ", ",
                               get_scale(),
                               ", ",
                               get_notnull() ? "not nullable" : "nullable",
                               ", ",
                               get_compression_name(),
                               ", ",
                               get_comp_param(),
                               ", ",
                               type_name[(int)subtype],
                               ": ",
                               get_size(),
                               ": ",
                               get_elem_type().get_size(),
                               ")");
        }

        public bool is_string()
        {
            return IS_STRING(_type);
        }

        public bool is_string_array()
        {
            return _type == SQLType.ARRAY && IS_STRING(subtype);
        }

        public bool is_integer()
        {
            return IS_INTEGER(_type);
        }

        public bool is_decimal()
        {
            return _type == SQLType.DECIMAL || _type == SQLType.NUMERIC;
        }

        public bool is_fp()
        {
            return _type == SQLType.FLOAT || _type == SQLType.DOUBLE;
        }

        public bool is_number()
        {
            return IS_NUMBER(_type);
        }

        public bool is_time()
        {
            return IS_DATETIME(_type);
        }

        public bool is_boolean()
        {
            return _type == SQLType.BOOLEAN;
        }

        public bool is_array()
        {
            return _type == SQLType.ARRAY;
        }

        public bool is_varlen_array()
        {
            return _type == SQLType.ARRAY && size <= 0;
        }

        public bool is_fixlen_array()
        {
            return _type == SQLType.ARRAY && size > 0;
        }

        public bool is_timeinterval()
        {
            return IS_INTERVAL(_type);
        }

        public bool is_geometry()
        {
            return IS_GEO(_type);
        }

        public bool is_varlen()
        {
            // TODO: logically this should ignore fixlen arrays
            return IS_STRING(_type) && compression != EncodingType.ENCODING_DICT || _type == SQLType.ARRAY || IS_GEO(_type);
        }

        // need this here till is_varlen can be fixed w/o negative impact to existing code
        public bool is_varlen_indeed()
        {
            // SQLTypeInfo.is_varlen() is broken with fixedlen array now
            // and seems left broken for some concern, so fix it locally
            return is_varlen() && !is_fixlen_array();
        }

        public bool is_dict_encoded_string()
        {
            return is_string() && compression == EncodingType.ENCODING_DICT;
        }

        public static bool operator !=(SQLTypeInfo ImpliedObject,
                                       SQLTypeInfo rhs)
        {
            return ImpliedObject._type       != rhs.get_type()                                                                                                                                     ||
                   ImpliedObject.subtype     != rhs.get_subtype()                                                                                                                                  ||
                   ImpliedObject.dimension   != rhs.get_dimension()                                                                                                                                ||
                   ImpliedObject.scale       != rhs.get_scale()                                                                                                                                    ||
                   ImpliedObject.compression != rhs.get_compression()                                                                                                                              ||
                   ImpliedObject.compression != EncodingType.ENCODING_NONE && ImpliedObject.comp_param != rhs.get_comp_param() && ImpliedObject.comp_param != TRANSIENT_DICT(rhs.get_comp_param()) ||
                   ImpliedObject.notnull != rhs.get_notnull();
        }

        public static bool operator ==(SQLTypeInfo ImpliedObject,
                                       SQLTypeInfo rhs)
        {
            return ImpliedObject._type       == rhs.get_type()                                                                                                                                       &&
                   ImpliedObject.subtype     == rhs.get_subtype()                                                                                                                                    &&
                   ImpliedObject.dimension   == rhs.get_dimension()                                                                                                                                  &&
                   ImpliedObject.scale       == rhs.get_scale()                                                                                                                                      &&
                   ImpliedObject.compression == rhs.get_compression()                                                                                                                                &&
                   (ImpliedObject.compression == EncodingType.ENCODING_NONE || ImpliedObject.comp_param == rhs.get_comp_param() || ImpliedObject.comp_param == TRANSIENT_DICT(rhs.get_comp_param())) &&
                   ImpliedObject.notnull == rhs.get_notnull();
        }

        // FIX-ME:  Work through variadic base classes
        public SQLTypeInfo CopyFrom(SQLTypeInfo rhs)
        {
            _type       = rhs.get_type();
            subtype     = rhs.get_subtype();
            dimension   = rhs.get_dimension();
            scale       = rhs.get_scale();
            notnull     = rhs.get_notnull();
            compression = rhs.get_compression();
            comp_param  = rhs.get_comp_param();
            size        = rhs.get_size();

            return this;
        }

        public bool is_castable(SQLTypeInfo new_type_info)
        {
            // can always cast between the same type but different precision/scale/encodings
            if(_type == new_type_info.get_type())
            {
                return true;

                // can always cast from or to string
            }
            else if(is_string() || new_type_info.is_string())
            {
                return true;

                // can cast between numbers
            }
            else if(is_number() && new_type_info.is_number())
            {
                return true;

                // can cast from timestamp or date to number (epoch)
            }
            else if((_type == SQLType.TIMESTAMP || _type == SQLType.DATE) && new_type_info.is_number())
            {
                return true;

                // can cast from date to timestamp
            }
            else if(_type == SQLType.DATE && new_type_info.get_type() == SQLType.TIMESTAMP)
            {
                return true;
            }
            else if(_type == SQLType.TIMESTAMP && new_type_info.get_type() == SQLType.DATE)
            {
                return true;
            }
            else if(_type == SQLType.BOOLEAN && new_type_info.is_number())
            {
                return true;
            }
            else if(_type == SQLType.ARRAY && new_type_info.get_type() == SQLType.ARRAY)
            {
                return get_elem_type().is_castable(new_type_info.get_elem_type());
            }
            else
            {
                return false;
            }
        }

        public bool is_null(Datum d)
        {
            // assuming Datum is always uncompressed
            switch(_type)
            {
                case SQLType.BOOLEAN:  return d.boolval     == false;
                case SQLType.TINYINT:  return d.tinyintval  == sbyte.MinValue;
                case SQLType.SMALLINT: return d.smallintval == short.MinValue;
                case SQLType.INT:      return d.intval      == int.MinValue;
                case SQLType.BIGINT:
                case SQLType.NUMERIC:
                case SQLType.DECIMAL: return d.bigintval == long.MinValue;
                case SQLType.FLOAT:  return Math.Abs(d.floatval  - float.MinValue)  < float.Epsilon;
                case SQLType.DOUBLE: return Math.Abs(d.doubleval - double.MinValue) < double.Epsilon;
                case SQLType.TIME:
                case SQLType.TIMESTAMP:
                case SQLType.DATE: return d.bigintval == long.MinValue;
                case SQLType.TEXT:
                case SQLType.VARCHAR:
                case SQLType.CHAR:
                    // @TODO handle null strings
                    break;
                case SQLType.NULLT: return true;
                case SQLType.ARRAY: return d.arrayval.is_null;
                default:            break;
            }

            return false;
        }

        public unsafe bool is_null(nint val)
        {
            if(_type == SQLType.FLOAT)
            {
                return Math.Abs(UnManaged.Unsafe.AsRef<float>(val) - float.MinValue) < float.Epsilon;
            }

            if(_type == SQLType.DOUBLE)
            {
                return Math.Abs(UnManaged.Unsafe.AsRef<double>(val) - double.MinValue) < double.Epsilon;
            }

            // val can be either compressed or uncompressed
            switch(size)
            {
                case 1:                  return UnManaged.Unsafe.AsRef<sbyte>(val) == sbyte.MinValue;
                case 2:                  return UnManaged.Unsafe.AsRef<short>(val) == short.MinValue;
                case 4:                  return UnManaged.Unsafe.AsRef<int>(val)   == int.MinValue;
                case 8:                  return UnManaged.Unsafe.AsRef<long>(val)  == long.MinValue;
                case (int)SQLType.NULLT: return true;
                default:
                    // @TODO(wei) handle null strings
                    break;
            }

            return false;
        }

        //public bool is_null_fixlen_array(string val,
        //                                 int    array_size)
        //{
        //    // Check if fixed length array has a NULL_ARRAY sentinel as the first element
        //    if(type == SQLTypes.ARRAY && !string.IsNullOrEmpty(val) && array_size > 0 && array_size == size)
        //    {
        //        // Need to create element type to get the size, but can't call get_elem_type()
        //        // since this is a function. Going through copy constructor instead.
        //        var elem_ti = this;
        //        elem_ti.set_type(subtype);
        //        elem_ti.set_subtype(SQLTypes.NULLT);
        //        var elem_size = elem_ti.get_storage_size();

        //        if(elem_size < 1)
        //        {
        //            return false;
        //        }

        //        if(subtype == SQLTypes.FLOAT)
        //        {
        //            return *(float)val == (FLT_MIN * 2.0);
        //        }

        //        if(subtype == SQLTypes.DOUBLE)
        //        {
        //            return *(double)val == (DBL_MIN * 2.0);
        //        }

        //        switch(elem_size)
        //        {
        //            case 1:  return val         == (INT8_MIN  + 1);
        //            case 2:  return *(short)val == (INT16_MIN + 1);
        //            case 4:  return *(int)val   == (INT32_MIN + 1);
        //            case 8:  return *(long)val  == (INT64_MIN + 1);
        //            default: return false;
        //        }
        //    }

        //    return false;
        //}

        //public bool is_null_point_coord_array(string val,
        //                                      int    array_size)
        //{
        //    if(type == SQLTypes.ARRAY && subtype == SQLTypes.TINYINT && val != 0 && array_size > 0 && array_size == size)
        //    {
        //        if(array_size == 2 * sizeof(double))
        //        {
        //            return *(double)val == (DBL_MIN * 2.0);
        //        }

        //        if(array_size == 2 * sizeof(int))
        //        {
        //            return *(uint)val == DefineConstants.NULL_ARRAY_COMPRESSED_32;
        //        }
        //    }

        //    return false;
        //}

        public SQLTypeInfo get_elem_type()
        {
            return new(subtype, dimension, scale, notnull, compression, comp_param, SQLType.NULLT);
        }

        public SQLTypeInfo get_array_type()
        {
            return new(SQLType.ARRAY, dimension, scale, notnull, compression, comp_param, _type);
        }

        private SQLType      _type;       // type id
        private SQLType      subtype;     // element type of arrays
        private int          dimension;   // VARCHAR/CHAR length or NUMERIC/DECIMAL precision
        private int          scale;       // NUMERIC/DECIMAL scale
        private bool         notnull;     // nullable?  a hint, not used for type checking
        private EncodingType compression; // compression scheme
        private int          comp_param;  // compression parameter when applicable for certain schemes
        private int          size;        // size of the type in bytes.  -1 for variable size

        private static string[] type_name = new string[(int)SQLType.SQLTYPE_LAST]
        {
            "NULL", "BOOLEAN", "CHAR", "VARCHAR", "NUMERIC", "DECIMAL", "INTEGER", "SMALLINT", "FLOAT", "DOUBLE", "TIME", "TIMESTAMP", "BIGINT", "TEXT", "DATE", "ARRAY", "INTERVAL_DAY_TIME",
            "INTERVAL_YEAR_MONTH", "POINT", "LINESTRING", "POLYGON", "MULTIPOLYGON", "TINYINT", "GEOMETRY", "GEOGRAPHY", "EVAL_CONTEXT_TYPE", "VOID", "CURSOR"
        };

        private static string[] comp_name = new string[(int)EncodingType.ENCODING_LAST]
        {
            "NONE", "FIXED", "RL", "DIFF", "DICT", "SPARSE", "COMPRESSED", "DAYS"
        };

        private int get_storage_size()
        {
            switch(_type)
            {
                case SQLType.BOOLEAN: return sizeof(sbyte);
                case SQLType.TINYINT: return sizeof(sbyte);
                case SQLType.SMALLINT:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(short);
                        case EncodingType.ENCODING_FIXED:
                        case EncodingType.ENCODING_SPARSE: return comp_param / 8;
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF: break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.INT:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(int);
                        case EncodingType.ENCODING_FIXED:
                        case EncodingType.ENCODING_SPARSE: return comp_param / 8;
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF: break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.BIGINT:
                case SQLType.NUMERIC:
                case SQLType.DECIMAL:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(long);
                        case EncodingType.ENCODING_FIXED:
                        case EncodingType.ENCODING_SPARSE: return comp_param / 8;
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF: break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.FLOAT:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(float);
                        case EncodingType.ENCODING_FIXED:
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF:
                        case EncodingType.ENCODING_SPARSE:
                            Debug.Assert(false);

                            break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.DOUBLE:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(double);
                        case EncodingType.ENCODING_FIXED:
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF:
                        case EncodingType.ENCODING_SPARSE:
                            Debug.Assert(false);

                            break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.TIMESTAMP:
                case SQLType.TIME:
                case SQLType.INTERVAL_DAY_TIME:
                case SQLType.INTERVAL_YEAR_MONTH:
                case SQLType.DATE:
                    switch(compression)
                    {
                        case EncodingType.ENCODING_NONE: return sizeof(long);
                        case EncodingType.ENCODING_FIXED:
                            if(_type == SQLType.TIMESTAMP && dimension > 0)
                            {
                                Debug.Assert(false); // disable compression for timestamp precisions
                            }

                            return comp_param / 8;
                        case EncodingType.ENCODING_RL:
                        case EncodingType.ENCODING_DIFF:
                        case EncodingType.ENCODING_SPARSE:
                            Debug.Assert(false);

                            break;
                        case EncodingType.ENCODING_DATE_IN_DAYS:
                            switch(comp_param)
                            {
                                case 0: return 4; // Default date encoded in days is 32 bits
                                case 16:
                                case 32: return comp_param / 8;
                                default:
                                    Debug.Assert(false);

                                    break;
                            }

                            break;
                        default:
                            Debug.Assert(false);

                            break;
                    }

                    break;
                case SQLType.TEXT:
                case SQLType.VARCHAR:
                case SQLType.CHAR:
                    if(compression == EncodingType.ENCODING_DICT)
                    {
                        return sizeof(int); // @TODO(wei) must check DictDescriptor
                    }

                    break;
                case SQLType.ARRAY:
                    // TODO: return size for fixlen arrays?
                    break;
                case SQLType.POINT:
                case SQLType.LINESTRING:
                case SQLType.POLYGON:
                case SQLType.MULTIPOLYGON: break;
                default: break;
            }

            return -1;
        }

        public enum PackagingType
        {
            Chunk,
            StandardBuffer
        }

        public bool isStandardBufferPackaging()
        {
            return packaging_type_ == PackagingType.StandardBuffer;
        }

        public bool isChunkIteratorPackaging()
        {
            return packaging_type_ == PackagingType.Chunk;
        }

        public void setStandardBufferPackaging()
        {
            packaging_type_ = PackagingType.StandardBuffer;
        }

        public void setChunkIteratorPackaging()
        {
            packaging_type_ = PackagingType.Chunk;
        }

        private PackagingType packaging_type_ = PackagingType.Chunk;

        //public int get_array_context_logical_size()
        //{
        //    CORE_TYPE derived = new CORE_TYPE((CORE_TYPE)this);

        //    if (is_member_of_typeset<SQLTypes.kCHAR, SQLTypes.kTEXT, SQLTypes.kVARCHAR>(derived))
        //    {
        //        var comp_type = derived.get_compression();
        //        if (comp_type == EncodingType.ENCODING_DICT || comp_type == EncodingType.ENCODING_FIXED || comp_type == EncodingType.ENCODING_NONE)
        //        {
        //            return sizeof(int);
        //        }
        //    }
        //    return derived.get_logical_size();
        //}

        public bool is_date_in_days()
        {
            if(IS_DATETIME(_type))
            {
                EncodingType comp_type = get_compression();

                if(comp_type == EncodingType.ENCODING_DATE_IN_DAYS)
                {
                    return true;
                }
            }

            return false;
        }

        //public bool is_date()
        //{
        //    CORE_TYPE derived = new CORE_TYPE((CORE_TYPE)this);
        //    if (is_member_of_typeset<SQLTypes.kDATE>(derived))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public bool is_high_precision_timestamp()
        //{
        //    CORE_TYPE derived = new CORE_TYPE((CORE_TYPE)this);
        //    if (is_member_of_typeset<SQLTypes.kTIMESTAMP>(derived))
        //    {
        //        var dimension = derived.get_dimension();
        //        if (dimension > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool is_timestamp()
        //{
        //    CORE_TYPE derived = new CORE_TYPE((CORE_TYPE)this);
        //    if (is_member_of_typeset<SQLTypes.TIMESTAMP>(derived))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public bool Equals(SQLTypeInfo? other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return _type           == other._type       &&
                   subtype         == other.subtype     &&
                   dimension       == other.dimension   &&
                   scale           == other.scale       &&
                   notnull         == other.notnull     &&
                   compression     == other.compression &&
                   comp_param      == other.comp_param  &&
                   size            == other.size        &&
                   packaging_type_ == other.packaging_type_;
        }

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj))
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            if(obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((SQLTypeInfo)obj);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add((int)_type);
            hashCode.Add((int)subtype);
            hashCode.Add(dimension);
            hashCode.Add(scale);
            hashCode.Add(notnull);
            hashCode.Add((int)compression);
            hashCode.Add(comp_param);
            hashCode.Add(size);
            hashCode.Add((int)packaging_type_);

            return hashCode.ToHashCode();
        }
    }
}
