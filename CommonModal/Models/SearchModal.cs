using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModal.Models
{
    public class SearchModal
    {
        public string SearchString { set; get; }
        public string SortBy { set; get; }
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
    }
}
