import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Header from './components/Header';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import AdminLayout from './layouts/AdminLayout';
import AdminDashboard from './pages/admin/AdminDashboard';
import MarkaContextsPage from './pages/admin/MarkaContextsPage';
import AttributesPage from './pages/admin/AttributesPage';
import { authService } from './services/authService';

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  return authService.isAuthenticated() ? <>{children}</> : <Navigate to="/login" />;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        {/* Admin Routes */}
        <Route
          path="/admin"
          element={
            <ProtectedRoute>
              <AdminLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<AdminDashboard />} />
          <Route path="contexts" element={<MarkaContextsPage />} />
          <Route path="attributes" element={<AttributesPage />} />
          <Route path="attribute-sets" element={<div className="p-6 bg-white rounded-lg shadow"><h1 className="text-2xl font-bold">Attribute Sets (Coming Soon)</h1></div>} />
          <Route path="roles" element={<div className="p-6 bg-white rounded-lg shadow"><h1 className="text-2xl font-bold">Roles (Coming Soon)</h1></div>} />
          <Route path="users" element={<div className="p-6 bg-white rounded-lg shadow"><h1 className="text-2xl font-bold">Users (Coming Soon)</h1></div>} />
          <Route path="customers" element={<div className="p-6 bg-white rounded-lg shadow"><h1 className="text-2xl font-bold">Customers (Coming Soon)</h1></div>} />
        </Route>

        {/* Map View */}
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <div className="flex flex-col h-screen">
                <Header />
                <div className="flex-1 overflow-hidden">
                  <HomePage />
                </div>
              </div>
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
