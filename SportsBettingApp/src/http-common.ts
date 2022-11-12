import axios, { AxiosInstance } from "axios";
import { JsonHelper } from "./helpers/JsonHelper";

const apiClient: AxiosInstance = axios.create({
    //baseURL: "http://localhost:3000",

    headers: {
        "Content-type": "application/json",
    }
});


  
export default apiClient;