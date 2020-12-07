import { IPropertySearchRequestModel } from '@/interfaces/property';
import Address from "@/models/address";

export class PropertySearchRequestModel implements IPropertySearchRequestModel {
    constructor(obj: any = {}) {
        this.identifierTypeCode = obj.identifierTypeCode;
        this.identifier = obj.identifier;
        this.suitNumber = obj.suitNumber;

    }

    identifierTypeCode: string;
    identifier: string;
    suitNumber: string;
}

export class OtherPropertyModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.identifier = obj.identifier;
        this.description = obj.description;
        this.type = obj.type;
        this.isManuallyAdded = obj.isManuallyAdded;
    }

    id: number;
    identifier: string;
    description: string;
    type: string;
    isManuallyAdded: boolean;
}



export class VehicleOwnerViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.vehicleId = obj.vehicleId;
        this.bulgarianCitizenPin = obj.bulgarianCitizenPin;
        this.bulgarianCitizenFirstName = obj.bulgarianCitizenFirstName;
        this.bulgarianCitizenMiddleName = obj.bulgarianCitizenMiddleName;
        this.bulgarianCitizenLastName = obj.bulgarianCitizenLastName;
        this.foreignCitizenPin = obj.foreignCitizenPin;
        this.foreignCitizenPn = obj.foreignCitizenPn;
        this.foreignCitizenNamesCyrillic = obj.foreignCitizenNamesCyrillic;
        this.foreignCitizenNamesLatin = obj.foreignCitizenNamesLatin;
        this.foreignCitizenNationality = obj.foreignCitizenNationality;
        this.companyId = obj.companyId;
        this.companyName = obj.companyName;
        this.companyNameLatin = obj.companyNameLatin;
    }

    id: number;
    vehicleId: number;
    bulgarianCitizenPin: string;
    bulgarianCitizenFirstName: string;
    bulgarianCitizenMiddleName: string;
    bulgarianCitizenLastName: string;
    foreignCitizenPin: string;
    foreignCitizenPn: string;
    foreignCitizenNamesCyrillic: string;
    foreignCitizenNamesLatin: string;
    foreignCitizenNationality: string;
    companyId: string;
    companyName: string;
    companyNameLatin: string;
}


export class VehicleUserViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.vehicleId = obj.vehicleId;
        this.bulgarianCitizenPin = obj.bulgarianCitizenPin;
        this.bulgarianCitizenFirstName = obj.bulgarianCitizenFirstName;
        this.bulgarianCitizenMiddleName = obj.bulgarianCitizenMiddleName;
        this.bulgarianCitizenLastName = obj.bulgarianCitizenLastName;
        this.foreignCitizenPin = obj.foreignCitizenPin;
        this.foreignCitizenPn = obj.foreignCitizenPn;
        this.foreignCitizenNamesCyrillic = obj.foreignCitizenNamesCyrillic;
        this.foreignCitizenNamesLatin = obj.foreignCitizenNamesLatin;
        this.foreignCitizenNationality = obj.foreignCitizenNationality;
        this.companyId = obj.companyId;
        this.companyName = obj.companyName;
        this.companyNameLatin = obj.companyNameLatin;
    }

    id: number;
    vehicleId: number;
    bulgarianCitizenPin: string;
    bulgarianCitizenFirstName: string;
    bulgarianCitizenMiddleName: string;
    bulgarianCitizenLastName: string;
    foreignCitizenPin: string;
    foreignCitizenPn: string;
    foreignCitizenNamesCyrillic: string;
    foreignCitizenNamesLatin: string;
    foreignCitizenNationality: string;
    companyId: string;
    companyName: string;
    companyNameLatin: string;
}

