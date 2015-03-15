using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbConnectionsManager
{
    public class ConnectionManager : IDisposable
    {
        #region Properties

        private static string DefaultConnectionName { get; set; }

        public Connection Connection { get; protected set; }

        public string ConnectionString { get { return Connection.ConnectionString; } }

        public bool IsReady { get { return Connection.IsReady; } protected set { Connection.IsReady = value; } }

        public System.Data.ConnectionState State { get { return SqlConn.State; } }

        /// <summary>
        /// Get SqlConnection Server Version.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Maybe the object throw this exception</exception>
        public string ServerVersion
        {
            get { return SqlConn.ServerVersion; }
        }

        public SqlTransaction Transaction { get; protected set; }

        public System.Data.SqlClient.SqlConnection SqlConn { get; protected set; }

        #endregion


        #region Constructors

        public ConnectionManager(Connection connectionItem)
        {
            Connection = connectionItem;

            SqlConn = new SqlConnection(Connection.ConnectionString);
        }

        #endregion


        #region Methods


        public bool CheckDbConnection()
        {
            //
            // Clear all pool connected  (connection without reference address)
            SqlConnection.ClearAllPools();
            //
            // Check connection string validation
            if (string.IsNullOrEmpty(ConnectionString) || !IsServerOnline()) return IsReady = false;
            //
            // Try to Create provider factory for connect to Database
            try
            {
                DbProviderFactories.GetFactory(Connection.ProviderName);
            }
            catch
            {
                return IsReady = false;
            }

            //
            // try to connection to database and open link:
            try
            {
                Open();

                IsReady = (State == ConnectionState.Open);
            }
            catch
            {
                IsReady = false;
            }
            finally
            {
                Close();
            }

            return IsReady;
        }

        public async Task<bool> CheckDbConnectionAsync()
        {
            //
            // Clear all pool connected  (connection without reference address)
            SqlConnection.ClearAllPools();
            //
            // Check connection string validation
            if (string.IsNullOrEmpty(ConnectionString) || !await IsServerOnlineAsync()) return IsReady = false;
            //
            // Try to Create provider factory for connect to Database
            try
            {
                DbProviderFactories.GetFactory(Connection.ProviderName);
            }
            catch
            {
                return IsReady = false;
            }

            //
            // try to connection to database and open link:
            try
            {
                await OpenAsync();

                IsReady = (State == ConnectionState.Open);
            }
            catch
            {
                IsReady = false;
            }
            finally
            {
                Close();
            }

            return IsReady;
        }

        /// <summary>
        ///     Check giver server name by all connected server on the this network and find that.
        /// </summary>
        /// <returns>If find serverName in network then return True, otherwise return False.</returns>
        public bool IsServerOnline()
        {
            if (SqlConn.DataSource == null) return false;

            if (SqlConn.DataSource.Equals("localhost", StringComparison.OrdinalIgnoreCase)) return true;

            if (!IsNetworkAvailable()) return false;

            using (var dataSources = SqlDataSourceEnumerator.Instance.GetDataSources())
            {
                var serverName = SqlConn.DataSource.Contains(@"\")
                        ? SqlConn.DataSource.Substring(0, SqlConn.DataSource.IndexOf(@"\", System.StringComparison.Ordinal))
                        : SqlConn.DataSource;

                var isOn = dataSources.Rows.Cast<DataRow>().Any(row =>
                    string.Equals(row["ServerName"].ToString(), serverName, StringComparison.OrdinalIgnoreCase));

                if (!isOn) IsReady = false; // Just set IsReady to false when actually server is not found!

                return isOn;
            }
        }

        #region Check Network Availability
        /// <summary>
        /// Indicates whether any network connection is available
        /// </summary>
        /// <returns>
        ///     <c>true</c> if a network connection is available; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNetworkAvailable()
        {
            // only recognizes changes related to Internet adapters
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                // however, this will include all adapters
                return
                    NetworkInterface.GetAllNetworkInterfaces().
                    Where(face => face.OperationalStatus == OperationalStatus.Up).
                    Any(face =>
                        (face.NetworkInterfaceType != NetworkInterfaceType.Tunnel) &&
                        (face.NetworkInterfaceType != NetworkInterfaceType.Loopback));
            }

            return false;
        }
        #endregion

        /// <summary>
        ///     Check giver server name by all connected server on the this network and find that.
        /// </summary>
        /// <returns>If find serverName in network then return True, otherwise return False.</returns>
        public async Task<bool> IsServerOnlineAsync()
        {
            return await Task.Run(() => IsServerOnline());
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// The Connection was not Closed.
        /// The Connection current state is Open.
        /// </exception>
        public void Open()
        {
            SqlConn.Open();
        }

        /// <summary>
        /// Open connection the asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The Connection was not Closed.
        /// The Connection current state is Open.
        /// </exception>
        public async Task OpenAsync(System.Threading.CancellationToken cancellationToken)
        {
            //
            // Can not use Lock block for async methods
            await SqlConn.OpenAsync(cancellationToken);
        }

        /// <summary>
        /// Open connection the asynchronous.
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The Connection was not Closed.
        /// The Connection current state is Open.
        /// </exception>
        public async Task OpenAsync()
        {
            //
            // Can not use Lock block for async methods
            await SqlConn.OpenAsync();
        }

        /// <summary>
        /// Open connection the asynchronous.
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The Connection was not Open.
        /// The Connection current state is Closed.
        /// </exception>
        public void Close()
        {
            SqlConn.Close();
        }

        public SqlCommand CreateCommand()
        {
            return SqlConn.CreateCommand();
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] Params)
        {
            try
            {
                var ds = new DataSet();

                using (var da = new SqlDataAdapter())
                {
                    var cmd = CreateCommand();

                    cmd.CommandType = commandType;
                    cmd.CommandText = commandText;

                    if (Transaction != null && Transaction != default(SqlTransaction))
                        cmd.Transaction = Transaction;
                    else
                        cmd.Connection = SqlConn;

                    if (Params != null && Params.Length > 0)
                    {
                        foreach (var param in Params)
                            cmd.Parameters.Add(param);
                    }

                    da.SelectCommand = cmd;

                    Open();

                    da.Fill(ds);

                    return ds;
                }
            }
            finally
            {
                Close();
            }
        }

        public T ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] Params)
        {
            try
            {
                var cmd = CreateCommand();

                cmd.CommandType = commandType;
                cmd.CommandText = commandText;

                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                var retVal = cmd.ExecuteScalar();

                if (retVal is T)
                    return (T)retVal;
                else if (retVal == DBNull.Value)
                    return default(T);
                else
                    throw new Exception("Object returned was of the wrong type.");

            }
            finally
            {
                Close();
            }

        }

        public T ExecuteScalar<T>(SqlCommand cmd, params SqlParameter[] Params)
        {
            try
            {
                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                var retVal = cmd.ExecuteScalar();

                if (retVal is T)
                    return (T)retVal;
                else if (retVal == DBNull.Value)
                    return default(T);
                else
                    throw new Exception("Object returned was of the wrong type.");

            }
            finally
            {
                Close();
            }

        }

        public async Task<T> ExecuteScalarAsync<T>(SqlCommand cmd, params SqlParameter[] Params)
        {
            try
            {
                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                await OpenAsync();

                var retVal = await cmd.ExecuteScalarAsync();

                if (retVal is T)
                    return (T)retVal;
                else if (retVal == DBNull.Value)
                    return default(T);
                else
                    throw new Exception("Object returned was of the wrong type.");

            }
            finally
            {
                Close();
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] Params)
        {
            try
            {
                var cmd = CreateCommand();

                cmd.CommandType = commandType;
                cmd.CommandText = commandText;

                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                var retVal = await cmd.ExecuteScalarAsync();

                if (retVal is T)
                    return (T)retVal;
                else if (retVal == DBNull.Value)
                    return default(T);
                else
                    throw new Exception("Object returned was of the wrong type.");
            }
            finally
            {
                Close();
            }
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] Params)
        {
            try
            {
                var cmd = CreateCommand();

                cmd.CommandType = commandType;
                cmd.CommandText = commandText;

                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Close();
            }
        }

        public int ExecuteNonQuery(SqlCommand cmd, params SqlParameter[] Params)
        {
            try
            {
                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Close();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd, params SqlParameter[] Params)
        {
            try
            {
                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                await OpenAsync();

                return await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                Close();
            }
        }

        public SqlDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, params SqlParameter[] Params)
        {
            try
            {
                var cmd = CreateCommand();

                cmd.CommandType = commandType;
                cmd.CommandText = commandText;

                if (Transaction != null && Transaction != default(SqlTransaction))
                    cmd.Transaction = Transaction;
                else
                    cmd.Connection = SqlConn;

                if (Params != null && Params.Length > 0)
                {
                    foreach (var param in Params)
                        cmd.Parameters.Add(param);
                }

                Open();

                return cmd.ExecuteReader();

            }
            finally
            {
                Close();
            }
        }


        public bool BeginTransaction()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Closed && Transaction == null)
            {
                Open();
                Transaction = SqlConn.BeginTransaction();
                return true;
            }

            return false;
        }

        public bool RollBackTransaction()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Open && Transaction != null)
            {
                Transaction.Rollback();
                Close();

                Transaction.Dispose();
                Transaction = default(SqlTransaction);
                return true;
            }

            return false;
        }

        public bool CommitSqlTransaction()
        {
            if (SqlConn != null && SqlConn.State == ConnectionState.Open && Transaction != null)
            {
                Transaction.Commit();
                Close();

                Transaction.Dispose();
                Transaction = default(SqlTransaction);
                return true;
            }
            return false;
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// Returns a <see cref="System.Xml.Linq.XElement" /> that represents this instance.
        /// <example>
        ///     <connectionStrings>
        ///         <add name="TestConnectionString1"
        ///              connectionString="Data Source=localhost;Initial Catalog=Test;Persist Security Info=True;User ID=sa;Password=p12345"
        ///              providerName="System.Data.SqlClient" />
        ///         <add name="TestConnectionString2"
        ///              connectionString="Data Source=Behzad-PC;Initial Catalog=Test;Persist Security Info=True;User ID=sa;Password=123"
        ///              providerName="System.Data.SqlClient" />
        ///         <add name="TestConnectionString3"
        ///              connectionString="Data Source=Frosh;Initial Catalog=Test;Integration Security=True"
        ///              providerName="System.Data.SqlClient" />
        ///     </connectionStrings>
        /// </example>
        /// </summary>
        /// <returns>
        /// A <see cref="System.Xml.Linq.XElement" /> that represents this instance.
        /// </returns>
        public static string SaveToXml(bool encrypt = false)
        {
            return new XElement("ConnectionStrings", Connection.Items.Select(x => x.ToXml(encrypt).Element("add"))).ToString(SaveOptions.None);
        }

        /// <summary>
        /// Load an xml document with ConnectionManager configuration information.
        /// </summary>
        /// <param name="xmlText">The XML text.</param>
        /// <returns></returns>
        public static void LoadFromXml(string xmlText)
        {
            var xmlConnectionStrings = XElement.Parse(xmlText, LoadOptions.None);

            foreach (XElement xmlConn in xmlConnectionStrings.Elements("add"))
            {
                Connection.Items.Add(Connection.Parse(xmlConn));
            }
        }

        /// <summary>
        /// Add a new Connection instance.  
        /// Add ConnectionItems to the Connection instance before adding it to the ConnectionManager.
        /// </summary>
        /// <param name="name">Connection Name</param>
        /// <param name="conn">The connection.</param>
        /// <returns>ConnectionManager</returns>
        public static ConnectionManager Add(Connection conn, string name)
        {
            conn.Name = name;
            return new ConnectionManager(Connection.Items.Add(conn));
        }

        /// <summary>
        /// Edit a new Connection instance.  
        /// Update ConnectionItems to the Connection instance before adding it to the ConnectionManager.
        /// </summary>
        /// <param name="name">Name of connection in list.</param>
        /// <param name="conn">The connection.</param>
        public static void Edit(Connection conn, string name)
        {
            Connection.Items[name] = conn;
        }

        /// <summary>
        /// Edit a new Connection instance.  
        /// Update ConnectionItems to the Connection instance before adding it to the ConnectionManager.
        /// </summary>
        /// <param name="conn">The connection.</param>
        public static void Edit(Connection conn)
        {
            Edit(conn, conn.Name);
        }

        public static bool Remove(string connName)
        {
            return Connection.Items.Remove(connName);
        }

        public static bool Remove(ConnectionManager cm)
        {
            return Connection.Items.Remove(cm.Connection.Name);
        }

        public static void ClearAll()
        {
            Connection.Items.Clear();
        }

        /// <summary>
        /// Find a Connection instance using name and server type.
        /// <param name="connectionName">The connection name.</param>
        /// </summary>
        /// <returns></returns>
        public static ConnectionManager Find(string connectionName)
        {
            return new ConnectionManager(Connection.Items.Find(connectionName));
        }

        public static int Count
        {
            get { return Connection.Items.Count; }
        }

        public static ConnectionManager Factory(Connection conn)
        {
            return new ConnectionManager(conn);
        }

        public static IEnumerable<Connection> GetConnections()
        {
            return Connection.Items;
        }

        public static ConnectionManager GetDefaultConnection()
        {
            return Find(DefaultConnectionName);
        }

        public static void SetToDefaultConnection(string name)
        {
            DefaultConnectionName = name;
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            if (SqlConn != null && Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
            }

            // Please don't use of this code in finalize code.
            //      if (SqlConn.State == ConnectionState.Open)   SqlConn.Close();
            //
            // Because that's have Error:     'Internal .Net Framework Data Provider error 1'
            //
            SqlConn = null;
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        ~ConnectionManager()
        {
            Dispose();
        }

        #endregion
    }
}