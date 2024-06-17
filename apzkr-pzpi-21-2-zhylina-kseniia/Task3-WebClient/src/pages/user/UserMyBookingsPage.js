import React, { useEffect, useState } from 'react';
import { FormattedMessage } from 'react-intl';
import '../../styles/mybookings.css';
import { getBookingsByUser } from '../../services/BookingsService';
import { getAllCoworkingSpaces } from '../../services/CoworkingSpacesService';



const UserMyBookingsPage = () => {
    const [bookings, setBookings] = useState([]);
    const [error, setError] = useState(null);

    const userData = localStorage.getItem('user');
    let user = null;
    if (userData) {
      user = JSON.parse(userData);
    }

    useEffect(() => {
        const fetchBookings = async () => {
            try {
                const bookings = await getBookingsByUser(user.userId);
                setBookings(bookings);
                if (bookings.length === 0) {
                    setError("No bookings found.");
                } else {
                    setError(null);
                }
            } catch (error) {
                console.error('Error fetching bookings:', error);
                setError('Failed to fetch bookings');
            }
        };

        fetchBookings();
    }, []);

    return (
        <main className="main py-5">
            <div className="container">
                <div className="bookings-list">
                    {error ? (
                        <p>{error}</p>
                    ) : (
                        bookings.length > 0 ? (
                            bookings.map((booking) => (
                                <div key={booking.bookingId} className="booking">
                                    <h3><FormattedMessage id="userMyBookings.Code" defaultMessage="Code" /> {booking.bookingCode}</h3>
                                    <p>
                                        <strong><FormattedMessage id="userMyBookings.Start" defaultMessage="Start:" /></strong> {new Date(booking.startDateTime).toLocaleString()}
                                    </p>
                                    <p>
                                        <strong><FormattedMessage id="userMyBookings.End" defaultMessage="End:" /></strong> {new Date(booking.endDateTime).toLocaleString()}
                                    </p>
                                    <p>
                                        <strong><FormattedMessage id="userMyBookings.Workplacenumber" defaultMessage="Workplace number:" /></strong> {booking.workplaceId}
                                    </p>
                                    <p>
                                        <strong><FormattedMessage id="userMyBookings.Roomnumber" defaultMessage="Room number:" /></strong> {booking.roomId}
                                    </p>
                                    <p>
                                        <strong><FormattedMessage id="userMyBookings.Coworking" defaultMessage="Coworking:" /></strong> {booking.coworkingSpace.name} {booking.coworkingSpace.address} {booking.coworkingSpace.city}
                                    </p>
                                </div>
                            ))
                        ) : (
                            <p><FormattedMessage id="userMyBookings.Nobookingsfound" defaultMessage="No bookings found." /></p>
                        )
                    )}
                </div>
            </div>
        </main>
    );
};

export default UserMyBookingsPage;