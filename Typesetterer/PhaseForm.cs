using FlatUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Typesetterer
{
    internal class PhaseForm : FlatForm
    {
        private Enum _formPhase;

        private FlatUI.SizeTransition SizeTransition = new FlatUI.SizeTransition();

        private FlatUI.PointTransition PointTransition = new FlatUI.PointTransition();

        protected Dictionary<Enum, Phase> Phases = new Dictionary<Enum, Phase>();

        public Enum FormPhase
        {
            get
            {
                return this._formPhase;
            }
            set
            {
                if (this.FormPhase != value)
                {
                    this.Phases[this.FormPhase].PhaseChanged -= new EventHandler(this.HandlePhaseTransition);
                    this.Phases[this.FormPhase].HandleRemove();
                    this._formPhase = value;
                    base.MainControl = null;
                    this.SizeTransition.Set(base.Size);
                    FlatUI.PointTransition pointTransition = this.PointTransition;
                    Point location = base.Location;
                    int x = location.X + base.Width / 2;
                    location = base.Location;
                    pointTransition.Set(new Point(x, location.Y + base.Height / 2));
                    this.MinimumSize = System.Drawing.Size.Empty;
                    Phase item = this.Phases[value];
                    base.HideShadow();
                    item.PhaseChanged += new EventHandler(this.HandlePhaseTransition);
                    base.KeyboardShortcuts = item.KeyboardShortcuts;
                    base.FormMode = item.DisplayMode;
                    base.TitleBarColor = item.TitleBarColor;
                    this.BackColor = item.BackColor;
                    base.ShowMinimize = item.ShowMinimize;
                    base.ShowRestore = item.ShowRestore;
                    base.ShowTitle = item.ShowTitle;
                    base.ContextButton = item.ContextButton;
                    this.SizeTransition.To = item.ActualSize;
                    if (item.Location != Point.Empty)
                    {
                        FlatUI.PointTransition point = this.PointTransition;
                        int num = item.Location.X;
                        System.Drawing.Size actualSize = item.ActualSize;
                        int width = num + actualSize.Width / 2;
                        int y = item.Location.Y;
                        actualSize = item.ActualSize;
                        point.To = new Point(width, y + actualSize.Height / 2);
                    }
                    item.HandleAdd();
                    this.UpdateText();
                }
            }
        }

        protected string FormTitle
        {
            get;
            set;
        }

        protected Action ToPerformAfterTransition
        {
            get;
            set;
        }

        public PhaseForm()
        {
            this.SizeTransition.Transitioning += new EventHandler((object s, EventArgs e) =>
            {
                base.Size = this.SizeTransition.Value;
                Rectangle workingArea = Screen.GetWorkingArea(this);
                Point value = this.PointTransition.Value;
                int num = Math.Max(15, Math.Min(workingArea.Width - base.Width - 15, value.X - base.Width / 2));
                value = this.PointTransition.Value;
                base.Location = new Point(num, Math.Max(15, Math.Min(workingArea.Height - base.Height - 15, value.Y - base.Height / 2)));
            });
            this.SizeTransition.Transitioned += new EventHandler((object s, EventArgs e) =>
            {
                Phase item = this.Phases[this.FormPhase];
                base.MainControl = item.Panel;
                this.MinimumSize = item.MinimumSize;
                base.Sizable = item.Sizable;
                base.TopMost = item.TopMost;
                base.SetSize(base.Width, base.Height);
                base.HandleResizeEnd(null, null);
                base.ShowShadow();
                if (!item.ChangeTheme)
                {
                    base.ShadowColor = Color.FromArgb(0, item.AccentColor);
                }
                else
                {
                    Theme.Accent.To = item.AccentColor;
                }
                Action toPerformAfterTransition = this.ToPerformAfterTransition;
                if (toPerformAfterTransition != null)
                {
                    toPerformAfterTransition();
                }
                else
                {
                }
                this.ToPerformAfterTransition = null;
                item.HandleLoad();
            });
        }

        private void HandlePhaseTransition(object sender = null, EventArgs e = null)
        {
            this.FormPhase = ((PhaseChangedEventArgs)e).NewPhase;
        }

        protected virtual void SetPhase(Enum phaseEnum)
        {
            this._formPhase = phaseEnum;
            Phase item = this.Phases[phaseEnum];
            if (!item.ChangeTheme)
            {
                base.ShadowColor = item.AccentColor;
            }
            else
            {
                Theme.Accent.Set(Color.FromArgb(0, item.AccentColor));
                Theme.Accent.To = item.AccentColor;
            }
            base.SetSize(item.MinimumSize.Width, item.MinimumSize.Height);
            this.MinimumSize = item.MinimumSize;
            base.TopMost = item.TopMost;
            base.MainControl = item.Panel;
            item.PhaseChanged += new EventHandler(this.HandlePhaseTransition);
            this.UpdateText();
        }

        private void UpdateText()
        {
            string formTitle = this.FormTitle;
            string name = "";
            if (this.FormPhase != null && this.Phases[this.FormPhase].Name != "")
            {
                name = this.Phases[this.FormPhase].Name;
            }
            if (string.IsNullOrWhiteSpace(formTitle))
            {
                this.Text = name;
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                this.Text = formTitle;
                return;
            }
            this.Text = string.Format("{0} - {1}", formTitle, name);
        }
    }
}