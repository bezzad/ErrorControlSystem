using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace ConnectionsManager
{
    public class Connection : ICloneable
    {
        #region Fields

        private string _server;
        #endregion


        #region Properties

        public string UserId { get; set; }

        public string Password { get; set; }

        [DefaultValue("localhost")]
        public string Server
        {
            get { return _server; }
            set
            {
                _server = (string.IsNullOrEmpty(value) || value == ".")
                    ? "localhost"
                    : value;
            }
        }

        [DefaultValue("Master")]
        public string DatabaseName { get; set; }

        [DefaultValue(typeof(int), "1433")]
        public int PortNumber { get; set; }

        [DefaultValue("System.Data.SqlClient")]
        public string ProviderName { get; set; }

        [DefaultValue("SSPI")]
        public string IntegratedSecurity { get; set; }

        [DefaultValue(true)]
        public bool PersistSecurityInfo { get; set; }

        public string AttachDbFilename { get; set; }

        [DefaultValue(30)]
        public int TimeOut { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public int Id { get; protected set; }

        [DefaultValue(false)]
        public bool IsReady { get; protected set; }

        #endregion


        #region Read-only Properties

        public string ConnectionString
        {
            get
            {
                // Create Connection String
                return
                        String.Format("Data Source={0}{1};{2}{3}{4}",
                            Server,
                            PortNumber == 1433 || PortNumber == 0 ? "" : string.Format(",{0}", PortNumber),
                            string.IsNullOrEmpty(DatabaseName) ? "" : string.Format("Initial Catalog={0};", DatabaseName),
                            string.IsNullOrEmpty(UserId) ?
                                string.Format("Integrated Security={0};", IntegratedSecurity) :
                                string.Format("Persist Security Info={0};User ID={1};Password={2};", PersistSecurityInfo, UserId, Password),
                            TimeOut <= 0 ? "" : string.Format("Connection Timeout={0};", TimeOut));
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new NullReferenceException("You can't pass null reference object for ConnectionString property!");

                var conn = Parse(value);

                Clone(conn, this);
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="connectionName">The name of Connection to call that in from other usage classes</param>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="databaseName">The database name or initialCatalog.</param>
        /// <param name="timeOut">The time out always grater than zero 0.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="portNumber">The server port no. <example>default port for SQL is 1433</example></param>
        /// <param name="description">Gets or sets the description of the Connection object.</param>
        /// <param name="attachDbFilename">The attach database filename.</param>
        /// <param name="providerName">Name of the provider.<example>"System.Data.SqlClient"</example></param>
        public Connection(string connectionName, string server, string databaseName,
            int timeOut, string username,
            string pass, int portNumber, string description,
            string attachDbFilename, string providerName = "System.Data.SqlClient")
        {
            #region initialing

            Name = connectionName;

            Server = server;

            DatabaseName = databaseName;

            TimeOut = timeOut;

            UserId = username;

            Password = pass;

            PortNumber = portNumber;

            AttachDbFilename = attachDbFilename;

            Description = description;

            ProviderName = providerName;

            IntegratedSecurity = "sspi";

            PersistSecurityInfo = true;

            Id = GetUniqueId();

            #endregion
        }

        public Connection(string connectionName, string server, string databaseName,
            int timeOut, string username,
            string pass, int portNumber = 1433, string description = "")
            : this(connectionName, server, databaseName,
                timeOut, username,
                pass, portNumber, description, string.Empty)
        { }

        public Connection(string connectionName, string server, string databaseName,
            int timeOut = 30, string description = "")
            : this(connectionName, server, databaseName,
                timeOut, description, string.Empty)
        { }

        public Connection(string connectionName, string server, int timeOut, string description = "")
            : this(connectionName, server, "master",
                timeOut, description, string.Empty)
        { }


        public Connection(string connectionName, string connString)
        {
            ConnectionString = connString;

            Name = connectionName;
        }


        public Connection(string connectionName)
            : this()
        {
            Name = connectionName;
        }

        public Connection(Connection conn)
        {
            Clone(conn, this);
        }

        protected Connection()
        {
            SetToDefaultConnection();
        }
        #endregion


        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// <example>
        ///     <connectionStrings>
        ///         <add name="TestConnectionString"
        ///              connectionString="Data Source=localhost;Initial Catalog=Test;Persist Security Info=True;User ID=sa;Password=p12345"
        ///              providerName="System.Data.SqlClient" />
        ///     </connectionStrings>
        /// </example>
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool encrypt)
        {
            return ToXml(encrypt).ToString(SaveOptions.None);
        }

        public XElement ToXml(bool encrypt = false)
        {
            return
                new XElement("ConnectionStrings",
                    new XElement("add",
                        new XAttribute("name", encrypt ? Name.Encrypt() : Name),
                        new XAttribute("publicKeyToken", Id),
                        new XAttribute("description", Description ?? "Empty"),
                        new XAttribute("connectionString", encrypt ? "#" + ConnectionString.Encrypt() : ConnectionString),
                        new XAttribute("providerName", encrypt ? ProviderName.Encrypt() : ProviderName)));
        }

        private void SetToDefaultConnection()
        {
            // Use the DefaultValue property of each properties to actually set it, via reflection.
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                var attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];

                if (attr != null)
                {
                    prop.SetValue(this, attr.Value);
                }
            }

            Id = GetUniqueId();
        }

        protected static int GetUniqueId()
        {
            return DateTime.Now.GetHashCode();
        }

        public static Connection Parse(string connectionString)
        {
            if (connectionString.StartsWith("#")) // if connection string starts with '#' then it's Encrypted !
                connectionString = connectionString.Substring(1).Decrypt();


            var scsb = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            var conn = new Connection
            {
                Server = scsb.DataSource,
                DatabaseName = scsb.InitialCatalog,
                TimeOut = connectionString.Contains("Connection Timeout") ? scsb.ConnectTimeout : 0,
                UserId = scsb.UserID,
                Password = scsb.Password,
                PortNumber = 1433,
                AttachDbFilename = scsb.AttachDBFilename,
                PersistSecurityInfo = scsb.PersistSecurityInfo,
                IntegratedSecurity = scsb.IntegratedSecurity ?
                    connectionString.Substring(connectionString.IndexOf("Integrated Security=") + "Integrated Security=".Length, 4) :
                    "false"
            };

            return conn;
        }

        public static Connection Parse(XElement xmlConnection)
        {
            var add = xmlConnection.Element("add");

            return add == null ? ParseXElemntAdd(xmlConnection) : ParseXElemntAdd(add);
        }

        private static Connection ParseXElemntAdd(XElement add)
        {
            var name = add.Attribute("name").Value;

            var publicKeyToken = add.Attribute("publicKeyToken").Value;

            var description = add.Attribute("description").Value;

            var connectionString = add.Attribute("connectionString").Value;

            var providerName = add.Attribute("providerName").Value;

            var encrypted = connectionString.StartsWith("#");

            var conn = Parse(connectionString);

            conn.Name = encrypted ? name.Decrypt() : name;

            conn.ProviderName = encrypted ? providerName.Decrypt() : providerName;

            conn.Id = int.Parse(
                string.IsNullOrEmpty(publicKeyToken)
                    ? GetUniqueId().ToString()
                    : publicKeyToken);

            conn.Description = (description == "Empty") ? "" : description;

            return conn;
        }

        public static implicit operator Connection(string connectionString)
        {
            return Parse(connectionString);
        }

        public static implicit operator Connection(XElement xmlConnection)
        {
            return Parse(xmlConnection);
        }

        public static explicit operator string(Connection conn)
        {
            return conn.ConnectionString;
        }

        public static explicit operator XElement(Connection conn)
        {
            return conn.ToXml();
        }


        #endregion


        #region Implement ICloneable

        public object Clone()
        {
            var connObj = new Connection();

            // copy each value over
            Clone(this, connObj);

            return connObj;
        }

        public static void Clone(Connection source, Connection destination)
        {
            // exceptions
            if (source == null)
                throw new ArgumentNullException("Source");
            if (destination == null)
                throw new ArgumentNullException("Destination");

            // copy each value over
            foreach (var pi in source.GetType().GetProperties()
                .Where(pi => pi.CanRead && pi.CanWrite && pi.Name != "ConnectionString"))
                pi.SetValue(destination, pi.GetValue(source, null), null);
        }

        #endregion
    }
}