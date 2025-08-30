// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public interface IStringAppenderCollectionBuilder : IKeyedCollectionContinuation<IFLogStringAppender>;

public class StringAppenderCollectionBuilder : KeyedCollectionContinuation<IFLogStringAppender, FLogStringAppender>, IStringAppenderCollectionBuilder;

public class FinalStringAppenderCollectionBuilder : FinalAppenderCollectionBuilder<IFLogStringAppender, FLogStringAppender>;
