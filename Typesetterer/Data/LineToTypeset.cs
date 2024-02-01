using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Typesetterer.Data
{
    [Serializable]
    public class LineToTypeset : INotifyPropertyChanged
    {
        private string _alteredContents
        {
            get;
            set;
        }

        private string _contents
        {
            get;
            set;
        }

        private bool _hasBeenTyped
        {
            get;
            set;
        }

        public string AlteredContents
        {
            get
            {
                return this._alteredContents;
            }
            set
            {
                if (this.AlteredContents != value)
                {
                    this._alteredContents = value;
                    PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
                    if (propertyChangedEventHandler == null)
                    {
                        return;
                    }
                    propertyChangedEventHandler(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public string Contents
        {
            get
            {
                return this._contents;
            }
            set
            {
                if (this.Contents != value)
                {
                    this._contents = value;
                    PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
                    if (propertyChangedEventHandler == null)
                    {
                        return;
                    }
                    propertyChangedEventHandler(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }

        [XmlAttribute]
        public bool HasBeenTyped
        {
            get
            {
                return this._hasBeenTyped;
            }
            set
            {
                if (this.HasBeenTyped != value)
                {
                    this._hasBeenTyped = value;
                    string alteredContents = this.AlteredContents;
                    this.AlteredContents = string.Concat(alteredContents, " ");
                    this.AlteredContents = alteredContents;
                }
            }
        }

        public string Note
        {
            get;
            set;
        }

        [XmlIgnore]
        public string Text
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.AlteredContents))
                {
                    return this.AlteredContents;
                }
                return this.Contents;
            }
        }

        public LineToTypeset()
        {
            string empty = string.Empty;
            string str = empty;
            this.Note = empty;
            string str1 = str;
            string str2 = str1;
            this.AlteredContents = str1;
            this.Contents = str2;
            this.HasBeenTyped = false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is LineToTypeset))
            {
                return false;
            }
            LineToTypeset lineToTypeset = obj as LineToTypeset;
            if (!this.Contents.Equals(lineToTypeset.Contents))
            {
                return false;
            }
            return this.Note.Equals(lineToTypeset.Note);
        }

        public override int GetHashCode()
        {
            return new { Contents = this.Contents, Note = this.Note }.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}