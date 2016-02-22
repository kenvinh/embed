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
        public static byte TotalQuestionSize = 200;
        public static byte ListeningQuestionSize = 100;
        public static byte ReadingQuestionSize = 100;
        public static byte ListeningQuestionStart = 1;
        public static byte ListeningQuestionEnd = 100;
        public static byte ReadingQuestionStart = 101;
        public static byte ReadingQuestionEnd = 200;

        public static byte MaxChoicePerAnswer = 4;


        public static Dictionary<string, byte> answer_map = new Dictionary<string, byte>()
        {
            {"a",1},
            {"b",2},
            {"c",3},
            {"d",4},
        };

        public static Dictionary<byte, SkillScore> score_translate = new Dictionary<byte, SkillScore>()
        {
            {0      , new SkillScore(   5     ,  5     ) },
            {1      , new SkillScore(   5     ,  5     ) },
            {2      , new SkillScore(   5     ,  5     ) },
            {3      , new SkillScore(   5     ,  5     ) },
            {4      , new SkillScore(   5     ,  5     ) },
            {5      , new SkillScore(   5     ,  5     ) },
            {6      , new SkillScore(   5     ,  5     ) },
            {7      , new SkillScore(   10    ,  5     ) },
            {8      , new SkillScore(   15    ,  5     ) },
            {9      , new SkillScore(   20    ,  5     ) },
            {10     , new SkillScore(   25    ,  10    ) },
            {11     , new SkillScore(   30    ,  15    ) },
            {12     , new SkillScore(   35    ,  20    ) },
            {13     , new SkillScore(   40    ,  25    ) },
            {14     , new SkillScore(   45    ,  30    ) },
            {15     , new SkillScore(   50    ,  35    ) },
            {16     , new SkillScore(   55    ,  40    ) },
            {17     , new SkillScore(   60    ,  45    ) },
            {18     , new SkillScore(   65    ,  50    ) },
            {19     , new SkillScore(   70    ,  55    ) },
            {20     , new SkillScore(   75    ,  60    ) },
            {21     , new SkillScore(   80    ,  65    ) },
            {22     , new SkillScore(   85    ,  70    ) },
            {23     , new SkillScore(   90    ,  75    ) },
            {24     , new SkillScore(   95    ,  80    ) },
            {25     , new SkillScore(   100   ,  90    ) },
            {26     , new SkillScore(   105   ,  95    ) },
            {27     , new SkillScore(   110   ,  100   ) },
            {28     , new SkillScore(   115   ,  110   ) },
            {29     , new SkillScore(   120   ,  115   ) },
            {30     , new SkillScore(   125   ,  120   ) },
            {31     , new SkillScore(   135   ,  125   ) },
            {32     , new SkillScore(   140   ,  130   ) },
            {33     , new SkillScore(   145   ,  135   ) },
            {34     , new SkillScore(   150   ,  140   ) },
            {35     , new SkillScore(   155   ,  145   ) },
            {36     , new SkillScore(   160   ,  150   ) },
            {37     , new SkillScore(   165   ,  155   ) },
            {38     , new SkillScore(   170   ,  160   ) },
            {39     , new SkillScore(   180   ,  170   ) },
            {40     , new SkillScore(   185   ,  175   ) },
            {41     , new SkillScore(   190   ,  180   ) },
            {42     , new SkillScore(   195   ,  185   ) },
            {43     , new SkillScore(   200   ,  195   ) },
            {44     , new SkillScore(   210   ,  200   ) },
            {45     , new SkillScore(   220   ,  205   ) },
            {46     , new SkillScore(   225   ,  210   ) },
            {47     , new SkillScore(   230   ,  220   ) },
            {48     , new SkillScore(   235   ,  225   ) },
            {49     , new SkillScore(   240   ,  230   ) },
            {50     , new SkillScore(   245   ,  235   ) },
            {51     , new SkillScore(   250   ,  240   ) },
            {52     , new SkillScore(   255   ,  250   ) },
            {53     , new SkillScore(   260   ,  255   ) },
            {54     , new SkillScore(   270   ,  260   ) },
            {55     , new SkillScore(   275   ,  270   ) },
            {56     , new SkillScore(   280   ,  275   ) },
            {57     , new SkillScore(   285   ,  280   ) },
            {58     , new SkillScore(   295   ,  285   ) },
            {59     , new SkillScore(   300   ,  290   ) },
            {60     , new SkillScore(   305   ,  295   ) },
            {61     , new SkillScore(   310   ,  300   ) },
            {62     , new SkillScore(   315   ,  305   ) },
            {13     , new SkillScore(   320   ,  310   ) },
            {64     , new SkillScore(   325   ,  320   ) },
            {65     , new SkillScore(   330   ,  325   ) },
            {66     , new SkillScore(   335   ,  330   ) },
            {67     , new SkillScore(   340   ,  335   ) },
            {68     , new SkillScore(   345   ,  340   ) },
            {69     , new SkillScore(   350   ,  345   ) },
            {70     , new SkillScore(   360   ,  350   ) },
            {71     , new SkillScore(   365   ,  355   ) },
            {72     , new SkillScore(   370   ,  360   ) },
            {73     , new SkillScore(   375   ,  365   ) },
            {74     , new SkillScore(   380   ,  370   ) },
            {75     , new SkillScore(   390   ,  375   ) },
            {76     , new SkillScore(   395   ,  380   ) },
            {77     , new SkillScore(   400   ,  385   ) },
            {78     , new SkillScore(   405   ,  390   ) },
            {79     , new SkillScore(   410   ,  395   ) },
            {80     , new SkillScore(   420   ,  400   ) },
            {81     , new SkillScore(   425   ,  405   ) },
            {82     , new SkillScore(   430   ,  405   ) },
            {83     , new SkillScore(   435   ,  410   ) },
            {84     , new SkillScore(   440   ,  415   ) },
            {85     , new SkillScore(   450   ,  420   ) },
            {86     , new SkillScore(   455   ,  425   ) },
            {87     , new SkillScore(   460   ,  430   ) },
            {88     , new SkillScore(   470   ,  435   ) },
            {89     , new SkillScore(   475   ,  445   ) },
            {90     , new SkillScore(   480   ,  450   ) },
            {91     , new SkillScore(   485   ,  455   ) },
            {92     , new SkillScore(   490   ,  465   ) },
            {93     , new SkillScore(   495   ,  470   ) },
            {94     , new SkillScore(   495   ,  480   ) },
            {95     , new SkillScore(   495   ,  485   ) },
            {96     , new SkillScore(   495   ,  490   ) },
            {97     , new SkillScore(   495   ,  495   ) },
            {98     , new SkillScore(   495   ,  495   ) },
            {99     , new SkillScore(   495   ,  495   ) },
            {100    , new SkillScore(   495   ,  495   ) },
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

        public static string ByteToAnswer(byte ans)
        {
            foreach(KeyValuePair<string,byte> pair in answer_map)
            {
                if (pair.Value == ans)
                {
                    return pair.Key;
                }
            }
            return "";
        }

        public static byte CheckQuestionList(List<input_question> qlist)
        {
            byte ans;
            byte index = 1;
            /* Check total number of question */
            if (qlist.Count != TotalQuestionSize)
            {
                throw new ArgumentException("Question list must be " + TotalQuestionSize.ToString());
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

    public struct SkillScore
    {
        public int ListeningScore;
        public int ReadingScore;

        public SkillScore(int list_score, int read_score)
        {
            ListeningScore = list_score;
            ReadingScore = read_score;
        }
    }
        
}
