using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexBot.DbHelper
{
    public class UserBuilder
    {
        private User user;

        public UserBuilder() {
            user = new User();
        }

        public UserBuilder Id(int id) {
            user.Id = id;
            return this;
        }

        public UserBuilder FirstName(string firstName) {
            user.FirstName = firstName;
            return this;
        }

        public UserBuilder LastName(string lastName) {
            user.LastName = lastName;
            return this;
        }

        public UserBuilder HiringDate(DateTime hiringDate) {
            user.HiringDate = hiringDate;
            return this;
        }

        public UserBuilder Skill(string skill) {
            user.Skill = skill;
            return this;
        }

        public UserBuilder PhoneNumber(String phoneNumber) {
            user.Phone = phoneNumber;
            return this;
        }

        public UserBuilder Level(String level) {
            user.Level = level;
            return this;
        }

        public UserBuilder Email(string email) {
            user.Email = email;
            return this;
        }

        public UserBuilder Location(string location) {
            user.Location = location;
            return this;
        }

        public User Build() {
            return user;
        }

        public void Clear() {
            user = new User();
        }
    }
}