import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { ForgotPasswordComponent } from './features/auth/forgot-password/forgot-password.component';
import { LoginComponent } from './features/auth/login/login.component';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';
import { BranchesComponent } from './features/general/branches/branches.component';
import { CompanyComponent } from './features/general/company/company.component';
import { CompanyCrudComponent } from './features/general/company/company-crud/company-crud.component';
import { CountriesComponent } from './features/general/countries/countries.component';
import { ParametersComponent } from './features/general/parameters/parameters.component';

export const routes: Routes = [
	{ path: '', redirectTo: 'login', pathMatch: 'full' },
	{ path: 'login', component: LoginComponent },
	{ path: 'forgot-password', component: ForgotPasswordComponent },
	{
		path: 'admin',
		component: AdminLayoutComponent,
		canActivate: [authGuard],
		children: [
			{ path: '', redirectTo: 'general/parameters', pathMatch: 'full' },
			{ path: 'general/parameters', component: ParametersComponent },
			{ path: 'general/company', component: CompanyComponent },
			{ path: 'general/company/new', component: CompanyCrudComponent, data: { mode: 'add' } },
			{ path: 'general/company/edit/:id', component: CompanyCrudComponent, data: { mode: 'edit' } },
			{ path: 'general/branches', component: BranchesComponent },
			{ path: 'general/countries', component: CountriesComponent }
		]
	},
	{ path: '**', redirectTo: 'login' }
];
