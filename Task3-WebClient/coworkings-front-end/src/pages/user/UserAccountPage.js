import React, { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FormattedMessage } from 'react-intl';
import '../../styles/accountPage.css';
import { AuthContext } from '../../contexts/AuthContext';
import { updateUser } from '../../services/UserService';


const UserAccountPage = () => {
    const { setUser, logout } = useContext(AuthContext);
    const [editingUser, setEditingUser] = useState(null);
    const navigate = useNavigate();
    const [errors, setErrors] = useState({});

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

    const handleEditUser = () => {
        setEditingUser({ ...user });
    };

    const userData = localStorage.getItem('user');
    let user = null;
    if (userData) {
      user = JSON.parse(userData);
    }

    const handleUpdateUser = async () => {
        // Validate Fullname
        if (!/^[a-zA-Z\s]*$/.test(editingUser.fullname)) {
            setErrors({ fullname: "Full name should contain only letters." });
            return;
        }

        try {
            await updateUser(user.userId, editingUser);

            const updatedUser = {
                ...user,
                password: editingUser.password,
                email: editingUser.email,
                fullname: editingUser.fullname,
            };

            setUser(updatedUser);
            localStorage.setItem('user', JSON.stringify(updatedUser));
            setEditingUser(null);
            setErrors({});
        } catch (error) {
            console.error('Error updating user:', error.response ? error.response.data : error.message);
            if (error.response && error.response.data && error.response.data.errors) {
                setErrors(error.response.data.errors);
            }
        }
    };

    const handleCancelEdit = () => {
        setEditingUser(null);
        setErrors({});
    };

    return (
        <main className="main py-5">
            <div className="container">
                <h2 className="title">
                    <FormattedMessage
                        id="userAccountPage.title"
                        defaultMessage="User Account"
                    />
                </h2>
                <div className="account-info">
                    {user && (
                        <>
                            <p>
                                <strong>
                                    <FormattedMessage
                                        id="userAccountPage.email"
                                        defaultMessage="Email:"
                                    />
                                </strong>{' '}
                                {editingUser ? (
                                    <input
                                        type="email"
                                        value={editingUser.email}
                                        onChange={(e) =>
                                            setEditingUser({ ...editingUser, email: e.target.value })
                                        }
                                        className="input-field"
                                    />
                                ) : (
                                    user.email
                                )}
                            </p>
                            <p>
                                <strong>
                                    <FormattedMessage
                                        id="userAccountPage.fullname"
                                        defaultMessage="Fullname:"
                                    />
                                </strong>{' '}
                                {editingUser ? (
                                    <input
                                        type="text"
                                        value={editingUser.fullname}
                                        onChange={(e) =>
                                            setEditingUser({ ...editingUser, fullname: e.target.value })
                                        }
                                        className="input-field"
                                    />
                                ) : (
                                    user.fullname
                                )}
                                {errors.fullname && <div className="error">{errors.fullname}</div>}
                            </p>
                        </>
                    )}
                    <div className="btn-group">
                        {editingUser ? (
                            <>
                                <button className="btn btn-primary" onClick={handleUpdateUser}>
                                    <FormattedMessage id="userAccountPage.saveChanges" defaultMessage="Save Changes" />
                                </button>
                                <button className="btn btn-secondary" onClick={handleCancelEdit}>
                                    <FormattedMessage id="userAccountPage.cancel" defaultMessage="Cancel" />
                                </button>
                            </>
                        ) : (
                            <>
                                <button className="btn btn-primary" onClick={handleEditUser}>
                                    <FormattedMessage id="userAccountPage.editAccount" defaultMessage="Edit Account" />
                                </button>
                                <button className="btn btn-danger" onClick={handleLogout}>
                                    <FormattedMessage id="accountPage.logout" defaultMessage="Logout" />
                                </button>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </main>
    );
};

export default UserAccountPage;
