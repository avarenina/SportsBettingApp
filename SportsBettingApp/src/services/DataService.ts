import http from "@/http-common";

/* eslint-disable */
class DataService {
  getAllSports(): Promise<any> {
    return http.get("/api/data/sports");
  }
  getTimes(): Promise<any> {
    return http.get("/api/data/betting-days");
  }
  getBettingPairs(): Promise<any> {
    return http.get("/api/data/betting-pairs");
  }
  getSpecialOffer(): Promise<any> {
    return http.get("/api/data/special-offer");
  }
}

export default new DataService();
