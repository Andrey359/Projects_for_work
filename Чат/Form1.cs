using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Чат
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            if (name.Length < 1)
            {
                MessageBox.Show("Не указано имя");
                return;
            }
            int port = (int)numericUpDown1.Value;
            Form2 form = new Form2(true, port, name);
            form.Show();
            //Close();
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            if (name.Length < 1)
            {
                MessageBox.Show("Не указано имя");
                return;
            }
            int port = (int)numericUpDown1.Value;
            Form2 form = new Form2(false, port, name);
            form.Show();
            //Close();
        }
    }
}
