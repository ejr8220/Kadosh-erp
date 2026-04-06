import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { GridModule } from '@syncfusion/ej2-angular-grids';
import { ClickEventArgs } from '@syncfusion/ej2-navigations';

export interface BranchInlineRow {
  id: number;
  code: string;
  name: string;
  email: string;
  phone: string;
  address: string;
  status: string;
}

type InternalBranchInlineRow = BranchInlineRow & {
  isEditing: boolean;
};

@Component({
  selector: 'app-grid-inline',
  standalone: true,
  imports: [CommonModule, GridModule, ButtonModule],
  templateUrl: './grid-inline.component.html',
  styleUrl: './grid-inline.component.scss'
})
export class GridInlineComponent implements OnChanges {
  @Input() rows: BranchInlineRow[] = [];
  @Output() rowsChange = new EventEmitter<BranchInlineRow[]>();
  @Output() rowSaved = new EventEmitter<BranchInlineRow>();
  @Output() rowDeleted = new EventEmitter<number>();

  data: InternalBranchInlineRow[] = [];
  errorMessage = '';
  readonly toolbar = [
    { id: 'add-inline', prefixIcon: 'e-icons e-add', text: '', tooltipText: 'Agregar nuevo' }
  ];

  get canAdd(): boolean {
    return !this.hasEditingRow;
  }

  get hasEditingRow(): boolean {
    return this.data.some((x) => x.isEditing);
  }

  ngOnChanges(): void {
    this.data = (this.rows ?? []).map((x) => ({
      ...x,
      status: x.status || 'Active',
      isEditing: false
    }));
  }

  onToolbarClick(args: ClickEventArgs): void {
    const itemId = (args.item.id ?? '').toLowerCase();
    if (!itemId.includes('add-inline')) {
      return;
    }

    if (!this.canAdd) {
      return;
    }

    this.onAddRow();
  }

  onAddRow(): void {
    if (!this.canAdd) {
      return;
    }

    this.errorMessage = '';
    this.data = [
      ...this.data,
      {
        id: this.nextId(),
        code: '',
        name: '',
        email: '',
        phone: '',
        address: '',
        status: 'Active',
        isEditing: true
      }
    ];
  }

  onEditRow(row: InternalBranchInlineRow): void {
    if (this.hasEditingRow) {
      return;
    }

    this.errorMessage = '';
    row.isEditing = true;
  }

  onUpdateField<K extends keyof BranchInlineRow>(
    rowId: number,
    field: K,
    event: Event
  ): void {
    const target = event.target as HTMLInputElement | null;
    const value = (target?.value ?? '') as BranchInlineRow[K];
    const row = this.data.find((x) => x.id === rowId);
    if (!row) {
      return;
    }

    (row as BranchInlineRow)[field] = value;
  }

  onSaveRow(row: InternalBranchInlineRow): void {
    // Encontrar el índice de la fila actual en el array de datos
    const rowIndex = this.data.findIndex((x) => x.id === row.id);
    if (rowIndex === -1) {
      this.errorMessage = 'Error: No se encontró la fila.';
      return;
    }

    // Obtener la referencia directa del array para asegurar que tenemos los datos actualizados
    const currentRow = this.data[rowIndex];

    const normalizedCode = String(currentRow.code ?? '').trim().toUpperCase();
    const normalizedName = String(currentRow.name ?? '').trim();
    const normalizedEmail = String(currentRow.email ?? '').trim();

    if (!normalizedCode || !normalizedName) {
      this.errorMessage = 'Codigo y nombre son obligatorios para la sucursal.';
      return;
    }

    if (normalizedEmail && !this.isValidEmail(normalizedEmail)) {
      this.errorMessage = 'El formato del email de la sucursal no es valido.';
      return;
    }

    const duplicated = this.data.some(
      (x) => x.id !== currentRow.id && String(x.code ?? '').trim().toUpperCase() === normalizedCode
    );

    if (duplicated) {
      this.errorMessage = `Ya existe una sucursal con el codigo ${normalizedCode}.`;
      return;
    }

    this.errorMessage = '';
    currentRow.code = normalizedCode;
    currentRow.name = normalizedName;
    currentRow.email = normalizedEmail;
    currentRow.phone = String(currentRow.phone ?? '').trim();
    currentRow.address = String(currentRow.address ?? '').trim();
    currentRow.status = currentRow.status || 'Active';
    currentRow.isEditing = false;

    this.emitRows();
    const { isEditing: _, ...savedRow } = currentRow;
    this.rowSaved.emit(savedRow);
  }

  onDeleteRow(row: InternalBranchInlineRow): void {
    this.errorMessage = '';
    this.rowDeleted.emit(row.id);
    this.data = this.data.filter((x) => x.id !== row.id);
    this.emitRows();
  }

  private nextId(): number {
    const min = this.data.reduce((acc, x) => Math.min(acc, Number(x.id) || 0), 0);
    return min - 1;
  }

  private emitRows(): void {
    const output = this.data
      .filter((x) => !x.isEditing)
      .map(({ isEditing, ...row }) => row);

    this.rowsChange.emit(output);
  }

  private isValidEmail(value: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
  }
}