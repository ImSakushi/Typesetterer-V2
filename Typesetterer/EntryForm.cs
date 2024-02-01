using FlatUI;
using Ps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Typesetterer.Data;
using Typesetterer.Overlay;
using Typesetterer.Properties;
using Typesetterer.Prototyper;
using Typesetterer.Releasify;
using Typesetterer.Wizard;

namespace Typesetterer
{
    internal class EntryForm : PhaseForm
    {
        private readonly EntryForm.WinEventDelegate WinEvent;

        private const int EVENT_SYSTEM_FOREGROUND = 3;

        private const int GWL_EXSTYLE = -20;

        private const short SWP_NOMOVE = 2;

        private const short SWP_NOSIZE = 1;

        private const short SWP_NOACTIVATE = 16;

        private const short SWP_SHOWWINDOW = 64;

        private const uint WS_EX_TOPMOST = 8;

        private FormWindowState _photoshopState;

        private FormWindowState PhotoshopState
        {
            get
            {
                return this._photoshopState;
            }
            set
            {
                if (this.PhotoshopState != value)
                {
                    this._photoshopState = value;
                    if (value != FormWindowState.Minimized)
                    {
                        if (value == FormWindowState.Maximized)
                        {
                            base.TopMost = true;
                            return;
                        }
                        base.TopMost = true;
                    }
                }
            }
        }

        private bool SettingStuff
        {
            get;
            set;
        }

        private bool Transitioned
        {
            get;
            set;
        }

