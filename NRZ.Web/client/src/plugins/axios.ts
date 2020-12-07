import axios from 'axios';
import store from '../store/store';
import router from '../router/router';

axios.interceptors.request.use(config => {
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

    return config;
});

// Add a 401 response interceptor
axios.interceptors.response.use(response => {
    return response;
}, function (error) {
    if (401 === error.response.status) {
        store.dispatch('logout').then(() => {
            router.push('/login');
        });
    } else {
        return Promise.reject(error);
    }
});