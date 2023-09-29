using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class ArgvParser
    {
        #region Private variables

        private StringDictionary parameters;

        #endregion

        #region Public properties

        public string this[string param]
        {
            get { return parameters[param]; }
        }

        #endregion

        #region Private methods
        private void Extract(IEnumerable<string> args)
        {
            parameters = new StringDictionary();
            var splitter = new Regex(@"^([/-]|--){1}(?<name>\w+)([:=])?(?<value>.+)?$",
                RegexOptions.Compiled);
            char[] trimChars = {'"', '\''};
            string parameter = null;
            
            foreach (var arg in args)
            {
                var part = splitter.Match(arg);
                if (!part.Success)
                {
                    if (parameter != null)
                    {
                        parameters[parameter] = arg.Trim(trimChars);
                    }
                }
                else
                {
                    parameter = part.Groups["name"].Value;
                    parameters.Add(parameter,
                        part.Groups["value"].Value.Trim(trimChars));
                }
            }
        }

        #endregion

        #region Constructors

        public ArgvParser(string args)
        {
            var extractor = new Regex(@"(['""][^""]+['""])\s*|([^\s]+)\s*",
                RegexOptions.Compiled);
            
            var matches = extractor.Matches(args);
            var parts = new string[matches.Count - 1];

            for (var i = 1; i < matches.Count; i++)
            {
                parts[i - 1] = matches[i].Value.Trim();
            }
        }

        public ArgvParser(IEnumerable<string> args)
        {
            Extract(args);
        }

        #endregion
    }
}