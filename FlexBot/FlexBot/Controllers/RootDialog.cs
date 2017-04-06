using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FlexBot.Controllers
{
    [LuisModel("ea3fe896-d375-48ba-9e98-05aefbc94b86", "0800c3fb16b54b238266d8473e500b7b")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        private const string EntitySkillName = "Skill";

        private const string EntityLocationName = "Location";

        private const string EntityLevelName = "Level";

        protected string skill { get; set; }
        protected string knowledgeLevel { get; set; }
        protected string location { get; set; }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hi, I am Skylnet. I am an employee skills expert. Try asking me: 'Find me people who know Java' or 'Update skills of Anthony'");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("FindEmployees")]
        public async Task FindEmployees(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Okay, let me take a look...");

            var entities = new List<EntityRecommendation>(result.Entities);

            foreach (var entity in entities)
            {
                if (entity.Type.Equals(EntitySkillName))
                {
                    skill = entity.Entity.ToString();
                }

                if (entity.Type.Equals(EntityLocationName))
                {
                    location = entity.Entity.ToString();
                }

                if (entity.Type.Equals(EntityLevelName))
                {
                    knowledgeLevel = entity.Entity.ToString();
                }
            }

            if (skill != null && location != null && knowledgeLevel != null)
            {
                await SearchEmployees(context);
            }
            else if (skill != null && location == null && knowledgeLevel == null)
            {
                await context.PostAsync($"Okay, what knowledge level are you interested in?");
                context.Wait(MessageReceivedLevelAskForLocation); // State transition: wait for user to provide skill
            }
            else if (location != null && skill == null && knowledgeLevel == null)
            {
                await context.PostAsync($"Okay, which skill are you interested in?");
                context.Wait(MessageReceivedSkillAskForLevel); // State transition: wait for user to provide skill
            }
            else if (skill != null && location != null && knowledgeLevel == null)
            {
                await context.PostAsync($"Okay, what knowledge level are you interested in?");
                context.Wait(MessageReceivedKnowledgeLevel);
            }
            else if (skill != null && knowledgeLevel != null && location == null)
            {
                await context.PostAsync($"Okay, what location are you interested in?");
                context.Wait(MessageReceivedLocation);
            }
            else
            {
                await context.PostAsync($"Sorry, I am unable to understand. Try asking me: 'Find me people who know Java' ");
            }

        }

        public async Task SearchEmployees(IDialogContext context)
        {
            await context.PostAsync($"Okay, looking for people who know {skill} with knowledge level {knowledgeLevel} and located in {location}");
            //perform search and give results
        }

        public async Task MessageReceivedSkillAskForLevel(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.skill = (await argument).Text;
            await context.PostAsync("What knowledge level are you interested in?");
            context.Wait(MessageReceivedKnowledgeLevel); // State transition: wait for user to provide knowledge level
        }

        public async Task MessageReceivedLevelAskForLocation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.knowledgeLevel = (await argument).Text;
            await context.PostAsync("Which location are you interested in?");
            context.Wait(MessageReceivedLocation); // State transition: wait for user to provide cover location
        }

        public async Task MessageReceivedLocation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.location = (await argument).Text;
            // do your search/aggregation here
            await SearchEmployees(context);
        }

        public async Task MessageReceivedKnowledgeLevel(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.knowledgeLevel = (await argument).Text;
            // do your search/aggregation here
            await SearchEmployees(context);
        }
    }
}