import { IResetPasswordModel, ILoginModel, IRegisterModel } from '../interfaces/auth';

export class LoginModel implements ILoginModel {
    userName = '';
    password = '';
}

export class RegisterModel extends LoginModel implements IRegisterModel{
    repeatPassword = '';
    email = '';
    firstName = '';
    middleName = '';
    lastName = '';
    userType = '';
}

export class ResetPasswordModel implements IResetPasswordModel {
    constructor(obj: any = {}) {
        this.password = obj.password || '';
        this.repeatPassword = obj.repeatPassword || '';
        this.token = obj.token || '';
    }

    password: string;
    repeatPassword: string;
    token: string;
}

export class LoginResultModel{
    
}