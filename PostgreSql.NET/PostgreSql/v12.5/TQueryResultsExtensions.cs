using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Apache.Thrift.Database;

using Microsoft.Data.Analysis;

using System.Text.Json;

namespace PostgreSql
{
    public static class TQueryResultsExtensions
    {
        public static string ToHtml(this TQueryResult queryResult)
        {
            StringBuilder writer = new StringBuilder();

            writer.AppendLine("<table style=\"width:100%\">");

            writer.AppendLine("<tr>");

            foreach(TColumnType rowDesc in queryResult.Row_set.Row_desc)
            {
                writer.AppendLine($"<th>{rowDesc.Col_name}</th>");
            }

            writer.AppendLine("</tr>");

            writer.AppendLine("<tr>");

            foreach(TColumnType rowDesc in queryResult.Row_set.Row_desc)
            {
                writer.AppendLine($"<td>{rowDesc.Col_type.Type.ToString()}</td>");
            }

            writer.AppendLine("</tr>");

            TDatumType col_type;
            bool       is_null;

            for(int row = 0; row < queryResult.Row_set.Columns.FirstOrDefault()!.Nulls.Count; ++row)
            {
                writer.AppendLine("<tr>");

                for(int i = 0; i < queryResult.Row_set.Columns.Count; ++i)
                {
                    col_type = queryResult.Row_set.Row_desc[i].Col_type.Type;

                    is_null = queryResult.Row_set.Columns[i].Nulls[row];

                    switch(col_type)
                    {
                        case TDatumType.BOOL:
                        case TDatumType.TINYINT:
                        case TDatumType.SMALLINT:
                        case TDatumType.INT:
                        case TDatumType.BIGINT:
                        {
                            if(!is_null)
                            {
                                writer.AppendLine($"<td>{queryResult.Row_set.Columns[i].Data.Int_col[row]}</td>");
                            }

                            break;
                        }
                        case TDatumType.FLOAT:
                        case TDatumType.DOUBLE:
                        {
                            if(!is_null)
                            {
                                writer.AppendLine($"<td>{queryResult.Row_set.Columns[i].Data.Real_col[row]}</td>");
                            }

                            break;
                        }
                        case TDatumType.DATE:
                        case TDatumType.TIME:
                        case TDatumType.STR:
                        {
                            if(!is_null)
                            {
                                writer.AppendLine($"<td>{queryResult.Row_set.Columns[i].Data.Str_col[row]}</td>");
                            }

                            break;
                        }
                        default:
                        {
                            throw new NotSupportedException(col_type.ToString());
                        }
                    }
                }

                writer.AppendLine("</tr>");
            }

            writer.AppendLine("</table>");

            return writer.ToString();
        }

        public static DataFrame ToDataFrame(this TQueryResult queryResult)
        {
            List<DataFrameColumn> columns = new List<DataFrameColumn>(queryResult.Row_set.Columns.Count);

            int nRows = queryResult.Row_set.Columns.FirstOrDefault()!.Nulls.Count;

            foreach(TColumnType rowDesc in queryResult.Row_set.Row_desc)
            {
                switch(rowDesc.Col_type.Type)
                {
                    case TDatumType.BOOL:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<bool>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.TINYINT:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<sbyte>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.SMALLINT:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<short>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.INT:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<int>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.BIGINT:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<long>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.FLOAT:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<float>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.DOUBLE:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<double>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.DATE:
                    case TDatumType.TIME:
                    {
                        columns.Add(new PrimitiveDataFrameColumn<DateTime>(rowDesc.Col_name, nRows));

                        break;
                    }
                    case TDatumType.STR:
                    {
                        columns.Add(new StringDataFrameColumn(rowDesc.Col_name, nRows));

                        break;
                    }
                    default:
                    {
                        throw new NotSupportedException(rowDesc.Col_name);
                    }
                }
            }

            TDatumType col_type;
            bool       is_null;

            for(int row = 0; row < nRows; ++row)
            {
                for(int i = 0; i < queryResult.Row_set.Columns.Count; ++i)
                {
                    col_type = queryResult.Row_set.Row_desc[i].Col_type.Type;

                    is_null = queryResult.Row_set.Columns[i].Nulls[row];

                    switch(col_type)
                    {
                        case TDatumType.BOOL:
                        case TDatumType.TINYINT:
                        case TDatumType.SMALLINT:
                        case TDatumType.INT:
                        case TDatumType.BIGINT:
                        {
                            if(!is_null)
                            {
                                columns[i][row] = queryResult.Row_set.Columns[i].Data.Int_col[row];
                            }

                            break;
                        }
                        case TDatumType.FLOAT:
                        case TDatumType.DOUBLE:
                        {
                            if(!is_null)
                            {
                                columns[i][row] = queryResult.Row_set.Columns[i].Data.Real_col[row];
                            }

                            break;
                        }
                        case TDatumType.DATE:
                        case TDatumType.TIME:
                        case TDatumType.STR:
                        {
                            if(!is_null)
                            {
                                columns[i][row] = queryResult.Row_set.Columns[i].Data.Str_col[row];
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double? DoubleValue(this object obj)
        {
            if(obj is null || obj is DBNull)
            {
                return null;
            }

            if(obj is double value)
            {
                return value;
            }

            throw new NotSupportedException();
        }
    }
}