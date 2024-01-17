using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class Test_2 : Form
    {
        public Test_2()
        {
            InitializeComponent();
        }

        private void Test_2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            int ball = 2;
            //считаем баллы
            if (radioButton4.Checked)
                count++;
            if (radioButton5.Checked)
                count++;
            if (radioButton9.Checked)
                count++;
            if (radioButton13.Checked)
                count++;
            if (radioButton17.Checked)
                count++;
            if (radioButton21.Checked)
                count++;
            //выставляем оценку
            if (count == 6 || count == 5)
                ball = 5;
            if (count == 4 || count == 3)
                ball = 4;
            MessageBox.Show($"Результат выполнения: {count} из 6.\r\nОценка {ball}", "Результат", MessageBoxButtons.OK);
            //обновляем запись в таблице
            var db = new DB();
            //указываем студента и оценку автоматически,а номер теста ручками, 
            db.queryExecute($"update информация set оценка={ball} where [ID ТЕСТА]=2 and [id студента]={PersonalData.IdStudentOrTeacher}");
            //чистим форму
            panel2.Dispose();
            panel1.Dispose();
        }
    }
}
