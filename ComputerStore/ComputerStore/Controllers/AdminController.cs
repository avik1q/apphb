using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComputerStore.DAL;
using ComputerStore.Models;
using ComputerStore.ViewModel;

namespace ComputerStore.Controllers
{
    public class AdminController : Controller
    {
        private AdminDAL db = new AdminDAL();

        // GET: Admin
        public ActionResult Index()
        {

            try
            {
                string admin = Session["Admin"].ToString();// if admin is login
                Admin loginAdmin = db.Admins.Single(adms => adms.Name == admin); // double chack in db
                if (loginAdmin != null)// if admin is login display index page
                {
                    return View();
                }
            }
            catch // if something is go bed
            {
                TempData["Error"] = " !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home"); //#change - add return
            } // if claint try to rich to index page
            TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
            return RedirectToAction("home", "Home");

        }
        public ActionResult login()
        {
            try
            {
                string admin = Session["Admin"].ToString();
                Admin loginAdmin = db.Admins.Single(adms => adms.Name == admin);
                if (loginAdmin != null) // if admin is login before
                {
                    return View("Index"); // goto index
                }
            }
            catch  // if something is go bed
            {
                return View();
            }
            return View();

        }
        public ActionResult Logout()
        {
            Session["Admin"] = null; //delete the admin Session
            Session["Password"] = null;
            return RedirectToAction("home", "home"); // goto home page
        }

