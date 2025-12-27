import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';

export default function Header() {
  const navigate = useNavigate();
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const user = authService.getUser();

  useEffect(() => {
    const handleOnline = () => setIsOnline(true);
    const handleOffline = () => setIsOnline(false);

    window.addEventListener('online', handleOnline);
    window.addEventListener('offline', handleOffline);

    return () => {
      window.removeEventListener('online', handleOnline);
      window.removeEventListener('offline', handleOffline);
    };
  }, []);

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  return (
    <header className="bg-blue-600 text-white shadow-lg">
      <div className="container mx-auto px-4 py-3 flex items-center justify-between">
        <div className="flex items-center space-x-3">
          <h1 className="text-2xl font-bold">Marka</h1>
          <span className="text-sm text-blue-100">Location Management</span>
        </div>

        <div className="flex items-center space-x-4">
          {/* User Info */}
          {user && (
            <div className="text-sm">
              <span className="font-medium">{user.firstName} {user.lastName}</span>
              <span className="text-blue-200 ml-2">({user.role})</span>
            </div>
          )}

          {/* Online/Offline Indicator */}
          <div className="flex items-center space-x-2">
            <div
              className={`w-2 h-2 rounded-full ${
                isOnline ? 'bg-green-400' : 'bg-red-400'
              }`}
            />
            <span className="text-sm">
              {isOnline ? 'Online' : 'Offline'}
            </span>
          </div>

          {/* Logout Button */}
          <button
            onClick={handleLogout}
            className="px-3 py-1 bg-blue-700 hover:bg-blue-800 rounded text-sm font-medium transition-colors"
          >
            Logout
          </button>
        </div>
      </div>
    </header>
  );
}
