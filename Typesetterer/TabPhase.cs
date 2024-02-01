using FlatUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Typesetterer
{
    public class TabPhase : Phase
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
                        if (selectedIndex > -1)
                        {
                            base.Panel.Controls.Remove(((ControlStep)this.Steps[selectedIndex]).Panel);
                        }
                        item.Panel.Dock = DockStyle.Fill;
                        base.Panel.Controls.Add(item.Panel);
                        item.HandleLoad();
                    }
                    else if (this.Steps[this.SelectedIndex] is CompositeStep)
                    {
                        if (selectedIndex > -1)
                        {
                            (this.Steps[selectedIndex] as CompositeStep).HandleRemove();
                        }
                        (this.Steps[this.SelectedIndex] as CompositeStep).HandleAdd();
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

        public Font TabFont
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

        private int TitleBarHeight
        {
            get;
            set;
        }

        public TabPhase(FlatForm parent) : base(parent)
        {
            this.Steps = new List<Step>();
            this.TitleBarHeight = 29;
            this.TabGroup = new ButtonGroup<CompositeToggleButton>()
            {
                StartChecked = true,
                EnforceCheck = true
            };
            this.TabGroup.CheckedButtonChanged += new EventHandler((object s, EventArgs e) => this.SelectedIndex = this.TabGroup.IndexOf(s as CompositeToggleButton));
            this.TabFont = Theme.RegularSmallerItemFont;
            this.TabHorizontalPadding = 10;
            base.Panel = new System.Windows.Forms.Panel();
        }

        public void AddStep(Step step)
        {
            this.Steps.Add(step);
            this.CreateTabButton(step);
        }

        protected virtual CompositeToggleButton CreateTabButton(Step step)
        {
            int right;
            System.Drawing.Size size = TextRenderer.MeasureText(Drawing.Measurer, step.Name, this.TabFont);
            CompositeToggleButton compositeToggleButton = new CompositeToggleButton(base.Parent)
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
            compositeToggleButton.CheckedBackColor = base.BackColor;
            return compositeToggleButton;
        }

        public override void HandleAdd()
        {
            foreach (CompositeToggleButton tabGroup in this.TabGroup)
            {
                base.Parent.CompositeButtonGroup.Add(tabGroup);
            }
            base.HandleAdd();
        }

        public override void HandleLoad()
        {
            base.HandleLoad();
            if (this.SelectedIndex == -1 && this.Steps.Count > 0)
            {
                this.SelectedIndex = 0;
            }
        }

        public override void HandleRemove()
        {
            foreach (CompositeToggleButton tabGroup in this.TabGroup)
            {
                base.Parent.CompositeButtonGroup.Remove(tabGroup);
            }
            base.HandleRemove();
        }

        public event EventHandler TabSwitched;
    }
}