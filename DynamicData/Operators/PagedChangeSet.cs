using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData.Kernel;

namespace DynamicData.Operators
{
    internal sealed class PagedChangeSet<TObject, TKey> : ChangeSet<TObject, TKey>, IPagedChangeSet<TObject, TKey>, IEquatable<PagedChangeSet<TObject, TKey>>
    {
        public static bool operator ==(PagedChangeSet<TObject, TKey> left, PagedChangeSet<TObject, TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PagedChangeSet<TObject, TKey> left, PagedChangeSet<TObject, TKey> right)
        {
            return !Equals(left, right);
        }

        private readonly IKeyValueCollection<TObject, TKey> _sortedItems;
        private readonly IPageResponse _response;

        public PagedChangeSet(IKeyValueCollection<TObject, TKey> sortedItems, IEnumerable<Change<TObject, TKey>> updates, IPageResponse response)
            : base(updates)
        {
            _response = response;
            _sortedItems = sortedItems;
        }

        public IKeyValueCollection<TObject, TKey> SortedItems
        {
            get { return _sortedItems; }
        }

        public IPageResponse Response
        {
            get { return _response; }
        }


        #region Equality Members

        public bool Equals(PagedChangeSet<TObject, TKey> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_sortedItems, other._sortedItems);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is PagedChangeSet<TObject, TKey> && Equals((PagedChangeSet<TObject, TKey>) obj);
        }

        public override int GetHashCode()
        {
            return (_sortedItems != null ? _sortedItems.GetHashCode() : 0);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the SortedItems <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the SortedItems <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Response: {1}, SortedItems: {2}", base.ToString(), _response, _sortedItems);
        }
    }
}