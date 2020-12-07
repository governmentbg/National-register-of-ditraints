import axios from 'axios';
import store from '../store/store';
import router from '../router/router';
import Http from 'axios-typescript-response';

Http.axiosInstance.interceptors.request.use((config: any) => {
    config.baseURL = store.getters.baseUrl;
    //Auth token
    const token = store.state.user!.token;
    
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    //Culture
    const lang = store.getters.Language;
    config.headers["Accept-Language"] = lang || 'bg';
    config.headers["Accept"] = '*/*';
    config.headers["Content-Type"] = 'application/json';

    return config;
});

// Add a 401 response interceptor
Http.axiosInstance.interceptors.response.use((response: any) => {
    return response;
}, function (error: any) {
    if (401 === error.response.status) {
        store.dispatch('logout').then(() => {
            router.push('/login');
        });
    } else {
        return Promise.reject(error);
    }
});

export default Http;