using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TEST.Exercise.Domain.Entities;

namespace TEST.EntityFrameworkCore
{
    public static class QuestionFileHelper
    {
        /// <summary>
        /// 通过上传的文件获得试题
        /// </summary>
        /// <param name="fileName">上传的文件路径</param>
        /// <param name="_exerciseTypeService">后端服务接口</param>
        /// <param name="type">试题类型（单选/多选/判断）</param>
        /// <returns></returns>
        public static List<Question> GetQuestionsByFile(string fileName, string questionType, long questionTypeId = 0)//IExerciseTypeService _exerciseTypeService,string type
        {
            List<Question> questions = new List<Question>();
            string content = System.IO.File.ReadAllText(fileName, Encoding.GetEncoding("gb2312"));
            if (questionType == "单选"|| questionType == "多选")
            {
                //content = HandleContent(content);
            }            
            if (questionType == "单选")
            {
                questions = GetXuanZeQuestions(content,questionTypeId);
            }
            if (questionType == "多选")
            {
                questions = GetXuanZeQuestions(content, questionTypeId);
            }
            if (questionType == "判断")
            {
                questions = GetPanDuanQuestions(content, questionTypeId);
            }
            if (questionType == "填空")
            {
                questions = GetOtherQuestions(content, questionTypeId);
            }
            if (questionType == "简答")
            {
                questions = GetOtherQuestions(content, questionTypeId);
            }
            if (questionType == "案例分析")
            {
                questions = GetOtherQuestions(content, questionTypeId);
            }
            return questions;
        }
        public static List<Question> GetPanDuanQuestions(string content, long questionTypeId)
        {
            List<Question> questions = new List<Question>();
            MatchCollection matchCollection = Regex.Matches(content, @"\d+\．[\s\S]*?答案[：|:][\s\S]*?(?=\d+\．[\s\S]*?答案[：|:][\s\S]*?|$)");
            foreach (Match item in matchCollection)
            {
                Question question = new Question();
                question.QuestionTypeId = questionTypeId;//_exerciseTypeService.GetIdByType(questionsType).Data;//题目类型
                question.QuestionTypeName = string.Empty;
                question.Content = Regex.Match(item.Value, @"[\s\S]*?(?=答案[：|:])").Value.Trim();
                question.Content = Regex.Replace(question.Content, @"^\d+[．|.|、]", string.Empty);
                question.Options = "{'正确','错误'}";
                question.Answer = Regex.Match(item.Value, @"(?<=答案\s*\S\s*)[\s\S]*?(?=解析)").Value.Trim();
                question.AnswerNote = Regex.Match(item.Value, @"解析[\s\S]*").Value.Trim();
                if (question.AnswerNote.Contains("命题单位"))
                {
                    question.AnswerNote = Regex.Match(question.AnswerNote, @"[\s\S]*?(?=命题单位)").Value.Trim();
                }

                questions.Add(question);
            }
            return questions;
        }
        public static List<Question> GetOtherQuestions(string content, long questionTypeId)
        {
            List<Question> questions = new List<Question>();
            MatchCollection matchCollection = Regex.Matches(content, @"\d+\．[\s\S]*?答案[：|:][\s\S]*?(?=\d+\．[\s\S]*?答案[：|:][\s\S]*?|$)");
            foreach (Match item in matchCollection)
            {
                Question question = new Question();
                question.QuestionTypeId = questionTypeId;//_exerciseTypeService.GetIdByType(questionsType).Data;//题目类型
                question.QuestionTypeName = string.Empty;
                question.Content = Regex.Match(item.Value, @"[\s\S]*?(?=答案[：|:])").Value.Trim();
                question.Content = Regex.Replace(question.Content, @"^\d+[．|.|、]", string.Empty);
                string option = string.Empty;
                question.Answer = Regex.Match(item.Value, @"(?<=答案\s*\S\s*)[\s\S]*?(?=解析)").Value.Trim();
                question.AnswerNote = Regex.Match(item.Value, @"解析[\s\S]*").Value.Trim();
                if (question.AnswerNote.Contains("命题单位"))
                {
                    question.AnswerNote = Regex.Match(question.AnswerNote, @"[\s\S]*?(?=命题单位)").Value.Trim();
                }

                questions.Add(question);
            }
            return questions;
        }

        /// <summary>
        /// 选择题
        /// </summary>
        /// <param name="content"></param>
        /// <param name="questionTypeId"></param>
        /// <returns></returns>
        public static List<Question> GetXuanZeQuestions(string content, long questionTypeId)
        {
            List<Question> questions = new List<Question>();
            MatchCollection matchCollection = Regex.Matches(content, @"\d+\．[\s\S]*?答案[：|:][\s\S]*?(?=\d+\．[\s\S]*?答案[：|:][\s\S]*?|$)");
            foreach (Match item in matchCollection)
            {
                Question question = new Question();
                question.QuestionTypeId = questionTypeId;//_exerciseTypeService.GetIdByType(questionsType).Data;//题目类型
                question.QuestionTypeName = string.Empty;
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
