using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace ConnectionsManager
{
    public class ConnectionCollection : ICollection<Connection>, IDisposable
    {
        #region Properties

        /// <summary>
        /// Use to add or remove ConnectionItem instances to a Connection.
        /// </summary>
        protected Dictionary<string, ConnectionManager> Items;

        #endregion


        #region Methods

        public ConnectionCollection()
        {
            Items = new Dictionary<string, ConnectionManager>();
        }

        public void SetValue(Connection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            Items[conn.Name.ToUpper()] = new ConnectionManager(conn);
        }

        private void SetValue(string name, Connection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            Items[name.ToUpper()] = new ConnectionManager(conn);
        }

        public ConnectionManager this[string connectionName]
        {
            get { return Find(connectionName); }

            set { SetValue(connectionName, value); }
        }

        /// <summary>
        /// Find a Connection instance using name and server type.
        /// <param name="connectionName">The Connection name.</param>
        /// </summary>
        /// <returns>If the connection name is exist then return Connection, either not exist return null</returns>
        public ConnectionManager Find(string connectionName)
        {
            return Items.ContainsKey(connectionName.ToUpper()) ? Items[connectionName.ToUpper()] : null;
        }

        /// <summary>
        /// Remove a Connection instance from the Connection.
        /// </summary>
        /// <param name="name">The Connection name</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            return Items.Remove(name.ToUpper());
        }

        public bool Contains(string connName)
        {
            return Items.ContainsKey(connName.ToUpper());
        }

        public IEnumerable<ConnectionManager> GetConnectionManagers
        {
            get { return Items.Values; }
        }

        #endregion


        #region Implement ICollection<Connection>

        /// <summary>
        /// Add a new Connection instance.  
        /// Add ConnectionItems to the Connection instance before adding it to the Connection.
        /// </summary>
        /// <param name="conn">The Connection.</param>
        /// <returns></returns>
        public void Add(Connection conn)
        {
            if (Items.ContainsKey(conn.Name.ToUpper())) // Exist Connection, so update old Connection
                SetValue(conn);
            else // New Connection
                Items.Add(conn.Name.ToUpper(), new ConnectionManager(conn));
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(Connection item)
        {
            return Contains(item.Name.ToUpper()) && Items[item.Name.ToUpper()].ConnectionString == item.ConnectionString;
        }

        /// <summary>
        /// Copy this Array to destining array from arrayIndex
        /// </summary>
        /// <param name="array">destination array for copy to that</param>
        /// <param name="arrayIndex">Beginning point of array</param>
        /// <exception cref="System.ArgumentNullException">If the array is null</exception>
        /// <exception cref="System.ArgumentException">If the array is not 1D, so Rank is not less or greater than 1</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the arrayIndex is less than 0</exception>
        /// <exception cref="System.ArgumentException">If the array.Length - arrayIndex is less than sourceArray.Count()</exception>
        public void CopyTo(Connection[] array, int arrayIndex)
        {
            Items.Values.ToArray<Connection>().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Remove a Connection instance from the Connection.
        /// </summary>
        /// <param name="item">The Connection.</param>
        /// <returns></returns>
        public bool Remove(Connection item)
        {
            return Items.Remove(item.Name.ToUpper());
        }

        IEnumerator<Connection> IEnumerable<Connection>.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        #endregion

        #region Implement IDisposable

        public void Dispose()
        {
            foreach (var conn in GetConnectionManagers)
                conn.Dispose();

            Clear();
        }

        #endregion
    }
}