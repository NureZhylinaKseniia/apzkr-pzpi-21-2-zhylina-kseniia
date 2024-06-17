// src/Room.js
import React from 'react';
import '../../styles/room.css';
import { FormattedMessage } from 'react-intl';

const Room = ({ roomNumber, workplaces, setSelectedWorkplace, selectedWorkplace }) => {
  return (
    <div className="room-container">
      <h2><FormattedMessage id="userRooms.Room" defaultMessage="Room" /> {roomNumber}</h2>
      <div className="room">
        {workplaces.map((workplace) => (
          <button
            key={workplace.workplaceId}
            className={`workplace-button ${selectedWorkplace === workplace.workplaceId ? 'selected' : ''}`}
            onClick={() => setSelectedWorkplace(workplace.workplaceId)}
          >
            <FormattedMessage id="userRooms.Workplace" defaultMessage="Workplace" /> {workplace.workplaceId}
          </button>
        ))}
      </div>
    </div>
  );
};

export default Room;
