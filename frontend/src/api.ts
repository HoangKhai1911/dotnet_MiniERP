import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // Thay đổi port nếu backend của bạn chạy port khác
});

// Interceptor: Tự động đính kèm Token vào Header trước khi gửi request
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

// Xử lý nếu Token hết hạn (Backend trả về 401)
api.interceptors.response.use((response) => {
  return response;
}, (error) => {
  if (error.response && error.response.status === 401) {
    localStorage.removeItem('token');
    window.location.href = '/login'; // Bắt đăng nhập lại
  }
  return Promise.reject(error);
});

export default api;