using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexBot.Models
{

    public enum SkillLevel
    {
        UKNOWN,
        NONE,
        INTERESTED,
        BEGINNER,
        INTERMEDIATE,
        ADVANCED,
        ANY,
    }

    public enum Location
    {
        UNKNOWN,
        TORONTO,
        KRAKOW,
        NYC,
        RALEIGH,
        LONDON,
        WROCLAW,
        ANY,
    }

    public enum Skill
    {
        UNKNOWN,
        CSHARP,
        JAVA,
        ANY,
    }

    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "I do not understand \"{0}\".", "Try again, I don't get \"{0}\".")]
    [Template(TemplateUsage.EnumSelectOne, "What {&} are you looking for? {||}")]
    public class FindEmployeeModel
    {
        public SkillLevel skillLevel;
        public Location location;
        public Skill skill;

        public static IForm<FindEmployeeModel> BuildForm()
        {
            OnCompletionAsyncDelegate<FindEmployeeModel> processOrder = async (context, state) =>
            {
                await context.PostAsync("Let me get what you asked for");
            };

            return new FormBuilder<FindEmployeeModel>()
                    .Message("Let's Find You Employees!")
                    .Build();
        }

        public override string ToString()
        {
            return "Find employees with skill: " + skill + " at skill level: " + skillLevel + " at location: " + location;
        }
    }
}