export class VehicleViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationNumber = obj.registrationNumber;
        this.firstRegistrationDate = obj.firstRegistrationDate ? new Date(obj.firstRegistrationDate) : undefined;
        this.vin = obj.vin;
        this.engineNumber = obj.engineNumber;
        this.vehicleType = obj.vehicleType;
        this.model = obj.model;
        this.typeApprovalNumber = obj.typeApprovalNumber;
        this.approvalType = obj.approvalType;
        this.tradeDescription = obj.tradeDescription;
        this.color = obj.color;
        this.category = obj.category;
        this.offRoadSymbols = obj.offRoadSymbols;
        this.massG = obj.massG;
        this.massF1 = obj.massF1;
        this.massF2 = obj.massF2;
        this.massF3 = obj.massF3;
        this.vehicleNumOfAxles = obj.vehicleNumOfAxles;
        this.vehicleMassO1 = obj.vehicleMassO1;
        this.vehicleMassO2 = obj.vehicleMassO2;
        this.capacity = obj.capacity;
        this.maxPower = obj.maxPower;
        this.fuel = obj.fuel;
        this.environmentalCategory = obj.environmentalCategory;
        this.vehicleDocumentNumber = obj.vehicleDocumentNumber;
        this.vehicleDocumentDate = obj.vehicleDocumentDate ? new Date(obj.vehicleDocumentDate) : undefined;
        this.extensionRequestId = obj.extensionRequestId || undefined;

        this.owners = [];
        if (obj.owners) {
            obj.owners.forEach((own: any) => {
                this.owners.push(new VehicleOwnerViewModel(own));
            });
        }

        this.users = [];
        if (obj.users) {
            obj.users.forEach((user: any) => {
                this.users.push(new VehicleUserViewModel(user));
            });
        }

    }

    id: number;
    registrationNumber: string;
    firstRegistrationDate?: Date;
    vin: string;
    engineNumber: string;
    vehicleType: string;
    model: string;
    typeApprovalNumber: string;
    approvalType: string;
    tradeDescription: string;
    color: string;
    category: string;
    offRoadSymbols: string;
    massG: string;
    massF1: string;
    massF2: string;
    massF3: string;
    vehicleNumOfAxles: string;
    vehicleMassO1: string;
    vehicleMassO2: string;
    capacity: string;
    maxPower: string;
    fuel: string;
    environmentalCategory: string;
    vehicleDocumentNumber: string;
    vehicleDocumentDate?: Date;
    extensionRequestId?: number;

    owners: VehicleOwnerViewModel[];
    users: VehicleUserViewModel[];
}



export class AircraftDebtViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.aircraftId = obj.aircraftId || 0;
        this.inputDate = obj.inputDate ? new Date(obj.inputDate) : undefined;
        this.debtTypeCode = obj.debtTypeCode;
        this.debtType = obj.debtType;
        this.isActive = obj.isActive || null;
        this.applicantIdentifier = obj.applicantIdentifier;
        this.applicantName = obj.applicantName;
        this.documentIncomingNumber = obj.documentIncomingNumber;
        this.documentIncomingDate = obj.documentIncomingDate ? new Date(obj.documentIncomingDate) : undefined;
        this.documentExternalNumber = obj.documentExternalNumber;
        this.documentExternalDate = obj.documentExternalDate ? new Date(obj.documentExternalDate) : undefined;
        this.repaymentDate = obj.repaymentDate ? new Date(obj.repaymentDate) : undefined;
        this.repaymentDocumentIncomingNumber = obj.repaymentDocumentIncomingNumber;
        this.repaymentDocumentIncomingDate = obj.repaymentDocumentIncomingDate ? new Date(obj.repaymentDocumentIncomingDate) : undefined;
        this.repaymentDocumentExternalNumber = obj.repaymentDocumentExternalNumber;
        this.repaymentDocumentExternalDate = obj.repaymentDocumentExternalDate ? new Date(obj.repaymentDocumentExternalDate) : undefined;
        this.repaymentNotes = obj.repaymentNotes;
        this.notes = obj.notes;
    }

    id: number;
    aircraftId: number;
    inputDate?: Date;
    debtTypeCode: number;
    debtType: string;
    isActive?: boolean;
    applicantIdentifier: string;
    applicantName: string;
    documentIncomingNumber: string;
    documentIncomingDate?: Date;
    documentExternalNumber: string;
    documentExternalDate?: Date;
    repaymentDate?: Date;
    repaymentDocumentIncomingNumber: string;
    repaymentDocumentIncomingDate?: Date;
    repaymentDocumentExternalNumber: string;
    repaymentDocumentExternalDate?: Date;
    repaymentNotes: string;
    notes: string;

}

export class AircraftRegistrationOperatorEntityViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationId = obj.registrationId || 0;
        this.identifier = obj.identifier;
        this.name = obj.name;
    }

    id: number;
    registrationId: number;
    identifier: string;
    name: string;
}

export class AircraftRegistrationOperatorPersonViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationId = obj.registrationId || 0;
        this.identifier = obj.identifier;
        this.names = obj.names;
    }

    id: number;
    registrationId: number;
    identifier: string;
    names: string;
}

export class AircraftRegistrationOwnerEntityViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationId = obj.registrationId || 0;
        this.identifier = obj.identifier;
        this.name = obj.name;
    }

    id: number;
    registrationId: number;
    identifier: string;
    name: string;
}

export class AircraftRegistrationOwnerPersonViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationId = obj.registrationId || 0;
        this.identifier = obj.identifier;
        this.names = obj.names;
    }

    id: number;
    registrationId: number;
    identifier: string;
    names: string;
}

export class AircraftRegistrationViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.aircraftId = obj.aircraftId || 0;
        this.actNumber = obj.actNumber || 0;
        this.registrationNumber = obj.registrationNumber || 0;
        this.registrationDate = obj.registrationDate ? new Date(obj.registrationDate) : undefined;
        this.registrationMark = obj.registrationMark;
        this.isLastRegistration = obj.isLastRegistration || null;
        this.registrationStatusIsActive = obj.registrationStatusIsActive || null;
        this.registrationStatusCode = obj.registrationStatusCode || 0;
        this.registrationStatusName = obj.registrationStatusName;
        this.registrationDocumentNumber = obj.registrationDocumentNumber;
        this.registrationDocumentDate = obj.registrationDocumentDate ? new Date(obj.registrationDocumentDate) : undefined;
        this.registrationDocumentDescription = obj.registrationDocumentDescription;
        this.deregistrationDate = obj.deregistrationDate ? new Date(obj.deregistrationDate) : undefined;
        this.deregistrationReason = obj.deregistrationReason;
        this.deregistrationDescription = obj.deregistrationDescription;
        this.deregistrationCountryCode = obj.deregistrationCountryCode;
        this.deregistrationCountryName = obj.deregistrationCountryName;
        this.leasingDocumentNumber = obj.leasingDocumentNumber;
        this.leasingDocumentDate = obj.leasingDocumentDate ? new Date(obj.leasingDocumentDate) : undefined;
        this.leasingEndDate = obj.leasingEndDate ? new Date(obj.leasingEndDate) : undefined;
        this.leasingAgreement = obj.leasingAgreement;
        this.leasingLessorPersonIdentifier = obj.leasingLessorPersonIdentifier;
        this.leasingLessorPersonNames = obj.leasingLessorPersonNames;
        this.leasingLessorEntityIdentifier = obj.leasingLessorEntityIdentifier;
        this.leasingLessorEntityName = obj.leasingLessorEntityName;

        this.operatorEntities = [];
        if (obj.operatorEntities) {
            obj.operatorEntities.forEach((op: any) => {
                this.operatorEntities.push(new AircraftRegistrationOperatorEntityViewModel(op));
            });
        }

        this.operatorPeople = [];
        if (obj.operatorPeople) {
            obj.operatorPeople.forEach((op: any) => {
                this.operatorPeople.push(new AircraftRegistrationOperatorPersonViewModel(op));
            });
        }

        this.ownerEntities = [];
        if (obj.ownerEntities) {
            obj.ownerEntities.forEach((owner: any) => {
                this.ownerEntities.push(new AircraftRegistrationOwnerEntityViewModel(owner));
            });
        }

        this.ownerPeople = [];
        if (obj.ownerPeople) {
            obj.ownerPeople.forEach((owner: any) => {
                this.ownerPeople.push(new AircraftRegistrationOwnerPersonViewModel(owner));
            });
        }

    }

    id: number;
    aircraftId: number;
    actNumber?: number;
    registrationNumber?: number;
    registrationDate?: Date;
    registrationMark: string;
    isLastRegistration?: boolean;
    registrationStatusIsActive?: boolean;
    registrationStatusCode?: number;
    registrationStatusName: string;
    registrationDocumentNumber: string;
    registrationDocumentDate?: Date;
    registrationDocumentDescription: string;
    deregistrationDate?: Date;
    deregistrationReason: string;
    deregistrationDescription: string;
    deregistrationCountryCode: string;
    deregistrationCountryName: string;
    leasingDocumentNumber: string;
    leasingDocumentDate?: Date;
    leasingEndDate?: Date;
    leasingAgreement: string;
    leasingLessorPersonIdentifier: string;
    leasingLessorPersonNames: string;
    leasingLessorEntityIdentifier: string;
    leasingLessorEntityName: string;

    operatorEntities: AircraftRegistrationOperatorEntityViewModel[];
    operatorPeople: AircraftRegistrationOperatorPersonViewModel[];
    ownerEntities: AircraftRegistrationOwnerEntityViewModel[];
    ownerPeople: AircraftRegistrationOwnerPersonViewModel[];
}

