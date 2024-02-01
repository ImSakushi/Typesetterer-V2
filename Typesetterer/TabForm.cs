using FlatUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Typesetterer
{
    internal class TabForm : FlatForm
    {
        private int _selectedIndex = -1;

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
                    if (this.Steps[this.SelectedIndex] is ControlStep)
                    {
                        ControlStep item = this.Steps[this.SelectedIndex] as ControlStep;
                        base.MainControl = item.Panel;
                        Control nextControl = base.MainControl.GetNextControl(base.MainControl, true);
                        if (nextControl != null)
                        {
                            System.Windows.Forms.Padding padding = nextControl.Padding;
                            List<Control> controls = new List<Control>();
                            while ((nextControl is Label || nextControl is Panel) && !controls.Contains(nextControl))
                            {
                                nextControl = base.MainControl.GetNextControl(nextControl, true);
                            }
                            if (nextControl == null)
                            {
                                nextControl = base.MainControl.GetNextControl(base.MainControl, true);
                            }
                            nextControl.Select();
                            nextControl.Focus();
                        }
                    }
                    else if (this.Steps[this.SelectedIndex] is CompositeStep)
                    {
                        if (selectedIndex > -1)
                        {
                            (this.Steps[selectedIndex] as CompositeStep).HandleRemove();
                        }
                        (this.Steps[this.SelectedIndex] as CompositeStep).HandleAdd();
                        base.Invalidate();
                    }
                }
                EventHandler eventHandler = this.TabSwitched;
                if (eventHandler == null)
                {
                    return;
                }
                eventHandler(this, null);
            }
        }

        protected List<Step> Steps
        {
            get;
            set;
        }

        public System.Drawing.Font TabFont
        {
            get;
            set;
        }

        protected ButtonGroup<CompositeToggleButton> TabGroup
        {
            get;
            set;
        }

        public int TabHorizontalPadding
        {
            get;
            set;
        }

        public TabForm()
        {
            this.Steps = new List<Step>();
            this.TabGroup = new ButtonGroup<CompositeToggleButton>()
            {
                StartChecked = true,
                EnforceCheck = true
            };
            this.TabGroup.CheckedButtonChanged += new EventHandler((object s, EventArgs e) => this.SelectedIndex = this.TabGroup.IndexOf(s as CompositeToggleButton));
            this.TabFont = Theme.RegularSmallerItemFont;
            this.TabHorizontalPadding = 10;
        }

        public void AddStep(Step step)
        {
            this.Steps.Add(step);
            this.CompositeButtonGroup.Add(this.CreateTabButton(step));
            if (this.Steps.Count == 1)
            {
                this.SelectedIndex = 0;
            }
        }

        protected virtual CompositeToggleButton CreateTabButton(Step step)
        {
            int right;
            System.Drawing.Size size = TextRenderer.MeasureText(Drawing.Measurer, step.Name, this.TabFont);
            CompositeToggleButton compositeToggleButton = new CompositeToggleButton(this)
            {
                Text = step.Name
            };
            if (this.TabGroup.Count > 0)
            {
                Rectangle bounds = this.TabGroup[this.TabGroup.Count - 1].Bounds;
                right = bounds.Right;
            }
            else
            {
                right = 0;
            }
            compositeToggleButton.Bounds = new Rectangle(right, 0, size.Width + this.TabHorizontalPadding * 2, this.TitleBarHeight);
            compositeToggleButton.Font = this.TabFont;
            compositeToggleButton.CheckedFont = this.TabFont;
            compositeToggleButton.Group = this.TabGroup;
            compositeToggleButton.ForeColor = Theme.DarkForeground;
            compositeToggleButton.BackColor = base.TitleBarColor;
            compositeToggleButton.CheckedForeColor = Theme.DarkForeground;
            compositeToggleButton.CheckedBackColor = this.BackColor;
            return compositeToggleButton;
        }

        public event EventHandler TabSwitched;
    }
}