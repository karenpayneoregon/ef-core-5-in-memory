using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace GlobbingConsoleNetCoreApp
{
    public class GlobbingOperations
    {
        public static IEnumerable<string> _includePatterns;
        public static IEnumerable<string>[] _excludePatterns;

        public static string SearchFolder => "C:\\Users\\paynek\\Documents\\Snagit\\";

        /// <summary>
        /// If <see cref="SearchFolder"/> does not exists, exit stage right :-)
        /// </summary>
        public static void Sample1()
        {
            if (!Directory.Exists(SearchFolder)) return;

            Matcher matcher = new();
            matcher.AddIncludePatterns(new[] { "*.pdf", "**/*.ico" });


            PatternMatchingResult matchingResult = matcher.Execute(
                new DirectoryInfoWrapper(new DirectoryInfo(SearchFolder)));


            if (matchingResult.HasMatches)
            {

                foreach (var file in matchingResult.Files)
                {
                    Debug.WriteLine(file.Path);
                }

                Debug.WriteLine($"Match count {matchingResult.Files.Count()}");

            }
            else
            {
                Debug.WriteLine("No matches");
            }


        }

        public static PatternMatchingResult Sample2(DirectoryInfoBase directoryInfo)
        {
            var files = new List<FilePatternMatch>();

            foreach (var includePattern in _includePatterns)
            {
                Matcher matcher = new();
                
                matcher.AddInclude(includePattern);
                matcher.AddExcludePatterns(_excludePatterns);

                var matchingResult = matcher.Execute(directoryInfo);

                if (matchingResult.Files.Any())
                {
                    files.AddRange(matchingResult.Files);
                }
            }

            return new PatternMatchingResult(files);
        }
    }
}