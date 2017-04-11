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
            // Yes or No
            //IMessageActivity replyToConversation = context.MakeMessage();
            //replyToConversation.Text = "Please pick a choice";
            //replyToConversation.Type = "message";
            //replyToConversation.Attachments = new List<Attachment>();
            //List<CardImage> cardImages = new List<CardImage>();
            ////cardImages.Add(new CardImage(url: "https://pbs.twimg.com/profile_images/791067045991358464/yy_F__YU.jpg"));
            ////cardImages.Add(new CardImage(url: "https://pbs.twimg.com/profile_images/791067045991358464/yy_F__YU.jpg"));
            //List<CardAction> cardButtons = new List<CardAction>();
            //CardAction yesButton = new CardAction()
            //{
            //    Value = "Yes",
            //    Type = "postBack",
            //    Title = "Yes"
            //};
            //CardAction noButton = new CardAction()
            //{
            //    Value = "No",
            //    Type = "postBack",
            //    Title = "No"
            //};
            //cardButtons.Add(yesButton);
            //cardButtons.Add(noButton);

            //HeroCard plCard = new HeroCard()
            //{
            //    Title = "Yes or No?",
            //    Subtitle = "Please select one",
            //    Images = cardImages,
            //    Buttons = cardButtons
            //};
            //Attachment plAttachment = plCard.ToAttachment();
            //replyToConversation.Attachments.Add(plAttachment);
            //await context.PostAsync(replyToConversation);




            // Location based (Uncomment to try and comment above)
            IMessageActivity replyToConversation = context.MakeMessage();
            replyToConversation.Text = "Please pick a choice";
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.seetorontonow.com/wp-content/uploads/2017/01/toronto-skyline-with-fireworks.jpg"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction torontoButton = new CardAction()
            {
                Value = "Toronto",
                Type = "postBack",
                Title = "Toronto"
            };
            CardAction krakowButton = new CardAction()
            {
                Value = "Krakow",
                Type = "postBack",
                Title = "Krakow"
            };
            cardButtons.Add(torontoButton);
            cardButtons.Add(krakowButton);

            HeroCard plCard = new HeroCard()
            {
                Title = "Pick Location",
                Subtitle = "Select as many as you like",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();

            replyToConversation.Attachments.Add(plAttachment);
            await context.PostAsync(replyToConversation);

            //await context.PostAsync($"Hi, I am Skylnet. I am an employee skills expert. Try asking me: 'Find me people who know Java' or 'Update skills of Anthony'");

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

        [LuisIntent("ChangeRequest")]
        public async Task ChangeRequest(IDialogContext context, LuisResult result)
        {
            skill = null;
            location = null;
            knowledgeLevel = null;
            await context.PostAsync($"All right what do you want me to do now?");
            context.Done<object>(new object());
        }

        [LuisIntent("ChangeLocation")]
        public async Task ChangeLocation(IDialogContext context, LuisResult result)
        {
            location = null;
            var entities = new List<EntityRecommendation>(result.Entities);
            foreach (var entity in entities)
            {
                if (entity.Type.Equals(EntityLocationName))
                {
                    location = entity.Entity;
                    await SearchEmployees(context);
                    return;
                }
            }

            await context.PostAsync($"OK, what is the new location?");
            context.Done<object>(new object());
        }

        [LuisIntent("ChangeSkill")]
        public async Task ChangeSkill(IDialogContext context, LuisResult result)
        {
            skill = null;
            var entities = new List<EntityRecommendation>(result.Entities);
            foreach (var entity in entities)
            {
                if (entity.Type.Equals(EntitySkillName))
                {
                    skill = entity.Entity;
                    await SearchEmployees(context);
                    return;
                }
            }

            await context.PostAsync($"OK, what is the new skill?");
            context.Done<object>(new object());
        }

        [LuisIntent("ChangeKnowledgeLevel")]
        public async Task ChangeKnowledgeLevel(IDialogContext context, LuisResult result)
        {
            knowledgeLevel = null;
            var entities = new List<EntityRecommendation>(result.Entities);
            foreach (var entity in entities)
            {
                if (entity.Type.Equals(EntityLevelName))
                {
                    knowledgeLevel = entity.Entity;
                    await SearchEmployees(context);
                    return;
                }
            }

            await context.PostAsync($"OK, what is the new location?");
            context.Done<object>(new object());
        }

        public async Task SearchEmployees(IDialogContext context)
        {
            await context.PostAsync($"Okay, looking for people who know {skill} with knowledge level {knowledgeLevel} and located in {location}");
            //perform search and give results
            context.Done<object>(new object());
        }
    }
}