import axios from "axios";
import { Promise, reject } from 'q';

export default class DatesService {

    getValidDates(): Promise<string[]> {
        const promise = Promise<string[]>((resolve, reject) => {
            axios.get('/api/validdates')
                .then(res => {
                    const data: string[] = res.data
                    resolve(data);
                })
                .catch(error => {
                    if (error.response) {
                        // The request was made and the server responded with a status code
                        // that falls out of the range of 2xx
                        console.log(error.response.data);
                        console.log(error.response.status);
                        console.log(error.response.headers);
                        reject(error.response.data);
                    } else if (error.request) {
                        // The request was made but no response was received
                        // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
                        // http.ClientRequest in node.js
                        console.log(error.request);
                        reject("Client error");
                    } else {
                        // Something happened in setting up the request that triggered an Error
                        console.log('Error', error.message);
                        reject(error.message);
                    }
                });
        });
        return promise;        
    }
}