using System;

namespace FlexBot.DbHelper
{
    public class UserSkillsView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skill { get; set; }
        public string Level { get; set; }
        public string Location { get; set; }
    }
}