import { IAgriculturalMachineryCreationModel } from "@/interfaces/agriculturalMachinery";
import Company from './company';
import Person from './person';

export default class AgriculturalMachineryCreationModel implements IAgriculturalMachineryCreationModel {
    constructor(obj: any) {
        this.registrationNumber = obj.registrationNumber || "";
        this.frameNumber = obj.frameNumber || "";
        this.type = obj.type || "";
        this.person = obj.person || null;
        this.company = obj.company || null;
    }
    registrationNumber: string;
    frameNumber: string;
    type: string;
    person: Person | null;
    company: Company | null;
}