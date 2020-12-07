export default class Address {
    constructor(obj: any = {}) {
        this.id = obj.id || null;
        this.regionId = obj.regionId || null;
        this.municipalityId = obj.municipalityId || null;
        this.cityId = obj.cityId || null;
        this.streetAddress = obj.streetAddress || null;
    }

    id: number|null;
    regionId: number|null;
    municipalityId: number|null;
    cityId: number|null;
    streetAddress: string|null;
}
