using System.Collections.Generic;
using System.ComponentModel;

namespace Typesetterer.Data
{
    public class BindingListInvoked<T> : BindingList<T>
    {
        private readonly ISynchronizeInvoke _invoke;

        public IList<T> DataSource
        {
            get
            {
                return this;
            }
            set
            {
                if (value != null)
                {
                    this.ClearItems();
                    base.RaiseListChangedEvents = false;
                    foreach (T t in value)
                    {
                        base.Add(t);
                    }
                    base.RaiseListChangedEvents = true;
                    this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
            }
        }

        public BindingListInvoked()
        {
        }

        public BindingListInvoked(ISynchronizeInvoke invoke)
        {
            this._invoke = invoke;
        }

        public BindingListInvoked(IList<T> items)
        {
            this.DataSource = items;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (this._invoke == null || !this._invoke.InvokeRequired)
            {
                base.OnListChanged(e);
                return;
            }
            this._invoke.BeginInvoke(new BindingListInvoked<T>.ListChangedDelegate(this.OnListChanged), new object[] { e });
        }

        private delegate void ListChangedDelegate(ListChangedEventArgs e);
    }
}