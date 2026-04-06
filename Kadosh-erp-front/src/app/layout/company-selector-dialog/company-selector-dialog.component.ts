import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { SessionCompany } from '../../core/models/session.model';

@Component({
  selector: 'app-company-selector-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './company-selector-dialog.component.html',
  styleUrl: './company-selector-dialog.component.scss'
})
export class CompanySelectorDialogComponent {
  @Input() companies: SessionCompany[] = [];
  @Output() companyChange = new EventEmitter<SessionCompany>();

  selectCompany(company: SessionCompany): void {
    this.companyChange.emit(company);
  }
}
