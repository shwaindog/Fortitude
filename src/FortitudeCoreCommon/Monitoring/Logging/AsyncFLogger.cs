﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

internal sealed class AsyncFLogger : IFLogger
{
    private readonly IFLogger logger;
    private readonly PollingRing<FLogEvent> ring;
    private bool defaultEnabled = true;

    public AsyncFLogger(PollingRing<FLogEvent> ring, IFLogger logger)
    {
        this.ring = ring;
        this.logger = logger;
        HierarchicalSetting = logger.HierarchicalSetting;
    }

    public bool IsDebugEnabled => logger.IsDebugEnabled;

    public void OnLogEvent(FLogEvent evt)
    {
        throw new NotImplementedException("Should not be accessing through AsyncFLogger");
    }

    public void Debug(string fmt, params object?[] args)
    {
        PushFormat(FLogLevel.Debug, fmt, args);
    }

    public void Debug(string msg)
    {
        PushMessage(FLogLevel.Debug, msg);
    }

    public void Debug(object obj)
    {
        PushObject(FLogLevel.Debug, obj);
    }

    public bool IsInfoEnabled => logger.IsInfoEnabled;

    public void Info(string fmt, params object?[] args)
    {
        PushFormat(FLogLevel.Info, fmt, args);
    }

    public void Info(string msg)
    {
        PushMessage(FLogLevel.Info, msg);
    }

    public void Info(object obj)
    {
        PushObject(FLogLevel.Info, obj);
    }

    public bool IsWarnEnabled => logger.IsWarnEnabled;

    public void Warn(string fmt, params object?[] args)
    {
        PushFormat(FLogLevel.Warn, fmt, args);
    }

    public void Warn(string msg)
    {
        PushMessage(FLogLevel.Warn, msg);
    }

    public void Warn(object obj)
    {
        PushObject(FLogLevel.Warn, obj);
    }

    public bool IsErrorEnabled => logger.IsErrorEnabled;

    public void Error(string fmt, params object?[] args)
    {
        PushFormat(FLogLevel.Error, fmt, args);
    }

    public void Error(string msg)
    {
        PushMessage(FLogLevel.Error, msg);
    }

    public void Error(object obj)
    {
        PushObject(FLogLevel.Error, obj);
    }

    public string Name => logger.Name;

    public string FullNameOfLogger => Name;

    public Action<IHierarchicalLogger, string> SettingTranslation => logger.SettingTranslation;

    public string DefaultStringValue => "default";

    public bool Enabled { get; set; }
    public string HierarchicalSetting { get; set; }

    public bool DefaultEnabled
    {
        get => defaultEnabled;
        set
        {
            defaultEnabled = value;
            SettingTranslation(this, HierarchicalSetting);
        }
    }

    private void PushFormat(FLogLevel level, string fmt, object?[] args)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.LogTime = TimeContext.UtcNow;
        evt.Logger = logger;
        evt.Level = level;
        evt.MsgFormat = fmt;
        evt.MsgParams = args;
        ring.Publish(seqId);
    }

    private void PushMessage(FLogLevel level, string msg)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.LogTime = TimeContext.UtcNow;
        evt.Logger = logger;
        evt.Level = level;
        evt.MsgFormat = msg;
        ring.Publish(seqId);
    }

    private void PushObject(FLogLevel level, object obj)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.LogTime = TimeContext.UtcNow;
        evt.Logger = logger;
        evt.Level = level;
        evt.MsgObject = obj;
        ring.Publish(seqId);
    }

    private void PushException(FLogLevel level, string msg, Exception ex)
    {
        var seqId = ring.Claim();
        var evt = ring[seqId];
        evt.LogTime = TimeContext.UtcNow;
        evt.Logger = logger;
        evt.Level = level;
        evt.MsgFormat = msg;
        evt.Exception = ex;
        ring.Publish(seqId);
    }

    public void WarnException(string msg, Exception ex)
    {
        PushException(FLogLevel.Warn, msg, ex);
    }

    public void ErrorException(string msg, Exception ex)
    {
        PushException(FLogLevel.Error, msg, ex);
    }

    public override string? ToString() => logger.ToString();
}