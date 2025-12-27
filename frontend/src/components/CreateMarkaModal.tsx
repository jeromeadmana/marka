import { useState } from 'react';
import { markasApi } from '../services/api';
import { authService } from '../services/authService';
import type { CreateMarkaDto } from '../types/marka';

interface CreateMarkaModalProps {
  latitude: number;
  longitude: number;
  onClose: () => void;
  onSuccess: () => void;
}

export default function CreateMarkaModal({
  latitude,
  longitude,
  onClose,
  onSuccess,
}: CreateMarkaModalProps) {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    address: '',
    category: 'General',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const user = authService.getUser();
      if (!user) {
        throw new Error('User not authenticated');
      }

      const createDto: CreateMarkaDto = {
        name: formData.name,
        description: formData.description || undefined,
        latitude,
        longitude,
        address: formData.address || undefined,
        category: formData.category,
        customerId: user.customerId,
        createdByUserId: user.userId,
      };

      await markasApi.create(createDto);
      onSuccess();
    } catch (err) {
      setError('Failed to create marka. Please try again.');
      console.error('Error creating marka:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-full max-w-md">
        <h2 className="text-xl font-bold mb-4">Create New Marka</h2>

        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Name *
            </label>
            <input
              type="text"
              required
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter marka name"
            />
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Description
            </label>
            <textarea
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              rows={3}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter description"
            />
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Address
            </label>
            <input
              type="text"
              value={formData.address}
              onChange={(e) =>
                setFormData({ ...formData, address: e.target.value })
              }
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Enter address"
            />
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Category
            </label>
            <select
              value={formData.category}
              onChange={(e) =>
                setFormData({ ...formData, category: e.target.value })
              }
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              <option value="General">General</option>
              <option value="Historical">Historical</option>
              <option value="Shopping">Shopping</option>
              <option value="Park">Park</option>
              <option value="Commercial">Commercial</option>
              <option value="Residential">Residential</option>
              <option value="Restaurant">Restaurant</option>
              <option value="Office">Office</option>
            </select>
          </div>

          <div className="mb-4 text-sm text-gray-600">
            <p>Location: {latitude.toFixed(6)}, {longitude.toFixed(6)}</p>
          </div>

          {error && (
            <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-md text-red-700 text-sm">
              {error}
            </div>
          )}

          <div className="flex gap-3">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50"
            >
              {loading ? 'Creating...' : 'Create Marka'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
