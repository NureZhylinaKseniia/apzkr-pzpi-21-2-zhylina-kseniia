import React, { useContext, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { AuthProvider } from './contexts/AuthContext';
import { FormattedMessage } from 'react-intl';
import { AuthContext } from './contexts/AuthContext';
import { IntlProvider } from 'react-intl';
import { LanguageContext } from './contexts/LanguageContext';
import Role from './enums/Role';
import en from './locales/en.json';
import uk from './locales/ua.json';

import Header from './components/Header';
import Home from './pages/Home';
import About from './pages/About';
import Contacts from './pages/Contacts';
import Registration from './pages/Registration';
import Login from './pages/Login';

import UserHomePage from './pages/user/UserHomePage';
import UserAccountPage from './pages/user/UserAccountPage';
import UserRoomsPage from './pages/user/UserRoomsPage';
import UserMyBookingsPage from './pages/user/UserMyBookingsPage';

import AdminHomePage from './pages/admin/AdminHomePage';
import AdminAccountPage from './pages/admin/AdminAccountPage';
import AdminAllBookingsPage from './pages/admin/AdminAllBookingsPage';
import AdminAllUsersPage from './pages/admin/AdminAllUsersPage';
import AdminRoomsPage from './pages/admin/AdminRoomsPage';


const messages = {
    en,
    uk,
};

function App() {
    const { user, setUser } = useContext(AuthContext);
    const { language } = useContext(LanguageContext);


    useEffect(() => {
        const storedUser = localStorage.getItem('user');
        if (storedUser) {
            setUser(JSON.parse(storedUser));
        }
    }, []);

    const renderHeader = () => {

        switch (user?.role) {
            case Role.User:
                return (
                    <Header
                        title={<FormattedMessage id="userHeader.title" defaultMessage="User" />}
                        navigationItems={[
                            { id: 'home', path: '/user', labelId: 'userHeader.home', defaultLabel: 'Home' },
                            { id: 'rooms', path: '/user/rooms', labelId: 'userHeader.rooms', defaultLabel: 'Rooms' },
                            { id: 'myBookings', path: '/user/myBookings', labelId: 'userHeader.myBookings', defaultLabel: 'My Bookings' },
                        ]}
                        accountButton={{ path: '/user/account', labelId: 'userHeader.account', defaultLabel: 'Account' }}
                    />
                );
            case Role.Admin:
                return (
                    <Header
                        title={<FormattedMessage id="adminHeader.title" defaultMessage="Admin" />}
                        navigationItems={[
                            { id: 'home', path: '/admin', labelId: 'adminHeader.home', defaultLabel: 'Home' },
                            { id: 'allBookings', path: '/admin/allBookings', labelId: 'adminHeader.allBookings', defaultLabel: 'All bookings' },
                            { id: 'allUsers', path: '/admin/allUsers', labelId: 'adminHeader.allUsers', defaultLabel: 'All users' },
                            { id: 'rooms', path: '/admin/rooms', labelId: 'adminHeader.rooms', defaultLabel: 'Rooms' },
                        ]}
                        accountButton={{ path: '/admin/account', labelId: 'adminHeader.account', defaultLabel: 'Account' }}
                    />
                );
            default:
                return (
                    <Header
                    title={<FormattedMessage id="app.title" defaultMessage="Coworkings" />}
                    navigationItems={[
                        { id: 'home', path: '/', labelId: 'app.home', defaultLabel: 'Home' },
                        { id: 'about', path: '/about', labelId: 'app.about', defaultLabel: 'About' },
                        { id: 'contacts', path: '/contacts', labelId: 'app.contacts', defaultLabel: 'Contacts' },
                    ]}
                    authButtons={[
                        { id: 'login', path: '/login', labelId: 'app.login', defaultLabel: 'Login' },
                        { id: 'register', path: '/registration', labelId: 'app.register', defaultLabel: 'Register' },
                    ]}
                    />
                );
        }
    };

    return (
        <AuthProvider>
            <IntlProvider messages={messages[language]} locale={language} defaultLocale="en">
                <Router>
                    <div className="app">
                        {renderHeader()}
                        <Routes>
                            <Route exact path="/" element={<Home />} />
                            <Route path="/about" element={<About />} />
                            <Route path="/contacts" element={<Contacts />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/registration" element={<Registration />} />

                            <Route path="/user" element={user && user.role === Role.User ? <UserHomePage /> : <Navigate to="/" />} />
                            <Route path="/user/account" element={<UserAccountPage />} />
                            <Route path="/user/rooms" element={<UserRoomsPage />} />
                            <Route path="/user/myBookings" element={<UserMyBookingsPage />} />

                            <Route path="/admin" element={user && user.role === Role.Admin ? <AdminHomePage /> : <Navigate to="/" />} />
                            <Route path="/admin/account" element={<AdminAccountPage />} />
                            <Route path="/admin/allBookings" element={<AdminAllBookingsPage />} />
                            <Route path="/admin/allUsers" element={<AdminAllUsersPage />} />
                            <Route path="/admin/rooms" element={<AdminRoomsPage />} />
                        </Routes>
                    </div>
                </Router>
            </IntlProvider>
        </AuthProvider>
    );
}

export default App;