using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace ThemeConverter
{
    public class ThemeConverter : BaseCodeGeneratorWithSite
    {
        public const string Name = nameof(ThemeConverter);
        public const string Description = "Converts VS Code themes (JSON/JSONC) to .pkgdef";
        public override string GetDefaultExtension() => ".pkgdef";

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            var tmpFolder = Path.Combine(Path.GetTempPath(), "themes");

            try
            {
                PackageUtilities.EnsureOutputPath(tmpFolder);

                var tmpJson = Path.Combine(tmpFolder, Path.GetFileName(inputFileName));
                File.WriteAllText(tmpJson, CleanJson(inputFileContent));

                var extDir = Path.GetDirectoryName(GetType().Assembly.Location);
                var exe = Path.Combine(extDir, "tools", "ThemeConverter.exe");
                var args = $"/c ThemeConverter.exe -i \"{tmpJson}\"";

                RunExecutable(exe, args);

                var pkgdef = Path.ChangeExtension(tmpJson, ".pkgdef");
                var bytes = File.ReadAllBytes(pkgdef);

                return bytes;
            }
            catch (Exception ex)
            {
                ex.Log();
            }
            finally
            {
                Directory.Delete(tmpFolder, true);
            }

            return null;
        }

        public void RunExecutable(string exe, string args)
        {
            var start = new ProcessStartInfo("cmd")
            {
                WorkingDirectory = Path.GetDirectoryName(exe),
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(start)
                   .WaitForExit();
        }

        private static string CleanJson(string json)
        {
            //return Regex.Replace(json, @"^[\s]*//", "", RegexOptions.Multiline);

            var lines = json.Split('\r');
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                var clean = line.Trim().TrimStart('/');
                sb.AppendLine(clean);
            }

            return sb.ToString();
        }
    }
}
