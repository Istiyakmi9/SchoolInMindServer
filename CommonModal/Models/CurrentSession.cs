
using BottomhalfCore.Annotations;
using CommonModal.ProcedureModel;

namespace CommonModal.Models
{
    [Scoped]
    public class CurrentSession
    {
        public string UserAgent { set; get; }
        public string Authorization { set; get; }
        public string Culture { set; get; } = "en";
        public string RequestPath { set; get; }
        public string FileUploadFolderName { set; get; }
        public UserDetail CurrentUserDetail { set; get; }
    }
}
