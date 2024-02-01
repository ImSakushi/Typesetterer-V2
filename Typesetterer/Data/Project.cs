using Ps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Typesetterer.Data
{
    public class Project
    {
        [XmlIgnore]
        public readonly static string ProjectExtension;

        public List<Group> Groups
        {
            get;
            set;
        }

        [XmlIgnore]
        public string Name
        {
            get;
            set;
        }

        public string Pairings
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Group group in this.Groups)
                {
                    for (int i = 0; i < group.Styles.Count; i++)
                    {
                        stringBuilder.Append(this.Styles.IndexOf(group.Styles[i]));
                        if (i < group.Styles.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                    stringBuilder.Append(";");
                }
                return stringBuilder.ToString();
            }
            set
            {
                string[] strArrays = value.Split(new char[] { ';' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string[] strArrays1 = strArrays[i].Split(new char[] { ',' });
                    for (int j = 0; j < (int)strArrays1.Length; j++)
                    {
                        if (strArrays1[j].Length > 0)
                        {
                            try
                            {
                                int num = int.Parse(strArrays1[j]);
                                this.Groups[i].Styles.Add(this.Styles[num]);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        public List<TextStyle> Styles
        {
            get;
            set;
        }

        static Project()
        {
            Project.ProjectExtension = ".et2";
        }

        public Project()
        {
            this.Groups = new List<Group>();
            this.Styles = new List<TextStyle>();
            this.Name = "Default";
        }

        public Project(Project toCopy) : this()
        {
            foreach (TextStyle style in toCopy.Styles)
            {
                this.Styles.Add(new TextStyle(style));
            }
            foreach (Group group in toCopy.Groups)
            {
                Group group1 = new Group(group.Name, group.LabelColor);
                this.Groups.Add(group1);
                for (int i = 0; i < group.Styles.Count; i++)
                {
                    int num = toCopy.Styles.IndexOf(group.Styles[i]);
                    group1.Styles.Add(this.Styles[num]);
                }
            }
            this.Name = string.Concat(toCopy.Name, " Copy");
            List<string> strs = new List<string>();
            foreach (object project in State.Projects)
            {
                strs.Add(((Project)project).Name);
            }
            for (int j = 0; j < 200; j++)
            {
                if (j > 0)
                {
                    this.Name = string.Format("{0} Copy {1}", toCopy.Name, j);
                }
                if (!strs.Contains(this.Name))
                {
                    break;
                }
            }
            this.Save();
        }

        public void Delete()
        {
            File.Delete(Path.Combine(State.ProjectsPath, string.Concat(this.Name, Project.ProjectExtension)));
        }

        public void DrawStyleColors(Graphics g, TextStyle style, Rectangle bounds, bool vertical = true)
        {
            Group[] array = this.GetGroupsForStyle(style).ToArray<Group>();
            if ((int)array.Length < 1)
            {
                return;
            }
            if (vertical)
            {
                float height = (float)bounds.Height / (float)((int)array.Length);
                float y = (float)bounds.Y;
                for (int i = 0; i < (int)array.Length; i++)
                {
                    RectangleF rectangleF = new RectangleF((float)bounds.Left, y, (float)bounds.Width, height);
                    using (SolidBrush solidBrush = new SolidBrush(array[i].LabelColor))
                    {
                        g.FillRectangle(solidBrush, rectangleF);
                    }
                    y += height;
                }
                return;
            }
            float width = (float)bounds.Width / (float)((int)array.Length);
            float x = (float)bounds.X;
            for (int j = 0; j < (int)array.Length; j++)
            {
                RectangleF rectangleF1 = new RectangleF(x, (float)bounds.Top, width, (float)bounds.Height);
                using (SolidBrush solidBrush1 = new SolidBrush(array[j].LabelColor))
                {
                    g.FillRectangle(solidBrush1, rectangleF1);
                }
                x += width;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                Project project = obj as Project;
                if (project != null)
                {
                    return this.Name == project.Name;
                }
            }
            return false;
        }

        public IEnumerable<Group> GetGroupsForStyle(TextStyle style)
        {
            Project project = null;
            foreach (Group group in project.Groups)
            {
                if (!group.Styles.Contains(style))
                {
                    continue;
                }
                yield return group;
            }
        }

        public override int GetHashCode()
        {
            return new { Name = this.Name, Groups = this.Groups, Styles = this.Styles }.GetHashCode();
        }

        public static Project Load(string filepath)
        {
            Project project;
            Project project1 = null;
            using (ZipArchive zipArchive = ZipFile.OpenRead(filepath))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
                ZipArchiveEntry entry = zipArchive.GetEntry(string.Concat(fileNameWithoutExtension, ".txt"));
                if (entry != null)
                {
                    List<string> strs = new List<string>();
                    using (Stream stream = entry.Open())
                    {
                        project1 = Loader.ImportAsXml<Project>(stream);
                        if (project1 != null)
                        {
                            project1.Name = fileNameWithoutExtension;
                            foreach (TextStyle style in project1.Styles)
                            {
                                string filename = style.Font.Filename;
                                if (!string.IsNullOrEmpty(FontFactory.GetFilename(style.Font)) || strs.Contains(filename))
                                {
                                    continue;
                                }
                                strs.Add(filename);
                            }
                        }
                    }
                    List<string>.Enumerator enumerator = strs.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            string current = enumerator.Current;
                            if (string.IsNullOrEmpty(current))
                            {
                                continue;
                            }
                            string str = Path.Combine(State.TemporaryFontsPath, current);
                            if (!File.Exists(str))
                            {
                                ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry(current);
                                if (zipArchiveEntry != null)
                                {
                                    zipArchiveEntry.ExtractToFile(str);
                                }
                            }
                            FontFactory.AddFont(str);
                        }
                        return project1;
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                else
                {
                    project = project1;
                }
            }
            return project;
        }

        public void RemoveStyle(TextStyle style)
        {
            foreach (Group group in this.Groups)
            {
                group.Styles.Remove(style);
            }
            this.Styles.Remove(style);
        }

        public void Rename(string name)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            name = string.Join("_", name.Split(invalidFileNameChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd(new char[] { '.' });
            if (this.Name != name)
            {
                this.Delete();
                this.Name = name;
                this.Save();
            }
        }

        public void ResetToDefaults()
        {
            this.Groups.Clear();
            this.Groups.AddRange(new List<Group>()
            {
                new Group("Bubble", Color.FromArgb(255, 117, 110)),
                new Group("Overlay", Color.FromArgb(250, 167, 85)),
                new Group("SFX", Color.FromArgb(137, 189, 248)),
                new Group("Special", Color.FromArgb(162, 141, 221))
            });
        }

        public void Save()
        {
            string str = Path.Combine(State.ProjectsPath, this.Name);
            string str1 = Path.Combine(State.ProjectsPath, string.Concat(this.Name, Project.ProjectExtension));
            if (File.Exists(str1))
            {
                File.Delete(str1);
            }
            Directory.CreateDirectory(str);
            foreach (TextStyle style in this.Styles)
            {
                string filename = FontFactory.GetFilename(style.Font);
                style.Font.Filename = Path.GetFileName(filename);
                string str2 = Path.Combine(str, Path.GetFileName(filename));
                File.Copy(filename, str2, true);
                (new FileInfo(str2)).IsReadOnly = false;
            }
            this.ExportAsXml(Path.Combine(str, string.Concat(this.Name, ".txt")));
            ZipFile.CreateFromDirectory(str, str1, CompressionLevel.Optimal, false);
            Directory.Delete(str, true);
        }

        public void SetGroupsForStyle(TextStyle style, List<Group> setGroups)
        {
            if (!this.Styles.Contains(style))
            {
                this.Styles.Add(style);
            }
            foreach (Group group in this.Groups)
            {
                if (!setGroups.Contains(group))
                {
                    group.Styles.Remove(style);
                }
                else
                {
                    if (group.Styles.Contains(style))
                    {
                        continue;
                    }
                    group.Styles.Add(style);
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}