using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using phantom_Inventory.Repositories;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static phantom_mask.Models.Query;

namespace phantom_mask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;
        private readonly Context _context;
        private readonly IPharmacyRepository _pharmacy;
        private readonly IPurchaseHistoryRepository _purchaseHistory;
        private readonly IInventoryRepository _inventory;
        private readonly IMaskRepository _mask;
        private readonly IUserRepository _user;
        public QueryController(ILogger<QueryController> logger,
                               Context context,
                               IPharmacyRepository pharmacy,
                               IPurchaseHistoryRepository purchaseHistory,
                               IInventoryRepository inventory,
                               IMaskRepository mask,
                               IUserRepository user
            )
        {
            _logger = logger;
            _context = context;
            _pharmacy = pharmacy;
            _purchaseHistory = purchaseHistory;
            _inventory = inventory;
            _mask = mask;
            _user = user;
        }

        [HttpGet]
        public string Index()
        {
            return "查詢GetPharmacyList";
        }


        /// <summary>
        /// 列出特定時間有開放的藥局
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        [HttpPost("GetPharmacyList")]
        public string GetPharmacyList([FromForm] string startTime, [FromForm] string endTime, [FromForm] string weekDay)
        {
            List<Pharmacy> pharmacyData = _pharmacy.GetAll().ToList();
            List<Store> result = new List<Store>();
            foreach (Pharmacy item in pharmacyData)
            {
                //將openHours解析出來
                string[] stringArray = item.OpeningHours.Split(" / ");
                foreach (var timeStr in stringArray)
                {
                    var t = timeStr.Replace(" ", "");
                    //切時間(由後往前11字)
                    string timeRang = t.Substring(t.Length - 11, 11);
                    string[] time = timeRang.Split("-");

                    //切星期
                    string weekDayStr = t.Split(timeRang).FirstOrDefault();
                    //Wed、Tue-Fri、Sun,Mon
                    //區間
                    var weekDayRange = weekDayStr.Contains("-") == true ? weekDayStr.Split("-") : null;
                    //個別天
                    var weekDays = weekDayStr.Contains(",") == true ? weekDayStr.Split(",") : null;

                    Store store = new Store()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        OpeningHours = item.OpeningHours
                    };

                    //TODO:需完成判斷
                    if (startTime != null && endTime != null)
                    {
                        if (DateTime.Parse(time[0]) <= DateTime.Parse(startTime) && DateTime.Parse(time[1]) >= DateTime.Parse(endTime))
                        {
                            result.Add(store);
                        }
                    }

                    if (weekDay != null)
                    {
                        if (weekDayRange != null)
                        {

                        }
                        if (weekDays != null)
                        {
                            foreach (var day in weekDays)
                            {
                                if (day == weekDay)
                                {
                                    result.Add(store);
                                }
                            }
                        }
                    }
                }

            }
            var jsonString = JsonConvert.SerializeObject(result);

            return jsonString;
        }



        /// <summary>
        /// 用藥局名稱查詢該店所有口罩
        /// </summary>
        /// <param name="pharmacyName">藥局名稱</param>
        /// <param name="orderBy">排序項目</param>
        /// <returns></returns>
        [HttpPost("GetGoodList")]
        public string GetGoodList([FromForm] string pharmacyName, [FromForm] string orderBy)
        {
            var inventoryData = _inventory.GetAll()
                .Include(x => x.Pharmacy)
                .Include(x => x.Mask)
                .Where(x => x.Pharmacy.Name == pharmacyName)
                .ToList();

            //依照排序
            switch (orderBy)
            {
                case "name":
                    inventoryData = inventoryData.OrderBy(x => x.Mask.Name).ToList();
                    break;
                case "price":
                    inventoryData = inventoryData.OrderBy(x => x.Price).ToList();
                    break;
                default:
                    inventoryData = inventoryData.OrderBy(x => x.Mask.Name).ToList();
                    break;
            }

            List<GoodDetail> result = new List<GoodDetail>();
            foreach (Inventory item in inventoryData)
            {
                GoodDetail goodDetail = new GoodDetail()
                {
                    Id = item.MaskId,
                    Name=item.Mask.Name,
                    Price=item.Price,
                };
                result.Add(goodDetail);
            }
            var jsonString = JsonConvert.SerializeObject(result);

            return jsonString;
        }

    }
}
