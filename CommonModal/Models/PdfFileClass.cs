using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonModal.Models
{
    public class PdfFileClass
    {
        public string StoreName { set; get; }
        public string OwnerName { set; get; }
        public string StoreLicenceCode { set; get; }
        public string BillNo { set; get; }
        public DateTime BillDate { set; get; }
        public string Gross_Total { set; get; }
        public string Dicount { set; get; }
        public string Tax { set; get; }
        public string Net_Total { set; get; }

    }
}