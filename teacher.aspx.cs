using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SLC_Classview_CSharp
{
    public partial class Teacher : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    Response.Write(Session["access_token"]);
        //}

        /// <summary>
        /// Ac webmethod all to get the sections (classes) the teacher has
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetSections()
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                string apiEndPoint = "https://api.sandbox.slcedu.org/api/rest/v1/sections";
                return Global.Utility.GetJsonData(apiEndPoint);
            }
            return "";
        }

        [WebMethod]
        public static string GetStudentsBySectionId(string sectionId){
            if (HttpContext.Current.Session["access_token"] != null)
            {
                string apiEndPoint = string.Format("https://api.sandbox.slcedu.org/api/rest/v1/sections/{0}/studentSectionAssociations/students", sectionId);
                var data = Global.Utility.GetJsonData(apiEndPoint);
                return data;
            }
            return "";  
        }

        [WebMethod]
        public static string GetStudentAssessmentByStudentId(string studentId)
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                string apiEndPoint = string.Format("https://api.sandbox.slcedu.org/api/rest/v1/students/{0}/studentAssessments/assessments?assessmentCategory!=Other", studentId);
                var data = Global.Utility.GetJsonData(apiEndPoint);
                return data;
            }
            return "";
        }

        /// <summary>
        /// update assessment score result
        /// </summary>
        /// <param name="studentAssessmentId"></param>
        /// <param name="studentId"></param>
        /// <param name="assessmentId"></param>
        /// <param name="score"></param>
        /// <param name="administrationDate"></param>
        /// <param name="assessmentName"></param>
        /// <param name="maxScore"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool UpdateStudentAssessmentAssociation(string studentAssessmentId, string studentId, string assessmentId, string score, string administrationDate, string assessmentName, string maxScore)
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                try
                {
                    var token = HttpContext.Current.Session["access_token"].ToString();
                    Assessment.Utility.UpdateStudentAssessmentAssociation(studentAssessmentId, studentId, assessmentId, administrationDate, score, token);

                    //create a note
                    try
                    {
                        var custom = Student.Utility.GetStudentCustom(studentId);
                        var customObject = JsonConvert.DeserializeObject<News>(custom);
                        var newNews = new NewsItem
                        {
                            title = "New score for " + assessmentName + "!",
                            type = "ASSESSMENT",
                            createdDate = DateTime.Now,
                            relatedObjectId = assessmentId,
                            scoreResult = score + '/' + maxScore
                        };

                        customObject.news.Add(newNews);

                        Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
                    }
                    catch(Exception ex){
                        var is404 = ex.Message.Contains("(404)"); //404 means there's no data in custom yet. Maybe SLC can provide a better response?
                        if (is404)
                        {
                            var customObject = new News(); //create new custom object
                            customObject.news = new List<NewsItem>();
                            var newNews = new NewsItem
                            {
                                title = "New score for " + assessmentName + "!",
                                type = "ASSESSMENT",
                                createdDate = DateTime.Now,
                                relatedObjectId = assessmentId,
                                scoreResult = score + '/' + maxScore
                            };

                            customObject.news.Add(newNews);

                            Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
                        }
                    }                  

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }              
            }
            return false;
        }

        /// <summary>
        /// Get all homework assignments
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetAssignments()
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                var apiEndPoint = "https://api.sandbox.slcedu.org/api/rest/v1/assessments?assessmentCategory=Other";
                var data = Global.Utility.GetJsonData(apiEndPoint);
                return data;
            }
            return "";
            
        }

        [WebMethod]
        public static bool AssignAssignmentToStudents(string assignmentId, string assignmentTitle, string dueDate)
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                try
                {
                    var token = HttpContext.Current.Session["access_token"].ToString();
                    const string karrieId = "2012aj-832065fd-0b25-11e2-985e-024775596ac8";
                    const string mattId = "2012pe-832288e4-0b25-11e2-985e-024775596ac8";

                    Student.Utility.AssignAssignmentToStudent(assignmentId, karrieId, DateTime.Now.ToString("yyyy-MM-dd"), dueDate, token);
                    Student.Utility.AssignAssignmentToStudent(assignmentId, mattId, DateTime.Now.ToString("yyyy-MM-dd"), dueDate, token);


                    //create feeds
                    CreateAssignmentNewsItem(karrieId, assignmentTitle, assignmentId, dueDate, token);
                    CreateAssignmentNewsItem(mattId, assignmentTitle, assignmentId, dueDate, token);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }               
            }
            return false;

        }

        /// <summary>
        /// create new assignment (assessment with type Other)
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="assessmentTitle"></param>
        /// <param name="assessmentId"></param>
        /// <param name="dueDate"></param>
        /// <param name="token"></param>
        public static void CreateAssignmentNewsItem(string studentId, string assessmentTitle, string assessmentId, string dueDate, string token)
        {
            try
            {
                var custom = Student.Utility.GetStudentCustom(studentId);
                var customObject = JsonConvert.DeserializeObject<News>(custom);
                var newNews = new NewsItem
                {
                    title = "New assignment " + assessmentTitle + "! Due on " + dueDate,
                    type = "ASSIGNMENT",
                    createdDate = DateTime.Now,
                    relatedObjectId = assessmentId,
                };

                customObject.news.Add(newNews);

                Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
            }
            catch (Exception ex)
            {
                var is404 = ex.Message.Contains("(404)"); //404 means there's no data in custom yet. Maybe SLC can provide a better response?
                if (is404)
                {
                    var customObject = new News(); //create new custom object
                    customObject.news = new List<NewsItem>();
                    var newNews = new NewsItem
                    {
                        title = "New assignment " + assessmentTitle + "! Due on " + dueDate,
                        type = "ASSIGNMENT",
                        createdDate = DateTime.Now,
                        relatedObjectId = assessmentId,
                    };

                    customObject.news.Add(newNews);

                    Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
                }
            }   
        }

        /// <summary>
        /// create event feed item
        /// </summary>
        /// <param name="eventTitle"></param>
        /// <param name="eventDescription"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool createEventNews(string eventTitle, string eventDescription, string eventDate)
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                try
                {
                    var token = HttpContext.Current.Session["access_token"].ToString();
                    const string karrieId = "2012aj-832065fd-0b25-11e2-985e-024775596ac8";
                    const string mattId = "2012pe-832288e4-0b25-11e2-985e-024775596ac8";

                    CreateEventNewsItem(karrieId, eventTitle, eventDescription, eventDate, token);
                    CreateEventNewsItem(mattId, eventTitle, eventDescription, eventDate, token);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;

        }

        /// <summary>
        /// create generic feed item
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="noteTitle"></param>
        /// <param name="noteDescription"></param>
        /// <returns></returns>
        [WebMethod]
        public static bool createNoteNews(string studentId, string noteTitle, string noteDescription)
        {
            if (HttpContext.Current.Session["access_token"] != null)
            {
                try
                {
                    var token = HttpContext.Current.Session["access_token"].ToString();
                    //const string karrieId = "2012aj-832065fd-0b25-11e2-985e-024775596ac8";
                    //const string mattId = "2012pe-832288e4-0b25-11e2-985e-024775596ac8";

                    CreateNoteNewsItem(studentId, noteTitle, noteDescription, token);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;

        }

        public static void CreateEventNewsItem(string studentId, string eventTitle, string eventDescription, string eventDate, string token)
        {
            try
            {
                var custom = Student.Utility.GetStudentCustom(studentId);
                var customObject = JsonConvert.DeserializeObject<News>(custom);
                var newNews = new NewsItem
                {
                    title = "New event " + eventTitle + "!",
                    type = "EVENT",
                    createdDate = DateTime.Now,
                    dueDate = eventDate
                };

                customObject.news.Add(newNews);

                Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
            }
            catch (Exception ex)
            {
                var is404 = ex.Message.Contains("(404)"); //404 means there's no data in custom yet. Maybe SLC can provide a better response?
                if (is404)
                {
                    var customObject = new News(); //create new custom object
                    customObject.news = new List<NewsItem>();
                    var newNews = new NewsItem
                    {
                        title = "New event " + eventTitle + "!",
                        type = "EVENT",
                        createdDate = DateTime.Now,
                        dueDate = eventDate
                    };

                    customObject.news.Add(newNews);

                    Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
                }
            }
        }

        public static void CreateNoteNewsItem(string studentId, string note, string noteDescription, string token)
        {
            try
            {
                var custom = Student.Utility.GetStudentCustom(studentId);
                var customObject = JsonConvert.DeserializeObject<News>(custom);
                var newNews = new NewsItem
                {
                    title = note,
                    description = noteDescription,
                    type = "NOTE",
                    createdDate = DateTime.Now
                };

                customObject.news.Add(newNews);

                Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
            }
            catch (Exception ex)
            {
                var is404 = ex.Message.Contains("(404)"); //404 means there's no data in custom yet. Maybe SLC can provide a better response?
                if (is404)
                {
                    var customObject = new News(); //create new custom object
                    customObject.news = new List<NewsItem>();
                    var newNews = new NewsItem
                    {
                        title = note,
                        description = noteDescription,
                        type = "NOTE",
                        createdDate = DateTime.Now
                    };

                    customObject.news.Add(newNews);

                    Student.Utility.PostStudentCustom(studentId, JsonConvert.SerializeObject(customObject), token);
                }
            }
        }
    }
}