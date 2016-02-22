using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace Toeic_score
{
    class user_test
    {
    }

    public class AnsInfo
    {
        public byte question_num;
        public byte max_choice;
        public byte correct_choice;
        public byte user_choice;
    }

    [DelimitedRecord(",")]
    public class TestInfo
    {
        public DateTime TimeTaken;
        public DateTime TimeFinish;
        public int Duration;
        public string TestName;
        public int ScoreTotal;
        public int ScoreListening;
        public int ScoreReading;
        public int CorrectAnsListening;
        public int CorrectAnsReading;

        [FieldConverter(typeof(Converter_AnsRecord))]
        public List<AnsInfo> AnsDetail;

        public TestInfo()
        {
            AnsDetail = new List<AnsInfo>();
        }

        public void CountCorrectAnswer()
        {
            CorrectAnsListening = 0;
            CorrectAnsReading = 0;

            for (int i=0; i< AnsDetail.Count; i++)
            {
                if (AnsDetail[i].user_choice == AnsDetail[i].correct_choice)
                {
                    if ((byte)(i+1) < toeic.ListeningQuestionEnd)
                    {
                        CorrectAnsListening++;
                    }
                    else
                    {
                        CorrectAnsReading++;
                    }
                }
            }
        }

        public static List<AnsInfo> ParseAnsInfo(string input)
        {
            /* Each answer size: 3 + 2 + 2 + 2 = 9 
             * Total answer: 100 x 9 = 900
             */
            if (input.Length != 900)
            {
                throw new ArgumentException("AnsInfo Length must be 900");
            }

            List<AnsInfo> result = new List<AnsInfo>();
            int index = 0;
            while (index < input.Length)
            {
                string item = input.Substring((int)index, 9);

                string str_NumOfQuestion = item.Substring(0, 3);
                string str_MaxChoice = item.Substring(3, 2);
                string str_CorrectChoice = item.Substring(5, 2);
                string str_UserChoice = item.Substring(7, 2);

                index += 9;

                try
                {
                    AnsInfo ans_item = new AnsInfo();
                    ans_item.question_num = byte.Parse(str_NumOfQuestion);
                    ans_item.max_choice = byte.Parse(str_MaxChoice);
                    ans_item.correct_choice = byte.Parse(str_CorrectChoice);
                    ans_item.user_choice = byte.Parse(str_UserChoice);
                    result.Add(ans_item);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Cannot parse AnsInfo in user file, string:" + input);
                }
            }

            /* Finish, return the list */
            return result;
        }

    }

    public class UserInfo
    {
        public string Name;
        public int ID;
        public List<TestInfo> Tests;
        public string Hash;

        public UserInfo()
        {
            Tests = new List<TestInfo>();
            Hash = "";
        }
    }

  
}
