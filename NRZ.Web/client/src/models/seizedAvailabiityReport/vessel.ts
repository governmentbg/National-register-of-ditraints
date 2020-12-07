export default class Vessel {
    bt: string;
    nt: string;
    maxLength: string;
    lengthBetweenPerpendiculars: string;
    maxWidth: string;
    waterplane: string;
    shipboardHeight: string;
    deadWeight: string;
    numberOfEngines: string;
    enginesFuel: string;
    sumEnginePower: string;
    bodyNumber: string;

    constructor(obj: any = {}) {
        this.bt = obj.bt || '';
        this.nt = obj.nt || '';
        this.maxLength = obj.maxLength || '';
        this.lengthBetweenPerpendiculars = obj.lengthBetweenPerpendiculars || '';
        this.maxWidth = obj.maxWidth;
        this.waterplane = obj.waterplane || '';
        this.shipboardHeight = obj.shipboardHeight || '';
        this.deadWeight = obj.deadWeight || '';
        this.numberOfEngines = obj.numberOfEngines || '';
        this.enginesFuel = obj.enginesFuel || '';
        this.sumEnginePower = obj.sumEnginePower || '';
        this.bodyNumber = obj.bodyNumber || '';
    }
}
