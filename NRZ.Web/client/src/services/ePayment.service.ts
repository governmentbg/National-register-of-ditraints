import http from './http.service';
import { EServiceSettingsModel } from '../models/eServicesSettings';

class EPaymentService {
    baseUrl = '/api/ePayment/';

    list() {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}list`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    getPaymentRequestHistory(requestId: number) {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}getPaymentRequestHistory/${requestId}`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    loadSettings() {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}loadSettings`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    changeSettings(data: EServiceSettingsModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}changeEServiceSettings`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }

}

const ePaymentService = new EPaymentService();
export default ePaymentService;