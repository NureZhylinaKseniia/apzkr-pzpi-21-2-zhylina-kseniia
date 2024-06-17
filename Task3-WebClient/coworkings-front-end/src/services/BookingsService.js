import apiInstance from './ApiService';

export const getBookingsByWorkplaceId = async (workplaceId) => {
  try {
    const response = await apiInstance.get(`/api/booking/byWorkplace/${workplaceId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching bookings:', error);
    throw error;
  }
};

export const createBooking = async (bookingData) => {
  try {
    const response = await apiInstance.post('/api/booking', bookingData);
    return response.data;
  } catch (error) {
    console.error('Error creating booking:', error);
    throw error;
  }
};

export const getBookingsByUser = async (userId) => {
  try {
      const response = await apiInstance.get(`/api/booking/byUser/${userId}`);
      return response.data;
  } catch (error) {
      console.error('Error fetching bookings:', error);
      throw error;
  }
};

export const getAllBookings = async () => {
  try {
    const response = await apiInstance.get('/api/booking');
    return response.data;
  } catch (error) {
    console.error('Error fetching all bookings:', error);
    throw error;
  }
};

export const deleteBooking = async (bookingId) => {
  try {
    await apiInstance.delete(`/api/booking/delete/${bookingId}`);
  } catch (error) {
    console.error('Error deleting booking:', error);
    throw error;
  }
};

export default apiInstance;