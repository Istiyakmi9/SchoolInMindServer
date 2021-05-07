using BottomhalfCore.Annotations;

namespace CommonModal.Models
{
    public class GradeDetail
    {
        public long GradeUid { set; get; }     
        [Required]
        public string Grade { set; get; }
        public string Description { set; get; }
        [Required]
        public int MinMarks { set; get; }
        [Required]
        public int MaxMarks { set; get; }
    }
}