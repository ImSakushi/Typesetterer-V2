using FlatUI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Typesetterer
{
    internal class CompositeStep : Step
    {
        public List<CompositeButton> CompositeButtons
        {
            get;
            set;
        }

        public List<Control> Controls
        {
            get;
            set;
        }

        public Dictionary<ShortcutKeys, Func<bool>> KeyboardShortcuts
        {
            get;
            protected set;
        }

        public virtual FlatForm Parent
        {
            get;
            set;
        }

        public CompositeStep()
        {
            this.Controls = new List<Control>();
            this.CompositeButtons = new List<CompositeButton>();
            this.KeyboardShortcuts = new Dictionary<ShortcutKeys, Func<bool>>();
        }

        public override void HandleAdd()
        {
            base.HandleAdd();
            if (this.Parent != null)
            {
                Control[] array = this.Controls.ToArray();
                for (int i = 0; i < (int)array.Length; i++)
                {
                    Control control = array[i];
                    this.Parent.Controls.Add(control);
                }
                foreach (CompositeButton compositeButton in this.CompositeButtons)
                {
                    this.Parent.CompositeButtonGroup.Add(compositeButton);
                    this.Parent.Controls.Add(compositeButton.FocusControl);
                }
                foreach (KeyValuePair<ShortcutKeys, Func<bool>> keyboardShortcut in this.KeyboardShortcuts)
                {
                    this.Parent.KeyboardShortcuts.Add(keyboardShortcut.Key, keyboardShortcut.Value);
                }
            }
        }

        public virtual bool HandleMouseDown(object sender, MouseEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseMove(object sender, MouseEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseUp(object sender, MouseEventArgs e)
        {
            return false;
        }

        public virtual bool HandleMouseWheel(object sender, MouseEventArgs e)
        {
            return false;
        }

        public virtual void HandlePaint(PaintEventArgs e)
        {
        }

        public override void HandleRemove()
        {
            if (this.Parent != null)
            {
                Control[] array = this.Controls.ToArray();
                for (int i = 0; i < (int)array.Length; i++)
                {
                    Control control = array[i];
                    this.Parent.Controls.Remove(control);
                }
                foreach (CompositeButton compositeButton in this.CompositeButtons)
                {
                    this.Parent.CompositeButtonGroup.Remove(compositeButton);
                    this.Parent.Controls.Remove(compositeButton.FocusControl);
                }
                foreach (KeyValuePair<ShortcutKeys, Func<bool>> keyboardShortcut in this.KeyboardShortcuts)
                {
                    this.Parent.KeyboardShortcuts.Remove(keyboardShortcut.Key);
                }
            }
        }

        public virtual void OnSizeChanged(object sender, EventArgs e)
        {
        }
    }
}