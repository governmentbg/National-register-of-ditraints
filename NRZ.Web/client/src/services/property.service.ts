import http from 'axios';
import { IPropertySearchRequestModel } from '@/interfaces/property';
import { PropertyModel } from '@/models/property';

class PropertyService {
    baseUrl = '/api/property/';

    searchForPropertyInDB(propertyType: string, data: IPropertySearchRequestModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}SearchInDB/${propertyType}`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    searchForPropertyInAdministrator(propertyType: string, data: IPropertySearchRequestModel) {
        return new Promise((resolve, reject) => {
            http.post(`${this.baseUrl}SearchInAdministrator/${propertyType}`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    create(data: PropertyModel) {
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

    update(data: PropertyModel) {
        return new Promise((resolve, reject) => {
            http.put(`${this.baseUrl}create`, data)
                .then(result => {
                    resolve(result.data);
                })
                .catch(error => {
                    reject(error);
                });
        });
    }
}

const propertyService = new PropertyService();
export default propertyService;