using AutoMapper;
using Building.BLL.Services.Interfaces;
using Building.Domain.Entity;
using Building.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Building.Controllers
{
    public class Director : Controller
    {
        private readonly IBuildingSiteService buildingSiteService;
        private readonly IMapper mapper;
        private readonly IQueryService queryService;
        private readonly IEmployeeService employeeService;
        public Director(IBuildingSiteService buildingSiteService, IQueryService queryService, IEmployeeService employeeService, IMapper mapper)
        {
            this.buildingSiteService = buildingSiteService;
            this.queryService = queryService;
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

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
            TempData["currenturl"] = JsonConvert.SerializeObject(HttpContext.Request.Path);
            var response = queryService.GetBuildingQuery(id).Data;

            ViewBag.Search = search;  //save searching query in input
            if (!String.IsNullOrEmpty(search))
            {
                //response = response.Where(x => x.QueryDetails.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); //search query 
            }

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetNoAgreementQuery(string search)
        {

            var response = queryService.GetNoAgreement().Data;
            ViewBag.Search = search;  //save searching query in input
            if (!String.IsNullOrEmpty(search))
            {
             //   response = response.Where(x => x.Material.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList(); //search query 
            }

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> PointAgreement(int id)
        {

            var query = await queryService.GetQuery(id);
            if (query != null)
            {
                query.Data.Status = "Согласовано";
                await queryService.UpdateQuery(query.Data);

            }
            return Redirect(TempData["currenturl"].ToString());
            //return RedirectToAction("GetQuery", new { id = TempData["id"] });
        }

        [HttpGet]
        public IActionResult GetMpz(int id, SortState sortOrder = SortState.DataDesc)
        {
            var response = queryService.GetMPZ(id).Data;
            ViewBag.NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.DataSort = sortOrder == SortState.DataDesc ? SortState.DataAsc : SortState.DataDesc;
            ViewBag.SnabSort = sortOrder == SortState.SnabDesc ? SortState.SnabAsc : SortState.SnabDesc;



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

            return View(response);

        }
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
    }
}
