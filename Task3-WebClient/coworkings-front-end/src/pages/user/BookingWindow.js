// src/BookingWindow.js
import React, { useEffect, useState, useContext } from 'react';
import { getBookingsByWorkplaceId, createBooking } from '../../services/BookingsService';
import { AuthContext } from '../../contexts/AuthContext';
import '../../styles/bookingWindow.css';
import { FormattedMessage } from 'react-intl';

const BookingWindow = ({ workplaceId, onClose }) => {
  const [bookings, setBookings] = useState([]);
  const [selectedDate, setSelectedDate] = useState(null);
  const [selectedTime, setSelectedTime] = useState(null);

  useEffect(() => {
    const fetchBookings = async () => {
      try {
        const bookingsData = await getBookingsByWorkplaceId(workplaceId);
        setBookings(bookingsData);
      } catch (error) {
        console.error('Error fetching bookings:', error);
      }
    };

    fetchBookings();
  }, [workplaceId]);

  const handleBooking = async () => {
    if (selectedDate && selectedTime) {
      const startDateTime = new Date(`${selectedDate}T${selectedTime}:00`);
      const endDateTime = new Date(startDateTime);
      endDateTime.setHours(startDateTime.getHours() + 1);

      const userData = localStorage.getItem('user');
      let user = null;
      if (userData) {
        user = JSON.parse(userData);
      }

      const bookingData = {
        userId: user.userId,
        startDateTime: startDateTime.toISOString(),
        endDateTime: endDateTime.toISOString(),
        workplacesId: [workplaceId]
      };

      try {
        const response = await createBooking(bookingData);
        alert('Booking created successfully');
        onClose();
      } catch (error) {
        console.error('Error creating booking:', error);
        alert('Failed to create booking');
      }
    } else {
      alert('Please select a date and time');
    }
  };

  const isTimeBooked = (date, time) => {
    const startDateTime = new Date(`${date}T${time}:00`);
    return bookings.some(booking => {
      const bookingStart = new Date(booking.startDateTime);
      const bookingEnd = new Date(booking.endDateTime);
      return startDateTime >= bookingStart && startDateTime < bookingEnd;
    });
  };

  const hours = Array.from({ length: 13 }, (_, i) => i + 8);

  return (
    <div className="booking-window">
      <h2><FormattedMessage id="userRooms.BookWorkplace" defaultMessage="Book Workplace" /></h2>
      <button onClick={onClose}>Close</button>
      <input
        type="date"
        value={selectedDate || ''}
        onChange={(e) => setSelectedDate(e.target.value)}
      />
      {selectedDate && (
        <div>
          {hours.map((hour) => {
            const time = hour.toString().padStart(2, '0') + ':00';
            return (
              <button
                key={hour}
                disabled={isTimeBooked(selectedDate, time)}
                className={`booking-time-button ${selectedTime === time ? 'selected' : ''}`}
                onClick={() => setSelectedTime(time)}
              >
                {time}
              </button>
            );
          })}
        </div>
      )}
      <button onClick={handleBooking}><FormattedMessage id="userRooms.Book" defaultMessage="Book" /></button>
    </div>
  );
};

export default BookingWindow;
