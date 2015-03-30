using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;

namespace Shared
{
    public class CurrentProcesses : IList<String>, ICloneable
    {
        #region Variables Declarations
        protected IList<String> CollectionStrings { get; set; }
        #endregion

        #region Constructor
        public CurrentProcesses()
        {
            CollectionStrings = System.Diagnostics.Process.GetProcesses().Select(x => x.ProcessName).ToList();
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return String.Join(", ", CollectionStrings);
        }

        public static List<String> Parse(string fromString)
        {
            List<String> result;

            TryParse(fromString, out result);

            return result;
        }

        public static bool TryParse(string fromString, out List<String> result)
        {
            // Help Link:  http://regexpal.com/

            // Clear result:
            result = null;

            if (string.IsNullOrEmpty(fromString)) return false;

            var succes = true;

            try
            {
                // Example CurrentProcesses.ToString() list's:
                //
                // system, myApp, ErrorControlSystem, explorer, sqlservr, devenv, ...
                //
                var matches = Regex.Matches(fromString, @"[\w]*", RegexOptions.Singleline);

                if (matches.Count > 0)
                {
                    var mathesArray = new string[matches.Count];
                    matches.CopyTo(mathesArray, 0);
                    result = mathesArray.ToList();
                }
                else succes = false;
            }
            catch { succes = false; }

            return succes;
        }
        #endregion


        #region Implement Interface IList<ExtenisonButton>

        // Summary:
        //     Inserts an element into the System.Collections.Generic.List<String> at the specified
        //     index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert. The value can be null for reference types.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is greater than System.Collections.Generic.List<String>.Count.
        public void Insert(int index, String item)
        {
            CollectionStrings.Insert(index, item);
        }

        // Summary:
        //     Inserts the elements of a collection into the System.Collections.Generic.List<String>
        //     at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which the new elements should be inserted.
        //
        //   collection:
        //     The collection whose elements should be inserted into the System.Collections.Generic.List<String>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is greater than System.Collections.Generic.List<String>.Count.
        public void InsertRange(int index, IEnumerable<String> collection)
        {
            foreach (var item in collection.Reverse())
                Insert(index, item);
        }

        // Summary:
        //     Removes all the elements that match the conditions defined by the specified
        //     predicate.
        //
        // Parameters:
        //   match:
        //     The System.Predicate<String> delegate that defines the conditions of the elements
        //     to remove.
        //
        // Returns:
        //     The number of elements removed from the System.Collections.Generic.List<String>
        //     .
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     match is null.
        public int RemoveAll(Predicate<String> match)
        {
            var counter = 0;

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            this.Where(entity => match(entity))
                      .ToList().ForEach(entity => { Remove(entity); counter++; });

            return counter;
        }

        // Summary:
        //     Removes the element at the specified index of the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<String>.Count.
        public void RemoveAt(int index)
        {
            CollectionStrings.RemoveAt(index);
        }

        bool ICollection<String>.Remove(String item)
        {
            return CollectionStrings.Remove(item);
        }

        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   item:
        //     The object to remove from the System.Collections.Generic.List<String>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     true if item is successfully removed; otherwise, false. This method also
        //     returns false if item was not found in the System.Collections.Generic.List<String>.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Remove(String item)
        {
            return CollectionStrings.Remove(item);
        }

        // Summary:
        //     Removes a range of elements from the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   index:
        //     The zero-based starting index of the range of elements to remove.
        //
        //   count:
        //     The number of elements to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        //     index and count do not denote a valid range of elements in the System.Collections.Generic.List<String>.
        public void RemoveRange(int index, int count)
        {
            var buffer = new List<String>();

            for (var i = index; i < index + count; i++)
                buffer.Add(this[i]);

            foreach (var anyItem in buffer)
                Remove(anyItem);
        }

        // Summary:
        //     Gets or sets the element at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the element to get or set.
        //
        // Returns:
        //     The element at the specified index.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-index is equal to or greater than System.Collections.Generic.List<String>.Count.
        public String this[int index]
        {
            get
            {
                return CollectionStrings[index];
            }
            set
            {
                CollectionStrings[index] = value;
            }
        }

        // Summary:
        //     Removes all elements from the System.Collections.Generic.List<String>.
        public void Clear()
        {
            CollectionStrings.Clear();
        }

