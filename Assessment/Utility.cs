using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLC_Classview_CSharp.Assessment
{
    public class Utility
    {
        public static string FormatAssessmentIdentificationCode(string identificationSystem, string ID)
        {
            return string.Format("'assessmentIdentificationCode': [{'identificationSystem': {0}, 'ID': {1}}]", identificationSystem, ID);
        }

        /*create assessement
         *  "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentTitle, fieldValue=, expectedTypes=[STRING]] 
         *  ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentIdentificationCode, fieldValue=, expectedTypes=[LIST]]",
         * "assessmentTitle": "Homework 1", "assessmentIdentificationCode": [{"identificationSystem": "Test Contractor", "ID": "Homework 1"}]
         * */
        public static string CreateAssessment(string endPoint, string token, string param)
        {
            //var param = string.Format("{'assessmentTitle': {0}, 'assessmentIdentificationCode': [{'identificationSystem': {1}, 'ID': {2}}]", assessmentTitle, identificationSystem, ID);
            var result = Global.Utility.PostData(endPoint, token, param);
            return result;
        }

        /*update assessment
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentTitle, fieldValue=, expectedTypes=[STRING]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentIdentificationCode, fieldValue=, expectedTypes=[LIST]] 
         * ValidationError [type=ENUMERATION_MISMATCH, fieldName=academicSubject, fieldValue=AP Cal, expectedTypes=[Agriculture, Food, and Natural Resources, 
         * Architecture and Construction, Business and Marketing, Communication and Audio/Visual Technology, Composite, Computer and Information Sciences, 
         * Critical Reading, ELA, Engineering and Technology, English, English Language and Literature, Fine and Performing Arts, Foreign Language and Literature, 
         * Health Care Sciences, Hospitality and Tourism, Human Services, Life and Physical Sciences, Manufacturing, Mathematics, Military Science, Miscellaneous, 
         * Other, Physical, Health, and Safety Education, Public, Protective, and Government Service, Reading, Religious Education and Theology, Science, 
         * Social Sciences and History, Social Studies, Transportation, Distribution and Logistics, Writing]]",
         * {"assessmentTitle": "Homework 1", 
            "assessmentIdentificationCode": [{"identificationSystem": "Test Contractor", "ID": "Homework 1"}], 
            "assessmentItem": [{
            "identificationCode": "AssessmentItem-1",
            "correctResponse": "False",
            "maxRawScore": 5,
            "itemCategory": "True-False"
            }]}
         * */
        public static void UpdateAssessment(string endPoint, string token, string param)
        {

        }

        /* update assessment item
         * Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=identificationCode, fieldValue=, expectedTypes=[STRING]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=itemCategory, fieldValue=, expectedTypes=[TOKEN]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=maxRawScore, fieldValue=, expectedTypes=[INTEGER]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=correctResponse, fieldValue=, expectedTypes=[STRING]]",
         * */
        public static void UpdateAssessmentItem() { }

        /*add sectionAssessmentAssociation
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=sectionId, fieldValue=, expectedTypes=[REFERENCE]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentId, fieldValue=, expectedTypes=[REFERENCE]] 
         * ValidationError [type=UNKNOWN_FIELD, fieldName=assessment, fieldValue={}, expectedTypes=[]]",
         * */
        public static void AddSectionAssessmentAssociation() { }

        /* UpdateStudent
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=studentUniqueStateId, fieldValue=, expectedTypes=[STRING]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=name, fieldValue=, expectedTypes=[COMPLEX]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=sex, fieldValue=, expectedTypes=[TOKEN]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=birthData, fieldValue=, expectedTypes=[COMPLEX]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=firstName, fieldValue=, expectedTypes=[STRING]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=lastSurname, fieldValue=, expectedTypes=[STRING]]",
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=studentUniqueStateId, fieldValue=, expectedTypes=[STRING]]
         * {"name": {"firstName": "Karrie", "lastSurname": "Sollars"}, "sex": "Female", "birthData": {"birthDate": "1999-03-12"}, "studentUniqueStateId": "800000021"} 
        */
        public static void UpdateStudent() { }

        /*CreateStudentAssessment: https://api.sandbox.slcedu.org/api/rest/v1/studentAssessments
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=administrationDate, fieldValue=, expectedTypes=[DATE]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=studentId, fieldValue=, expectedTypes=[REFERENCE]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentId, fieldValue=, expectedTypes=[REFERENCE]]",
         * */
        public static void CreateStudentAssessment() { }

        /*update studentAssessmentAssociation
         * 
         * "Validation failed: ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=administrationDate, fieldValue=, expectedTypes=[DATE]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=studentId, fieldValue=, expectedTypes=[REFERENCE]] 
         * ValidationError [type=REQUIRED_FIELD_MISSING, fieldName=assessmentId, fieldValue=, expectedTypes=[REFERENCE]]",
         * */
        public static void UpdateStudentAssessmentAssociation(string studentAssessmentId, string studentId, string assessmentId, string administrationDate, string score, string token)
        {
            var param = string.Format("{{\"studentId\": \"{0}\", \"assessmentId\": \"{1}\", \"administrationDate\": \"{2}\", \"scoreResults\": [{{\"assessmentReportingMethod\": \"Other\", \"result\": \"{3}\"}}]}}", studentId, assessmentId, administrationDate, score);
            var api = string.Format("https://api.sandbox.slcedu.org/api/rest/v1/studentAssessments/{0}", studentAssessmentId);
            Global.Utility.PutData(api, token, param);        
        }

        
    }
}