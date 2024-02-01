using FlatUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Typesetterer
{
    public class Phase : ControlStep
    {
        public Color BackColor
        {
            get;
            protected set;
        }

        public bool ChangeTheme
        {
            get;
            protected set;
        }

        public CompositeButton ContextButton
        {
            get;
            protected set;
        }

        public FlatForm.FormDisplayMode DisplayMode
        {
            get;
            protected set;
        }

        public Dictionary<ShortcutKeys, Func<bool>> KeyboardShortcuts
        {
            get;
            protected set;
        }

        public virtual Point Location
        {
            get;
            protected set;
        }

        public FlatForm Parent
        {
            get;
            protected set;
        }

        public bool ShowMinimize
        {
            get;
            protected set;
        }

        public bool ShowRestore
        {
            get;
            protected set;
        }

        public bool ShowTitle
        {
            get;
            protected set;
        }

        public bool Sizable
        {
            get;
            protected set;
        }

        public Color TitleBarColor
        {
            get;
            protected set;
        }

        public bool TopMost
        {
            get;
            protected set;
        }

        public Phase(FlatForm parent)
        {
            this.Parent = parent;
            this.DisplayMode = FlatForm.FormDisplayMode.Normal;
            this.BackColor = Theme.DarkBackground;
            this.TitleBarColor = Theme.DarkBackground;
            this.KeyboardShortcuts = new Dictionary<ShortcutKeys, Func<bool>>();
            this.ChangeTheme = true;
            this.ShowMinimize = true;
            this.ShowRestore = true;
            this.ShowTitle = true;
        }

        public virtual void HandleApplicationClosing()
        {
        }

        protected virtual void OnPhaseChanged(PhaseChangedEventArgs e)
        {
            EventHandler eventHandler = this.PhaseChanged;
            if (eventHandler == null)
            {
                return;
            }
            eventHandler(this, e);
        }

        public event EventHandler PhaseChanged;
    }
}