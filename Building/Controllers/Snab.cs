using AutoMapper;
using Building.BLL.Services.Implementations;
using Building.BLL.Services.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Building.Domain.ViewModel;
using Building.Helper;
using Building.Helper.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IMaterialService materialService;
        private readonly Email _email;
        public Snab(IBuildingSiteService buildingSiteService, IQueryService queryService, IEmployeeService employeeService, IMapper mapper, IQueryDetailsService queryDetailsService, Email email, IMaterialService materialService)
        {
            this.buildingSiteService = buildingSiteService;
            this.queryService = queryService;
            this.employeeService = employeeService;
            this.mapper = mapper;
            this.queryDetailsService = queryDetailsService;
            _email = email;
            this.materialService = materialService;
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
        public async Task<IActionResult> CreateOrder()
        {
            ViewBag.Catalog = new SelectList(materialService.GetCatalog().Data, "CatalogID", "Name");
            var buildingList = await buildingSiteService.GetAll();
            ViewBag.Building = new SelectList(buildingList.Data, "BuildingId", "Name");
            var emptyList = new List<QueryViewModel>();

            return View(emptyList);
        }
        [HttpGet]
        public JsonResult LoadMaterial(int id)
        {
            var state = materialService.GetMaterials(id).Data;
            return Json(new SelectList(state, "MaterialId", "Name"));
        }
        [HttpPost]
        public async Task<JsonResult> CreateQuery(IEnumerable<QueryViewModel> queries)
        {
            if (queries.Count() != 0)
            {
                
                var siteId = buildingSiteService.GetIdByName(queries.First().Site).Data;
                Query query = new Query();
                query.Name = string.Format("Моя заявка от {0}", DateTime.Now);                
                query.SiteId = siteId;
                query.State = "Заявка принята";
                query.Status = "Не согласовано";
                query.Deadline = queries.First().Deadline.ToString();
                query.CreateDate = DateTime.Now.ToString();
                query.SnabId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

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
                    queryDetail.State = "Обработан";
                    queryDetail.CreateDate = DateTime.Now.ToShortDateString();
                    queryDetailsService.Create(queryDetail);

                }

                return Json("Заявка успешно создана");


            }
            return Json("Добавьте записи");
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
        private async void CreateDocument(IFormCollection formCollection)
        {
            
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
            bool isSendEmail = bool.Parse(formCollection["selected"]);

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


                if (isSendEmail)
                {
                    EmailData emailData = new EmailData()
                    {
                        CompanyName = companyName,
                        Recipient = formCollection["Recipient"],
                        Attachment = doc.DocumentPath
                    };
                    _email.CreateEmail(emailData);
                }
                MemoryStream memory = new MemoryStream();
                using(var stream = new FileStream(doc.DocumentPath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "QueryOrder.docx");
            }
            return RedirectToAction("CreateDoc");
        }
        [HttpPost]
        public async Task<IActionResult> CreateDocAndSend(IFormCollection formCollection)
        {
            CreateDocument(formCollection);
            
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
