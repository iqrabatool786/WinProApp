using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinProApp.DataModels.DataBase;
using WinProApp.Models;
using WinProApp.Services.Domain;
using WinProApp.ViewModels;
using WinProApp.ViewModels.Warehouse.Categories;

namespace WinProApp.Controllers.Warehouse
{
    [Authorize]
    public class CategoryController : BasedUserController
    {
        public readonly CategoryServices _categoryService;

        public CategoryController(CategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("/Warehouse/Categories")]
        [HttpGet]
        public IActionResult Index()
        {
            var allCategories = _categoryService.GetByAllAsync().Result;

            List<CategoryViewModel> catList = new List<CategoryViewModel>();
            var parentCats = allCategories.Where(c => c.ParentCategoryId == 0).ToList();

            foreach (var catItem in parentCats)
            {
                var curCats = new CategoryViewModel();
                curCats.Id = catItem.Id;
                curCats.ParentId = catItem.ParentCategoryId;
                curCats.CategoryNameEng = catItem.NameEng;
                curCats.CategoryNameArabic = catItem.NameArabic;
                catList.Add(curCats);

               GetCategoryHierarchy(catItem.Id, catList, catList,0);
            }


            ViewBag.CategoryListEng = new SelectList(catList, "Id", "CategoryNameEng");
            ViewBag.CategoryListArabic = new SelectList(catList, "Id", "CategoryNameArabic");

            return View();
        }

        [Route("/Warehouse/CategoryList")]
        public IActionResult CategoryList(JQueryDataTableParamModel param)
        {
            var results = _categoryService.GetList(param).Result.Select(x => new ListViewModel()
            {
                Id = x.Id,
                ParentCategoryId = x.ParentCategoryId,
                NameEng = x.NameEng,
                NameArabic = x.NameArabic,
                ParentCategoryNameEng = x.ParentCategoryId > 0 ? _categoryService.GetByIdAsync(x.ParentCategoryId).Result.NameEng : null,
                ParentCategoryNameArabic = x.ParentCategoryId > 0 ? _categoryService.GetByIdAsync(x.ParentCategoryId).Result.NameArabic : null,
                TotalRecordCount = x.TotalRecordCount
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

        [Route("/Warehouse/CreateUpdateCategory")]
        [HttpPost]
        public async Task<IActionResult> CreateUpdateCategory(AddEditViewModel model)
        {
            if (model.Id > 0)
            {
                var info = await _categoryService.GetByIdAsync(model.Id);
                info.Id = model.Id;
                info.ParentCategoryId = model.ParentCategoryId;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;


                await _categoryService.UpdateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
            else
            {
                var info = new Categories();
                info.ParentCategoryId = model.ParentCategoryId;
                info.NameEng = model.NameEng;
                info.NameArabic = model.NameArabic;

                await _categoryService.CreateAsync(info);
                return new JsonResult(new { id = model.Id });
            }
        }

        [Route("/Warehouse/GetCategoryInfo/{id}")]
        [HttpGet]
        public async Task<AddEditViewModel> GetCategoryInfo(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);

                var info = new AddEditViewModel()
                {
                    Id = result.Id,
                    NameEng = result.NameEng,
                    NameArabic = result.NameArabic,
                    ParentCategoryId = result.ParentCategoryId,
                };
                return info;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/DeleteCategory/{id}")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int childCount = await _categoryService.GetSubCategoryCountByIdAsync(id);
                if (childCount == 0)
                {
                    var result = await _categoryService.GetByIdAsync(id);
                    await _categoryService.DeleteAsync(result);
                    return new JsonResult("success");
                }
                else
                {
                    return new JsonResult("error");
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception source: {0}", ex.Message);
                throw;
            }
        }

        [Route("/Warehouse/CategoryValidateNameEng")]
        public IActionResult CategoryValidateNameEng(string NameEng, int Id, int ParentCategoryId)
        {
            bool flag = _categoryService.ValidateNameEng(NameEng, Id, ParentCategoryId);

            return Json(data: flag);

        }

        [Route("/Warehouse/CategoryValidateNameArabic")]
        public IActionResult CategoryValidateNameArabic(string NameArabic, int Id, int ParentCategoryId)
        {
            bool flag = _categoryService.ValidateNameEng(NameArabic, Id, ParentCategoryId);

            return Json(data: flag);

        }

        // ---------- Create SubCategory hierarchy
        public async Task<List<CategoryViewModel>> GetCategoryHierarchy(int parentId, List<CategoryViewModel> model, List<CategoryViewModel> category, int level=0)
        {
            string str = "---";
           // int levelX = 0;

            var children = _categoryService.GetByAllAsync().Result.Where(x => x.ParentCategoryId == parentId).ToList();

            if (children.Count > 0)
            {
                foreach (var child in children)
                {
                    level = 0;
                    str = "---";
                    level = await GetLevel(child.ParentCategoryId, 1);
                    // str = "---";
                    //    levelX = await GetLevel(parentId, 1);

                    if (level > 0)
                        {
                            for (int i = 0; i < level; i++)
                            {
                                str += str;
                            }

                        }
                        else
                        {
                            level = 0;
                            str = str;
                        }


                    var catInfo = new CategoryViewModel();
                    catInfo.Id = child.Id;
                    catInfo.ParentId = child.ParentCategoryId;
                    catInfo.CategoryNameEng = str + " " + child.NameEng;
                    catInfo.CategoryNameArabic = str + " " + child.NameArabic;

                    model.Add(catInfo);

                    
                    await GetCategoryHierarchy(child.Id, model, category, 0);

                    
                }
                level = 0;
            }

            return model;
        }


        public async Task<int> GetLevel(int categoryId, int level = 0)
        {
            var categoryInfo = _categoryService.GetByIdAsync(categoryId).Result;

            if (categoryInfo.ParentCategoryId > 0)
            {
                GetLevel(categoryInfo.ParentCategoryId, level++);
            }

            return level;
        }
    }
}