        public EntryForm()
        {
            Action action2 = null;
            Action action3 = null;
            Action action4 = null;
            Action action5 = null;
            Action action6 = null;
            base.FormTitle = "Typesetterer 2";
            base.ByteIcon = ByteImages.Logo;
            this.Text = base.FormTitle;
            this.ForeColor = Theme.DarkForeground;
            base.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.WinEvent = new EntryForm.WinEventDelegate(this.WinEventProc);
            EntryForm.SetWinEventHook(3, 3, IntPtr.Zero, this.WinEvent, 0, 0, 0);
            State.Initialize();
            EntryPointPhase entryPointPhase = new EntryPointPhase(this);
            WizardPhase wizardPhase = new WizardPhase(this);
            PrototyperPhase prototyperPhase = new PrototyperPhase(this);
            OverlayPhase overlayPhase = new OverlayPhase(this);
            ReleasifyPhase releasifyPhase = new ReleasifyPhase(this);
            this.Phases.Add(EntryForm.Phase.EntryPoint, entryPointPhase);
            this.Phases.Add(EntryForm.Phase.Wizard, wizardPhase);
            this.Phases.Add(EntryForm.Phase.Prototyper, prototyperPhase);
            this.Phases.Add(EntryForm.Phase.Overlay, overlayPhase);
            this.Phases.Add(EntryForm.Phase.Releasify, releasifyPhase);
            wizardPhase.LaunchPrototyper += new EventHandler((object s, EventArgs e) =>
            {
                State.Photoshop = new Photoshop((e as LaunchEventArgs).PhotoshopVersion);
                this.FormPhase = EntryForm.Phase.Prototyper;
                State.CurrentMode = State.Mode.Prototyper;
                this.Transitioned = false;
                EntryForm u003cu003e4_this = this;
                Action u003cu003e9_6 = action2;
                if (u003cu003e9_6 == null)
                {
                    Action action = () =>
                    {
                        Theme.Accent.Set(Color.FromArgb(0, Theme.Blue));
                        Theme.Accent.To = Theme.Blue;
                        prototyperPhase.SetToolWindowVisibility(true);
                        this.CompositeButtonGroup.Reset();
                        this.Transitioned = true;
                    };
                    Action action1 = action;
                    action2 = action;
                    u003cu003e9_6 = action1;
                }
                u003cu003e4_this.ToPerformAfterTransition = u003cu003e9_6;
            });
            wizardPhase.LaunchOverlay += new EventHandler((object s, EventArgs e) =>
            {
                base.FormPhase = EntryForm.Phase.Overlay;
                State.CurrentMode = State.Mode.Overlay;
                this.Transitioned = false;
                LaunchEventArgs launchEventArg = e as LaunchEventArgs;
                base.ToPerformAfterTransition = () =>
                {
                    State.Photoshop = new Photoshop(launchEventArg.PhotoshopVersion);
                    State.Photoshop.BringToFront();
                    this.Transitioned = true;
                };
            });
            entryPointPhase.ResumeClicked += new EventHandler((object s, EventArgs e) =>
            {
                Action action;
                ReleaseState releaseState = Loader.ImportAsXml<ReleaseState>(Path.Combine(State.UnfinishedPath, "last.txt"));
                State.Pages.DataSource = releaseState.LinkedPages;
                State.Pages.Position = releaseState.PageIndex;
                State.Photoshop = new Photoshop(releaseState.PhotoshopVersion);
                if (releaseState.TypesettererState != ReleaseState.States.Prototyper)
                {
                    this.FormPhase = EntryForm.Phase.Overlay;
                    State.CurrentMode = State.Mode.Overlay;
                    this.Transitioned = false;
                    EntryForm u003cu003e4_this = this;
                    Action u003cu003e9_10 = action4;
                    if (u003cu003e9_10 == null)
                    {
                        Action front = () =>
                        {
                            State.Photoshop.BringToFront();
                            overlayPhase.Sync();
                            this.CompositeButtonGroup.Reset();
                            this.Transitioned = true;
                        };
                        action = front;
                        action4 = front;
                        u003cu003e9_10 = action;
                    }
                    u003cu003e4_this.ToPerformAfterTransition = u003cu003e9_10;
                    return;
                }
                State.Photoshop.InsertCallback(() => this.UIThread(() => base.Activate()));
                this.FormPhase = EntryForm.Phase.Prototyper;
                State.CurrentMode = State.Mode.Prototyper;
                this.Transitioned = false;
                EntryForm entryForm = this;
                Action u003cu003e9_9 = action3;
                if (u003cu003e9_9 == null)
                {
                    Action action1 = () =>
                    {
                        Theme.Accent.Set(Color.FromArgb(0, Theme.Blue));
                        Theme.Accent.To = Theme.Blue;
                        prototyperPhase.SetToolWindowVisibility(true);
                        this.CompositeButtonGroup.Reset();
                        this.Transitioned = true;
                    };
                    action = action1;
                    action3 = action1;
                    u003cu003e9_9 = action;
                }
                entryForm.ToPerformAfterTransition = u003cu003e9_9;
            });
            prototyperPhase.TransitionToOverlay += new EventHandler((object s, EventArgs e) =>
            {
                if (this.WindowState != FormWindowState.Maximized)
                {
                    Settings.Default.PrototyperFormLocation = this.Location;
                    Settings.Default.PrototyperFormSize = this.Size;
                }
                else
                {
                    Settings @default = Settings.Default;
                    Rectangle restoreBounds = this.RestoreBounds;
                    @default.PrototyperFormLocation = restoreBounds.Location;
                    Settings size = Settings.Default;
                    restoreBounds = this.RestoreBounds;
                    size.PrototyperScriptFormSize = restoreBounds.Size;
                    this.WindowState = FormWindowState.Normal;
                }
                this.FormPhase = EntryForm.Phase.Overlay;
                State.CurrentMode = State.Mode.Overlay;
                this.Transitioned = false;
                EntryForm u003cu003e4_this = this;
                Action u003cu003e9_12 = action5;
                if (u003cu003e9_12 == null)
                {
                    Action action = () =>
                    {
                        State.Photoshop.OpenDocument(State.CurrentPage.PsdPath);
                        State.Photoshop.BringToFront();
                        base.BringToFront();
                        overlayPhase.Sync();
                        this.CompositeButtonGroup.Reset();
                        this.Transitioned = true;
                    };
                    Action action1 = action;
                    action5 = action;
                    u003cu003e9_12 = action1;
                }
                u003cu003e4_this.ToPerformAfterTransition = u003cu003e9_12;
            });
            overlayPhase.TransitionToPrototyper += new EventHandler((object s, EventArgs e) =>
            {
                Settings.Default.OverlayLocation = this.Location;
                Settings.Default.OverlaySize = this.Size;
                if (State.Photoshop.Version.Number < 11)
                {
                    State.Photoshop.Minimize();
                }
                this.FormPhase = EntryForm.Phase.Prototyper;
                State.CurrentMode = State.Mode.Prototyper;
                this.Transitioned = false;
                EntryForm u003cu003e4_this = this;
                Action u003cu003e9_13 = action6;
                if (u003cu003e9_13 == null)
                {
                    Action action = () =>
                    {
                        prototyperPhase.SetToolWindowVisibility(true);
                        base.Activate();
                        this.CompositeButtonGroup.Reset();
                        prototyperPhase.Panel.Invalidate();
                        this.Transitioned = true;
                    };
                    Action action1 = action;
                    action6 = action;
                    u003cu003e9_13 = action1;
                }
                u003cu003e4_this.ToPerformAfterTransition = u003cu003e9_13;
            });
            base.FormClosing += new FormClosingEventHandler((object s, FormClosingEventArgs e) =>
            {
                foreach (KeyValuePair<Enum, Typesetterer.Phase> phase in this.Phases)
                {
                    phase.Value.HandleApplicationClosing();
                }
                Settings.Default.Save();
                State.Shortcuts.ExportAsXml(State.ShortcutPath);
            });
            this.SetPhase(EntryForm.Phase.EntryPoint);
            Theme.Accent.Frames = Transitions.DelayedEaseUp;
            base.TrueCenterToScreen();
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, EntryForm.WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (!this.SettingStuff && this.Transitioned && State.CurrentMode == State.Mode.Overlay && State.Photoshop.Window != null && !base.IsDisposed)
            {
                this.SettingStuff = true;
                if (State.Photoshop.Window.Handle == hwnd)
                {
                    EntryForm.SetWindowPos(base.Handle, -1, 0, 0, 0, 0, 3);
                }
                else if (hwnd != base.Handle && ((long)EntryForm.GetWindowLong(base.Handle, -20) & (long)8) != 0)
                {
                    EntryForm.SetWindowPos(base.Handle, -2, 0, 0, 0, 0, 19);
                    EntryForm.SetWindowPos(hwnd, 0, 0, 0, 0, 0, 67);
                }
                this.SettingStuff = false;
            }
        }

        public enum Phase
        {
            EntryPoint,
            Wizard,
            Prototyper,
            Overlay,
            Releasify
        }

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    }
}