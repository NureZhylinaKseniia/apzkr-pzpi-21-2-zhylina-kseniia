import apiInstance from './ApiService';

export const getAllUsers = async () => {
    try {
        const response = await apiInstance.get('/api/user');
        return response.data;
    } catch (error) {
        console.error('Error fetching users:', error.response ? error.response.data : error.message);
        throw error;
    }
};

export const deleteUser = async (userId) => {
    try {
        await apiInstance.delete(`/api/user/delete/${userId}`);
    } catch (error) {
        console.error('Error deleting user:', error.response ? error.response.data : error.message);
        throw error;
    }
};

export const updateUser = async (userId, userData) => {
    try {
        if (!userId) throw new Error("User ID is undefined");
        const response = await apiInstance.put(`/api/user/edit/${userId}`, userData);
        console.log("User updated successfully:", response.data);
        return response.data;
    } catch (error) {
        console.error('Error updating user:', error.response ? error.response.data : error.message);
        throw error;
    }
};

export const createUser = async (userData) => {
    try {
        const response = await apiInstance.post('/api/user', userData);
        return response.data;
    } catch (error) {
        console.error('Error creating user:', error.response ? error.response.data : error.message);
        throw error;
    }
};

export const getUserByEmail = async (email) => {
    try {
      const response = await apiInstance.get(`/api/user/byEmail/${email}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching user by email:', error);
      throw error;
    }
  };
  
  export const getManagerByEmail = async (email) => {
    try {
      const response = await apiInstance.get(`/api/manager/byEmail/${email}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching manager by email:', error);
      throw error;
    }
  };
