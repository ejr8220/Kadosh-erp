import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { SessionCompany } from '../../core/models/session.model';
import { SessionService } from '../../core/services/session.service';
import { CompanySelectorDialogComponent } from '../company-selector-dialog/company-selector-dialog.component';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatMenuModule,
    DialogModule,
    CompanySelectorDialogComponent
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  sidebarOpen = true;
  companyDialogOpen = false;
  generalExpanded = true;
  maestrosExpanded = true;

  constructor(
    public readonly session: SessionService,
    private readonly router: Router
  ) {}

  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  toggleGeneral(): void {
    this.generalExpanded = !this.generalExpanded;
  }

  toggleMaestros(): void {
    this.maestrosExpanded = !this.maestrosExpanded;
  }

  openCompanyDialog(): void {
    this.companyDialogOpen = true;
  }

  closeCompanyDialog(): void {
    this.companyDialogOpen = false;
  }

  changeCompany(company: SessionCompany): void {
    this.session.setCompany(company);
    this.closeCompanyDialog();
  }

  logout(): void {
    this.session.logout();
    this.router.navigateByUrl('/login');
  }
}
