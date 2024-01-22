namespace CRS.CUSTOMER.REPOSITORY
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CRS.CUSTOMER.SHARED;

    public class RepositoryDao
    {
        System.Data.SqlClient.SqlConnection _connection;
        /// <summary>
        /// Initialize
        /// </summary>
        public RepositoryDao()
        {
            Init();
        }
        private void Init()
        {
            _connection = new System.Data.SqlClient.SqlConnection(GetConnectionString());
        }
        private void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                _connection.Close();
            _connection.Open();
        }
        private void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                this._connection.Close();
        }
        private string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DBConnString"] != null ? ConfigurationManager.ConnectionStrings["DBConnString"].ConnectionString : "";
        }
        protected DataTable ExecuteDataTable(string procName, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(procName, _connection);
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            catch (Exception x)
            {
                dt.Rows.Add(9, x.Message);
            }
            finally
            {
                cmd.Dispose();
                _connection.Close();
            }
            return dt;
        }
        public object ParseColumnValue(DataRow row, string ColumnName)
        {
            if (row == null || string.IsNullOrEmpty(ColumnName))
                return null;
            else
            {
                if (row.Table.Columns.Contains(ColumnName))
                    return row[ColumnName];
                else
                    return null;
            }
        }
        public string LogError(Exception Apperr, string page)
        {
            Exception err = Apperr;
            if (Apperr.InnerException != null)
                err = Apperr.InnerException;



            var errPage = FilterString(page);
            var errMsg = FilterString(err.Message);
            var errDetails = FilterString(Apperr.Message);



            var st = new StackTrace(err, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();

            var eSql = "sproc_error_handler @error_code='1',@msg=" + FilterString("Message => " + err.Message + " and LineNumber => " + line.ToString()) + ",@error_script=" + FilterString("") + ",@error_category=" + FilterString("Application") + ",@error_source=" + FilterString(page);
            var eRow = ExecuteDataRow(eSql);
            return ParseColumnValue(eRow, "id").ToString();
        }
        /// <summary>
        /// Returns DataSet (Used in case multiple tables are returned)
        /// </summary>
        /// <param name="sql">Sql Query</param>
        /// <returns></returns>
        public System.Data.DataSet ExecuteDataset(string sql)
        {
            var ds = new System.Data.DataSet();
            var cmd = new System.Data.SqlClient.SqlCommand(sql, _connection);
            cmd.CommandTimeout = 120;
            System.Data.SqlClient.SqlDataAdapter da;

            try
            {
                OpenConnection();
                //da = new SqlDataAdapter(sql, _connection);
                da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
                //throw ex;
                DataTable objDt1 = new DataTable();
                var GetCallingAssembly = Assembly.GetCallingAssembly();
                var GetExecutingAssembly = Assembly.GetExecutingAssembly();
                objDt1.Columns.Add("Code");
                StackTrace stackTrace = new StackTrace();
                MethodBase methodBase = stackTrace.GetFrame(stackTrace.FrameCount - 1).GetMethod(); // Not sure about the "- 1"
                Console.WriteLine(methodBase.Name);
                var x = stackTrace.GetFrame(1).GetMethod().Name;
                objDt1.Columns.Add("Message");
                objDt1.Columns.Add("id");
                var eSql = "sproc_error_handler @error_code='1',@msg=" + FilterString(ex.Message) + ",@error_script=" + FilterString(sql) + ",@error_category=" + FilterString("SQL Query") + ",@error_source=" + FilterString("Application");
                var eRow = ExecuteDataRow(eSql);
                ParseColumnValue(eRow, "id").ToString();
                objDt1.Rows.Add(1, ConfigurationManager.AppSettings["phase"] != null && ConfigurationManager.AppSettings["phase"].ToString().ToUpper() == "DEVELOPMENT" ? ex.Message : "Something went wrong. Please contact support. Error Id:" + ParseColumnValue(eRow, "id").ToString(), ParseColumnValue(eRow, "id").ToString());
                ds.Tables.Add(objDt1);
            }
            finally
            {
                da = null;
                CloseConnection();
            }
            return ds;
        }
        /// <summary>
        /// Returns DataRow (Used in case sql query returns a single table with a single row)
        /// </summary>
        /// <param name="sql">Sql Query</param>
        /// <returns></returns>
        public System.Data.DataRow ExecuteDataRow(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return ds.Tables[0].Rows[0];
            }
        }
        /// <summary>
        /// Returns DataTable (Used in case sql query returns a single table)
        /// </summary>
        /// <param name="sql">Sql Query</param>
        /// <returns></returns>
        public System.Data.DataTable ExecuteDataTable(string sql)
        {
            using (var ds = ExecuteDataset(sql))
            {
                if (ds == null || ds.Tables.Count == 0)
                    return null;

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return ds.Tables[0];
            }
        }
        /// <summary>
        /// Parses Sql query To List Object
        /// </summary>
        /// <param name="sql">Sql Query</param>
        /// <returns></returns>
        public System.Collections.Generic.List<object> SqlQueryToListObject(string sql)
        {
            var dt = ExecuteDataTable(sql);
            var list = new System.Collections.Generic.List<object>();
            if (null != dt)
            {
                foreach (System.Data.DataRow item in dt.Rows)
                {
                    list.Add(new { id = item["Value"], text = item["Name"].ToString() });
                }
            }
            return list;
        }
        /// <summary>
        /// Used to filter string for sql parameter
        /// </summary>
        /// <param name="strVal">Parameter Value</param>
        /// <param name="unicode">true if contains unicode</param>
        /// <returns></returns>
        public String FilterParameter(string strVal, bool unicode = false)
        {
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = (unicode ? "N" : "") + "'" + str + "'";

            return str.TrimEnd().TrimStart();
        }



        /// <summary>
        /// Checks if Database Value is not null and then return it to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string DBNullToValue(object obj)
        {
            if (obj != DBNull.Value)
            {
                return obj.ToString();
            }
            return null;
        }
        private String FilterQuote(string strVal)
        {
            if (string.IsNullOrEmpty(strVal))
            {
                strVal = "";
            }
            var str = strVal.Trim();

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(";", "");
                //str = str.Replace(",", "");
                str = str.Replace("--", "");
                str = str.Replace("'", "''");

                str = str.Replace("/*", "");
                str = str.Replace("*/", "");

                str = str.Replace(" select ", "");
                str = str.Replace(" insert ", "");
                str = str.Replace(" update ", "");
                str = str.Replace(" delete ", "");

                str = str.Replace(" drop ", "");
                str = str.Replace(" truncate ", "");
                str = str.Replace(" create ", "");

                str = str.Replace(" begin ", "");
                str = str.Replace(" end ", "");
                str = str.Replace(" char(", "");

                str = str.Replace(" exec ", "");
                str = str.Replace(" xp_cmd ", "");

                str = str.Replace(" sp_help ", "");
                str = str.Replace(" sproc_ ", "");
                str = str.Replace(" apiproc_ ", "");


                str = str.Replace("<script", "");

            }
            else
            {
                str = "null";
            }
            return str;
        }
        /// <summary>
        /// Parses DataTable To CommonDbResponse object
        /// </summary>
        /// <param name="dt">DataTable Object</param>
        /// <returns></returns>
        public CommonDbResponse ParseCommonDbResponse(System.Data.DataTable dt)
        {
            var res = new CommonDbResponse();
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    string Code = dt.Rows[0][0].ToString();
                    string Message = dt.Rows[0][1].ToString();
                    string Extra1 = "", Extra2 = "", Extra3 = "", Extra4 = "", Extra5 = "";
                    if (dt.Columns.Count > 2)
                    {
                        Extra1 = dt.Rows[0][2].ToString();
                    }
                    if (dt.Columns.Count > 3)
                    {
                        Extra2 = dt.Rows[0][3].ToString();
                    }
                    if (dt.Columns.Count > 4)
                    {
                        Extra3 = dt.Rows[0][4].ToString();
                    }
                    if (dt.Columns.Count > 5)
                    {
                        Extra4 = dt.Rows[0][5].ToString();
                    }
                    if (dt.Columns.Count > 6)
                    {
                        Extra5 = dt.Rows[0][6].ToString();
                    }
                    res.SetMessage(Code, Message, Extra1, Extra2, Extra3, Extra4, Extra5, dt);
                    if (dt.Columns.Contains("id"))
                    {
                        res.Id = dt.Rows[0]["id"].ToString();
                    }
                }
            return res;
        }
        /// <summary>
        /// Parses Sql Query To CommonDbResponse object
        /// </summary>
        /// <param name="sql">Sql Query</param>
        /// <returns></returns>
        public CommonDbResponse ParseCommonDbResponse(string sql)
        {
            return ParseCommonDbResponse(ExecuteDataTable(sql));
        }
        /// <summary>
        /// Parses Sql Query To Datatable and then Dictionary(Only Used if Col1 is key And Col2 is value)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<string, string> ParseSqlToDictionary(string sql)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, string>();
            var dt = ExecuteDataTable(sql);
            if (null == dt || dt.Rows.Count == 0)
            {
                return dictionary;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                dictionary.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                //}
            }
            return dictionary;
        }
        /// <summary>
        /// Parse Datatable to Dictionary(Only Used if Col1 is key And Col2 is value)
        /// </summary>
        /// <param name="dt">DataTable Object</param>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<string, string> DataTableToDictionary(DataTable dt)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, string>();
            if (null == dt || dt.Rows.Count == 0)
            {
                return dictionary;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //for (int j = 0; j < dt.Columns.Count; j++)
                //{
                dictionary.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
                //}
            }
            return dictionary;
        }
        /// <summary>
        /// Set Exception Response and return as CommonDbResponse object 
        /// </summary>
        /// <param name="msg">Exception Message</param>
        /// <returns></returns>
        public CommonDbResponse ExceptionDbResponse(string msg)
        {
            CommonDbResponse dr = new CommonDbResponse()
            {
                Code = ResponseCode.Exception,
                Message = msg,
                Extra1 = ""
            };
            return dr;
        }
        /// <summary>
        /// Converts List To DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable("dt");

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        /// <summary>
        /// Convert Datatable To String
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <returns></returns>
        public string DataTableToString(DataTable dt)
        {
            string xml = null;
            using (TextWriter writer = new StringWriter())
            {
                dt.WriteXml(writer);
                xml = writer.ToString();
            }
            return xml;
        }
        /// <summary>
        /// Convert DataTable To ListObject
        /// </summary>
        /// <typeparam name="T">Object Of AnyType</typeparam>
        /// <param name="Table">Datatable</param>
        /// <returns></returns>
        public IList<T> DataTableToListObject<T>(DataTable Table) where T : class, new()
        {
            try
            {
                var dataList = new List<T>(Table.Rows.Count);
                Type classType = typeof(T);
                IList<PropertyInfo> propertyList = classType.GetProperties();
                if (propertyList.Count == 0)
                    return new List<T>();
                List<string> columnNames = Table.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
                try
                {
                    foreach (DataRow dataRow in Table.AsEnumerable().ToList())
                    {
                        var classObject = new T();
                        foreach (PropertyInfo property in propertyList)
                        {

                            if (property != null && property.CanWrite)
                            {
                                if (columnNames.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                                {
                                    if (dataRow[property.Name] != System.DBNull.Value)
                                    {
                                        object propertyValue = System.Convert.ChangeType(
                                                dataRow[property.Name],
                                                property.PropertyType
                                            );
                                        property.SetValue(classObject, propertyValue, null);
                                    }

                                }
                            }
                        }
                        dataList.Add(classObject);
                    }
                    return dataList;
                }

                catch (Exception)
                {
                    return new List<T>();
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public String FilterString(string strVal)
        {
            var str = FilterQuote(strVal);

            if (str.ToLower() != "null")
                str = "'" + str + "'";

            return str.TrimEnd().TrimStart();
        }
        public object ConvertType<T>(object source)
        {
            Type t = typeof(T);
            return Convert.ChangeType(source, t);
        }
        public T JsonToModel<T>(string json)
        {
            T model = default(T);
            model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
        public T ObjectToModel<T>(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            T model = default(T);
            model = JsonConvert.DeserializeObject<T>(json);
            return model;
        }
    }
}
