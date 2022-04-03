// ReSharper disable InconsistentNaming

using System;

using Apache.Thrift.Database;

namespace PostgreSql
{
    public enum EncodingType
    {
        ENCODING_NONE         = 0, // no encoding
        ENCODING_FIXED        = 1, // Fixed-bit encoding
        ENCODING_RL           = 2, // Run Length encoding
        ENCODING_DIFF         = 3, // Differential encoding
        ENCODING_DICT         = 4, // Dictionary encoding
        ENCODING_SPARSE       = 5, // Null encoding for sparse columns
        ENCODING_GEOINT       = 6, // Encoding coordinates as intergers
        ENCODING_DATE_IN_DAYS = 7, // Date encoding in days
        ENCODING_LAST         = 8
    }

    public enum SQLType
    {
        NULLT               = 0, // type for null values
        BOOLEAN             = 1,
        CHAR                = 2,
        VARCHAR             = 3,
        NUMERIC             = 4,
        DECIMAL             = 5,
        INT                 = 6,
        SMALLINT            = 7,
        FLOAT               = 8,
        DOUBLE              = 9,
        TIME                = 10,
        TIMESTAMP           = 11,
        BIGINT              = 12,
        TEXT                = 13,
        DATE                = 14,
        ARRAY               = 15,
        INTERVAL_DAY_TIME   = 16,
        INTERVAL_YEAR_MONTH = 17,
        POINT               = 18,
        LINESTRING          = 19,
        POLYGON             = 20,
        MULTIPOLYGON        = 21,
        TINYINT             = 22,
        GEOMETRY            = 23,
        GEOGRAPHY           = 24,
        EVAL_CONTEXT_TYPE   = 25, // Placeholder Type for ANY
        VOID                = 26,
        CURSOR              = 27,
        SQLTYPE_LAST        = 28
    }

    public class TargetMetaInfo
    {
        private readonly string      _resname;
        private readonly SQLTypeInfo _ti;
        private readonly SQLTypeInfo _physical_ti;

        public TargetMetaInfo(string      resname,
                              SQLTypeInfo ti)
        {
            _resname     = resname;
            _ti          = ti;
            _physical_ti = ti;
        }

        public TargetMetaInfo(string      resname,
                              SQLTypeInfo ti,
                              SQLTypeInfo physical_ti)
        {
            _resname     = resname;
            _ti          = ti;
            _physical_ti = physical_ti;
        }

        public string get_resname()
        {
            return _resname;
        }

        public SQLTypeInfo get_type_info()
        {
            return _ti;
        }

        public SQLTypeInfo get_physical_type_info()
        {
            return _physical_ti;
        }
    }

    public static class SqlToThrift
    {
        public static TDatumType GetDatumType(SQLType type, out int size)
        {
            switch(type)
            {
                case SQLType.BOOLEAN:
                    
                {
                    size = sizeof(bool);
                    return TDatumType.BOOL;
                }
                case SQLType.TINYINT:
                {
                    size = sizeof(sbyte);
                    return TDatumType.TINYINT;
                }
                case SQLType.SMALLINT:
                {
                    size = sizeof(short);
                    return TDatumType.SMALLINT;
                }
                case SQLType.INT:
                {
                    size = sizeof(int);
                    return TDatumType.INT;
                }
                case SQLType.BIGINT:
                {
                    size = sizeof(long);
                    return TDatumType.BIGINT;
                }
                case SQLType.FLOAT:
                {
                    size = sizeof(float);
                    return TDatumType.FLOAT;
                }
                case SQLType.DOUBLE:
                {
                    size = sizeof(double);
                    return TDatumType.DOUBLE;
                }
                case SQLType.CHAR:
                case SQLType.TEXT:
                case SQLType.VARCHAR:
                {
                    size = -1;
                    return TDatumType.STR;
                }
                case SQLType.DATE:
                {
                    size = -1;
                    return TDatumType.DATE;
                }
                case SQLType.TIME:
                {
                    size = -1;
                    return TDatumType.TIME;
                }
                case SQLType.TIMESTAMP:
                {
                    size = -1;
                    return TDatumType.TIMESTAMP;
                }
                default:
                {
                    throw new NotSupportedException(type.ToString());
                }
            }
        }

        public static TColumnType createTColumnType(string    colName,
                                                    TTypeInfo colType)
        {
            return createTColumnType(colName, colType, false);
        }

        public static TColumnType createTColumnType(string    colName,
                                                    TTypeInfo colType,
                                                    bool      irk)
        {
            TColumnType ct = new TColumnType
            {
                Col_name            = colName,
                Col_type            = colType,
                Is_reserved_keyword = irk,
                Is_system           = false,
                Is_physical         = false
            };

            return ct;
        }

