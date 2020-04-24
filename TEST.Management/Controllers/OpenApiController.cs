using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDSLJ.MiniApp.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TEST.Api.Result;
using TEST.Api.Route;
using TEST.Exercise.Application.AnswerRecords;
using TEST.Exercise.Application.AnswerRecords.Dto;
using TEST.Exercise.Application.Departments;
using TEST.Exercise.Application.Departments.Dto;
using TEST.Exercise.Application.Examinations;
using TEST.Exercise.Application.Examinations.Dto;
using TEST.Exercise.Application.Exercises;
using TEST.Exercise.Application.Exercises.Dto;
using TEST.Exercise.Application.ExerciseType;
using TEST.Exercise.Application.Scores;
using TEST.Exercise.Application.Scores.Dto;
using TEST.Exercise.Application.Users;
using TEST.Exercise.Application.Users.Dto;
using TEST.JWT;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TEST.Management.Controllers
{
    [RouteV1("OpenApi")]
    [ApiController]
    public class OpenApiController : Controller
    {
        private readonly IDepartmentService _iDepartmentService;
        private readonly IExaminationService _iExaminationService;
        private readonly IScoreService _iScoreService;
        private readonly IUserService _userService;
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseTypeService _exerciseTypeService;
        private readonly TokenProvider _tokenOption;
        private readonly IAnswerRecordService _answerRecordService;


        public OpenApiController(IDepartmentService iDepartmentService,
            IExaminationService iExaminationService, IScoreService iScoreService,
            IConfiguration configuration, IUserService userService, IAnswerRecordService answerRecordService,
            IExerciseService exerciseService, IExerciseTypeService exerciseTypeService)
        {
            _answerRecordService = answerRecordService;
            _iDepartmentService = iDepartmentService;
            _iExaminationService = iExaminationService;
            _iScoreService = iScoreService;
            _userService = userService;
            _exerciseService = exerciseService;
            _exerciseTypeService = exerciseTypeService;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Audience:Secret").Value));
            var issuer = configuration.GetSection("Audience:Issuer").Value;
            var audience = configuration.GetSection("Audience:Audience").Value;

            _tokenOption = new TokenProvider(new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            });
        }

        #region 部门
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<List<DepartmentDto>> GetAllDepartment()
        {
            return _iDepartmentService.GetAllDepartment();
        }
        #endregion

        #region 考试
        /// <summary>
        /// 考试/测试/练习 接口
        /// </summary>
        /// <param name="dayOrWeek">按天还是周（天:day 周：week）</param>
        /// <returns></returns>
        [HttpGet]
        public Result<ExaminationOutPut> TestStart([FromQuery]string dayOrWeek)
        {
            return _iExaminationService.TestStart(dayOrWeek);
        }
        /// <summary>
        /// 交卷接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result<TestEndOutput> TestEnd([FromBody]ExaminationIntput examinationIntput)
        {
            return _iExaminationService.TestEnd(long.Parse(User.Identity.Name), examinationIntput);
        }
        #endregion

        #region 排名
        /// <summary>
        /// 排名接口
        /// </summary>
        /// <param name="examinationId">考试编号</param>
        /// <param name="dayOrWeek">周排名还是日排名（日排名：day 周排名:week）如果为空则排序所有</param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public Result<List<ScoreOutput>> Ranking([FromQuery]ScoreIntput scoreIntput)
        {
            return _iScoreService.Ranking(scoreIntput);
        }
        #endregion

        #region 用户
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result<LoginOutput> Login([FromBody]LoginReq req)
        {

            string weChatOpenId = WeChatHelper.GetSession(req.LoginCode).OpenId;//根据loginCode获取openid
            return _userService.Login(weChatOpenId, _tokenOption);
        }
        /// <summary>
        /// 用户完善个人信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost]
        public Result<LoginOutput> ImproveInformation([FromBody]UserInput userInput)
        {
            return _userService.ImproveInformation(long.Parse(User.Identity.Name), userInput);
        }
        #endregion

        /// <summary>
        /// 获取某个类型的所有的题
        /// </summary>
        /// <param name="questionType"></param>
        /// <returns></returns>
        [HttpGet]
        public Result<List<QuestionsDto>> GetAllQuestionsByType([FromQuery]string questionType)
        {
            string questionTypeId = null;
            if (questionType== "Single")//单选
            {
                questionTypeId = _exerciseTypeService.GetIdByType("单选").Data.ToString();
            }
            if (questionType == "Multiple")//多选
            {
                questionTypeId = _exerciseTypeService.GetIdByType("多选").Data.ToString();
            }
            if (questionType == "Judge")//判断
            {
                questionTypeId = _exerciseTypeService.GetIdByType("判断").Data.ToString();
            }
            if (questionType == "Completion")//填空
            {
                questionTypeId = _exerciseTypeService.GetIdByType("填空").Data.ToString();
            }
            if (questionType == "Case")//案例
            {
                questionTypeId = _exerciseTypeService.GetIdByType("案例分析").Data.ToString();
            }
            if (questionType == "Simple")//简答
            {
                questionTypeId = _exerciseTypeService.GetIdByType("简答").Data.ToString();
            }
            if (string.IsNullOrEmpty(questionTypeId))
            {
                return Result<List<QuestionsDto>>.Fail("不存在该类型试题");
            }
            return _exerciseService.GetExercise(questionTypeId);
        }

        /// <summary>
        /// 创建答题记录
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        [HttpPost]
        public Result<bool> RecordAnswer([FromBody]List<string>questions)
        {
            return _answerRecordService.AddAnswerRecord(long.Parse(User.Identity.Name), questions);
        }
        /// <summary>
        /// 获取用户各个类型题目已答题数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<AnswerRecordDto> GetRecordAnser()
        {
            return _answerRecordService.GetRecordAnser(long.Parse(User.Identity.Name));
        }

    }

    public class LoginReq
    {
        public string LoginCode { get; set; }
    }
}
