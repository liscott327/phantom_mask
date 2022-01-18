using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phantom_mask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;
        private readonly IPharmacyRepository _pharmacy;

        public QueryController(ILogger<QueryController> logger, IPharmacyRepository pharmacy)
        {
            _logger = logger;
            _pharmacy = pharmacy;
        }

        /// <summary>
        /// 列出特定時間有開放的藥局
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        //[HttpGet("GetPharmacyList")]
        public string GetPharmacyList(string startTime, string endTime, string weekDay)
        {
            var allStore = _pharmacy.GetAll();






            //JsonResult result = new JsonResult();

            //return result;
            return "OK";
        }
    }
}
