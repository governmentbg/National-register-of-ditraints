import Company from '@/models/company';
import Person from '@/models/person';

export interface IAgriculturalMachineryCreationModel{
    registrationNumber: string;
    frameNumber: string;
    type: string;
    person: Person | null;
    company: Company | null;
}