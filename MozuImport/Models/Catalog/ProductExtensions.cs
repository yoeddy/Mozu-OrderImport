using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mozu.Api.Contracts.ProductRuntime;
//using Mozu.Api.Contracts.CommerceRuntime.Returns;

namespace MozuImport.Models
{
    public static class ProductExtensions
    {
        public static ProductProperty AddProperty(this Product product, string propertyName, string value, bool isMultiValue=false )
        {
            if (product.Properties == null)
            {
                product.Properties = new List<ProductProperty>();
            }

            ProductProperty productProperty =
                    product.Properties.FirstOrDefault(p => p.AttributeFQN == "tenant~" + propertyName);

            if (productProperty == null)
            {
                // Add a new ProductProperty
                productProperty = new ProductProperty();
                productProperty.AttributeFQN = "tenant~" + propertyName;
                productProperty.AttributeDetail.DataType = "text";
                productProperty.IsMultiValue = isMultiValue;
                productProperty.AttributeFQN = propertyName;
                product.Properties.Add(productProperty);
            }

            // Add the property value
            productProperty.Values = new List<Mozu.Api.Contracts.ProductRuntime.ProductPropertyValue>();
            productProperty.Values.Add(new ProductPropertyValue(){StringValue = value});

            return productProperty;
        }

        public static string Property(this Mozu.Api.Contracts.ProductRuntime.Product product, string propertyName)
        {
            var styleProp = product.Properties.FirstOrDefault(p => p.AttributeFQN == "tenant~" + propertyName);
            if (styleProp != null)
            {
                string text = string.Empty;
                foreach (var item in styleProp.Values)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        text = " " + text; // prepend a space
                    }

                    text += item.StringValue;
                }
                return text;
            }
            return null;
        }

        public static string StripInvalidValueCharacters(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            else return text.Replace(" ", "_")
                .Replace("-", "_")
                .Replace("/", "_")
                .Replace("&", "_")
                .Replace(".", "_")
                .Replace("(", "_")
                .Replace(")", "_")
                .Replace("®", "")
                .Replace("™", "")
                .Replace("’", "")
                .Replace("'", "");
        }
    }
}
