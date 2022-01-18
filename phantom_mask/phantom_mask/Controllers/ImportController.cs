using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using phantom_mask.Data;
using phantom_mask.Models.pharmacy;
using phantom_mask.Repositories;
using System.Collections.Generic;

namespace phantom_mask.Controllers
{
    [Route("[controller]")]
    public class ImportController : Controller
    {
        private readonly IPharmacyRepository _pharmacy;
        private readonly Context _context;

        public ImportController(IPharmacyRepository pharmacy, Context context)
        {
            _pharmacy = pharmacy;
            _context = context;

        }

        public string Index()
        {
            return "Import File First!";
        }

        [HttpPost("PharmacyData")]
        public string PharmacyData(string jsonData)
        {
            var data = _pharmacy.GetAll();
            //不重複匯入
            if (data == null)
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
                    _pharmacy.Add(pharmacy);
                    _context.SaveChanges();


                    foreach (Mask good in item.masks)
                    {

                        //寫入Mask



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


        public class Pharmacies
        {
            public string name { get; set; }
            public decimal cashBalance { get; set; }
            public string openingHours { get; set; }
            public List<Mask> masks { get; set; }
        }

        public class Mask
        {
            public string name { get; set; }
            public decimal price { get; set; }
        }
    }
}
