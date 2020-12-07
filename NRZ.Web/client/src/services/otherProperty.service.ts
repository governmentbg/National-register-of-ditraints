import http from 'axios';
import { OtherPropertyModel } from "@/models/property";

class OtherPropertyService {
    baseUrl = '/api/otherProperty/';

    create(data: OtherPropertyModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}create`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
}

const otherPropertyService = new OtherPropertyService();
export default otherPropertyService;