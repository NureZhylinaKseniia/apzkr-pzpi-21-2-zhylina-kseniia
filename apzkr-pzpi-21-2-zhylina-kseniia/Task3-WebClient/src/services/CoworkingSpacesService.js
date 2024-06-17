import apiInstance from './ApiService';

export const getAllCoworkingSpaces = async () => {
  try {
    const response = await apiInstance.get('/api/coworkingSpaces');
    return response.data;
  } catch (error) {
    console.error('Error fetching coworking spaces:', error);
    throw error;
  }
};

export const getCoworkingSpaceByManagerId = async (managerId) => {
  try {
    const response = await apiInstance.get(`/api/coworkingSpaces/byManager/${managerId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching coworking space by manager ID:', error);
    throw error;
  }
};