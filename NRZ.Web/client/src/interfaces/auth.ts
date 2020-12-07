export interface IRole {
    name: string;
    nameNormalized: string;
    claims: Map<string, IPermission>;
}

export interface IPermission {
    [key: string]: boolean;
}

export interface ILoginModel{
    userName: string;
    password: string;
}

export interface IRegisterModel extends ILoginModel{
    repeatPassword: string;
    email: string;
}

export interface IResetPasswordModel {
    password: string;
    repeatPassword: string;
    token: string;
}

