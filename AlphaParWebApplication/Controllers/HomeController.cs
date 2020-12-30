using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AlphaParWebApplication.Models;

namespace AlphaParWebApplication.Controllers
{
    public class HomeController : Controller
    {
        ServiceReference1.ServiceClient serviceRef;

        public HomeController()
        {
            serviceRef = new ServiceReference1.ServiceClient();
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
        }

        public ActionResult Login()
        {
            if (Session["userToken"] != null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.LoginInfo loginInfo)
        {
            if (Session["userToken"] != null)
                return RedirectToAction("Index");
            if (String.IsNullOrEmpty(loginInfo.username) || String.IsNullOrEmpty(loginInfo.password))
            {
                ViewBag.ErrorMessage = "Please provide username & password";
                return RedirectToAction("Login");
            }
            try
            {
                string userToken = serviceRef.LoginWithAD(loginInfo.username, loginInfo.password);

                Session["userToken"] = userToken;

                return RedirectToAction("Index");
            }
            catch(FaultException<ServiceReference1.ServiceFault> fault)
            {
                ViewBag.ErrorMessage = fault.Detail.Message;

                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            if (Session["userToken"] == null)
                return RedirectToAction("Login");

            try
            {
                string username = serviceRef.GetUsernameByToken(Session["userToken"].ToString());
                ViewData["username"] = username;

                ServiceReference1.Client[] clients = serviceRef.GetClients(Session["userToken"].ToString());
                ClientWrapper[] clientWrappers = new ClientWrapper[clients.Count()];
                for (int i = 0; i < clients.Count(); i++)
                {
                    clientWrappers[i] = new ClientWrapper(clients[i]);
                }

                return View(clientWrappers);
            }
            catch(FaultException<ServiceReference1.ServiceFault> fault)
            {
                if (fault.Detail.Message == "Error: Token expired or invalid")
                    return RedirectToAction("Logout");
                else
                    ViewBag.ErrorMessage = fault.Detail.Message;
                return View();
            }
        }

        public ActionResult DeleteClient(string urlEncodedJsonclient)
        {
            if (Session["userToken"] == null)
                return RedirectToAction("Login");

            var jsonClient = HttpUtility.UrlDecode(urlEncodedJsonclient);
            ServiceReference1.Client client = new JavaScriptSerializer().Deserialize<ServiceReference1.Client>(jsonClient);

            try
            {
                bool clientIsDeleted = serviceRef.DeleteClient(Session["userToken"].ToString(), client);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult InsertClient(ServiceReference1.Client client)
        {
            if (Session["userToken"] == null)
                return RedirectToAction("Login");

            if (string.IsNullOrEmpty(client.address) ||
                string.IsNullOrEmpty(client.firstname) ||
                string.IsNullOrEmpty(client.lastname) ||
                string.IsNullOrEmpty(client.phone)
                )
            {
                ViewBag.ErrorMessage = "Please provide all required information to add a new client.";
                return RedirectToAction("Index");
            }
            try
            {
                serviceRef.InsertClient(Session["userToken"].ToString(), client);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}