import Address from "@/models/address";

export default class Company {
    name: string;
    eik: string;
    address?: Address | null;
    companyCaseNumber: string;

    constructor(obj: any = {}) {
        this.name = obj.name || '';
        this.eik = obj.eik || '';
        this.companyCaseNumber = obj.companyCaseNumber || '';
        this.address = obj.address ? new Address(obj.address) : null;
    }
}

export class RegixCompanyModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.uic = obj.uic;
        this.name = obj.name;
        this.legalFormAbbr = obj.legalFormAbbr;
        this.legalFormName = obj.legalFormName;
        this.statusCode = obj.statusCode;
        this.statusName = obj.statusName;
        this.statusNameEn = obj.statusNameEn;
        this.requestId = obj.requestId || 0;
        this.updatedAt = obj.updatedAt ? new Date(obj.updatedAt) : undefined;

    }

    id: number;
    uic: string;
    name: string;
    legalFormAbbr: string;
    legalFormName: string;
    statusCode: string;
    statusName: string;
    statusNameEn: string;
    requestId?: number;
    updatedAt?: Date;
}