        public ActionResult updateItem(Inventory item)
        {
            Admin loginAdmin = null;
            try
            {
                string admin = Session["Admin"].ToString();
                loginAdmin = db.Admins.Single(adms => adms.Name == admin); // double chack in db
            }
            catch // if something is go bed
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }
            string ok = null;
            if (loginAdmin != null) // if admin in the system
            {
                string barcode = item.BarCode;
                ModelState.Clear();
                TryValidateModel(item);
                InventoryDAL invDB = new InventoryDAL();


                if (ModelState.IsValid)
                {

                    var itemToUpdate = item;


                    invDB.Inventorys.Attach(itemToUpdate);//attach to DB
                    invDB.Entry(itemToUpdate).State = EntityState.Modified;// modified mode

                    invDB.SaveChanges(); //save DB
                    ok = item.Name + " עודכן בהצלחה  ";


                }
                else// if something is go bed
                {
                    TempData["Error"] = "נא למלא את כל השדות בטופס";
                    return RedirectToAction("ViewItem");
                }
            }
            else// if admin not login
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }

            TempData["Ok"] = ok;

            return RedirectToAction("ViewItem");
        }
        public ActionResult deleteItem(Inventory item)
        {
            string Barcode = Request.Form["Text1"];
            if (Session["Admin"] != null)
            {

                InventoryDAL invDal = new InventoryDAL();
                Inventory inv = invDal.Inventorys.Single(x => x.BarCode == Barcode); // search item in DB
                invDal.Inventorys.Remove(inv); // DELETE item 
                invDal.SaveChanges();// save db
                return RedirectToAction("ViewItem");
            }
            else
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }
        }
        public ActionResult ViewItem()
        {
            try
            {
                string admin = Session["Admin"].ToString();
                Admin loginAdmin = db.Admins.Single(adms => adms.Name == admin);
                if (loginAdmin != null) // if admin login
                {
                    if (TempData["Error"] != null)// if any errors
                        ViewData["Error"] = TempData["Error"];
                    else
                        if (TempData["Ok"] != null)//if any confirm actions
                        ViewData["Ok"] = TempData["Ok"];

                    InventoryDAL inventorydb = new InventoryDAL();
                    inventoryVM items = new inventoryVM();
                    items.itemsList = inventorydb.Inventorys.ToList<Inventory>();// get list of inventory

                    return View("ViewItem", items);

                }
            }
            catch// if admin not in the system
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }// if claint try to rich to index page
            TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
            return RedirectToAction("home", "Home");


        }
        public ActionResult ViewOrders()
        {
            try
            {
                string admin = Session["Admin"].ToString();
                Admin loginAdmin = db.Admins.Single(adms => adms.Name == admin);
                if (loginAdmin != null) // if admin not is the system
                {
                    if (TempData["Error"] != null)// if any errors
                        ViewData["Error"] = TempData["Error"];
                    else
                        if (TempData["Ok"] != null)//if any confirm actions
                        ViewData["Ok"] = TempData["Ok"];

                    OrdersDAL ordersDB = new OrdersDAL();
                    OrdersVM items = new OrdersVM();
                    items.ordersList = ordersDB.Order.ToList<Orders>();

                    return View("ViewOrders", items);

                }
            }
            catch// if admin not in the system
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }// if claint try to rich to index page
            TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
            return RedirectToAction("home", "Home");


        }


        public ActionResult HandleAdminLogin(Admin adm)
        {

            if (adm.Name != null && adm.Password != null)// chack adm not null
            {
                try
                {
                    Admin loginAdmin = db.Admins.Single(adms => adms.Name == adm.Name && adms.Password == adm.Password);// search admin and password in admin DB
                    if (loginAdmin != null)// if found
                    {
                        Session["Admin"] = adm.Name;
                        return View("Index");
                    }
                }
                catch
                {
                    TempData["alertMessage"] = "שם המשתמש ו/או הסיסמה לא נכונים";
                    return View("Login");
                }
            }//if adm==null
            TempData["alertMessage"] = "נא למלא את השדות";

            return View("Login");

        }
        public ActionResult InventoryPage()// add new product
        {
            try
            {
                string admin = Session["Admin"].ToString();
                Admin loginAdmin = db.Admins.Single(adms => adms.Name == admin);
                if (loginAdmin != null)// if admin in system
                {
                    if (TempData["Error"] != null)  //  if any errors  
                        ViewData["Error"] = TempData["Error"];
                    else
                        if (TempData["Ok"] != null) //if any confirm actions
                        ViewData["Ok"] = TempData["Ok"];
                    return View("InventoryPage");
                }
            }
            catch// if admin not is the system
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }
            // if claint try to rich to InventoryPage
            TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
            return RedirectToAction("home", "Home");


        }

        public ActionResult AddNewItem(Inventory inv)
        {
            Admin loginAdmin = null;
            try
            {
                string admin = Session["Admin"].ToString();
                loginAdmin = db.Admins.Single(adms => adms.Name == admin);
            }
            catch// if admin not is the system
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }
            string ok = null;
            if (loginAdmin != null)// if admin login
            {
                string barcode = inv.BarCode;
                ModelState.Clear();
                TryValidateModel(inv);
                InventoryDAL invDB = new InventoryDAL();


                if (ModelState.IsValid)
                {

                    var courseToUpdate = invDB.Inventorys.Find(inv.BarCode);// try to find item with the same barcode

                    if (courseToUpdate == null) // if not found item with the same barcode
                    {
                        invDB.Inventorys.Add(inv);// add item to DB
                        invDB.SaveChanges(); // save DB
                        ok = " הוסף בהצלחה " + inv.Name + " המוצר ";
                    }
                    else // if found item with the same barcode
                    {
                        TempData["error"] = " אין אפשרות להוסיף שני מוצרים בעלי אותו מק''ט ";
                        return RedirectToAction("InventoryPage");
                    }

                }
                else//if the form not fill up
                {
                    TempData["error"] = "נא למלא את כל השדות בטופס";
                    return RedirectToAction("InventoryPage");
                }
            }
            else // if admmin not in the system
            {
                TempData["Error"] =" !אין אפשרות לצפות בדף זה ללא הרשאת מנהל";
                return RedirectToAction("home", "home");
            }

            TempData["ok"] = ok;
            return RedirectToAction("InventoryPage");
        }


    }
}

