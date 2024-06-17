// AdminAllBookings.js
import React, { useEffect, useState } from 'react';
import { getAllBookings, deleteBooking } from '../../services/BookingsService';
import '../../styles/adminBookings.css';
import { FormattedMessage } from 'react-intl';

const AdminAllBookings = () => {
  const [bookings, setBookings] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: 'startDateTime', direction: 'ascending' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchBookings();
  }, []);

  const fetchBookings = async () => {
    setLoading(true);  // Start loading
    setError(null);    // Clear previous errors
    try {
      const data = await getAllBookings();
      setBookings(data);
    } catch (error) {
      console.error('Error fetching bookings:', error);
      setError('Failed to fetch bookings');
    } finally {
      setLoading(false); // Stop loading
    }
  };

  const handleSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  const sortedBookings = bookings.length > 0 ? [...bookings].sort((a, b) => {
    const aValue = a[sortConfig.key];
    const bValue = b[sortConfig.key];

    if (typeof aValue === 'string' && typeof bValue === 'string') {
      return sortConfig.direction === 'ascending'
        ? aValue.localeCompare(bValue)
        : bValue.localeCompare(aValue);
    } else if (aValue instanceof Date && bValue instanceof Date) {
      return sortConfig.direction === 'ascending'
        ? new Date(aValue) - new Date(bValue)
        : new Date(bValue) - new Date(aValue);
    } else {
      return sortConfig.direction === 'ascending'
        ? aValue - bValue
        : bValue - aValue;
    }
  }) : [];

  const handleDelete = async (bookingId) => {
    try {
      await deleteBooking(bookingId);
      setBookings(bookings.filter((booking) => booking.bookingId !== bookingId));
    } catch (error) {
      console.error('Error deleting booking:', error);
      setError('Failed to delete booking');
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div className="admin-all-bookings">
      <h1><FormattedMessage id="adminAllBookings.allBookings" defaultMessage="All Bookings" /></h1>
      <table>
        <thead>
          <tr>
            <th onClick={() => handleSort('bookingId')}><FormattedMessage id="adminAllBookings.bookingID" defaultMessage="Booking ID" /></th>
            <th onClick={() => handleSort('startDateTime')}><FormattedMessage id="adminAllBookings.startDateTime" defaultMessage="Start Date & Time" /></th>
            <th onClick={() => handleSort('endDateTime')}><FormattedMessage id="adminAllBookings.endDateTime" defaultMessage="End Date & Time" /></th>
            <th><FormattedMessage id="adminAllBookings.bookingCode" defaultMessage="Booking Code" /></th>
            <th><FormattedMessage id="adminAllBookings.user" defaultMessage="User" /></th>
            <th><FormattedMessage id="adminAllBookings.actions" defaultMessage="Actions" /></th>
          </tr>
        </thead>
        <tbody>
          {sortedBookings.map((booking) => (
            <tr key={booking.bookingId}>
              <td>{booking.bookingId}</td>
              <td>{new Date(booking.startDateTime).toLocaleString()}</td>
              <td>{new Date(booking.endDateTime).toLocaleString()}</td>
              <td>{booking.bookingCode}</td>
              <td>{booking.user?.fullname}</td>
              <td>
                <button onClick={() => handleDelete(booking.bookingId)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AdminAllBookings;