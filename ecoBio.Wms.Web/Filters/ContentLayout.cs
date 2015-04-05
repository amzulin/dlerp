using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Enterprise.Invoicing.Web
{
    public class ContentLayout : log4net.Layout.PatternLayout
    {
        public ContentLayout()
        {
            this.AddConverter("Contents", typeof(ContentsPatternConvert));
            this.AddConverter("UserId", typeof(UserPatternConvert));

        }
    }

    public class ContentsPatternConvert : log4net.Layout.Pattern.PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            LogContent logMessage = loggingEvent.MessageObject as LogContent;
            if (logMessage != null)
            {
                writer.Write(logMessage.Contents);
            }
        }
    }

    public class UserPatternConvert : log4net.Layout.Pattern.PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            LogContent logMessage = loggingEvent.MessageObject as LogContent;
            if (logMessage != null)
            {
                writer.Write(logMessage.UserId);
            }
        }
    }

}