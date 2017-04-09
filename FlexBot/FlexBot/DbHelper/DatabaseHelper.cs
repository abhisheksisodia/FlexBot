using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace FlexBot.DbHelper
{
    public class DatabaseHelper
    {
        private SqlConnection connection;
        public DatabaseHelper() {
            //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\baptid3\\Source\\Repos\\FlexBot\\FlexBot\\FlexBot\\App_Data\\SkillsDatabase.mdf;Integrated Security=True";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStringName"].ConnectionString;

            connection = new SqlConnection(connectionString);
        }

        public List<UserSkillsView> GetUserByLocationAndSkill(string location, string skills) {
            
            SqlCommand command = new SqlCommand();
            SqlDataReader dataReader;

            command.CommandText = "dbo.GetUsersBySkillAndLocation";
            command.Parameters["skill"].Value = skills;
            command.Parameters["location"].Value = location;

            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;
            List<UserSkillsView> result = new List<UserSkillsView>();

            connection.Open();
            dataReader = command.ExecuteReader();
           
            // Parse dateReader
            while (dataReader.Read()) { 

                result.Add(readRow(dataReader));
            }

            dataReader.Close();
            connection.Close();

            return result;
        }

        public List<UserSkillsView> GetUserByLocationAndProficiency(string location, string proficiency) {
            SqlCommand command = new SqlCommand();
            SqlDataReader dataReader;

            command.CommandText = "dbo.GetUsersBySkillAndProficiency";
            command.Parameters["proficiency"].Value = proficiency;
            command.Parameters["location"].Value = location;

            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;
            List<UserSkillsView> result = new List<UserSkillsView>();

            connection.Open();
            dataReader = command.ExecuteReader();

            // Parse dateReader
            while (dataReader.Read())
            {
                result.Add(readRow(dataReader));
            }

            dataReader.Close();
            connection.Close();

            return result;
        }

        public List<UserSkillsView> GetUserBySkillAndProficiency(string skill, string proficiency)
        {
            SqlCommand command = new SqlCommand();
            SqlDataReader dataReader;

            command.CommandText = "dbo.GetUsersBySkillAndProficiency";
            command.Parameters["proficiency"].Value = proficiency;
            command.Parameters["skill"].Value = skill;

            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;
            List<UserSkillsView> result = new List<UserSkillsView>();

            connection.Open();
            dataReader = command.ExecuteReader();

            // Parse dateReader
            while (dataReader.Read())
            {
                result.Add(readRow(dataReader));
            }

            dataReader.Close();
            connection.Close();

            return result;
        }

        public List<UserSkillsView> GetUserBySkillProficiencyAndLocation(string skill, string proficiency, string location)
        {
            SqlCommand command = new SqlCommand();
            SqlDataReader dataReader;

            command.CommandText = "dbo.GetUsersBySkillLocationProficiency";
            command.Parameters["proficiency"].Value = proficiency;
            command.Parameters["skill"].Value = skill;
            command.Parameters["location"].Value = location;

            command.CommandType = CommandType.StoredProcedure;
            command.Connection = connection;
            List<UserSkillsView> result = new List<UserSkillsView>();

            connection.Open();
            dataReader = command.ExecuteReader();

            // Parse dateReader
            while (dataReader.Read())
            {
                result.Add(readRow(dataReader));
            }

            dataReader.Close();
            connection.Close();

            return result;
        }


        private UserSkillsView readRow(IDataRecord rowData) {
            UserSkillsViewBuilder builder = new UserSkillsViewBuilder();

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