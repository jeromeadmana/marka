export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  customerId: string;
}

export interface AuthResponse {
  token: string;
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  customerId: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  user: AuthResponse | null;
  token: string | null;
}
