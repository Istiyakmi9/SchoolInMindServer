using CommonModal.ORMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IUploadExcelService<T>
    {
        string UploadClassDetail(UploadXml objClassdetail, string ProcedureName);
        string UploadStudentAttendenceDetail(UploadXml objClassdetail, string ProcedureName);
        string UploadFacultyDetail(UploadXml objClassdetail, string ProcedureName);
        string UploadAttendence();
        string UploadResult();
        string UploadDriverVehicle();
        string UploadStudentDetail();
        string UploadSubjects();
        string UploadStudentDetailService(UploadXml objClassdetail, string ProcedureName);
    }

    public class UploadXml
    {
        public string xmlData { set; get; }
        public string schoolTenentId { set; get; }
        public string AdminId { set; get; }
        public string ClassDetailUid { set; get; }
    }
}
