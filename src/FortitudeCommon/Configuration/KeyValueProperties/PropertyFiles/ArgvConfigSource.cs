using System;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public class ArgvConfigSource : ConfigSourceBase
    {
        #region Constructors

        public ArgvConfigSource(string[] arguments)
        {
            parser = new ArgvParser(arguments);
            this.arguments = arguments;
        }

        #endregion

        #region Private methods
        private IConfig GetConfig(string name)
        {
            IConfig result;

            if (Configs[name] == null)
            {
                result = new ConfigBase(name, this);
                Configs.Add(result);
            }
            else
            {
                result = Configs[name];
            }

            return result;
        }

        #endregion

        #region Private variables

        private readonly string[] arguments;
        private readonly ArgvParser parser;

        #endregion

        #region Public methods

        public override void Save()
        {
            throw new ArgumentException("Source is read only");
        }

        public override void Reload()
        {
            throw new ArgumentException("Source cannot be reloaded");
        }

        public void AddSwitch(string configName, string longName)
        {
            AddSwitch(configName, longName, null);
        }

        public void AddSwitch(string configName, string longName,
            string shortName)
        {
            var config = GetConfig(configName);

            if (shortName != null &&
                (shortName.Length < 1 || shortName.Length > 2))
            {
                throw new ArgumentException("Short name may only be 1 or 2 characters");
            }

            // Look for the long name first
            if (parser[longName] != null)
            {
                config.Set(longName, parser[longName]);
            }
            else if (shortName != null && parser[shortName] != null)
            {
                config.Set(longName, parser[shortName]);
            }
        }

        public string[] GetArguments()
        {
            var result = new string[arguments.Length];
            Array.Copy(arguments, 0, result, 0, arguments.Length);

            return result;
        }

        #endregion
    }
}