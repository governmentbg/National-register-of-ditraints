export class Request {
    constructor(obj: any = {}) {
        this.firstName = obj.firstName || null; 
        this.middleName = obj.middleName || null; 
        this.lastName = obj.lastName || null; 
        this.personalIdentifier = obj.personalIdentifier || null; 
        this.regionId = obj.regionId || null; 
        this.municipalityId = obj.municipalityId || null; 
        this.cityId = obj.cityId || null; 
        this.streetAddress = obj.streetAddress || null; 
        this.phoneNumber = obj.phoneNumber || null; 
        this.email = obj.email || null; 
        this.userTypeCode = obj.userTypeCode || null; 
        this.nameOfLegalEntity = obj.nameOfLegalEntity || null; 
        this.identifierOfLegalEntity = obj.identifierOfLegalEntity || null; 
        this.regionIdOfLegalEntity = obj.regionIdOfLegalEntity || null; 
        this.municipalityIdOfLegalEntity = obj.municipalityIdOfLegalEntity || null; 
        this.cityIdOfLegalEntity = obj.cityIdOfLegalEntity || null; 
        this.streetAddressOfLegalEntity = obj.streetAddressOfLegalEntity || null; 
        this.companyCaseNumber = obj.companyCaseNumber || null; 
        this.propertyTypeCode = obj.propertyTypeCode || null;
        this.propertyId = obj.propertyId || null;
        this.isPersonalIdentifierTypeLNCh = obj.isPersonalIdentifierTypeLNCh != undefined ? obj.isPersonalIdentifierTypeLNCh : null;
    }
    
    firstName: string;
    middleName: string;
    lastName: string;
    personalIdentifier: string;
    regionId: number;
    municipalityId: number;
    cityId: number;
    streetAddress: string;
    phoneNumber: string;
    email: string;
    userTypeCode: string;
    nameOfLegalEntity: string;
    identifierOfLegalEntity: string;
    regionIdOfLegalEntity: string;
    municipalityIdOfLegalEntity: number;
    cityIdOfLegalEntity: number;
    streetAddressOfLegalEntity: string;
    companyCaseNumber: string;
    propertyTypeCode: string;
    propertyId: number;
    isPersonalIdentifierTypeLNCh: boolean;
}

export class RequestViewModel {
    constructor(obj: any = {}) {
        this.id = obj.id || null; 
        this.firstName = obj.firstName || null; 
        this.middleName = obj.middleName || null; 
        this.lastName = obj.lastName || null; 
        this.personalIdentifierType = obj.personalIdentifierType || null;
        this.personalIdentifier = obj.personalIdentifier || null; 
        this.region = obj.region || null; 
        this.municipality = obj.municipality || null; 
        this.city = obj.city || null; 
        this.streetAddress = obj.streetAddress || null; 
        this.phoneNumber = obj.phoneNumber || null; 
        this.email = obj.email || null;

        // todo: get User type name drom DB
        let userTypeName;
        switch(obj.userTypeCode) {
            case 'AUCPAR':
                userTypeName = 'Участник в публична продан'
                break;
            case 'CHSI':
                userTypeName = 'Частен съдебен изпълнител'
                break;
            case 'CHSIHelper':
                userTypeName = 'Помощник на ЧСИ'
                break;
            case 'DSI':
                userTypeName = 'Държавен съдебен изпълнител'
                break;
            case 'NAP':
                userTypeName = 'Публичен изпълнител'
                break;
            case 'SYN':
                userTypeName = 'Синдици'
                break;
            case 'REQUESTER':
                userTypeName = 'Заявител'
                break;
            default:
        }
        this.userTypeName = userTypeName || ''; 

        this.nameOfLegalEntity = obj.nameOfLegalEntity || null; 
        this.identifierOfLegalEntity = obj.identifierOfLegalEntity || null; 
        this.regionOfLegalEntity = obj.regionOfLegalEntity || null; 
        this.municipalityOfLegalEntity = obj.municipalityOfLegalEntity || null; 
        this.cityOfLegalEntity = obj.cityOfLegalEntity || null; 
        this.streetAddressOfLegalEntity = obj.streetAddressOfLegalEntity || null; 
        this.companyCaseNumber = obj.companyCaseNumber || null; 
        this.propertyTypeName = obj.propertyTypeName || null;
        this.propertyTypeNameEn = obj.propertyTypeNameEn || null;
        this.createdOn = obj.createdOn || null;

        const propertyIds = [
            (Number)(obj.propertyIdOtherProperty),
            (Number)(obj.propertyIdVehicle),
            (Number)(obj.propertyIdAircraft)];
            const propertyId = propertyIds.filter(id => id)[0];
        this.propertyId = propertyId || undefined;

        const propertyIdentifiers = [
            obj.otherPropertyIdentifier,
            obj.vehicleRegistrationNumber,
            obj.aircraftMsnserialNumber];
        this.propertyRegistrationNumber = propertyIdentifiers.filter(id => id)[0];
        this.statusCode = obj.statusCode;
        this.statusName = obj.statusName;
        this.statusNameEn = obj.statusNameEn;
    }
    
    id: number;
    firstName: string;
    middleName: string;
    lastName: string;
    personalIdentifierType: string;
    personalIdentifier: string;
    region: string;
    municipality: string;
    city: string;
    streetAddress: string;
    phoneNumber: string;
    email: string;
    userTypeName: string;
    nameOfLegalEntity: string;
    identifierOfLegalEntity: string;
    regionOfLegalEntity: string;
    municipalityOfLegalEntity: string;
    cityOfLegalEntity: string;
    streetAddressOfLegalEntity: string;
    companyCaseNumber: string;
    propertyTypeName: string;
    propertyTypeNameEn: string;
    propertyId?: number;
    propertyRegistrationNumber: string;
    createdOn: string;
    statusCode: string;
    statusName: string;
    statusNameEn: string;
}