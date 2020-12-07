export default class Agriformachinery {
    registrationNumber: string;
    frameNumber: string;
    type: string;
    owner: string;

    constructor(obj: any = {}) {
        this.registrationNumber = obj.registrationNumber || '';
        this.frameNumber = obj.frameNumber || '';
        this.type = obj.type || '';
        this.owner = obj.owner || '';
    }
}
