using FlatUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Typesetterer
{
    [DesignerCategory("")]
    internal class StepTabDisplay : Panel
    {
        private int _tabHeight = 35;

        private int _dividerHeight = 6;

        private System.Drawing.Font _tabFont = new System.Drawing.Font("Segoe UI", 16f, FontStyle.Regular, GraphicsUnit.Pixel);

        private Color _inactiveForeground = Color.FromArgb(63, 63, 70);

        private Color _activeForeground = Theme.DarkForeground;

        private Color _dividerBackground = Color.FromArgb(63, 63, 70);

        private int _selectedIndex = -1;

        public Color ActiveForeground
        {
            get
            {
                return this._activeForeground;
            }
            set
            {
                if (this.ActiveForeground != value)
                {
                    this._activeForeground = value;
                    base.Invalidate();
                }
            }
        }

        public System.Windows.Forms.Padding ChildPadding
        {
            get;
            set;
        }

        private int ClickIndex
        {
            get;
            set;
        }

        public Color DividerBackground
        {
            get
            {
                return this._dividerBackground;
            }
            set
            {
                if (this.DividerBackground != value)
                {
                    this._dividerBackground = value;
                    base.Invalidate();
                }
            }
        }

        public int DividerHeight
        {
            get
            {
                return this._dividerHeight;
            }
            set
            {
                if (this.DividerHeight != value)
                {
                    this._dividerHeight = value;
                    if (this.SelectedIndex > -1)
                    {
                        this.PositionControl();
                    }
                    base.Invalidate();
                }
            }
        }

        private int HoverIndex
        {
            get;
            set;
        }

        public Color InactiveForeground
        {
            get
            {
                return this._inactiveForeground;
            }
            set
            {
                if (this.InactiveForeground != value)
                {
                    this._inactiveForeground = value;
                    base.Invalidate();
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                int selectedIndex = this.SelectedIndex;
                this._selectedIndex = Math.Max(0, Math.Min(value, this.Steps.Count - 1));
                if (this.SelectedIndex != selectedIndex && this.SelectedIndex < this.Steps.Count)
                {
                    if (selectedIndex > -1 && selectedIndex < this.Steps.Count)
                    {
                        base.Controls.Remove(this.Steps[selectedIndex].Panel);
                    }
                    Panel panel = this.Steps[this.SelectedIndex].Panel;
                    Control nextControl = panel.GetNextControl(panel, true);
                    base.Controls.Add(panel);
                    this.PositionControl();
                    if (nextControl != null)
                    {
                        if (nextControl.Parent != null)
                        {
                            List<Control> controls = new List<Control>();
                            while ((nextControl is Label || nextControl is Panel) && !controls.Contains(nextControl))
                            {
                                controls.Add(nextControl);
                                nextControl = panel.GetNextControl(nextControl, true);
                            }
                        }
                        nextControl.Select();
                        nextControl.Focus();
                    }
                    if (selectedIndex != -1)
                    {
                        this.Tabs[selectedIndex].BarColor.To = this.DividerBackground;
                        Theme.Accent.To = this.Steps[this.SelectedIndex].AccentColor;
                    }
                    this.Tabs[this.SelectedIndex].BarColor.To = this.Steps[this.SelectedIndex].AccentColor;
                    base.Invalidate();
                }
            }
        }

        public int StepLimit
        {
            get;
            set;
        }

        private List<ControlStep> Steps
        {
            get;
            set;
        }

        public System.Drawing.Font TabFont
        {
            get
            {
                return this._tabFont;
            }
            set
            {
                if (this.TabFont != value)
                {
                    this._tabFont = value;
                    base.Invalidate();
                }
            }
        }

        public int TabHeight
        {
            get
            {
                return this._tabHeight;
            }
            set
            {
                if (this.TabHeight != value)
                {
                    this._tabHeight = value;
                    base.Invalidate();
                }
            }
        }

        private List<StepTabDisplay.Tab> Tabs
        {
            get;
            set;
        }

        public StepTabDisplay()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.Steps = new List<ControlStep>();
            this.Tabs = new List<StepTabDisplay.Tab>();
            this.StepLimit = 5;
            this.HoverIndex = -1;
            this.ClickIndex = -1;
            this.ChildPadding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            base.MouseLeave += new EventHandler((object s, EventArgs e) =>
            {
                int hoverIndex = this.HoverIndex;
                this.HoverIndex = -1;
                if (hoverIndex > -1)
                {
                    base.Invalidate(this.Tabs[hoverIndex].Bounds);
                }
            });
            base.ParentChanged += new EventHandler((object s, EventArgs e) =>
            {
                if (base.Parent != null && this.SelectedIndex > -1)
                {
                    this.Tabs[this.SelectedIndex].BarColor.Set(this.DividerBackground);
                    this.Tabs[this.SelectedIndex].BarColor.To = this.Steps[this.SelectedIndex].AccentColor;
                }
            });
        }

        public void AddStep(ControlStep step)
        {
            int right;
            System.Drawing.Size size = TextRenderer.MeasureText(Drawing.Measurer, step.Name, this.TabFont);
            if (this.Tabs.Count > 0)
            {
                Rectangle bounds = this.Tabs[this.Tabs.Count - 1].Bounds;
                right = bounds.Right;
            }
            else
            {
                right = 0;
            }
            Rectangle rectangle = new Rectangle(right, 0, size.Width, size.Height);
            System.Drawing.Size minimumSize = this.MinimumSize;
            int width = minimumSize.Width + size.Width;
            minimumSize = this.MinimumSize;
            this.MinimumSize = new System.Drawing.Size(width, minimumSize.Height);
            StepTabDisplay.Tab tab = new StepTabDisplay.Tab()
            {
                Bounds = rectangle,
                MinimumSize = size,
                BarColor = new ColorTransition()
                {
                    Frames = Transitions.LongerLinear
                }
            };
            tab.BarColor.Set(this.DividerBackground);
            tab.BarColor.Transitioning += new EventHandler((object s, EventArgs e) => this.Invalidate(new Rectangle(tab.Bounds.Left, this.TabHeight, tab.Bounds.Width, this.DividerHeight)));
            this.Steps.Add(step);
            this.Tabs.Add(tab);
            if (this.Steps.Count == 1)
            {
                this.SelectedIndex = 0;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && this.HoverIndex > -1 && this.Tabs[this.HoverIndex].Bounds.Contains(e.Location))
            {
                this.ClickIndex = this.HoverIndex;
                base.Invalidate(this.Tabs[this.ClickIndex].Bounds);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                if (i != this.SelectedIndex && i <= this.StepLimit && this.Tabs[i].Bounds.Contains(e.Location))
                {
                    if (this.HoverIndex != i)
                    {
                        int hoverIndex = this.HoverIndex;
                        this.HoverIndex = i;
                        base.Invalidate(this.Tabs[this.HoverIndex].Bounds);
                        if (hoverIndex > -1)
                        {
                            base.Invalidate(this.Tabs[hoverIndex].Bounds);
                        }
                    }
                    return;
                }
            }
            int num = this.HoverIndex;
            this.HoverIndex = -1;
            if (num > -1)
            {
                base.Invalidate(this.Tabs[num].Bounds);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.ClickIndex > -1 && this.Tabs[this.ClickIndex].Bounds.Contains(e.Location) && this.SelectedIndex != this.ClickIndex)
            {
                int selectedIndex = this.SelectedIndex;
                this.SelectedIndex = this.ClickIndex;
                base.Invalidate(this.Tabs[this.ClickIndex].Bounds);
                if (selectedIndex > -1)
                {
                    base.Invalidate(this.Tabs[selectedIndex].Bounds);
                }
                this.ClickIndex = -1;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            for (int i = 0; i < this.Tabs.Count; i++)
            {
                Rectangle bounds = this.Tabs[i].Bounds;
                bounds.Offset(-3, 0);
                TextRenderer.DrawText(e.Graphics, this.Steps[i].Name, this.TabFont, bounds, (i == this.SelectedIndex || i == this.HoverIndex ? this.ActiveForeground : this.InactiveForeground), TextFormatFlags.VerticalCenter);
                using (SolidBrush solidBrush = new SolidBrush(this.Tabs[i].BarColor.Value))
                {
                    Graphics graphics = e.Graphics;
                    Rectangle rectangle = this.Tabs[i].Bounds;
                    int left = rectangle.Left;
                    int tabHeight = this.TabHeight;
                    rectangle = this.Tabs[i].Bounds;
                    graphics.FillRectangle(solidBrush, left, tabHeight, rectangle.Width, this.DividerHeight);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            int width = base.Width;
            System.Drawing.Size minimumSize = this.MinimumSize;
            int num = width - minimumSize.Width;
            System.Windows.Forms.Padding padding = base.Padding;
            int num1 = (int)Math.Ceiling((double)((float)(num - padding.Horizontal) / (float)this.Steps.Count));
            int left = base.Padding.Left;
            foreach (StepTabDisplay.Tab tab in this.Tabs)
            {
                minimumSize = tab.MinimumSize;
                tab.Bounds = new Rectangle(left, 0, minimumSize.Width + num1, this.TabHeight);
                left += tab.Bounds.Width;
            }
            base.Invalidate();
            this.PositionControl();
        }

        private void PositionControl()
        {
            Control item = base.Controls[0];
            int left = base.Padding.Left;
            System.Windows.Forms.Padding childPadding = this.ChildPadding;
            int num = left + childPadding.Left;
            childPadding = base.Padding;
            int top = childPadding.Top + this.TabHeight + this.DividerHeight;
            childPadding = this.ChildPadding;
            int top1 = top + childPadding.Top;
            int width = base.Width;
            childPadding = base.Padding;
            int horizontal = width - childPadding.Horizontal;
            childPadding = this.ChildPadding;
            int horizontal1 = horizontal - childPadding.Horizontal;
            int height = base.Height;
            childPadding = base.Padding;
            int vertical = height - childPadding.Vertical - this.TabHeight - this.DividerHeight;
            childPadding = this.ChildPadding;
            item.SetBounds(num, top1, horizontal1, vertical - childPadding.Vertical);
        }

        private class Tab
        {
            public ColorTransition BarColor
            {
                get;
                set;
            }

            public Rectangle Bounds
            {
                get;
                set;
            }

            public System.Drawing.Size MinimumSize
            {
                get;
                set;
            }

            public Tab()
            {
            }
        }
    }
}