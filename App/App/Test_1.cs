using System;
using System.Windows.Forms;

namespace App
{
    public partial class Test_1 : Form
    {
        public Test_1()
        {
            InitializeComponent();
        }

        private void Test_1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            int ball = 2;
            //считаем баллы
            if (radioButton2.Checked)
                count++;
            if (radioButton8.Checked)
                count++;
            if (radioButton9.Checked)
                count++;
            if (radioButton15.Checked)
                count++;
            if (radioButton20.Checked)
                count++;
            if (radioButton24.Checked)
                count++;
            //выставляем оценку
            if (count == 6 || count == 5)
                ball = 5;
            if (count == 4 || count == 3)
                ball = 4;
            MessageBox.Show($"Результат выполнения: {count} из 6.\r\nОценка {ball}", "Результат", MessageBoxButtons.OK);
            //обновляем запись в таблице. Укажем номер текста ручками, а оценку и студента автоматически.
            var db = new DB();
            db.queryExecute($"update информация set оценка={ball} where [ID ТЕСТА]=1 and [id студента]={PersonalData.IdStudentOrTeacher}");
            //чистим форму
            panel2.Dispose();
            panel1.Dispose();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void test1_Click(object sender, EventArgs e)
        {

        }
    }
}
