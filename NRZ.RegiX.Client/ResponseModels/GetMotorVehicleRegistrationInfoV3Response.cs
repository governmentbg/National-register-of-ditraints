﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.RegiX.Client.ResponseModels
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:4.0.30319.42000
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    using System.Xml.Serialization;

    // 
    // This source code was auto-generated by xsd, Version=4.8.3928.0.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    [System.Xml.Serialization.XmlRootAttribute("GetMotorVehicleRegistrationInfoV3Response", Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response", IsNullable = false)]
    public partial class GetMotorVehicleRegistrationInfoV3Response : BaseResponse
    {

        private HeaderResponseTypeV3 headerField;

        private GetMotorVehicleRegistrationInfoV3ResponseTypeResponse responseField;

        /// <remarks/>
        public HeaderResponseTypeV3 Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public GetMotorVehicleRegistrationInfoV3ResponseTypeResponse Response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class HeaderResponseTypeV3
    {

        private string messageIDField;

        private string messageRefIDField;

        private System.DateTime dateTimeField;

        private string operationField;

        private object userNameField;

        private object organizationUnitField;

        private object callerIPAddressField;

        private object callContextField;

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return this.messageIDField;
            }
            set
            {
                this.messageIDField = value;
            }
        }

        /// <remarks/>
        public string MessageRefID
        {
            get
            {
                return this.messageRefIDField;
            }
            set
            {
                this.messageRefIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime DateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public string Operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }

        /// <remarks/>
        public object UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }

        /// <remarks/>
        public object OrganizationUnit
        {
            get
            {
                return this.organizationUnitField;
            }
            set
            {
                this.organizationUnitField = value;
            }
        }

        /// <remarks/>
        public object CallerIPAddress
        {
            get
            {
                return this.callerIPAddressField;
            }
            set
            {
                this.callerIPAddressField = value;
            }
        }

        /// <remarks/>
        public object CallContext
        {
            get
            {
                return this.callContextField;
            }
            set
            {
                this.callContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class VehicleDataTypeV3
    {

        private string registrationNumberField;

        private System.DateTime firstRegistrationDateField;

        private bool firstRegistrationDateFieldSpecified;

        private string vINField;

        private string engineNumberField;

        private string vehicleTypeField;

        private string modelField;

        private string typeApprovalNumberField;

        private string approvalTypeField;

        private string tradeDescriptionField;

        private string colorField;

        private string categoryField;

        private string offRoadSymbolsField;

        private string massGField;

        private string massF1Field;

        private string massF2Field;

        private string massF3Field;

        private string vehNumOfAxlesField;

        private string vehMassO1Field;

        private string vehMassO2Field;

        private string capacityField;

        private string maxPowerField;

        private string fuelField;

        private string environmentalCategoryField;

        private VehicleDocDataTypeV3 vehicleDocumentField;

        /// <remarks/>
        public string RegistrationNumber
        {
            get
            {
                return this.registrationNumberField;
            }
            set
            {
                this.registrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime FirstRegistrationDate
        {
            get
            {
                return this.firstRegistrationDateField;
            }
            set
            {
                this.firstRegistrationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstRegistrationDateSpecified
        {
            get
            {
                return this.firstRegistrationDateFieldSpecified;
            }
            set
            {
                this.firstRegistrationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string VIN
        {
            get
            {
                return this.vINField;
            }
            set
            {
                this.vINField = value;
            }
        }

        /// <remarks/>
        public string EngineNumber
        {
            get
            {
                return this.engineNumberField;
            }
            set
            {
                this.engineNumberField = value;
            }
        }

        /// <remarks/>
        public string VehicleType
        {
            get
            {
                return this.vehicleTypeField;
            }
            set
            {
                this.vehicleTypeField = value;
            }
        }

        /// <remarks/>
        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        public string TypeApprovalNumber
        {
            get
            {
                return this.typeApprovalNumberField;
            }
            set
            {
                this.typeApprovalNumberField = value;
            }
        }

        /// <remarks/>
        public string ApprovalType
        {
            get
            {
                return this.approvalTypeField;
            }
            set
            {
                this.approvalTypeField = value;
            }
        }

        /// <remarks/>
        public string TradeDescription
        {
            get
            {
                return this.tradeDescriptionField;
            }
            set
            {
                this.tradeDescriptionField = value;
            }
        }

        /// <remarks/>
        public string Color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        public string Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        public string OffRoadSymbols
        {
            get
            {
                return this.offRoadSymbolsField;
            }
            set
            {
                this.offRoadSymbolsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MassG
        {
            get
            {
                return this.massGField;
            }
            set
            {
                this.massGField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MassF1
        {
            get
            {
                return this.massF1Field;
            }
            set
            {
                this.massF1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MassF2
        {
            get
            {
                return this.massF2Field;
            }
            set
            {
                this.massF2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string MassF3
        {
            get
            {
                return this.massF3Field;
            }
            set
            {
                this.massF3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string VehNumOfAxles
        {
            get
            {
                return this.vehNumOfAxlesField;
            }
            set
            {
                this.vehNumOfAxlesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string VehMassO1
        {
            get
            {
                return this.vehMassO1Field;
            }
            set
            {
                this.vehMassO1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string VehMassO2
        {
            get
            {
                return this.vehMassO2Field;
            }
            set
            {
                this.vehMassO2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string Capacity
        {
            get
            {
                return this.capacityField;
            }
            set
            {
                this.capacityField = value;
            }
        }

        /// <remarks/>
        public string MaxPower
        {
            get
            {
                return this.maxPowerField;
            }
            set
            {
                this.maxPowerField = value;
            }
        }

        /// <remarks/>
        public string Fuel
        {
            get
            {
                return this.fuelField;
            }
            set
            {
                this.fuelField = value;
            }
        }

        /// <remarks/>
        public string EnvironmentalCategory
        {
            get
            {
                return this.environmentalCategoryField;
            }
            set
            {
                this.environmentalCategoryField = value;
            }
        }

        /// <remarks/>
        public VehicleDocDataTypeV3 VehicleDocument
        {
            get
            {
                return this.vehicleDocumentField;
            }
            set
            {
                this.vehicleDocumentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class VehicleDocDataTypeV3
    {

        private string vehDocumentNumberField;

        private System.DateTime vehDocumentDateField;

        private bool vehDocumentDateFieldSpecified;

        /// <remarks/>
        public string VehDocumentNumber
        {
            get
            {
                return this.vehDocumentNumberField;
            }
            set
            {
                this.vehDocumentNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime VehDocumentDate
        {
            get
            {
                return this.vehDocumentDateField;
            }
            set
            {
                this.vehDocumentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VehDocumentDateSpecified
        {
            get
            {
                return this.vehDocumentDateFieldSpecified;
            }
            set
            {
                this.vehDocumentDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class UsersDataTypeV3
    {

        private OwnerTypeV3 userField;

        /// <remarks/>
        public OwnerTypeV3 User
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class OwnerTypeV3
    {

        private BGTypeV3 bulgarianCitizenField;

        private FCTypeV3 foreignCitizenField;

        private CompanyTypeV3 companyField;

        /// <remarks/>
        public BGTypeV3 BulgarianCitizen
        {
            get
            {
                return this.bulgarianCitizenField;
            }
            set
            {
                this.bulgarianCitizenField = value;
            }
        }

        /// <remarks/>
        public FCTypeV3 ForeignCitizen
        {
            get
            {
                return this.foreignCitizenField;
            }
            set
            {
                this.foreignCitizenField = value;
            }
        }

        /// <remarks/>
        public CompanyTypeV3 Company
        {
            get
            {
                return this.companyField;
            }
            set
            {
                this.companyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class BGTypeV3
    {

        private string pINField;

        private BGTypeV3Names namesField;

        /// <remarks/>
        public string PIN
        {
            get
            {
                return this.pINField;
            }
            set
            {
                this.pINField = value;
            }
        }

        /// <remarks/>
        public BGTypeV3Names Names
        {
            get
            {
                return this.namesField;
            }
            set
            {
                this.namesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class BGTypeV3Names
    {

        private string firstField;

        private string surnameField;

        private string familyField;

        /// <remarks/>
        public string First
        {
            get
            {
                return this.firstField;
            }
            set
            {
                this.firstField = value;
            }
        }

        /// <remarks/>
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        public string Family
        {
            get
            {
                return this.familyField;
            }
            set
            {
                this.familyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class FCTypeV3
    {

        private string pINField;

        private string pnField;

        private string namesCyrillicField;

        private string namesLatinField;

        private string nationalityField;

        /// <remarks/>
        public string PIN
        {
            get
            {
                return this.pINField;
            }
            set
            {
                this.pINField = value;
            }
        }

        /// <remarks/>
        public string PN
        {
            get
            {
                return this.pnField;
            }
            set
            {
                this.pnField = value;
            }
        }

        /// <remarks/>
        public string NamesCyrillic
        {
            get
            {
                return this.namesCyrillicField;
            }
            set
            {
                this.namesCyrillicField = value;
            }
        }

        /// <remarks/>
        public string NamesLatin
        {
            get
            {
                return this.namesLatinField;
            }
            set
            {
                this.namesLatinField = value;
            }
        }

        /// <remarks/>
        public string Nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class CompanyTypeV3
    {

        private string idField;

        private string nameField;

        private string nameLatinField;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string NameLatin
        {
            get
            {
                return this.nameLatinField;
            }
            set
            {
                this.nameLatinField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class OwnersDataTypeV3
    {

        private OwnerTypeV3[] ownerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Owner")]
        public OwnerTypeV3[] Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class ReturnInformationTypeV3
    {

        private string returnCodeField;

        private string infoField;

        /// <remarks/>
        public string ReturnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }

        /// <remarks/>
        public string Info
        {
            get
            {
                return this.infoField;
            }
            set
            {
                this.infoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class GetMotorVehicleRegistrationInfoV3ResponseTypeResponse : BaseResponse
    {

        private GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult[] resultsField;

        private ReturnInformationTypeV3 returnInformationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Result", IsNullable = false)]
        public GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult[] Results
        {
            get
            {
                return this.resultsField;
            }
            set
            {
                this.resultsField = value;
            }
        }

        /// <remarks/>
        public ReturnInformationTypeV3 ReturnInformation
        {
            get
            {
                return this.returnInformationField;
            }
            set
            {
                this.returnInformationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://egov.bg/RegiX/MVR/MPS/GetMotorVehicleRegistrationInfoV3Response")]
    public partial class GetMotorVehicleRegistrationInfoV3ResponseTypeResponseResult
    {

        private VehicleDataTypeV3 vehicleDataField;

        private OwnersDataTypeV3 ownersDataField;

        private UsersDataTypeV3[] usersDataField;

        /// <remarks/>
        public VehicleDataTypeV3 VehicleData
        {
            get
            {
                return this.vehicleDataField;
            }
            set
            {
                this.vehicleDataField = value;
            }
        }

        /// <remarks/>
        public OwnersDataTypeV3 OwnersData
        {
            get
            {
                return this.ownersDataField;
            }
            set
            {
                this.ownersDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("UsersData")]
        public UsersDataTypeV3[] UsersData
        {
            get
            {
                return this.usersDataField;
            }
            set
            {
                this.usersDataField = value;
            }
        }
    }

}
