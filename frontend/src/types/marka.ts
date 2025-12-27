export interface Marka {
  id: string;
  name: string;
  description?: string;
  latitude: number;
  longitude: number;
  address?: string;
  category?: string;
  status: string;
  customerId: string;
  createdByUserId: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string;
}

export interface CreateMarkaDto {
  name: string;
  description?: string;
  latitude: number;
  longitude: number;
  address?: string;
  category?: string;
  customerId: string;
  createdByUserId: string;
}

export interface UpdateMarkaDto {
  name?: string;
  description?: string;
  latitude?: number;
  longitude?: number;
  address?: string;
  category?: string;
  status?: string;
}
