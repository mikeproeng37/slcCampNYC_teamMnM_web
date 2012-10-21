using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLC_Classview_CSharp.Student
{
    public class Utility
    {
        /// <summary>
        /// get student custom to add news feed
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static string GetStudentCustom(string studentId)
        {
            var api = string.Format("https://api.sandbox.slcedu.org/api/rest/v1/students/{0}/custom", studentId);
            return Global.Utility.GetJsonData(api);
        }

        public static string PostStudentCustom(string studentId, string param, string token)
        {
            var api = string.Format("https://api.sandbox.slcedu.org/api/rest/v1/students/{0}/custom", studentId);
            return Global.Utility.PostData(api, token, param); 
        }

        /// <summary>
        /// create studentAssessmentAssociation
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="studentId"></param>
        /// <param name="adminDate"></param>
        /// <param name="dueDate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string AssignAssignmentToStudent(string assessmentId, string studentId, string adminDate, string dueDate, string token)
        {
            var apiEndPoint = "https://api.sandbox.slcedu.org/api/rest/v1/studentAssessments";
            var param = "{\"assessmentId\":\"" + assessmentId + "\", \"studentId\":\"" + studentId + "\", \"administrationDate\":\"" + adminDate + "\", \"administrationEndDate\":\"" + dueDate + "\"}";
            return Global.Utility.PostData(apiEndPoint, token, param);
        }
    }
}