// src/pages/admin/AdminAccountPage.js
import React, { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { FormattedMessage } from 'react-intl';
import '../../styles/accountPage.css';
import { AuthContext } from '../../contexts/AuthContext';

const AdminAccountPage = () => {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            await logout();
            localStorage.clear();
            navigate('/');
            window.location.reload();
        } catch (error) {
            console.error('Logout failed:', error);
        }
    };
    return (
        <main className="main py-5">
            <div className="container">
                <h2>
                    <FormattedMessage
                        id="adminAccountPage.title"
                        defaultMessage="Admin Account"
                    />
                </h2>
                <div className="account-info">
                    {user && (
                        <>
                        
                            <p>
                                <strong>
                                    <FormattedMessage
                                        id="adminAccountPage.fullname"
                                        defaultMessage="Fullname:"
                                    />
                                </strong>{' '}
                                {user.fullname}
                            </p>
                            <p>
                                <strong>
                                    <FormattedMessage
                                        id="adminAccountPage.email"
                                        defaultMessage="Email:"
                                    />
                                </strong>{' '}
                                {user.email}
                            </p>
                        </>
                    )}
                    <button className="btn btn-primary" onClick={handleLogout}>
                        <FormattedMessage id="accountPage.logout" defaultMessage="Logout"/>
                    </button>
                </div>
            </div>
        </main>
    );
};

export default AdminAccountPage;