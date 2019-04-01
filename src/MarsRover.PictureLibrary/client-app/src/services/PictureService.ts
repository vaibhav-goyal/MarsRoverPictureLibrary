import PictureDTO from '../DTOs/PictureDTO';
import axios from "axios";
import { Promise, reject } from 'q';

export default class PictureService {
    getPicture(date: string, pictureNo: number): Promise<PictureDTO> {
        let query = "?date=" + encodeURIComponent(date);
        query += "&pictureNo=" + encodeURIComponent(pictureNo.toString());
        const promise = Promise<PictureDTO>((resolve, reject) => {
            axios.get('/api/pictures' + query)
                .then(res => {
                    const data: PictureDTO = res.data;
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
                        reject("Client Error");
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