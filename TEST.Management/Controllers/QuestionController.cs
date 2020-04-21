using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using TEST.Exercise.Application.Exercises;
using TEST.Exercise.Application.ExerciseType;
using TEST.Exercise.Domain.Entities;
using X.PagedList;

namespace TEST.Management.Controllers
{
    public class QuestionController : Controller
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IExerciseService _exerciseService;
        private readonly IExerciseTypeService _exerciseTypeService;
        public QuestionController(IExerciseService exerciseService, IExerciseTypeService exerciseTypeService, IWebHostEnvironment webHostEnvironment)
        {
            _exerciseService = exerciseService;
            _exerciseTypeService = exerciseTypeService;
            _webHostEnvironment = webHostEnvironment;
            
        }

        [HttpGet]
        public IActionResult Index(int? pageIndex = 1,int pageSize=5, string searchString = null)
        {
            ViewData["QuestionType"] = new SelectList(_exerciseTypeService.GetExerciseTypes().Data,"Id","Name");
            var listPaged = _exerciseService.GetExercise(searchString).Data.ToPagedList(pageIndex ?? 1, 5);
            if (listPaged.PageNumber != 1 && pageIndex.HasValue && pageIndex > listPaged.PageCount)
            {
                listPaged = null;
            }
            ViewData["PageSize"] = pageSize;
            return View(listPaged);
        }

        [HttpPost]
        public IActionResult QuestionList([FromBody]PageList pageList)
        {
            int? pageIndex = int.Parse(pageList.PageIndex);
            ViewData["QuestionType"] = new SelectList(_exerciseTypeService.GetExerciseTypes().Data, "Id", "Name");
            var listPaged = _exerciseService.GetExercise(pageList.SearchString).Data.ToPagedList(pageIndex ?? 1, int.Parse(pageList.PageSize));
            if (listPaged.PageNumber != 1 && pageIndex.HasValue && pageIndex > listPaged.PageCount)
            {
                listPaged = null;
            }
            ViewData["PageSize"] = pageList.PageSize;
            return View();
        }

        /// <summary>
        /// Excel数据模板下载
        /// </summary>
        /// <returns></returns>
        public IActionResult DownloadDataTemplate()
        {           
            return File("数据模板.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "数据模板.xlsx");
        }
        /// <summary>
        /// Excel上传，支持多个文件同时上传
        /// </summary>
        /// <param name="files">上传的文件列表</param>
        /// <returns></returns>
        [HttpPost]        
        public void UploadDateTemplateFile(List<IFormFile> files)
        {
            List<Question> questions = new List<Question>();
            long size = 0;
            #region
            string fileSaveRootPath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadFiles_"+ System.DateTime.Now.ToString("yyyyMMddHHmmss"));//文件保存路径
            if (!Directory.Exists(fileSaveRootPath))
            {
                Directory.CreateDirectory(fileSaveRootPath);
            }
            #endregion
            foreach (var file in files)//遍历上传的Excel文件列表
            {
                #region 获取到对应的Excel文件
                var fileName = file.FileName.Trim('"');//获取文件名
                
                fileName = Path.Combine(fileSaveRootPath, fileName);//文件上传的路径
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(fileName))//创建文件流
                {
                    file.CopyTo(fs);//将上载文件的内容复制到目标流
                    fs.Flush();//清除此流的缓冲区并导致将任何缓冲数据写入
                }
                #endregion
                #region 读取Excel文件
                using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;
                    for (int row = 4; row < rowCount+1; row++)
                    {
                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 2].Value != null && worksheet.Cells[row, 3].Value != null)
                        {
                            questions.Add(new Question()
                            {
                                QuestionTypeId = _exerciseTypeService.GetIdByType(worksheet.Cells[row, 1].Value.ToString()).Data,
                                Content = worksheet.Cells[row, 2].Value.ToString(),
                                Answer = worksheet.Cells[row, 3].Value.ToString(),
                                AnswerNote = worksheet.Cells[row, 4].Value == null ? string.Empty : worksheet.Cells[row, 4].Value.ToString(),
                                QuestionTypeName = worksheet.Cells[row, 1].Value.ToString()//添加这个字段主要是为了管理界面便于取值
                            }) ;
                        }
                        else
                        { 
                        
                        }
                        
                    }
                }
                #endregion
            }
            if (_exerciseService.AddQuestions(questions).IsSuccess)
            {
                ViewBag.Message = $"{files.Count}个文件 /{size}字节上传成功!";
            }
            else
            {
                ViewBag.Message = "上传失败!";
            }
            #region
            if (Directory.Exists(fileSaveRootPath))
            {
                Directory.Delete(fileSaveRootPath, true);
            }
           
            #endregion
            

        }
    }

    public class PageList
    {
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchString { get; set; }
    }
}