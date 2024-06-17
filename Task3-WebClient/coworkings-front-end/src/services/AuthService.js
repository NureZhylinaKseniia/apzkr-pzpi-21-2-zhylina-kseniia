// src/services/AuthService.js
import apiInstance from './ApiService';

export const register = async (userData) => {
    try {
        const response = await apiInstance.post('/api/user', userData);
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const login = async (credentials) => {
    try {
        const response = await apiInstance.post('/api/user/login', credentials);
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

export const logout = async () => {
    try {
        const response = await apiInstance.post('/api/user/logout');
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};