using System.ComponentModel;

namespace Typesetterer.Data
{
    public class BoolSource : INotifyPropertyChanged
    {
        private bool _value
        {
            get;
            set;
        }

        public bool Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
                if (propertyChangedEventHandler == null)
                {
                    return;
                }
                propertyChangedEventHandler(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public BoolSource()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}