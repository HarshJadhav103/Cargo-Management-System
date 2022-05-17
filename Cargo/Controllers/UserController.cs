using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Web.Security;
using Cargo.Models;

namespace Cargo.Controllers
{
    public class UserController : Controller
    {
        // GET: Customer
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

            var result = from temp in db.Users
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
        public ActionResult Register(User u, HttpPostedFileBase file)
        {
            Database1Entities db = new Database1Entities();

            if (ModelState.IsValid)

            {

                if (file != null && file.ContentLength > 0)
                {

                    var array1 = new[] { ".jpg",".png" };

                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  



                    if (array1.Contains(ext)) //check what type of extension  
                    {

                        string path = Path.Combine(Server.MapPath("~/CustomerImage"), Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        u.ImageName = file.FileName;
                        u.Date = System.DateTime.Now.ToShortDateString();
                        u.Time = System.DateTime.Now.ToShortTimeString();


                        db.Users.Add(u);
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
        public ActionResult Login(User u)
        {
            Database1Entities db = new Database1Entities();

            var result = (from test in db.Users
                          where test.Email_ID == u.Email_ID && test.Password == u.Password
                          select test).Count();

            if (result == 1)
            {

                //get customerid
                var result1 = from test1 in db.Users
                              where test1.Email_ID == u.Email_ID
                              select new { test1.UserId };


                foreach (var temp in result1)
                {
                    Session["UserId"] = temp.UserId;
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

            /*
            var result = from t1 in db.Cargo_Detail
                         join t2 in db.Quotations on t1.CargoId equals t2.CargoId

                         select new Cargo.Models.User_Quotation
                         {
                             CargoId = t1.CargoId,
                             Email_ID = t1.Email_ID,
                             CompanyName = t1.CompanyName,
                             CompAddress = t1.CompAddress,
                             City = t1.City,
                             Area = t1.Area,
                             Phone = t1.Phone,
                             WorkCategory = t1.WorkCategory,
                             Status = t2.Status

                         };
                         */


            var result = from t1 in db.Cargo_Detail
                         

                         select new Cargo.Models.User_Quotation
                         {
                             CargoId = t1.CargoId,
                             Email_ID = t1.Email_ID,
                             OwnerName=t1.OwnerName,
                             CompanyName = t1.CompanyName,
                             CompAddress = t1.CompAddress,
                             City = t1.City,
                             //Area = t1.Area,
                             Phone = t1.Phone,
                             WorkCategory = t1.WorkCategory

                         };



            return View(result.ToList());

        }



        public ActionResult Cargo_Request()
        {
            Database1Entities db = new Database1Entities();

            ViewBag.cargo = db.Cargo_Detail.ToList();


            return View();

        }

        [HttpPost]
        public ActionResult Cargo_Request(User_Cargo_Request u, int id)
        {
            Database1Entities db = new Database1Entities();



            var qid = db.User_Cargo_Request.Find(id);

            u.CargoId = id;

            u.RequestDate = System.DateTime.Now.ToShortDateString();

            u.UserId = Convert.ToInt16(Session["UserId"]);


            db.User_Cargo_Request.Add(u);
            db.SaveChanges();

            ModelState.Clear();



            ViewBag.cargo = db.Cargo_Detail.ToList();
            ViewBag.Message = "Cargo Request Sent Successfully";
            return View();

        }

        public ActionResult Details(int id)
        {
            Database1Entities db = new Database1Entities();

            var cid = db.Cargo_Detail.Find(id);

            Cargo_Detail c = new Cargo_Detail();
      //      c.Area = cid.Area;
            c.City = cid.City;
            c.CompAddress = cid.CompAddress;
            c.CompanyName = cid.CompanyName;
            c.Email_ID = cid.Email_ID;
            c.ImageName = cid.ImageName;
            c.WorkCategory = cid.WorkCategory;
            c.Phone = cid.Phone;
            c.OwnerName = cid.OwnerName;


            return View(c);
        }

        public ActionResult Map(int id)
        {

            Database1Entities db = new Database1Entities();

            var cid = db.Cargo_Detail.Find(id);

            ViewBag.lat = cid.latitude;
            ViewBag.lng = cid.Longitude;


            return View();
        }

        public ActionResult View_Request()
        {
            Database1Entities db = new Database1Entities();

            int id = Convert.ToInt16(Session["UserId"]);


            var result = from t1 in db.Cargo_Detail
                         join t2 in db.User_Cargo_Request on t1.CargoId equals t2.CargoId
                         join t3 in db.Users on t2.UserId equals t3.UserId
                         where t3.UserId == id
                         select new Cargo.Models.User_Cargo
                         {
                            RequestId = t2.RequestId,
                             CompanyName = t1.CompanyName,
                             WorkCategory = t2.WorkCategory,
                             FromLocation = t2.FromLocation,
                             ToLocation = t2.ToLocation,
                             RequestDate = t2.RequestDate,
                             DeliveryDate = t2.DeliveryDate,
                             Description = t2.Description

                         };

            return View(result.ToList());

        }

        public ActionResult Add_Item(int id)
        {
            Database1Entities db = new Database1Entities();

            var result = db.User_Cargo_Request.Find(id);

            Session["RequestId"] = result.RequestId;

            return View();
           // return RedirectToAction("view_request");
        }

        [HttpPost]
        public ActionResult Add_Item(User_Cargo_Request_Item u)
        {
            Database1Entities db = new Database1Entities();

            if (ModelState.IsValid)

            {

                u.UserId = Convert.ToInt16(Session["UserId"]);

                u.RequestId = Convert.ToInt16(Session["RequestId"]);

                db.User_Cargo_Request_Item.Add(u);

                db.SaveChanges();

            }

            //return View();
            return RedirectToAction("view_request");
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

        public ActionResult Approved(int id)
        {
            Database1Entities db = new Database1Entities();

            var result = db.User_Cargo_Request.Find(id);

            return View();
        }

        [HttpPost]
        public ActionResult Approved(Quotation q, string button, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string buttonClicked = Request.Form["SubmitButton"];
            if (buttonClicked == "Approve")
            {
                Database1Entities db = new Database1Entities();

                var result = db.Quotations.Find(id);

                result.Status = "Approved";

                db.SaveChanges();

            }

         //   return RedirectToAction("InnerHome");

            int requestid = Convert.ToInt16(Session["quotationid"]);

            return RedirectToAction("Buy", new { id = requestid });


        }

        public ActionResult Buy(int id)
        {

            Database1Entities db = new Database1Entities();

            var result = db.Cargo_Detail.Find(id);


            Payment p = new Payment();
            p.UserId = Convert.ToInt32(Session["UserId"]);
            p.CargoId = id;
            p.Price = "1000";
            p.Date = System.DateTime.Now.ToShortDateString();
            p.Time = System.DateTime.Now.ToShortTimeString();

            db.Payments.Add(p);
            db.SaveChanges();


            Cargo.Models.Paypal p1 = new Cargo.Models.Paypal();

            p1.cmd = "_xclick";
            p1.business = "cms.cargomanagementsystem-facilitator@gmail.com";
            //seller email id


            p1.cancel_return = "";
            p1.@return = "";
            p1.notify_url = "";



            //p1.item_name = result.RequestId;
            p1.amount = 1000;
            //p1.ItemNumber = id;


            p1.currency_code = "USD";
            p1.no_shipping = "1";

            return View(p1);


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

        public ActionResult View_Item(int id)
        {
            Database1Entities db = new Database1Entities();

            Session["requestid"] = id; 
            //int id = Convert.ToInt16(Session["RequestId"]);

            var result = from t1 in db.User_Cargo_Request_Item
                         join t2 in db.User_Cargo_Request on t1.RequestId equals t2.RequestId
                         join t3 in db.Cargo_Detail on t2.CargoId equals t3.CargoId

                         where t1.RequestId == id

                         select new Cargo.Models.View_Item
                         {
                             RequestId = t2.RequestId,
                             CompanyName = t3.CompanyName,
                             Item_Name = t1.Item_Name,
                             Qty = t1.Qty,
                             ItemId=t1.ItemId


                         };

            return View(result.ToList());
        }

        public ActionResult EditItem(int id)
        {
            Database1Entities db = new Database1Entities();

            var cid = db.User_Cargo_Request_Item.Find(id);

            User_Cargo_Request_Item cust = new User_Cargo_Request_Item();
            cust.ItemId = id;
            cust.Item_Name = cid.Item_Name;
            cust.Qty = cid.Qty;

            return View(cust);
        }
        [HttpPost]
        public ActionResult EditItem(User_Cargo_Request_Item cust,int id)
        {
            Database1Entities db = new Database1Entities();

           // var cid = db.User_Cargo_Request_Item.Find(cust.ItemId);

            var cid = db.User_Cargo_Request_Item.Find(id);

            cid.Item_Name = cust.Item_Name;
            cid.Qty = cust.Qty;

            db.SaveChanges();

            //            return View(cid);
            int requestid = Convert.ToInt16(Session["requestid"]);

            return RedirectToAction("view_item", new { id = requestid });
        }

        public ActionResult Delete(int id)
        {
            Database1Entities db = new Database1Entities();

            var cid = db.Quotations.Find(id);
            
            db.Quotations.Remove(cid);

            db.SaveChanges();

            return RedirectToAction("InnerHome");
        }

        public ActionResult DeleteItem(int id)
        {
            Database1Entities db = new Database1Entities();

            var result = db.User_Cargo_Request_Item.Find(id);
            db.User_Cargo_Request_Item.Remove(result);
            db.SaveChanges();

            int requestid =  Convert.ToInt16( Session["requestid"]);

            return RedirectToAction("view_item",new { id = requestid });
        }

        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(Feedback f)
        {
            Database1Entities db = new Database1Entities();
            db.Feedbacks.Add(f);
            db.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

      
    }
}
