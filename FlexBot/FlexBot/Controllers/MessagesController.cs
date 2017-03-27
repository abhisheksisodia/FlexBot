using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace FlexBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {
                string replyString;
                LuisResponse StLUIS = await GetEntityFromLUIS(activity.Text);
                if (StLUIS.intents.Count() > 0)
                {
                    switch (StLUIS.intents[0].intent)
                    {
                        case "PeopleList":
                            replyString = "Here is a list of people: Abhishek, Filip, Daniel, Anthony, Camilo";
                            break;
                        case "Intro":
                            replyString = "I am FlexBot. I can help you find people within your organization based on skill set, knowledge level and more";
                            break;
                        case "None":
                            replyString = "Please ask something meaningful. For instance, you can ask me about to give a list of people who are skilled in a particular technology";
                            break;
                        default:
                            replyString = "Sorry, I am unable to understand...";
                            break;
                    }
                }
                else
                {
                    replyString = "Sorry, I am not getting you...";
                }

                Activity reply = activity.CreateReply(replyString);
                // return our reply to the user
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                string replyMessage = string.Empty;
                replyMessage += $"Hi there\n\n";
                replyMessage += $"I am FlexBot. Designed to help you find people within your company.  \n";
                replyMessage += $"I can help you find people based on skill set, knowledge level, location and more!  \n";
                Activity reply = activity.CreateReply(replyMessage);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private static async Task<LuisResponse> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            LuisResponse Data = new LuisResponse();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/eb1f40b0-c8da-4f20-9264-c8d175117dea?subscription-key=9bee6378df734716b7e2c3f346b30219&timezoneOffset=0.0&verbose=true&q=" + Query;
                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<LuisResponse>(JsonDataResponse);
                }
            }
            return Data;
        }
    }
}