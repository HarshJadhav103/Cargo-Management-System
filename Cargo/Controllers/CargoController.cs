using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Security;

namespace Cargo.Controllers
{
    public class CargoController : Controller
    {
        // GET: Cargo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(FormCollection frm)
        {
            string email = frm["txtemail"].ToString();

            Database1Entities db = new Database1Entities();

            var result = from temp in db.Cargo_Detail
                         where temp.Email_ID == email
                         select new { temp.Password };

            foreach (var test in result)
            {
                string pass = test.Password;


                MailSender.SendEmail("cms.cargomanagementsystem@gmail.com", "cargoadmin", email, "Password" , "Your password is : " + pass, System.Web.Mail.MailFormat.Html, "");
                // https://myaccount.google.com/lesssecureapps


            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Cargo_Detail c, HttpPostedFileBase file)
        {

            Database1Entities db = new Database1Entities();
            if (ModelState.IsValid)
            {

                if (file != null && file.ContentLength > 0)
                {

                    var array1 = new[] { ".jpg" };

                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  



                    if (array1.Contains(ext)) //check what type of extension  
                    {

                        string path = Path.Combine(Server.MapPath("~/CargoImage"), Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        c.ImageName = file.FileName;
                        c.Date = System.DateTime.Now.ToShortDateString();
                        c.Time = System.DateTime.Now.ToShortTimeString();


                        db.Cargo_Detail.Add(c);
                        db.SaveChanges();
                        ModelState.Clear();

                    }
                    else
                    {
                        ViewBag.Message = "Plz upload only Image file...!!";
                    }
                }
            }
            ViewBag.Message = "Registartion Successfully";

            return View();

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Cargo_Detail c)
        {

            Database1Entities db = new Database1Entities();
         

                var result = (from test in db.Cargo_Detail
                              where test.Email_ID == c.Email_ID && test.Password == c.Password
                              select test).Count();

            if (result == 1)
            {

                //get customerid
                var result1 = from test1 in db.Cargo_Detail
                              where test1.Email_ID == c.Email_ID
                              select new { test1.CargoId };


                foreach (var temp in result1)
                {
                    Session["CargoId"] = temp.CargoId;
                }

                ViewBag.Message = "Login succ";
                return RedirectToAction("InnerHome");
            }
            else
            {
                ViewBag.Message = "Invalid UserName or Pass.";

            }
        
            return View();
        }

        public ActionResult InnerHome()
        {
            Database1Entities db = new Database1Entities();
            int id = Convert.ToInt16(Session["CargoId"]);
            var result = from temp in db.User_Cargo_Request
                         where temp.CargoId == id
                         select temp;
            return View(result.ToList());

        }

        public ActionResult Details(int id)
        {
            Database1Entities db = new Database1Entities();

            var test = db.User_Cargo_Request.Find(id);

            int userid = Convert.ToInt16( test.UserId);

            var result = db.Users.Find(userid);
           // User u = new User();
           // u.Name = cid.Name;
           // u.Email_ID = cid.Email_ID;
           // u.Address = cid.Address;
            //u.Phone = cid.Phone;
           
            return View(result);
        }

        public ActionResult User_Cargo_Request_Item(int id)
        {

            Database1Entities db = new Database1Entities();

            int cargoid = Convert.ToInt16(Session["CargoId"]);

            var result = from t1 in db.User_Cargo_Request_Item
                         join t2 in db.User_Cargo_Request on t1.RequestId equals t2.RequestId
                         where t2.CargoId == cargoid
                         && t2.RequestId == id

                         select new Cargo.Models.User_Cargo
                         {
                             
                             RequestId = t2.RequestId,
                             Item_Name = t1.Item_Name,
                             Qty = t1.Qty

                         };

            return View(result.ToList());


        }

       
        public ActionResult Add_Quotation(int id)
        {
            Database1Entities db = new Database1Entities();

            var result = db.User_Cargo_Request.Find(id);

            Session["RequestId"] = result.RequestId;

            return View();
        }

        [HttpPost]
        public ActionResult Add_Quotation(Quotation q)
        {
            Database1Entities db = new Database1Entities();

            if (ModelState.IsValid)

            {

                q.RequestId = Convert.ToInt16(Session["RequestId"]);

                q.CargoId = Convert.ToInt16(Session["CargoId"]);

                q.Date = System.DateTime.Now.ToShortDateString();

                q.Time = System.DateTime.Now.ToShortTimeString();

                q.Status = "Pending";

                db.Quotations.Add(q);

                db.SaveChanges();

                ModelState.Clear();
            }
            return RedirectToAction("InnerHome");
        }

        public ActionResult MainHome()
        {
            return View();
        }
        public ActionResult Logout()
        {

            //disable browsers back buttons.

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));

            Response.Cache.SetNoStore();


            FormsAuthentication.SignOut();

            Session.Abandon();
            Session.RemoveAll();


            return RedirectToAction("Login");
        }
        public ActionResult Approved(int id)
        {
            Database1Entities db = new Database1Entities();

            var result = db.User_Cargo_Request.Find(id);

            return View();
        }
        public ActionResult View_Quotation(int id)
        {
            Database1Entities db = new Database1Entities();

            //int id = Convert.ToInt16(Session["RequestId"]);

            var result = from t1 in db.Quotations
                         join t2 in db.User_Cargo_Request on t1.RequestId equals t2.RequestId
                         join t3 in db.Cargo_Detail on t2.CargoId equals t3.CargoId

                         where t1.RequestId == id

                         select new Cargo.Models.User_Cargo
                         {

                             QuotationId = t1.QuotationId,
                             RequestId = t2.RequestId,
                             CompanyName = t3.CompanyName,
                             Description = t1.Description,
                             Date = t1.Date,
                             Time = t1.Time,
                             Status = t1.Status


                         };

            return View(result.ToList());

        }
        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

    }
}