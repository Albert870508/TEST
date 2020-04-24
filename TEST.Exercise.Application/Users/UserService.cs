using System;
using System.Collections.Generic;
using System.Text;
using TEST.Api.Result;
using TEST.Domain.Repositories;
using TEST.Domain.Uow;
using TEST.Exercise.Application.Users.Dto;
using TEST.Exercise.Domain.Entities;
using TEST.JWT;

namespace TEST.Exercise.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _user;
        private readonly IRepository<Question> _question;
        private readonly IRepository<QuestionType> _questionType;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IRepository<User> user,
            IRepository<Question> question,
            IRepository<QuestionType> questionType,
            IUnitOfWork unitOfWork)
        {
            _question = question;
            _questionType = questionType;
            _user = user;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 登陆接口
        /// </summary>
        /// <param name="loginCode"></param>
        /// <param name="tokenOptions"></param>
        /// <returns></returns>
        public Result<LoginOutput> Login(string weChatOpenId, TokenProvider tokenOptions)
        {
            //Questions();
            //_unitOfWork.SaveChanges();

            if (_user.Any(u => u.WeChatOpenId == weChatOpenId))//如果用户存在
            {
                User userInfo = _user.FirstOrDefault(u => u.WeChatOpenId == weChatOpenId);
                if (string.IsNullOrEmpty(userInfo.UserName) ||
                    string.IsNullOrEmpty(userInfo.PhoneNumber) || string.IsNullOrEmpty(userInfo.Department))
                { //资料不完善，返回false让用户完善信息
                    return Result<LoginOutput>.Success(new LoginOutput
                    {
                        UserName = userInfo.UserName,
                        Token = tokenOptions.GenerateToken(userInfo.Id.ToString()).access_token,
                        Perfect = false,
                        PhoneNumber = userInfo.PhoneNumber,
                        Dempartment = userInfo.Department
                    });
                }
                else
                {
                    return Result<LoginOutput>.Success(new LoginOutput
                    {
                        UserName = userInfo.UserName,
                        Token = tokenOptions.GenerateToken(userInfo.Id.ToString()).access_token,
                        Perfect = true,
                        PhoneNumber = userInfo.PhoneNumber,
                        Dempartment = userInfo.Department
                    }); ;
                }
            }
            else //用户不存在创建用户（相当于注册）,返回false让用户完善信息
            {
                _user.Insert(new User() { WeChatOpenId = weChatOpenId });
                _unitOfWork.SaveChanges();
                string userId = (_user.FirstOrDefault(u => u.WeChatOpenId == weChatOpenId)).Id.ToString();
                return Result<LoginOutput>.Success(new LoginOutput
                {
                    UserName = null,
                    Token = tokenOptions.GenerateToken(userId).access_token,
                    Perfect = false,
                    PhoneNumber = null,
                    Dempartment = null
                });
            }
        }
        /// <summary>
        /// 完善个人信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public Result<LoginOutput> ImproveInformation(long UserId, UserInput userInput)
        {
            if (!_user.Any(m => m.Id == UserId))
            {
                return Result<LoginOutput>.Fail("用户不存在");
            }
            if (String.IsNullOrEmpty(userInput.UserName) || string.IsNullOrEmpty(userInput.UserPhone) || string.IsNullOrEmpty(userInput.UserDempartment))
            {
                return Result<LoginOutput>.Fail("不能为空");
            }
            else
            {
                var thisUser = _user.FirstOrDefault(UserId);
                thisUser.UserName = userInput.UserName;
                thisUser.PhoneNumber = userInput.UserPhone;
                thisUser.Department = userInput.UserDempartment;
                _user.Update(thisUser);
                _unitOfWork.SaveChanges();
                return Result<LoginOutput>.Success(new LoginOutput()
                {
                    UserName = thisUser.UserName,
                    PhoneNumber = thisUser.PhoneNumber,
                    Dempartment = thisUser.Department
                });
            }

        }


        private void Questions()
        {
            var m = _questionType.FirstOrDefault(n => n.Name == "多选");
            var r = _questionType.FirstOrDefault(n => n.Name == "单选");
            var q = _questionType.FirstOrDefault(n => n.Name == "判断");

            new List<Question>() {
                 new Question(){
                    QuestionTypeId=r.Id,
                    Content="党的十九大报告提出的三大攻坚战是指(    )",
                    Options="{" +
                    "\"A\":\"精准脱贫、风险防控、污染防治\"," +
                    "\"B\":\"防范化解重大风险、精准脱贫、污染防治\"," +
                    "\"C\":\"污染防控、扶贫攻坚、风险治理\"," +
                    "\"D\":\"反腐倡廉、精准脱贫、污染防控\"}",
                    Answer="B",
                    AnswerNote="突出抓重点、补短板、强弱项，特别是要坚决打好防范化解重大风险、精准脱贫、污染防治的攻坚战，使全面建成小康社会得到人民认可、经得起历史检验。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="我国每年将(    )设立为“扶贫日”。",
                    Options="{" +
                    "\"A\":\"10月17日\"," +
                    "\"B\":\"3月12日\"," +
                    "\"C\":\"3月15日\"," +
                    "\"D\":\"10月10日\"}",
                    Answer="A",
                    AnswerNote="《国务院关于同意设立“扶贫日”的批复》同意自2014年起，将每年的10月17日 设立为“扶贫日”，具体工作由国务院扶贫办商有关部门组织实施。",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="习近平总书记在中共十八届二中全会第二次全体会议上指出，做好民生工作的工作思路是(    )",
                    Options="{" +
                    "\"A\":\"守住底线、突出重点、完善制度、引导舆论\"," +
                    "\"B\":\"守住底线、突出重点、完善制度、引导预期\"," +
                    "\"C\":\"兜底线  织密网  建机制\"," +
                    "\"D\":\"把握大局  围绕中心 突出重点  形成合力\"}",
                    Answer="A",
                    AnswerNote="要按照“守住底线、突出重点、完善制度、引导舆论”的思路做好民生工作，采取有力措施解决好群众生活中的实际困难和问题。(习近平总书记在中共十八届二中全会第二次全体会议上的讲话2013年2月28日）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="2015年6月，在部分省区市扶贫攻坚与“十三五”时期经济社会发展座谈会上，习近平总书记提出了扶贫开发工作的管理体制。其准确表述是(    )",
                    Options="{" +
                    "\"A\":\"中央统筹、省负总责、市（地）县抓落实\"," +
                    "\"B\":\"中央统筹、东西协作、精准扶贫脱贫\"," +
                    "\"C\":\"中央统筹、省负总责、市县抓落实\"," +
                    "\"D\":\"中央统筹、东西协作、精准滴灌\"}",
                    Answer="A",
                    AnswerNote="要强化扶贫开发工作领导责任制，把中央统筹、省负总责、市（地）县抓落实的管理体制，片为重点、工作到村、扶贫到户的工作机制，党政一把手负总责的扶贫开发工作责任制，真正落到实处。（《在部分省区市扶贫攻坚与“十三五”时期经济社会发展座谈会上的讲话》（2015年6月18日））。",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="“十三五”期间脱贫攻坚的目标是，到2020年实现“两不愁、三保障”。它们分别是指：到2020年，稳定实现农村贫困人口（    ）。",
                    Options="{" +
                    "\"A\":\"不愁吃、不愁穿，住房、生活、医疗有保障\"," +
                    "\"B\":\"不愁吃、不愁喝，住房、养老、医疗有保障\"," +
                    "\"C\":\"不愁吃、不愁穿，义务教育、基本医疗和住房安全有保障\"," +
                    "\"D\":\"不愁吃、不愁穿，基本医疗、安全住房和死亡安葬有保障\"}",
                    Answer="C",
                    AnswerNote="“十三五”期间脱贫攻坚的目标是，到2020年实现“两不愁、三保障”。“两不愁”，就是稳定实现农村贫困人口不愁吃、不愁穿； “三保障”，就是农村贫困人口义务教育、基本医疗和住房安全有保障。（《在中央扶贫开发工作会议上的讲话》（2015年11月27日））",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="新时期脱贫攻坚的目标，集中到一点，就是到2020年实现“两个确保”，即是，确保(    )。",
                    Options="{" +
                    "\"A\":\"农村贫困人口实现脱贫，贫困县全部脱贫摘帽。\"," +
                    "\"B\":\"贫困人口实现脱贫，贫困县全部摘帽。\"," +
                    "\"C\":\"农村人口实现脱贫，贫困县全部摘帽。\"," +
                    "\"D\":\"全国人民实现脱贫，贫困县全部摘帽。\"}",
                    Answer="A",
                    AnswerNote="新时期脱贫攻坚的目标，集中到一点，就是到2020年实现“两个确保”：农村贫困人口实现脱贫，贫困县全部脱贫摘帽。（《在中央扶贫开发工作会议上的讲话》（2015年11月27日））",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="党的十九大的主题是：不忘初心，牢记使命，高举中国特色社会主义伟大旗帜，决胜全面建成小康社会，夺取新时代中国特色社会主义伟大胜利，(    )。 ",
                    Options="{" +
                    "\"A\":\"为实现四个现代化不懈奋斗\"," +
                    "\"B\":\"为实现改革开放总目标不懈奋斗\"," +
                    "\"C\":\"为实现经济体制改革总目标不懈奋斗\"," +
                    "\"D\":\"为实现中华民族伟大复兴的中国梦不懈奋斗\"}",
                    Answer="D",
                    AnswerNote="大会的主题是：不忘初心，牢记使命，高举中国特色社会主义伟大旗帜，决胜全面建成小康社会，夺取新时代中国特色社会主义伟大胜利，为实现中华民族伟大复兴的中国梦不懈奋斗。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="(    )是近代以来中华民族最伟大的梦想",
                    Options="{" +
                    "\"A\":\"富国强兵\"," +
                    "\"B\":\"民族独立\"," +
                    "\"C\":\"实现中华民族伟大复兴\"," +
                    "\"D\":\"全面建成小康社会\"}",
                    Answer="C",
                    AnswerNote="实现中华民族伟大复兴是近代以来中华民族最伟大的梦想。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="当前，国内外形势正在发生深刻复杂变化，我国发展仍处于重要(    )，前景十分光明，挑战也十分严峻。",
                    Options="{" +
                    "\"A\":\"战略发展期\"," +
                    "\"B\":\"战略机遇期\"," +
                    "\"C\":\"战略调整期\"," +
                    "\"D\":\"战略规划期\"}",
                    Answer="B",
                    AnswerNote="当前，国内外形势正在发生深刻复杂变化，我国发展仍处于重要战略机遇期，前景十分光明，挑战也十分严峻。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="新时代中国特色社会主义思想，明确中国特色社会主义事业总体布局是(    )。",
                    Options="{" +
                    "\"A\":\"“五位一体”\"," +
                    "\"B\":\"“四个全面”\"," +
                    "\"C\":\"“四个自信”\"," +
                    "\"D\":\"两步走战略\"}",
                    Answer="A",
                    AnswerNote="明确中国特色社会主义事业总体布局是“五位一体”、战略布局是“四个全面”，强调坚定道路自信、理论自信、制度自信、文化自信。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="中国特色社会主义进入新时代，我国社会主要矛盾已经转化为（   ）。",
                    Options="{" +
                    "\"A\":\"人民日益增长的物质生活需要和落后的社会生产之间的矛盾\"," +
                    "\"B\":\"人民日益增长的美好生活需要和不平衡不充分的发展之间的矛盾\"," +
                    "\"C\":\"生产力和生产关系之间的矛盾\"," +
                    "\"D\":\"人民日益增长的美好生活需要和落后的社会生产之间的矛盾\"}",
                    Answer="B",
                    AnswerNote="中国特色社会主义进入新时代，我国社会主要矛盾已经转化为人民日益增长的美好生活需要和不平衡不充分的发展之间的矛盾。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                },
                new Question(){
                    QuestionTypeId=r.Id,
                    Content="新时代中国特色社会主义思想，明确全面深化改革总目标是（   ）。",
                    Options="{" +
                    "\"A\":\"完善和发展中国特色社会主义制度\"," +
                    "\"B\":\"完善和发展中国特色社会主义制度、推进国家治理体系和治理能力现代化\"," +
                    "\"C\":\"推进国家治理体系和治理能力现代化\"," +
                    "\"D\":\"建设社会主义现代化强国\"}",
                    Answer="B",
                    AnswerNote="明确全面深化改革总目标是完善和发展中国特色社会主义制度、推进国家治理体系和治理能力现代化。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }
            }.ForEach(item =>
            {
                _question.Insert(item);
            });

            new List<Question>() {

                new Question(){
                    QuestionTypeId=m.Id,
                    Content="中国梦的本质是（    ）。",
                    Options="{" +
                    "\"A\":\"国家富强\"," +
                    "\"B\":\"民族振兴\"," +
                    "\"C\":\"人民幸福\"," +
                    "\"D\":\"世界团结\"}",
                    Answer="ABC",
                    AnswerNote="中国梦视野宽广、内涵丰富、意蕴深远。习近平总书记多次强调，中国梦的本质是国家富强、民族振兴、人民幸福。（《习近平新时代中国特色社会主义思想三十讲》）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="中国共产党是代表最广大人民利益的政党，一切工作成败得失必然要由人民群众来检验，以人民（    ）作为根本标准。",
                    Options="{" +
                    "\"A\":\"拥护不拥护\"," +
                    "\"B\":\"赞成不赞成\"," +
                    "\"C\":\"高兴不高兴\"," +
                    "\"D\":\"答应不答应\"}",
                    Answer="ABCD",
                    AnswerNote="我们党是代表最广大人民利益的政党，一切工作成败得失必然要由人民群众来检验，以人民拥护不拥护、赞成不赞成、高兴不高兴、答应不答应作为根本标准。（《习近平新时代中国特色社会主义思想三十讲》）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="供给侧结构改革的重点是（    ）",
                    Options="{" +
                    "\"A\":\"解放和发展社会生产力，用改革的办法推进结构调整\"," +
                    "\"B\":\"减少无效和低端供给，扩大有效和中高端供给\"," +
                    "\"C\":\"增强供给结构对需求变化的适应性和灵活性\"," +
                    "\"D\":\"提高全要素生产率\"}",
                    Answer="ABCD",
                    AnswerNote="供给侧结构改革的重点是，解放和发展社会生产力，用改革的办法推进结构调整，减少无效和低端供给，扩大有效和中高端供给，增强供给结构对需求变化的适应性和灵活性，提高全要素生产率。（《习近平新时代中国特色社会主义思想三十讲》）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="党的十九大报告提出，全面推进依法治国总目标是（    ）",
                    Options="{" +
                    "\"A\":\"建设中国特色社会主义法制体系\"," +
                    "\"B\":\"建设社会主义法治国家\"," +
                    "\"C\":\"有法可依 有法必依 执法必须 违法必究\"," +
                    "\"D\":\"科学立法  严格执法 公正司法 全民守法\"}",
                    Answer="AB",
                    AnswerNote="明确全面推进依法治国总目标是建设中国特色社会主义法制体系、建设社会主义法治国家。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="党的十九大的主题是：不忘初心，牢记使命，（    ）。",
                    Options="{" +
                    "\"A\":\"高举中国特色社会主义伟大旗帜\"," +
                    "\"B\":\"决胜全面建成小康社会\"," +
                    "\"C\":\"夺取新时代中国特色社会主义伟大胜利\"," +
                    "\"D\":\"为实现中华民族伟大复兴的中国梦不懈奋斗\"}",
                    Answer="ABCD",
                    AnswerNote="大会的主题是：不忘初心，牢记使命，高举中国特色社会主义伟大旗帜，决胜全面建成小康社会，夺取新时代中国特色社会主义伟大胜利，为实现中华民族伟大复兴的中国梦不懈奋斗。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="统筹推进“五位一体”总体布局，协调推进“四个全面”战略布局，提高党（    ）的能力和定力，确保党始终总揽全局、协调各方。",
                    Options="{" +
                    "\"A\":\"把方向\"," +
                    "\"B\":\"谋大局\"," +
                    "\"C\":\"定政策\"," +
                    "\"D\":\"促改革\"}",
                    Answer="ABCD",
                    AnswerNote="坚持稳中求进工作总基调，统筹推进“五位一体”总体布局，协调推进“四个全面”战略布局，提高党把方向、谋大局、定政策、促改革的能力和定力，确保党始终总揽全局、协调各方。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="出台中央八项规定，严厉整治（    ）和奢靡之风，坚决反对特权。",
                    Options="{" +
                    "\"A\":\"形式主义\"," +
                    "\"B\":\"官僚主义\"," +
                    "\"C\":\"享乐主义\"," +
                    "\"D\":\"自由主义\"}",
                    Answer="ABC",
                    AnswerNote="出台中央八项规定，严厉整治形式主义、官僚主义、享乐主义和奢靡之风，坚决反对特权。巡视利剑作用彰显，实现中央和省级党委巡视全覆盖。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="党政军民学，东西南北中，党是领导一切的。必须增强（    ），自觉维护党中央权威和集中统一领导，自觉在思想上政治上行动上同党中央保持高度一致。",
                    Options="{" +
                    "\"A\":\"政治意识\"," +
                    "\"B\":\"大局意识\"," +
                    "\"C\":\"核心意识\"," +
                    "\"D\":\"看齐意识\"}",
                    Answer="ABCD",
                    AnswerNote="坚持党对一切工作的领导。党政军民学，东西南北中，党是领导一切的。必须增强政治意识、大局意识、核心意识、看齐意识，自觉维护党中央权威和集中统一领导，自觉在思想上政治上行动上同党中央保持高度一致。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="（    ），紧密联系、相互贯通、相互作用，其中起决定性作用的是党的建设新的伟大工程。",
                    Options="{" +
                    "\"A\":\"伟大斗争\"," +
                    "\"B\":\"伟大工程\"," +
                    "\"C\":\"伟大事业\"," +
                    "\"D\":\"伟大梦想\"}",
                    Answer="ABCD",
                    AnswerNote="伟大斗争，伟大工程，伟大事业，伟大梦想，紧密联系、相互贯通、相互作用，其中起决定性作用的是党的建设新的伟大工程。推进伟大工程，要结合伟大斗争、伟大事业、伟大梦想的实践来进行，确保党在世界形势深刻变化的历史进程中始终走在时代前列。（习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="要加快建设创新型国家，就必须培养造就一大批具有国际水平的（    ）和高水平创新团队。",
                    Options="{" +
                    "\"A\":\"战略科技人才\"," +
                    "\"B\":\"科技领军人才\"," +
                    "\"C\":\"青年科技人才\"," +
                    "\"D\":\"国际视野人才\"}",
                    Answer="ABC",
                    AnswerNote="加快建设创新型国家。倡导创新文化，强化知识产权创造、保护、运用。培养造就一大批具有国际水平的战略科技人才、科技领军人才、青年科技人才和高水平创新团队。 （习近平：决胜全面建成小康社会 夺取新时代中国特色社会主义伟大胜利——在中国共产党第十九次全国代表大会上的报告）",
                    QuestionTypeName="习近平新时代中国特色社会主义思想、党的十九大精神"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="就业促进法的立法目的是：（    ）",
                    Options="{" +
                    "\"A\":\"促进就业\"," +
                    "\"B\":\"促进经济发展与扩大就业相协调\"," +
                    "\"C\":\"促进社会和谐稳定\"," +
                    "\"D\":\"构建和发展和谐稳定的劳动关系\"}",
                    Answer="ABC",
                    AnswerNote="《就业促进法》第一条规定，为了促进就业，促进经济发展与扩大就业相协调，促进社会和谐稳定，制定本法。",
                    QuestionTypeName="就业创业"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="地方各级人民政府和有关部门应当加强对失业人员从事个体经营的指导，提供（    ）等服务。",
                    Options="{" +
                    "\"A\":\"政策咨询\"," +
                    "\"B\":\"就业培训\"," +
                    "\"C\":\"开业指导\"," +
                    "\"D\":\"业务信息\"}",
                    Answer="ABC",
                    AnswerNote="《就业促进法》第二十四条规定，地方各级人民政府和有关部门应当加强对失业人员从事个体经营的指导，提供政策咨询、就业培训和开业指导等服务。",
                    QuestionTypeName="就业创业"
                }, new Question(){
                    QuestionTypeId=m.Id,
                    Content="劳动者就业，不因（    ）等不同而受歧视。",
                    Options="{" +
                    "\"A\":\"民族\"," +
                    "\"B\":\"种族\"," +
                    "\"C\":\"性别\"," +
                    "\"D\":\"宗教信仰\"}",
                    Answer="ABCD",
                    AnswerNote="《就业促进法》第三条规定，劳动者依法享有平等就业和自主择业的权利。劳动者就业，不因民族、种族、性别、宗教信仰等不同而受歧视。",
                    QuestionTypeName="就业创业"
                }
            }.ForEach(item =>
            {
                _question.Insert(item);
            });
        }
    }
}
