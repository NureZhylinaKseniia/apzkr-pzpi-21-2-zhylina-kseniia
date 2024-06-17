import apiInstance from './ApiService';

export const getAllRooms = async () => {
    try {
        const response = await apiInstance.get('/api/rooms' );
        return response.data;
    } catch (error) {
        console.error('Error fetching rooms:', error);
        throw error;
    }
};

export const createRoom = async (roomData) => {
    try {
      const response = await apiInstance.post('/api/rooms', roomData);
      return response.data;
    } catch (error) {
      console.error('Error creating room:', error);
      throw error;
    }
  };