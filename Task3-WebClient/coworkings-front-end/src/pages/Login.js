// Login.js
import React, { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { FormattedMessage } from 'react-intl';
import '../styles/login.css';
import { AuthContext } from '../contexts/AuthContext';
import { login as loginUser } from '../services/AuthService';
import LoginCredentials from '../models/LoginCredentials';
import { getUserByEmail, getManagerByEmail } from '../services/UserService';
import Role from '../enums/Role';

const Login = () => {
    const [credentials, setCredentials] = useState(new LoginCredentials('', '', true));
    const [selectedRole, setSelectedRole] = useState(Role.User);
    const { login } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setCredentials({ ...credentials, [e.target.name]: e.target.value });
    };

    const handleRoleChange = (e) => {
        setSelectedRole(parseInt(e.target.value));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const userData = await loginUser(credentials);
            if (userData) {
                login(userData);
                localStorage.clear();
                const userWithRole = { ...userData, role: selectedRole };
                localStorage.setItem('user', JSON.stringify(userWithRole));
                
                let id;
                if (selectedRole === Role.User) {
                    const user = await getUserByEmail(credentials.email);
                    id = user?.id;
                    localStorage.setItem('userId', id);
                    navigate('/user');
                } else if (selectedRole === Role.Admin) {
                    const manager = await getManagerByEmail(credentials.email);
                    id = manager?.managerId;
                    localStorage.setItem('managerId', id);
                    navigate('/admin');
                }

                window.location.reload();
            } else {
                console.error('Invalid user data:', userData);
            }
        } catch (error) {
            console.error('Login failed:', error);
        }
    };

    return (
        <main className="main py-5">
            <div className="container">
                <div className="row justify-content-center">
                    <div className="col-md-6">
                        <h2>
                            <FormattedMessage id="login.title" defaultMessage="Login" />
                        </h2>
                        <form onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label htmlFor="login" className="form-label">
                                    <FormattedMessage id="login.title" defaultMessage="Login" />
                                </label>
                                <input type="email" className="form-control" id="login" name="email" value={credentials.email} onChange={handleChange} required />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="password" className="form-label">
                                    <FormattedMessage id="login.password" defaultMessage="Password" />
                                </label>
                                <input type="password" className="form-control" id="password" name="password" value={credentials.password} onChange={handleChange} required />
                            </div>

                            <div className="mb-3">
                                <label htmlFor="role" className="form-label">
                                    <FormattedMessage id="login.role" defaultMessage="Role" />
                                </label>
                                <select className="form-select" id="role" value={selectedRole} onChange={handleRoleChange} required>
                                    <option value={Role.User}>
                                        <FormattedMessage id="login.role.user" defaultMessage="User" />
                                    </option>
                                    <option value={Role.Admin}>
                                        <FormattedMessage id="login.role.admin" defaultMessage="Manager" />
                                    </option>
                                </select>
                            </div>

                            <button type="submit" className="btn btn-primary">
                                <FormattedMessage id="login.submit" defaultMessage="Login" />
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </main>
    );
};

export default Login;