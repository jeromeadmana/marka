import type { Marka } from '../types/marka';

interface MarkasListProps {
  markas: Marka[];
  onMarkaClick: (marka: Marka) => void;
}

export default function MarkasList({ markas, onMarkaClick }: MarkasListProps) {
  if (markas.length === 0) {
    return (
      <div className="p-4 text-center text-gray-500">
        <p>No markas yet.</p>
        <p className="text-sm mt-2">Click on the map to add a location.</p>
      </div>
    );
  }

  return (
    <div className="divide-y divide-gray-200">
      {markas.map((marka) => (
        <div
          key={marka.id}
          onClick={() => onMarkaClick(marka)}
          className="p-4 hover:bg-gray-50 cursor-pointer transition-colors"
        >
          <h3 className="font-semibold text-gray-800">{marka.name}</h3>
          {marka.description && (
            <p className="text-sm text-gray-600 mt-1">{marka.description}</p>
          )}
          {marka.address && (
            <p className="text-xs text-gray-500 mt-2">{marka.address}</p>
          )}
          <div className="flex items-center justify-between mt-2">
            {marka.category && (
              <span className="inline-block px-2 py-1 text-xs font-medium bg-blue-100 text-blue-800 rounded">
                {marka.category}
              </span>
            )}
            <span className="text-xs text-gray-400">
              {new Date(marka.createdAt).toLocaleDateString()}
            </span>
          </div>
        </div>
      ))}
    </div>
  );
}
