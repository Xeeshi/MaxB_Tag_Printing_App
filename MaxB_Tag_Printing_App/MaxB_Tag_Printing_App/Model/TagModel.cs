using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxB_Tag_Printing_App.Model
{
  public  class TagModel
    {
        public string L2 { get; set; }
        public string AltBarcode { get; set; }
        public string Barcode { get; set; }
        public string ProductItemID { get; set; }
        public string MOQ { get; set; }
        public string MOQUnit { get; set; }
        public string LongName { get; set; }
        public string Target { get; set; }
        public string TagType { get; set; }
        public string BranchID { get; set; }
        public string SaleRate { get; set; }
        public string Facings { get; set; }

        public decimal BQty { get; set; }




    }
}
