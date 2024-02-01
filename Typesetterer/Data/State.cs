using Ps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Typesetterer.Overlay.Legacy;
using Typesetterer.Properties;

namespace Typesetterer.Data
{
    public static class State
    {
        public static BoolSource SaveOnNext;

        public readonly static string ShortcutPath;

        public readonly static string ProjectsPath;

        public readonly static string UnfinishedPath;

        public readonly static string TemporaryFontsPath;

        public readonly static string TempPrototyperDirName;

        public static BindingSource Pages;

        public static BindingSource Page;

        public static BindingSource Projects;

        private static Ps.Photoshop _photoshop
        {
            get;
            set;
        }

        public static LineToTypeset CurrentLine
        {
            get
            {
                return State.Page.Current as LineToTypeset;
            }
        }

        public static State.Mode CurrentMode
        {
            get;
            set;
        }

        public static LinkedPage CurrentPage
        {
            get
            {
                return State.Pages.Current as LinkedPage;
            }
        }

        public static Project CurrentProject
        {
            get
            {
                return State.Projects.Current as Project;
            }
        }

        public static Ps.Photoshop Photoshop
        {
            get
            {
                return State._photoshop;
            }
            set
            {
                State._photoshop = value;
                value.LoadFonts(new Action<Dictionary<string, List<TextFont>>>(State.LoadPhotoshopFonts));
            }
        }

        private static Dictionary<string, List<TextFont>> PhotoshopFontInformation
        {
            get;
            set;
        }

        public static BindingSource PhotoshopFonts
        {
            get;
            set;
        }

        public static BindingSource PhotoshopFontStyles
        {
            get;
            set;
        }

        public static KeyboardShortcuts Shortcuts
        {
            get;
            private set;
        }

        static State()
        {
            State.SaveOnNext = new BoolSource();
            State.ShortcutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shortcuts.txt");
            State.ProjectsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projects");
            State.UnfinishedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Unfinished");
            State.TemporaryFontsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temporary Fonts");
            State.TempPrototyperDirName = "prototyper";
            State.Pages = new BindingSource();
            State.Page = new BindingSource();
            State.Projects = new BindingSource();
        }

        public static void ConvertOldProject(string filename)
        {
            Project project = Project.Load(Converter.ConvertOldStyleToNew(filename));
            State.Projects.Add(project);
            FontFactory.ReconcileInstalledFonts();
            State.Projects.Position = State.Projects.IndexOf(project);
        }

        public static void Initialize()
        {
            State.SaveOnNext.Value = Settings.Default.SaveOnNext;
            State.SaveOnNext.PropertyChanged += new PropertyChangedEventHandler((object s, PropertyChangedEventArgs e) => Settings.Default.SaveOnNext = State.SaveOnNext.Value);
            State.Pages.CurrentChanged += new EventHandler((object s, EventArgs e) =>
            {
                State.Page.DataSource = State.CurrentPage.Lines;
                if (State.Photoshop != null)
                {
                    State.Photoshop.OpenDocument(State.CurrentPage.PsdPath);
                    State.Photoshop.SelectTopmostLayer();
                }
            });
            State.Page.DataSourceChanged += new EventHandler((object s, EventArgs e) =>
            {
                if (State.Page.Count > 1)
                {
                    int count = State.Page.Count;
                    while (count > 0 && !(State.Page[count - 1] as LineToTypeset).HasBeenTyped)
                    {
                        count--;
                    }
                    State.Page.Position = count;
                }
            });
            string lastProject = Settings.Default.LastProject;
            State.Projects.CurrentChanged += new EventHandler((object s, EventArgs e) =>
            {
                if (State.CurrentProject != null)
                {
                    Settings.Default.LastProject = State.CurrentProject.Name;
                }
            });
            Directory.CreateDirectory(State.ProjectsPath);
            Directory.CreateDirectory(State.TemporaryFontsPath);
            FileInfo[] files = (new DirectoryInfo(State.ProjectsPath)).GetFiles(string.Concat("*", Project.ProjectExtension));
            for (int i = 0; i < (int)files.Length; i++)
            {
                Project project = Project.Load(files[i].FullName);
                if (project != null)
                {
                    State.Projects.Add(project);
                }
            }
            FontFactory.ReconcileInstalledFonts();
            if (State.Projects.Count == 0)
            {
                Project project1 = new Project()
                {
                    Name = "Default"
                };
                project1.ResetToDefaults();
                project1.Save();
                State.Projects.Add(project1);
            }
            State.Projects.Sort = "Name";
            if (!string.IsNullOrEmpty(lastProject))
            {
                int num = State.Projects.List.OfType<Project>().ToList<Project>().FindIndex((Project f) => f.Name == lastProject);
                if (num > -1)
                {
                    State.Projects.Position = num;
                }
            }
            State.Shortcuts = Loader.ImportAsXml<KeyboardShortcuts>(State.ShortcutPath) ?? new KeyboardShortcuts();
            State.PhotoshopFonts = new BindingSource();
            State.PhotoshopFontStyles = new BindingSource();
            State.PhotoshopFonts.PositionChanged += new EventHandler((object s, EventArgs e) => State.PhotoshopFontStyles.DataSource = State.PhotoshopFontInformation[State.PhotoshopFonts.Current as string]);
        }

        private static void LoadPhotoshopFonts(Dictionary<string, List<TextFont>> fonts)
        {
            List<TextFont> textFonts;
            foreach (KeyValuePair<string, List<TextFont>> temporaryFont in FontFactory.TemporaryFonts)
            {
                if (fonts.TryGetValue(temporaryFont.Key, out textFonts))
                {
                    foreach (TextFont value in temporaryFont.Value)
                    {
                        if (textFonts.Contains(value))
                        {
                            continue;
                        }
                        textFonts.Add(value);
                    }
                }
                else
                {
                    fonts.Add(temporaryFont.Key, temporaryFont.Value);
                }
            }
            State.PhotoshopFontInformation = fonts;
            List<string> list = fonts.Keys.ToList<string>();
            list.Sort();
            State.PhotoshopFonts.DataSource = list;
        }

        public enum Mode
        {
            None,
            Overlay,
            Prototyper
        }
    }
}