//**********************************************************************************
// Creator: T. Shrove
// Date: 7/25/09
// Email: tshrove@gmail.com
// Website: http://www.tshrove.com
// Code Website: http://code.tshrove.com
// This is for use only. Not for sale. If you make any changes to it please email 
// me a copy of the updated source code.
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CafeT.GenericList
{
    /// <summary>
    /// This is a generic list that has added the events:
    /// ItemRemove, ItemAdded, ItemsCleared, BeforeItemAdded, and BeforeItemRemoved
    /// that was not added by default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericList<T> : IList<T>
    {
        #region Members
        private List<T> m_pItems = null;
        #endregion

        #region Events
        /// <summary>
        /// Raises when an item is added to the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> ItemAdded;
        /// <summary>
        /// Raises before an item is added to the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> BeforeItemAdded;
        /// <summary>
        /// Raises when an item is removed from the list.
        /// </summary>
        public event EventHandler<EventArgs> ItemRemoved;
        /// <summary>
        /// Raises before an item is removed from the list.
        /// </summary>
        public event EventHandler<GenericItemEventArgs<T>> BeforeItemRemoved;
        /// <summary>
        /// Raises when the items are cleared from the list.
        /// </summary>
        public event EventHandler<EventArgs> ItemsCleared;
        #endregion

        #region Protected Properties
        /// <summary>
        /// Returns the list of items in the class.
        /// </summary>
        protected List<T> Items
        {
            get { return this.m_pItems; }
            private set { this.m_pItems = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor for the class.
        /// </summary>
        public GenericList()
            :this(0)
        {
            //Nothing to do
        }
        /// <summary>
        /// Constructor that sets the size of the list to the capacity number.
        /// </summary>
        /// <param name="capacity">Number of items you want the list to default to in size.</param>
        public GenericList(int capacity)
        {
            this.Items = new List<T>(capacity);
        }
        #endregion

        #region IList Methods
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
        //     index is not a valid index in the System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     The property is set and the System.Collections.Generic.IList<T> is read-only.
        public T this[int index] 
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }
        // Summary:
        //     Determines the index of a specific item in the System.Collections.Generic.IList<T>.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.IList<T>.
        //
        // Returns:
        //     The index of item if found in the list; otherwise, -1.
        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }
        //
        // Summary:
        //     Inserts an item to the System.Collections.Generic.IList<T> at the specified
        //     index.
        //
        // Parameters:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert into the System.Collections.Generic.IList<T>.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     The System.Collections.Generic.IList<T> is read-only.
        public void Insert(int index, T item)
        {
            OnBeforeItemAdded(this, new GenericItemEventArgs<T>(item));
            this.Items.Insert(index, item);
            OnItemAdded(this, new GenericItemEventArgs<T>(item));
        }
        //
        // Summary:
        //     Removes the System.Collections.Generic.IList<T> item at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the item to remove.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList<T>.
        //
        //   System.NotSupportedException:
        //     The System.Collections.Generic.IList<T> is read-only.
        public void RemoveAt(int index)
        {
            OnBeforeItemRemoved(this, new GenericItemEventArgs<T>(this.Items[index]));
            this.Items.RemoveAt(index);
            OnItemRemoved(this, new EventArgs());
        }
        #endregion

        #region ICollection Methods and Properties
        // Summary:
        //     Gets the number of elements contained in the System.Collections.Generic.ICollection<T>.
        //
        // Returns:
        //     The number of elements contained in the System.Collections.Generic.ICollection<T>.
        public int Count { get { return this.Items.Count; } }
        //
        // Summary:
        //     Gets a value indicating whether the System.Collections.Generic.ICollection<T>
        //     is read-only.
        //
        // Returns:
        //     true if the System.Collections.Generic.ICollection<T> is read-only; otherwise,
        //     false.
        public bool IsReadOnly { get { return false; } }

        // Summary:
        //     Adds an item to the System.Collections.Generic.ICollection<T>.
        //
        // Parameters:
        //   item:
        //     The object to add to the System.Collections.Generic.ICollection<T>.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The System.Collections.Generic.ICollection<T> is read-only.
        public void Add(T item)
        {
            OnBeforeItemAdded(this, new GenericItemEventArgs<T>(item));
            this.Items.Add(item);
            OnItemAdded(this, new GenericItemEventArgs<T>(item));
        }
        //
        // Summary:
        //     Removes all items from the System.Collections.Generic.ICollection<T>.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The System.Collections.Generic.ICollection<T> is read-only.
        public void Clear()
        {
            this.Items.Clear();
            OnItemsCleared(this, new EventArgs());
        }
        //
        // Summary:
        //     Determines whether the System.Collections.Generic.ICollection<T> contains
        //     a specific value.
        //
        // Parameters:
        //   item:
        //     The object to locate in the System.Collections.Generic.ICollection<T>.
        //
        // Returns:
        //     true if item is found in the System.Collections.Generic.ICollection<T>; otherwise,
        //     false.
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }
        //
        // Summary:
        //     Copies the elements of the System.Collections.Generic.ICollection<T> to an
        //     System.Array, starting at a particular System.Array index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements
        //     copied from System.Collections.Generic.ICollection<T>. The System.Array must
        //     have zero-based indexing.
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
        //     array is multidimensional.  -or- arrayIndex is equal to or greater than the
        //     length of array.  -or- The number of elements in the source System.Collections.Generic.ICollection<T>
        //     is greater than the available space from arrayIndex to the end of the destination
        //     array.  -or- Type T cannot be cast automatically to the type of the destination
        //     array.
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }
        //
        // Summary:
        //     Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection<T>.
        //
        // Parameters:
        //   item:
        //     The object to remove from the System.Collections.Generic.ICollection<T>.
        //
        // Returns:
        //     true if item was successfully removed from the System.Collections.Generic.ICollection<T>;
        //     otherwise, false. This method also returns false if item is not found in
        //     the original System.Collections.Generic.ICollection<T>.
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     The System.Collections.Generic.ICollection<T> is read-only.
        public bool Remove(T item)
        {
            OnBeforeItemRemoved(this, new GenericItemEventArgs<T>(item));
            bool happened = this.Items.Remove(item);
            OnItemRemoved(this, new EventArgs());
            return happened;
        }
        #endregion

        #region IEnumerable<T> Methods
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     A System.Collections.Generic.IEnumerator<T> that can be used to iterate through
        //     the collection.
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
        #endregion

        #region IEnumerable Methods
        // Summary:
        //     Returns an enumerator that iterates through a collection.
        //
        // Returns:
        //     An System.Collections.IEnumerator object that can be used to iterate through
        //     the collection.
        // Summary:
        //     Returns an enumerator that iterates through a collection.
        //
        // Returns:
        //     An System.Collections.IEnumerator object that can be used to iterate through
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// Raises when an Item is added to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (ItemAdded != null)
                ItemAdded(sender, e);
        }
        /// <summary>
        /// Raises before an Item is added to the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemAdded(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemAdded != null)
                BeforeItemAdded(sender, e);
        }
        /// <summary>
        /// Raises when an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventsArgs</param>
        protected virtual void OnItemRemoved(object sender, EventArgs e)
        {
            if (ItemRemoved != null)
                ItemRemoved(sender, e);
        }
        /// <summary>
        /// Raises before an Item is removed from the list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">GenericItemEventArgs</param>
        protected virtual void OnBeforeItemRemoved(object sender, GenericItemEventArgs<T> e)
        {
            if (BeforeItemRemoved != null)
                BeforeItemRemoved(sender, e);
        }
        /// <summary>
        /// Raises when the Items are cleared from this list.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected virtual void OnItemsCleared(object sender, EventArgs e)
        {
            if (ItemsCleared != null)
                ItemsCleared(sender, e);
        }
        #endregion
    }

    public class GenericItemEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Item
        /// </summary>
        public T Item { get; private set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="item"></param>
        public GenericItemEventArgs(T item)
        {
            this.Item = item;
        }
    }
}
