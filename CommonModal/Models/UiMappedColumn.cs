using CommonModal.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CommonModal.Models
{
    public class Columns
    {
        public string column { set; get; }
        public string header { set; get; }
        public string width { set; get; }
        public string type { set; get; } = "";
    }

    public class UiMappedColumn
    {
        public static DataTable GetTable()
        {
            DataTable uiColumnMappingTable = new DataTable("Column");
            uiColumnMappingTable.Columns.Add("column", typeof(string));
            uiColumnMappingTable.Columns.Add("header", typeof(string));
            uiColumnMappingTable.Columns.Add("width", typeof(string));
            uiColumnMappingTable.Columns.Add("type", typeof(string));
            return uiColumnMappingTable;
        }

        public static DataSet BuildColumn<T>(DataSet ds)
        {
            DataTable uiColumnMappingTable = GetTable();
            if (ds != null && ds.Tables != null && ds.Tables.Count == 2)
            {
                ds.Tables[0].TableName = "Rows";
                ds.Tables[1].TableName = "Total";

                Type type = typeof(T);
                if (type == typeof(Student))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "Index", "S.No#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "FName", "Student Name", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "ClassSection", "Class-Sec", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "emailId", "Email", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "mobilenumber", "Mobile", null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "registrationno", "Reg No.#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "rollno", "Roll No.#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "studentUid", null, null, ApplicationConstant.Hidden });
                }
                else if (type == typeof(GradeDetail))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "Grade", "Grade", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "MaxMarks", "Max", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "MinMarks", "Min", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Description", "Desc", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "GradeUid", null, null, ApplicationConstant.Hidden });
                }
                else if (type == typeof(NoticeBoard))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "RowIndex", "S.No#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "SchoolNotificationId", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "NotificationHeadline", "Topic", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "NotificationDetail", "Description", "25%", null });
                    uiColumnMappingTable.Rows.Add(new[] { "IsForSchool", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "IsForClass", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "IsForSection", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "FirstName", "First name", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "LastName", "Last name", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Class", "Class", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Section", "Sec", null, null });

                }
                if (type == typeof(ParentDetails))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "Index", "S.No#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "FName", "Student Name", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "ClassSection", "Class-Sec", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "emailId", "Email", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "mobilenumber", "Mobile", null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "registrationno", "Reg No.#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "rollno", "Roll No.#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "studentUid", null, null, ApplicationConstant.Hidden });
                }
                else if (type == typeof(Faculty))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "StaffMemberUid", "S.No#", null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "FName", "Faculty Name", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "MobileNumber", "Mob. No.#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Class", "Class", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Section", "Sec", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "Email", "Email", null, null });
                }
                else if (type == typeof(Announcement))
                {
                    uiColumnMappingTable.Rows.Add(new[] { "RowIndex", "S.No#", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "AnnouncementId", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "Title", "Title", null, null });
                    uiColumnMappingTable.Rows.Add(new[] { "AnnouncementMessage", "Message", "40%", null });
                    uiColumnMappingTable.Rows.Add(new[] { "ClassDetailId", null, null, ApplicationConstant.Hidden });
                    uiColumnMappingTable.Rows.Add(new[] { "CreatedOn", "Date", null, null });
                }
            }
            ds.Tables.Add(uiColumnMappingTable);
            return ds;
        }
    }
}