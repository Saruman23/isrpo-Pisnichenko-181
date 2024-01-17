using System.Data.SqlClient;

namespace App
{
    public class PersonalData
    {
        public static int IdStudentOrTeacher { get; private set; }
        public static string Role { get; private set; }

        public bool SetPersonalData(string login, string password)
        {
            var db = new DB();

            string sqlExpression = "SELECT " +
                "CASE WHEN пр.[ID Преподавателя] IS NOT NULL THEN пр.[ID Преподавателя] ELSE с.[ID Студента] END AS [ID Студента или преподавателя], " +
                "  Роль " +
                " FROM Пользователи п " +
                " LEFT JOIN Студенты с ON с.[ID Пользователя] = п.[ID Пользователя]" +
                " LEFT JOIN Преподаватели пр ON п.[ID Пользователя] = пр.[ID Пользователя] " +
                " WHERE п.логин=@Login AND п.Пароль=@Password ";

            using (SqlConnection connection = new SqlConnection(db.stringCon()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            Role = reader["Роль"].ToString();
                            IdStudentOrTeacher = (int)reader["ID Студента или преподавателя"];
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
    }
}
