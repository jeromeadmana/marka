import axios from 'axios';
import type { Marka, CreateMarkaDto, UpdateMarkaDto } from '../types/marka';
import { authService } from './authService';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5229';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add auth token to all requests
api.interceptors.request.use(
  (config) => {
    const token = authService.getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Handle 401 responses by redirecting to login
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      authService.logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const markasApi = {
  getAll: async (): Promise<Marka[]> => {
    const response = await api.get<Marka[]>('/api/markas');
    return response.data;
  },

  getById: async (id: string): Promise<Marka> => {
    const response = await api.get<Marka>(`/api/markas/${id}`);
    return response.data;
  },

  create: async (data: CreateMarkaDto): Promise<Marka> => {
    const response = await api.post<Marka>('/api/markas', data);
    return response.data;
  },

  update: async (id: string, data: UpdateMarkaDto): Promise<void> => {
    await api.put(`/api/markas/${id}`, data);
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/api/markas/${id}`);
  },
};

export default api;
