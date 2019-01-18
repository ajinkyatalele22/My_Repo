using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ajinkya_Repository.Models;
using Ajinkya_Repository.Models.Classes;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace Ajinkya_Repository.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MainPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.Name = null;
                Int32 UserID = Convert.ToInt32(Request.Cookies["CurrentUser"].Value.ToString());

                using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
                {
                    var Data = dc.Registrations.SingleOrDefault(x => x.ID == UserID);

                    if (Data != null)
                    {
                        ViewBag.Name = Data.First_Name + " " + Data.Last_Name;
                    }
                    else
                    {
                        ViewBag.Name = "";
                    }
                }

                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login_Model login, string ReturnUrl = "")
        {
            ViewBag.Message = null;
            string message = "";

            using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
            {
                var v = dc.Registrations.Where(a => a.EmailID == login.Email_ID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0 && v.IsEmailVerified == true)
                    {
                        int timeout = login.RememberMe ? 21600 : 30;
                        var ticket = new FormsAuthenticationTicket(login.Email_ID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(login.Email_ID, true);
                            Response.Cookies["CurrentUser"].Value = (v.ID).ToString();
                            Response.Cookies["CurrentUser"].Expires = DateTime.Now.AddMinutes(timeout);
                            Session.Timeout = (timeout);
                            return RedirectToAction("Dashboard", "Home");
                        }
                    }
                    else
                    {
                        message = "Your Email Is Not Verified Till Now Please Verify It .... / Wrong Password ....";
                    }
                }
                else
                {
                    message = "Invalid Account Please Register Yourself . . . ";
                }
            }

            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]Registration reg)
        {
            bool Status = false;
            string message = "";

            if (ModelState.IsValid)
            {
                #region
                var isExist = IsEmailExist(reg.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email Already Exist");
                    return View(reg);
                }
                #endregion

                #region
                reg.ActivationCode = Guid.NewGuid();
                #endregion

                #region
                reg.Password = Crypto.Hash(reg.Password);
                reg.ConfirmPassword = Crypto.Hash(reg.ConfirmPassword);
                #endregion
                reg.IsEmailVerified = false;

                #region
                using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
                {
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.Registrations.Add(reg);
                    dc.SaveChanges();

                    #region 
                    SendVerificationLinkEmail(reg.EmailID, reg.ActivationCode.ToString());
                    message = "Registration Successfully Done . . . Account Activation link " +
                        "has been send to your email id : " + reg.EmailID;
                    Status = true;
                    #endregion
                }
                #endregion

                #region
                using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
                {
                    var Data = dc.Registrations.SingleOrDefault(x => x.EmailID == reg.EmailID);

                    if (Data != null)
                    {
                        Data.Password = reg.Password;
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                    }
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(reg);
        }

        [Authorize]
        public ActionResult Logout()
        {
            Response.Cookies["CurrentUser"].Value = "0";
            Response.Cookies["CurrentUser"].Expires = DateTime.Now.AddSeconds(1);
            FormsAuthentication.SignOut();
            return RedirectToAction("MainPage", "Home");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
            {
                var v = dc.Registrations.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            #region
            var pass = "YourPassword"; //Enter Your Gmail Password here 
            #endregion
            var verifyUrl = "/Home/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("YourMailID@gmail.com", "Verification Mail"); // Enter Your MailID Here
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = pass;

            string subject = "";
            string body = "";

            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your account is " +
                                "Successfully created. Please click on the below link to verify your account " +
                                "<br/><br/><a href='" + link + "'>" + link + "</a>";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password !";
                body = "Hi, <br/><br/> We Got request for reset your password . Please Click on the below link to reset your password " +
                    "<br/><br/><a href=" + link + ">Reset Password Link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
        }
        
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;

            using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; 
                var v = dc.Registrations.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();

                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                    ViewBag.Message = v.EmailID + " Has Been Verified Successfully ...";
                }
                else
                {
                    ViewBag.Message = "Invalid Request . . . ";
                }
            }

            ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(Login_Model lm)
        {
            string message = "";

            using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
            {
                var account = dc.Registrations.Where(a => a.EmailID == lm.Email_ID).FirstOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailID, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;

                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your Email . . . ";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
            {
                var user = dc.Registrations.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPassword model = new ResetPassword();
                    model.ResetCode = id;
                    System.Web.HttpContext.Current.Session["Code"] = id.ToString();
                    Session.Timeout = (10);
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            ViewBag.Message = null;
            var message = "";
            model.ResetCode = Session["Code"].ToString();
            Session["Code"] = null;
            Session.Timeout = (1);
            if (model.ResetCode != null)
            {
                using (Ajinkya_RepositoryEntities dc = new Ajinkya_RepositoryEntities())
                {
                    var user = dc.Registrations.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";

                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New Password updated sucessfully . . . ";
                    }
                }
            }
            else
            {
                message = "Something Wrong . . .";
            }

            ViewBag.Message = message;
            return View(model);
        }
    }
}