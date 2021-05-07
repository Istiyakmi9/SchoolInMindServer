using BottomhalfCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class MenuAndRolesModal
    {
        public string OldCatagory { set; get; }
        public string Category { set; get; }
        public string MenuName { set; get; }
        public string AccessLevelUid { set; get; }
        public int PermissionLevel { set; get; }
        public int AccessCode { set; get; }
        public bool IsActive { set; get; }
        public string Childs { set; get; }
    }

    public class MenuAndRoles
    {
        public string AccessLevelUid { set; get; }
        public int AccessCode { set; get; }
        [Required]
        public string RoleName { set; get; }
        public string RoleDescription { set; get; }
        public List<MenuAndRolesModal> MenuAndRolesModal { set; get; }
    }

    public class RoleModal
    {
        public int AccessUid { set; get; }
        public int AccessCode { set; get; }
        public int PermissionLevel { set; get; }
    }
}
