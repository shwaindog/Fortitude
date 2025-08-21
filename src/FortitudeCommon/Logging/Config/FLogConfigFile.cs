using FortitudeCommon.Extensions;

namespace FortitudeCommon.Logging.Config;

public static class FLogConfigFile
{
    public const string DefaultConfigPath         = ".";
    public const string DefaultConfigFileName     = "FLogConfig.json";
    public const string DefaultConfigFullFilePath = $"{DefaultConfigPath}/{DefaultConfigFileName}";

    public const string EnvVarNameForDefaultFullConfigFilePath = "FLogConfigFullFilePath";
    public const string EnvVarNameForConfigDirPath = "FLogConfigDirPath";
    public const string EnvVarNameForConfigFileName = "FLogConfigFileName";

    private static string? resolvedConfigFilePath;
    private static string? resolvedConfigDirPath;
    private static string? resolvedConfigFileName;

    public static string ConfigFullFilePath
    {
        get
        {
            if (resolvedConfigFilePath != null) return resolvedConfigFilePath;
            var envVarDefaultFlogFullConfigPath = Environment.GetEnvironmentVariable(EnvVarNameForDefaultFullConfigFilePath);
            if (envVarDefaultFlogFullConfigPath != null) return resolvedConfigFilePath ??= envVarDefaultFlogFullConfigPath;
            resolvedConfigDirPath  = Environment.GetEnvironmentVariable(EnvVarNameForConfigDirPath);
            resolvedConfigFileName = Environment.GetEnvironmentVariable(EnvVarNameForConfigFileName);
            if (resolvedConfigDirPath == null && resolvedConfigFileName == null) return resolvedConfigFilePath ??= DefaultConfigFullFilePath;
            resolvedConfigDirPath  ??= DefaultConfigPath;
            resolvedConfigFileName ??= DefaultConfigFileName;
            return resolvedConfigFilePath ??= $"{resolvedConfigDirPath}/{resolvedConfigFileName}";
        }
    }

    public static string ConfigHostFileName(string machineName)
    {
        var originalFileName = ConfigFileName;
        var machineConfigFile = stackalloc char[originalFileName.Length + machineName.Length + 1].ResetMemory();
        var lastIndexOf = machineName.LastIndexOf('.');
        if (lastIndexOf >= 0)
        {
            machineConfigFile.Append(originalFileName[..lastIndexOf]);
            machineConfigFile.Append('-');
            machineConfigFile.Append(machineName);
            machineConfigFile.Append(originalFileName[(lastIndexOf)..]);
            var asString = new String(machineConfigFile);
            return asString;
        }
        return  originalFileName + "-" + machineName;
    }

    public static string ConfigHostFullFilePath(string machineName)
    {
        return  $"{ConfigFileDirPath}/{ConfigHostFileName(machineName)}";
    }

    public static string ConfigLoginHostFileName(string login, string machineName)
    {
        var originalFileName = ConfigFileName;
        var machineConfigFile = stackalloc char[originalFileName.Length + login.Length + machineName.Length + 2].ResetMemory();
        var lastIndexOf = machineName.LastIndexOf('.');
        if (lastIndexOf >= 0)
        {
            machineConfigFile.Append(originalFileName[..lastIndexOf]);
            machineConfigFile.Append('-');
            machineConfigFile.Append(login);
            machineConfigFile.Append('-');
            machineConfigFile.Append(machineName);
            machineConfigFile.Append(originalFileName[(lastIndexOf)..]);
            var asString = new String(machineConfigFile);
            return asString;
        }
        return  originalFileName + "-" + login + "-" +  machineName;
    }

    public static string ConfigLoginHostFullFilePath(string login, string machineName)
    {
        return  $"{ConfigFileDirPath}/{ConfigLoginHostFileName(login, machineName)}";
    }

    public static string ConfigFileDirPath
    {
        get
        {
            if (resolvedConfigDirPath != null) return resolvedConfigDirPath;
            var envVarDefaultFlogFullConfigPath = Environment.GetEnvironmentVariable(EnvVarNameForDefaultFullConfigFilePath);
            if (envVarDefaultFlogFullConfigPath != null)
            {
                resolvedConfigDirPath = Path.GetDirectoryName(envVarDefaultFlogFullConfigPath);
                return resolvedConfigDirPath!;
            }
            resolvedConfigDirPath       =   Environment.GetEnvironmentVariable(EnvVarNameForConfigDirPath);
            return resolvedConfigDirPath ??= DefaultConfigPath;
        }
    }

    public static string ConfigFileName
    {
        get
        {
            if (resolvedConfigFileName != null) return resolvedConfigFileName;
            var envVarDefaultFlogFullConfigPath = Environment.GetEnvironmentVariable(EnvVarNameForDefaultFullConfigFilePath);
            if (envVarDefaultFlogFullConfigPath != null)
            {
                resolvedConfigFileName = Path.GetFileName(envVarDefaultFlogFullConfigPath);
                return resolvedConfigFileName!;
            }
            resolvedConfigFileName       =   Environment.GetEnvironmentVariable(EnvVarNameForConfigFileName);
            return resolvedConfigDirPath ??= DefaultConfigFileName;
        }
    }
}
