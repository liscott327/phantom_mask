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
        /// <param name="startTime">開始時間ex.09:16</param>
        /// <param name="endTime">結束時間ex.03:27</param>
        /// <param name="weekDay">星期幾ex.Sat</param>
        /// <returns></returns>
        [HttpPost("GetPharmacyList")]
        public string GetPharmacyList([FromForm] string startTime, [FromForm] string endTime, [FromForm] string weekDay)
        {
            List<Pharmacy> pharmacyData = _pharmacy.GetAll().ToList();
            List<Store> result = new List<Store>();
            if (weekDay != null && startTime != null && endTime != null)
            {
                foreach (Pharmacy item in pharmacyData)
                {
                    Store store = new Store()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        OpeningHours = item.OpeningHours
                    };

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
                        string[] weekDayRange = weekDayStr.Contains("-") == true ? weekDayStr.Split("-") : null;
                        //個別天
                        string[] weekDays = weekDayStr.Contains(",") == true ? weekDayStr.Split(",") : null;

                        //若3個參數都有填
                        if (weekDay != null && startTime != null && endTime != null)
                        {

                            bool passTimeQuery = false;            //通過時間區間查詢
                            bool passWeekDayQuery = false;         //通過星期幾查詢
                                                                   //判斷時間區間
                            if (startTime != null && endTime != null)
                            {
                                if (DateTime.Parse(time[0]) <= DateTime.Parse(startTime) && DateTime.Parse(time[1]) >= DateTime.Parse(endTime))
                                {
                                    passTimeQuery = true;
                                }
                            }

                            int queryWeekDay = TransWeekDayToNum(weekDay);              //查詢的星期幾
                                                                                        //判斷區間類型
                            if (weekDayRange != null)
                            {
                                //先將英文星期轉成數字比較
                                int startWeekDay = TransWeekDayToNum(weekDayRange[0]);
                                int endWeekDay = TransWeekDayToNum(weekDayRange[1]);

                                if (startWeekDay <= queryWeekDay && endWeekDay >= queryWeekDay)
                                {
                                    passWeekDayQuery = true;
                                }
                            }
                            //陣列類型
                            if (weekDays != null)
                            {
                                foreach (var day in weekDays)
                                {
                                    if (day == weekDay)
                                    {
                                        passWeekDayQuery = true;
                                    }
                                }
                            }
                            //單一天類型
                            if (TransWeekDayToNum(weekDayStr) == queryWeekDay)
                            {
                                passWeekDayQuery = true;
                            }

                            //判斷時間區間
                            if (startTime != null && endTime != null)
                            {
                                if (DateTime.Parse(time[0]) <= DateTime.Parse(startTime) && DateTime.Parse(time[1]) >= DateTime.Parse(endTime))
                                {
                                    passTimeQuery = true;
                                }
                            }

                            if (passWeekDayQuery == true && passTimeQuery == true)
                            {
                                result.Add(store);
                            }
                        }                       
                    }
                }
            }
            else if (startTime != null && endTime != null)
            {
                foreach (Pharmacy item in pharmacyData)
                {
                    Store store = new Store()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        OpeningHours = item.OpeningHours
                    };

                    //將openHours解析出來
                    string[] stringArray = item.OpeningHours.Split(" / ");
                    foreach (var timeStr in stringArray)
                    {
                        var t = timeStr.Replace(" ", "");
                        //切時間(由後往前11字)
                        string timeRang = t.Substring(t.Length - 11, 11);
                        string[] time = timeRang.Split("-");

                        //判斷時間區間
                        if (startTime != null && endTime != null)
                        {
                            if (DateTime.Parse(time[0]) <= DateTime.Parse(startTime) && DateTime.Parse(time[1]) >= DateTime.Parse(endTime))
                            {
                                result.Add(store);
                            }
                        }
                    }
                }
            }
            else if (weekDay != null)
            {
                foreach (Pharmacy item in pharmacyData)
                {
                    Store store = new Store()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        OpeningHours = item.OpeningHours
                    };

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
                        string[] weekDayRange = weekDayStr.Contains("-") == true ? weekDayStr.Split("-") : null;
                        //個別天
                        string[] weekDays = weekDayStr.Contains(",") == true ? weekDayStr.Split(",") : null;

                        bool passWeekDayQuery = false;         //通過星期幾查詢
                        //判斷
                        int queryWeekDay = TransWeekDayToNum(weekDay);              //查詢的星期幾
                                                                                    //判斷區間類型
                        if (weekDayRange != null)
                        {
                            //先將英文星期轉成數字比較
                            int startWeekDay = TransWeekDayToNum(weekDayRange[0]);
                            int endWeekDay = TransWeekDayToNum(weekDayRange[1]);

                            if (startWeekDay <= queryWeekDay && endWeekDay >= queryWeekDay)
                            {
                                passWeekDayQuery = true;
                            }
                        }
                        //陣列類型
                        if (weekDays != null)
                        {
                            foreach (var day in weekDays)
                            {
                                if (day == weekDay)
                                {
                                    passWeekDayQuery = true;
                                }
                            }
                        }
                        //單一天類型
                        if (TransWeekDayToNum(weekDayStr) == queryWeekDay)
                        {
                            passWeekDayQuery = true;
                        }

                        if (passWeekDayQuery == true)
                        {
                            result.Add(store);
                        }
                    }
                }
            }
            else
            {
                return "參數有誤";
            }

            string jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }

        /// <summary>
        /// 轉星期簡寫英文為數字
        /// </summary>
        /// <param name="weekDay"></param>
        /// <returns></returns>
        public int TransWeekDayToNum(string weekDay)
        {
            int dayNum = 0;
            switch (weekDay)
            {
                case "Mon":
                    dayNum = 1;
                    break;
                case "Tue":
                    dayNum = 2;
                    break;
                case "Wed":
                    dayNum = 3;
                    break;
                case "Thu":
                    dayNum = 4;
                    break;
                case "Fri":
                    dayNum = 5;
                    break;
                case "Sat":
                    dayNum = 6;
                    break;
                case "Sun":
                    dayNum = 7;
                    break;
            }
            return dayNum;
        }



        /// <summary>
        /// 用藥局名稱查詢該店所有口罩
        /// (List all masks that are sold by a given pharmacy, sorted by mask name or mask price)
        /// </summary>
        /// <param name="pharmacyName">藥局名稱</param>
        /// <param name="orderBy">排序項目</param>
        /// <returns></returns>
        [HttpPost("GetGoodList")]
        public string GetGoodList([FromForm] string pharmacyName, [FromForm] string orderBy)
        {
            List<Inventory> inventoryData = _inventory.GetAll()
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
                    Name = item.Mask.Name,
                    Price = item.Price,
                };
                result.Add(goodDetail);
            }
            string jsonString = JsonConvert.SerializeObject(result);

            return jsonString;
        }

        //列出在一個價格範圍內擁有多於或少於 x 個面膜產品的所有藥店

        /// <summary>
        /// 查詢日期範圍內交易總額排名前x名使用者
        /// (The top x users by total transaction amount of masks within a date range)
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="resultCount">顯示前幾項資料</param>
        /// <returns></returns>
        [HttpPost("GetTopOfTransaction")]

        public string GetTopOfTransaction([FromForm] DateTime startDate, [FromForm] DateTime endDate, [FromForm] int resultCount)
        {
            var purchaseHistoryData = _purchaseHistory.GetAll()
                .Include(x => x.User)
                .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate);

            List<TransactionAmountRank> result = new List<TransactionAmountRank>();

            if (purchaseHistoryData != null)
            {
                var calcTransactionAmount = purchaseHistoryData
                    .GroupBy(x => new
                    {
                        id = x.User.Id,
                        name = x.User.Name,
                    })
                    .Select(a => new
                    {
                        Id = a.Key.id,
                        Name = a.Key.name,
                        TotalTransactionAmount = a.Sum(b => b.TransactionAmount),
                    })
                    .OrderByDescending(y => y.TotalTransactionAmount)
                    .ToList();

                //若要顯示的數量超過查詢結果的資料數
                resultCount = resultCount > purchaseHistoryData.Count() ? purchaseHistoryData.Count() : resultCount;

                for (int i = 0; i < resultCount; i++)
                {
                    var item = calcTransactionAmount[i];

                    TransactionAmountRank transactionAmountRank = new TransactionAmountRank()
                    {
                        UserId = item.Id,
                        Name = item.Name,
                        TotalTransactionAmount = item.TotalTransactionAmount
                    };
                    result.Add(transactionAmountRank);
                }
            }
            string jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }

        /// <summary>
        /// 查詢某日期區間所交易的口罩及交易總額
        /// (The total amount of masks and dollar value of transactions that happened within a date range)
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns></returns>
        [HttpPost("GetMaskTotalTransaction")]
        public string GetMaskTotalTransaction([FromForm] DateTime startDate, [FromForm] DateTime endDate)
        {
            var purchaseHistoryData = _purchaseHistory.GetAll()
                .Include(x => x.Mask)
                .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate);
            List<MaskTotalTransaction> result = new List<MaskTotalTransaction>();

            if (purchaseHistoryData != null)
            {
                result = purchaseHistoryData
                    .GroupBy(x => new
                    {
                        id = x.MaskId,
                        name = x.Mask.Name,
                    })
                    .Select(a => new MaskTotalTransaction
                    {
                        MaskId = a.Key.id,
                        Name = a.Key.name,
                        TotalTransactionAmount = a.Sum(b => b.TransactionAmount),
                    })
                    .ToList();
            }
            string jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }

        /// <summary>
        /// Search for pharmacies or masks by name, ranked by relevance to search term
        /// </summary>
        /// <param name="searchBy">查詢類型</param>
        /// <param name="keyword">查詢名稱</param>
        /// <returns></returns>
        [HttpPost("SearchName")]
        public string SearchName([FromForm] string searchBy, [FromForm] string keyword)
        {
            if (string.IsNullOrEmpty(searchBy) == true)
            {
                return "尚未填寫查詢類型";
            }
            List<SearchResult> result = new List<SearchResult>();
            if (searchBy == "mask")
            {
                List<Mask> maskData = _mask.GetByFilter(x => x.Name.ToUpper().Contains(keyword.ToUpper())).ToList();
                if (maskData != null)
                {
                    foreach (Mask item in maskData)
                    {
                        SearchResult searchResult = new SearchResult()
                        {
                            Id = item.Id,
                            Name = item.Name,
                        };
                        result.Add(searchResult);
                    };
                }
            }
            else
            {
                List<Pharmacy> pharmacyData = _pharmacy.GetByFilter(x => x.Name.ToUpper().Contains(keyword.ToUpper())).ToList();
                if (pharmacyData != null)
                {
                    foreach (Pharmacy item in pharmacyData)
                    {
                        SearchResult searchResult = new SearchResult()
                        {
                            Id = item.Id,
                            Name = item.Name,
                        };
                        result.Add(searchResult);
                    };
                }
            }
            string jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }


        /// <summary>
        /// Process a user purchases a mask from a pharmacy, and handle all relevant data changes in an atomic transaction
        /// </summary>
        /// <param name="userName">使用者名稱(購買人)</param>
        /// <param name="pharmacyName">藥局名稱</param>
        /// <param name="maskName">口罩名稱</param>
        /// <returns></returns>
        [HttpPost("Purchase")]
        public string Purchase([FromForm] string userName, [FromForm] string pharmacyName, [FromForm] string maskName)
        {
            if (string.IsNullOrEmpty(userName) == true || string.IsNullOrEmpty(pharmacyName) == true || string.IsNullOrEmpty(maskName) == true)
            {
                return "錯誤，尚有欄位未填寫";
            }

            User user = _user.GetByFilter(x => x.Name == userName).FirstOrDefault();
            if (user == null)
            {
                return "錯誤，無法找到該user";
            }

            Inventory inventoryData = _inventory.GetAll()
                .Include(x => x.Pharmacy)
                .Include(x => x.Mask)
                .Where(x => x.Pharmacy.Name == pharmacyName && x.Mask.Name == maskName)
                .FirstOrDefault();
            if (inventoryData == null)
            {
                return "錯誤，無法找到該藥局販售此項目";
            }

            //更新user
            user.CashBalance = user.CashBalance - inventoryData.Price;
            _user.Update(user);
            _user.Save();

            //更新pharmacy
            inventoryData.Pharmacy.CashBalance = inventoryData.Pharmacy.CashBalance + inventoryData.Price;
            _pharmacy.Update(inventoryData.Pharmacy);
            _pharmacy.Save();

            //寫入purchaseHistory
            PurchaseHistory purchaseHistory = new PurchaseHistory()
            {
                UserId = user.Id,
                PharmacyId = inventoryData.PharmacyId,
                MaskId = inventoryData.MaskId,
                TransactionAmount = inventoryData.Price
            };
            _purchaseHistory.Add(purchaseHistory);
            _purchaseHistory.Save();

            return "付款成功";
        }



    }
}
