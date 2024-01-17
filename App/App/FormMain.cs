using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace App
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public void FormMain_Load(object sender, EventArgs e)
        {
            //грузим данные во все combobox
            SetDataComboBox();
            //Сделаем стартовый индекс у комбобокса на форме поиска студентов
            comboBox3.SelectionStart = 0;
            checkRole(PersonalData.Role);
            //установим доступность кнопок в зависимости от пройденого теста
            isPassed();
        }
        private void SetDataComboBox()
        {

            var db = new DB();
            //студенты для изменения данных для ЛК
            var StudentForUpdate = "select с.[id студента] from Студенты с inner join Пользователи п on с.[id пользователя]=п.[id пользователя]";
            comboBox2.Items.Clear();
            db.loadElementToComboBox(StudentForUpdate, "id студента", comboBox2);

            //для добавление новой учетки студенту. Грузим только студентов без учеток.
            comboBox1.Items.Clear();
            var queryAddNewUser = "select * from студенты where [id пользователя] is null";
            db.loadElementToComboBox(queryAddNewUser, "id студента", comboBox1);

        }
        private void isPassed()
        {
            var db = new DB();
            using (SqlConnection connection = new SqlConnection(db.stringCon()))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"select [ID Теста] from информация where [ID Студента] = {PersonalData.IdStudentOrTeacher} and Оценка = 0", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<int> testIds = new List<int>();
                        //если есть записи, а они должны быть. (Под каждого студента и тест. Иначе записей не будет и кнопки будут недоступны.)
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ID Теста")))
                            {
                                //записываем в лист
                                int testId = Convert.ToInt32(reader["ID Теста"]);
                                testIds.Add(testId);
                            }
                        }
                        //делаем кнопки доступными
                        if (testIds.Contains(1))
                        {
                            btn1.Enabled = true;
                        }

                        if (testIds.Contains(2))
                        {
                            btn2.Enabled = true;
                        }
                    }
                }
            }
        }
        private string checkRole(string role)
        {
            switch (role.ToUpper())
            {
                case ("ПРЕПОДАВАТЕЛЬ"):
                    tabPage1.Dispose();
                    MessageBox.Show($"Добро пожаловать", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return role;
                case ("УЧЕНИК"):
                    tabPage3.Dispose();
                    tabPage4.Dispose();
                    MessageBox.Show($"Добро пожаловать", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return role;
                default:
                    tabControl1.Dispose();
                    MessageBox.Show("Учетная запись некорректна.\rОбратитесь в тех. поддержку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return role;
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            var db = new DB();
            //убедимся, что студент еще не проходил первый тест
            if (db.GetSinglValues($"select top 1 оценка from информация where [id теста]=1 and оценка<>0 and [id студента]={PersonalData.IdStudentOrTeacher}", "оценка") != "")
            {
                btn1.Enabled = false;
                MessageBox.Show($"Вы уже прошли тест", "Уведомление", MessageBoxButtons.OK);
            }
            else
            {
                //на всякий случай очистим панель для форм
                acriveForm = null;
                openForm(new Test_1());
            }
        }
        private Form acriveForm = null;
        private void openForm(Form childForm)
        {
            //Для открытия формы с тестом в панеле
            if (acriveForm != null)
                acriveForm.Close();
            acriveForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            var db = new DB();
            //убедимся, что студент еще не проходил первый тест. Укажем номер теста ручками, оценку и id студента  - автоматически.
            if (db.GetSinglValues($"select top 1 оценка from информация where [id теста]=2 and оценка<>0 and [id студента]={PersonalData.IdStudentOrTeacher}", "оценка") != "")
            {
                btn2.Enabled = false;
                MessageBox.Show($"Вы уже прошли тест", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //на всякий случай очистим панель для форм
                acriveForm = null;
                openForm(new Test_2());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text) || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Пароль не указан или студент не выбран.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var db = new DB();
            var query = $"update п " +
                $" set Пароль='{textBox3.Text}' " +
                $" from Пользователи п " +
                $" inner join Студенты с on п.[ID Пользователя] = с.[ID Пользователя] " +
                $" where [id студента]={comboBox2.SelectedItem}";
            //если запрос выполнился и не вернулось null
            if (db.queryExecute(query) != null)
            {
                SetDataComboBox();
                MessageBox.Show("Пароль успешно обновлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //проверка на пустоту и тд
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var db = new DB();
            var queryInsert = "insert into Пользователи (Логин, пароль, роль) " +
                             $"values('{textBox1.Text}','{textBox2.Text}','Ученик')";
            //если запрос выполнился и не вернулось null
            if (db.queryExecute(queryInsert) != null)
            {
                var queryUpdate = "update Студенты set [id пользователя]=(" +
                                   " select top 1 [id Пользователя] from пользователи order by [id пользователя] desc" +
                                   " ) " +
                                   $" where [id студента]={comboBox1.SelectedItem} ";
                //если запрос выполнился и не вернулось null
                if (db.queryExecute(queryUpdate) != null)
                {
                    SetDataComboBox();
                    MessageBox.Show("Новый пользователь успешно зарегистрирован.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var db = new DB();
            //проверим что данные для поиска не пустые, если пустые, то выведем все из таблицы
            if (comboBox3.SelectedIndex == -1 || string.IsNullOrEmpty(textBox5.Text))
            {
                var queryAllSeatch = $"select * from Студенты";
                //если запрос выполнился и не вернул null
                if (db.queryReturnData(queryAllSeatch, dataGridView1) != null)
                {
                    MessageBox.Show("Паратры для поиска не заполнены.\r\nПолучена общая информация о всех студентах.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            var querySearch = $"select * from Студенты where [{comboBox3.SelectedItem}]={textBox5.Text}";
            //если запрос выполнился и не вернул null
            if (db.queryReturnData(querySearch, dataGridView1) != null)
            {
                MessageBox.Show("Поиск успешно выполнен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
