using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLC_Classview_CSharp
{
    public class NewsItem
    {
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public DateTime createdDate { get; set; }
        public string relatedObjectId { get; set; } //the id of the item (assessment, event, note, etc) this news is refering to
        public string scoreResult { get; set; } //application to assessments
        public string dueDate { get; set; }
    }
}