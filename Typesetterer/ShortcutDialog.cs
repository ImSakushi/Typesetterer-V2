using FlatUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Typesetterer.Data;

namespace Typesetterer
{
    internal class ShortcutDialog : FlatForm
    {
        private FlatScrollableDisplay display
        {
            get;
            set;
        }

        private HiddenScrollableControl panel
        {
            get;
            set;
        }

        private List<ShortcutKeys> shortcuts
        {
            get;
            set;
        }

        private FlatDropDownList ShortcutSelect
        {
            get;
            set;
        }

        public int StartOn
        {
            get;
            set;
        }

        public ShortcutDialog()
        {
            this.Text = "View / Edit Keyboard Shortcuts";
            base.DialogButtons = FlatForm.FormDialogButtons.OK;
            base.DialogAlignment = HorizontalAlignment.Center;
            base.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new System.Drawing.Size(564, 400);
            this.StartOn = 0;
            this.ShortcutSelect = new FlatDropDownList()
            {
                Width = 135,
                Height = 25,
                Left = 8,
                AutoSize = false,
                ForeColor = Theme.DarkForeground,
                Top = 40,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            this.panel = new HiddenScrollableControl()
            {
                AutoScroll = true,
                ForeColor = Theme.DarkForeground,
                BackColor = Theme.DarkerBackground
            };
            this.shortcuts = new List<ShortcutKeys>();
            this.display = new FlatScrollableDisplay()
            {
                Control = this.panel,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Top = this.ShortcutSelect.Bottom + 8,
                Left = 8,
                Width = base.Width - 16,
                Height = 284
            };
            this.display.SizeChanged += new EventHandler((object s, EventArgs e) => this.HandleSizeChanged());
            this.ShortcutSelect.SelectedIndexChanged += new EventHandler((object s, EventArgs e) =>
            {
                this.shortcuts.Clear();
                object selectedItem = this.ShortcutSelect.SelectedItem;
                PropertyInfo[] properties = selectedItem.GetType().GetProperties();
                for (int i = 0; i < (int)properties.Length; i++)
                {
                    PropertyInfo propertyInfo = properties[i];
                    if (propertyInfo.PropertyType == typeof(ShortcutKeys))
                    {
                        this.shortcuts.Add(propertyInfo.GetValue(selectedItem, null) as ShortcutKeys);
                    }
                }
                int count = this.panel.Controls.Count - this.shortcuts.Count;
                if (count < 0)
                {
                    for (int j = 0; j < Math.Abs(count); j++)
                    {
                        this.panel.Controls.Add(new ShortcutDialog.ShortcutEditor());
                    }
                }
                else if (count > 0)
                {
                    for (int k = 0; k < count; k++)
                    {
                        this.panel.Controls.RemoveAt(this.panel.Controls.Count - 1);
                    }
                }
                this.HandleSizeChanged();
            });
            Typesetterer.Data.KeyboardShortcuts shortcuts = State.Shortcuts;
            this.ShortcutSelect.Items.Add(shortcuts);
            PropertyInfo[] propertyInfoArray = shortcuts.GetType().GetProperties();
            for (int l = 0; l < (int)propertyInfoArray.Length; l++)
            {
                PropertyInfo propertyInfo1 = propertyInfoArray[l];
                Type propertyType = propertyInfo1.PropertyType;
                if (propertyType != typeof(ShortcutKeys) && propertyType != typeof(Keys))
                {
                    this.ShortcutSelect.Items.Add(propertyInfo1.GetValue(shortcuts, null));
                }
            }
            this.ShortcutSelect.SelectedIndex = 0;
            this.ShortcutSelect.Enabled = true;
            this.ShortcutSelect.CalculateDropdownSize();
            Label label = new Label()
            {
                AutoSize = true,
                Text = "Shortcuts for the",
                ForeColor = Theme.DarkForeground,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Padding = new System.Windows.Forms.Padding(0),
                Margin = new System.Windows.Forms.Padding(0),
                Location = new Point(8, this.ShortcutSelect.Top + 6)
            };
            this.ShortcutSelect.Location = new Point(label.Right - 12, this.ShortcutSelect.Top);
            base.Controls.Add(this.display);
            base.Controls.Add(label);
            base.Controls.Add(this.ShortcutSelect);
            base.Shown += new EventHandler((object s, EventArgs e) => this.ShortcutSelect.SelectedIndex = this.StartOn);
            base.KeyboardShortcuts.Add(State.Shortcuts.CancelDialog, new Func<bool>(() =>
            {
                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return true;
            }));
        }

        private void HandleSizeChanged()
        {
            int num = 8;
            int num1 = 8;
            int width = (this.panel.Width - num1 - 4) / 2;
            int num2 = 58;
            this.panel.AutoScrollPosition = new Point(0, 0);
            for (int i = 0; i < this.shortcuts.Count; i++)
            {
                ShortcutDialog.ShortcutEditor item = this.panel.Controls[i] as ShortcutDialog.ShortcutEditor;
                item.SetBounds((i % 2 == 0 ? num1 : width + 5), num, width, num2);
                if (i % 4 == 1 || i % 4 == 2)
                {
                    item.BackColor = Theme.DarkBackground;
                }
                item.Description.Text = this.shortcuts[i].Description;
                item.Box.Text = this.shortcuts[i].Value.GetDisplayString();
                item.Set.Enabled = false;
                item.Value = this.shortcuts[i];
                if (i % 2 == 1)
                {
                    num = num + num2 + 5;
                }
            }
            if (this.shortcuts.Count % 2 == 1)
            {
                num += num2;
            }
            this.panel.AutoScrollMinSize = new System.Drawing.Size(0, num + 8);
            this.display.Sync(null, null);
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool HideCaret(IntPtr hWnd);

        private class ShortcutEditor : Panel
        {
            private ShortcutKeys _value
            {
                get;
                set;
            }

            public FlatBox Box
            {
                get;
                set;
            }

            public FlatButton Default
            {
                get;
                set;
            }

            public Label Description
            {
                get;
                set;
            }

            public FlatButton Set
            {
                get;
                set;
            }

            public Keys TemporaryValue
            {
                get;
                set;
            }

            public ShortcutKeys Value
            {
                get
                {
                    return this._value;
                }
                set
                {
                    this._value = value;
                    this.Default.Enabled = this.Value.Value != this.Value.Default;
                }
            }

            public ShortcutEditor()
            {
                this.Description = new Label()
                {
                    Height = 18,
                    Width = 220,
                    AutoEllipsis = true,
                    Location = new Point(8, 8)
                };
                this.Box = new FlatBox()
                {
                    Width = 130,
                    Height = 22,
                    Location = new Point(11, 27)
                };
                this.Box.Edit.ReadOnly = true;
                this.Box.Edit.PreviewKeyDown += new PreviewKeyDownEventHandler((object s2, PreviewKeyDownEventArgs e2) => e2.IsInputKey = true);
                this.Box.Edit.KeyUp += new KeyEventHandler((object s2, KeyEventArgs e2) =>
                {
                    Keys keyCode = e2.KeyCode;
                    if (keyCode <= Keys.Shift)
                    {
                        if ((int)keyCode - (int)Keys.ShiftKey <= (int)Keys.RButton || keyCode == Keys.Shift)
                        {
                            e2.Handled = true;
                            return;
                        }
                        this.TemporaryValue = e2.Modifiers | e2.KeyCode;
                        this.Box.Edit.Text = this.TemporaryValue.GetDisplayString();
                        this.Set.Enabled = this.TemporaryValue != this.Value.Value;
                        e2.Handled = true;
                        return;
                    }
                    else if (keyCode != Keys.Control && keyCode != Keys.Alt)
                    {
                        this.TemporaryValue = e2.Modifiers | e2.KeyCode;
                        this.Box.Edit.Text = this.TemporaryValue.GetDisplayString();
                        this.Set.Enabled = this.TemporaryValue != this.Value.Value;
                        e2.Handled = true;
                        return;
                    }
                    e2.Handled = true;
                });
                this.Box.Edit.GotFocus += new EventHandler((object s2, EventArgs e2) =>
                {
                    ShortcutDialog.HideCaret(this.Box.Edit.Handle);
                    this.Box.BackColor = Theme.Accent.Value;
                    this.Box.Invalidate();
                });
                this.Box.Edit.LostFocus += new EventHandler((object s, EventArgs e) =>
                {
                    this.Box.BackColor = Theme.DarkestBackground;
                    this.Box.Invalidate();
                });
                this.Set = new FlatButton()
                {
                    Text = "Set",
                    Width = 50,
                    Height = 22,
                    Location = new Point(this.Box.Right + 3, this.Box.Top)
                };
                this.Set.RoughClick += new EventHandler((object s, EventArgs e) =>
                {
                    this.Value.Value = this.TemporaryValue;
                    this.Set.Enabled = false;
                    this.Default.Enabled = this.Value.Value != this.Value.Default;
                });
                this.Default = new FlatButton()
                {
                    Text = "Default",
                    Width = 50,
                    Height = 22,
                    Location = new Point(this.Set.Right + 3, this.Set.Top),
                    Enabled = false
                };
                this.Default.RoughClick += new EventHandler((object s, EventArgs e) =>
                {
                    this.Value.Value = this.Value.Default;
                    this.Box.Text = this.Value.Value.GetDisplayString();
                    this.Default.Enabled = this.Value.Value != this.Value.Default;
                });
                base.Controls.Add(this.Description);
                base.Controls.Add(this.Box);
                base.Controls.Add(this.Set);
                base.Controls.Add(this.Default);
            }
        }
    }
}