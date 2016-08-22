using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComputerStore.Models;
using ComputerStore.ViewModel;
using ComputerStore.DAL;
using System.Data;
using System.Data.Entity;

namespace ComputerStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("home");
        }

        public ActionResult GetitemsReqAsync()
        {
            InventoryDAL inventorydb = new InventoryDAL(); // open inventory DB
            inventoryVM items = new inventoryVM(); 
            items.itemsList = inventorydb.Inventorys.ToList<Inventory>(); // get inventory list form DB
            return Json(items.itemsList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult home()
        {
            Session["barcode"] = null;   // null the barcode Sessin to clear place

            if (TempData["Error"] != null) // if any action is failed
                ViewData["Error"] = TempData["Error"];
           
    if (TempData["Ok"] != null)// if action is successed
                ViewData["Ok"] = TempData["Ok"];
            return View();
        }
        public ActionResult buy()
        {
            string barcode = "NOBARCODE";
           
            // make a link form share!!
            if (TempData["Error"] != null)  // if any action is failed
                ViewData["Error"] = TempData["Error"];
            if (Request.QueryString["ref"] == null) // if page refreshed
            {
                 if (TempData["Amount"] != null) // if the action failed because the Amount is more then the store have
                    {
                    ViewData["Amount"] = TempData["Amount"];

                }
            }
            else // page is colled from home page
                Session["barcode"] = Request.QueryString["ref"];

            
            
            if (Session["barcode"] != null) // get the barcode from Session
                barcode = Session["barcode"].ToString();




            InventoryDAL invDB = new InventoryDAL();
            if (Session["barcode"] != null) 
            {
                Inventory inv = invDB.Inventorys.Single(inve => inve.BarCode == barcode);// find the barcode in the  inventory to pull the name
                Orders ord = new Orders();
                OrdersDAL ordDB = new OrdersDAL();


                ord.Name = inv.Name; // the name is order
                ViewData["pic"] = inv.url; // the picture to show in the buy page
                ord.Date = DateTime.Now; // the date now
                return View(ord);
            }
            else
            {

                return HttpNotFound(); // if someone try to enter the page not from home page
            }
        }

        public ActionResult HandleOrderData(Orders ord)
        {
            // action to save order
            if (Session["barcode"] == null) // if someone try to rich this action not from buy page
                return HttpNotFound();
            string barcode = Session["barcode"].ToString();
            ModelState.Clear();
            TryValidateModel(ord); // valiate model order
            InventoryDAL invDB = new InventoryDAL();
            Inventory inv = invDB.Inventorys.Single(inve => inve.BarCode == barcode); // find the item from inventory by barcode
            if (ord.Amount > inv.Amount) // if someone try to order more item then the store have
            {
                TempData["Error"] = "הכמות גדולה מידי";
                TempData["Amount"] = inv.Amount;
                TempData["key1"] = barcode;

                return RedirectToAction("buy", "Home"); // go to buy page
            }
            if (ModelState.IsValid) // if model is valid
            {
                inv.Amount = inv.Amount - ord.Amount; // dicrise the amount from item in invenroty
                invDB.Inventorys.Attach(inv); // try to save the item

                invDB.Entry(inv).State = inv.BarCode == inv.BarCode ?
                           EntityState.Added :
                           EntityState.Modified;
                invDB.Entry(inv).State = EntityState.Modified;

                invDB.SaveChanges();// save db of inventory

                OrdersDAL ordDB = new OrdersDAL();
                ord.Date = DateTime.Now;
                ordDB.Order.Add(ord); // try to enter to DB the order
                ordDB.SaveChanges(); // save db of order
                TempData["Ok"] = " !תודה שקנית אצלנו ";
                Session["barcode"] = null; // to clear the barcode session
                return RedirectToAction("home"); // return to home page
            }
            else // if something bad happend 
            {
                TempData["Error"] = " הפעולה לא הצליחה ";
                 TempData["key1"] = barcode;
                return RedirectToAction("buy", "Home"); // return to buy page
            }

        }
        public ActionResult SearchItems()
        {
            if (TempData["Error"] != null) // if any action is failed
                ViewData["Error"] = TempData["Error"];

            InventoryDAL invDal = new InventoryDAL(); 
            inventoryVM invVm = new inventoryVM();
            invVm.item = new Inventory();
            invVm.itemsList = new List<Inventory>();// display null
            return View(invVm);
        }

        public ActionResult HandleSearchItem()
        {
            InventoryDAL invDal = new InventoryDAL();
            inventoryVM invVm = new inventoryVM();
            invVm.item = new Inventory();

            bool hasTxtTitle, hasFrom; // try to know what search is on
            string invStr="";
            if(Request.Form["txtTextForSearch"]!=null)
                invStr = Request.Form["txtTextForSearch"].ToString();
            int From = 0,To=0;
            if ( invStr.Length== 0)
            {
                hasTxtTitle = false;
            }
            else
            {
                hasTxtTitle = true;
                invStr = Request.Form["txtTextForSearch"].ToString();
            }
            if (Request.Form["txtFromPrice"]==null)
            {
                hasFrom = false;
            }
            else
            {
                if (Request.Form["txtToPrice"]=="")
                    hasFrom = false;
                else {
                    try
                    {
                        From = int.Parse(Request.Form["txtFromPrice"].ToString());
                        To = int.Parse(Request.Form["txtToPrice"].ToString());
                        if (From> To)// swich if value is not from small to big
                        {
                            int tmp = From;
                            From = To;
                            To = tmp;
                        }
                    }
                    catch
                    {
                        hasFrom = false;
                        TempData["Error"] = "נא להזין בשדה המחיר מספר שלם";
                        return RedirectToAction("SearchItems");
                    }
  
                    hasFrom = true;
                    
                }
            }
            if (hasTxtTitle && hasFrom) // if in tow form of search is entered values
            { // display the items that name & price is equle to the values
                invVm.itemsList = invDal.Inventorys.Where<Inventory>(x => (x.Name.Contains(invStr) && x.Price >= From && x.Price <= To)).ToList<Inventory>();
            }
            else
            {
                if (hasTxtTitle) // if only name is entered
                { // display the items that name is contains the value
                    invVm.itemsList = invDal.Inventorys.Where<Inventory>(x => (x.Name.Contains(invStr))).ToList<Inventory>();
                }
                else
                {
                    if (hasFrom) // if search by price is choose
                    { // display the items that price is between min and max
                        invVm.itemsList = invDal.Inventorys.Where<Inventory>(x => ( x.Price >= From && x.Price <= To)).ToList<Inventory>();

                    }
                    else
                    {// if no values entered
                        invVm.itemsList = new List<Inventory>();
                        TempData["Error"]="אין אפשרות לעשות חיפוש חלקי או ריק";
                    }
                }
            }
            return View("SearchItems", invVm);
        }

    
    }
}