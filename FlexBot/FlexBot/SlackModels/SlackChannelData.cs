using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using FlexBot.Models;

namespace FlexBot.Slack
{
    public class SlackChannelData
    {
        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachment
        {
            get; set;
        }
    }
}