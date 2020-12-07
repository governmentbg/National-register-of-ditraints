export default class RealEstate {
    type: string;
    floor: string;
    area: string;
    propertyConstructionType: string;
    identifier: string;
    identifierType: string;
    description: string;

    constructor(obj: any = {}) {
        this.type = obj.type || '';
        this.floor = obj.floor || '';
        this.area = obj.area || '';
        this.propertyConstructionType = obj.propertyConstructionType || '';
        this.identifier = obj.identifier || '';
        this.identifierType = obj.identifierType || '';
        this.description = obj.description || '';
    }
}
