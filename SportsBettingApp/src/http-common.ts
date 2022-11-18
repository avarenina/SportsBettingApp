import axios, { AxiosInstance } from "axios";
import { JsonHelper } from "./helpers/JsonHelper";

const apiClient: AxiosInstance = axios.create({
    //baseURL: "http://localhost:3000",
});

apiClient.defaults.headers.post['Content-Type'] = 'application/json';
  
export default apiClient;