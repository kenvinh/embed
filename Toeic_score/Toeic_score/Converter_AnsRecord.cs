using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace Toeic_score
{
    public class Converter_AnsRecord : ConverterBase
    {
        public override object StringToField(string from)
        {
            /* Parse the string:  */
            List<AnsInfo> ans_list = new List<AnsInfo>();
            
            if (from.Length != 0)
            {
                ans_list = TestInfo.ParseAnsInfo(from);
            }
            
            return ans_list;
        }

        public override string FieldToString(object from)
        {
            List<AnsInfo> ans_list = (List<AnsInfo>)from;

            string output = "";

            if (ans_list.Count != 0)
            {

                foreach (AnsInfo item in ans_list)
                {
                    string str_item = "";
                    str_item = str_item + item.question_num.ToString("000");
                    str_item = str_item + item.max_choice.ToString("00");
                    str_item = str_item + item.correct_choice.ToString("00");
                    str_item = str_item + item.user_choice.ToString("00");

                    output = output + str_item;
                }

            }

            return output;
        }

    }
}
