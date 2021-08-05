using System.Text.RegularExpressions;

namespace Resources
{
    public static class PathResolver
    {

        internal static string GetPath(string filePath)
        {
            string exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            string appRoot = appPathMatcher.Match(exePath).Value;
            return System.IO.Path.Combine(appRoot, filePath);
        }

    }
}
