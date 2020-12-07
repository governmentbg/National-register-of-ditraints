import {IRole, IPermission} from '@/interfaces/auth';
import aspNetRole from "@/models/aspNetRole";

export default class UserModel {
    constructor(obj: any = {}) {
        this.id = obj.id;
        this.userId = obj.userId;
        this.userName = obj.userName;
        this.userType = obj.userType;
        this.authType = obj.authType;
        this.fullName = obj.fullName;
        this.email = obj.email;
        this.token = obj.token;
        this.tokenExpiration = new Date(obj.tokenExpiration);
        this.roles = obj.roles ? obj.roles.map((x: string) => ({name: x, nameNormalized: x.toUpperCase()})) : [];
        this.claims = new Map<string, IPermission>();

        if (obj.claims && obj.claims.length) {
            for (let i = 0; i < obj.claims.length; i++) {
                const claim = obj.claims[i];
                this.claims.set(claim.name, claim.permissions);
            }
        }
    }

    id: string | null;
    userId: string;
    userName: string;
    userType: string;
    authType: string;
    fullName: string;
    email: string;
    token: string;
    tokenExpiration?: Date;
    roles: IRole[];
    claims: Map<string, IPermission>;
}