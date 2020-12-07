export enum PropertyType {
    Vehicle = 'VEHICLE',
    Aircraft = 'AIRCRAFT',
    Vessel = 'VESSEL',
    AgriForMachinery = 'AGRIFORMACHINERY',
    RealEstate = 'REALESTATE',
    Other = 'OTHER',
}

export enum DistraintStatus {
    Registered = 'REGISTERED',
    Enforced = 'ENFORCED',
    Revoked = 'REVOKED',
    Exempted = 'EXEMPTED',
}

export enum RegixSearchCriteriaType {
    REGNUMBER = 'REGNUMBER',
    OWNER = 'OWNER',
    MSN = 'MSN',
}

export enum UserType {
    CHSI = 'CHSI',
    CHSIHELPER = 'CHSIHELPER',
    DSI = 'DSI',
    NAP = 'NAP',
    SYN = 'SYN',
    AUCPAR = 'AUCPAR',
}

export enum AuthType {
    USER = 'USER',
}

export enum UserRegisterType {
    USER = 'USER',
    ESIGN = 'ESIGN',
    EAUTH = 'EAUTH',
    CHSI = 'CHSI'
}

export enum IdentificationType {
    EGN = 'EGN',
    LN4 = 'LN4' // Как ... как е ЛНЧ на английски???
}

export enum PersonType {
    PhysicalPerson = 'PHYSICAL_PERSON',
    Company = 'COMPANY'
}

export enum UserRoles {
    SysAdmin = 'SYSADMIN',
    AuctionParticipant = 'AUCTIONPARTICIPANT',
    AuctionOrgaziner = 'AUCTIONORGAZINER'
}

export enum RequesterType {
    CHSI = 'CHSI',
    CHSIHELPER = 'CHSIHELPER',
    DSI = 'DSI',
    NAP = 'NAP',
    SYN = 'SYN',
    AUCPAR = 'AUCPAR',
    APPLICANT = 'APPLICANT'
}
