using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileHelpers;
using Toeic_score;

namespace Toeic_score
{
    public partial class Form1 : Form
    {
        string input_path;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toeic test = new toeic();

            try
            {
                var engine = new FileHelperEngine<input_question>();

                var result = engine.ReadFile(input_path);

                test.question_list = result.ToList<input_question>();

                byte check = toeic.CheckQuestionList(test.question_list);

                MessageBox.Show("Test file OK", "File check done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                /* Check complete, now  display the answer form dynamically */
                Form ans_form = new Form();
                ans_form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                ans_form.AutoScaleDimensions = new SizeF(400, 400);
                ans_form.Name = "form_ans";
                ans_form.Text = "Answer Form";



                ans_form.Show();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "File reading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.InitialDirectory = Application.StartupPath;
            fd.Filter = "CSV Files (*.csv)|*.csv";
            fd.ShowDialog();

            input_path = fd.FileName;

            textBox1.Text = input_path;
        }
    }
}
