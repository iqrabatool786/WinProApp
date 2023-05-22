using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WinProApp.ViewModels.Administrator;
using System.Globalization;
using System.Resources.NetStandard;
using System.Xml;
using System.Resources;
using Microsoft.Data.SqlClient;
using System.Data;
using WinProApp.Services.Domain;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.Models;
using WinProApp.Services.Domain.AssetSection;
using WinProApp.DataModels.DataBase;
using Microsoft.AspNetCore.Hosting;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WinProApp.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        public readonly ReportHeadService _reportHeadService;
        public readonly StoreService _storeService;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ReportHeadService reportHeadService, StoreService storeService, IWebHostEnvironment webHostEnvironment)
        {
            _reportHeadService = reportHeadService;
            _storeService = storeService;
            _webHostEnvironment = webHostEnvironment;
        }
        //--- English Main Resource File Update
        // GET: AdminController
        [Route("/Administrator/MainResourceEn")]
        public async Task<IActionResult> MainResourceEn()
        {
            List<ResourcesViewModel> model = new List<ResourcesViewModel>();
            string resxFile = @".\Resources\WebResource.resx";

            using (ResXResourceReader resxReader = new ResXResourceReader(resxFile))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    model.Add(new ResourcesViewModel { Name = (string)entry.Key, Value = (string)entry.Value });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMainResourceEn()
        {
            string[] rNames = Request.Form["txtName"];
            string[] rValues = Request.Form["txtValue"];
            var resx = new List<DictionaryEntry>();

            string resxFile = @".\Resources\WebResource.resx";
            using (ResXResourceReader reader = new ResXResourceReader(resxFile))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();

                for (int i = 0; i < rNames.Length; i++)
                {
                    string strName = rNames[i].ToString();
                    var existingResource = resx.Where(x => x.Key.ToString() == strName).FirstOrDefault();
                    if (existingResource.Value != rValues[i])
                    {
                        var modifiedResx = new DictionaryEntry() { Key = rNames[i], Value = rValues[i] };
                        resx.Remove(existingResource);  // REMOVING RESOURCE!
                        resx.Add(modifiedResx);
                    }
                }
            }

            using (var writer = new ResXResourceWriter(resxFile))
            {
                resx.ForEach(r =>
                {
                    writer.AddResource(r.Key.ToString(), r.Value.ToString());
                });
                writer.Generate();
                writer.Close();
            }

            return RedirectToAction("MainResourceEn");
        }

        //--- Arabic Main Resource File Update
        [Route("/Administrator/MainResourceAb")]
        public async Task<IActionResult> MainResourceAb()
        {
            List<ResourcesViewModel> model = new List<ResourcesViewModel>();
            string resxFile = @".\Resources\WebResource.ar.resx";

            using (ResXResourceReader resxReader = new ResXResourceReader(resxFile))
            {
                foreach (DictionaryEntry entry in resxReader)
                {
                    model.Add(new ResourcesViewModel { Name = (string)entry.Key, Value = (string)entry.Value });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMainResourceAb()
        {
            string[] rNames = Request.Form["txtName"];
            string[] rValues = Request.Form["txtValue"];
            var resx = new List<DictionaryEntry>();

            string resxFile = @".\Resources\WebResource.ar.resx";
            using (ResXResourceReader reader = new ResXResourceReader(resxFile))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();

                for (int i = 0; i < rNames.Length; i++)
                {
                    string strName = rNames[i].ToString();
                    var existingResource = resx.Where(x => x.Key.ToString() == strName).FirstOrDefault();
                    if (existingResource.Value != rValues[i])
                    {
                        var modifiedResx = new DictionaryEntry() { Key = rNames[i], Value = rValues[i] };
                        resx.Remove(existingResource);  // REMOVING RESOURCE!
                        resx.Add(modifiedResx);
                    }
                }
            }

            using (var writer = new ResXResourceWriter(resxFile))
            {
                resx.ForEach(r =>
                {
                    writer.AddResource(r.Key.ToString(), r.Value.ToString());
                });
                writer.Generate();
                writer.Close();
            }

            return RedirectToAction("MainResourceEn");
        }

        // GET: AdminController/Backup
        [Route("/Administrator/Backup")]
        [HttpGet]
        public async Task<IActionResult> Backup()
        {
            string curPath = "./DatabaseBackup";
            DirectoryInfo d = new DirectoryInfo(curPath);

            FileInfo[] Files = d.GetFiles("*.*");
            List<BackupFileListViewModel> FileList= new List<BackupFileListViewModel>();
            string str = "";

            foreach (FileInfo file in Files)
            {
                FileList.Add(new BackupFileListViewModel { BackupFileName = file.Name });
            }
            ViewBag.FileList = FileList;

            return View();
        }


        // GET: AdminController/Backup
        [Route("/Administrator/BackupDb")]
        [HttpPost]
        public async Task<IActionResult> BackupDb()
        {
            

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string connString = builder.Build().GetSection("ConnectionStrings").GetSection("WinProAppContextConnection").Value;

            string curPath= "./DatabaseBackup";
            string backupDIR = curPath;// Path.GetFullPath(curPath); 

            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }

            try
            {
                SqlConnection con = new SqlConnection(connString);
                SqlCommand sqlcmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                con.Open();
                sqlcmd = new SqlCommand("backup database winprerp_winproERP to disk='" + backupDIR + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
                sqlcmd.ExecuteNonQuery();
                con.Close();

                return new JsonResult(true);
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }

            
            //return RedirectToAction(Backup);
        }


        [Route("/Administrator/BackupDelete/{filename}")]
        [HttpPost]
        public async Task<IActionResult> BackupDelete(string filename)
        {
            string curPath = "./DatabaseBackup/" + filename;
            if (System.IO.File.Exists(curPath))
            {
                System.IO.File.Delete(curPath);
            }

            return new JsonResult(true);
        }

        [Route("/Administrator/FileDownload/{filename}")]
        public ActionResult FileDownload(string filename)
        {
            String FilePath = "./DatabaseBackup/";

            FilePath = System.IO.Path.Combine(FilePath, filename);
            var fs = new FileStream(FilePath, FileMode.Open);
            ;
            return File(fs, "application/octet-stream", filename);
        }

        #region Reports Headers Management
        //_reportHeadService

        [Route("/Administrator/ReportHeads")]
        [HttpGet]
        public async Task<IActionResult> ReportHeads()
        {
            var storeList = await _storeService.GetByAllAsync();

            ViewBag.Stores = new SelectList(storeList, "Id", "Name");
            return View();
        }

        [Route("/Administrator/ReportHeadsList")]
        [HttpPost]
        public IActionResult ReportHeadsList(JQueryDataTableParamModel param)
        {
            var results = _reportHeadService.GetList(param).Result.Select(x => new ReportHeadsListViewModel()
            {
                Id = x.Id,
                StoreId = x.StoreId,
                StoreName = x.StoreId != null ? _reportHeadService.GetStoreNameByIdAsync(x.StoreId.Value).Result : null,
                StoreAddress = x.StoreId != null ? _storeService.GetByIdAsync(x.StoreId.Value).Result.Address : null,
                VatNo = x.StoreId != null ? _storeService.GetByIdAsync(x.StoreId.Value).Result.VatNo : null,
                Logo = x.Logo,
                ReportHeaderEng = x.ReportHeaderEng,
                ReportHeaderArabic = x.ReportHeaderArabic,
                ReportFooterEng = x.ReportFooterEng,
                ReportFooterArabic = x.ReportFooterArabic,
                DefaultStore = x.DefaultStore ==true?"Yes":"No",
                TotalRecordCount= x.TotalRecordCount,

            }).ToList();

            int TotalRecordCount = 0;

            if (results.Count > 0)
                TotalRecordCount = results[0].TotalRecordCount;

            return Json(data: new
            {
                param.sEcho,
                iTotalRecords = TotalRecordCount,
                iTotalDisplayRecords = TotalRecordCount,
                aaData = results
            });
        }

        [Route("/Administrator/CreateUpdateReportHead")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateReportHead(ReportHeadAddEditViewModel model, IFormFile LogoImg)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = webRootPath + "/Images/Logo/";

            string logoName = null;
            if (LogoImg != null && LogoImg.Length > 0)
            {
                string curLogoName = Path.GetFileName(LogoImg.FileName);
                string curLogoExtention = Path.GetExtension(LogoImg.FileName);

                logoName = DateTime.Now.ToString("ddMMyyyy_HHmmss") + curLogoExtention;
                string DocSavePath = System.IO.Path.Combine(uploadPath, logoName);

                using (var stream = System.IO.File.Create(DocSavePath))
                {
                    await LogoImg.CopyToAsync(stream);
                }

            }

            if (model.Id > 0)
            {
                var info = await _reportHeadService.GetByIdAsync(model.Id);
                string? oldLogo = info.Logo;

                info.Id = model.Id;
                info.StoreId = model.StoreId;
                if(logoName != null && oldLogo != null)
                {
                    string oldLogoPath = System.IO.Path.Combine(uploadPath, oldLogo);
                    if (System.IO.File.Exists(oldLogoPath))
                    {
                        System.IO.File.Delete(oldLogoPath);
                    }
                    
                }
                if(logoName != null)
                {
                    info.Logo = logoName;
                }
                else
                {
                    info.Logo = model.Logo;
                }
                
                info.ReportHeaderEng = model.ReportHeaderEng;
                info.ReportHeaderArabic = model.ReportHeaderArabic;
                info.ReportFooterEng = model.ReportFooterEng;
                info.ReportFooterArabic = model.ReportFooterArabic;
                info.DefaultStore = model.DefaultStore;
                await _reportHeadService.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new ReportHeads();
                info.Id = model.Id;
                info.StoreId = model.StoreId;
                info.Logo = model.Logo;
                info.ReportHeaderEng = model.ReportHeaderEng;
                info.ReportHeaderArabic = model.ReportHeaderArabic;
                info.ReportFooterEng = model.ReportFooterEng;
                info.ReportFooterArabic = model.ReportFooterArabic;
                info.DefaultStore= model.DefaultStore;
                await _reportHeadService.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Administrator/GetHeaderInfo/{id}")]
        [HttpPost]
        public async Task<IActionResult> GetHeaderInfo(int id)
        {
            try
            {
                var result = await _reportHeadService.GetByStoreIdAsync(id);
                if (result != null)
                {
                    var info = new ReportHeadStoreDetailViewModel()
                    {
                        Id = result.Id,
                        StoreId = result.StoreId,
                        StoreCode = result.StoreId != null ? _storeService.GetByIdAsync(result.StoreId.Value).Result.StoreCode : null,
                        StoreAddress = result.StoreId != null ? _storeService.GetByIdAsync(result.StoreId.Value).Result.Address : null,
                        VatNo = result.StoreId != null ? _storeService.GetByIdAsync(result.StoreId.Value).Result.VatNo : null,
                        Logo = result.Logo,
                        ReportHeaderEng = result.ReportHeaderEng,
                        ReportHeaderArabic = result.ReportHeaderArabic,
                        ReportFooterEng = result.ReportFooterEng,
                        ReportFooterArabic = result.ReportFooterArabic,
                        DefaultStore=result.DefaultStore,
                    };
                    return Json(data: info);
                }
                else
                {
                    return Json(data: null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Administrator/DeleteReportHead/{id}")]
        [HttpPost]
        public async Task<IActionResult> DeleteReportHead(int id)
        {
            try
            {
                var result = await _reportHeadService.GetByIdAsync(id);
                await _reportHeadService.DeleteAsync(result);

                return new JsonResult(new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        #endregion

    }
}
