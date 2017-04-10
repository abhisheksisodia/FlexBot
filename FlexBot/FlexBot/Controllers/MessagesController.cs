using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using FlexBot.Controllers;

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
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                string introMessage = string.Empty;
                introMessage += $"Hi there\n\n";
                introMessage += $"I am SkylNet. I am an employee skills expert.  \n";
                introMessage += $"I can help you find people based on skills, knowledge level, location and more!  \n";
                introMessage += $"I can also help you to update employee skills!  \n";
                introMessage += $"What would you like to do today? Find employees or manage their skills?   \n";
                Activity reply = activity.CreateReply(introMessage);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        //temp method to be removed later
        private async Task sendReply(Activity activity, String replyString)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply = activity.CreateReply(replyString);
            // return our reply to the user
            await connector.Conversations.ReplyToActivityAsync(reply);
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
                string RequestURI = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/ea3fe896-d375-48ba-9e98-05aefbc94b86?subscription-key=0800c3fb16b54b238266d8473e500b7b&verbose=true&timezoneOffset=0.0&q=" + Query;
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