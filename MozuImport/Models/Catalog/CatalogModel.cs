using System;
using System.Globalization;
using Mozu.Api;

namespace MozuImport.Models.Catalog
{
    public class CatalogModel
    {
        private readonly IApiContext _apiContext = null;
        public string WorkbookFileName { get; set; }
        public string MasterCatalogName { get; set; }
        public string CatalogName { get; set; }

        public PropertyCollection Attributes { get; private set; }
        public PropertyCollection AttributeValues { get; private set; }
        public PropertyCollection ProductTypes { get; private set; }
        public PropertyCollection ProductTypeAttributes { get; private set; }
        public PropertyCollection Categories { get; private set; }
        public PropertyCollection Products { get; private set; }
        public PropertyCollection ProductOptions { get; private set; }
        public PropertyCollection ProductExtras { get; private set; }
        public PropertyCollection ProductCatalogs { get; private set; }
        public PropertyCollection ProductBundles { get; private set; } 
        public PropertyCollection ProductImages { get; private set; }
        public PropertyCollection LocationInventory { get; private set; }
        public PropertyCollection Discounts { get; private set; }
        public PropertyCollection ProductImageVariations { get; private set; }

        public CatalogModel(IApiContext apiContext)
        {
            // create ALL the things!
            _apiContext = apiContext;
            this.MasterCatalogName = "Basic Store SiteGroup";
            this.CatalogName = "Basic Store Site";

            this.Attributes = _getStandardAttributes();
            this.AttributeValues = _createAttributeValues();
            this.ProductTypes = _getStandardProductTypes();
            this.ProductTypeAttributes = _getStandardProductTypeAttributes();
            this.Categories = _getStandardCategories();
            this.Products = _createProductsCollection();
            this.ProductCatalogs = _createProductCatalogsCollection();
            this.ProductOptions = _createProductOptionsCollection();
            this.ProductExtras = _createProductExtrasCollection();
            this.ProductBundles = _createProductBundlesCollection();
            this.ProductImages = _createProductImagesCollection();
            this.ProductImageVariations = _createProductImageVariationsCollection();
            this.LocationInventory = _createLocationInventoryCollection();
            this.Discounts = _createDiscountsCollection();
        }

        private PropertyCollection _createProductImageVariationsCollection()
        {
            this.ProductBundles = new PropertyCollection("ProductImageVariations");
            this.ProductBundles.Headers = new String[]{
                "ProductCode",
                "StyleColor",
                "Color",
                "ImageQuery",
                "FileName",
                "VariationCode"
            };

            return this.ProductBundles;
        }

        private PropertyCollection _createProductBundlesCollection()
        {
            this.ProductBundles = new PropertyCollection("ProductBundles");
            this.ProductBundles.Headers = new String[]{
                "ProductCode",
                "Code",
                "Name",
                "Quantity"
            };

            return this.ProductBundles;
        }

        private PropertyCollection _createProductCatalogsCollection()
        {
            this.ProductCatalogs = new PropertyCollection("ProductCatalogs");
            this.ProductCatalogs.Headers = new string[]{
                "CatalogName",
                "ProductCode",
                "IsActive",
                "ProductName",
                "ISOCurrencyCode",
                "Price",
                "SalePrice",
                "MSRP",
                "MAP",
                "MAPEffectiveStartDate",
                "MAPEffectiveEndDate",
                "ProductShortDescription",
                "ContentFullProductDescription",
                "SEOMetaTagTitle",
                "SEOMetaTagDescription",
                "SEOMetaTagKeywords",
                "SEOFriendlyURL",
                "Categories",
                "CategoryIds"
            };

            return this.ProductCatalogs;
        }

        private PropertyCollection _createProductExtrasCollection()
        {
            this.ProductExtras = new PropertyCollection("ProductExtras");
            this.ProductExtras.Headers = new string[]{
                "MasterCatalogName",
                "ProductCode",
                "AttributeCode",
                "Value",
                "ExtraCost",
                "RequiredByShopper"
            };

            return this.ProductExtras;
        }

