using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace Toeic_score
{
#if false
    public class question_info
    {
        public byte number_of_choice;
        public byte correct_choice;
    }

    public class part_info
    {
        public List<question_info> question_list;
    }

    public class test_info
    {
        public List<part_info> part_list;
    }

    [DelimitedRecord(",")]
    public class toeic
    {
        public test_info listening_part;
        public test_info reading_part;
        
        public byte GetNumOfListeningPart()
        {
            return (byte)listening_part.part_list.Count;
        }

        public byte GetNumOfReadingPart()
        {
            return (byte)reading_part.part_list.Count;
        }

        public byte GetNumOfQuestion(List<question_info> qlist)
        {
            return (byte)qlist.Count;
        }

        public byte CheckData()
        {
            byte total_question = 0;

            /* Check listening part */
            foreach (part_info part in listening_part.part_list)
            {
                total_question = (byte)(total_question + (byte)part.question_list.Count);
            }

            if (total_question != 100)
            {
                return 1;
            }

            /* Check reading part */
            total_question = 0;

            foreach (part_info part in reading_part.part_list)
            {
                total_question = (byte)(total_question + (byte)part.question_list.Count);
            }

            if (total_question != 100)
            {
                return 2;
            }

            return 0;
        }

    }

#endif

    [DelimitedRecord(",")]
    public class input_question
    {
        public byte question_num;
        public byte max_choice;
        public string correct_choice;
    }

    public class toeic
    {
        public static List<byte> ParsedCorrectChoice = new List<byte>();
        public static List<byte> ParsedUserChoice = new List<byte>();

        public static Dictionary<string, byte> answer_map = new Dictionary<string, byte>()
        {
            {"a",1},
            {"b",2},
            {"c",3},
            {"d",4},
        };

        public List<input_question> question_list;

        public static byte AnswerToByte(string ans)
        {
            byte value;
            if (answer_map.TryGetValue(ans, out value) == true)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }


        public static byte CheckQuestionList(List<input_question> qlist)
        {
            byte ans;
            byte index = 1;
            /* Check total number of question */
            if (qlist.Count != 100)
            {
                throw new ArgumentException("Question list must be 100");
            }

            /* Check max choice. <= 4 */
            /* Check max choice and correct choice. 0 < Correct choice <= max choice */
            foreach (input_question q in qlist)
            {
                /* Trim leading and trailing space */
                q.correct_choice = q.correct_choice.Trim();

                if (q.correct_choice == "")
                {
                    throw new ArgumentException("Empty answer, at line" + index.ToString());
                }

                ans = AnswerToByte(q.correct_choice);
                if (ans == 0)
                {
                    throw new ArgumentException("Answer is: a,b,c or d only, at line" + index.ToString());                    
                }


                if (q.max_choice > 5)
                {
                    throw new ArgumentException("Max choice must <= 4, at line" + index.ToString());
                }

                if (ans > q.max_choice)
                {
                    throw new ArgumentException("Answer must <= max choice, at line" + index.ToString());
                }

                ParsedCorrectChoice.Add(ans);

                index++;
            }

            /* No error found */
            return 0;
        }
    }

    
        
}
