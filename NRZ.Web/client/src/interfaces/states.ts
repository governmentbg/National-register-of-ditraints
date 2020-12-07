import User from '@/store/modules/user.module';
import { IRole, IPermission } from './auth';

export interface UserState {
    userId: string;
    userName: string;
    userType: string;
    authType: string;
    email: string;
    fullName: string;  
    token: string;
    tokenExpiration: Date|null;
    roles: IRole[];
    claims: Map<string, IPermission>;
}

export interface MainState {
    baseUrl: string;
    language: string;
    sideMenu: boolean;
    dateTimeFormat: string;
    dateTimeLongFormat: string;
    dateFormat: string;
    timeFormat: string;
    user: User|null;
    eAuthUrl: string;
}