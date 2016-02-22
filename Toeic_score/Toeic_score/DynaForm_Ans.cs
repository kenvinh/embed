using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toeic_score
{
    public class DynaForm_Ans
    {
        private Form ans_form = new Form();
        private List<input_question> qulist;

        private List<Label> lbl_list = new List<Label>();
        private List<RdbArray> rdb_list = new List<RdbArray>();

        public void CreateNewAnsForm()
        {
            int i;
            ans_form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ans_form.AutoScaleDimensions = new SizeF(400, 400);
            ans_form.Name = "form_ans";
            ans_form.Text = "Answer Form";
            ans_form.AutoScroll = true;
            ans_form.Height = Screen.FromControl(ans_form).Bounds.Height;
            ans_form.Width = 900;
            ans_form.Location = new Point((Screen.FromControl(ans_form).Bounds.Width - ans_form.Width) / 2, 0);

            ans_form.AutoSize = false;


            int initial_x = 25;
            int initial_y = 25;
            int line_height = 35;

            for (i = 0; i < qulist.Count; i++)
            {
                /* Add a rectangle to make color */
                Panel pn = new Panel();

                pn.Location = new Point(initial_x, initial_y + (line_height * i));
                pn.Size = new Size(toeic.MaxChoicePerAnswer * 150 + 200, line_height);
                if ((i % 2) == 0)
                {
                    pn.BackColor = Color.AliceBlue;
                }
                else
                {
                    pn.BackColor = Color.ForestGreen;
                }

                pn.Name = "pnl_Q" + i.ToString();
                ans_form.Controls.Add(pn);

                /* Add label */
                Label lbl = new Label();
                lbl.Location = new Point(5, 10);
                lbl.Name = "lbl_Question" + (i + 1).ToString("000");
                lbl.Text = "Question " + (i + 1).ToString("000");
                lbl_list.Add(lbl);
                pn.Controls.Add(lbl);
                
                /* Add radio button */
                RdbArray rdb_array = new RdbArray();
                rdb_list.Add(rdb_array);

                PlaceRadioButtonArray(  (byte)(i),
                                        qulist[i].max_choice,
                                        new Point(200, initial_y), 
                                        ref rdb_list[i].array,
                                        ref pn);
            }

            Button btn = new Button();
            btn.Name = "btn_Submit";
            btn.Text = "Submit";
            btn.Size = new Size(80, 40);
            btn.Location = new Point(400, qulist.Count*line_height + 60);
            btn.Click += new System.EventHandler(this.btn_SubmitClick);

            ans_form.Controls.Add(btn);

            ans_form.ShowDialog();
        }


        private void btn_SubmitClick(object sender, EventArgs e)
        {
            DialogResult ans;
            ans = MessageBox.Show("You have done your test?", "Test done?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ans == DialogResult.Yes)
            {
                this.ans_form.Close();
            }
        }

        public static void CreateRecordForm(List<AnsInfo> anslist)
        {

        }

        public byte CheckUserAnswer(byte question_num)
        {
            if (question_num == 199)
            {
                question_num = 199;
            }

            for (int i=0; i<toeic.MaxChoicePerAnswer; i++)
            {
                if (rdb_list[question_num].array[i].Checked == true)
                {
                    return (byte)(i + 1);
                }
            }
            return 0;
        }

        private static void PlaceRadioButtonArray(  byte quindex,
                                                    byte qty,
                                                    Point form_pos,
                                                    ref RadioButton[] rdbarray,
                                                    ref Panel panel_obj)
        {
            int i;

            
            for (i=0; i< qty; i++)
            {
                rdbarray[i].Location = new Point(200 + (150 * i), 5);
                rdbarray[i].Text = toeic.ByteToAnswer((byte)(i + 1)).ToUpper();
                rdbarray[i].Name = "rdb_Q" + quindex.ToString() + "_" + (i + 1).ToString();
                panel_obj.Controls.Add(rdbarray[i]);
            }

            for (i= qty; i< toeic.MaxChoicePerAnswer; i++)
            {
                rdbarray[i].Dispose();
            }


        }

        public class RdbArray
        {
            public RadioButton[] array = new RadioButton[toeic.MaxChoicePerAnswer];

            public RdbArray()
            {
                for (int i=0; i < array.Length; i++)
                {
                    array[i] = new RadioButton();
                }
            }
        }

        public List<input_question> QuestionList
        {
            get { return qulist; }
            set { qulist = value; }
        }

    }
}
