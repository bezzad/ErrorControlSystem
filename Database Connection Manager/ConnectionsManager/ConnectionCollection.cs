using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectionsManager
{
    public class ConnectionCollection : ICollection<Connection>, IEnumerator<Connection>
    {
        #region Properties

        private int _current;

        /// <summary>
        /// Use to add or remove ConnectionItem instances to a Connection.
        /// </summary>
        private Dictionary<string, Connection> _items;

        public IEnumerable<ConnectionManager> AllConnectionManagers
        {
            get
            {
                return _items.Values.Where(conn => conn.Id > 0).Select(conn => new ConnectionManager(conn));
            }
        }

        #endregion


        #region Methods
        public ConnectionCollection()
        {
            // Enumerators are positioned before the first element 
            // until the first MoveNext() call. 
            _current = -1;

            _items = new Dictionary<string, Connection>();
        }

        public void SetValue(Connection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            _items[conn.Name.ToUpper()] = conn;
        }

        private void SetValue(string name, Connection conn)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _items[name.ToUpper()] = conn;
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
            return _items.ContainsKey(connectionName.ToUpper()) ? new ConnectionManager(_items[connectionName.ToUpper()]) : null;
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
            if (_items.ContainsKey(conn.Name.ToUpper())) // Exist Connection, so update old Connection
                SetValue(conn);
            else // New Connection
                _items.Add(conn.Name.ToUpper(), conn);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Connection item)
        {
            return Contains(item.Name.ToUpper()) && _items[item.Name.ToUpper()].ConnectionString == item.ConnectionString;
        }

        public bool Contains(string connName)
        {
            return _items.ContainsKey(connName.ToUpper());
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
            _items.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
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
            return _items.Remove(item.Name.ToUpper());
        }

        /// <summary>
        /// Remove a Connection instance from the Connection.
        /// </summary>
        /// <param name="name">The Connection name</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            return _items.Remove(name.ToUpper());
        }

        public IEnumerator<Connection> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        #endregion

        #region Implement IEnumerator<ConnectionManager>

        public Connection Current
        {
            get { return _items.ElementAt(_current).Value; }
        }

        public void Dispose()
        {
            foreach (KeyValuePair<string, Connection> item in _items)
            {
                item.Value.Dispose();
            }

            Clear();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _items.ElementAt(_current).Value; }
        }

        public bool MoveNext()
        {
            _current++;
            return _items.Count != 0 && _items.Count > _current;
        }

        public void Reset()
        {
            _current = -1;
        }


        #endregion
    }
}
