using FlexBot.DbHelper;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexBot.Cards
{
    public class PersonDetailCard
    {
        public Attachment GetThumbnailCard()
        {
            var heroCard = new ThumbnailCard
            {
                Title = "Abhishek Sisodia",
                Subtitle = "Consultant, Located in Toronto",
                Text = "Java, C#, Swift, Hololens, Android, iOS",
                Images = new List<CardImage> { new CardImage("http://vignette2.wikia.nocookie.net/jamesbond/images/d/dc/James_Bond_%28Pierce_Brosnan%29_-_Profile.jpg/revision/latest?cb=20130506224906") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Contact", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }

        public Attachment GetPeopleDetailsCard(UserSkillsView user)
        {
            var heroCard = new ThumbnailCard
            {
                Title = $"{user.FirstName}, {user.LastName}",
                Subtitle = $"Consultant, Located in {user.Location}",
                Text = $"Java, C#, Swift, Hololens, Android, iOS \n\n  Email: {user.Email}",
                Images = new List<CardImage> { new CardImage("http://vignette2.wikia.nocookie.net/jamesbond/images/d/dc/James_Bond_%28Pierce_Brosnan%29_-_Profile.jpg/revision/latest?cb=20130506224906") },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Contact", value: "https://docs.botframework.com/en-us/") }
            };

            return heroCard.ToAttachment();
        }
    }
}