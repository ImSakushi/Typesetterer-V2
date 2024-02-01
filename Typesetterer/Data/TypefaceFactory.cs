using Ps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Typesetterer.Data
{
    public static class TypefaceFactory
    {
        private static Dictionary<ValueTuple<TextFont, bool, bool>, GlyphTypeface> GlyphTypefaces
        {
            get;
        }

        private static Dictionary<ValueTuple<TextFont, bool, bool>, Typeface> Typefaces
        {
            get;
        }

        static TypefaceFactory()
        {
            TypefaceFactory.Typefaces = new Dictionary<ValueTuple<TextFont, bool, bool>, Typeface>();
            TypefaceFactory.GlyphTypefaces = new Dictionary<ValueTuple<TextFont, bool, bool>, GlyphTypeface>();
        }

        public static GlyphTypeface CreateGlyphs([TupleElementNames(new string[] { "font", "bold", "italic" })] ValueTuple<TextFont, bool, bool> key)
        {
            GlyphTypeface glyphTypeface;
            GlyphTypeface glyphTypeface1;
            lock (TypefaceFactory.GlyphTypefaces)
            {
                if (!TypefaceFactory.GlyphTypefaces.TryGetValue(key, out glyphTypeface))
                {
                    if (!TypefaceFactory.CreateTypeface(key).TryGetGlyphTypeface(out glyphTypeface))
                    {
                        string filename = FontFactory.GetFilename(key.Item1);
                        if (!string.IsNullOrEmpty(filename))
                        {
                            Console.WriteLine(string.Concat("failed to get glyphs by typeface for ", key.Item1.Name));
                            glyphTypeface = new GlyphTypeface(new Uri(filename));
                        }
                    }
                    if (glyphTypeface == null)
                    {
                        Console.WriteLine(string.Concat("failed to get glyphs for ", key.Item1.Name));
                    }
                    else
                    {
                        TypefaceFactory.GlyphTypefaces.Add(key, glyphTypeface);
                    }
                }
                glyphTypeface1 = glyphTypeface;
            }
            return glyphTypeface1;
        }

        public static GlyphTypeface CreateGlyphs(TextStyle style)
        {
            return TypefaceFactory.CreateGlyphs(TypefaceFactory.GetKey(style));
        }

        public static GlyphTypeface CreateGlyphs(TextFont info)
        {
            return TypefaceFactory.CreateGlyphs(new ValueTuple<TextFont, bool, bool>(info, false, false));
        }

        private static Typeface CreateTypeface(FontFamily family, bool bold, bool italic)
        {
            FontStyle fontStyle = (italic ? FontStyles.Oblique : FontStyles.Normal);
            return new Typeface(family, fontStyle, (bold ? FontWeights.Bold : FontWeights.Normal), FontStretches.Normal);
        }

        private static Typeface CreateTypeface(string name, bool bold, bool italic)
        {
            return TypefaceFactory.CreateTypeface(new FontFamily(name), bold, italic);
        }

        private static Typeface CreateTypeface([TupleElementNames(new string[] { "font", "bold", "italic" })] ValueTuple<TextFont, bool, bool> key)
        {
            Typeface typeface;
            Typeface typeface1;
            lock (TypefaceFactory.Typefaces)
            {
                if (!TypefaceFactory.Typefaces.TryGetValue(key, out typeface))
                {
                    string filename = FontFactory.GetFilename(key.Item1);
                    if (!string.IsNullOrEmpty(filename))
                    {
                        ICollection<FontFamily> fontFamilies = Fonts.GetFontFamilies(filename);
                        if (fontFamilies.Count != 1)
                        {
                            Console.WriteLine(fontFamilies.Count);
                        }
                        else
                        {
                            FontFamily fontFamily = fontFamilies.First<FontFamily>();
                            string lower = key.Item1.SubFamily.ToLower();
                            bool flag = lower.Contains("bold");
                            bool flag1 = lower.Contains("italic");
                            typeface = TypefaceFactory.CreateTypeface(fontFamily, key.Item2 | flag, key.Item3 | flag1);
                        }
                    }
                    if (typeface == null)
                    {
                        typeface = TypefaceFactory.CreateTypeface(key.Item1.PostScriptName, key.Item2, key.Item3);
                    }
                    if (typeface == null)
                    {
                        typeface = TypefaceFactory.CreateTypeface(key.Item1.Name, key.Item2, key.Item3);
                    }
                    if (typeface == null)
                    {
                        Console.WriteLine(string.Concat("failed for ", key.Item1.Name));
                    }
                    else
                    {
                        TypefaceFactory.Typefaces.Add(key, typeface);
                    }
                }
                typeface1 = typeface;
            }
            return typeface1;
        }

        public static Typeface CreateTypeface(TextStyle style)
        {
            return TypefaceFactory.CreateTypeface(TypefaceFactory.GetKey(style));
        }

        [return: TupleElementNames(new string[] { "font", "bold", "italic" })]
        private static ValueTuple<TextFont, bool, bool> GetKey(TextStyle style)
        {
            TextFont font = style.Font;
            bool fauxBold = false;
            bool fauxItalic = false;
            StyleModifiers? styles = style.Styles;
            if (styles.HasValue)
            {
                StyleModifiers valueOrDefault = styles.GetValueOrDefault();
                fauxBold = valueOrDefault.FauxBold;
                fauxItalic = valueOrDefault.FauxItalic;
            }
            return new ValueTuple<TextFont, bool, bool>(font, fauxBold, fauxItalic);
        }
    }
}