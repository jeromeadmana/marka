import { useState, useEffect } from 'react';
import api from '../../services/api';

interface Attribute {
  id: string;
  name: string;
  label: string;
  type: string;
  defaultValue: string | null;
  required: boolean;
  readOnly: boolean;
  isActive: boolean;
  customerName: string;
}

export default function AttributesPage() {
  const [attributes, setAttributes] = useState<Attribute[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingAttr, setEditingAttr] = useState<Attribute | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    label: '',
    type: 'text',
    defaultValue: '',
    required: false,
    readOnly: false
  });

  useEffect(() => {
    fetchAttributes();
  }, []);

  const fetchAttributes = async () => {
    try {
      const response = await api.get('/attributes');
      setAttributes(response.data);
    } catch (error) {
      console.error('Error fetching attributes:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingAttr) {
        await api.put(`/attributes/${editingAttr.id}`, formData);
      } else {
        await api.post('/attributes', formData);
      }
      setShowModal(false);
      setEditingAttr(null);
      setFormData({ name: '', label: '', type: 'text', defaultValue: '', required: false, readOnly: false });
      fetchAttributes();
    } catch (error) {
      console.error('Error saving attribute:', error);
      alert('Failed to save attribute');
    }
  };

  const handleEdit = (attr: Attribute) => {
    setEditingAttr(attr);
    setFormData({
      name: attr.name,
      label: attr.label,
      type: attr.type,
      defaultValue: attr.defaultValue || '',
      required: attr.required,
      readOnly: attr.readOnly
    });
    setShowModal(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this attribute?')) return;

    try {
      await api.delete(`/attributes/${id}`);
      fetchAttributes();
    } catch (error) {
      console.error('Error deleting attribute:', error);
      alert('Failed to delete attribute');
    }
  };

  if (loading) {
    return <div className="flex justify-center items-center h-64">Loading...</div>;
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Attributes</h1>
        <button
          onClick={() => {
            setEditingAttr(null);
            setFormData({ name: '', label: '', type: 'text', defaultValue: '', required: false, readOnly: false });
            setShowModal(true);
          }}
          className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          + New Attribute
        </button>
      </div>

      <div className="bg-white rounded-lg shadow overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Name</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Label</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Type</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Required</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
              <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-200">
            {attributes.length === 0 ? (
              <tr>
                <td colSpan={6} className="px-6 py-4 text-center text-gray-500">
                  No attributes found. Create one to get started!
                </td>
              </tr>
            ) : (
              attributes.map((attr) => (
                <tr key={attr.id}>
                  <td className="px-6 py-4 text-sm font-medium text-gray-900">{attr.name}</td>
                  <td className="px-6 py-4 text-sm text-gray-500">{attr.label}</td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    <span className="px-2 py-1 bg-gray-100 rounded text-xs">{attr.type}</span>
                  </td>
                  <td className="px-6 py-4 text-sm text-gray-500">
                    {attr.required ? '✓' : '-'}
                  </td>
                  <td className="px-6 py-4">
                    <span className={`px-2 py-1 text-xs font-semibold rounded ${
                      attr.isActive ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'
                    }`}>
                      {attr.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-right text-sm font-medium">
                    <button onClick={() => handleEdit(attr)} className="text-blue-600 hover:text-blue-900 mr-3">
                      Edit
                    </button>
                    <button onClick={() => handleDelete(attr.id)} className="text-red-600 hover:text-red-900">
                      Delete
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-lg p-6 w-full max-w-md">
            <h2 className="text-xl font-bold mb-4">{editingAttr ? 'Edit Attribute' : 'New Attribute'}</h2>
            <form onSubmit={handleSubmit}>
              <div className="mb-4">
                <label className="block text-sm font-medium mb-2">Name *</label>
                <input
                  type="text"
                  required
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>

              <div className="mb-4">
                <label className="block text-sm font-medium mb-2">Label *</label>
                <input
                  type="text"
                  required
                  value={formData.label}
                  onChange={(e) => setFormData({ ...formData, label: e.target.value })}
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>

              <div className="mb-4">
                <label className="block text-sm font-medium mb-2">Type *</label>
                <select
                  value={formData.type}
                  onChange={(e) => setFormData({ ...formData, type: e.target.value })}
                  className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="text">Text</option>
                  <option value="number">Number</option>
                  <option value="date">Date</option>
                  <option value="dropdown">Dropdown</option>
                  <option value="boolean">Boolean</option>
                </select>
              </div>

              <div className="mb-4 flex items-center gap-4">
                <label className="flex items-center">
                  <input
                    type="checkbox"
                    checked={formData.required}
                    onChange={(e) => setFormData({ ...formData, required: e.target.checked })}
                    className="mr-2"
                  />
                  Required
                </label>
                <label className="flex items-center">
                  <input
                    type="checkbox"
                    checked={formData.readOnly}
                    onChange={(e) => setFormData({ ...formData, readOnly: e.target.checked })}
                    className="mr-2"
                  />
                  Read Only
                </label>
              </div>

              <div className="flex justify-end gap-3">
                <button
                  type="button"
                  onClick={() => { setShowModal(false); setEditingAttr(null); }}
                  className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
                >
                  Cancel
                </button>
                <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">
                  {editingAttr ? 'Update' : 'Create'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
