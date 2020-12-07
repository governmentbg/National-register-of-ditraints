import http from 'axios';

class SettingsService {
    baseUrl = '/api/settings/';

    getVersion() {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}Version`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    getUseRegix() {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}UseRegix`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }

    getToday() {
        return new Promise((resolve, reject) => {
            http.get(`${this.baseUrl}today`)
                .then(result => {
                    resolve(result.data);
                })
                .catch(result => {
                    reject(result);
                });
        });
    }
}

const settingsService = new SettingsService();
export default settingsService;