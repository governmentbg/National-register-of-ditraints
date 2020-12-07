import http from '@/services/http.service';
import Person, { RegixPersonModel } from '../models/person';


class PersonService {
    private baseUrl = '/api/person/';

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

    getByUserId(userId: string): Promise<Person> {
        return http.get<Person>(`${this.baseUrl}GetPersonByUserId/${userId}`, Person, true);
    }
}

const personService = new PersonService();
export default personService;
