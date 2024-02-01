using System.Drawing;
using System.Drawing.Imaging;

namespace Typesetterer.Data
{
    public class PsdLoader
    {
        private readonly static float[] BytePCTs;

        static PsdLoader()
        {
            PsdLoader.BytePCTs = new float[] { 0f, 0.003921569f, 0.007843138f, 0.01176471f, 0.01568628f, 0.01960784f, 0.02352941f, 0.02745098f, 0.03137255f, 0.03529412f, 0.03921569f, 0.04313726f, 0.04705882f, 0.05098039f, 0.05490196f, 0.05882353f, 0.0627451f, 0.06666667f, 0.07058824f, 0.07450981f, 0.07843138f, 0.08235294f, 0.08627451f, 0.09019608f, 0.09411765f, 0.09803922f, 0.1019608f, 0.1058824f, 0.1098039f, 0.1137255f, 0.1176471f, 0.1215686f, 0.1254902f, 0.1294118f, 0.1333333f, 0.1372549f, 0.1411765f, 0.145098f, 0.1490196f, 0.1529412f, 0.1568628f, 0.1607843f, 0.1647059f, 0.1686275f, 0.172549f, 0.1764706f, 0.1803922f, 0.1843137f, 0.1882353f, 0.1921569f, 0.1960784f, 0.2f, 0.2039216f, 0.2078431f, 0.2117647f, 0.2156863f, 0.2196078f, 0.2235294f, 0.227451f, 0.2313726f, 0.2352941f, 0.2392157f, 0.2431373f, 0.2470588f, 0.2509804f, 0.254902f, 0.2588235f, 0.2627451f, 0.2666667f, 0.2705882f, 0.2745098f, 0.2784314f, 0.282353f, 0.2862745f, 0.2901961f, 0.2941177f, 0.2980392f, 0.3019608f, 0.3058824f, 0.3098039f, 0.3137255f, 0.3176471f, 0.3215686f, 0.3254902f, 0.3294118f, 0.3333333f, 0.3372549f, 0.3411765f, 0.345098f, 0.3490196f, 0.3529412f, 0.3568628f, 0.3607843f, 0.3647059f, 0.3686275f, 0.372549f, 0.3764706f, 0.3803922f, 0.3843137f, 0.3882353f, 0.3921569f, 0.3960784f, 0.4f, 0.4039216f, 0.4078431f, 0.4117647f, 0.4156863f, 0.4196078f, 0.4235294f, 0.427451f, 0.4313726f, 0.4352941f, 0.4392157f, 0.4431373f, 0.4470588f, 0.4509804f, 0.454902f, 0.4588235f, 0.4627451f, 0.4666667f, 0.4705882f, 0.4745098f, 0.4784314f, 0.4823529f, 0.4862745f, 0.4901961f, 0.4941176f, 0.4980392f, 0.5019608f, 0.5058824f, 0.509804f, 0.5137255f, 0.5176471f, 0.5215687f, 0.5254902f, 0.5294118f, 0.5333334f, 0.5372549f, 0.5411765f, 0.5450981f, 0.5490196f, 0.5529412f, 0.5568628f, 0.5607843f, 0.5647059f, 0.5686275f, 0.572549f, 0.5764706f, 0.5803922f, 0.5843138f, 0.5882353f, 0.5921569f, 0.5960785f, 0.6f, 0.6039216f, 0.6078432f, 0.6117647f, 0.6156863f, 0.6196079f, 0.6235294f, 0.627451f, 0.6313726f, 0.6352941f, 0.6392157f, 0.6431373f, 0.6470588f, 0.6509804f, 0.654902f, 0.6588235f, 0.6627451f, 0.6666667f, 0.6705883f, 0.6745098f, 0.6784314f, 0.682353f, 0.6862745f, 0.6901961f, 0.6941177f, 0.6980392f, 0.7019608f, 0.7058824f, 0.7098039f, 0.7137255f, 0.7176471f, 0.7215686f, 0.7254902f, 0.7294118f, 0.7333333f, 0.7372549f, 0.7411765f, 0.7450981f, 0.7490196f, 0.7529412f, 0.7568628f, 0.7607843f, 0.7647059f, 0.7686275f, 0.772549f, 0.7764706f, 0.7803922f, 0.7843137f, 0.7882353f, 0.7921569f, 0.7960784f, 0.8f, 0.8039216f, 0.8078431f, 0.8117647f, 0.8156863f, 0.8196079f, 0.8235294f, 0.827451f, 0.8313726f, 0.8352941f, 0.8392157f, 0.8431373f, 0.8470588f, 0.8509804f, 0.854902f, 0.8588235f, 0.8627451f, 0.8666667f, 0.8705882f, 0.8745098f, 0.8784314f, 0.8823529f, 0.8862745f, 0.8901961f, 0.8941177f, 0.8980392f, 0.9019608f, 0.9058824f, 0.9098039f, 0.9137255f, 0.9176471f, 0.9215686f, 0.9254902f, 0.9294118f, 0.9333333f, 0.9372549f, 0.9411765f, 0.945098f, 0.9490196f, 0.9529412f, 0.9568627f, 0.9607843f, 0.9647059f, 0.9686275f, 0.972549f, 0.9764706f, 0.9803922f, 0.9843137f, 0.9882353f, 0.9921569f, 0.9960784f, 1f };
        }

        public PsdLoader()
        {
        }

        public static unsafe Bitmap LoadFromFile(string filepath)
        {
            // 
            // Current member / type: System.Drawing.Bitmap Typesetterer.Data.PsdLoader::LoadFromFile(System.String)
            // File path: C:\Users\Pablo\Documents\Typesetterer-v2_0_0_0\Typesetterer.exe
            // 
            // Product version: 2019.1.118.0
            // Exception in: System.Drawing.Bitmap LoadFromFile(System.String)
            // 
            // L'argument spécifié n'était pas dans les limites de la plage des valeurs valides.
            // Nom du paramètre : Target of array indexer expression is not an array.
            //    à ..() dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\Expressions\ArrayIndexerExpression.cs:ligne 129
            //    à ..() dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\Expressions\UnaryExpression.cs:ligne 109
            //    à ..() dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Ast\Expressions\UnaryExpression.cs:ligne 95
            //    à Telerik.JustDecompiler.Decompiler.ExpressionDecompilerStep.() dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\ExpressionDecompilerStep.cs:ligne 143
            //    à Telerik.JustDecompiler.Decompiler.ExpressionDecompilerStep.(DecompilationContext ,  ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\ExpressionDecompilerStep.cs:ligne 73
            //    à ..(MethodBody ,  , ILanguage ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:ligne 88
            //    à ..(MethodBody , ILanguage ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\DecompilationPipeline.cs:ligne 70
            //    à Telerik.JustDecompiler.Decompiler.Extensions.( , ILanguage , MethodBody , DecompilationContext& ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:ligne 95
            //    à Telerik.JustDecompiler.Decompiler.Extensions.(MethodBody , ILanguage , DecompilationContext& ,  ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\Extensions.cs:ligne 58
            //    à ..(ILanguage , MethodDefinition ,  ) dans C:\DeveloperTooling_JD_Agent1\_work\15\s\OpenSource\Cecil.Decompiler\Decompiler\WriterContextServices\BaseWriterContextService.cs:ligne 117
            // 
            // mailto: JustDecompilePublicFeedback@telerik.com

        }

        public static void SetDefaultGrayscalePalette(Bitmap source)
        {
            ColorPalette palette = source.Palette;
            Color[] entries = palette.Entries;
            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }
            source.Palette = palette;
        }
    }
}