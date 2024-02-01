using FlatUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Typesetterer
{
    internal class LayoutStep : ControlStep
    {
        private List<string> _supportedDragDropTypes
        {
            get;
            set;
        }

        protected int DragDropItemLimit
        {
            get;
            set;
        }

        protected virtual bool IsDragOver
        {
            get;
            set;
        }

        protected List<string> SupportedDragDropTypes
        {
            get
            {
                return this._supportedDragDropTypes;
            }
            set
            {
                this._supportedDragDropTypes = value;
                base.Panel.AllowDrop = (value == null ? false : value.Count > 0);
            }
        }

        public LayoutStep()
        {
            this.DragDropItemLimit = -1;
            base.Panel = new TLayout()
            {
                Direction = Orientation.Vertical,
                Horizontal = Method.Equal,
                Vertical = Method.Equal,
                ForeColor = Theme.DarkForeground
            };
            base.Panel.DragOver += new DragEventHandler((object s, DragEventArgs e) =>
            {
                bool flag = false;
                string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (this.DragDropItemLimit == -1 || (int)data.Length <= this.DragDropItemLimit)
                {
                    string[] strArrays = data;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str = strArrays[i];
                        flag |= this.SupportedDragDropTypes.Contains(Path.GetExtension(str));
                    }
                    if (flag)
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.IsDragOver = true;
                    }
                }
            });
            base.Panel.DragDrop += new DragEventHandler((object s, DragEventArgs e) =>
            {
                if (this.IsDragOver)
                {
                    string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
                    List<string> strs = new List<string>();
                    string[] strArrays = data;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str = strArrays[i];
                        if (this.SupportedDragDropTypes.Contains(Path.GetExtension(str)))
                        {
                            strs.Add(str);
                        }
                    }
                    strs.Sort();
                    this.HandleDragDropFiles(strs);
                    this.IsDragOver = false;
                }
            });
            base.Panel.DragLeave += new EventHandler((object s, EventArgs e) => this.IsDragOver = false);
        }

        public override void DestroyControls()
        {
            ((TLayout)base.Panel).Reset();
            base.DestroyControls();
        }

        protected virtual void HandleDragDropFiles(List<string> files)
        {
        }
    }
}