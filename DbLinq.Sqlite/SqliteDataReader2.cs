#region MIT license
////////////////////////////////////////////////////////////////////
// MIT license:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//
// Authors:
//        Jiri George Moudry
////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using DbLinq.Vendor.Implementation;

namespace DbLinq.Sqlite
{
    /// <summary>
    /// This class wraps SQLiteDataReader.
    /// It logs exceptions and provides methods to retrieve nullable types.
    /// 
    /// When we have a workaround for FatalExecutionEngineError on nullables, 
    /// this can go away.
    /// </summary>
    public class SqliteDataReader2 : DataReader2
    {
        public SqliteDataReader2(IDataReader rdr)
            : base(rdr)
        {
        }
        
        public override short? GetInt16N(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetInt16(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt16N failed: "+ex);
                return null;
            }
        }

        public override char? GetCharN(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetChar(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetCharN failed: "+ex);
                return null;
            }
        }

        public override bool GetBoolean(int index)
        {
            //support for 'Product.Discontinued' field in Northwind DB - it's nullable, but MS samples map it to plain bool
            if (_rdr.IsDBNull(index))
                return false; 

            return _rdr.GetBoolean(index); 
        }

        public override bool? GetBooleanN(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetBoolean(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetBooleanN failed: "+ex);
                return null;
            }
        }

        public override int GetInt32(int index)
        {
            try
            {
                return _rdr.GetInt32(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32("+index+") failed: "+ex);
                return 0;
            }
        }
        public override int? GetInt32N(int index)
        {
            try
            {
                if (_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetInt32(index);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetInt32N(" + index + ") failed: " + ex);
                return 0;
            }
        }

        
        public override uint GetUInt32(int index)
        {
            // picrap: ???
            //try
            //{
            //    return _rdr.GetUInt32(index);
            //} 
            //catch(Exception ex)
            //{
            //    Console.WriteLine("GetUInt32("+index+") failed: "+ex);
            //    try {
            //            object obj = _rdr.GetValue(index);
            //            Console.WriteLine("GetUInt32 failed, offending val: "+obj);
            //    } catch(Exception){}
            return 0;
            //}
        }

        public override uint? GetUInt32N(int index)
        {
            // picrap: ????
            //try
            //{
            //    if (_rdr.IsDBNull(index))
            return null;
            //    return _rdr.GetUInt32(index);
            //} 
            //catch(Exception ex)
            //{
            //    Console.WriteLine("GetUInt32 failed: "+ex);
            //    return null;
            //}
        }
        
        public override float GetFloat(int index)
        {
            try
            {
                return _rdr.GetFloat(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32 failed: "+ex);
                return 0;
            }
        }

        public override float? GetFloatN(int index)
        {
            try
            {
                if (_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetFloat(index);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFloatN failed: " + ex);
                return 0;
            }
        }

        public override double GetDouble(int index)
        {
            try
            {
                return _rdr.GetDouble(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32 failed: "+ex);
                return 0;
            }
        }
        public override double? GetDoubleN(int index)
        {
            try
            {
                if (_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetDouble(index);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetInt32 failed: " + ex);
                return 0;
            }
        }

        public override decimal GetDecimal(int index)
        {
            try
            {
                return _rdr.GetDecimal(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32 failed: "+ex);
                return 0;
            }
        }
        public override decimal? GetDecimalN(int index)
        {
            try
            {
                if (_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetDecimal(index);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDecimal(" + index + ") failed: " + ex);
                return 0;
            }
        }
        public override DateTime GetDateTime(int index)
        {
            try
            {
                return _rdr.GetDateTime(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32 failed: "+ex);
                return new DateTime();
            }
        }
        public override DateTime? GetDateTimeN(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetDateTime(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt32 failed: "+ex);
                return null;
            }
        }

        public override long GetInt64(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return -1;
                return _rdr.GetInt64(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt64N failed: "+ex);
                return 0;
            }
        }
        public override long? GetInt64N(int index)
        {
            try
            {
                if(_rdr.IsDBNull(index))
                    return null;
                return _rdr.GetInt64(index);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetInt64N failed: "+ex);
                return null;
            }
        }
        public override ulong GetUInt64(int index)
        {
            return (ulong)GetInt64(index);
        }

        public override string GetString(int index)
        {
            try
            {
                object obj = _rdr.GetValue(index);
                if (obj == null)
                    return null; //nullable text?

                return obj as string;
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetString("+index+") failed: "+ex);
                return null;
            }
        }

        

        public override byte[] GetBytes(int index)
        {
            try
            {
                //System.Data.SqlClient.SqlDataReader rdr2;
                //rdr2.GetSqlBinary(); //SqlBinary does not seem to exist on MySql
                object obj = _rdr.GetValue(index);
                if(_rdr.IsDBNull(index))
                    return null; //nullable blob?
                byte[] bytes = obj as byte[];
                if(bytes!=null)
                    return bytes; //works for BLOB field
                Console.WriteLine("GetBytes(" + index + "): received unexpected type:"+obj);
                //return _rdr.GetInt32(index);
                return new byte[0];
            } 
            catch(Exception ex)
            {
                Console.WriteLine("GetBytes failed: "+ex);
                return null;
            }
        }

        /// <summary>
        /// helper method for reading an integer, and casting it to enum
        /// </summary>
        public override T2 GetEnum<T2>(int index)
        {
            try
            {
                int value = (_rdr.IsDBNull(index))
                                ? 0
                                : _rdr.GetInt32(index);
                return (T2)Enum.ToObject(typeof(T2), value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetEnum failed: " + ex);
                return new T2();
            }
        }
    }
}