        // Summary:
        //     Determines whether an element is in the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.List<String>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     true if item is found in the System.Collections.Generic.List<String>; otherwise,
        //     false.
        public bool Contains(String item)
        {
            return CollectionStrings.Contains(item);
        }

        // Summary:
        //     Copies the entire System.Collections.Generic.List<String> to a compatible one-dimensional
        //     array, starting at the beginning of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.Generic.List<String>. The System.Array must have
        //     zero-based indexing.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.List<String> is
        //     greater than the number of elements that the destination array can contain.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public void CopyTo(String[] array)
        {
            array = new String[Count];

            for (var index = 0; index < Count; index++)
                CollectionStrings.CopyTo(array, index);
        }

        // Summary:
        //     Copies the entire System.Collections.Generic.List<String> to a compatible one-dimensional
        //     array, starting at the specified index of the target array.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.Generic.List<String>. The System.Array must have
        //     zero-based indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     array is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.List<String> is
        //     greater than the available space from arrayIndex to the end of the destination
        //     array.
        public void CopyTo(String[] array, int arrayIndex)
        {
            CollectionStrings.CopyTo(array, arrayIndex);
        }

        // Summary:
        //     Gets the number of elements actually contained in the System.Collections.Generic.List<String>.
        //
        // Returns:
        //     The number of elements actually contained in the System.Collections.Generic.List<String>.
        public int Count
        {
            get { return CollectionStrings.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        // Summary:
        //     Returns an enumerator that iterates through the System.Collections.Generic.List<String>.
        //
        // Returns:
        //     A System.Collections.Generic.List<String>.Enumerator for the System.Collections.Generic.List<String>.
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public IEnumerator<String> GetEnumerator()
        {
            return CollectionStrings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return CollectionStrings.GetEnumerator();
        }

        // Summary:
        //     Adds an object to the end of the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   item:
        //     The object to be added to the end of the System.Collections.Generic.List<String>.
        //     The value can be null for reference types.
        public void Add(String item)
        {
            CollectionStrings.Add(item);
        }

        // Summary:
        //     Adds an object to the end of the Process.
        //
        // Parameters:
        //   item:
        //     The object to be added to the end of the System.Collections.Generic.List<String> Processes.
        //     The value can be null for reference types.
        public void Add(Process item)
        {
            CollectionStrings.Add(item.ProcessName);
        }

        // Summary:
        //     Adds the elements of the specified collection to the end of the System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements should be added to the end of the System.Collections.Generic.List<String>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public void AddRange(IEnumerable<String> collection)
        {
            foreach (var extBtn in collection)
            { Add(extBtn); }
        }

        // Summary:
        //     Adds the elements of the specified collection to the end of the System.Collections.Generic.List<Process>.
        //
        // Parameters:
        //   collection:
        //     The collection whose elements should be added to the end of the System.Collections.Generic.List<Process>.
        //     The collection itself cannot be null, but it can contain elements that are
        //     null, if type T is a reference type.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     collection is null.
        public void AddRange(IEnumerable<Process> collection)
        {
            AddRange(collection.Select(x => x.ProcessName));
        }

        // Summary:
        //     Creates a shallow copy of a range of elements in the source System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   index:
        //     The zero-based System.Collections.Generic.List<String> index at which the range
        //     starts.
        //
        //   count:
        //     The number of elements in the range.
        //
        // Returns:
        //     A shallow copy of a range of elements in the source System.Collections.Generic.List<String>.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is less than 0.-or-count is less than 0.
        //
        //   System.ArgumentException:
        //     index and count do not denote a valid range of elements in the System.Collections.Generic.List<String>.
        public List<String> GetRange(int index, int count)
        {
            if (index + count > Count || index < 0 || count <= 0) throw new ArgumentOutOfRangeException();

            var lstRange = new List<String>();

            for (var i = index; i < index + count; i++)
                lstRange.Add(this[i]);

            return lstRange;
        }

        // Summary:
        //     Searches for the specified object and returns the zero-based index of the
        //     first occurrence within the entire System.Collections.Generic.List<String>.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.List<String>. The value
        //     can be null for reference types.
        //
        // Returns:
        //     The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List<String>,
        //     if found; otherwise, –1.
        public int IndexOf(String item)
        {
            return CollectionStrings.IndexOf(item);
        }

        #endregion

        #region Implement Interface ICloneable
        public object Clone()
        {
            return CollectionStrings.Select(x => x.Clone()).ToList() as object;
        }
        #endregion
    }
}