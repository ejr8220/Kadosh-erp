import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, forkJoin, map, of, switchMap } from 'rxjs';
import { SessionCompany, SessionState } from '../models/session.model';

interface LoginRequest {
  userCodeOrEmail: string;
  password: string;
}

interface AuthTokenResponse {
  accessToken: string;
  accessTokenExpiresAt: string;
  userId: number;
  userCode: string;
}

interface UserResponse {
  id: number;
  userCode: string;
}

interface UserRoleResponse {
  userId: number;
  roleId: number;
}

interface CompanyUserResponse {
  userId: number;
  companyId: number;
}

interface RolePermissionResponse {
  roleId: number;
  permissionId: number;
}

interface PermissionResponse {
  id: number;
  name: string;
}

interface CompanyResponse {
  id: number;
  legalRepresentative?: string;
}

interface DataResult<T> {
  result: T[];
  count: number;
}

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  constructor(private readonly http: HttpClient) {}

  login(userCodeOrEmail: string, password: string): Observable<SessionState> {
    const body: LoginRequest = { userCodeOrEmail, password };

    return this.http.post<AuthTokenResponse>('/api/security/auth/login', body).pipe(
      switchMap((auth) =>
        forkJoin({
          user: this.http.get<UserResponse>(`/api/security/user/${auth.userId}`),
          userRoles: this.postFilter<UserRoleResponse>('/api/security/userrole/filter'),
          companyUsers: this.postFilter<CompanyUserResponse>('/api/security/companyuser/filter'),
          rolePermissions: this.postFilter<RolePermissionResponse>('/api/security/rolepermission/filter'),
          permissions: this.postFilter<PermissionResponse>('/api/security/permission/filter')
        }).pipe(
          switchMap((payload) => {
            const rolesByUser = payload.userRoles.filter((x) => x.userId === auth.userId);
            const companyUsersByUser = payload.companyUsers.filter((x) => x.userId === auth.userId);

            const roleIds = [...new Set(rolesByUser.map((x) => x.roleId))];
            const permissionIds = new Set(
              payload.rolePermissions
                .filter((x) => roleIds.includes(x.roleId))
                .map((x) => x.permissionId)
            );

            const permissionNames = payload.permissions
              .filter((x) => permissionIds.has(x.id))
              .map((x) => x.name);

            const companyIds = [...new Set(companyUsersByUser.map((x) => x.companyId))];

            if (companyIds.length === 0) {
              return of({
                user: { id: payload.user.id, name: payload.user.userCode },
                token: { accessToken: auth.accessToken, expiresAt: auth.accessTokenExpiresAt },
                permissions: permissionNames,
                companies: []
              });
            }

            return forkJoin(
              companyIds.map((companyId) =>
                this.http.get<CompanyResponse>(`/api/general/company/${companyId}`).pipe(
                  map((company) => this.mapCompany(company, companyId))
                )
              )
            ).pipe(
              map((companies) => ({
                user: { id: payload.user.id, name: payload.user.userCode },
                token: { accessToken: auth.accessToken, expiresAt: auth.accessTokenExpiresAt },
                permissions: permissionNames,
                companies
              }))
            );
          })
        )
      )
    );
  }

  private postFilter<T>(url: string, where: Array<Record<string, unknown>> = []): Observable<T[]> {
    return this.http
      .post<DataResult<T>>(url, {
        skip: 0,
        take: 10000,
        requiresCounts: true,
        sorted: [],
        search: [],
        where,
        aggregates: []
      })
      .pipe(map((response) => response?.result ?? []));
  }

  private mapCompany(company: CompanyResponse, fallbackId: number): SessionCompany {
    const computedName = company.legalRepresentative?.trim();
    return {
      id: company.id ?? fallbackId,
      name: computedName && computedName.length > 0 ? computedName : `Empresa ${fallbackId}`
    };
  }
}