        public static TTypeInfo type_info_to_thrift(SQLTypeInfo ti)
        {
            TTypeInfo thrift_ti = new TTypeInfo
            {
                Type       = ti.is_array() ? type_to_thrift(ti.get_elem_type()) : type_to_thrift(ti),
                Encoding   = encoding_to_thrift(ti),
                Nullable   = !ti.get_notnull(),
                Is_array   = ti.is_array(),
                Precision  = ti.get_precision(),
                Scale      = ti.get_scale(),
                Comp_param = ti.get_comp_param(),
                Size       = ti.get_size()
            };

            return thrift_ti;
        }

        public static void fixup_geo_column_descriptor(ref TColumnType colType,
                                                       SQLType         subtype,
                                                       int             outputSrid)
        {
            colType.Col_type.Precision = (int)subtype;
            colType.Col_type.Scale     = outputSrid;
        }

        public static TDatumType type_to_thrift(SQLTypeInfo typeInfo)
        {
            SQLType type = typeInfo.get_type();

            if(type == SQLType.ARRAY)
            {
                type = typeInfo.get_subtype();
            }

            switch(type)
            {
                case SQLType.BOOLEAN:  return TDatumType.BOOL;
                case SQLType.TINYINT:  return TDatumType.TINYINT;
                case SQLType.SMALLINT: return TDatumType.SMALLINT;
                case SQLType.INT:      return TDatumType.INT;
                case SQLType.BIGINT:   return TDatumType.BIGINT;
                case SQLType.FLOAT:    return TDatumType.FLOAT;
                case SQLType.NUMERIC:
                case SQLType.DECIMAL: return TDatumType.DECIMAL;
                case SQLType.DOUBLE: return TDatumType.DOUBLE;
                case SQLType.TEXT:
                case SQLType.VARCHAR:
                case SQLType.CHAR: return TDatumType.STR;
                case SQLType.TIME:                return TDatumType.TIME;
                case SQLType.TIMESTAMP:           return TDatumType.TIMESTAMP;
                case SQLType.DATE:                return TDatumType.DATE;
                case SQLType.INTERVAL_DAY_TIME:   return TDatumType.INTERVAL_DAY_TIME;
                case SQLType.INTERVAL_YEAR_MONTH: return TDatumType.INTERVAL_YEAR_MONTH;
                case SQLType.POINT:               return TDatumType.POINT;
                case SQLType.LINESTRING:          return TDatumType.LINESTRING;
                case SQLType.POLYGON:             return TDatumType.POLYGON;
                case SQLType.MULTIPOLYGON:        return TDatumType.MULTIPOLYGON;
                case SQLType.GEOMETRY:            return TDatumType.GEOMETRY;
                case SQLType.GEOGRAPHY:           return TDatumType.GEOGRAPHY;
            }

            throw new NotSupportedException();
        }

        public static TEncodingType encoding_to_thrift(SQLTypeInfo typeInfo)
        {
            switch(typeInfo.get_compression())
            {
                case EncodingType.ENCODING_NONE:         return TEncodingType.NONE;
                case EncodingType.ENCODING_FIXED:        return TEncodingType.FIXED;
                case EncodingType.ENCODING_RL:           return TEncodingType.RL;
                case EncodingType.ENCODING_DIFF:         return TEncodingType.DIFF;
                case EncodingType.ENCODING_DICT:         return TEncodingType.DICT;
                case EncodingType.ENCODING_SPARSE:       return TEncodingType.SPARSE;
                case EncodingType.ENCODING_GEOINT:       return TEncodingType.GEOINT;
                case EncodingType.ENCODING_DATE_IN_DAYS: return TEncodingType.DATE_IN_DAYS;
            }

            throw new NotSupportedException();
        }

        public static TColumnType convert_target_metainfo(TargetMetaInfo target,
                                                          long           idx)
        {
            TColumnType projInfo = new TColumnType
            {
                Col_name = target.get_resname()
            };

            if(string.IsNullOrEmpty(projInfo.Col_name))
            {
                projInfo.Col_name = "result_" + $"{idx + 1}";
            }

            SQLTypeInfo targetTi = target.get_type_info();

            projInfo.Col_type.Type     = type_to_thrift(targetTi);
            projInfo.Col_type.Encoding = encoding_to_thrift(targetTi);
            projInfo.Col_type.Nullable = !targetTi.get_notnull();
            projInfo.Col_type.Is_array = targetTi.get_type() == SQLType.ARRAY;

            if(SQLTypeInfo.IS_GEO(targetTi.get_type()))
            {
                fixup_geo_column_descriptor(ref projInfo, targetTi.get_subtype(), targetTi.get_output_srid());
            }
            else
            {
                projInfo.Col_type.Precision = targetTi.get_precision();
                projInfo.Col_type.Scale     = targetTi.get_scale();
            }

            if(targetTi.get_type() == SQLType.DATE)
            {
                projInfo.Col_type.Size = targetTi.get_size();
            }

            projInfo.Col_type.Comp_param = (targetTi.is_date_in_days() && targetTi.get_comp_param() == 0) ? 32 : targetTi.get_comp_param();

            return projInfo;
        }
    }
}
