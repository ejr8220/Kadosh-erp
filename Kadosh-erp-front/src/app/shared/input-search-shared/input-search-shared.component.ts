import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { GridColumn } from '../grid-shared/grid-column.model';
import { GridSharedComponent } from '../grid-shared/grid-shared.component';

@Component({
  selector: 'app-input-search-shared',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    DialogModule,
    ButtonModule,
    GridSharedComponent
  ],
  templateUrl: './input-search-shared.component.html',
  styleUrl: './input-search-shared.component.scss'
})
export class InputSearchSharedComponent implements OnChanges {
  @Input() title = 'Seleccionar registro';
  @Input() placeholder = 'Buscar';
  @Input() columns: GridColumn[] = [];
  @Input() gridUrl = '';
  @Input() apiBaseUrl = '';
  @Input() useFilterPost = false;
  @Input() initialDisplayValue = '';
  @Input() parameters: Record<string, string | number | boolean | null | undefined> = {};
  @Input() valueShow = 'id';
  @Input() selection: 'single' | 'multiple' = 'single';
  @Input() disabled = false;

  @Output() selected = new EventEmitter<any | any[]>();
  @Output() clear = new EventEmitter<void>();

  opened = false;
  displayValue = '';
  tempSelection: any | any[] | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if ('initialDisplayValue' in changes) {
      this.displayValue = this.initialDisplayValue;
    }

    if ('disabled' in changes && this.disabled) {
      this.opened = false;
    }
  }

  onActionButtonClick(): void {
    if (this.disabled) {
      return;
    }

    if (!this.displayValue) {
      this.opened = true;
      return;
    }

    this.displayValue = '';
    this.tempSelection = null;
    this.clear.emit();
  }

  onSelectionChange(value: any | any[]): void {
    this.tempSelection = value;
    // En modo búsqueda, emitir selección inmediatamente (antes de cerrar)
    this.selected.emit(value);

    if (Array.isArray(value)) {
      this.displayValue = `${value.length} seleccionados`;
    } else if (value) {
      this.displayValue = this.resolveDisplayValue(value);
    }
  }

  closeModal(): void {
    this.opened = false;
  }

  acceptSelection(): void {
    if (!this.tempSelection) {
      this.closeModal();
      return;
    }

    this.closeModal();
  }

  private resolveDisplayValue(row: any): string {
    let output = this.valueShow;
    const tokens = this.valueShow.match(/[a-zA-Z_][a-zA-Z0-9_]*/g) ?? [];

    tokens.forEach((token) => {
      const value = row?.[token] ?? '';
      output = output.replace(token, String(value));
    });

    return output;
  }
}
