using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using Typesetterer.Prototyper;
using Typesetterer.Prototyper.Data;

namespace Typesetterer.Data
{
    public class LinkedPage
    {
        [XmlIgnore]
        public object Lock = new object();

        private bool _showInfo;

        private System.Drawing.Size _frame;

        private readonly List<Page> Hooked = new List<Page>();

        private readonly List<TextEntry> PreviousEntries = new List<TextEntry>();

        private int count;

        [XmlIgnore]
        public Rectangle Bounds
        {
            get;
            private set;
        }

        [XmlIgnore]
        public Typesetterer.Prototyper.EntryLayer EntryLayer
        {
            get;
            private set;
        }

        [XmlIgnore]
        public System.Drawing.Size Frame
        {
            get
            {
                return this._frame;
            }
            set
            {
                this._frame = value;
                if (this.SourceSize != System.Drawing.Size.Empty)
                {
                    System.Drawing.Size size = this.SourceSize.FitIn(value);
                    this.Bounds = new Rectangle(size.CenterIn(value), size);
                }
            }
        }

        [XmlIgnore]
        public BindingList<LineToTypeset> Lines
        {
            get;
            set;
        }

        public BindingList<Page> Pages
        {
            get;
            set;
        }

        public string PsdPath
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool ShowInfo
        {
            get
            {
                return this._showInfo;
            }
            set
            {
                this._showInfo = value;
                if (this.EntryLayer != null)
                {
                    this.EntryLayer.ShowInfo = value;
                }
            }
        }

        [XmlIgnore]
        public Bitmap Source
        {
            get;
            private set;
        }

        [XmlIgnore]
        public bool SourceLoaded
        {
            get;
            private set;
        }

        [XmlIgnore]
        public System.Drawing.Size SourceSize
        {
            get;
            private set;
        }

        public LinkedPage() : this("")
        {
        }

        public LinkedPage(string psdPath) : this(psdPath, new List<Page>())
        {
        }

        public LinkedPage(string psdPath, List<Page> pages)
        {
            this.PsdPath = psdPath;
            this.Pages = new BindingList<Page>();
            this.Lines = new BindingList<LineToTypeset>();
            foreach (Page page1 in pages)
            {
                this.Pages.Add(page1);
            }
            this.Pages.ListChanged += new ListChangedEventHandler((object s, ListChangedEventArgs e) =>
            {
                int itemTotal = this.GetItemTotal();
                if (this.count == itemTotal)
                {
                    this.Lines.AddNew();
                    this.Lines.RemoveAt(this.Lines.Count - 1);
                    return;
                }
                foreach (Page hooked in this.Hooked)
                {
                    hooked.PropertyChanged -= new PropertyChangedEventHandler(this.HandleListChanged);
                }
                this.Hooked.Clear();
                this.Lines.Clear();
                foreach (Page page in this.Pages)
                {
                    foreach (LineToTypeset line in page.Lines)
                    {
                        this.Lines.Add(line);
                    }
                    page.PropertyChanged += new PropertyChangedEventHandler(this.HandleListChanged);
                    this.Hooked.Add(page);
                }
                this.count = itemTotal;
            });
        }

        public void CreateTextFromEntry(TextEntry entry)
        {
            System.Windows.Point photoshopLocation = entry.PhotoshopLocation;
            double x = photoshopLocation.X;
            System.Drawing.Size sourceSize = this.SourceSize;
            float width = (float)(x / (double)((float)sourceSize.Width) * 100);
            double y = photoshopLocation.Y;
            sourceSize = this.SourceSize;
            float height = (float)(y / (double)((float)sourceSize.Height) * 100);
            Selection selection = entry.Selection;
            string str = "";
            for (int i = 0; i < entry.Lines.Count; i++)
            {
                str = string.Concat(str, entry.Lines[i].Text);
                if (i != entry.Lines.Count - 1)
                {
                    str = string.Concat(str, "\\r");
                }
            }
            State.Photoshop.OpenDocument(this.PsdPath);
            State.Photoshop.SelectTopmostLayer();
            State.Photoshop.CreateTextEntry(entry.Style, str, !(entry.Selection is Line), width, height, selection.XX, selection.XY, selection.YX, selection.YY, new Action<string>(entry.SetPhotoshopID));
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                LinkedPage linkedPage = obj as LinkedPage;
                if (linkedPage != null && this.PsdPath.Equals(linkedPage.PsdPath))
                {
                    return (new HashSet<Page>(this.Pages)).SetEquals(linkedPage.Pages);
                }
            }
            return false;
        }

        public void Free()
        {
            lock (this)
            {
                if (this.SourceLoaded)
                {
                    this.Source.Dispose();
                    this.Source = null;
                    this.SourceSize = System.Drawing.Size.Empty;
                    this.SourceLoaded = false;
                    this.Bounds = Rectangle.Empty;
                    this.Frame = System.Drawing.Size.Empty;
                    this.PreviousEntries.AddRange(this.EntryLayer.Items);
                    Bitmap displayImage = this.EntryLayer.DisplayImage;
                    if (displayImage != null)
                    {
                        displayImage.Dispose();
                    }
                    else
                    {
                    }
                    this.EntryLayer.DisplayImage = null;
                    this.EntryLayer.SourceSize = System.Drawing.Size.Empty;
                    this.EntryLayer = null;
                }
            }
        }

        public override int GetHashCode()
        {
            return (new ValueTuple<string, BindingList<Page>>(this.PsdPath, this.Pages)).GetHashCode();
        }

        private int GetItemTotal()
        {
            int num = 0;
            foreach (Page page in this.Pages)
            {
                foreach (LineToTypeset line in page.Lines)
                {
                    num++;
                }
            }
            return num;
        }

        private void HandleListChanged(object sender, EventArgs e)
        {
            int itemTotal = this.GetItemTotal();
            if (itemTotal != this.count)
            {
                this.Lines.Clear();
                foreach (Page page in this.Pages)
                {
                    foreach (LineToTypeset line in page.Lines)
                    {
                        this.Lines.Add(line);
                    }
                }
                this.count = itemTotal;
            }
        }

        public void Load()
        {
            lock (this.Lock)
            {
                this.Source = PsdLoader.LoadFromFile(this.PsdPath);
                this.SourceSize = this.Source.Size;
                this.EntryLayer = new Typesetterer.Prototyper.EntryLayer()
                {
                    SourceSize = this.SourceSize,
                    ShowInfo = this.ShowInfo
                };
                if (this.PreviousEntries.Count > 0)
                {
                    this.EntryLayer.AddItem(this.PreviousEntries.ToArray());
                }
                this.EntryLayer.RecreateTextEvent += new EventHandler((object s, EventArgs e) => this.CreateTextFromEntry(this.EntryLayer.Items.Last.Value));
                this.Frame = this._frame;
                this.Update();
                this.SourceLoaded = true;
                EventHandler eventHandler = this.PsdLoaded;
                if (eventHandler != null)
                {
                    eventHandler(this, null);
                }
                else
                {
                }
            }
        }

        public void Update()
        {
            if (this.Bounds == Rectangle.Empty || this.Bounds.Width == 0 || this.Bounds.Height == 0)
            {
                return;
            }
            this.EntryLayer.ScaledSize = this.Bounds.Size;
        }

        public event EventHandler PsdLoaded;
    }
}