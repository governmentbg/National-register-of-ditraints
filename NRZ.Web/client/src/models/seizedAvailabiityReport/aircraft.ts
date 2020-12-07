export default class Aircraft {
    producerName: string;
    producerNameEn: string;
    producerCountryCode: string;
    producerCountryName: string;
    airCategoryCode: string;
    airCategoryName: string;
    icao: string;
    msnSerialNumber: string;
    modelName: string;
    modelNameEn: string;

    constructor(obj: any = {}) {
        this.producerName = obj.producerName || '';
        this.producerNameEn = obj.producerNameEn || '';
        this.producerCountryCode = obj.producerCountryCode || '';
        this.producerCountryName = obj.producerCountryName || '';
        this.airCategoryCode = obj.airCategoryCode || '';
        this.airCategoryName = obj.airCategoryName || '';
        this.icao = obj.icao || '';
        this.msnSerialNumber = obj.msnSerialNumber || '';
        this.modelName = obj.modelName || '';
        this.modelNameEn = obj.modelNameEn || '';
    }
}
