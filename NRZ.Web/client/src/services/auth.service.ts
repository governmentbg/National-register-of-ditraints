import http from './http.service';
import { ILoginModel, IRegisterModel, IResetPasswordModel } from '@/interfaces/auth';
import UserModel from '@/models/user';
import { AxiosResponse } from 'axios';
import { EAuthRequestModel } from '@/models/eAuthRequest.model';

class AuthService {
    baseUrl = '/api/account/';
    eAuthBaseUrl = '/eAuth/';

    eAuthLogin(): Promise<EAuthRequestModel> {
        return new Promise((resolve, reject) => {
            http
                .get<EAuthRequestModel>(`${this.eAuthBaseUrl}personRequest?type=login`)
                .then((result: AxiosResponse<EAuthRequestModel>) => {
                    resolve(new EAuthRequestModel(result.data));
                }).catch(error => {
                    reject(error);
                });
        }); 
    }

    eAuthRegister(userType: string | null, chsiNumber: number | null, email: string, lang: string | null): Promise<EAuthRequestModel> {
        return new Promise((resolve, reject) => {
            let url = `${this.eAuthBaseUrl}personRequest?type=register`
            if (userType) {
                url += `&userType=${userType}`;
            }
            if (chsiNumber) {
                url += `&chsiNumber=${chsiNumber}`;
            }

            if (lang) {
                url += `&lang=${lang}`;
            }

            if (email) {
                url += `&email=${email}`;
            }
            
            http
                .get<EAuthRequestModel>(url)
                .then((result: AxiosResponse<EAuthRequestModel>) => {
                    resolve(new EAuthRequestModel(result.data));
                }).catch(error => {
                    reject(error);
                });
        }); 
    }

    eAuthResquestPost(samlRequest: string | null, eAuthUrl: string) {
        return http.post(eAuthUrl, { SAMLRequest: samlRequest });
    }

    login(data: ILoginModel) {
        return http.post<UserModel>(`${this.baseUrl}login`, data, UserModel, true);
    }
    logout() {
        return new Promise((resolve, reject) => {
            http
                .post(`${this.baseUrl}logout`, null)
                .then((result: AxiosResponse) => {
                    resolve(result.data);
                }).catch(error => {
                    reject(error);
                });
        }); 
    }
    register(data: IRegisterModel) {
        return new Promise((resolve, reject) => {
            http
                .post(`${this.baseUrl}register`, data)
                .then((result: AxiosResponse) => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
    resetPassword(data: IResetPasswordModel) {
        return new Promise((resolve, reject) => {
            http
                .post(`${this.baseUrl}resetPassword`, data)
                .then(result => {
                    localStorage.setItem('user', JSON.stringify(result.data));
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
    sendResetPasswordMail(email: string) {
        return new Promise((resolve, reject) => {
            http
                .post(`${this.baseUrl}sendResetPasswordMail`, email)
                .then((result: AxiosResponse) => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error); 
                });
        });
    }
}

const authService = new AuthService();
export default authService;