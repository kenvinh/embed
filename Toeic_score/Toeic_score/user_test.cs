using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public AnsInfo AnsDetail;
    }

    public class UserInfo
    {
        public string Name;
        public int ID;
        public List<TestInfo> Tests;
        public string Hash;
    }
}