export class AircraftViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.producerName = obj.producerName;
        this.producerNameEn = obj.producerNameEn;
        this.producerCountryCode = obj.producerCountryCode;
        this.producerCountryName = obj.producerCountryName;
        this.airCategoryCode = obj.airCategoryCode;
        this.airCategoryName = obj.airCategoryName;
        this.icao = obj.icao;
        this.msnserialNumber = obj.msnserialNumber;
        this.modelName = obj.modelName;
        this.modelNameEn = obj.modelNameEn;
        this.extensionRequestId = obj.extensionRequestId || undefined;

        this.debts = [];
        if (obj.debts) {
            obj.debts.forEach((debt: any) => {
                this.debts.push(new AircraftDebtViewModel(debt));
            });
        }

        this.registrations = [];
        if (obj.registrations) {
            obj.registrations.forEach((reg: any) => {
                this.registrations.push(new AircraftRegistrationViewModel(reg));
            });
        }
        
    }

    id: number;
    producerName: string;
    producerNameEn: string;
    producerCountryCode: string;
    producerCountryName: string;
    airCategoryCode: string;
    airCategoryName: string;
    icao: string;
    msnserialNumber: string;
    modelName: string;
    modelNameEn: string;
    extensionRequestId?: number;

    debts: AircraftDebtViewModel[];
    registrations: AircraftRegistrationViewModel[];
}

export class PropertyItemModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.description = obj.description;
        this.identifier = obj.identifier;    
        this.propertyTypeCode = obj.propertyTypeCode; 
    }

    id: number;
    description: string;
    identifier: string;
    propertyTypeCode: string;
}

export class PropertyModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.type = obj.type;
        this.floor = obj.floor;    
        this.area = obj.area || undefined;
        this.address = obj.address ? new Address(obj.address) : new Address();
        this.propertyConstructionTypeId = obj.propertyConstructionTypeId;    
        this.identifier = obj.identifier;    
        this.identifierType = obj.identifierType;    
        this.description = obj.description;    
        this.isManuallyAdded = obj.isManuallyAdded;    
    }

    id: number;
    type: string;
    floor: string;
    area?: number;
    address?: Address | null;
    propertyConstructionTypeId: string;
    identifier: string;
    identifierType: string;
    description: string;
    isManuallyAdded: boolean;
}



