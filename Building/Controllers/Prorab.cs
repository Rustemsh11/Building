using AutoMapper;
using Building.BLL.Services.Interfaces;
using Building.DAL;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;


namespace Building.Controllers
{
    [Authorize]
    public class Prorab : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IQueryService queryService;
        private readonly IMaterialService materialService;
        private readonly IMapper mapper;
        private readonly IQueryDetailsService queryDetailsService;
        private readonly IBuildingSiteService buildingSiteService;
        private readonly ICatalogService catalogService;


        public Prorab(IEmployeeService employeeService, IMapper mapper,
                        IQueryService queryService, IMaterialService materialService,
                        IQueryDetailsService queryDetailsService, IBuildingSiteService buildingSiteService,
                        ICatalogService catalogService,
                        BuildingContext building)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.queryService = queryService;
            this.materialService = materialService;
            this.queryDetailsService = queryDetailsService;
            this.buildingSiteService = buildingSiteService;
            this.catalogService = catalogService;
        }

        /// <summary>
        /// get main page with all query of employee
        /// </summary>

        [Route("Prorab/Main")]
        public async Task<IActionResult> Main(string search, SortState sortOrder = SortState.DataDesc)
        {
            int id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value); // get id user

           
            var response = queryService.GetEmployeeQuery(id).Data; // get all query of employee


            ViewBag.Search = search;  //save searching query in input
            if (!String.IsNullOrEmpty(search))
            {
                response = response.Where(x => x.QueryDetails.First().Material.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); //search query 
            }
            
            response = Sort(response, sortOrder);
            ViewBag.NotViewedQueries = await GetCountNotViewedQueries(id, "Заявка доставлена");

            return View(response);
        }







        /// <summary>
        /// action for autocomplete search. Get material name 
        /// </summary>
        /// <returns></returns>
        [Produces("application/json")] ///Delete!!!!
        public async Task<IActionResult> Search()
        {
            string term = HttpContext.Request.Query["term"].ToString();
            var material = materialService.GetMaterial(term);
            return Ok(material.Data);
        }


        
        /// <summary>
        /// create query
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.Catalog =  new SelectList(materialService.GetCatalog().Data,"CatalogID", "Name");
            
            var emptyList = new List<QueryViewModel>();    

            return View(emptyList);
        }
        

        [HttpGet]
        public JsonResult LoadMaterial(int id)
        {
            var state = materialService.GetMaterials(id).Data;
            return Json(new SelectList(state, "MaterialId", "Name"));
        }

        /// <summary>
        /// create query and save query details
        /// </summary>          
        int i = 1;
        [HttpPost]
        public async Task CreateQuery(IEnumerable<QueryViewModel> queries)
        {
            if (queries.Count() != 0)
            {
                int prorabId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var siteId = await employeeService.GetEmployeeSite(prorabId);

                Query query = new Query();
                query.Name = string.Format("Заявка от {0} ({1})", DateTime.Now, i++);
                query.ProrabId = prorabId;
                query.SiteId = siteId.Data.BuildingId;
                query.State = "В работе";
                query.Status = "Не согласовано";
                query.Deadline = queries.First().Deadline.ToString();
                query.CreateDate = DateTime.Now.ToString();

                queryService.Create(query);
                foreach (var item in queries)
                {
                
                    var material = await materialService.GetIdByName(item.MaterialName);
                    QueryDetail queryDetail = new QueryDetail();
                    queryDetail.QueryId = query.QueryId;
                    queryDetail.MaterialId = material.Data.MaterialId;
                    queryDetail.CatalogId = material.Data.IdCatalog;
                    queryDetail.Name = item.Name;
                    queryDetail.Count = item.Count;
                    queryDetail.Measure = item.Measure;
                    queryDetail.Additional = item.Additional;
                    queryDetail.State = "Заявка создана";
                    queryDetail.CreateDate= DateTime.Now.ToShortDateString();
                    queryDetailsService.Create(queryDetail);

                }
                
                


            }
            
        }


        #region Save Photo(Delete)
        //        public async Task<IActionResult> CreateQuery(List<Query> query, List<IFormFile> photo)
        //{
        //    return RedirectToAction("Main");
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateQuery(Dictionary<int, string> value, Query query, List<IFormFile> postedFiles)
        //{

        //    int id = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        //    var siteId = await employeeService.GetEmployeeSite(id);

        //    query.ProrabId = id;
        //    query.SiteId = siteId.Data.BuildingId;
        //    query.State = "Заявка создана";
        //    query.Status = "Не согласовано";
        //    queryService.Create(query);
        //    if (postedFiles != null)

        //    {
        //        if (postedFiles.Count > 0)
        //        {
        //            foreach (var item in postedFiles)
        //            {
        //                byte[] image = null;
        //                using (var fs1 = item.OpenReadStream())
        //                using (var ms1 = new MemoryStream())
        //                {
        //                    fs1.CopyTo(ms1);
        //                    image = ms1.ToArray();
        //                }
        //                query.Photo = image;

        //            }
        //        }
        //    }



        //    foreach (KeyValuePair<int, string> item in value)
        //    {

        //        QueryDetail queryDetail = new QueryDetail()
        //        {
        //            CharacteristicId = item.Key,
        //            MaterialId = query.MaterialCharacId,
        //            Value = item.Value,
        //            QueryId = query.QueryId
        //        };
        //        queryDetailsService.Create(queryDetail);

        //    }


        //    return RedirectToAction("Main");
        //}
        #endregion

        [HttpGet]
        public async Task<IActionResult> QueryDetails(string id)
        {
            var response = await queryDetailsService.GetQuery(Convert.ToInt32(id));
            return PartialView(response.Data);

        }


        /// <summary>
        /// Delete Query
        /// </summary>
        /// <param name="formCollection">Get selected query</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteQuery(IFormCollection formCollection)
        {
            string getid = formCollection["queryId"];
            string[] ids = getid.Split(new char[] { ',' });
            foreach (var id in ids)
            {
                queryService.Delete(int.Parse(id));
            }
            return RedirectToAction("Main");
        }


        [HttpGet]
        public IActionResult Update()
        {
            return RedirectToAction("Main");
        }


        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var response = await employeeService.ProfileInfo(id);
            return View(response.Data);
        }


        [HttpGet]
        public async Task<IActionResult> ProfileEdit(int id)
        {
            var response = await employeeService.ProfileInfo(id);
            return View(response.Data);
        }


        [HttpPost]
        public async Task<IActionResult> ProfileEdit(Employee emp, List<IFormFile> postedFiles)
        {
            var employee = await employeeService.GetEmployee(emp.EmployeeId);

            employee.Data.Name = emp.Name;
            employee.Data.SecondName = emp.SecondName;
            employee.Data.MiddleName = emp.MiddleName;
            employee.Data.Birthday = emp.Birthday;
            employee.Data.Phone = emp.Phone;

            if (postedFiles.Count > 0)
            {
                foreach (var item in postedFiles)
                {
                    byte[] image = null;
                    using (var fs1 = item.OpenReadStream())
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        image = ms1.ToArray();
                    }
                    employee.Data.Photo = image;

                }
            }
            await employeeService.UpdateEmployee(employee.Data);
            return RedirectToAction("Profile", new { id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value });
        }


        [HttpGet]
        public async Task<IActionResult> BuildingSite(int id)
        {
            var siteId = await employeeService.GetEmployeeSite(id);
            var resp = await buildingSiteService.GetBuildingInfo(siteId.Data.BuildingId);
            return View(resp.Data);
        }
        [HttpGet]



        public async Task<IActionResult> GetDelivered(int idProrab, SortState sortOrder = SortState.DataDesc)
        {
            //var query = queryService.GetDelivered(idProrab);
            var query = await queryDetailsService.GetDelivered(idProrab, "Заявка доставлена");
            var setViewed = await queryDetailsService.SetViewedQueryDetail(query.Data.ToList());
            //response.Where(c => c.QueryDetails.Where(c => c.State == "Заявка доставлена"));
            //var response = query.Data.Where(c => c.State == "Заявка доставлена").ToList().AsEnumerable();
            //var response = Sort(query.Data, sortOrder);

            return View(query.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmQuery(IFormCollection formCollection)
        {
            string getid = formCollection["queryId"];
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var query = await queryDetailsService.GetDetailOfQuery(int.Parse(id));                    
                    query.Data.State = "Подтверждено";
                    await queryDetailsService.Update(query.Data);
                }
            }

            return RedirectToAction("GetDelivered", new { idProrab = @User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value });
        }
        [HttpPost]
        public async Task<IActionResult> RefuteQuery(IFormCollection formCollection)
        {
            string getid = formCollection["queryId"];
            string comment = formCollection["comment"];
            
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });
                string[] comments = comment.Split(new char[] { ','});
                for (int i = 0; i < ids.Length; i++)
                {

                    var query = await queryDetailsService.GetDetailOfQuery(int.Parse(ids[i]));                    
                    query.Data.State = "Опровергнут";
                    query.Data.Comment = comments[i];
                    await queryDetailsService.Update(query.Data);

                }
            }           

            return RedirectToAction("GetDelivered", new { idProrab = @User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value });
        }

        
        private async Task<int> GetCountNotViewedQueries(int siteId, string queryState)
        {
            var responce = await queryDetailsService.GetNotViewedQueriesForProrab(siteId, queryState);
            return responce.Data;
        }
        [HttpGet]
        public async Task<ActionResult> GetMpz(int prorabId, SortState sortOrder = SortState.DataDesc)
        {

            var response = await queryDetailsService.GetDelivered(prorabId, "Подтверждено");
            //ViewBag.NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            //ViewBag.DataSort = sortOrder == SortState.DataDesc ? SortState.DataAsc : SortState.DataDesc;
            //ViewBag.SnabSort = sortOrder == SortState.SnabDesc ? SortState.SnabAsc : SortState.SnabDesc;



            //switch (sortOrder)
            //{
            //    case SortState.NameDesc:
            //        response = response.OrderByDescending(s => s.Material.Name);
            //        break;
            //    case SortState.NameAsc:
            //        response = response.OrderBy(s => s.Material.Name);
            //        break;
            //    case SortState.DataAsc:
            //        response = response.OrderBy(s => Convert.ToDateTime(s.DeliveryDate));
            //        break;
            //    case SortState.SnabAsc:
            //        response = response.OrderBy(s => s.Snab.SecondName);
            //        break;
            //    case SortState.SnabDesc:
            //        response = response.OrderByDescending(s => s.Snab.SecondName);
            //        break;
            //    default:
            //        response = response.OrderByDescending(s => s.DeliveryDate);
            //        break;
            //}

            return View(response.Data);

        }

        /// <summary>
        /// sort method
        /// </summary>
        private IEnumerable<Query> Sort(IEnumerable<Query> query, SortState sortOrder = SortState.DataDesc)
        {
            ViewBag.NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.DataSort = sortOrder == SortState.DataDesc ? SortState.DataAsc : SortState.DataDesc;



            switch (sortOrder)
            {
                case SortState.NameDesc:
                    query = query.OrderByDescending(s => s.QueryDetails.First().Material.Name);
                    break;
                case SortState.NameAsc:
                    query = query.OrderBy(s => s.QueryDetails.First().Material.Name);
                    break;
                case SortState.DataAsc:
                    query = query.OrderBy(s => Convert.ToDateTime(s.CreateDate));
                    break;
                default:
                    query = query.OrderByDescending(s => Convert.ToDateTime(s.CreateDate));
                    break;
            }
            return query;
        }
    }
}
