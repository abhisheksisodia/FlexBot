using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace FlexBot.Slack
{
    public class SlackChannelData
    {
        [JsonProperty("attachments")]
        public List<SlackAttachment> Attachment
        {
            get; set; }
    }

    public class SlackAttachment
    {
        [JsonProperty("fallback")]
        public String Fallback { get; set; }
        [JsonProperty("color")]
        public String Color { get; set; }
        [JsonProperty("pretext")]
        public String Pretext { get; set; }
        [JsonProperty("author_name")]
        public String AuthorName { get; set; }
        [JsonProperty("author_link")]
        public String AuthorLink { get; set; }
        [JsonProperty("author_icon")]
        public String AuthorIcon { get; set; }
        [JsonProperty("title")]
        public String Title { get; set; }
        [JsonProperty("title_link")]
        public String TitleLink { get; set; }
        [JsonProperty("text")]
        public String Text { get; set; }
        [JsonProperty("fields")]
        public List<SlackField> Fields { get; set; }
        [JsonProperty("image_url")]
        public String ImageUrl { get; set; }
        [JsonProperty("thumb_url")]
        public String ThumbUrl { get; set; }
        [JsonProperty("footer")]
        public String Footer { get; set; }
        [JsonProperty("footer_icon")]
        public String FooterIcon { get; set; }
        [JsonProperty("ts")]
        public String Ts { get; set; }
        [JsonProperty("actions")]
        public List<SlackAction> Actions { get; set; }
    }

    public class SlackField
    {
        [JsonProperty("title")]
        public String Title { get; set; }
        [JsonProperty("value")]
        public String Value { get; set; }
        [JsonProperty("short")]
        public bool Short { get; set; }
    }

    public class SlackAction
    {

        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("text")]
        public String Text { get; set; }
        [JsonProperty("type")]
        public String Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}