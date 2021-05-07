using BottomhalfCore.Annotations;
using System;

namespace CommonModal.ORMModels
{
    public class ExamDescription
    {
        public string ExamDescriptionUid { set; get; }
        public string TanentUid { set; get; }
        public int ExamId { set; get; }
        
        [Required]
        public string ExamName { set; get; }
        public DateTime? ExpectedDate { set; get; }
        public DateTime? ActualDate { set; get; }
        public string Description { set; get; }
    }
}
