using FlexBot.Cards;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FlexBot.Models;

using System;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using FlexBot.DbHelper;

namespace FlexBot.Controllers
{
    [LuisModel("ea3fe896-d375-48ba-9e98-05aefbc94b86", "0800c3fb16b54b238266d8473e500b7b")]
    [Serializable]
    public class RootDialog : LuisDialog<FindEmployeeModel>
    {
        private readonly BuildFormDelegate<FindEmployeeModel> findEmployeeForm;

        internal RootDialog(BuildFormDelegate<FindEmployeeModel> findEmployeeForm)
        {
            this.findEmployeeForm = findEmployeeForm;
        }

        [LuisIntent("FindEmployees")]
        public async Task FindEmployees(IDialogContext context, LuisResult result)
        {
            var entities = new List<EntityRecommendation>(result.Entities);

            // TODO: Use pre-populated values
            var findForm = new FormDialog<FindEmployeeModel>(new FindEmployeeModel(), findEmployeeForm, FormOptions.PromptInStart, entities);
            context.Call(findForm, FindEmployeeFormComplete);
        }

        private async Task FindEmployeeFormComplete(IDialogContext context, IAwaitable<FindEmployeeModel> result)
        {
            FindEmployeeModel form = null;
            try
            {
                form = await result;
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form!");
                return;
            }

            // SUCCESS
            if (form != null)
            {
                // TODO give them what they want here
                DatabaseHelper db = new DatabaseHelper();
                // TODO map enum locations to strings or refactor database to match enums
               // String results = db.GetUserBySkillProficiencyAndLocation("java","any","toronto").ToString();
                await context.PostAsync("Your Request: " + form.ToString() + "/n  and your results: ");
            }
            else
            {
                await context.PostAsync("Form returned empty response!");
            }

            context.Wait(MessageReceived);
        }

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



        //[LuisIntent("ChangeRequest")]
        //public async Task ChangeRequest(IDialogContext context, LuisResult result)
        //{
        //    skill = null;
        //    location = null;
        //    knowledgeLevel = null;
        //    await context.PostAsync($"All right what do you want me to do now?");
        //    context.Done<object>(new object());
        //}

        //[LuisIntent("ChangeLocation")]
        //public async Task ChangeLocation(IDialogContext context, LuisResult result)
        //{
        //    location = null;
        //    var entities = new List<EntityRecommendation>(result.Entities);
        //    foreach (var entity in entities)
        //    {
        //        if (entity.Type.Equals(EntityLocationName))
        //        {
        //            location = entity.Entity;
        //            await SearchEmployees(context);
        //            return;
        //        }
        //    }

        //    await context.PostAsync($"OK, what is the new location?");
        //    context.Done<object>(new object());
        //}

        //[LuisIntent("ChangeSkill")]
        //public async Task ChangeSkill(IDialogContext context, LuisResult result)
        //{
        //    skill = null;
        //    var entities = new List<EntityRecommendation>(result.Entities);
        //    foreach (var entity in entities)
        //    {
        //        if (entity.Type.Equals(EntitySkillName))
        //        {
        //            skill = entity.Entity;
        //            await SearchEmployees(context);
        //            return;
        //        }
        //    }

        //    await context.PostAsync($"OK, what is the new skill?");
        //    context.Done<object>(new object());
        //}

        //[LuisIntent("ChangeKnowledgeLevel")]
        //public async Task ChangeKnowledgeLevel(IDialogContext context, LuisResult result)
        //{
        //    knowledgeLevel = null;
        //    var entities = new List<EntityRecommendation>(result.Entities);
        //    foreach (var entity in entities)
        //    {
        //        if (entity.Type.Equals(EntityLevelName))
        //        {
        //            knowledgeLevel = entity.Entity;
        //            await SearchEmployees(context);
        //            return;
        //        }
        //    }

        //    await context.PostAsync($"OK, what is the new location?");
        //    context.Done<object>(new object());
        //}

        //public async Task SearchEmployees(IDialogContext context)
        //{
        //    await context.PostAsync($"Okay, looking for people who know {skill} with knowledge level {knowledgeLevel} and located in {location}");
        //    //perform search and give results
        //    context.Done<object>(new object());
        //}
    }
}