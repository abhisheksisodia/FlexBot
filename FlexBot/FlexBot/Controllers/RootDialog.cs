using FlexBot.Cards;
using FlexBot.DbHelper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FlexBot.Slack;

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

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            string message = $"To find people try asking me: Find me people who are expert in java and are located in Toronto \n\n";
            message += $"You can also just provide me with one search criteria \n\n";
            message += $"I can also help you with updating skills of people. Try asking me: Update skills for Anthony \n\n";

            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("ShowDetails")]
        public async Task ShowDetails(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();

            PersonDetailCard pCard = new PersonDetailCard();
            var attachment = pCard.GetThumbnailCard();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(attachment);

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
                ProficiencyLevelCard proficiencySelectorCard = new ProficiencyLevelCard();
                await proficiencySelectorCard.ShowOptions(context);
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
                    ProficiencyLevelCard proficiencySelectorCard = new ProficiencyLevelCard();
                    await proficiencySelectorCard.ShowOptions(context);
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

        [LuisIntent("UpdateEmployee")]
        public async Task UpdateEmployee(IDialogContext context, LuisResult result)
        {
            var entities = new List<EntityRecommendation>(result.Entities);
            var dbHelper = new DatabaseHelper();
            string firstName = "";
            string lastName = "";
            string skillToUpdate = "";
            string levelToUpdate = "";
            foreach (var entitity in entities)
            {
                if (entitity.Type == "Person::FirstName")
                {
                    firstName = entitity.Entity;
                }
                if (entitity.Type == "Person::LastName")
                {
                    lastName = entitity.Entity;
                }
                if (entitity.Type == EntitySkillName)
                {
                    skillToUpdate = entitity.Entity;
                }
                if (entitity.Type == EntityLevelName)
                {
                    levelToUpdate = entitity.Entity;
                }
            }

            dbHelper.UpdateSkillForUser(firstName, lastName, skillToUpdate, levelToUpdate);
            context.PostAsync($"Updated {firstName} {lastName}'s {skillToUpdate} skill to {levelToUpdate}");
        }

        [LuisIntent("ChangeRequest")]
        public async Task ChangeRequest(IDialogContext context, LuisResult result)
        {
            skill = null;
            location = null;
            knowledgeLevel = null;
            await context.PostAsync($"Alright, what would you like to do now?");
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
                    await context.PostAsync($"Location changed to: {location}");
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
                    await context.PostAsync($"Skill changed to: {skill}");
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
                    await context.PostAsync($"Knowledge level changed to: {knowledgeLevel}");
                    await SearchEmployees(context);
                    return;
                }
            }

            await context.PostAsync($"OK, what is the new location?");
            context.Done<object>(new object());
        }

        public async Task SearchEmployees(IDialogContext context)
        {

            //perform search and give results
            DatabaseHelper dbHelper = new DatabaseHelper();
            if (skill != null && knowledgeLevel != null && location != null)
            {
                await context.PostAsync($"Okay, looking for people who know {skill} with knowledge level {knowledgeLevel} and located in {location}");
                List<UserSkillsView> results = dbHelper.GetUserBySkillProficiencyAndLocation(skill, knowledgeLevel, location);
                foreach (var user in results)
                {
                    var message = context.MakeMessage();

                    PersonDetailCard pCard = new PersonDetailCard();
                    var skills = dbHelper.GetSkillsForUser(user.FirstName, user.LastName);
                    var attachment = pCard.GetPeopleDetailsCard(user, skills);
                    message.Attachments = new List<Attachment>();
                    message.Attachments.Add(attachment);

                    await context.PostAsync(message);
                }

                if (results.Count == 0)
                {
                    await context.PostAsync("Sorry, I wasn't able to find what you were looking for.");
                }
            }

            context.Done<object>(new object());
        }
    }
}