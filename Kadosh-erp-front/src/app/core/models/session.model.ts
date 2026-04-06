export interface SessionUser {
  id: number;
  name: string;
}

export interface SessionCompany {
  id: number;
  name: string;
}

export interface SessionToken {
  accessToken: string;
  expiresAt: string;
}

export interface SessionState {
  user: SessionUser;
  token: SessionToken;
  permissions: string[];
  companies: SessionCompany[];
}
