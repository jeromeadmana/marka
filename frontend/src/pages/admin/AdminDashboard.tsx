export default function AdminDashboard() {
  const userStr = localStorage.getItem('user');
  const user = userStr ? JSON.parse(userStr) : null;

  return (
    <div>
      <h1 className="text-3xl font-bold text-gray-900 mb-6">
        Welcome to Admin Dashboard
      </h1>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {/* Quick Stats Cards */}
        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-blue-500 rounded-md p-3">
              <span className="text-2xl text-white">📍</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Marka Types</p>
              <p className="text-2xl font-semibold text-gray-900">-</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-green-500 rounded-md p-3">
              <span className="text-2xl text-white">📝</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Attributes</p>
              <p className="text-2xl font-semibold text-gray-900">-</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6">
          <div className="flex items-center">
            <div className="flex-shrink-0 bg-purple-500 rounded-md p-3">
              <span className="text-2xl text-white">👥</span>
            </div>
            <div className="ml-4">
              <p className="text-sm font-medium text-gray-600">Users</p>
              <p className="text-2xl font-semibold text-gray-900">-</p>
            </div>
          </div>
        </div>
      </div>

      <div className="mt-8 bg-white rounded-lg shadow p-6">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">
          Quick Actions
        </h2>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <a
            href="/admin/contexts"
            className="flex items-center p-4 border-2 border-gray-200 rounded-lg hover:border-blue-500 hover:bg-blue-50 transition-colors"
          >
            <span className="text-3xl mr-4">📍</span>
            <div>
              <h3 className="font-semibold text-gray-900">Manage Marka Types</h3>
              <p className="text-sm text-gray-600">Create and configure marka contexts</p>
            </div>
          </a>

          <a
            href="/admin/attributes"
            className="flex items-center p-4 border-2 border-gray-200 rounded-lg hover:border-green-500 hover:bg-green-50 transition-colors"
          >
            <span className="text-3xl mr-4">📝</span>
            <div>
              <h3 className="font-semibold text-gray-900">Manage Attributes</h3>
              <p className="text-sm text-gray-600">Define custom fields for markas</p>
            </div>
          </a>

          <a
            href="/admin/roles"
            className="flex items-center p-4 border-2 border-gray-200 rounded-lg hover:border-purple-500 hover:bg-purple-50 transition-colors"
          >
            <span className="text-3xl mr-4">👥</span>
            <div>
              <h3 className="font-semibold text-gray-900">Manage Roles</h3>
              <p className="text-sm text-gray-600">Configure user roles and permissions</p>
            </div>
          </a>

          <a
            href="/admin/users"
            className="flex items-center p-4 border-2 border-gray-200 rounded-lg hover:border-yellow-500 hover:bg-yellow-50 transition-colors"
          >
            <span className="text-3xl mr-4">👤</span>
            <div>
              <h3 className="font-semibold text-gray-900">Manage Users</h3>
              <p className="text-sm text-gray-600">Add and configure user accounts</p>
            </div>
          </a>
        </div>
      </div>

      <div className="mt-8 bg-blue-50 border border-blue-200 rounded-lg p-6">
        <h3 className="text-lg font-semibold text-blue-900 mb-2">
          Getting Started
        </h3>
        <p className="text-blue-800 mb-4">
          Welcome, {user?.email}! You're logged in as <strong>{user?.role}</strong>.
        </p>
        <ul className="list-disc list-inside text-blue-800 space-y-2">
          <li>Create marka types (e.g., "Fire Hydrant", "Street Sign")</li>
          <li>Define attributes for each type (e.g., "Flow Rate", "Condition")</li>
          <li>Set up user roles with specific permissions</li>
          <li>Invite team members to collaborate</li>
        </ul>
      </div>
    </div>
  );
}