        private PropertyCollection _createProductImagesCollection()
        {
            this.ProductImages = new PropertyCollection("ProductImages");
            this.ProductImages.Headers = new string[]{
                "MasterCatalogName",
                "ProductCode",
                "Name",
                "LocaleCode",
                "Sequence",
                "AltText",
                "ImageUrl",
                "ImageLabel",
                "ImagePath",
                "VideoUrl"
            };

            return this.ProductImages;
        }

        private PropertyCollection _createDiscountsCollection()
        {
            this.Discounts = new PropertyCollection("Discounts");
            this.Discounts.Headers = new string[]{
                "Id",
                "SiteName",
                "DiscountName",
                "LocaleCode",
                "Scope",
                "AppliesTo",
                "Type",
                "TypeValue",
                "StartDate",
                "EndDate",
                "MinimumOrderAmount",
                "CouponCode",
                "MaxRedemptionCount",
                "Status",
                "AppliesWithProducts",
                "AppliesWithCategories",
                "AppliesToAllProducts",
                "IncludedProducts",
                "ExcludedProducts",
                "IncludedCategories",
                "ExcludedCategories"
            };
            return this.Discounts;
        }

        private PropertyCollection _createLocationInventoryCollection()
        {
            this.LocationInventory = new PropertyCollection("LocationInventory");
            this.LocationInventory.Headers = new string[]{
                "MasterCatalogName",
                "ProductCode",
                "LocationCode",
                "StockOnHand",
                "StockUpdateOption",
                "StockOnBackOrder",
                "StockAvailable"
            };

            return this.LocationInventory;
        }

        private PropertyCollection _createProductOptionsCollection()
        {
            this.ProductOptions = new PropertyCollection("ProductOptions");
            this.ProductOptions.Headers = new string[]{
                "MasterCatalogName",
                "ProductCode",
                "VariationCode",
                "Enabled",
                "ExtraCost",
                "ExtraWeight",
                "Color",
                "Size"
            };

            return this.ProductOptions;
        }

        private PropertyCollection _createProductsCollection()
        {
            this.Products = new PropertyCollection("Products");
            this.Products.Headers = new string[]{
                "MasterCatalogName",
                "ProductCode",
                "ProductTypeName",
                "ProductUsage",
                "ProductName",
                "ContentLocaleCode",
                "ISOCurrencyCode",
                "Price",
                "SalePrice",
                "Cost",
                "MSRP",
                "MAP",
                "MAPEffectiveStartDate",
                "MAPEffectiveEndDate",
                "RestrictDiscount",
                "RestrictDiscountStartDate",
                "RestrictDiscountEndDate",
                "ManufacturerPartNumber",
                "UPC",
                "DistributorPartNumber",
                "IsTaxable",
                "ManageStock",
                "OutOfStockBehavior",
                "PackageWeight",
                "PackageLength",
                "PackageWidth",
                "PackageHeight",
                "ProductShortDescription",
                "ContentFullProductDescription",
                "SEOMetaTagTitle",
                "SEOMetaTagDescription",
                "SEOMetaTagKeywords",
                "SEOFriendlyURL",
                "availability",
                "product-crosssell",
                "product-upsell",
                "Brand",
                "SubCategory",
                "Features",
                "Gender",
                "Collection",
                "Activity",
                "Style",
                "Technical_Specs",
                "CustomImageMap"
            };
            return this.Products;
        }

        private PropertyCollection _getStandardCategories()
        {
            this.Categories = new PropertyCollection("Categories");
            this.Categories.Headers = new string[]
            {
                "CatalogName",
                "Id",
                "ParentCategoryId",
                "CategoryName",
                "IsDisplayed",
                "LocaleCode",
                "MetaTagTitle",
                "MetaTagDescription",
                "PageTitle",
                "CategoryDescription",
                "MetaTagKeyWords"
            };

            return this.Categories;
        }

        private PropertyContainer _createCategory(int id, int parentId, string categoryName, bool isDisplayed,
            string pageTitle = null, string description = null, string metaTagTitle = null, string metaTagDescription = null, string metaTagKeywords = null)
        {
            var attr = new PropertyContainer();
            attr["CatalogName"] = this.CatalogName;
            attr["Id"] = (id > 0) ? id.ToString() : null;
            attr["ParentCategoryId"] = (parentId > 0) ? parentId.ToString() : null;
            attr["CategoryName"] = categoryName;
            attr["CategoryDescription"] = description ?? categoryName;
            attr["IsDisplayed"] = (isDisplayed) ? "Yes" : "No";
            attr["LocaleCode"] = "en-US";
            attr["MetaTagTitle"] = categoryName;
            attr["MetaTagDescription"] = metaTagDescription ?? categoryName + " Apparel and Activewear";
            attr["PageTitle"] = pageTitle ?? categoryName;
            attr["MetaTagKeyWords"] = metaTagKeywords;

            string key = id.ToString() + "~" + categoryName;
            this.Categories.Add(key, attr);

            return attr;
        }

