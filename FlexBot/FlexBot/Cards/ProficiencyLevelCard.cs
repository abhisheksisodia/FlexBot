using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FlexBot.Cards
{
    public class ProficiencyLevelCard
    {
        private const string InterestedLevel = "Interested";
        private const string FoundationsLevel = "Foundations";
        private const string IntermediateLevel = "Intermediate";
        private const string AdvancedLevel = "Advanced";
        private const string ExpertLevel = "Expert";
        private const string NoneLevel = "None";

        private IEnumerable<string> options = new List<string> { InterestedLevel, FoundationsLevel, IntermediateLevel, AdvancedLevel, ExpertLevel, NoneLevel };

        public async Task ShowOptions(IDialogContext context)
        {
            IMessageActivity replyToConversation = context.MakeMessage();
            replyToConversation.Text = "Okay, please select a proficiency level you are interested in. Select None if you don't want to include level";
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction interestedButton = new CardAction()
            {
                Value = InterestedLevel,
                Type = "postBack",
                Title = InterestedLevel
            };
            CardAction foundationsButton = new CardAction()
            {
                Value = FoundationsLevel,
                Type = "postBack",
                Title = FoundationsLevel
            };
            CardAction intermediateButton = new CardAction()
            {
                Value = IntermediateLevel,
                Type = "postBack",
                Title = IntermediateLevel
            }; CardAction advancedButton = new CardAction()
            {
                Value = AdvancedLevel,
                Type = "postBack",
                Title = AdvancedLevel
            };
            CardAction expertButton = new CardAction()
            {
                Value = ExpertLevel,
                Type = "postBack",
                Title = ExpertLevel
            };
            cardButtons.Add(interestedButton);
            cardButtons.Add(foundationsButton);
            cardButtons.Add(intermediateButton);
            cardButtons.Add(advancedButton);
            cardButtons.Add(expertButton);

            HeroCard plCard = new HeroCard()
            {
                Title = null,
                Subtitle = null,
                Images = null,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();

            replyToConversation.Attachments.Add(plAttachment);
            await context.PostAsync(replyToConversation);
            context.Done<object>(new object());
        }
    }
}