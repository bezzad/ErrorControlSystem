using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ErrorHandlerEngine.CacheHandledErrors;
using ErrorHandlerEngine.ExceptionManager;
using Newtonsoft.Json;

namespace ErrorHandlerEngine.ModelObjecting
{
    public class ErrorUniqueCollection : IList<Error>, IDisposable, ICloneable
    {
        #region Events

        public delegate void ErrorHandler(ErrorUniqueCollection sender);

        public event ErrorHandler OnDataChanged = delegate { };

        #endregion

        #region Private Properties

        private volatile List<long> _errorsKeyIndexer;

        private volatile Dictionary<long, Error> _errors;

        private static readonly object SyncLocker = new object();

        #endregion

        #region Constructor

        public ErrorUniqueCollection()
        {
            _errors = new Dictionary<long, Error>();
            _errorsKeyIndexer = new List<long>();
        }

        public ErrorUniqueCollection(IEnumerable<Error> errors)
            : this()
        {
            this.AddRange(errors.ToArray());
        }


        #endregion

        #region Methods

        public void AddRange(Error[] errors)
        {
            if (errors != null && errors.Any())
                foreach (var err in errors)
                {
                    AddWithout_IO(err);
                }
        }


        public void AddWithout_IO(Error item)
        {
            if (item == null) throw new NoNullAllowedException();

            var key = item.Id;

            if (_errors.ContainsKey(key)) // is duplicate
            {
                _errors[key].Duplicate++;
                _errors[key].IsHandled = item.IsHandled;
                _errors[key].StackTrace = item.StackTrace;
                _errors[key].ErrorDateTime = item.ErrorDateTime;
                _errors[key].ServerDateTime = item.ServerDateTime;
            }
            else
            {
                _errors.Add(key, item);
                _errorsKeyIndexer.Add(key);
            }
        }


        public async Task AddByConcurrencyToFileAsync(Error item)
        {
            Add(item);

            await UpdateFileAsync(this);
        }

        public async Task RemoveByConcurrencyAsync(Error error)
        {
            Remove(error);

            await UpdateFileAsync(this);
        }

        public static async Task UpdateFileAsync(ErrorUniqueCollection euc)
        {
            Kernel.IsSelfException = true;

            var errors = euc.ToList();

            // Convert one Errors list object to JSON string
            var json = await JsonConvert.SerializeObjectAsync(errors, Formatting.Indented);

            await Task.Run(() =>
            {
                lock (SyncLocker)
                {
                    try
                    {
                        RoutingDataStoragePath.WriteTextToLog(json);
                    }
                    finally { Kernel.IsSelfException = false; }
                }
            });

        }
        #endregion

        #region Implement IDisposable

        public void Dispose()
        {
            _errors.Clear();
            _errorsKeyIndexer.Clear();
        }

        #endregion

        #region Implement ICloneable

        public object Clone()
        {
            return new ErrorUniqueCollection(_errors.Select(x => x.Value));
        }

        #endregion

        #region Implement IList<Error>

        public int IndexOf(Error item)
        {
            if (item == null) return -1;

            return _errorsKeyIndexer.IndexOf(item.Id);
        }

        public void Insert(int index, Error item)
        {
            if (item == null) throw new NoNullAllowedException();

            if (_errors.ContainsKey(item.Id)) throw new DuplicateNameException();

            _errorsKeyIndexer.Insert(index, item.Id);
            _errors.Add(item.Id, item);

            OnDataChanged(this);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > _errorsKeyIndexer.Count - 1) throw new ArgumentOutOfRangeException();

            _errors.Remove(_errorsKeyIndexer[index]);
            _errorsKeyIndexer.RemoveAt(index);

            OnDataChanged(this);
        }

        public void Add(Error item)
        {
            AddWithout_IO(item);

            OnDataChanged(this);
        }

        public bool Contains(Error item)
        {
            if (item == null) throw new NoNullAllowedException();

            return _errors.ContainsKey(item.Id);
        }

        public void CopyTo(Error[] array, int arrayIndex)
        {
            _errors.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(Error item)
        {
            if (item == null) throw new NoNullAllowedException();

            if (_errors.ContainsKey(item.Id))
            {
                if (_errors.Remove(item.Id) & _errorsKeyIndexer.Remove(item.Id))
                {
                    OnDataChanged(this);
                    return true;
                }
            }

            return false;
        }

        IEnumerator<Error> IEnumerable<Error>.GetEnumerator()
        {
            return _errors.Values.GetEnumerator();
        }

        public Error this[int index]
        {
            get
            {
                if (index < 0 || index > _errorsKeyIndexer.Count - 1) throw new ArgumentOutOfRangeException();

                return _errors[_errorsKeyIndexer[index]];
            }
            set
            {
                if (index < 0 || index > _errorsKeyIndexer.Count - 1) throw new ArgumentOutOfRangeException();

                _errors[_errorsKeyIndexer[index]] = value;
            }
        }


        public void Clear()
        {
            _errorsKeyIndexer.Clear();
            _errors.Clear();

            OnDataChanged(this);
        }

        public int Count
        {
            get { return _errorsKeyIndexer.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.Values.GetEnumerator();
        }
        #endregion

    }
}