import { OtherPropertyModel, VehicleViewModel, AircraftViewModel, VesselViewModel, AgriculturalMachineryViewModel } from './property';
import { RegixPersonModel } from './person';
import { RegixCompanyModel } from './company';

export class DistraintModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.propertyId = obj.propertyId || 0;
        this.propertyRegNumber = obj.propertyRegNumber;
        this.propertyTypeCode = obj.propertyTypeCode;
        this.propertyTypeName = obj.propertyTypeName;
        this.propertyTypeNameEn = obj.propertyTypeNameEn;
        this.statusCode = obj.statusCode;
        this.statusName = obj.statusName;
        this.statusNameEn = obj.statusNameEn;
        this.enforcementDate = obj.enforcementDate ? new Date(obj.enforcementDate) : undefined;
        this.inFavourOf = obj.inFavourOf;
        this.suitNumber = obj.suitNumber;
        this.debtor = obj.debtor;
        this.deactivated = obj.deactivated || false;
        this.createdOn = new Date(obj.createdOn);
        this.createdBy = obj.createdBy;
        this.createdByUserType = obj.createdByUserType;
        this.createdByUserName = obj.createdByUserName;
        this.location = obj.location;
        this.inFavourOfPersonId = obj.inFavourOfPersonId || undefined;
        this.inFavourOfPerson = obj.inFavourOfPerson || new RegixPersonModel();
        this.debtorPersonId = obj.debtorPersonId || undefined;
        this.debtorPerson = obj.debtorPerson || new RegixPersonModel();
        this.inFavourOfCompanyId = obj.inFavourOfCompanyId || undefined;
        this.inFavourOfCompany = obj.inFavourOfCompany || new RegixCompanyModel();
        this.debtorCompanyId = obj.debtorCompanyId || undefined;
        this.debtorCompany = obj.debtorCompany || new RegixCompanyModel();
        this.isInFavourOfPerson = obj.isInFavourOfPerson || false;
        this.isDebtorPerson = obj.isDebtorPerson || false;
    }

    id: number;
    propertyId: number;
    propertyRegNumber: string;
    propertyTypeCode: string;
    propertyTypeName: string;
    propertyTypeNameEn: string;
    statusCode: string;
    statusName: string;
    statusNameEn: string;
    enforcementDate?: Date;
    inFavourOf: string;
    suitNumber: string;
    debtor: string;
    deactivated: boolean;
    createdOn: Date;    
    createdBy: string;
    createdByUserType: string;
    createdByUserName: string;
    location: string;
    
    inFavourOfPersonId?: number;
    inFavourOfPerson: RegixPersonModel;
    debtorPersonId?: number;
    debtorPerson: RegixPersonModel;

    inFavourOfCompanyId?: number;
    inFavourOfCompany: RegixCompanyModel;
    debtorCompanyId?: number;
    debtorCompany: RegixCompanyModel;
    isInFavourOfPerson: boolean;
    isDebtorPerson: boolean;
}

