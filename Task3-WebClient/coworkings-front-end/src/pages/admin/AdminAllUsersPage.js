// AdminAllUsers.js
import React, { useEffect, useState } from 'react';
import { getAllUsers, deleteUser } from '../../services/UserService';
import '../../styles/adminUsers.css';
import { FormattedMessage } from 'react-intl';

const AdminAllUsers = () => {
  const [users, setUsers] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: 'username', direction: 'ascending' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getAllUsers();
      setUsers(data);
    } catch (error) {
      console.error('Error fetching users:', error);
      setError('Failed to fetch users');
    } finally {
      setLoading(false);
    }
  };

  const handleSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  const sortedUsers = users.length > 0 ? [...users].sort((a, b) => {
    const aValue = a[sortConfig.key];
    const bValue = b[sortConfig.key];

    if (typeof aValue === 'string' && typeof bValue === 'string') {
      return sortConfig.direction === 'ascending'
        ? aValue.localeCompare(bValue)
        : bValue.localeCompare(aValue);
    } else {
      return sortConfig.direction === 'ascending'
        ? aValue - bValue
        : bValue - aValue;
    }
  }) : [];

  const handleDelete = async (userId) => {
    try {
      await deleteUser(userId);
      setUsers(users.filter((user) => user.userId !== userId));
    } catch (error) {
      console.error('Error deleting user:', error);
      setError('Failed to delete user');
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="admin-all-users">
      <h1><FormattedMessage id="adminAllUsers.allUsers" defaultMessage="All Users" /></h1>
      <table>
        <thead>
          <tr>
            <th onClick={() => handleSort('userId')}><FormattedMessage id="adminAllUsers.userID" defaultMessage="User ID" /></th>
            <th onClick={() => handleSort('email')}><FormattedMessage id="adminAllUsers.email" defaultMessage="Email" /></th>
            <th onClick={() => handleSort('fullname')}><FormattedMessage id="adminAllUsers.fullName" defaultMessage="Full Name" /></th>
            <th><FormattedMessage id="adminAllUsers.Actions" defaultMessage="Actions" /></th>
          </tr>
        </thead>
        <tbody>
          {sortedUsers.map((user) => (
            <tr key={user.userId}>
              <td>{user.userId}</td>
              <td>{user.email}</td>
              <td>{user.fullname}</td>
              <td>
                <button onClick={() => handleDelete(user.userId)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AdminAllUsers;
