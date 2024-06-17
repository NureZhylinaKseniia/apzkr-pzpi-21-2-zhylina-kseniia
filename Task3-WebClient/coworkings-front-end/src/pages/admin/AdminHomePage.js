import React from 'react';
import { FormattedMessage } from 'react-intl';

const AdminHomePage = () => {
    return (
        <main className="main py-5">
            <div className="container">
                <h2>
                    <FormattedMessage
                        id="adminHomePage.welcome"
                        defaultMessage="Administration page!"
                    />
                </h2>
            </div>
        </main>
    );
};

export default AdminHomePage;