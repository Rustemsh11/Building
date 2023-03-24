using AutoMapper;
using Building.BLL.Services.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;


namespace Building.Controllers
{
    public class Snab : Controller
    {
        private readonly IBuildingSiteService buildingSiteService;
        private readonly IMapper mapper;
        private readonly IQueryService queryService;
        private readonly IEmployeeService employeeService;
        private readonly IQueryDetailsService queryDetailsService;
        public Snab(IBuildingSiteService buildingSiteService, IQueryService queryService, IEmployeeService employeeService, IMapper mapper, IQueryDetailsService queryDetailsService)
        {
            this.buildingSiteService = buildingSiteService;
            this.queryService = queryService;
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.queryDetailsService = queryDetailsService;
        }



        /// <summary>
        /// get all site for the choosen
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Main()
        {
            var response = await buildingSiteService.GetAll();

            return View(response.Data);
        }

        /// <summary>
        /// get query for building site
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetQuery(int id, string search)
        {
            TempData["id"] = id;
            var response = queryService.GetBuildingQuery(id).Data;

            ViewBag.Search = search;  //save searching query in input
            if (!String.IsNullOrEmpty(search))
            {
                response = response.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); //search query 
            }

            return View(response);
        }

        /// <summary>
        /// take query and change state of query and query detail
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> TakeQuery(IFormCollection formCollection)
        {
            string getid = formCollection["queryId"];
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var query = await queryService.GetQuery(int.Parse(id));
                    query.Data.SnabId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                    query.Data.State = "Обработан";

                    var queryDetail = await queryDetailsService.GetQuery(query.Data.QueryId);
                    foreach (var item in queryDetail.Data)
                    {
                        item.State = "Заявка принята";

                    }
                    await queryService.UpdateQuery(query.Data);
                }
            }

            return RedirectToAction("GetQuery", new { id = TempData["id"] });

        }


        [HttpGet]
        public async Task<IActionResult> GetSnabQueries(int id, string search, SortState sortOrder = SortState.NameAsc)
        {
            var resp = queryService.GetSnabQuery(id).Data;

            ViewBag.Search = search;  //save searching query in input
            if (!String.IsNullOrEmpty(search))
            {
                resp = resp.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); //search query 
            }
            resp = Sort(resp, sortOrder);
            return View(resp);
        }



        [HttpGet]
        public async Task<IActionResult> GetQueryDetail(string id)
        {
            var response = await queryDetailsService.GetQuery(Convert.ToInt32(id));
            TempData["id"] = id;
            return View(response.Data);
        }


        [HttpPost]
        public async Task<IActionResult> SendToAgreement(int id)
        {
            var query = await queryService.GetQuery(id);
            if (query != null)
            {
                query.Data.Status = "На согласовании";
                await queryService.UpdateQuery(query.Data);

            }
            return RedirectToAction("GetQuery", new { id = TempData["id"] });
        }

        [HttpPost]
        public async Task<IActionResult> StateDelivery(IFormCollection formCollection)
        {
            string getid = formCollection["queryDetailId"];
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var queryDetail = await queryDetailsService.GetDetailOfQuery(int.Parse(id));
                    queryDetail.Data.State = "Заявка доставляется";
                    await queryDetailsService.Update(queryDetail.Data);
                }
            }


            return RedirectToAction("GetQueryDetail", new { id = TempData["id"] });

        }


        [HttpPost]
        public async Task<IActionResult> StateFindedSuplyer(IFormCollection formCollection)
        {
            string getid = formCollection["queryDetailId"];
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var queryDetail = await queryDetailsService.GetDetailOfQuery(int.Parse(id));
                    queryDetail.Data.State = "Найден поставщик";
                    await queryDetailsService.Update(queryDetail.Data);
                }
            }


            return RedirectToAction("GetQueryDetail", new { id = TempData["id"] });

        }


        [HttpPost]
        public async Task<IActionResult> StateDelivered(IFormCollection formCollection)
        {
            
            string getid = formCollection["queryDetailId"];
            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var queryDetail = await queryDetailsService.GetDetailOfQuery(int.Parse(id));
                    queryDetail.Data.State = "Заявка доставлена";
                    queryDetail.Data.DeliveryDate = DateTime.Now.ToShortDateString();
                    await queryDetailsService.Update(queryDetail.Data);
                }
                string ids2 = (string)TempData["id"];
                var queryDetails = await queryDetailsService.GetQuery(Convert.ToInt32(ids2));
                if(queryDetails.Data.All(c => c.State == "Заявка доставлена" || c.State== "Опровергнут"))
                {
                    var query = await queryService.GetQuery(Convert.ToInt32(ids2));
                    query.Data.State = "Выполнен";
                    await queryService.UpdateQuery(query.Data);
                }
            }

            return RedirectToAction("GetQueryDetail", new { id = TempData["id"] });


        }

        [HttpGet]
        public async Task<IActionResult> FindSuplyer(string catalogName, string materialName, int queryId)
        {
            ViewBag.QueryId = queryId;
            var link = StroyPriceParsing.GetParsingLink(catalogName,materialName);
            string getPage = StroyPriceParsing.GetStroyPricePage(link);
            var list = StroyPriceParsing.ParsTovar(getPage);
            return View(list);
        }
         
        public async Task<IActionResult> CreateDoc(int id)
        {
            var queryDetails = await queryDetailsService.GetDeliveredBySnab(id, "Заявка принята");
            TempData["snabId"] = id;
            
            var docInfo = new WordDocument();
            docInfo.QueryDetails = queryDetails.Data;

            return View(docInfo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoc(IFormCollection formCollection)
        {
            WordDocument doc = new WordDocument();
            List<QueryDetail> queryDetails = new List<QueryDetail>();

            var snab = await employeeService.ProfileInfo(Convert.ToInt32(TempData["snabId"]));

            string getid = formCollection["queryDetailId"];
            string companyName = formCollection["SuplyerName"];
            string inn = formCollection["SuplyerINN"];
            string kpp = formCollection["SuplyerKPP"];
            string responseDate = formCollection["ResponseDate"];

            if (getid != null)
            {
                string[] ids = getid.Split(new char[] { ',' });

                foreach (var id in ids)
                {
                    var queryDetail = await queryDetailsService.GetQueryDetailsList(int.Parse(id));                    
                    queryDetails.Add(queryDetail.Data.First());
                }
                var buildingInfo = await buildingSiteService.GetBuildingInfo(queryDetails.First().Query.SiteId);
                doc.SuplyerName = companyName;
                doc.SuplyerINN = inn;
                doc.SuplyerKPP = kpp;
                doc.SnabName = snab.Data.SecondName + snab.Data.Name;
                doc.SnabPhone = snab.Data.Phone;
                doc.ResponseDate = responseDate;
                doc.QueryDetails = queryDetails;
                doc.BuildingName = buildingInfo.Data.Name;
                doc.BuildingAddress = buildingInfo.Data.Address;
                doc.Create();
            }
            
            return RedirectToAction("CreateDoc");
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveredOrder(int id, SortState sortOrder = SortState.NameAsc)
        {
            var resp = await queryDetailsService.GetDeliveredBySnab(id, "Подтверждено");

            //resp = Sort(resp.Data, sortOrder);

            return View(resp.Data);

        }
        public async Task<IActionResult> GetRefuteOrder(int id, SortState sortOrder = SortState.NameAsc)
        {
            var resp = await queryDetailsService.GetDeliveredBySnab(id, "Опровергнут");

            //resp = Sort(resp.Data, sortOrder);

            return View(resp.Data);

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

        public async Task<IActionResult> ObjectInfo()
        {
            var response = await buildingSiteService.GetAll();

            return View(response.Data);
        }

        private IEnumerable<Query> Sort(IEnumerable<Query> query, SortState sortOrder = SortState.NameAsc)
        {
            ViewBag.NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.DataSort = sortOrder == SortState.DataAsc ? SortState.DataDesc : SortState.DataAsc;



            switch (sortOrder)
            {
                case SortState.NameDesc:
                    query = query.OrderByDescending(s => s.Name);
                    break;
                case SortState.DataAsc:
                    query = query.OrderBy(s => s.DeliveryDate);
                    break;
                case SortState.DataDesc:
                    query = query.OrderByDescending(s => s.DeliveryDate);
                    break;
                default:
                    query = query.OrderBy(s => s.Name);
                    break;
            }
            return query;
        }


    }
}
