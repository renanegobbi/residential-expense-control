import axios from "axios";

export const http = axios.create({
  baseURL: "/api/v1",
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json-patch+json",
  },
});