        private PropertyCollection _getStandardProductTypeAttributes()
        {
            this.ProductTypeAttributes = new PropertyCollection("ProductTypeAttributes");
            this.ProductTypeAttributes.Headers = new string[]{
                "MasterCatalogName",
                "ProductType",
                "AttributeCode",
                "VocabularyValues",
                "Type",
                "IsRequiredByAdmin",
                "IsHiddenProperty",
                "IsMultiValueProperty",
                "VocabularyValueLocaleCode",
                "Order"
            };

            var attr = _addProductTypeAttribute("Base", "availability", null,
                "Property", false, false, false, 1);
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 24 Hours");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 24 to 48 Hours");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 24 to 72 Hours");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 1 to 2 Days");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 2 to 3 Days");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 1 to 2 Weeks");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 2 to 3 Weeks");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 2 to 4 Weeks");
            AddToProductTypeAttribute("Base", "availability", "Usually Ships in 4 to 6 Weeks");

            attr = _addProductTypeAttribute("Base", "product-crosssell", null,
                "Property", false, true, true, 2);
            attr = _addProductTypeAttribute("Base", "product-upsell", null,
                "Property", false, true, true, 3);

            attr = _addProductTypeAttribute("Apparel Product", "availability",
                "Inherited from Base. Do not add values here.",
                "Property", false, true, true, 1);
            attr = _addProductTypeAttribute("Apparel Product", "product-crosssell",
                "Inherited from Base. Do not add values here.",
                "Property", false, true, true, 2);
            attr = _addProductTypeAttribute("Apparel Product", "product-upsell",
                "Inherited from Base. Do not add values here.",
                "Property", false, true, true, 3);
            attr = _addProductTypeAttribute("Apparel Product", "Color",
                null, "Option", true, false, false, 4);
            attr = _addProductTypeAttribute("Apparel Product", "Size",
                null, "Option", true, false, false, 5);
            attr = _addProductTypeAttribute("Apparel Product", "Gift_Wrapped",
                null, "Extra", false, false, false, 6);
            
            attr = _addProductTypeAttribute("Apparel Product", "Brand",
                null, "Property", false, false, false, 7);

            attr = _addProductTypeAttribute("Apparel Product", "SubCategory",
                null, "Property", false, false, false, 8);

            attr = _addProductTypeAttribute("Apparel Product", "Gender",
                null, "Property", false, false, false, 9);
            AddToProductTypeAttribute("Apparel Product", "Gender", "Male");
            AddToProductTypeAttribute("Apparel Product", "Gender", "Female");
            AddToProductTypeAttribute("Apparel Product", "Gender", "Unisex");

            attr = _addProductTypeAttribute("Apparel Product", "Activity",
                null, "Property", false, false, true, 10);
            AddToProductTypeAttribute("Apparel Product", "Activity", "Training");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Running");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Hiking");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Climbing");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Skiing");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Snowboarding");
            AddToProductTypeAttribute("Apparel Product", "Activity", "Trekking/Travel");

            attr = _addProductTypeAttribute("Apparel Product", "Collection",
                null, "Property", false, false, false, 11);

            attr = _addProductTypeAttribute("Apparel Product", "Style",
                null, "Property", false, false, false, 12); 
            
            attr = _addProductTypeAttribute("Apparel Product", "Features",
                null, "Property", false, false, false, 13);

            attr = _addProductTypeAttribute("Apparel Product", "Technical_Specs",
                null, "Property", false, false, false, 14);

            return this.ProductTypeAttributes;
        }

        public void AddGenderOption(string gender)
        {
            if (string.IsNullOrEmpty(gender))
                return;

            AddAttributeValue("Gender", gender);
            AddToProductTypeAttribute("Apparel Product", "Gender", gender);
        }

        public void AddProductImageVariation(string productCode, string variationCode, string styleColor, string colorValue)
        {
            if (string.IsNullOrEmpty(styleColor))
                return;
            PropertyContainer model = new PropertyContainer();

            var key = styleColor;
            if (this.ProductImageVariations.ContainsKey(key))
            {
                model = this.ProductImageVariations[key];
                if (model["VariationCode"] == null)
                    model["VariationCode"] = string.Empty;
                else
                {
                    model["VariationCode"] += ",";
                }

                model["VariationCode"] += variationCode;

            }
            else
            {
                model = new PropertyContainer();
                model["ProductCode"] = productCode;
                model["StyleColor"] = styleColor;
                model["Color"] = colorValue;
                model["VariationCode"] = variationCode;

                // take apart the styleColor value and constuct an ImageQuery
                var styleCode = (styleColor.Length > 4)
                    ? styleColor.Substring(0, 4)
                    : "No";

                var colorCode = (styleColor.Length >= 7)
                    ? styleColor.Substring(4, 3)
                    : "Image";

                model["ImageQuery"] = string.Format("{0}_{1}_*_*.*", styleCode, colorCode);
                model["FileName"] = null;
                this.ProductImageVariations.Add(key, model);
            }
        }

        public void AddColorOption(string colorName)
        {
            if (string.IsNullOrEmpty(colorName))
                return;

            AddAttributeValue("Color", colorName);
            AddToProductTypeAttribute("Apparel Product", "Color", colorName);
        }

        public void AddSizeOption(string sizeName)
        {
            sizeName = sizeName.Trim(); 
            if (string.IsNullOrEmpty(sizeName))
                return;

            AddAttributeValue("Size", sizeName);
            AddToProductTypeAttribute("Apparel Product", "Size", sizeName);
        }

        public void AddSubCategory(string typeName)
        {
            typeName = typeName.Trim();
            if (string.IsNullOrEmpty(typeName))
                return;

            AddAttributeValue("SubCategory", typeName);
            AddToProductTypeAttribute("Apparel Product", "SubCategory", typeName);
        }

        public void AddCollection(string collectionName)
        {
            collectionName = collectionName.Trim();
            if (string.IsNullOrEmpty(collectionName))
                return;

            AddAttributeValue("Collection", collectionName);
            AddToProductTypeAttribute("Apparel Product", "Collection", collectionName);
        }

        public void AddBrandSelection(string brandName)
        {
            brandName = brandName.Trim();
            if (string.IsNullOrEmpty(brandName))
                return;

            AddAttributeValue("Brand", brandName);
            AddToProductTypeAttribute("Apparel Product", "Brand", brandName);
        }

        private PropertyContainer _addProductTypeAttribute(string productType, string attributeCode, string values, string type, bool isRequiredByAdmin, bool isHiddenProperty, bool isMultiValueProperty, int order)
        {
            var attr = new PropertyContainer();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["ProductType"] = productType;
            attr["AttributeCode"] = attributeCode;
            attr["VocabularyValues"] = values;
            attr["Type"] = type;
            attr["IsRequiredByAdmin"] = (isRequiredByAdmin) ? "Yes" : "No";
            attr["IsHiddenProperty"] = (isHiddenProperty) ? "Yes" : "No";
            attr["IsMultiValueProperty"] = (isMultiValueProperty) ? "Yes" : "No";
            attr["VocabularyValueLocaleCode"] = null;
            attr["Order"] = order.ToString();

            string key = productType + "~" + attributeCode;
            this.ProductTypeAttributes.Add(key, attr);
            return attr;
        }

        public PropertyContainer AddToProductTypeAttribute(string productType, string attributeCode, string vocabularyValue)
        {
            string key = productType + "~" + attributeCode;
            var attr = this.ProductTypeAttributes[key];
            var values = attr["VocabularyValues"];

            if (values != null && values.Contains(vocabularyValue + "\r\n"))
            {
                // value is already here -- don't bother re-adding
                return attr;
            }

            if (!string.IsNullOrEmpty(values))
            {
                if (!values.EndsWith("\r\n"))
                    values += "\r\n";

                values += vocabularyValue + "\r\n";
            }
            else
            {
                values = vocabularyValue + "\r\n";
            }
            
            attr["VocabularyValues"] = values;

            return attr;
        }

        private PropertyCollection _getStandardProductTypes()
        {
            var productTypes = new PropertyCollection("ProductTypes");
            productTypes.Headers = new string[]{
                "MasterCatalogName",
                "ProductType",
                "Standard",
                "Configurable",
                "Bundle",
                "Component"
            };

            var model = new ProductTypeModel();
            model["MasterCatalogName"] = this.MasterCatalogName;
            model["ProductType"] = "Base";
            model["Standard"] = "Yes";
            model["Configurable"] = "Yes";
            model["Bundle"] = "Yes";
            model["Component"] = "Yes";
            productTypes.Add(model["ProductType"], model);

            model = new ProductTypeModel();
            model["MasterCatalogName"] = this.MasterCatalogName;
            model["ProductType"] = "Apparel Product";
            model["Standard"] = "No";
            model["Configurable"] = "Yes";
            model["Bundle"] = "No";
            model["Component"] = "No";
            productTypes.Add(model["ProductType"], model);

            return productTypes;
        }

        private PropertyCollection _getStandardAttributes()
        {
            var attributes = new PropertyCollection("Attributes");
            attributes.Headers = new string[]{
                "MasterCatalogName",
                "AttributeCode",
                "AttributeName",
                "ContentDescription",
                "ContentLocaleCode",
                "DataType",
                "InputType",
                "IsExtra",
                "IsOption",
                "IsProperty",
                "Namespace",
                "Order"
            };
            var attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Availability";
            attr["AttributeCode"] = "availability";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Product Cross-Sells";
            attr["AttributeCode"] = "product-crosssell";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextBox";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Product Upsells";
            attr["AttributeCode"] = "product-upsell";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextBox";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Color";
            attr["AttributeCode"] = "Color";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "Yes";
            attr["IsProperty"] = "No";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Size";
            attr["AttributeCode"] = "Size";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "Yes";
            attr["IsProperty"] = "No";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Brand";
            attr["AttributeCode"] = "Brand";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "ProductType";
            attr["AttributeCode"] = "SubCategory";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Features";
            attr["AttributeCode"] = "Features";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextArea";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Gift Wrapped";
            attr["AttributeCode"] = "Gift_Wrapped";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "Bool";
            attr["InputType"] = "YesNo";
            attr["IsExtra"] = "Yes";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "No";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Gender";
            attr["AttributeCode"] = "Gender";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Collection";
            attr["AttributeCode"] = "Collection";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Activity";
            attr["AttributeCode"] = "Activity";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "List";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Style";
            attr["AttributeCode"] = "Style";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextBox";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "Technical Specs";
            attr["AttributeCode"] = "Technical_Specs";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextArea";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);

            attr = new AttributeModel();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["AttributeName"] = "CustomImageMap";
            attr["AttributeCode"] = "CustomImageMap";
            attr["ContentDescription"] = null;
            attr["ContentLocaleCode"] = "en-US";
            attr["DataType"] = "String";
            attr["InputType"] = "TextArea";
            attr["IsExtra"] = "No";
            attr["IsOption"] = "No";
            attr["IsProperty"] = "Yes";
            attr["Namespace"] = "Tenant";
            attr["Order"] = null;
            attributes.Add(attr["AttributeCode"], attr);  

            return attributes;
        }

        private PropertyCollection _createAttributeValues()
        {
            this.AttributeValues = new PropertyCollection("AttributeValues");
            this.AttributeValues.Headers = new string[]{
                "MasterCatalogName",
                "AttributeCode",
                "Value",
                "en-US"
            };
            AddAttributeValue("availability", "24hrs", "Usually Ships in 24 Hours");
            AddAttributeValue("availability", "24-48hrs", "Usually Ships in 24 to 48 Hours");
            AddAttributeValue("availability", "24-72hrs", "Usually Ships in 24 to 72 Hours");
            AddAttributeValue("availability", "1-2days", "Usually Ships in 1 to 2 Days");
            AddAttributeValue("availability", "2-3days", "Usually Ships in 2 to 3 Days");
            AddAttributeValue("availability", "1-2weeks", "Usually Ships in 1 to 2 Weeks");
            AddAttributeValue("availability", "2-3weeks", "Usually Ships in 2 to 3 Weeks");
            AddAttributeValue("availability", "2-4weeks", "Usually Ships in 2 to 4 Weeks");
            AddAttributeValue("availability", "4-6weeks", "Usually Ships in 4 to 6 Weeks");

            AddAttributeValue("Activity", "Training");
            AddAttributeValue("Activity", "Running");
            AddAttributeValue("Activity", "Hiking");
            AddAttributeValue("Activity", "Climbing");
            AddAttributeValue("Activity", "Skiing");
            AddAttributeValue("Activity", "Snowboarding");
            AddAttributeValue("Activity", "Trekking/Travel");

            return this.AttributeValues;
        }

        public AttributeValueModel AddAttributeValue(string attributeCode, string displayValue)
        {
            string value = displayValue.StripInvalidValueCharacters();
            return AddAttributeValue(attributeCode, value, displayValue);
        }

        public AttributeValueModel AddAttributeValue(string attributeCode, string value, string displayValue)
        {
            string key = attributeCode + "_" + value;

            if (!this.AttributeValues.ContainsKey(key))
            {
                var attr = new AttributeValueModel();
                attr["MasterCatalogName"] = this.MasterCatalogName;
                attr["AttributeCode"] = attributeCode;
                attr["Value"] = value;
                attr["en-US"] = displayValue;
                this.AttributeValues.Add(key, attr);
            }
            return (AttributeValueModel)this.AttributeValues[key];
        }

        public ProductOptionModel AddProductOption(string productCode, string variationCode, string color, string size,
            bool enabled = true, string extraCost = "0.00", string extraWeight = "0.00")
        {
            ProductOptionModel attr = null;
            if (this.ProductOptions.ContainsKey(variationCode))
            {
                // variation already added
                attr = (ProductOptionModel) this.ProductOptions[variationCode];
            }
            else
            {
                attr = new ProductOptionModel();
                attr["MasterCatalogName"] = this.MasterCatalogName;
                attr["ProductCode"] = productCode;
                attr["VariationCode"] = variationCode;
                attr["Color"] = color;
                attr["Size"] = size;
                attr["Enabled"] = (enabled) ? "Yes" : "No";
                attr["ExtraCost"] = extraCost;
                attr["ExtraWeight"] = extraWeight;
                this.ProductOptions.Add(variationCode, attr);
            }

            // Add to color and size options list
            this.AddColorOption(color);
            this.AddSizeOption(size);

            return attr;
        }

        public LocationInventoryModel AddLocationInventory(string variationCode, int stockAvailable, int stockOnHand,
            string locationCode = "homebase",  int stockOnBackOrder = 0, string stockUpdateOption = null)
        {
            LocationInventoryModel attr = null;
            if (this.LocationInventory.ContainsKey(variationCode))
            {
                // variation already added
                attr = (LocationInventoryModel) this.LocationInventory[variationCode];
            }
            else
            {
                attr = new LocationInventoryModel();
                attr["MasterCatalogName"] = this.MasterCatalogName;
                attr["ProductCode"] = variationCode; // variation code is assigned to ProductCode column
                attr["LocationCode"] = locationCode;
                attr["StockOnHand"] = stockOnHand.ToString("0");
                attr["StockUpdateOption"] = stockUpdateOption;
                attr["StockOnBackOrder"] = stockOnBackOrder.ToString("0");
                attr["StockAvailable"] = stockAvailable.ToString("0");
                this.LocationInventory.Add(variationCode, attr);
            }

            return attr;
        }

        public PropertyContainer AddProductExtra(string productCode, string attributeCode, string value, double extraCost, bool required)
        {
            var attr = new PropertyContainer();
            attr["MasterCatalogName"] = this.MasterCatalogName;
            attr["ProductCode"] = productCode;
            attr["AttributeCode"] = attributeCode;
            attr["Value"] = value;
            attr["ExtraCost"] = extraCost.ToString("0.00");
            attr["RequiredByShopper"] = (required) ? "Yes" : "No";

            string key = productCode + "~" + attributeCode;
            this.ProductExtras.Add(key, attr);
            return attr;
        }
    }
}
