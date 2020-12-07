export default class Vehicle {
    vehicleType: string;
    model: string;
    category: string;
    color: string;
    registrationNumber: string;
    firstRegistrationDate: string;
    vin: string;
    capacity: string;
    maxPower: string;
    fuel: string;
    environmentalCategory: string;
    vehicleDocumentNumber: string;
    vehicleDocumentDate: string;

    constructor(obj: any = {}) {
        this.registrationNumber = obj.registrationNumber || '';
        this.firstRegistrationDate = obj.firstRegistrationDate || '';
        this.vin = obj.vin || '';
        this.vehicleType = obj.vehicleType || '';
        this.model = obj.model || '';
        this.color = obj.color || '';
        this.category = obj.category || '';
        this.capacity = obj.capacity || '';
        this.maxPower = obj.maxPower || '';
        this.fuel = obj.fuel || '';
        this.environmentalCategory = obj.environmentalCategory || '';
        this.vehicleDocumentNumber = obj.vehicleDocumentNumber || '';
        this.vehicleDocumentDate = obj.vehicleDocumentDate || '';
    }
}
