export class EServiceSettingsModel {
    constructor(obj: any = {}) {
        this.serviceProviderName = obj.serviceProviderName;
        this.serviceProviderBank = obj.serviceProviderBank;
        this.serviceProviderBic = obj.serviceProviderBic;
        this.serviceProviderIban = obj.serviceProviderIban;
        this.currency = obj.currency;
        this.seizedPropertyCertificateRequestExpirationDays = obj.seizedPropertyCertificateRequestExpirationDays || undefined;
        this.seizedPropertyCertificateFee = obj.seizedPropertyCertificateFee || undefined;
        this.seizedPropertyCertificateReason = obj.seizedPropertyCertificateReason;
        this.seizedPropertyReportRequestExpirationDays = obj.seizedPropertyReportRequestExpirationDays || undefined;
        this.seizedPropertyByOwnerReportFee = obj.seizedPropertyByOwnerReportFee || undefined;
        this.seizedPropertyReportReason = obj.seizedPropertyReportReason;
        this.administrativeServiceNotificationUrl = obj.administrativeServiceNotificationUrl;
    }

    serviceProviderName: string;
    serviceProviderBank: string;
    serviceProviderBic: string;
    serviceProviderIban: string;
    currency: string;
    seizedPropertyCertificateRequestExpirationDays?: number;
    seizedPropertyCertificateFee?: number;
    seizedPropertyCertificateReason: string;
    seizedPropertyReportRequestExpirationDays?: number;
    seizedPropertyByOwnerReportFee?: number;
    seizedPropertyReportReason: string;
    administrativeServiceNotificationUrl: string;
}