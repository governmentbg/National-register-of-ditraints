import UserModel from '@/models/user';
import { UserState } from '@/interfaces/states';
import { IPermission, IRole } from '@/interfaces/auth';
import { VuexModule, Module, Mutation, Action } from 'vuex-module-decorators'

@Module({ namespaced: true })
class User extends VuexModule implements UserState {
    userId = ''
    fullName = ''
    userName = ''
    userType = ''
    authType = ''
    email = ''
    token = ''
    tokenExpiration: Date | null = null
    roles: IRole[] = []
    claims = new Map<string, IPermission>()

    constructor(args: VuexModule) {
        super(args);
        const userString = localStorage.getItem('appUser');

        if (userString) {
            const userData = JSON.parse(userString);
            this.userId = userData.userId;
            this.userName = userData.userName;
            this.userType = userData.userType;
            this.authType = userData.authType;
            this.email = userData.email;
            this.token = userData.token;
            this.tokenExpiration = new Date(userData.tokenExpiration);
            this.roles = userData.roles;
        }
    }

    @Mutation
    public setUserData(model: UserModel): void {
        this.userId = model.userId;
        this.userName = model.userName;
        this.userType = model.userType;
        this.authType = model.authType;
        this.email = model.email;
        this.token = model.token;
        this.tokenExpiration = model.tokenExpiration || null;
        this.roles = model.roles;
        this.claims = model.claims;
        this.fullName = model.fullName;
    }

    @Mutation
    public clearUserData(): void {
        this.userId = '';
        this.userName = '';
        this.userType = '';
        this.authType = '';
        this.email = '';
        this.token = '';
        this.tokenExpiration = null;
        this.roles = [];
    }

    @Action({ rawError: true })
    public setUser(model: UserModel) {
        localStorage.setItem('appUser', JSON.stringify(model));
        this.context.commit("setUserData", model);
    }

    @Action({ rawError: true })
    public removeUser() {
        localStorage.removeItem('user');
        this.context.commit("clearUserData")
    }

    //Getters
    get getUserId() { return this.userId }
    get getUserName() { return this.userName }
    get getUserType() { return this.userType }
    get getAuthType() { return this.authType }
    get getUserEmail() { return this.email }
    get getAuthToken() { return this.token }
    get getUserRoles() { return this.roles.join(', ') }
    get hasRole() {
        return (roleName: string | string[]) => {
            if (typeof roleName == 'string') {
                return this.roles.some(x => x.name == roleName);
            } else if (roleName instanceof Array) {
                roleName.forEach(role => {
                    const hasRole = this.roles.some(x => x.name == role);
                    if (hasRole) {
                        return hasRole;
                    }
                })
            }
            
            return false;
        }
    }
    get hasClaim() {
        return (claim: string, permission: string) => {
            const permissions = this.claims.get(claim);

            if (permissions) {
                return permissions[permission];
            }

            return false;
        }
    }
    get isAuthenticated() {
        return this.token && this.tokenExpiration && (new Date(this.tokenExpiration) > new Date())
    }
}
export default User;