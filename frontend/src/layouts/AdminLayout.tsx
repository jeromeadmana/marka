import { useState } from 'react';
import { Link, Outlet, useNavigate, useLocation } from 'react-router-dom';

interface User {
  email: string;
  role: string;
}

export default function AdminLayout() {
  const [sidebarOpen, setSidebarOpen] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  const userStr = localStorage.getItem('user');
  const user: User | null = userStr ? JSON.parse(userStr) : null;
  const isSuperAdmin = user?.role === 'SuperAdmin';

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    navigate('/login');
  };

  const isActive = (path: string) => location.pathname.startsWith(path);

  return (
    <div className="flex h-screen bg-gray-100">
      {/* Sidebar */}
      <aside className={`bg-gray-900 text-white transition-all duration-300 ${sidebarOpen ? 'w-64' : 'w-20'}`}>
        <div className="flex items-center justify-between p-4 border-b border-gray-700">
          {sidebarOpen && <h1 className="text-xl font-bold">Marka Admin</h1>}
          <button
            onClick={() => setSidebarOpen(!sidebarOpen)}
            className="p-2 rounded hover:bg-gray-700"
          >
            {sidebarOpen ? '←' : '→'}
          </button>
        </div>

        <nav className="mt-6">
          {/* Dashboard */}
          <Link
            to="/admin"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin') && location.pathname === '/admin' ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">📊</span>
            {sidebarOpen && <span className="ml-3">Dashboard</span>}
          </Link>

          {/* Marka Contexts */}
          <Link
            to="/admin/contexts"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/contexts') ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">📍</span>
            {sidebarOpen && <span className="ml-3">Marka Types</span>}
          </Link>

          {/* Attributes */}
          <Link
            to="/admin/attributes"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/attributes') ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">📝</span>
            {sidebarOpen && <span className="ml-3">Attributes</span>}
          </Link>

          {/* Attribute Sets */}
          <Link
            to="/admin/attribute-sets"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/attribute-sets') ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">📦</span>
            {sidebarOpen && <span className="ml-3">Attribute Sets</span>}
          </Link>

          {/* Roles */}
          <Link
            to="/admin/roles"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/roles') ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">👥</span>
            {sidebarOpen && <span className="ml-3">Roles</span>}
          </Link>

          {/* Users */}
          <Link
            to="/admin/users"
            className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/users') ? 'bg-gray-700' : ''}`}
          >
            <span className="text-xl">👤</span>
            {sidebarOpen && <span className="ml-3">Users</span>}
          </Link>

          {/* Customers (SuperAdmin only) */}
          {isSuperAdmin && (
            <Link
              to="/admin/customers"
              className={`flex items-center px-4 py-3 hover:bg-gray-700 ${isActive('/admin/customers') ? 'bg-gray-700' : ''}`}
            >
              <span className="text-xl">🏢</span>
              {sidebarOpen && <span className="ml-3">Customers</span>}
            </Link>
          )}

          {/* Divider */}
          <div className="my-4 border-t border-gray-700"></div>

          {/* Map View */}
          <Link
            to="/"
            className="flex items-center px-4 py-3 hover:bg-gray-700"
          >
            <span className="text-xl">🗺️</span>
            {sidebarOpen && <span className="ml-3">Map View</span>}
          </Link>
        </nav>
      </aside>

      {/* Main Content */}
      <div className="flex-1 flex flex-col overflow-hidden">
        {/* Top Bar */}
        <header className="bg-white shadow-sm">
          <div className="flex items-center justify-between px-6 py-4">
            <h2 className="text-2xl font-semibold text-gray-800">
              Admin Dashboard
            </h2>
            <div className="flex items-center gap-4">
              <span className="text-sm text-gray-600">{user?.email}</span>
              <span className="px-2 py-1 text-xs font-semibold rounded bg-blue-100 text-blue-800">
                {user?.role}
              </span>
              <button
                onClick={handleLogout}
                className="px-4 py-2 text-sm font-medium text-white bg-red-600 rounded hover:bg-red-700"
              >
                Logout
              </button>
            </div>
          </div>
        </header>

        {/* Page Content */}
        <main className="flex-1 overflow-y-auto bg-gray-100 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
