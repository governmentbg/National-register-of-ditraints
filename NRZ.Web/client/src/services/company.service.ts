import http from 'axios';
import { RegixCompanyModel } from '../models/company';


class CompanyService {
    baseUrl = '/api/company/';

    getFromRegix(identifier: string) {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}getFromRegix/${identifier}`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }


}

const companyService = new CompanyService();
export default companyService;
