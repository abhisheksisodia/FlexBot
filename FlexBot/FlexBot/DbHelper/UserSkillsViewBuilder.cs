using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexBot.DbHelper
{
    public class UserSkillsViewBuilder
    {
        private UserSkillsView userSkillsView;

        public UserSkillsViewBuilder() {
            userSkillsView = new UserSkillsView();
        }

        public UserSkillsViewBuilder Id(int id) {
            userSkillsView.Id = id;
            return this;
        }

        public UserSkillsViewBuilder FirstName(string firstName) {
            userSkillsView.FirstName = firstName;
            return this;
        }

        public UserSkillsViewBuilder LastName(string lastName) {
            userSkillsView.LastName = lastName;
            return this;
        }

        public UserSkillsViewBuilder HiringDate(DateTime hiringDate) {
            userSkillsView.HiringDate = hiringDate;
            return this;
        }

        public UserSkillsViewBuilder Skill(string skill) {
            userSkillsView.Skill = skill;
            return this;
        }

        public UserSkillsViewBuilder PhoneNumber(String phoneNumber) {
            userSkillsView.Phone = phoneNumber;
            return this;
        }

        public UserSkillsViewBuilder Level(String level) {
            userSkillsView.Level = level;
            return this;
        }

        public UserSkillsViewBuilder Email(string email) {
            userSkillsView.Email = email;
            return this;
        }

        public UserSkillsViewBuilder Location(string location) {
            userSkillsView.Location = location;
            return this;
        }

        public UserSkillsView Build() {
            return userSkillsView;
        }

        public void Clear() {
            userSkillsView = new UserSkillsView();
        }
    }
}