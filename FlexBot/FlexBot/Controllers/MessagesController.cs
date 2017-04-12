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
using FlexBot.Models;
using Microsoft.Bot.Builder.FormFlow;
using System.Diagnostics;

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
            if (activity != null)
            {
                // one of these will have an interface and process it
                switch (activity.GetActivityType())
                {
                    case ActivityTypes.Message:
                        {
                            await Conversation.SendAsync(activity, MakeRoot);
                            break;
                        }
                    case ActivityTypes.ConversationUpdate:
                    case ActivityTypes.ContactRelationUpdate:
                    case ActivityTypes.Typing:
                    case ActivityTypes.DeleteUserData:
                    default:
                        Trace.TraceError($"Activity Error: {activity.GetActivityType()}");
                        break;
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private static IForm<FindEmployeeModel> BuildFindEmployeeForm()
        {
            var builder = new FormBuilder<FindEmployeeModel>();

            return builder
                .Field(nameof(FindEmployeeModel.skill))
                .Field(nameof(FindEmployeeModel.skillLevel))
                .Field(nameof(FindEmployeeModel.location))
                .AddRemainingFields()
                .Build();
        }

        internal static IDialog<FindEmployeeModel> MakeRoot()
        {
            return Chain.From(() => new RootDialog(BuildFindEmployeeForm));
        }


        //temp method to be removed later
        private async Task sendReply(Activity activity, String replyString)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply = activity.CreateReply(replyString);
            // return our reply to the user
            await connector.Conversations.ReplyToActivityAsync(reply);
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