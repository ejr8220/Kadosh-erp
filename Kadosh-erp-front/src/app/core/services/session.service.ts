import { Injectable, signal } from '@angular/core';
import { SessionCompany, SessionState, SessionToken, SessionUser } from '../models/session.model';

const USER_KEY = 'kadosh_user';
const COMPANY_KEY = 'kadosh_company';
const TOKEN_KEY = 'kadosh_token';
const PERMISSIONS_KEY = 'kadosh_permissions';
const COMPANIES_KEY = 'kadosh_companies';

@Injectable({ providedIn: 'root' })
export class SessionService {
  readonly user = signal<SessionUser | null>(this.readUser());
  readonly token = signal<SessionToken | null>(this.readToken());
  readonly permissions = signal<string[]>(this.readPermissions());
  readonly companies = signal<SessionCompany[]>(this.readCompanies());
  readonly company = signal<SessionCompany | null>(this.readCompany());

  isAuthenticated(): boolean {
    return this.user() !== null && !!this.token()?.accessToken;
  }

  startSession(state: SessionState): void {
    this.user.set(state.user);
    this.token.set(state.token);
    this.permissions.set(state.permissions ?? []);
    this.companies.set(state.companies ?? []);

    sessionStorage.setItem(USER_KEY, JSON.stringify(state.user));
    sessionStorage.setItem(TOKEN_KEY, JSON.stringify(state.token));
    sessionStorage.setItem(PERMISSIONS_KEY, JSON.stringify(state.permissions ?? []));
    sessionStorage.setItem(COMPANIES_KEY, JSON.stringify(state.companies ?? []));

    const selected = this.company();
    const companies = this.companies();
    const selectedStillExists = selected ? companies.some((x) => x.id === selected.id) : false;

    if (!selectedStillExists) {
      if (companies.length > 0) {
        this.setCompany(companies[0]);
      } else {
        this.company.set(null);
        sessionStorage.removeItem(COMPANY_KEY);
      }
    }
  }

  logout(): void {
    this.user.set(null);
    this.token.set(null);
    this.permissions.set([]);
    this.companies.set([]);
    this.company.set(null);

    sessionStorage.removeItem(USER_KEY);
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(PERMISSIONS_KEY);
    sessionStorage.removeItem(COMPANIES_KEY);
    sessionStorage.removeItem(COMPANY_KEY);
  }

  setCompany(company: SessionCompany): void {
    this.company.set(company);
    sessionStorage.setItem(COMPANY_KEY, JSON.stringify(company));
  }

  private readUser(): SessionUser | null {
    const raw = sessionStorage.getItem(USER_KEY);
    return raw ? (JSON.parse(raw) as SessionUser) : null;
  }

  private readToken(): SessionToken | null {
    const raw = sessionStorage.getItem(TOKEN_KEY);
    return raw ? (JSON.parse(raw) as SessionToken) : null;
  }

  private readPermissions(): string[] {
    const raw = sessionStorage.getItem(PERMISSIONS_KEY);
    return raw ? (JSON.parse(raw) as string[]) : [];
  }

  private readCompanies(): SessionCompany[] {
    const raw = sessionStorage.getItem(COMPANIES_KEY);
    return raw ? (JSON.parse(raw) as SessionCompany[]) : [];
  }

  private readCompany(): SessionCompany | null {
    const raw = sessionStorage.getItem(COMPANY_KEY);
    return raw ? (JSON.parse(raw) as SessionCompany) : null;
  }
}
