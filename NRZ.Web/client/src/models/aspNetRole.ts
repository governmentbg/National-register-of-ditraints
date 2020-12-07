import UserModel from "@/models/user";
import {IPermission, IRole} from "@/interfaces/auth";

export default class AspNetRole implements IRole{
    constructor(obj: any = {}) {
        this.id = obj.id;
        this.name = obj.name;
        this.nameNormalized = obj.nameNormalized;
        this.active = obj.active;
        this.deactivated = obj.deactivated;
        this.users = [];
        this.claims = new Map<string, IPermission>();

        if (obj.users) {
            obj.users.forEach((user: any) => {
                this.users.push(new UserModel(user))
            })
        }

        if (obj.claims && obj.claims.length) {
            for (let i = 0; i < obj.claims.length; i++) {
                const claim = obj.claims[i];
                this.claims.set(claim.name, claim.permissions);
            }
        }
    }

    id: string;
    name: string;
    nameNormalized: string;
    active: boolean;
    deactivated: boolean;
    users: UserModel[];
    claims: Map<string, IPermission>;
}