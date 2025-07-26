using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IBufferFlushAppenderAsyncClient : IAppenderAsyncClient
{
    int AppenderBufferFlushQueueNum { get; set; }


}