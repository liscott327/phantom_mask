using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using phantom_Inventory.Repositories;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System.Collections.Generic;
using System.Linq;
using static phantom_mask.Models.ImportData;

namespace phantom_mask.Controllers
{
    [Route("[controller]")]
    public class ImportController : Controller
    {
        private readonly Context _context;
        private readonly IPharmacyRepository _pharmacy;
        private readonly IPurchaseHistoryRepository _purchaseHistory;
        private readonly IInventoryRepository _inventory;
        private readonly IMaskRepository _mask;
        private readonly IUserRepository _user;

        public ImportController(Context context,
                                IPharmacyRepository pharmacy,
                                IPurchaseHistoryRepository purchaseHistory,
                                IInventoryRepository inventory,
                                IMaskRepository mask,
                                IUserRepository user
            )
        {
            _context = context;
            _pharmacy = pharmacy;
            _purchaseHistory = purchaseHistory;
            _inventory = inventory;
            _mask = mask;
            _user = user;
        }

        public string Index()
        {
            return "Import File First!";
        }

        [HttpPost("PharmacyData")]
        public string PharmacyData(string jsonData)
        {
            bool dataIsExisted = _pharmacy.GetAll().Any();
            //不重複匯入
            if (!dataIsExisted)
            {
                //json string轉object
                List<Pharmacies> jsonObject = JsonConvert.DeserializeObject<List<Pharmacies>>(jsonData);
                foreach (Pharmacies item in jsonObject)
                {
                    //寫入Pharmacy
                    Pharmacy pharmacy = new Pharmacy
                    {
                        Name = item.name,
                        CashBalance = item.cashBalance,
                        OpeningHours = item.openingHours,
                    };
                    _context.Pharmacy.Add(pharmacy);
                    _context.SaveChanges();

                    foreach (Good good in item.masks)
                    {
                        //如果有相同名稱的商品，僅寫入Investory，並帶MaskId
                        Mask maskData = _mask.GetByFilter(x=>x.Name == good.name).FirstOrDefault();
                        if (maskData != null)
                        {
                            //寫入Investory
                            Inventory inventory = new Inventory
                            {
                                PharmacyId = pharmacy.Id,
                                MaskId = maskData.Id,
                                Price = good.price
                            };
                            _context.Inventory.Add(inventory);
                            _context.SaveChanges();
                        }
                        else
                        {
                            //寫入Mask
                            Mask mask = new Mask
                            {
                                Name = good.name
                            };
                            _context.Mask.Add(mask);
                            _context.SaveChanges();

                            //寫入Investory
                            Inventory inventory = new Inventory
                            {
                                PharmacyId = pharmacy.Id,
                                MaskId = mask.Id,
                                Price = good.price
                            };
                            _context.Inventory.Add(inventory);
                            _context.SaveChanges();
                        }                        
                    }
                }
                return "Import Pharmacy Data Success";
            }
            else
            {
                return "Pharmacy Data is Existed";
            }
        }

        [HttpPost("ImportUserData")]
        public string ImportUserData(string jsonData)
        {


            return "Import User Data Success";
        }


    }
}
