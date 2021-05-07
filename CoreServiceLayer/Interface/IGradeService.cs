using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServiceLayer.Interface
{
    public interface IGradeService
    {
        string InsertUpdateGradeService(GradeDetail gradeDetail);
        string GetGradesService(SearchModal searchModal);
    }
}
