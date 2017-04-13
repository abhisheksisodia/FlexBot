using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace FlexBot.DbHelper
{
    public class DatabaseHelper
    {
        //private SqlConnection connection;
        private string connectionString;
        public DatabaseHelper() {
            //connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\baptid3\\Source\\Repos\\FlexBot\\FlexBot\\FlexBot\\App_Data\\SkillsDatabase.mdf;Integrated Security=True";
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStringName"].ConnectionString;
        }

        public List<User> GetUserByLocationAndSkill(string location, string skills) {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.GetUsersBySkillAndLocation";

                SqlParameter skillParam = new SqlParameter();
                skillParam.ParameterName = "@skill";
                skillParam.Value = skills;
                command.Parameters.Add(skillParam);

                SqlParameter locationParam = new SqlParameter();
                locationParam.ParameterName = "@location";
                locationParam.Value = location;
                command.Parameters.Add(locationParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<User> result = new List<User>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readUserSkillsRow(dataReader));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex) {
                string message = ex.Message;
            }

            return null;
        }

        public List<User> GetUserByLocationAndProficiency(string location, string proficiency) {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.GetUsersByLocationAndProficiency";
                SqlParameter proficiencyParam = new SqlParameter();
                proficiencyParam.ParameterName = "@proficiency";
                proficiencyParam.Value = proficiency;
                command.Parameters.Add(proficiencyParam);

                SqlParameter locationParam = new SqlParameter();
                locationParam.ParameterName = "@location";
                locationParam.Value = location;
                command.Parameters.Add(locationParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<User> result = new List<User>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readUserSkillsRow(dataReader));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex) {
                String message = ex.Message;
            }

            return null;
        }

        public List<string> GetSkillsForUser(string firstName, string lastName)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.GetSkillsForUserByName";
                SqlParameter firstNameParam = new SqlParameter();
                firstNameParam.ParameterName = "@firstName";
                firstNameParam.Value = firstName;
                command.Parameters.Add(firstNameParam);

                SqlParameter lastNameParam = new SqlParameter();
                lastNameParam.ParameterName = "@lastName";
                lastNameParam.Value = lastName;
                command.Parameters.Add(lastNameParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<string> result = new List<string>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(dataReader.GetString(0));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                String message = ex.Message;
            }

            return null;
        }

        public int UpdateSkillForUser(string firstName, string lastName, string skillName, string skillLevel)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.updateUserSkillByName";
                SqlParameter firstNameParam = new SqlParameter();
                firstNameParam.ParameterName = "@firstName";
                firstNameParam.Value = firstName;
                command.Parameters.Add(firstNameParam);

                SqlParameter lastNameParam = new SqlParameter();
                lastNameParam.ParameterName = "@lastName";
                lastNameParam.Value = lastName;
                command.Parameters.Add(lastNameParam);

                SqlParameter skillNameParam = new SqlParameter();
                skillNameParam.ParameterName = "@skillName";
                skillNameParam.Value = skillName;
                command.Parameters.Add(skillNameParam);

                SqlParameter knowledgeLevelParam = new SqlParameter();
                knowledgeLevelParam.ParameterName = "@knowledgeLevel";
                knowledgeLevelParam.Value = skillLevel;
                command.Parameters.Add(knowledgeLevelParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                int result;

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
               
                
                    result = (dataReader.GetInt32(0));
               

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                String message = ex.Message;
            }

            return -4;
        }

        public List<User> GetUserBySkillAndProficiency(string skill, string proficiency)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.GetUsersBySkillAndProficiency";
                SqlParameter proficiencyParam = new SqlParameter();
                proficiencyParam.ParameterName = "@proficiency";
                proficiencyParam.Value = proficiency;
                command.Parameters.Add(proficiencyParam);

                SqlParameter skillParam = new SqlParameter();
                skillParam.ParameterName = "@skill";
                skillParam.Value = skill;
                command.Parameters.Add(skillParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<User> result = new List<User>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readUserSkillsRow(dataReader));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return null;
        }

        public List<User> GetAllUsers()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.ShowAllUsers";

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<User> result = new List<User>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read()) {
                    result.Add(readUserSkillsRow(dataReader));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex) {
                string message = ex.Message;
            }

            return null;
        }

        public List<User> GetUserBySkillProficiencyAndLocation(string skill, string proficiency, string location)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

                command.CommandText = "dbo.GetUsersBySkillLocationProficiency";

                SqlParameter proficiencyParam = new SqlParameter();
                proficiencyParam.ParameterName = "@proficiency";
                proficiencyParam.Value = proficiency;
                command.Parameters.Add(proficiencyParam);

                SqlParameter skillParam = new SqlParameter();
                skillParam.ParameterName = "@skill";
                skillParam.Value = skill;
                command.Parameters.Add(skillParam);

                SqlParameter locationParam = new SqlParameter();
                locationParam.ParameterName = "@location";
                locationParam.Value = location;
                command.Parameters.Add(locationParam);

                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                List<User> result = new List<User>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readUserSkillsRow(dataReader));
                }

                dataReader.Close();
                connection.Close();

                return result;
            }
            catch (Exception ex) {
                string message = ex.Message;
            }

            return null; 
        }


        private User readUserSkillsRow(IDataRecord rowData) {
            UserBuilder builder = new UserBuilder();

            builder.Id(rowData.GetInt32(0));
            builder.FirstName(rowData.GetString(1));
            builder.LastName(rowData.GetString(2));
            builder.HiringDate(rowData.GetDateTime(3));
            builder.Email(rowData.GetString(4));
            builder.PhoneNumber(rowData.GetString(5));
            builder.Skill(rowData.GetString(6));
            builder.Level(rowData.GetString(7));
            builder.Location(rowData.GetString(8));

            return builder.Build();
        }
    }
}