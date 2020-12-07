/* eslint-disable */
import Company from './company';
import FileModel from './file';
import Person from './person';

export class Auction {
    constructor(obj: any = {}) {
        this.id = obj.id;
        this.announcement = new AuctionAnnouncement(obj.announcement);
        this.winner = obj.winner ? new Person(obj.winner) : undefined;
        this.bids = obj.bids;
        this.endDate = obj.endDate;
        this.startDate = obj.startDate;
        this.bidStep = obj.bidStep;
        this.startPrice = obj.startPrice;
        this.currentPrice = obj.currentPrice;
        this.endPrice = obj.endPrice;
        this.completed = obj.completed;
        this.started = obj.started;
        this.canParticipate = obj.canParticipate || false;
        this.timeSpan = obj.timeSpan || null;
    }

    id: number;
    announcement: AuctionAnnouncement;
    winner?: Person;
    bids: [];
    endDate: Date;
    startDate: Date;
    bidStep: number;
    startPrice: number;
    currentPrice: number;
    endPrice: number;
    completed: boolean;
    started: boolean;
    canParticipate: boolean;
    timeSpan: number | null;
}

export class AuctionAnnouncement {
    constructor(obj: any = {}) {
        this.id = obj.id || null;
        this.code = obj.code;
        this.title = obj.title;
        this.order = obj.order;
        this.orderName = obj.orderName;
        this.propertyType = obj.propertyType;
        this.propertyTypeName = obj.propertyTypeName;
        this.startPrice = obj.startPrice;
        this.bidStep = obj.bidStep;
        this.registerStartDate = obj.registerStartDate ? new Date(obj.registerStartDate) : null;
        this.registerEndDate = obj.registerEndDate ? new Date(obj.registerEndDate) : null;
        this.auctionStartDate = obj.auctionStartDate ? new Date(obj.auctionStartDate) : null;
        this.auctionEndDate = obj.auctionEndDate ? new Date(obj.auctionEndDate) : null;
        this.items = obj.items || [];
        this.description = obj.description || '';
        this.attachments = [];

        if (obj.attachments && obj.attachments.length) {
            obj.attachments.forEach((element: any) => {
                const item = new FileModel({ id: element.id, file: element });

                this.attachments.push(item);
            });
        }
    }

    id: number | null;
    code: string;
    title: string;
    order: string;
    orderName: string;
    propertyType: string;
    propertyTypeName: string;
    startPrice: number;
    bidStep: number;
    registerStartDate: Date | null;
    registerEndDate: Date | null;
    auctionStartDate: Date | null;
    auctionEndDate: Date | null;
    description: string;
    items: object[];
    attachments: FileModel[];
}

export class AuctionItem {
    id: number | null = null;
    nrzId: number | null = null;
    propertyType = '';
    title = '';
    description = '';
    isManuallyAdded = false;
    objectType = '';
    identifier = '';
}

export class Registration {
    constructor(obj: any = {}) {
        this.id = obj.id;
        this.uniqueNumber = obj.uniqueNumber;
        this.participantId = obj.participantId;
        this.participant = obj.participant || new Person();
        this.announcementId = obj.announcementId;
        this.announcementCreatedBy = obj.announcementCreatedBy;
        this.isApproved = obj.isApproved;
        this.processedOn = obj.processedOn;
        this.processedBy = obj.processedBy;
        this.representationType = obj.representationType;
        this.representationTypeName = obj.representationTypeName;
        this.isOwner = obj.isOwner || false;
        this.isOwnerSpouse = obj.isOwnerSpouse || false;
        this.appliedByCourtEnforcer = obj.appliedByCourtEnforcer || false;
        this.createdOn = obj.createdOn;
        this.createdBy = obj.createdBy;
        this.representedUserId = obj.representedUserId;
        this.representedPersonId = obj.representedPersonId;
        this.representedPerson = obj.representedPerson;
        this.representedCompanyId = obj.representedCompanyId;
        this.representedCompany = obj.representedCompany;
        this.resultDeliveryType = obj.resultDeliveryType;
        this.resultDeliveryTypeName = obj.resultDeliveryTypeName;
        this.deletedOn = obj.deletedOn;
        this.deletedBy = obj.deletedBy;
        this.deleted = obj.deleted;
        this.rejectReason = obj.rejectReason;
        this.attachments = [];

        if (obj.attachments && obj.attachments.length) {
            obj.attachments.forEach((element: any) => {
                const item = new FileModel({ id: element.id, file: element });

                this.attachments.push(item);
            });
        }
    }

    id: number;
    uniqueNumber: string;
    participantId: string;
    participant: Person;
    announcementId: number;
    announcementCreatedBy: string;
    isApproved: boolean | null;
    processedOn: Date | null;
    processedBy: string;
    representationType: string;
    representationTypeName: string;
    isOwner: boolean;
    isOwnerSpouse: boolean;
    appliedByCourtEnforcer: boolean;
    createdOn: Date | null;
    createdBy: string;
    representedUserId: string;
    representedPersonId: number | null;
    representedPerson: Person | null;
    representedCompanyId: number | null;
    representedCompany: Company | null;
    resultDeliveryType: string;
    resultDeliveryTypeName: string;
    deletedOn: Date | null;
    deletedBy: string;
    deleted: boolean;
    rejectReason: string;
    attachments: FileModel[];
}