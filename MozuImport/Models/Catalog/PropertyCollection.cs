using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MozuImport.Models.Catalog
{
    public class PropertyCollection: Dictionary<string, PropertyContainer>
    {
        public string CollectionName { get; set; }
        public IEnumerable<string> Headers { get; set; }

        public PropertyCollection(string collectionName) : base()
        {
            CollectionName = collectionName;
        }


    }
}
