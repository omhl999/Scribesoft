namespace MelissaData.Connector.MDServices
{
    using Scribe.Core.ConnectorApi;
    using Scribe.Core.ConnectorApi.Metadata;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    //-----------------------------------------------------------------------------------------------------
    public class MetadataProvider : IMetadataProvider
    {
        private IEnumerable<IActionDefinition> actionDefinitions;

        private IEnumerable<IObjectDefinition> objectDefinitions;
        private Uri serviceUri;

        private Uri MetaDataUri;

        //-----------------------------------------------------------------------------------------------------
        public MetadataProvider(Uri serviceUri)
        {
            this.serviceUri = serviceUri;
            string uri = serviceUri.ToString() + "/$metadata";
            this.MetaDataUri = new Uri(uri);
        }

        private IEnumerable<IActionDefinition> ActionDefinitions { get { return this.actionDefinitions ?? (this.actionDefinitions = this.GetActionDefinitions()); } }

        private IEnumerable<IObjectDefinition> ObjectDefinitions { get { return this.objectDefinitions ?? (this.objectDefinitions = this.GetObjectDefinitions()); } }

        //-----------------------------------------------------------------------------------------------------
        private IEnumerable<IActionDefinition> GetActionDefinitions()
        {
            return new List<IActionDefinition>
{
                new ActionDefinition()
                {
                FullName = "BulkExecute",
                Description = "Concurrent Processing",
                KnownActionType = KnownActions.Create,
                Name = "BulkExecute",
                SupportsBulk = true,
                SupportsConstraints = true,
                SupportsInput = true,
                SupportsLookupConditions = true,
                SupportsMultipleRecordOperations = true,
                SupportsRelations = true,
                SupportsSequences = true
                }
                ,
                new ActionDefinition()
                {
                FullName = "BulkQuery",
                Description = "Concurrent Processing",
                KnownActionType = KnownActions.Query,
                Name = "BulkQuery",
                SupportsBulk = true,
                SupportsConstraints = true,
                SupportsInput = true,
                SupportsLookupConditions = true,
                SupportsMultipleRecordOperations = true,
                SupportsRelations = true,
                SupportsSequences = true
                }
                ,
                new ActionDefinition
                {
                FullName = "Execute",
                Description = "Processes Information",
                KnownActionType = KnownActions.Create,
                Name = "Execute",
                SupportsBulk = false,
                SupportsConstraints = true,
                SupportsInput = true,
                SupportsLookupConditions = true,
                SupportsMultipleRecordOperations = true,
                SupportsRelations = true,
                SupportsSequences = true
                }
                };
        }

        //-----------------------------------------------------------------------------------------------------
        private IEnumerable<IObjectDefinition> GetObjectDefinitions()
        {
            PropertyDefinitionsGlobal = new List<IPropertyDefinition>();
            PropertyDefinitionsGlobal.Add(new PropertyDefinition
            {
                Name = "Setting_DeliveryLines",
                FullName = "Setting_DeliveryLines",
                Description = "(OFF,ON),The options allows you to specify if the Address Lines 1-8 should contain just the delivery address or the entire address.",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = true,
                Size = 4096
            });

            PropertyDefinitionsGlobal.Add(new PropertyDefinition
      {
          Name = "Setting_LineSeperator",
          FullName = "Setting_LineSeperator",
          Description = "(Semicolon, pipe, cr, lf, crlf, tab , br)This is the line separator used for the FormattedAddress result.",
          IsPrimaryKey = false,
          Nullable = false,
          PresentationType = "string",
          PropertyType = typeof(string).Name,
          UsedInActionInput = true,
          UsedInActionOutput = true,
          UsedInQuerySelect = true,
          Size = 4096
      });
            PropertyDefinitionsGlobal.Add(new PropertyDefinition
         {
             Name = "Setting_OutputScript",
             FullName = "Setting_OutputScript",
             Description = "(Nochange, latn, native)This is the script type used for all applicable fields.",
             IsPrimaryKey = false,
             Nullable = false,
             PresentationType = "string",
             PropertyType = typeof(string).Name,
             UsedInActionInput = true,
             UsedInActionOutput = true,
             UsedInQuerySelect = true,
             Size = 4096
         });
            PropertyDefinitionsGlobal.Add(new PropertyDefinition
       {
           Name = "Setting_CountryOfOrigin",
           FullName = "Setting_CountryOfOrigin",
           Description = "(Any Accept ISO code) This is used to determine whether or not to include the country name as the last line in FormattedAddress",
           IsPrimaryKey = false,
           Nullable = false,
           PresentationType = "string",
           PropertyType = typeof(string).Name,
           UsedInActionInput = true,
           UsedInActionOutput = true,
           UsedInQuerySelect = true,
           Size = 4096
       });
            foreach (IPropertyDefinition property in PropertyDefinitionsGlobal)
            {
                if (property.Name == "MD_Latitude")
                {
                    property.PresentationType = "string";
                    property.PropertyType = typeof(string).Name;
                }
                if (property.Name == "MD_Longitude")
                {
                    property.PresentationType = "string";
                    property.PropertyType = typeof(string).Name;
                }
            }

            List<PropertyInfo> responsePropertiesA = typeof(SOAP3.ResponseRecord).GetProperties().ToList();
            foreach (PropertyInfo property in responsePropertiesA)
            {
                PropertyDefinitionsGlobal.Add(new PropertyDefinition
                {
                    Name = "MD_" + property.Name,
                    FullName = "MD_" + property.Name,
                    Description = property.Name,
                    IsPrimaryKey = false,
                    Nullable = false,
                    PresentationType = "string",
                    PropertyType = typeof(string).Name,
                    UsedInActionInput = true,
                    UsedInActionOutput = true,
                    UsedInQuerySelect = true,
                    Size = 4096
                });
            }

            PropertyDefinitionsPersonator = new List<IPropertyDefinition>();
            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_Actions",
                FullName = "Setting_Actions",
                Description = "(Append, Check, Move, or Verify), separate with comma if there are multiple actions. The Check action allows you to pass in a name, address, phone number, and email address as one record. A record does not need to include all of those inputs, any combination of them, or even just one is sufficient to constitute a record and be checked. This action is available for US and Canada. Exception: Address field requires a street address and either a city + state or a 5 digit ZIP Code™ to be checked. Check looks at each of these subsets independently.The Verify action compares different groups of data to the centric group defined by the user. It verifies the record as a whole, letting you know whether each group coincides with the centric piece of data in the Melissa Data Knowledge Base. You can define fields like address, phone number, or email as the centric data against which the other groups of data are compared. Auto-detection of the centric data is also available. The Verify action returns only results codes, telling you which sections of data passed verification against the centric data and which sections did not. With Verify, you can enter records, select the centric data as the field you are most confident in, and determine the accuracy of your input information. The Verify action is available only for US addresses.The Move action allows you to update your US contact records with data returned by the Personator Web Service. The service allows for retrieving the most current address for a person or business. Thus if an old address is entered for a particular individual, Personator will return the latest address for that person, giving you the freshest and most up-to-date contact information. The Append action allows you to enrich your US contact records with data returned by the Personator Web Service. The service will return elements based on the selected point of centricity which can either be the address, email, or phone. Through the Append action, you can fill in missing information in your contacts, correct them, and ensure that each of the data elements coincide, thus giving an accurate representation of each contact record.",
                IsPrimaryKey = true,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });
            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_AdvancedAddressCorrection",
                FullName = "Setting_AdvancedAddressCorrection",
                Description = "(off, on) US only. Uses the name input to perform more advanced address corrections. This can correct or append house numbers, street names, cities, states, and ZIP codes.  ",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });
            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_UsePreferredCity",
                FullName = "Setting_UsePreferredCity",
                Description = "(off, on) For every city in the United States, there is an official name that is preferred by the U.S. Postal Service. There may be one or more unofficial or “vanity” names in use. Normally, Personator allows you to verify addresses using known vanity names. If the usePreferredCity is set to on, Personator will substitute the  preferred city name for all vanity names when it verifies an address.",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });
            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_CentricHint",
                FullName = "Setting_CentricHint",
                Description = "(Auto, Address, Phone, Email) When set to Auto, it first uses Address if available, followed by Phone if no Address is available, and lastly Email if neither Address nor Phone are available. Use this to tell the service which piece of information to use as the primary point of reference when appending data.",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });
            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_Append",
                FullName = "Setting_Append",
                Description = "(Blank, CheckError, Always) Setting the Append option to Blank will cause the service to return information only when the input address, phone, email, name or company is blank.Setting the Append option to Always will cause the service to return information all the time, regardless of whether the input address, phone, email, name or company is blank or incorrect. Setting the Append option to CheckError will cause the service to return information when there are errors to either the address, phone, email, name or company",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });

            PropertyDefinitionsPersonator.Add(new PropertyDefinition
            {
                Name = "Setting_Columns",
                FullName = "Setting_Columns",
                Description = "(Blank, GrpDemographicBasic, GrpAll, GrpCensus, GrpGeocode, GrpAddressDetails, GrpNameDetails, GrpParsedAddress, GrpParsedPhone, GrpParsedEmail) Columns are delimited with a comma. The Personator Web Service returns specific columns for input data based on your needs. At a minimum, default columns are always returned. Default columns for specific actions are designated in the Default Action column. Beyond the default columns, you can request the presence of additional columns individually by specifying their column name, or the group that contains that column.",
                IsPrimaryKey = false,
                Nullable = false,
                PresentationType = "string",
                PropertyType = typeof(string).Name,
                UsedInActionInput = true,
                UsedInActionOutput = true,
                UsedInQuerySelect = false,
                Size = 4096
            });

            List<PropertyInfo> BrequestProperties = typeof(SOAP3.RequestRecord).GetProperties().ToList();
            foreach (PropertyInfo property in BrequestProperties)
            {
                PropertyDefinitionsGlobal.Add(new PropertyDefinition
                {
                    Name = "Input_" + property.Name,
                    FullName = "Input_" + property.Name,
                    Description = "Please map your input data for " + property.Name,
                    IsPrimaryKey = false,
                    Nullable = false,
                    PresentationType = "string",
                    PropertyType = typeof(string).Name,
                    UsedInActionInput = true,
                    UsedInActionOutput = false,
                    UsedInQuerySelect = false,
                    Size = 4096
                });
            }

            List<PropertyInfo> responseProperties = typeof(SOAP.ResponseRecord).GetProperties().ToList();
            foreach (PropertyInfo property in responseProperties)
            {
                PropertyDefinitionsPersonator.Add(new PropertyDefinition
                {
                    Name = "MD_" + property.Name,
                    FullName = "MD_" + property.Name,
                    Description = property.Name,
                    IsPrimaryKey = false,
                    Nullable = false,
                    PresentationType = "string",
                    PropertyType = typeof(string).Name,
                    UsedInActionInput = true,
                    UsedInActionOutput = true,
                    UsedInQuerySelect = true,
                });
            }
            foreach (IPropertyDefinition property in PropertyDefinitionsPersonator)
            {
                if (property.Name == "MD_Latitude")
                {
                    property.PresentationType = "string";
                    property.PropertyType = typeof(string).Name;
                }
                if (property.Name == "MD_Longitude")
                {
                    property.PresentationType = "string";
                    property.PropertyType = typeof(string).Name;
                }
                if (property.Name == "MD_AddressDeliveryInstallation")
                {
                    property.Size = 20;
                }

                if (property.Name == "MD_AddressExtras")
                {
                    property.Size = 50;
                }
                if (property.Name == "MD_AddressHouseNumber")
                {
                    property.Size = 4;
                }
                if (property.Name == "MD_AddressKey")
                {
                    property.Size = 11;
                }
                if (property.Name == "MD_AddressLine1")
                {
                    property.Size = 50;
                }
                if (property.Name == "MD_AddressLine2")
                {
                    property.Size = 50;
                }
                if (property.Name == "MD_AddressLockBox")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_AddressPostDirection")
                {
                    property.Size = 5;
                }
                if (property.Name == "MD_AddressPreDirection")
                {
                    property.Size = 5;
                }
                if (property.Name == "MD_AddressPrivateMailboxName")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_AddressPrivateMailboxRange")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_AddressRouteService")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_AddressStreetName")
                {
                    property.Size = 40;
                }
                if (property.Name == "MD_AddressStreetSuffix")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_AddressSuiteName")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_AddressSuiteNumber")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_AddressTypeCode")
                {
                    property.Size = 31;
                }
                if (property.Name == "MD_AreaCode")
                {
                    property.Size = 3;
                }
                if (property.Name == "MD_CarrierRoute")
                {
                    property.Size = 4;
                }
                if (property.Name == "MD_CBSACode")
                {
                    property.Size = 5;
                }
                if (property.Name == "MD_CBSADivisionCode")
                {
                    property.Size = 5;
                }
                if (property.Name == "MD_CBSADivisionLevel")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_CBSADivisionTitle")
                {
                    property.Size = 55;
                }
                if (property.Name == "MD_CBSALevel")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_CBSATitle")
                {
                    property.Size = 55;
                }
                if (property.Name == "MD_CensusBlock")
                {
                    property.Size = 4;
                }
                if (property.Name == "MD_CensusTract")
                {
                    property.Size = 6;
                }
                if (property.Name == "MD_City")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_CityAbbreviation")
                {
                    property.Size = 13;
                }
                if (property.Name == "MD_CompanyName")
                {
                    property.Size = 128;
                }
                if (property.Name == "MD_CongressionalDistrict")
                {
                    property.Size = 2;
                }
                if (property.Name == "MD_CountryCode")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_CountryName")
                {
                    property.Size = 25;
                }
                if (property.Name == "MD_CountyFIPS")
                {
                    property.Size = 5;
                }
                if (property.Name == "MD_CountyName")
                {
                    property.Size = 25;
                }
                if (property.Name == "MD_DeliveryIndicator")
                {
                    property.Size = 1;
                }
                if (property.Name == "MD_DeliveryPointCheckDigit")
                {
                    property.Size = 11;
                }
                if (property.Name == "MD_DeliveryPointCode")
                {
                    property.Size = 2;
                }
                if (property.Name == "MD_DomainName")
                {
                    property.Size = 50;
                }
                if (property.Name == "MD_EmailAddress")
                {
                    property.Size = 75;
                }
                if (property.Name == "MD_ExtensionData")
                {
                    property.Size = 256;
                }
                if (property.Name == "MD_Gender")
                {
                    property.Size = 1;
                }
                if (property.Name == "MD_Gender2")
                {
                    property.Size = 1;
                }
                if (property.Name == "MD_Latitude")
                {
                    property.Size = 12;
                }
                if (property.Name == "MD_Longitude")
                {
                    property.Size = 12;
                }
                if (property.Name == "MD_MailboxName")
                {
                    property.Size = 25;
                }
                if (property.Name == "MD_NameFirst")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_NameFirst2")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_NameFull")
                {
                    property.Size = 256;
                }
                if (property.Name == "MD_NameLast")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_NameLast2")
                {
                    property.Size = 35;
                }
                if (property.Name == "MD_NameMiddle")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NameMiddle2")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NamePrefix")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NamePrefix2")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NameSuffix")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NameSuffix2")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_NewAreaCode")
                {
                    property.Size = 3;
                }
                if (property.Name == "MD_PhoneExtension")
                {
                    property.Size = 14;
                }
                if (property.Name == "MD_PhoneNumber")
                {
                    property.Size = 30;
                }
                if (property.Name == "MD_PhonePrefix")
                {
                    property.Size = 3;
                }
                if (property.Name == "MD_PhoneSuffix")
                {
                    property.Size = 4;
                }
                if (property.Name == "MD_PlaceCode")
                {
                    property.Size = 7;
                }
                if (property.Name == "MD_PlaceName")
                {
                    property.Size = 60;
                }
                if (property.Name == "MD_Plus4")
                {
                    property.Size = 4;
                }
                if (property.Name == "MD_PostalCode")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_PrivateMailBox")
                {
                    property.Size = 15;
                }

                if (property.Name == "MD_Results")
                {
                    property.Size = 100;
                }
                if (property.Name == "MD_Salutation")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_State")
                {
                    property.Size = 15;
                }
                if (property.Name == "MD_StateName")
                {
                    property.Size = 25;
                }
                if (property.Name == "MD_Suite")
                {
                    property.Size = 20;
                }
                if (property.Name == "MD_TopLevelDomain")
                {
                    property.Size = 10;
                }
                if (property.Name == "MD_UrbanizationName")
                {
                    property.Size = 15;
                }
                if (property.Name == "MD_UTC")
                {
                    property.Size = 6;
                }
            }

            List<PropertyInfo> requestProperties = typeof(SOAP.RequestRecord).GetProperties().ToList();
            foreach (PropertyInfo property in requestProperties)
            {
                PropertyDefinitionsPersonator.Add(new PropertyDefinition
                {
                    Name = "Input_" + property.Name,
                    FullName = "Input_" + property.Name,
                    Description = "Please map your input data for " + property.Name,
                    IsPrimaryKey = false,
                    Nullable = false,
                    PresentationType = "string",
                    PropertyType = typeof(string).Name,
                    UsedInActionInput = true,
                    UsedInActionOutput = false,
                    UsedInQuerySelect = true,
                    Size = 4096
                });
            }

            foreach (IPropertyDefinition property in PropertyDefinitionsPersonator)
            {
                if (property.Name == "Input_FreeForm")
                {
                    property.Description = "When using FreeForm please make sure no other input field is mapped. ";
                }
            }
            return new List<IObjectDefinition>
{
new ObjectDefinition
{
FullName = "BulkPersonator",
Description = "Supports Concurrent Processing for Personator",
Hidden = false,
Name = "BulkPersonator",
SupportedActionFullNames = new List<string>{"BulkExecute","BulkQuery"},
PropertyDefinitions = PropertyDefinitionsPersonator
},
new ObjectDefinition
{
FullName = "BulkGlobal",
Description = "Supports Concurrent Processing for Global Address Verification",
Hidden = false,
Name = "BulkGlobal",
SupportedActionFullNames = new List<string>{"BulkExecute","BulkQuery"},
PropertyDefinitions = PropertyDefinitionsGlobal
},
new ObjectDefinition
{
FullName = "Personator",
Description = "All-in-one contact checking, verification, move update, and appending",
Hidden = true,
Name = "Personator",
SupportedActionFullNames = new List<string>{"Execute"},
PropertyDefinitions = PropertyDefinitionsPersonator
},
new ObjectDefinition
{
FullName = "Global_Address_Verification",
Description = "Verify and standardize mailing addresses from across the world with the flexibility and accuracy",
Hidden = true,
Name = "Global_Address_Verification",
SupportedActionFullNames = new List<string>{"Execute"},
PropertyDefinitions = PropertyDefinitionsGlobal
},
};
        }

        //-----------------------------------------------------------------------------------------------------
        public void ResetMetadata()
        {
        }

        //-----------------------------------------------------------------------------------------------------
        public IEnumerable<IActionDefinition> RetrieveActionDefinitions()
        {
            return this.ActionDefinitions;
        }

        //-----------------------------------------------------------------------------------------------------
        public IMethodDefinition RetrieveMethodDefinition(string objectName, bool shouldGetParameters = false)
        {
            throw new NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------
        public IEnumerable<IMethodDefinition> RetrieveMethodDefinitions(bool shouldGetParameters = false)
        {
            throw new NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------
        public IObjectDefinition RetrieveObjectDefinition(string objectName, bool shouldGetProperties = false, bool shouldGetRelations = false)
        {
            return this.ObjectDefinitions.First(od => od.Name == objectName);
        }

        //-----------------------------------------------------------------------------------------------------
        public IEnumerable<IObjectDefinition> RetrieveObjectDefinitions(bool shouldGetProperties = false, bool shouldGetRelations = false)
        {
            return this.ObjectDefinitions;
        }

        //-----------------------------------------------------------------------------------------------------
        public void Dispose()
        {
        }

        public List<IPropertyDefinition> PropertyDefinitionsPersonator { get; set; }

        public List<IPropertyDefinition> PropertyDefinitionsGlobal { get; set; }

        public List<IPropertyDefinition> PropertyDefinitionsPersonatorBulk { get; set; }
    }
}