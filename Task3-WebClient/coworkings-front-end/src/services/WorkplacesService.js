import apiInstance from './ApiService';

export const getAllWorkplacesByRoomId = async (roomId) => {
  try {
    const response = await apiInstance.get(`/api/workplaces/byRoom/${roomId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching workplaces:', error);
    throw error;
  }
};

export const createWorkplace = async (workplaceData) => {
  try {
    const response = await apiInstance.post('/api/workplaces', workplaceData);
    return response.data;
  } catch (error) {
    console.error('Error creating workplace:', error);
    throw error;
  }
};