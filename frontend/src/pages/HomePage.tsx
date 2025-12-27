import { useState, useEffect } from 'react';
import { markasApi } from '../services/api';
import type { Marka } from '../types/marka';
import MapView from '../components/MapView';
import MarkasList from '../components/MarkasList';
import EditMarkaModal from '../components/EditMarkaModal';

export default function HomePage() {
  const [markas, setMarkas] = useState<Marka[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedMarka, setSelectedMarka] = useState<Marka | null>(null);

  useEffect(() => {
    loadMarkas();
  }, []);

  const loadMarkas = async () => {
    try {
      setLoading(true);
      const data = await markasApi.getAll();
      setMarkas(data);
      setError(null);
    } catch (err) {
      setError('Failed to load markas');
      console.error('Error loading markas:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex h-screen">
      {/* Sidebar */}
      <div className="w-96 bg-white border-r border-gray-200 flex flex-col">
        <div className="p-4 border-b border-gray-200">
          <h2 className="text-xl font-bold text-gray-800">Markas</h2>
          <p className="text-sm text-gray-600">
            {markas.length} {markas.length === 1 ? 'location' : 'locations'}
          </p>
        </div>

        <div className="flex-1 overflow-y-auto">
          {loading && (
            <div className="p-4 text-center text-gray-600">
              Loading markas...
            </div>
          )}

          {error && (
            <div className="p-4 text-center text-red-600">
              {error}
            </div>
          )}

          {!loading && !error && (
            <MarkasList
              markas={markas}
              onMarkaClick={(marka) => setSelectedMarka(marka)}
            />
          )}
        </div>
      </div>

      {/* Map */}
      <div className="flex-1">
        <MapView markas={markas} onMarkaCreated={loadMarkas} />
      </div>

      {/* Edit Modal */}
      {selectedMarka && (
        <EditMarkaModal
          marka={selectedMarka}
          onClose={() => setSelectedMarka(null)}
          onSuccess={() => {
            setSelectedMarka(null);
            loadMarkas();
          }}
          onDelete={() => {
            setSelectedMarka(null);
            loadMarkas();
          }}
        />
      )}
    </div>
  );
}
