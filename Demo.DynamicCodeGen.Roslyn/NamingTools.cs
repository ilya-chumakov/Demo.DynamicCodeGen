using System.Text.RegularExpressions;

namespace Demo.DynamicCodeGen.Roslyn
{
    public static class NamingTools
    {
        /// <summary>
        /// Replace all chars except ASCII
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string Clean(string typeName)
        {
            return Regex.Replace(typeName, @"[^a-zA-Z0-9 -]", string.Empty);
        }
    }
}