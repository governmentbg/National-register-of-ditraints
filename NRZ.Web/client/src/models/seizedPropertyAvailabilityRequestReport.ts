export default class SeizedPropertyAvailabilityRequestReport {
    distraintId: string;
    propertyName: string;
    propertyType: string;
    propertyTypeCode: string;
    enforcementDate: string;
    enforcementBy: string;
    enforcedAt: string;
    inFavorOf: string;
    debtorName: string;


    constructor(obj: any = {}) {
        this.distraintId = obj.distraintId || '';
        this.propertyName = obj.propertyName || '';
        this.propertyType = obj.propertyType || '';
        this.propertyTypeCode = obj.propertyTypeCode || '';
        this.enforcementDate = obj.enforcementDate || '';
        this.enforcementBy = obj.enforcedBy || '';
        this.enforcedAt = obj.enforcedAt || '';
        this.inFavorOf = obj.inFavorOf || '';
        this.debtorName = obj.debtorName || '';
    }
}
