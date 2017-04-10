using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace FlexBot.DbHelper
{
    public class DatabaseHelper
    {
        //private SqlConnection connection;
        private string connectionString;
        public DatabaseHelper() {
            //string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\baptid3\\Source\\Repos\\FlexBot\\FlexBot\\FlexBot\\App_Data\\SkillsDatabase.mdf;Integrated Security=True";
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStringName"].ConnectionString;

            //connection = new SqlConnection(connectionString);
        }

        public List<UserSkillsView> GetUserByLocationAndSkill(string location, string skills) {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

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
                List<UserSkillsView> result = new List<UserSkillsView>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {

                    result.Add(readRow(dataReader));
                }

                //dataReader.Close();
                //connection.Close();

                return result;
            }
        }

        public List<UserSkillsView> GetUserByLocationAndProficiency(string location, string proficiency) {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

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
                List<UserSkillsView> result = new List<UserSkillsView>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readRow(dataReader));
                }

                //dataReader.Close();
                //connection.Close();

                return result;
            }
        }

        public List<UserSkillsView> GetUserBySkillAndProficiency(string skill, string proficiency)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                SqlDataReader dataReader;

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
                List<UserSkillsView> result = new List<UserSkillsView>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readRow(dataReader));
                }

                //dataReader.Close();
                //connection.Close();

                return result;
            }
        }

        public List<UserSkillsView> GetUserBySkillProficiencyAndLocation(string skill, string proficiency, string location)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
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
                List<UserSkillsView> result = new List<UserSkillsView>();

                connection.Open();
                dataReader = command.ExecuteReader();

                // Parse dateReader
                while (dataReader.Read())
                {
                    result.Add(readRow(dataReader));
                }

                return result;
            }
                

            //dataReader.Close();
            //connection.Close();

            
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