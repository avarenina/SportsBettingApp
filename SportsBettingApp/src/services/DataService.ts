import http from "@/http-common";

/* eslint-disable */
class DataService {
  getAllSports(): Promise<any> {
    return http.get("/data/sports");
  }
  getTimes(): Promise<any> {
    return http.get("/data/betting-days");
  }
  getBettingPairs(): Promise<any> {
    return http.get("/data/betting-pairs");
  }
}

export default new DataService();
