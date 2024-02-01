using Ps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace Typesetterer.Data
{
    [Serializable]
    public class Group
    {
        [XmlIgnore]
        public List<TextStyle> Styles;

        [XmlElement(Type = typeof(XmlColor))]
        public Color LabelColor
        {
            get;
            set;
        }

        [XmlAttribute]
        public string Name
        {
            get;
            set;
        }

        public Group() : this("", Color.Empty, new List<TextStyle>())
        {
        }

        public Group(string name, Color color) : this(name, color, new List<TextStyle>())
        {
        }

        public Group(string name, Color color, List<TextStyle> styles)
        {
            this.Name = name;
            this.LabelColor = color;
            this.Styles = styles;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Group))
            {
                return false;
            }
            Group group = obj as Group;
            if (!group.Name.Equals(this.Name))
            {
                return false;
            }
            return group.LabelColor == this.LabelColor;
        }

        public override int GetHashCode()
        {
            return new { Name = this.Name, LabelColor = this.LabelColor }.GetHashCode();
        }
    }
}