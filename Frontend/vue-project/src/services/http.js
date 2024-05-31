import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: "https://localhost:44372/api",
    headers: {
        "Content-type": "application/json"
    }
});

export default axiosInstance