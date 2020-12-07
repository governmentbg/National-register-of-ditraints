export class Nomenclature {
    constructor(obj: any = {}) {
        this.code = obj.code;
        this.name = obj.name;  
        this.nameEn = obj.nameEn;
        this.deactivated = obj.deactivated || false;

    }

    code: string;
    name: string;
    nameEn: string;
    deactivated: boolean;
}

export class PropertyTypeModel extends Nomenclature {
}