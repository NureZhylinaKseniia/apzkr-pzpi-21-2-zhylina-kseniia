import React, { useEffect, useState } from 'react';
import { getCoworkingSpaceByManagerId } from '../../services/CoworkingSpacesService';
import { getAllRooms, createRoom } from '../../services/RoomsService';
import { getAllWorkplacesByRoomId, createWorkplace } from '../../services/WorkplacesService';
import '../../styles/room.css';
import { FormattedMessage } from 'react-intl';

const AdminRooms = () => {
  const [coworkingSpace, setCoworkingSpace] = useState(null);
  const [rooms, setRooms] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [newRoomData, setNewRoomData] = useState({ roomName: '', description: '' });
  const [newWorkplaceData, setNewWorkplaceData] = useState({ roomId: '', hourlyRate: '' });

  const managerIdData = localStorage.getItem('managerId');
  let managerId = null;
  if (managerIdData) {
    managerId = JSON.parse(managerIdData);
  }

  useEffect(() => {
    const fetchCoworkingSpace = async () => {
      try {
        const coworkingSpaceData = await getCoworkingSpaceByManagerId(managerIdData);
        if (coworkingSpaceData) {
          setCoworkingSpace(coworkingSpaceData);
        } else {
          setCoworkingSpace(null);
        }
      } catch (error) {
        console.error('Error fetching coworking space:', error);
        setError('Failed to fetch coworking space');
      } finally {
        setLoading(false);
      }
    };

    if (managerIdData) {
      fetchCoworkingSpace();
    } else {
      setLoading(false);
    }
  }, [managerIdData]);

  useEffect(() => {
    const fetchRooms = async () => {
      if (coworkingSpace) {
        try {
          const roomsData = await getAllRooms();
          const filteredRooms = roomsData.filter(room => room.coworkingSpace.coworkingSpaceId == coworkingSpace[0].coworkingSpaceId);
          const roomsWithWorkplaces = await Promise.all(
            filteredRooms.map(async (room) => {
              const workplacesResponse = await getAllWorkplacesByRoomId(room.roomId);
              return {
                ...room,
                workplaces: workplacesResponse,
              };
            })
          );

          setRooms(roomsWithWorkplaces);
        } catch (error) {
          console.error('Error fetching rooms:', error);
          setRooms([]);
        }
      }
    };

    fetchRooms();
  }, [coworkingSpace]);

  const handleRoomInputChange = (event) => {
    const { name, value } = event.target;
    setNewRoomData({
      ...newRoomData,
      [name]: value,
    });
  };

  const handleWorkplaceInputChange = (event) => {
    const { name, value } = event.target;
    setNewWorkplaceData({
      ...newWorkplaceData,
      [name]: value,
    });
  };

  const handleRoomSubmit = async (event) => {
    event.preventDefault();
    try {
      const newRoom = {
        roomName: newRoomData.roomName,
        description: newRoomData.description,
        coworkingSpaceId: coworkingSpace[0].coworkingSpaceId,
      };
      await createRoom(newRoom);
      // Refresh the rooms data after adding a new room
      const roomsData = await getAllRooms();
      const filteredRooms = roomsData.filter(room => room.coworkingSpace.coworkingSpaceId == coworkingSpace[0].coworkingSpaceId);
      const roomsWithWorkplaces = await Promise.all(
        filteredRooms.map(async (room) => {
          const workplacesResponse = await getAllWorkplacesByRoomId(room.roomId);
          return {
            ...room,
            workplaces: workplacesResponse,
          };
        })
      );
      setRooms(roomsWithWorkplaces);
    } catch (error) {
      console.error('Error creating room:', error);
      setError('Failed to create room');
    }
  };

  const handleWorkplaceSubmit = async (event) => {
    event.preventDefault();
    try {
      const newWorkplace = {
        roomId: parseInt(newWorkplaceData.roomId, 10),
        hourlyRate: parseFloat(newWorkplaceData.hourlyRate),
      };
      await createWorkplace(newWorkplace);
      // Refresh the rooms data after adding a new workplace
      const roomsData = await getAllRooms();
      const filteredRooms = roomsData.filter(room => room.coworkingSpace.coworkingSpaceId == coworkingSpace[0].coworkingSpaceId);
      const roomsWithWorkplaces = await Promise.all(
        filteredRooms.map(async (room) => {
          const workplacesResponse = await getAllWorkplacesByRoomId(room.roomId);
          return {
            ...room,
            workplaces: workplacesResponse,
          };
        })
      );
      setRooms(roomsWithWorkplaces);
    } catch (error) {
      console.error('Error creating workplace:', error);
      setError('Failed to create workplace');
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!coworkingSpace) {
    return <div>You are not assigned to any coworking space.</div>;
  }

  return (
    <main className="main py-5">
      <div className="container">
        <div className="rooms">
          <h1>{coworkingSpace[0].name}</h1>
          <form onSubmit={handleRoomSubmit}>
            <h2><FormattedMessage id="adminRooms.addNewRoom" defaultMessage="Add New Room" /></h2>
            <div>
              <label>
                <FormattedMessage id="adminRooms.roomName" defaultMessage="Room Name:" />
                <input
                  type="text"
                  name="roomName"
                  value={newRoomData.roomName}
                  onChange={handleRoomInputChange}
                  required
                />
              </label>
            </div>
            <div>
              <label>
                <FormattedMessage id="adminRooms.description" defaultMessage="Description:" />
                <input
                  type="text"
                  name="description"
                  value={newRoomData.description}
                  onChange={handleRoomInputChange}
                  required
                />
              </label>
            </div>
            <button type="submit"><FormattedMessage id="adminRooms.addRoom" defaultMessage="Add Room" /></button>
          </form>
          <form onSubmit={handleWorkplaceSubmit}>
            <h2><FormattedMessage id="adminRooms.addNewWorkplace" defaultMessage="Add New Workplace" /></h2>
            <div>
              <label>
                <FormattedMessage id="adminRooms.roomID:" defaultMessage="Room ID:" />
                <input
                  type="number"
                  name="roomId"
                  value={newWorkplaceData.roomId}
                  onChange={handleWorkplaceInputChange}
                  required
                />
              </label>
            </div>
            <div>
              <label>
              <FormattedMessage id="adminRooms.HourlyRate" defaultMessage="Hourly Rate:" />
                <input
                  type="number"
                  name="hourlyRate"
                  value={newWorkplaceData.hourlyRate}
                  onChange={handleWorkplaceInputChange}
                  required
                />
              </label>
            </div>
            <button type="submit"><FormattedMessage id="adminRooms.AddWorkplace" defaultMessage="Add Workplace" /></button>
          </form>
          <div>
            {rooms.length > 0 ? (
              rooms.map((room) => (
                <div key={room.roomId} className="room">
                  <h2><FormattedMessage id="adminRooms.Room" defaultMessage="Room" /> {room.roomId}</h2>
                  <ul>
                    {room.workplaces.map((workplace) => (
                      <li key={workplace.workplaceId}><FormattedMessage id="adminRooms.Workplace" defaultMessage="Workplace" /> {workplace.workplaceId}</li>
                    ))}
                  </ul>
                </div>
              ))
            ) : (
              <p><FormattedMessage id="adminRooms.NoRoomsAvailable" defaultMessage="No rooms available" /></p>
            )}
          </div>
        </div>
      </div>
    </main>
  );
};

export default AdminRooms;
