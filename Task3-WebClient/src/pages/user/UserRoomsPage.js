// src/Rooms.js
import React, { useEffect, useState } from 'react';
import Room from './Room';
import BookingWindow from './BookingWindow';
import { getAllRooms } from '../../services/RoomsService';
import { getAllWorkplacesByRoomId } from '../../services/WorkplacesService';
import { getAllCoworkingSpaces } from '../../services/CoworkingSpacesService';
import { FormattedMessage } from 'react-intl';

const Rooms = () => {
  const [coworkingSpaces, setCoworkingSpaces] = useState([]);
  const [selectedCoworkingSpace, setSelectedCoworkingSpace] = useState(null);
  const [rooms, setRooms] = useState([]);
  const [selectedWorkplace, setSelectedWorkplace] = useState(null);

  useEffect(() => {
    const fetchCoworkingSpaces = async () => {
      try {
        const coworkingSpacesData = await getAllCoworkingSpaces();
        setCoworkingSpaces(coworkingSpacesData);
        console.log('Coworking spaces:', coworkingSpacesData);
      } catch (error) {
        console.error('Error fetching coworking spaces:', error);
      }
    };

    fetchCoworkingSpaces();
  }, []);

  useEffect(() => {
    const fetchRooms = async () => {
      if (selectedCoworkingSpace) {
        try {
          const roomsData = await getAllRooms();
          console.log('All rooms:', roomsData);

          const filteredRooms = roomsData.filter(room => {
            const match = room.coworkingSpace && room.coworkingSpace.coworkingSpaceId === selectedCoworkingSpace;
            console.log(`Room ID ${room.roomId} match:`, match);
            return match;
          });
          console.log('Filtered rooms:', filteredRooms);

          const roomsWithWorkplaces = await Promise.all(
            filteredRooms.map(async (room) => {
              const workplacesResponse = await getAllWorkplacesByRoomId(room.roomId);
              return {
                ...room,
                workplaces: workplacesResponse,
              };
            })
          );

          console.log('Rooms with workplaces:', roomsWithWorkplaces);
          setRooms(roomsWithWorkplaces);
        } catch (error) {
          console.error('Error fetching rooms:', error);
          setRooms([]);
        }
      }
    };

    fetchRooms();
  }, [selectedCoworkingSpace]);

  return (
    <main className="main py-5">
      <div className="container">
        <div className="rooms">
          <h1><FormattedMessage id="userRooms.Coworkings" defaultMessage="Coworkings" /></h1>
          <select
            value={selectedCoworkingSpace || ''}
            onChange={(e) => setSelectedCoworkingSpace(parseInt(e.target.value))}
          >
            <option value="" disabled><FormattedMessage id="userRooms.Selectacoworkingspace" defaultMessage="Select a coworking space" /></option>
            {coworkingSpaces.map((space) => (
              <option key={space.coworkingSpaceId} value={space.coworkingSpaceId}>
                {space.name}, {space.address}, {space.city}
              </option>
            ))}
          </select>
          <div>
          {rooms.length > 0 ? (
            rooms.map((room) => (
              <Room
                key={room.roomId}
                roomNumber={room.roomId}
                workplaces={room.workplaces}
                setSelectedWorkplace={setSelectedWorkplace}
                selectedWorkplace={selectedWorkplace}
              />
            ))
          ) : (
            <p><FormattedMessage id="userRooms.Noroomsavailable" defaultMessage="No rooms available" /></p>
          )}
        </div>
          {selectedWorkplace && (
            <BookingWindow workplaceId={selectedWorkplace} onClose={() => setSelectedWorkplace(null)} />
          )}
        </div>
      </div>
    </main>
  );
};

export default Rooms;