using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectionsManager
{
    public class ConnectionCollection : IEnumerable<ConnectionManager>, IDisposable
    {
        #region Properties

        /// <summary>
        /// Use to add or remove ConnectionItem instances to a Connection.
        /// </summary>
        protected Dictionary<string, ConnectionManager> Items = new Dictionary<string, ConnectionManager>();

        #endregion


        #region Methods

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


        /// <summary>
        /// Remove a Connection instance from the Connection.
        /// </summary>
        /// <param name="item">The Connection.</param>
        /// <returns></returns>
        public bool Remove(Connection item)
        {
            return Items.Remove(item.Name.ToUpper());
        }


        public bool Contains(string connName)
        {
            return Items.ContainsKey(connName.ToUpper());
        }


        public bool Contains(Connection item)
        {
            return Contains(item.Name.ToUpper()) && Items[item.Name.ToUpper()].ConnectionString == item.ConnectionString;
        }


        /// <summary>
        /// Add a new Connection instance.  
        /// Add ConnectionItems to the Connection instance before adding it to the Connection.
        /// </summary>
        /// <param name="conn">The Connection.</param>
        /// <returns></returns>
        public ConnectionManager Add(Connection conn)
        {
            var cm = new ConnectionManager(conn);

            if (Items.ContainsKey(cm.Name.ToUpper())) // Exist Connection, so update old Connection
                SetValue(cm);
            else // New Connection
                Items.Add(cm.Name.ToUpper(), cm);

            return cm;
        }


        public void Clear()
        {
            Items.Clear();
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

        
        #endregion


        #region Implement IDisposable

        public void Dispose()
        {
            foreach (var conn in this)
                conn.Dispose();

            Clear();
        }

        #endregion

        #region Implement IEnumerable<ConnectionManager>

        IEnumerator<ConnectionManager> IEnumerable<ConnectionManager>.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        #endregion

    }
}