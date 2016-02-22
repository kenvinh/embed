using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FileHelpers;
using Toeic_score;

namespace Toeic_score
{
    public partial class Form1 : Form
    {
        string input_path = @"D:\WORK\Visual Studio 2013\Projects\Toeic_score\Toeic_score\bin\Debug\test_repo\test_input.csv";

        private List<string> cmb_UsersList_Binding_FileListName = new List<string>();
        private List<string> cmb_UsersList_Binding_FileListDir = new List<string>();
        private string cmb_UsersList_Binding_Selected_FileDir;
        private string cmb_UsersList_Binding_Selected_FileName;

        public Form1()
        {
            InitializeComponent();

            CheckUserSelection();

            txb_TestSelect.Text = input_path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toeic test = new toeic();
            TestInfo test_info = new TestInfo();

            try
            {
                var engine = new FileHelperEngine<input_question>();

                var result = engine.ReadFile(input_path);

                test.question_list = result.ToList<input_question>();

                byte check = toeic.CheckQuestionList(test.question_list);

                /* Check complete, now  display the answer form dynamically*/
                /* Mark start time */
                test_info.TimeTaken = DateTime.Now;
                DynaForm_Ans ans_form_class = new DynaForm_Ans();
                ans_form_class.QuestionList = test.question_list;
                ans_form_class.CreateNewAnsForm();
                
                /* User has finished answer question, now map the score  */
                /* Mark stop time */
                test_info.TimeFinish = DateTime.Now;
                
                for (int i=0; i< test.question_list.Count; i++)
                {
                    /* Mapping between test input data and test result data */
                    AnsInfo ans = new AnsInfo();

                    ans.question_num = test.question_list[i].question_num;
                    ans.max_choice = test.question_list[i].max_choice;
                    ans.correct_choice = toeic.AnswerToByte(test.question_list[i].correct_choice);
                    ans.user_choice = ans_form_class.CheckUserAnswer((byte)i);

                    test_info.AnsDetail.Add(ans);
                }

                /* Mapping finished, now count correct answer */
                test_info.CountCorrectAnswer();

                /* Marking the score */

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

            txb_TestSelect.Text = input_path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form_AddUser form_add = new Form_AddUser();
            form_add.ShowDialog();
        }

        private void cmb_UsersList_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void cmb_UsersList_DropDown(object sender, EventArgs e)
        {
            try
            {
                cmb_UsersList_Binding_FileListName = SystemProperties.GetFolderFilesName(SystemProperties.UserFolderName);
                cmb_UsersList_Binding_FileListDir = SystemProperties.GetFolderFilesDir(SystemProperties.UserFolderName);

                cmb_UsersList.DataSource = cmb_UsersList_Binding_FileListName;
                
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message, "Folder reading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            this.Activate();//this.button1.Focus();
            button1.Focus();
        }

        private void cmb_UsersList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            cmb_UsersList_Binding_Selected_FileDir = cmb_UsersList_Binding_FileListDir[cb.SelectedIndex];
            cmb_UsersList_Binding_Selected_FileName = cmb_UsersList_Binding_FileListName[cb.SelectedIndex];
            
            var engine = new FileHelperEngine<TestInfo>();

            var result = engine.ReadFile(cmb_UsersList_Binding_Selected_FileDir);

            List<TestInfo> info = new List<TestInfo>();

            info = result.ToList<TestInfo>();

            string user_hello = "Who are you? " + cmb_UsersList_Binding_Selected_FileName + "\n\r";
            user_hello = user_hello + "You tried " + info.Count + " times";
            lbl_UserHello.Text = user_hello;
            CheckUserSelection();


        }

        private void CheckUserSelection()
        {
            if (cmb_UsersList.SelectedItem == null)
            {
                lbl_Status.Text = "Please select a user";
                txb_TestSelect.Enabled = false;
                btn_BrowseTestItem.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                lbl_Status.Text = "";
                txb_TestSelect.Enabled = true;
                btn_BrowseTestItem.Enabled = true;
                button1.Enabled = true;
            }
        }
    }
}
