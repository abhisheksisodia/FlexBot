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

        private const string EntityLevelName = "ProficiencyLevel";

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
                context.Done<object>(new object());
            }
            else if (skill != null && location != null && knowledgeLevel == null)
            {
                await context.PostAsync($"Okay, are you interested in any knowledge level?");
                context.Done<object>(new object());
                // display list of levels using formflow maybe plus none then wait user feedback
                // get user selection and update knowledge level
                // query with selected criteria
            }
            else if (skill != null && knowledgeLevel != null && location == null)
            {
                await context.PostAsync($"Okay, are you interested in any location?");
                context.Done<object>(new object());
                // include luis
                // query with selected criteria
            }
            else if (location != null && knowledgeLevel != null && skill == null)
            {
                await context.PostAsync($"Okay, which skill are you interested in?");
                context.Done<object>(new object());
                // include luis
                // query with selected criteria
            }
            else
            {
                if (skill == null)
                {
                    await context.PostAsync($"Okay, are you interested in any skill?");
                    context.Done<object>(new object());
                }
                else if (knowledgeLevel == null)
                {
                    await context.PostAsync($"Okay, are you interested in any knowledge level?");
                    context.Done<object>(new object());
                }
                else if (location == null)
                {
                    await context.PostAsync($"Okay, are you interested in any location?");
                    context.Done<object>(new object());
                }
                else
                {
                    await context.PostAsync($"Sorry, I did not understand. Try asking me: 'Find me people who know Java'");
                    skill = null;
                    location = null;
                    knowledgeLevel = null;
                }
            }

        }

        public async Task SearchEmployees(IDialogContext context)
        {
            await context.PostAsync($"Okay, looking for people who know {skill} with knowledge level {knowledgeLevel} and located in {location}");
            //perform search and give results
            context.Done<object>(new object());
        }
    }
}