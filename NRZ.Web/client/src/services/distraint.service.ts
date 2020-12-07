import http from 'axios';
import { DistraintCreateModel, DistraintEnforceModel, DistraintRevokeModel, DistraintExemptModel } from '../models/distraint';

class DistraintService {
    baseUrl = '/api/distraint/';

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

    search(searchText: string) {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}search/${searchText}`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    create(data: DistraintCreateModel) {
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

    get(id: number) {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}get/${id}`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    enforce(data: DistraintEnforceModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}enforce`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }

    revoke(data: DistraintRevokeModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}revoke`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }

    exempt(data: DistraintExemptModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}exempt`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
}

const distraintService = new DistraintService();
export default distraintService;