import { useEffect, useRef, useState } from 'react';
import type { Marka } from '../types/marka';
import CreateMarkaModal from './CreateMarkaModal';

interface MapViewProps {
  markas: Marka[];
  onMarkaCreated: () => void;
}

export default function MapView({ markas, onMarkaCreated }: MapViewProps) {
  const mapRef = useRef<HTMLDivElement>(null);
  const mapInstanceRef = useRef<google.maps.Map | null>(null);
  const markersRef = useRef<google.maps.Marker[]>([]);
  const [mapError, setMapError] = useState<string | null>(null);
  const [mapLoaded, setMapLoaded] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [clickLocation, setClickLocation] = useState<{ lat: number; lng: number } | null>(null);

  useEffect(() => {
    // Load Google Maps script
    const loadGoogleMaps = () => {
      if (window.google?.maps) {
        initializeMap();
        return;
      }

      const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;
      if (!apiKey) {
        setMapError('Google Maps API key not configured');
        return;
      }

      const script = document.createElement('script');
      script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&libraries=places`;
      script.async = true;
      script.defer = true;
      script.onload = initializeMap;
      script.onerror = () => {
        setMapError('Failed to load Google Maps. Displaying offline mode.');
      };
      document.head.appendChild(script);
    };

    const initializeMap = () => {
      if (!mapRef.current || !window.google?.maps) return;

      try {
        // Default to Manila, Philippines
        const map = new google.maps.Map(mapRef.current, {
          center: { lat: 14.5995, lng: 120.9842 },
          zoom: 12,
          mapTypeControl: true,
          streetViewControl: false,
          fullscreenControl: true,
        });

        mapInstanceRef.current = map;

        // Add click listener to create markas
        map.addListener('click', (event: google.maps.MapMouseEvent) => {
          if (event.latLng) {
            setClickLocation({
              lat: event.latLng.lat(),
              lng: event.latLng.lng(),
            });
            setShowCreateModal(true);
          }
        });

        setMapLoaded(true);
        setMapError(null);
      } catch (error) {
        console.error('Error initializing map:', error);
        setMapError('Failed to initialize map. Displaying offline mode.');
      }
    };

    loadGoogleMaps();
  }, []);

  useEffect(() => {
    if (!mapInstanceRef.current || !mapLoaded) return;

    // Clear existing markers
    markersRef.current.forEach((marker) => marker.setMap(null));
    markersRef.current = [];

    // Add markers for each marka
    markas.forEach((marka) => {
      const marker = new google.maps.Marker({
        position: { lat: marka.latitude, lng: marka.longitude },
        map: mapInstanceRef.current,
        title: marka.name,
      });

      const infoWindow = new google.maps.InfoWindow({
        content: `
          <div style="padding: 8px;">
            <h3 style="font-weight: bold; margin-bottom: 4px;">${marka.name}</h3>
            ${marka.description ? `<p style="font-size: 14px; margin-bottom: 4px;">${marka.description}</p>` : ''}
            ${marka.address ? `<p style="font-size: 12px; color: #666;">${marka.address}</p>` : ''}
          </div>
        `,
      });

      marker.addListener('click', () => {
        infoWindow.open(mapInstanceRef.current, marker);
      });

      markersRef.current.push(marker);
    });

    // Fit bounds if there are markas
    if (markas.length > 0) {
      const bounds = new google.maps.LatLngBounds();
      markas.forEach((marka) => {
        bounds.extend({ lat: marka.latitude, lng: marka.longitude });
      });
      mapInstanceRef.current.fitBounds(bounds);
    }
  }, [markas, mapLoaded]);

  if (mapError) {
    return (
      <div className="w-full h-full flex items-center justify-center bg-gray-100">
        <div className="text-center p-8">
          <div className="text-6xl mb-4">📍</div>
          <h3 className="text-xl font-bold text-gray-800 mb-2">Offline Mode</h3>
          <p className="text-gray-600 mb-2">{mapError}</p>
          <p className="text-sm text-gray-500">
            Map functionality requires an internet connection and valid API key.
          </p>
        </div>
      </div>
    );
  }

  return (
    <>
      <div ref={mapRef} className="w-full h-full" />
      {showCreateModal && clickLocation && (
        <CreateMarkaModal
          latitude={clickLocation.lat}
          longitude={clickLocation.lng}
          onClose={() => {
            setShowCreateModal(false);
            setClickLocation(null);
          }}
          onSuccess={() => {
            setShowCreateModal(false);
            setClickLocation(null);
            onMarkaCreated();
          }}
        />
      )}
    </>
  );
}
