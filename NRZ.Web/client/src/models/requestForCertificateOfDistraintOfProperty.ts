import Person from "@/models/person";
import Company from "@/models/company";
import { PropertyItemModel } from "@/models/property";

export default class RequestForCertificateOfDistraintOfProperty {
    requester: Person;
    inTheQualityOfPersonTypeCode: string;
    requesterCompany: Company;
    propertyItems: PropertyItemModel[] = [];

    constructor(obj: any = {}) {
        this.propertyItems = [];
        this.requester = new Person(obj.requester);
        this.requesterCompany = new Company(obj.requesterCompany);
        this.inTheQualityOfPersonTypeCode = obj.inTheQualityOfPersonTypeCode || '';
        if (obj.propertyItems && obj.propertyItems.length) {
            obj.propertyItems.forEach((element: PropertyItemModel) => {
                const item = new PropertyItemModel(
                    {
                        id: element.id,
                        description: element.description,
                        identifier: element.identifier,
                        propertyTypeCode: element.propertyTypeCode,
                    });
                this.propertyItems.push(item);
           }); 
        }
    }
}