export class DistraintCreateModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.propertyTypeCode = obj.propertyTypeCode;
        this.isNewProperty = obj.isNewProperty || false;
        this.propertyIdVehicle = obj.propertyIdVehicle || undefined;
        this.propertyIdAircraft = obj.propertyIdAircraft || undefined;
        this.propertyIdVessel = obj.propertyIdVessel || undefined;
        this.propertyIdAgriForMachinery = obj.propertyIdAgriForMachinery || undefined;
        this.propertyIdRealEstate = obj.propertyIdRealEstate || undefined;
        this.propertyIdOtherProperty = obj.propertyIdOtherProperty || undefined;

        this.newOtherProperty = obj.newOtherProperty || new OtherPropertyModel();
        this.vehicleProperty = obj.vehicleProperty || new OtherPropertyModel();
        this.aircraftProperty = obj.aircraftProperty || new AircraftViewModel();
        this.vesselProperty = obj.vesselProperty || new VesselViewModel();
        this.agriculturalMachineProperty = obj.agriculturalMachineProperty || new AgriculturalMachineryViewModel();
        this.searchedIdentifier = obj.searchedIdentifier;

        this.statusCode = obj.statusCode;
        this.inFavourOf = obj.inFavourOf;
        this.suitNumber = obj.suitNumber;
        this.debtor = obj.debtor;
        this.createdBy = obj.createdBy;
        this.location = obj.location;
        this.enforcementDate = obj.enforcementDate ? new Date(obj.enforcementDate) : undefined;
        this.enforcedBy = obj.enforcedBy;
        this.enforcedAt = obj.enforcedAt ? new Date(obj.enforcedAt) : undefined;

        this.inFavourOfPersonId = obj.inFavourOfPersonId || undefined;
        this.inFavourOfPerson = obj.inFavourOfPerson || new RegixPersonModel();
        this.debtorPersonId = obj.debtorPersonId || undefined;
        this.debtorPerson = obj.debtorPerson || new RegixPersonModel();

        this.inFavourOfCompanyId = obj.inFavourOfCompanyId || undefined;
        this.inFavourOfCompany = obj.inFavourOfCompany || new RegixCompanyModel();
        this.debtorCompanyId = obj.debtorCompanyId || undefined;
        this.debtorCompany = obj.debtorCompany || new RegixCompanyModel();
        this.isInFavourOfPerson = obj.isInFavourOfPerson || false;
        this.isDebtorPerson = obj.isDebtorPerson || false;
    }

    id: number;
    propertyTypeCode: string;
    isNewProperty: boolean;
    propertyIdVehicle?: number;
    propertyIdAircraft?: number;
    propertyIdVessel?: number;
    propertyIdAgriForMachinery?: number;
    propertyIdRealEstate?: number;
    propertyIdOtherProperty?: number;

    newOtherProperty: OtherPropertyModel;
    vehicleProperty: VehicleViewModel;
    aircraftProperty: AircraftViewModel;
    vesselProperty: VesselViewModel;
    agriculturalMachineProperty: AgriculturalMachineryViewModel;
    searchedIdentifier: string;


    statusCode: string;
    inFavourOf: string;
    suitNumber: string;
    debtor: string;
    createdBy: string;
    location: string;
    enforcementDate?: Date;
    enforcedBy: string;
    enforcedAt?: Date;

    inFavourOfPersonId?: number;
    inFavourOfPerson: RegixPersonModel;
    debtorPersonId?: number;
    debtorPerson: RegixPersonModel;

    inFavourOfCompanyId?: number;
    inFavourOfCompany: RegixCompanyModel;
    debtorCompanyId?: number;
    debtorCompany: RegixCompanyModel;
    isInFavourOfPerson: boolean;
    isDebtorPerson: boolean;
}


