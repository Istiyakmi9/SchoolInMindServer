using CommonModal.Models;
using CommonModal.ORMModels;
using System.Collections.Generic;
using System.Data;

namespace ServiceLayer.Interface
{
    public interface IAdminMasterService<T>
    {
        DataSet GetClassesService(SearchModal searchModal);
        DataSet DeleteClassAndSectionService(int Class, string Section);
        string AttendenceByClassDetailService(Classdetail classdetail);
        string AddUpdateAttendenceService(List<AttendanceClassWise> attendanceClassWise);
        string AddUpdateSingleClassAttendenceService(List<AttendanceSingleData> attendanceSingleData);
        string AddUpdateRoleService(MenuAndRoles menuAndRoles);
        string GetRoleService();
        string RolesByAccessLevelService(string AccessLevelUid);
        string GetAllMenuService();
        string InsertNewClassInfoService(Classdetail objClassdetail);
        string GetVehicleTypeService();
        string ViewAddSubjectService(string SearchStr, string SortBy, string PageIndex, string PageSize);
        string AddEditSubjectService(Subject ObjSubject);
        string InsertResultDescriptionService(ExamDescription ObjExamDescription);
        string MoveMenuItemService(MenuAndRolesModal menuAndRolesModal);
        string ExamDataInsertion(Examdetails ObjExamdetails);
        string ExamDescriptionService();
        string GetExamDetail(int Year);
    }
}
