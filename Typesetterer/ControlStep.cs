using System;
using System.Drawing;
using System.Windows.Forms;

namespace Typesetterer
{
    public class ControlStep : Step
    {
        private System.Windows.Forms.Panel _panel
        {
            get;
            set;
        }

        public Color AccentColor
        {
            get;
            set;
        }

        public System.Drawing.Size ActualSize
        {
            get
            {
                if (this.Size == System.Drawing.Size.Empty)
                {
                    return this.MinimumSize;
                }
                return this.Size;
            }
        }

        private Control LastParent
        {
            get;
            set;
        }

        public System.Drawing.Size MinimumSize
        {
            get;
            set;
        }

        public System.Windows.Forms.Panel Panel
        {
            get
            {
                return this._panel;
            }
            set
            {
                if (this.Panel != value)
                {
                    if (this.Panel != null)
                    {
                        this.Panel.ParentChanged -= new EventHandler(this.HandleParentChanged);
                    }
                    this._panel = value;
                    this.Panel.ParentChanged += new EventHandler(this.HandleParentChanged);
                }
            }
        }

        public virtual System.Drawing.Size Size
        {
            get;
            set;
        }

        public ControlStep()
        {
            base.Destroyed = true;
        }

        private void HandleParentChanged(object sender, EventArgs e)
        {
            Control parent = ((System.Windows.Forms.Panel)sender).Parent;
            if (parent != this.LastParent)
            {
                this.HandleRemove();
            }
            if (parent != null)
            {
                this.HandleAdd();
            }
        }
    }
}