using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TEST.Exercise.Application.ExerciseType;
using TEST.Exercise.Domain.Entities;

namespace TEST.Management.Helper
{
    /// <summary>
    /// 上传题库帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 通过上传的文件获得试题
        /// </summary>
        /// <param name="fileName">上传的文件路径</param>
        /// <param name="_exerciseTypeService">后端服务接口</param>
        /// <param name="type">试题类型（单选/多选/判断）</param>
        /// <returns></returns>
        public static List<Question> GetQuestionsByFile(string fileName, long questionTypeId=0)//IExerciseTypeService _exerciseTypeService,string type
        {
            List<Question> questions = new List<Question>();
            string content = System.IO.File.ReadAllText(fileName, Encoding.GetEncoding("gb2312"));
            string[] typeList = content.Split(new string[] { "#####################" }, StringSplitOptions.RemoveEmptyEntries);
            string questionsType = "单选";
            for (int i = 0; i < typeList.Length; i++)
            {
                if (i == 0)
                {
                    questionsType = typeList[i].Trim();
                    continue;
                }

                string typeTitle = Regex.Match(typeList[i], @"[\s\S]*?(?=\d+\．[\s\S]*?答案[：|:])").Value;
                MatchCollection matchCollection = Regex.Matches(typeList[i], @"\d+\．[\s\S]*?答案[：|:][\s\S]*?(?=\d+\．[\s\S]*?答案[：|:][\s\S]*?|$)");
                foreach (Match item in matchCollection)
                {
                    Question question = new Question();
                    question.QuestionTypeId = questionTypeId;//_exerciseTypeService.GetIdByType(questionsType).Data;//题目类型
                    question.QuestionTypeName = typeTitle;
                    question.Content = Regex.Match(item.Value, @"[\s\S]*?(?=A)").Value.Trim();
                    question.Content = Regex.Replace(question.Content, @"^\d+[．|.|、]", string.Empty);
                    string option = Regex.Match(item.Value, @"A[\s\S]*?(?=答案)").Value.Trim();
                    question.Options = GetJsonOption(option).Trim();
                    question.Answer = Regex.Match(item.Value, @"(?<=答案\s*\S\s*)[\s\S]*?(?=解析)").Value.Trim();
                    question.AnswerNote = Regex.Match(item.Value, @"解析[\s\S]*").Value.Trim();
                    if (question.AnswerNote.Contains("命题单位"))
                    {
                        question.AnswerNote = Regex.Match(question.AnswerNote, @"[\s\S]*?(?=命题单位)").Value.Trim();
                    }

                    questions.Add(question);
                }
            }
            return questions;
        }
        /// <summary>
        /// 如果是选择题，对选项进行Json处理
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        private static string GetJsonOption(string option)
        {
            option = Regex.Replace(option, @"([A-Z])[．|.|、|]", "\",\"$1\":\"");
            option = option.Replace("\r\n", string.Empty);
            if (option.StartsWith("\","))
            {
                option = new Regex("\",").Replace(option, string.Empty, 1);
            }
            option = Regex.Replace(option, "\\s*\"", "\"").Trim();
            option = "{" + option + "\"}";
            return option;
        }

        /// <summary>
        /// 对不符合格式的内容进行处理
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string HandleContent(string content)
        {
            MatchCollection itemList = Regex.Matches(content, @"解析[\s\S]*?(?=命题单位：)", RegexOptions.IgnoreCase);
            foreach (Match item in itemList)
            {
                string jiexiContent = item.Value;
                jiexiContent = Regex.Replace(jiexiContent, @"(\d+)\．", "$1、", RegexOptions.IgnoreCase);
                content = content.Replace(item.Value, jiexiContent);
            }
            content = Regex.Replace(content, "答案([A-Z])", "答案：$1");
            content = Regex.Replace(content, "答：", "答案：");
            return content;
        }
    }
}
