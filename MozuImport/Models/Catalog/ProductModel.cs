using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MozuImport.Models.Catalog
{
    public class ProductModel : PropertyContainer
    {
        public ProductModel()
            : base()
        {
        }

        public string WorkBookSection { get; set; }
        public string WorkBookSubsection { get; set; }
        public string Categories { get; set; }
        public string CategoryIds { get; set; }
    }
}
