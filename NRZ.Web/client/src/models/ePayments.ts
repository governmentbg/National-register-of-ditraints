
export class PaymentRequestModel {
    constructor(obj: any = {}) {
        this.aisPaymentId = obj.aisPaymentId || 0;
        this.serviceProviderName = obj.serviceProviderName;
        this.serviceProviderBank = obj.serviceProviderBank;
        this.serviceProviderBic = obj.serviceProviderBic;
        this.serviceProviderIban = obj.serviceProviderIban;
        this.currency = obj.currency;
        this.paymentTypeCode = obj.paymentTypeCode;
        this.paymentAmount = obj.paymentAmount || 0;
        this.paymentReason = obj.paymentReason;
        this.applicantUinTypeId = obj.applicantUinTypeId || 0;
        this.applicantUin = obj.applicantUin;
        this.applicantName = obj.applicantName;
        this.paymentReferenceType = obj.paymentReferenceType;
        this.paymentReferenceNumber = obj.paymentReferenceNumber;
        this.paymentReferenceDate = new Date(obj.paymentReferenceDate);
        this.expirationDate = new Date(obj.expirationDate);
        this.additionalInformation = obj.additionalInformation;
        this.administrativeServiceUri = obj.administrativeServiceUri;
        this.administrativeServiceSupplierUri = obj.administrativeServiceSupplierUri;
        this.administrativeServiceNotificationUrl = obj.administrativeServiceNotificationUrl;
    }

    aisPaymentId: number;
    serviceProviderName: string;
    serviceProviderBank: string;
    serviceProviderBic: string;
    serviceProviderIban: string;
    currency: string;
    paymentTypeCode: string;
    paymentAmount: number;
    paymentReason: string;
    applicantUinTypeId: number;
    applicantUin: string;
    applicantName: string;
    paymentReferenceType: string;
    paymentReferenceNumber: string;
    paymentReferenceDate: Date;    
    expirationDate: Date;
    additionalInformation: string;
    administrativeServiceUri: string;
    administrativeServiceSupplierUri: string;
    administrativeServiceNotificationUrl: string;
}

export class EServicePaymentRequestModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.aserviceTypeCode = obj.aserviceTypeCode;
        this.eserviceTypeName = obj.eserviceTypeName;
        this.eserviceTypeNameEn = obj.eserviceTypeNameEn;
        this.seizedPropertyCertificateRequestId = obj.seizedPropertyCertificateRequestId || 0;
        this.seizedPropertyReportRequestId = obj.seizedPropertyReportRequestId || 0;
        this.serviceRequestId = obj.serviceRequestId || 0;
        this.paymentRequestId = obj.paymentRequestId || 0;
        this.createdAt = new Date(obj.createdAt);
        this.statusCode = obj.statusCode;
        this.statusName = obj.statusName;
        this.statusNameEn = obj.statusNameEn;
        this.updatedAt = new Date(obj.updatedAt);
    }

    id: number;
    aserviceTypeCode: string;
    eserviceTypeName: string;
    eserviceTypeNameEn: string;
    seizedPropertyCertificateRequestId?: number;
    seizedPropertyReportRequestId?: number;
    serviceRequestId: number;
    paymentRequestId: number;
    createdAt: Date;
    statusCode: string;
    statusName: string;
    statusNameEn: string;
    updatedAt?: Date;
}

export class EServicePaymentStatusHistoryModel {
    constructor(obj: any = {}) {
        this.id = obj.id || 0;
        this.requestId = obj.requestId || 0;
        this.statusCode = obj.statusCode;
        this.statusCodeName = obj.statusCodeName;
        this.statusCodeNameEn = obj.statusCodeNameEn;
        this.updatedAt = new Date(obj.updatedAt);
        this.eServiceTime = new Date(obj.eServiceTime);
        this.errors = obj.errors
    }

    id: number;
    requestId: number;
    statusCode: string;
    statusCodeName: string;
    statusCodeNameEn: string;
    updatedAt: Date;
    eServiceTime?: Date;
    errors: string;
}