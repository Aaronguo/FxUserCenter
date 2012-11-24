using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FxUserCenter
{
    public class AboutModel
    {
        public int UserCount { get; set; }

        public int UserTodayCount { get; set; }

        public int InfoPublishAllCount { get; set; }

        public int InfoPublishTodayCount { get; set; }

        public int InfoEndCount { get; set; }

        public int PrivateMessageCount { get; set; }

        public int PrivateMessageTodayCount { get; set; }
    }
}