export class VesselRegistrationDataViewData {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.vesselId = obj.vesselId || 0;
        this.shipName = obj.shipName;
        this.shipNameLatin = obj.shipNameLatin;
        this.registrationPort = obj.registrationPort;
        this.registrationNumber = obj.registrationNumber;
        this.tom = obj.tom;
        this.page = obj.page;
        this.type = obj.type;
        this.status = obj.status;
        this.statusName = obj.statusName;
        this.statusNameEn = obj.statusNameEn;
    }

    id: number;
    vesselId: number;
    shipName: string;
    shipNameLatin: string;
    registrationPort: string;
    registrationNumber: string;
    tom: string;
    page: string;
    type: string;
    status: string;
    statusName: string;
    statusNameEn: string;
}


export class VesselOwnerViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.vesselId = obj.vesselId || 0;
        this.isCompany = obj.isCompany || undefined;
        this.companyName = obj.companyName;
        this.eik = obj.eik;
        this.personFirstName = obj.personFirstName;
        this.personMiddleName = obj.personMiddleName;
        this.personLastName = obj.personLastName;
        this.egn = obj.egn;
        this.imoNumber = obj.imoNumber;
        this.isUser = obj.isUser || undefined;
        this.address = obj.address;
    }

    id: number;
    vesselId: number;
    isCompany?: boolean;
    companyName: string;
    eik: string;
    personFirstName: string;
    personMiddleName: string;
    personLastName: string;
    egn: string;
    imoNumber: string;
    isUser?: boolean;
    address: string;

}

export class VesselEngineViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.vesselId = obj.vesselId || 0;
        this.systemModification = obj.systemModification;
        this.engineNumber = obj.engineNumber;
        this.power = obj.power || undefined;
        this.type = obj.type;
    }

    id: number;
    vesselId: number;
    systemModification: string;
    engineNumber: string;
    power?: number;
    type: string;
}

export class VesselViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.bt = obj.bt || undefined;
        this.nt = obj.nt || undefined;
        this.maxLength = obj.maxLength || undefined;
        this.lengthBetweenPerpendiculars = obj.lengthBetweenPerpendiculars || undefined;
        this.maxWidth = obj.maxWidth || undefined;
        this.waterplane = obj.waterplane || undefined;
        this.shipboardHeight = obj.shipboardHeight || undefined;
        this.deadweight = obj.deadweight || undefined;
        this.numberOfEngines = obj.numberOfEngines || undefined;
        this.enginesFuel = obj.enginesFuel;
        this.sumEnginePower = obj.sumEnginePower || undefined;
        this.bodyNumber = obj.bodyNumber;
        this.extensionRequestId = obj.extensionRequestId || undefined;

        this.engines = [];
        if (obj.engines) {
            obj.engines.forEach((engine: any) => {
                this.engines.push(new VesselEngineViewModel(engine));
            });
        }

        this.owners = [];
        if (obj.owners) {
            obj.owners.forEach((owner: any) => {
                this.owners.push(new VesselOwnerViewModel(owner));
            });
        }

        this.registrationData = new VesselRegistrationDataViewData();
        if (obj.registrationData) {
            this.registrationData = new VesselRegistrationDataViewData(obj.registrationData);
        }
        
    }

    id: number;
    bt?: number;
    nt?: number;
    maxLength?: number;
    lengthBetweenPerpendiculars?: number;
    maxWidth?: number;
    waterplane?: number;
    shipboardHeight?: number;
    deadweight?: number;
    numberOfEngines?: number;
    enginesFuel: string;
    sumEnginePower?: number;
    bodyNumber: string;
    extensionRequestId?: number;

    engines: VesselEngineViewModel[];
    owners: VesselOwnerViewModel[];
    registrationData: VesselRegistrationDataViewData;
}

export class AgriculturalMachineryViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.registrationNumber = obj.registrationNumber;
        this.frameNumber = obj.frameNumber;
        this.type = obj.type;
    }

    id: number;
    registrationNumber: string;
    frameNumber: string;
    type: string;
}
