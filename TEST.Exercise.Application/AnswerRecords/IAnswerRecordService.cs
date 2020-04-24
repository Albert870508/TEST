using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Exercise.Application.AnswerRecords.Dto;
using TEST.Exercise.Domain.Entities;

namespace TEST.Exercise.Application.AnswerRecords
{
    public interface IAnswerRecordService
    {
        /// <summary>
        /// 添加答题记录
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        Result<bool> AddAnswerRecord(long userId, List<string> questions);

        /// <summary>
        /// 获取某个用户各个类型试题的答题数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Result<AnswerRecordDto> GetRecordAnser(long userId);


    }
}
