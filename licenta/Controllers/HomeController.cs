using licenta.DatabaseConnection;
using licenta.Models;
using licenta.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace licenta.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomEmailService _emailService;
        private readonly TehnicalDepartmentDb db = new TehnicalDepartmentDb();

        public HomeController()//ICustomEmailService emailService)
        {
          //  _emailService = emailService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult DownloadRaport(myFilteredIncidentRequestViewModel model)
        {

            List<Request> requestsList = db.Requests.ToList();
            string company = Request.Cookies["companyCookie"].Value;
            List<User> companyUsers = db.Users.Where(m => m.company == company).ToList();
            requestsList = db.Requests.Where(m => db.Users.Any(l => m.createdBy == l.userId && l.company == company)).ToList();
            int[,] statusArray = new int[8, 3];
            try
            {
            }
            catch
            {
                return View();
            }
            string statusString;
            if (model.orderVal == "1")
            {
                requestsList.Reverse();
            }
            foreach (var req in requestsList)
            {
                List<RequestHistory> s = new List<RequestHistory>();
                s = db.RequestHistories.Where(m => m.requestId == req.requestId).ToList();
                statusString = s.Last().status;

                bool type = db.Requests.Where(m => m.requestId == req.requestId).FirstOrDefault().type;
                if (type == false)
                {
                    if (statusString == "Pending")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[0, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[0, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[0, 2]++;
                        }

                    }
                    if (statusString == "In Progress")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[1, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[1, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[1, 2]++;
                        }
                    }
                    if (statusString == "Waiting for approval")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[2, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[2, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[2, 2]++;
                        }
                    }
                    if (statusString == "Done")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[3, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[3, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[3, 2]++;
                        }
                    }
                }
                else
                {
                    if (statusString == "Pending")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[4, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[4, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[4, 2]++;
                        }

                    }
                    if (statusString == "In Progress")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[5, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[5, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[5, 2]++;
                        }
                    }
                    if (statusString == "Waiting for approval")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[6, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[6, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[6, 2]++;
                        }
                    }
                    if (statusString == "Done")
                    {
                        if (req.departmentAssigned == "Hardware")
                        {
                            statusArray[7, 0]++;
                        }
                        else if (req.departmentAssigned == "Software")
                        {
                            statusArray[7, 1]++;
                        }
                        else if (req.departmentAssigned == "Finances")
                        {
                            statusArray[7, 2]++;
                        }
                    }
                }


                if (!model.fm.isCheckedType[0] && !req.type)
                {
                    continue;
                }
                if (!model.fm.isCheckedType[1] && req.type)
                {
                    continue;
                }
                if (!model.fm.isCheckedStatus[0] && statusString == "Pending")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[1] == false && statusString == "In Progress")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[2] == false && statusString == "Waiting for approval")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[3] == false && statusString == "Done")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[0] == false && req.departmentAssigned == "Hardware")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[1] == false && req.departmentAssigned == "Software")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[2] == false && req.departmentAssigned == "Finances")
                {
                    continue;
                }


                model.myIncidents.Add(new myIncidentRequestViewModel
                {
                    Id = req.requestId,
                    IncidentRequest = !req.type ? "Incident" : "Request",
                    Title = req.title,
                    CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                    DepartmentAssigned = req.departmentAssigned,
                    Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                    Status = statusString

                });




            }













            int count = model.myIncidents.Count(x => x.IncidentRequest == "Incident");
            model.incidentrequestCount.Add("Incident", count);
            count = model.myIncidents.Count(x => x.IncidentRequest == "Request");
            model.incidentrequestCount.Add("Request", count);


            var products = new System.Data.DataTable("Raport");
            products.Columns.Add("/", typeof(string));
            products.Columns.Add("Incident", typeof(string));
            products.Columns.Add("Request", typeof(string));// typeof(string));
            products.Columns.Add("|", typeof(string));
            products.Columns.Add("Hardware", typeof(string));
            products.Columns.Add("Software", typeof(string));
            products.Columns.Add("Finances", typeof(string));
            products.Rows.Add("Pending", statusArray[0, 0] + statusArray[0, 1] + statusArray[0, 2], statusArray[4, 0] + statusArray[4, 1] + statusArray[4, 2], "|", statusArray[0, 0] + statusArray[4, 0], statusArray[0, 1] + statusArray[4, 1], statusArray[0, 2] + statusArray[4, 2]);
            products.Rows.Add("In Progress", statusArray[1, 0] + statusArray[1, 1] + statusArray[1, 2], statusArray[5, 0] + statusArray[5, 1] + statusArray[5, 2], "|", statusArray[1, 0] + statusArray[5, 0], statusArray[1, 1] + statusArray[5, 1], statusArray[1, 2] + statusArray[5, 2]);
            products.Rows.Add("Waiting for approval", statusArray[2, 0] + statusArray[2, 1] + statusArray[2, 2], statusArray[6, 0] + statusArray[6, 1] + statusArray[6, 2], "|", statusArray[2, 0] + statusArray[6, 0], statusArray[2, 1] + statusArray[6, 1], statusArray[2, 2] + statusArray[6, 2]);
            products.Rows.Add("Done", statusArray[3, 0] + statusArray[3, 1] + statusArray[3, 2], statusArray[7, 0] + statusArray[7, 1] + statusArray[7, 2], "|", statusArray[3, 0] + statusArray[7, 0], statusArray[3, 1] + statusArray[7, 1], statusArray[3, 2] + statusArray[7, 2]);
            products.Rows.Add("Total", model.incidentrequestCount["Incident"], model.incidentrequestCount["Request"], "|");
            //products.Rows.Add(model.incidentrequestCount["Incident"].ToString());
            //products.Rows.Add(model.incidentrequestCount["Request"].ToString());



            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            var date = DateTime.Now;
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=raport" + date.ToString() + ".xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Raport", model);
        }



        public ActionResult Raport(myFilteredIncidentRequestViewModel model)
        {
            if (model == null)
            {
                model = new myFilteredIncidentRequestViewModel();
            }
            List<Request> requestsList = db.Requests.ToList();
            string company = Request.Cookies["companyCookie"].Value;
            List<User> companyUsers = db.Users.Where(m => m.company == company).ToList();
            requestsList = db.Requests.Where(m => db.Users.Any(l => m.createdBy == l.userId && l.company == company)).ToList();





            //List<myIncidentRequestViewModel> myIncidentRequestViewModel = new List<myIncidentRequestViewModel>();
            try
            {
            }
            catch
            {
                return View();
            }
            string statusString;
            if (model.orderVal == "1")
            {
                requestsList.Reverse();
            }
            foreach (var req in requestsList)
            {
                List<RequestHistory> s = new List<RequestHistory>();
                s = db.RequestHistories.Where(m => m.requestId == req.requestId).ToList();
                //var lasts = s.Last().status;
                statusString = s.Last().status;
                //switch (s.Last().status)
                //{
                //    case 1:
                //        statusString=
                //};
                //de refacut intr o zi


                if (model.fm.isCheckedType[0] == false && req.type == false)
                {
                    continue;
                }
                if (model.fm.isCheckedType[1] == false && req.type == true)
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[0] == false && statusString == "Pending")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[1] == false && statusString == "In Progress")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[2] == false && statusString == "Waiting for approval")
                {
                    continue;
                }
                if (model.fm.isCheckedStatus[3] == false && statusString == "Done")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[0] == false && req.departmentAssigned == "Hardware")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[1] == false && req.departmentAssigned == "Software")
                {
                    continue;
                }
                if (model.fm.isCheckedCategory[2] == false && req.departmentAssigned == "Finances")
                {
                    continue;
                }


                model.myIncidents.Add(new myIncidentRequestViewModel
                {
                    Id = req.requestId,
                    IncidentRequest = req.type == false ? "Incident" : "Request",
                    Title = req.title,
                    CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                    DepartmentAssigned = req.departmentAssigned,
                    Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                    Status = statusString

                });




            }
            int count;
            count = model.myIncidents.Count(x => x.IncidentRequest == "Incident");
            model.incidentrequestCount.Add("Incident", count);
            count = model.myIncidents.Count(x => x.IncidentRequest == "Request");
            model.incidentrequestCount.Add("Request", count);

            count = model.myIncidents.Count(x => x.Status == "Pending");
            model.statusCount.Add("Pending", count);
            count = model.myIncidents.Count(x => x.Status == "In Progress");
            model.statusCount.Add("In Progress", count);
            count = model.myIncidents.Count(x => x.Status == "Waiting for approval");
            model.statusCount.Add("Waiting for approval", count);
            count = model.myIncidents.Count(x => x.Status == "Done");
            model.statusCount.Add("Done", count);

            count = model.myIncidents.Count(x => x.DepartmentAssigned == "Hardware");
            model.depassignedCount.Add("Hardware", count);
            count = model.myIncidents.Count(x => x.DepartmentAssigned == "Software");
            model.depassignedCount.Add("Software", count);
            count = model.myIncidents.Count(x => x.DepartmentAssigned == "Finances");
            model.depassignedCount.Add("Finances", count);
            // var users=db.Requests.Where(m=>m.createdBy==companyUsers)
            //var requests = db.Requests.Where()
            // return View(await requests.ToListAsync());
            return View(model);
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "Administrator,Employee")]


        [AllowAnonymous]
        public ActionResult LoginPartial(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        public ActionResult Unassigned()
        {
            List<myIncidentRequestViewModel> models = new List<myIncidentRequestViewModel>();
            List<Request> requestsList = new List<Request>();

            string company = Request.Cookies["companyCookie"].Value;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name && m.company == company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            requestsList = db.Requests.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned && m.createdBy == l.userId)).ToList();

            foreach (var req in requestsList)
            {
                //  DE REFACUT CAND VIN APROVALURILE
                if (db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString() != "Done"
                    && db.Requests.FirstOrDefault(m => m.requestId == req.requestId).employeeAssigned == null)
                {
                    models.Add(new Models.myIncidentRequestViewModel
                    {
                        Id = req.requestId,
                        IncidentRequest = req.type == false ? "Incident" : "Request",
                        Title = req.title,
                        CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                        DepartmentAssigned = req.departmentAssigned,
                        Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                        Status = "Pending"
                    });
                }
            }


            return View(models);
        }
        [Authorize(Roles = "Employee")]
        public ActionResult Assigned(int? id)
        {
            try
            {
                Request req = db.Requests.Where(m => m.requestId == id).FirstOrDefault();
                RequestHistory reqHistory = db.RequestHistories.Where(m => m.requestId == id).FirstOrDefault();

                req.employeeAssigned = User.Identity.Name;
                reqHistory.status = "In Progress";
                db.SaveChanges();

                return RedirectToAction("Dashboard");

            }
            catch
            {
                return View();

            }
        }
        [Authorize(Roles = "Employee")]
        public ActionResult Dashboard()
        {
            //HERE EMPLOYEES SHOULD SEE ALL THEIR ASSIGNED INCIDENTS OR REQUESTS!
            //THEY SHOULD HAVE THE OPTION TO SEND THEM TO OTHER DEP OR CHECK IT 
            //AS DONE WITH "EDIT STATE" BUTTON


            List<myIncidentRequestViewModel> models = new List<myIncidentRequestViewModel>();
            List<Request> requestsList = new List<Request>();
            List<Request> allrequestsList = new List<Request>();
            List<RequestHistory> allrequestsHistoryList = new List<RequestHistory>();

            string company = Request.Cookies["companyCookie"].Value;
            int? departmentIds = db.Users.Where(m => m.username == User.Identity.Name && m.company == company).FirstOrDefault().departmentId;

            string departmentName = db.Departments.Where(m => m.departmentId == departmentIds).First().name;
            //  List<Request> requests = db.Requests.Where(m => m.departmentAssigned == departmentName).ToList();


            allrequestsList = db.Requests.ToList();
            requestsList = allrequestsList.Where(m => db.Users.Any(l => l.company == company && departmentName == m.departmentAssigned && m.createdBy == l.userId)).ToList();
            allrequestsList = allrequestsList.OrderByDescending(c => c.requestId).ToList();
            //requestsList = db.Requests.Where(m => m.createdBy == db.Users.Where(l => l.userId == m.createdBy).FirstOrDefault().userId).ToList();

            //requestsList = requestsList.OrderByDescending(c => c.requestId).ToList();




            foreach (var req in requestsList)
            {

                allrequestsHistoryList = db.RequestHistories.Where(l => l.requestId == req.requestId).ToList();
                allrequestsHistoryList = allrequestsHistoryList.OrderByDescending(l => l.requestHistoryId).ToList();

                if (allrequestsHistoryList.First().to == req.departmentAssigned)
                {
                    string status = db.RequestHistories.Where(m => m.requestId == req.requestId).First().status.ToString();
                    if ((status != "Pending" || status != "Done")
                        && db.Requests.FirstOrDefault(m => m.requestId == req.requestId).employeeAssigned == User.Identity.Name)
                    {

                        models.Add(new Models.myIncidentRequestViewModel
                        {
                            Id = req.requestId,
                            IncidentRequest = req.type == false ? "Incident" : "Request",
                            Title = req.title,
                            CreatedBy = db.Users.Where(m => m.userId == req.createdBy).FirstOrDefault().username,
                            DepartmentAssigned = req.departmentAssigned,
                            Priority = req.priority == 0 ? "Low" : req.priority == 1 ? "Medium" : "High",
                            Status = "In progress"
                        });
                    }
                }
            }




            return View(models);
        }

        [HttpGet]
        public ActionResult ModalPartialDone(int userid)
        {
            ModalPartialDoneViewModel model = new ModalPartialDoneViewModel()
            {
                Id = userid
            };

            //_emailService.SendEmail(new EmailMessage()
            //{
            //    body = "this is a test",
            //    recieverAddress = "vali.scoican@yahoo.com",
            //    recieverName = "Vali Scoican",
            //    subject = "test email"
            //});


            return View(model);
        }


        [HttpPost]
        public ActionResult ModalPartialDone(ModalPartialDoneViewModel model)
        {
            Request request = db.Requests.Where(m => m.requestId == model.Id).FirstOrDefault();
            List<RequestHistory> requestHistory = db.RequestHistories.Where(m => m.requestId == request.requestId).ToList();



            int? fileresult = null;
            DatabaseConnection.File fileDb = null;
            if (model.File != null && model.File.ContentLength > 0)
            {
                try
                {


                    string path = Path.Combine(Server.MapPath("~/Files"),
                              Path.GetFileName(model.File.FileName));
                    model.File.SaveAs(path);

                    fileDb = new DatabaseConnection.File
                    {
                        fileName = model.File.FileName,
                        fileType = model.File.ContentType
                    };


                    db.Files.Add(fileDb);
                    db.SaveChanges();
                    fileresult = fileDb.fileId;
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                fileresult = null;
            }


            RequestHistory lastRequest = new RequestHistory
            {
                from = requestHistory.Last().to,
                to = requestHistory.First().from,
                message = model.Message,
                data = DateTime.Now,
                status = "Done",
                approval = "0",
                requestId = request.requestId,
                attachmentsId = fileresult

            };
            db.RequestHistories.Add(lastRequest);
            db.SaveChanges();

            //_emailService.SendEmail(new EmailMessage()
            //{
            //    body = "this is a test",
            //    recieverAddress = "tereanubogdan@yahoo.com",
            //    recieverName = "Bogdan Tereanu",
            //    subject = "test email"
            //});

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult ModalPartialSendView(int? userid)
        {

            List<RequestHistory> request = db.RequestHistories.Where(m => m.requestId == userid).ToList();
            request = request.OrderByDescending(d => d.data).ToList();

            string mode = request.First().approval;

            SendIncidentRequestViewModel model = new SendIncidentRequestViewModel();

            if (userid != null)
                model.ID = Int32.Parse(userid.ToString());
            List<Department> departments = db.Departments.ToList();
            if (mode != "0" && mode != null)
            {
                if (mode == "1")
                {
                    model.modalType = 1;
                }
                else
                {
                    string to = mode;
                    ViewBag.to = to;

                    model.modalType = 2;
                }
                foreach (var d in departments)
                {
                    if (d.name == request.First().from)
                    {
                        model.departmentsList.Add(new SelectListItem { Text = d.name, Value = d.name.ToString() });
                        break;
                    }
                }
            }
            else
            {
                model.modalType = 0;
                foreach (var d in departments)
                {
                    model.departmentsList.Add(new SelectListItem { Text = d.name, Value = d.name.ToString() });

                }
            }
            return PartialView(model);

        }

        [HttpPost]
        public ActionResult ModalPartialSendView(SendIncidentRequestViewModel model)
        {

            RequestHistory requestHistory = db.RequestHistories.Where(m => m.requestId == model.ID).FirstOrDefault();
            Request request = db.Requests.FirstOrDefault(m => m.requestId == requestHistory.requestId);


            int? fileresult = null;
            DatabaseConnection.File fileDb = null;
            if (model.file?.ContentLength > 0)
            {
                try
                {


                    string path = Path.Combine(Server.MapPath("~/Files"),
                              Path.GetFileName(model.file.FileName));
                    model.file.SaveAs(path);

                    fileDb = new DatabaseConnection.File
                    {
                        fileName = model.file.FileName,
                        fileType = model.file.ContentType
                    };
                    fileresult=fileDb.fileId;

                    db.Files.Add(fileDb);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message;
                }
            }

            string statusState;
            string approvalMessage = "0";

            if (model.NeedsApproval == "1")
            {
                statusState = "Waiting for approval";
                if (model.ApprovalType == "0")
                {
                    approvalMessage = "1";
                }
                else
                {
                    approvalMessage = User.Identity.Name;
                }
            }
            else
            {

                statusState = "Pending";
            }


            string dep = request.departmentAssigned;
            request.departmentAssigned = model.Category;
            if (model.NeedsApproval != null && model.NeedsApproval != "0" && model.NeedsApproval != "1")
            {
                request.employeeAssigned = model.NeedsApproval;
                statusState = "In Progress";
            }
            else
            {
                request.employeeAssigned = null;
            }

            db.SaveChanges();

            //int? f= fileDb.fileId;
            RequestHistory newrequestHistory = new RequestHistory
            {
                requestId = model.ID,
                from = dep,
                to = model.Category,
                data = DateTime.Now,
                message = model.Message,
                approval = approvalMessage,
                status = statusState,

            };
            try
            {
                newrequestHistory.attachmentsId = fileDb.fileId;
            }catch(Exception e)
            {
                Console.WriteLine("Nothing attacheed");
            }
            db.RequestHistories.Add(newrequestHistory);
            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            DatabaseConnection.File file = db.Files.Where(m => m.fileId == id).FirstOrDefault();
            return File("~\\Files\\" + file.fileName, file.fileType, file.fileName);
        }



        [HttpGet]
        public ActionResult History(int id)
        {
            Request request = db.Requests.FirstOrDefault(m => m.requestId == id);
            List<RequestHistory> requestHistory = db.RequestHistories.Where(m => m.requestId == id).ToList();
            requestHistory = requestHistory.OrderByDescending(d => d.data).ToList();
            HistoryRequestViewModel model = new HistoryRequestViewModel();

            if (request.fileId != null)
            {
                model.download = (int)request.fileId;
            }

            model.title = request.title;

            model.type = !request.type ? "Incident" : "Request";

            model.description = request.description;

            switch (request.priority)
            {
                case 0:
                    model.priority = Constants.RequestPriorityLow;
                    break;
                case 1:
                    model.priority = Constants.RequestPriorityMedium;
                    break;
                case 2:
                    model.priority = Constants.RequestPriorityHigh;
                    break;
            }

            PopulatHistoriesList(requestHistory, model);

            return View(model);
        }

        private static void PopulatHistoriesList(List<RequestHistory> requestHistory, HistoryRequestViewModel model)
        {
            foreach (var req in requestHistory)
            {
                string sendback;
                if (req.approval == "1")
                {
                    sendback = "Send back to department";
                }
                else if (req.approval == "0" || req.approval == null)
                {
                    sendback = "-";
                }
                else
                {
                    sendback = "Send back to " + req.approval;
                }


                model.histories.Add(new HistoryRequestModel
                {

                    from = req.from,
                    to = req.to,
                    data = req.data,
                    message = req.message,
                    status = req.status,
                    approvaltype = sendback,
                    attachmentsId = req.attachmentsId

                });
            }
        }
    }
}