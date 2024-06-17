// src/pages/user/UserHomePage.js

import React from 'react';
import { FormattedMessage } from 'react-intl';

const UserHomePage = () => {
    return (
        <main className="main py-5">
            <div className="container">
                <h2>
                    <FormattedMessage
                        id="userHomePage.welcome"
                        defaultMessage="User page!"
                    />
                </h2>
            </div>
        </main>
    );
};

export default UserHomePage;