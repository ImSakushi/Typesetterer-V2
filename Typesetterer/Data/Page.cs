using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Typesetterer.Data
{
    public class Page : INotifyPropertyChanged
    {
        private BindingList<LineToTypeset> _lines
        {
            get;
            set;
        }

        public bool IsDouble
        {
            get;
            set;
        }

        public BindingList<LineToTypeset> Lines
        {
            get
            {
                return this._lines;
            }
            set
            {
                if (this.Lines != value)
                {
                    if (this.Lines != null)
                    {
                        this.Lines.ListChanged -= new ListChangedEventHandler(this.HandleListChanged);
                        this.Lines.AddingNew -= new AddingNewEventHandler(this.HandleAddingNew);
                    }
                    this._lines = value;
                    if (this.Lines != null)
                    {
                        this.Lines.ListChanged += new ListChangedEventHandler(this.HandleListChanged);
                        this.Lines.AddingNew += new AddingNewEventHandler(this.HandleAddingNew);
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                if (!this.IsDouble)
                {
                    return string.Format("Page {0}", this.Number);
                }
                return string.Format("Pages {0}-{1}", this.Number, this.Number + 1);
            }
        }

        public int Number
        {
            get;
            set;
        }

        public string Parsed
        {
            get
            {
                string str = "";
                foreach (LineToTypeset line in this.Lines)
                {
                    str = string.Concat(str, line.Contents, Environment.NewLine);
                }
                return str;
            }
        }

        public string Text
        {
            get;
            set;
        }

        public Page() : this(0, false, "")
        {
        }

        public Page(int Number, bool IsDouble, string Text)
        {
            this.Number = Number;
            this.IsDouble = IsDouble;
            this.Text = Text;
            this.Lines = new BindingList<LineToTypeset>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Page))
            {
                return false;
            }
            Page page = obj as Page;
            if (!this.Text.Equals(page.Text) || this.Number != page.Number)
            {
                return false;
            }
            return (new HashSet<LineToTypeset>(this.Lines)).SetEquals(page.Lines);
        }

        public override int GetHashCode()
        {
            return new { Text = this.Text, IsDouble = this.IsDouble, Number = this.Number, Lines = this.Lines }.GetHashCode();
        }

        private void HandleAddingNew(object sender, EventArgs e = null)
        {
            PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
            if (propertyChangedEventHandler == null)
            {
                return;
            }
            propertyChangedEventHandler(this, new PropertyChangedEventArgs("Lines"));
        }

        private void HandleListChanged(object sender = null, EventArgs e = null)
        {
            PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
            if (propertyChangedEventHandler == null)
            {
                return;
            }
            propertyChangedEventHandler(this, new PropertyChangedEventArgs("Lines"));
        }

        public void Print()
        {
            Console.WriteLine("Page Number {0}:", this.Number);
            foreach (LineToTypeset line in this.Lines)
            {
                Console.WriteLine("\tContents:{0}", line.ToString());
                Console.WriteLine("\tNote:{0}", line.Note);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}