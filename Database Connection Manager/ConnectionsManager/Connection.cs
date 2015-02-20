using System;
using System.ComponentModel;
using System.Data.SqlClient;
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

        [DefaultValue("")]
        public string UserId { get; set; }

        [DefaultValue("")]
        public string Password { get; set; }

        [DefaultValue("localhost")]
        public string Server
        {
            get { return _server; }
            set
            {
                _server = (String.IsNullOrEmpty(value) || value == ".")
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

        [DefaultValue("")]
        public string AttachDbFilename { get; set; }

        [DefaultValue(30)]
        public int TimeOut { get; set; }

        [DefaultValue("")]
        public string Description { get; set; }

        [DefaultValue("")]
        public string Name { get; set; }

        public int Id { get; protected set; }

        [DefaultValue(false)]
        public bool IsReady { get; set; }

        #endregion


        #region Static Properties

        /// <summary>
        /// Use to add or remove ConnectionItem instances to a Connection.
        /// </summary>
        internal volatile static ConnectionCollection Items = new ConnectionCollection();

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
                            PortNumber == 1433 || PortNumber == 0 ? "" : String.Format(",{0}", PortNumber),
                            String.IsNullOrEmpty(DatabaseName) ? "" : String.Format("Initial Catalog={0};", DatabaseName),
                            String.IsNullOrEmpty(UserId) ?
                                String.Format("Integrated Security={0};", IntegratedSecurity) :
                                String.Format("Persist Security Info={0};User ID={1};Password={2};", PersistSecurityInfo, UserId, Password),
                            TimeOut <= 0 ? "" : String.Format("Connection Timeout={0};", TimeOut));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new NullReferenceException("You can't pass null reference object for ConnectionString property!");

                var conn = Parse(value);

                Clone(conn, this);
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="timeOut">The time out always grater than zero 0.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="description">Gets or sets the description of the Connection object.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="attachDbFilename">The attach database filename.</param>
        /// <param name="provider">Name of the provider.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database,
            string username, string pass, int timeOut, string description,
            int portNumber, string attachDbFilename, string provider, string name)
        {
            #region initialing

            Name = name;

            Server = server;

            DatabaseName = database;

            TimeOut = timeOut;

            UserId = username;

            Password = pass;

            PortNumber = portNumber;

            AttachDbFilename = attachDbFilename;

            Description = description;

            ProviderName = provider;

            IntegratedSecurity = "sspi";

            PersistSecurityInfo = true;

            Id = GetUniqueId();

            #endregion
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="timeOut">The time out always grater than zero 0.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="description">Gets or sets the description of the Connection object.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database,
            string username, string pass, int timeOut, string description, string name)
            : this(server, database, username, pass, timeOut, description, 1433, "", "System.Data.SqlClient", name)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="timeOut">The time out always grater than zero 0.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database,
            string username, string pass, int timeOut, string name = "")
            : this(server, database, username, pass, timeOut, "", 1433, "", "System.Data.SqlClient", name)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="description">Gets or sets the description of the Connection object.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database,
            string username, string pass, string description, string name)
            : this(server, database, username, pass, 10, description, 1433, "", "System.Data.SqlClient", name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="username">The database author user name.</param>
        /// <param name="pass">The database author password.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database,
            string username, string pass, string name = "")
            : this(server, database, username, pass, 10, "", 1433, "", "System.Data.SqlClient", name)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection" /> class.
        /// </summary>
        /// <param name="server">The server name or IP or data source.</param>
        /// <param name="database">The database name or initialCatalog.</param>
        /// <param name="name">Name of the connection</param>
        public Connection(string server, string database, string name = "")
            : this(server, database, "", "", 10, "", 1433, "", "System.Data.SqlClient", name)
        { }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// Server is <c>Master</c>
        /// </summary>
        public Connection()
            : this("master", "", "", "", 10, "", 1433, "", "System.Data.SqlClient", "")
        {
            SetToDefaultConnection();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        public Connection(string connString)
        {
            ConnectionString = connString;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="conn">The connection object.</param>
        public Connection(Connection conn)
        {
            Clone(conn, this);
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


            var scsb = new SqlConnectionStringBuilder(connectionString);

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
                    connectionString.Substring(connectionString.IndexOf("Integrated Security=") +
                    "Integrated Security=".Length, 4) :
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

            conn.Id = Int32.Parse(
                String.IsNullOrEmpty(publicKeyToken)
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