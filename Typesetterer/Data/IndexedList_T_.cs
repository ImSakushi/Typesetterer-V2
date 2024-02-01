using System;
using System.Collections.Generic;

namespace Typesetterer.Data
{
    [Serializable]
    public class IndexedList<T> : List<T>
    {
        private int _index;

        public T Current
        {
            get
            {
                return base[this.Index];
            }
        }

        public T First
        {
            get
            {
                return base[0];
            }
        }

        public int Index
        {
            get
            {
                return this._index;
            }
            set
            {
                if (value >= 0 && value < base.Count)
                {
                    this._index = value;
                    IndexedList<T>.IndexChangedHandler indexChangedHandler = this.IndexChanged;
                    if (indexChangedHandler == null)
                    {
                        return;
                    }
                    indexChangedHandler(this);
                }
            }
        }

        public bool IsNext
        {
            get
            {
                return this.Index + 1 < base.Count;
            }
        }

        public bool IsPrevious
        {
            get
            {
                return this.Index > 0;
            }
        }

        public T Last
        {
            get
            {
                return base[base.Count - 1];
            }
        }

        public T Next
        {
            get
            {
                if (!this.IsNext)
                {
                    return default(T);
                }
                return base[this.Index + 1];
            }
        }

        public T Previous
        {
            get
            {
                if (!this.IsPrevious)
                {
                    return default(T);
                }
                return base[this.Index - 1];
            }
        }

        public IndexedList()
        {
        }

        public IndexedList(IEnumerable<T> list) : base(list)
        {
        }

        public void MoveNext()
        {
            this.Index = this.Index + 1;
        }

        public void MovePrevious()
        {
            this.Index = this.Index - 1;
        }

        public event IndexedList<T>.IndexChangedHandler IndexChanged;

        public delegate void IndexChangedHandler(IndexedList<T> sender);
    }
}