export class DistraintViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.propertyTypeCode = obj.propertyTypeCode;
        this.propertyIdVehicle = obj.propertyIdVehicle || undefined;
        this.propertyIdAircraft = obj.propertyIdAircraft || undefined;
        this.propertyIdVessel = obj.propertyIdVessel || undefined;
        this.propertyIdAgriForMachinery = obj.propertyIdAgriForMachinery || undefined;
        this.propertyIdRealEstate = obj.propertyIdRealEstate || undefined;
        this.propertyIdOtherProperty = obj.propertyIdOtherProperty || undefined;

        this.otherProperty = obj.otherProperty || new OtherPropertyModel();
        this.vehicleProperty = obj.vehicleProperty || new VehicleViewModel();
        this.aircraftProperty = obj.aircraftProperty || new AircraftViewModel();
        this.vesselProperty = obj.vesselProperty || new VesselViewModel();
        this.agriculturalMachineProperty = obj.agriculturalMachineProperty || new VesselViewModel();

        this.statusCode = obj.statusCode;
        this.inFavourOf = obj.inFavourOf;
        this.inFavourOfIdentifier = obj.inFavourOfIdentifier;
        this.suitNumber = obj.suitNumber;
        this.debtor = obj.debtor;
        this.debtorIdentifier = obj.debtorIdentifier;
        this.createdBy = obj.createdBy;
        this.createdByUserType = obj.createdByUserType;
        this.createdByUserName = obj.createdByUserName;
        this.location = obj.location;
        this.enforcementDate = obj.enforcementDate ? new Date(obj.enforcementDate) : undefined;
        this.enforcedBy = obj.enforcedBy;
        this.enforcedAt = obj.enforcedAt ? new Date(obj.enforcedAt) : undefined;
        this.revocationDate = obj.revocationDate ? new Date(obj.revocationDate) : undefined;
        this.revokedBy = obj.revokedBy;
        this.revokedAt = obj.revokedAt ? new Date(obj.revokedAt) : undefined;
        this.exemptionDate = obj.exemptionDate ? new Date(obj.exemptionDate) : undefined;
        this.exemptedBy = obj.exemptedBy;
        this.exemptedAt = obj.exemptedAt ? new Date(obj.exemptedAt) : undefined;

        this.inFavourOfPersonId = obj.inFavourOfPersonId || undefined;
        this.inFavourOfPerson = obj.inFavourOfPerson || new RegixPersonModel();
        this.debtorPersonId = obj.debtorPersonId || undefined;
        this.debtorPerson = obj.debtorPerson || new RegixPersonModel();

        this.inFavourOfCompanyId = obj.inFavourOfCompanyId || undefined;
        this.inFavourOfCompany = obj.inFavourOfCompany || new RegixCompanyModel();
        this.debtorCompanyId = obj.debtorCompanyId || undefined;
        this.debtorCompany = obj.debtorCompany || new RegixCompanyModel();
        this.isInFavourOfPerson = obj.isInFavourOfPerson || false;
        this.isDebtorPerson = obj.isDebtorPerson || false;
    }

    id: number;
    propertyTypeCode: string;
    propertyIdVehicle?: number;
    propertyIdAircraft?: number;
    propertyIdVessel?: number;
    propertyIdAgriForMachinery?: number;
    propertyIdRealEstate?: number;
    propertyIdOtherProperty?: number;

    otherProperty: OtherPropertyModel;
    vehicleProperty: VehicleViewModel;
    aircraftProperty: AircraftViewModel;
    vesselProperty: VesselViewModel;
    agriculturalMachineProperty: AgriculturalMachineryViewModel;

    statusCode: string;
    inFavourOf: string;
    inFavourOfIdentifier: string;
    suitNumber: string;
    debtor: string;
    debtorIdentifier: string;
    createdBy: string;
    createdByUserType: string;
    createdByUserName: string;
    location: string;
    enforcementDate?: Date;
    enforcedBy: string;
    enforcedAt?: Date;
    revocationDate?: Date;
    revokedBy: string;
    revokedAt?: Date;
    exemptionDate?: Date;
    exemptedBy: string;
    exemptedAt?: Date;

    inFavourOfPersonId?: number;
    inFavourOfPerson: RegixPersonModel;
    debtorPersonId?: number;
    debtorPerson: RegixPersonModel;

    inFavourOfCompanyId?: number;
    inFavourOfCompany: RegixCompanyModel;
    debtorCompanyId?: number;
    debtorCompany: RegixCompanyModel;
    isInFavourOfPerson: boolean;
    isDebtorPerson: boolean;
}

export class DistraintEnforceModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.enforcementDate = obj.enforcementDate ? new Date(obj.enforcementDate) : undefined;
        this.enforcedBy = obj.enforcedBy;
        this.enforcedAt = obj.enforcedAt ? new Date(obj.enforcedAt) : undefined;
    }

    id: number;
    enforcementDate?: Date;
    enforcedBy: string;
    enforcedAt?: Date;

}

export class DistraintRevokeModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.revocationDate = obj.revocationDate ? new Date(obj.revocationDate) : undefined;
        this.revokedBy = obj.revokedBy;
        this.revokedAt = obj.revokedAt ? new Date(obj.revokedAt) : undefined;
    }

    id: number;
    revocationDate?: Date;
    revokedBy: string;
    revokedAt?: Date;
}

export class DistraintExemptModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.exemptionDate = obj.exemptionDate ? new Date(obj.exemptionDate) : undefined;
        this.exemptedBy = obj.exemptedBy;
        this.exemptedAt = obj.exemptedAt ? new Date(obj.exemptedAt) : undefined;
    }

    id: number;
    exemptionDate?: Date;
    exemptedBy: string;
    exemptedAt?: Date;
}