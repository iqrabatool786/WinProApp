using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using WinProApp.DataModels.DataBase;
using WinProApp.DataModels.DataBase.StoredProcedures;
using WinProApp.Models;
using WinProApp.ViewModels;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;


namespace WinProApp.Services.Domain
{
    public class CommonService
    {
        private readonly WinProDbContext _DbContext;

        public CommonService(WinProDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public async Task<int> GetCurrentStoreId()
        {
            int storeId = 0;
            if(_DbContext.ReportHeads.Where(x=>x.DefaultStore == true).Count() > 0)
            {
                storeId = _DbContext.ReportHeads.FirstOrDefaultAsync(x=>x.DefaultStore==true).Result.StoreId.Value;
            }
            return storeId;
        }

        public async Task<List<Units>> GetUnitsAsync()
        {
            return await _DbContext.Set<Units>().ToListAsync();
        }

        public async Task<Vat> GetVatAsync()
        {
            return await _DbContext.Set<Vat>().FirstAsync();
        }

        public async Task<SalesTypes> GetTransactionTypesAsync()
        {
            return await _DbContext.Set<SalesTypes>().FirstAsync();
        }

        public async Task<List<Categories>> GetCategoriesAsync()
        {
            return await _DbContext.Set<Categories>().ToListAsync();
        }

        public async Task<Categories> GetCategoryByIdAsync(int id)
        {
            return await _DbContext.Set<Categories>().FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<int> GetParentCategoryIdAsync(int id)
        {
            var catInfo = await _DbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            return catInfo != null ? catInfo.ParentCategoryId : 0;
        }

        public async Task<List<Colors>> GetColorsAsync()
        {
            return await _DbContext.Set<Colors>().ToListAsync();
        }

        public async Task<List<Sizes>> GetSizesAsync()
        {
            return await _DbContext.Set<Sizes>().ToListAsync();
        }

        public async Task<List<Departments>> GetDepartmentsAsync()
        {
            return await _DbContext.Set<Departments>().ToListAsync();
        }

        public async Task<List<Seassons>> GetSeassonsAsync()
        {
            return await _DbContext.Set<Seassons>().ToListAsync();
        }

        public async Task<string> GetStyleCodeByIdAsync(int id)
        {
            string codex = "";
            var info = await _DbContext.Styles.FirstOrDefaultAsync(x => x.Id == id);
            if(info != null)
            {
                codex = info.Code;
            }
            return codex;
        }

        public async Task<List<Styles>> GetStylesAsync()
        {
            return await _DbContext.Set<Styles>().ToListAsync();
        }

        public async Task<List<Vendors>> GetVendorsAsync()
        {
            return await _DbContext.Set<Vendors>().ToListAsync();
        }

        public async Task<List<Descriptions>> GetDescriptionsAsync()
        {
            return await _DbContext.Set<Descriptions>().ToListAsync();
        }

        public async Task<Descriptions> GetDescriptionInfoByIdAsync(int id)
        {
            return await _DbContext.Set<Descriptions>().FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<List<Brands>> GetBrandsAsync()
        {
            return await _DbContext.Set<Brands>().ToListAsync();
        }

        public async Task<List<Groups>> GetGroupsAsync()
        {
            return await _DbContext.Set<Groups>().ToListAsync();
        }

        public async Task<List<Categories>> GetCategorieAutocomplete(string Prefix)
        {
            return await _DbContext.Set<Categories>().Where(x=> (x.NameEng.Contains(Prefix) || x.NameArabic.Contains(Prefix))).ToListAsync();
        }

        public async Task<List<ProductInfo>> GetProductInfoAsync(string Prefix)
        {
            return await _DbContext.Set<ProductInfo>().Where(x=>x.ProductId.Contains(Prefix)).OrderBy(x=>x.ProductId).Take(20).ToListAsync();
        }

        public async Task<List<Styles>> GetSkuStylefoAsync(string Prefix)
        {
            return await _DbContext.Set<Styles>().Where(x => x.Code.Contains(Prefix)).OrderBy(x => x.Code).Take(20).ToListAsync();
        }

        public async Task<List<Departments>> GetDepartmentsInfoAsync(string Prefix)
        {
            return await _DbContext.Set<Departments>().Where(x => x.NameEng.Contains(Prefix)).OrderBy(x => x.NameEng).Take(20).ToListAsync();
        }

        public async Task<List<Seassons>> GetSeassonsInfoAsync(string Prefix)
        {
            return await _DbContext.Set<Seassons>().Where(x => x.NameEng.ToLower().Contains(Prefix.ToLower())).OrderBy(x => x.NameEng).Take(20).ToListAsync();
        }

        public async Task<List<Colors>> GetColorsInfoAsync(string Prefix)
        {
            return await _DbContext.Set<Colors>().Where(x => x.NameEng.Contains(Prefix)).OrderBy(x => x.NameEng).Take(20).ToListAsync();
        }

        public async Task<List<Sizes>> GetSizesInfoAsync(string Prefix)
        {
            return await _DbContext.Set<Sizes>().Where(x => x.NameEng.Contains(Prefix)).OrderBy(x => x.NameEng).Take(20).ToListAsync();
        }

        public async Task<List<Years>> GetAllYearsAsync()
        {
            return await _DbContext.Set<Years>().OrderBy(x => x.YearName).ToListAsync();
        }

       
        public async Task<List<Years>> GetYearsAsync(int? Prefix)
        {
            return await _DbContext.Set<Years>().Where(x => x.YearName == Prefix).OrderBy(x => x.YearName).Take(20).ToListAsync();
        }

        public async Task<ProductInfo> GetProductInfoByIdAsync(long id)
        {
            return await _DbContext.Set<ProductInfo>().FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<long> GetProductIdByBarcodeAsync(string code)
        {
            long Id = 0;
            var pro = await _DbContext.Set<ProductInfo>().Where(x => x.ProductId == code).ToListAsync();
            if (pro != null && pro.Count > 0)
            {
                var pr = await _DbContext.Set<ProductInfo>().FirstOrDefaultAsync(x => x.ProductId == code);
                if(pr != null)
                {
                    Id = pr.Id;
                }
            }
            return Id;
        }


        public async Task<int> GetStyleIdByCodeAsync(string code)
        {
            int Id = 0;
            if (_DbContext.Set<Styles>().Where(x => x.Code == code).Count() > 0)
            {
                Id = _DbContext.Set<Styles>().FirstOrDefaultAsync(x => x.Code == code).Result.Id;
            }
            return Id;
        }


        public async Task<int> GetYearIdByNameAsync(int yearname)
        {
            int Id = 0;
            if (_DbContext.Set<Years>().Where(x => x.YearName == yearname).Count() > 0)
            {
                Id = _DbContext.Set<Years>().FirstOrDefaultAsync(x => x.YearName == yearname).Result.Id;
            }
            return Id;
        }



        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _DbContext.Set<Supplier>().OrderBy(x => x.CompanyName).Take(20).ToListAsync();
        }

        public async Task<List<Supplier>> GetSupplierBpCodesAsync(string Prefix)
        {
            return await _DbContext.Set<Supplier>().Where(x => x.Bpcode.Contains(Prefix)).OrderBy(x => x.Bpcode).Take(20).ToListAsync();
        }

       
        public async Task<string> GetCompanyNameByIdAsync(long? id)
        {
            if (id != null)
            {
                long Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Suppliers.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strCompanyName = _DbContext.Suppliers.FirstOrDefault(x => x.Id == Id).CompanyName;
                        return strCompanyName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetCategoryNameByIdAsync(int? id)
        {
            if (id != null)
            { 
                int Id = id.Value;
                if (Id > 0)
                {
                    string catName = _DbContext.Categories.FirstOrDefault(x => x.Id == Id) != null ? _DbContext.Categories.FirstOrDefault(x => x.Id == Id).NameEng : null;
                    return catName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetSkuNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    string strName = _DbContext.Styles.FirstOrDefault(x => x.Id == Id).NameEng;
                    return strName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetSkuCodeByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if(Id > 0)
                {
                    string strCode = _DbContext.Styles.FirstOrDefault(x => x.Id == Id).Code;
                    return strCode;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetSessonNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if(Id > 0)
                {
                    string strName = _DbContext.Seassons.FirstOrDefault(x => x.Id == Id).NameEng;
                    return strName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetDepartmentNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    string strName = _DbContext.Departments.FirstOrDefault(x => x.Id == Id).NameEng;
                    return strName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetColorNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Colors.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Colors.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetSizeNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Sizes.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Sizes.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetDescriptionByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Descriptions.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Descriptions.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetUnitNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Units.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Units.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetVendorNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Vendors.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Vendors.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetBrandNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Brands.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Brands.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetGroupNameByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Groups.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Groups.FirstOrDefault(x => x.Id == Id).NameEng;
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<string> GetYearByIdAsync(int? id)
        {
            if (id != null)
            {
                int Id = id.Value;
                if (Id > 0)
                {
                    if (_DbContext.Years.Where(x => x.Id == Id).Count() > 0)
                    {
                        string strName = _DbContext.Years.FirstOrDefaultAsync(x => x.Id == Id).Result.YearName.ToString();
                        return strName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<string> GetProductNameEnglishByBarcodeAsync(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                if (_DbContext.ProductInfo.Where(x => x.ProductId == barcode).Count() > 0)
                {
                    string strName = _DbContext.ProductInfo.FirstOrDefault(x => x.ProductId == barcode).ProductNameEng;
                    return strName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<string> GetProductNameArabicByBarcodeAsync(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                if (_DbContext.ProductInfo.Where(x => x.ProductId == barcode).Count() > 0)
                {
                    string strName = _DbContext.ProductInfo.FirstOrDefault(x => x.ProductId == barcode).ProductNameArabic;
                    return strName;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public async Task<int> GetSessonIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if(_DbContext.Seassons.Where(x=>x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Seassons.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetDepartmentIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Departments.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Departments.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }


        public async Task<int> GetCategoryIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Categories.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Categories.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }                
            }
            return Id;
        }


        public async Task<int> GetSkuIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Styles.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Styles.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetSizeIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Sizes.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Sizes.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetColorIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Colors.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Colors.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetUnitIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Units.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Units.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetBrandIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Brands.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Brands.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetVendorIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Vendors.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Vendors.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }

        public async Task<int> GetGroupIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                if (_DbContext.Groups.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
                {
                    Id = _DbContext.Groups.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
                }
            }
            return Id;
        }


        public async Task<int> GetYearIdByNameAsync(string str)
        {
            int Id = 0;
            if (str != null)
            {
                int yearName = int.Parse(str);
                if (_DbContext.Years.Where(x => x.YearName == yearName).Count() > 0)
                {
                    Id = _DbContext.Years.FirstOrDefault(x => x.YearName == yearName).Id;
                }
            }
            return Id;
        }


        public async Task<long> GetCurrentProductIdByBarcodeAsync(string str)
        {
            long Id = 0;
            if (str != null)
            {
                if (_DbContext.ProductInfo.Where(x => x.ProductId == str).Count() > 0)
                {
                    Id =  _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.ProductId == str).Result.Id;
                }
            }
            return Id;
        }



        public async Task<List<CategoryViewModel>> GetCategoryHierarchy(int parentId, List<CategoryViewModel> model, List<CategoryViewModel> category, int level = 0)
        {
            string str = "---";

            var children = GetCategoriesAsync().Result.Where(x => x.ParentCategoryId == parentId).ToList();

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
            var categoryInfo = GetCategoryByIdAsync(categoryId).Result;

            if (categoryInfo.ParentCategoryId > 0)
            {
                GetLevel(categoryInfo.ParentCategoryId, level++);
            }

            return level;

        }


        public async Task<int> CheckCreateGetProductSku(string str)
        {
            int Id = 0;
            if (_DbContext.Styles.Where(x => x.Code == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Styles.FirstOrDefault(x => x.Code == str.ToLower()).Id;
            }
            else
            {
                var s = new Styles();
                s.NameEng = str;
                s.NameArabic= str;
                s.Code = str;

                _DbContext.Styles.Add(s);
                _DbContext.SaveChanges();
                Id= s.Id;
            }
            return Id;
        }


        public async Task<int> CheckCreateGetProductCategory(string str)
        {
            int Id = 0;
            if (_DbContext.Categories.Where(x => x.NameEng == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Categories.FirstOrDefault(x => x.NameEng == str.ToLower()).Id;
            }
            else
            {
                var s = new Categories();
                s.NameEng = str;
                s.NameArabic = str;
                s.ParentCategoryId = 0;

                _DbContext.Categories.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }


        public async Task<int> CheckCreateGetProductDepartment(string str)
        {
            int Id = 0;
            if (_DbContext.Departments.Where(x =>x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Departments.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Departments();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Departments.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductSeasson(string str)
        {
            int Id = 0;
            if (_DbContext.Seassons.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Seassons.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Seassons();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Seassons.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }


        public async Task<int> CheckCreateGetProductSize(string str)
        {
            int Id = 0;
            if (_DbContext.Sizes.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Sizes.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Sizes();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Sizes.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductColor(string str)
        {
            int Id = 0;
            if (_DbContext.Colors.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Colors.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Colors();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Colors.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }


        public async Task<int> CheckCreateGetProductYear(string str)
        {
            int Id = 0;
            int yearx = int.Parse(str);
            if (_DbContext.Years.Where(x => x.YearName == yearx).Count() > 0)
            {
                Id = _DbContext.Years.FirstOrDefault(x =>x.YearName==yearx).Id;
            }
            else
            {
                var s = new Years();
                s.YearName = yearx;

                _DbContext.Years.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductUnit(string str)
        {
            int Id = 0;
            if (_DbContext.Units.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Units.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Units();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Units.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductbrand(string str)
        {
            int Id = 0;
            if (_DbContext.Brands.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Brands.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Brands();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Brands.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductVendor(string str)
        {
            int Id = 0;
            if (_DbContext.Vendors.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Vendors.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Vendors();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Vendors.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<int> CheckCreateGetProductGroup(string str)
        {
            int Id = 0;
            if (_DbContext.Groups.Where(x => x.NameEng.ToLower() == str.ToLower()).Count() > 0)
            {
                Id = _DbContext.Groups.FirstOrDefault(x => x.NameEng.ToLower() == str.ToLower()).Id;
            }
            else
            {
                var s = new Groups();
                s.NameEng = str;
                s.NameArabic = str;

                _DbContext.Groups.Add(s);
                _DbContext.SaveChanges();
                Id = s.Id;
            }
            return Id;
        }

        public async Task<string> ProductImageById(long id)
        {
            string imgName = "";

            var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.Id == id);
            imgName = productInfo.Image;

            return imgName;
        }

        public async Task<ProductInfo> GetProductByBarcode(string barCode)
        {
            var productInfo = await _DbContext.ProductInfo.FirstOrDefaultAsync(x => x.ProductId == barCode);
            return productInfo;
        }

        public async Task<ProductInfo> UpdateProduct(ProductInfo model)
        {
            try
            {
                _DbContext.Update(model);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return model;
        }
    }
}
