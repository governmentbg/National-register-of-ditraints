export class EAuthRequestModel {
    // eslint-disable-next-line
    constructor(obj: any = {}) {
        this.requestId = obj.requestId;
        this.eAuthUrl = obj.eAuthUrl;
        this.SAMLRequest = obj.samlRequest;
        this.relayState = obj.relayState;
        this.samlRequestBeautified = obj.samlRequestBeautified;
        this.samlRequestDecoded = obj.samlRequestDecoded;
        this.relayStateDecoded = obj.relayStateDecoded;
        this.signatureStatusName = obj.signatureStatusName;
    }


    requestId: string | null;
    eAuthUrl: string | null;
    SAMLRequest: string | null; // Base64 кодиран. Думата SAML е с главни букви, защото това е изискване на SAML протокола и в частност на еАвт.
    relayState: string | null;
    samlRequestBeautified: string | null;
    samlRequestDecoded: string | null;
    relayStateDecoded: string | null;
    signatureStatusName: string | null;
}
