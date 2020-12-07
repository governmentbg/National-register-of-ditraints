import Person from "@/models/person";
import Company from "@/models/company";

export default class SeizedPropertyAvailabilityRequest {
    requester: Person;
    inTheQualityOfPersonTypeCode: string;
    requesterCompanyRepresentative: string;
    requesterCompanyEik: string;
    requesterCompanyCaseNumber: string;
    isCheckedPerson: boolean;
    checkedPerson?: Person;
    checkedCompany?: Company;

    constructor(obj: any = {}) {
        this.requester = new Person(obj.requester);
        this.inTheQualityOfPersonTypeCode = obj.inTheQualityOfPersonTypeCode || '';
        this.requesterCompanyRepresentative = obj.requesterCompanyRepresentative || '';
        this.requesterCompanyEik = obj.requesterCompanyEik || '';
        this.requesterCompanyCaseNumber = obj.requesterCompanyCaseNumber || '';
        this.isCheckedPerson = obj.isCheckedPerson || false;
        this.checkedPerson = this.isCheckedPerson ? new Person(obj.checkedPerson) : undefined;
        this.checkedCompany = this.isCheckedPerson ? undefined : new Company(obj.checkedCompany);
    }
}
