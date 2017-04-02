using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FlexBot.Controllers
{
    [Serializable]
    public class FindEmployeesDialog : IDialog<object>
    {
        protected string skill { get; set; }
        protected string knowledgeLevel { get; set; }
        protected string location { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStartConversation);
        }

        public async Task MessageReceivedStartConversation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("Okay. What skill are you looking for?");
            context.Wait(MessageReceivedSkill); // State transition: wait for user to provide skill
        }

        public async Task MessageReceivedSkill(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.skill = (await argument).Text;
            await context.PostAsync("What knowledge level are you interested in?");
            context.Wait(MessageReceivedKnowledgeLevel); // State transition: wait for user to provide knowledge level
        }

        public async Task MessageReceivedKnowledgeLevel(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.knowledgeLevel = (await argument).Text;
            await context.PostAsync("Which location are you interested in?");
            context.Wait(MessageReceivedLocation); // State transition: wait for user to provide cover location
        }

        public async Task MessageReceivedLocation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.location = (await argument).Text;
            // do your search/aggregation here
            await context.PostAsync($"OK, I found these users who know {skill} with knowledge level {knowledgeLevel} and located in {location} ...");
            context.Done<object>(new object()); // Signal completion
        }
    }
}