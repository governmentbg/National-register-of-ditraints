import Address from "@/models/address";

export default class Person {
    id: number | null;
    firstName: string;
    middleName: string;
    lastName: string;
    phone: string;
    email: string;
    identificationNumber: string;
    identificationType: string | null;
    userId: string | null;
    address?: Address | null;

    constructor(obj: any = {}) {
        this.id = obj.id || null;
        this.firstName = obj.firstName || '';
        this.middleName = obj.middleName || '';
        this.lastName = obj.lastName || '';
        this.phone = obj.phone || '';
        this.email = obj.email || '';
        this.identificationNumber = obj.identificationNumber || '';
        this.userId = obj.userId || null;
        this.identificationType = obj.identificationType || null;
        this.address = obj.address ? new Address(obj.address) : null;
    }
}

export class RegixPersonModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.identifier = obj.identifier;
        this.firstName = obj.firstName;
        this.middleName = obj.middleName;
        this.lastName = obj.lastName;
        this.dateOfBirth = obj.dateOfBirth ? new Date(obj.dateOfBirth) : undefined;
        this.dateOfDeath = obj.dateOfDeath ? new Date(obj.dateOfDeath) : undefined;
        this.fullName = `${this.firstName ?? ''} ${this.middleName ?? ''} ${this.lastName ?? ''}`;
        this.requestId = obj.requestId || 0;
    }

    id: number;
    identifier: string;
    firstName: string;
    middleName: string;
    lastName: string;
    dateOfBirth?: Date;
    dateOfDeath?: Date;
    fullName: string;
    requestId?: number;
}
