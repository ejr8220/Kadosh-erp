import { CommonModule, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild, ViewEncapsulation, inject } from '@angular/core';
import { Router } from '@angular/router';
import {
  ExcelExportService,
  FilterService,
  GridComponent,
  GridModule,
  PageService,
  SelectionService,
  SortService,
  ToolbarService,
  ToolbarItems
} from '@syncfusion/ej2-angular-grids';
import { ClickEventArgs } from '@syncfusion/ej2-navigations';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { DialogModule } from '@syncfusion/ej2-angular-popups';
import { GridColumn } from './grid-column.model';

@Component({
  selector: 'app-grid-shared',
  standalone: true,
  imports: [CommonModule, GridModule, ButtonModule, DialogModule],
  providers: [PageService, SortService, FilterService, SelectionService, ExcelExportService, ToolbarService, DatePipe],
  templateUrl: './grid-shared.component.html',
  styleUrl: './grid-shared.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class GridSharedComponent implements OnInit, OnChanges {
  @Input() columns: GridColumn[] = [];
  @Input() editUrl = '';
  @Input() newUrl = '';
  @Input() gridUrl = '';
  @Input() localData: any[] | null = null;
  @Input() titleDelete = 'Esta seguro de eliminar el registro?';
  @Input() parameters: Record<string, string | number | boolean | null | undefined> = {};
  @Input() selection: 'single' | 'multiple' = 'single';
  @Input() useFilterPost = false;
  @Input() apiBaseUrl = '';
  @Input() isSearchMode = false;

  @Output() selectionChange = new EventEmitter<any | any[]>();
  @Output() closeSearchMode = new EventEmitter<void>();
  @Output() addRequested = new EventEmitter<void>();
  @Output() editRequested = new EventEmitter<any>();
  @Output() deleteRequested = new EventEmitter<any>();

  @ViewChild('grid') grid?: GridComponent;

  readonly router = inject(Router);
  readonly http = inject(HttpClient);
  readonly datePipe = inject(DatePipe);

  data: any[] = [];
  toolbar: ToolbarItems[] = ['ExcelExport'];
  loading = false;
  showDeleteDialog = false;
  rowToDelete: any | null = null;
  selectedRows: any[] = [];
  expandedContactRows = new Set<string>();

  private readonly phoneNames = new Set(['telefono', 'teléfono']);

  ngOnInit(): void {
    this.loadData();
  }

  ngOnChanges(): void {
    this.loadData();
  }

  loadData(): void {
    if (this.localData) {
      this.data = [...this.localData];
      this.loading = false;
      return;
    }

    if (!this.gridUrl) {
      this.data = [];
      return;
    }

    this.loading = true;
    const endpoint = this.resolveApiUrl(this.gridUrl);
    const request$ = this.useFilterPost
      ? this.http.post<any>(`${endpoint}/filter`, this.buildFilterRequest())
      : this.http.get<any>(this.buildSearchUrl(endpoint));

    request$.subscribe({
      next: (response) => {
        if (Array.isArray(response)) {
          this.data = response;
        } else if (response?.result && Array.isArray(response.result)) {
          this.data = response.result;
        } else {
          this.data = [];
        }
        this.loading = false;
      },
      error: () => {
        this.data = [];
        this.loading = false;
      }
    });
  }

  onToolbarClick(args: ClickEventArgs): void {
    const itemId = (args.item.id ?? '').toLowerCase();

    if (itemId.includes('add-new')) {
      if (this.localData) {
        this.addRequested.emit();
        return;
      }

      this.router.navigateByUrl(this.newUrl);
      return;
    }

    if (itemId.includes('refresh-grid')) {
      this.loadData();
      return;
    }

    if (itemId.includes('excelexport')) {
      this.grid?.excelExport();
    }
  }

  onRowSelected(): void {
    this.syncSelection();
  }

  onRowDeselected(): void {
    this.syncSelection();
  }

  onRowDoubleClick(row: any): void {
    if (!this.isSearchMode) {
      return;
    }

    // En modo búsqueda, doble-click = seleccionar y cerrar
    if (this.selection === 'single') {
      this.selectionChange.emit(row);
    } else {
      // En múltiple, verificar si ya está seleccionada
      const isSelected = this.selectedRows.some((r) => this.resolveId(r) === this.resolveId(row));
      if (!isSelected) {
        this.selectedRows = [...this.selectedRows, row];
      }
      this.selectionChange.emit(this.selectedRows);
    }

    this.closeSearchMode.emit();
  }

  onEdit(row: any): void {
    if (this.localData) {
      this.editRequested.emit(row);
      return;
    }

    const id = this.resolveId(row);
    this.router.navigateByUrl(`${this.editUrl}/${id}`);
  }

  askDelete(row: any): void {
    this.rowToDelete = row;
    this.showDeleteDialog = true;
  }

  confirmDelete(): void {
    if (!this.rowToDelete) {
      return;
    }

    if (this.localData) {
      const row = this.rowToDelete;
      this.closeDeleteDialog();
      this.deleteRequested.emit(row);
      return;
    }

    const id = this.resolveId(this.rowToDelete);
    const endpoint = this.resolveApiUrl(this.gridUrl);
    this.http.delete(`${endpoint}/${id}`).subscribe({
      next: () => {
        this.closeDeleteDialog();
        this.loadData();
      },
      error: () => {
        this.closeDeleteDialog();
      }
    });
  }

  closeDeleteDialog(): void {
    this.showDeleteDialog = false;
    this.rowToDelete = null;
  }

  valueAccessor = (field: string, row: any): string | number | boolean | null => {
    const col = this.columns.find((x) => x.field === field);
    const value = row?.[field];

    if (value === null || value === undefined) {
      return '';
    }

    if (col?.type === 'boolean') {
      return value ? 'SI' : 'NO';
    }

    if (col?.type === 'date') {
      return this.datePipe.transform(value, 'dd/MM/yyyy HH:mm:ss') ?? '';
    }

    if (this.isStatusColumn(field)) {
      return this.formatStatus(value);
    }

    return value;
  };

  isStatusColumn(field: string): boolean {
    return field?.toLowerCase() === 'status';
  }

  isContactFormsColumn(field: string): boolean {
    return field?.toLowerCase() === 'contactforms';
  }

  getPhoneContact(row: any): string {
    const contacts = this.getContactForms(row);
    const phone = contacts.find((x) => this.isPhoneLabel(x.name));

    if (phone?.value) {
      return phone.value;
    }

    return String(row?.telefono ?? row?.Telefono ?? '').trim();
  }

  getSecondaryContactForms(row: any): Array<{ name: string; value: string }> {
    return this.getContactForms(row)
      .filter((x) => !this.isPhoneLabel(x.name))
      .filter((x) => x.value.trim() !== '');
  }

  hasExtraContactForms(row: any): boolean {
    return this.getSecondaryContactForms(row).length > 0;
  }

  isContactFormsExpanded(row: any): boolean {
    return this.expandedContactRows.has(this.getRowKey(row));
  }

  toggleContactForms(row: any, event: Event): void {
    event.preventDefault();
    event.stopPropagation();

    const key = this.getRowKey(row);
    if (this.expandedContactRows.has(key)) {
      this.expandedContactRows.delete(key);
      return;
    }

    this.expandedContactRows.add(key);
  }

  trackContact(contact: { name: string; value: string }, index: number): string {
    return `${contact.name}|${contact.value}|${index}`;
  }

  formatStatus(value: unknown): string {
    const raw = String(value ?? '').trim().toLowerCase();

    if (raw === 'active' || raw === 'activo') {
      return 'Activo';
    }

    if (raw === 'inactive' || raw === 'inactivo') {
      return 'Inactivo';
    }

    return String(value ?? '');
  }

  getStatusValue(row: any, field = 'status'): unknown {
    const direct = row?.[field];
    if (direct !== undefined && direct !== null && String(direct).trim() !== '') {
      return direct;
    }

    const pascal = row?.Status;
    if (pascal !== undefined && pascal !== null && String(pascal).trim() !== '') {
      return pascal;
    }

    if (typeof row?.isDeleted === 'boolean') {
      return row.isDeleted ? 'Inactive' : 'Active';
    }

    if (typeof row?.IsDeleted === 'boolean') {
      return row.IsDeleted ? 'Inactive' : 'Active';
    }

    return '';
  }

  getStatusClass(value: unknown): string {
    const raw = String(value ?? '').trim().toLowerCase();

    if (raw === 'active' || raw === 'activo') {
      return 'status-active';
    }

    if (raw === 'inactive' || raw === 'inactivo') {
      return 'status-inactive';
    }

    return 'status-default';
  }

  private syncSelection(): void {
    this.selectedRows = (this.grid?.getSelectedRecords() as any[]) ?? [];

    if (this.selection === 'single') {
      this.selectionChange.emit(this.selectedRows[0] ?? null);
      return;
    }

    this.selectionChange.emit(this.selectedRows);
  }

  private resolveId(row: any): number | string {
    return row?.id ?? row?.Id;
  }

  private getRowKey(row: any): string {
    return String(this.resolveId(row));
  }

  private getContactForms(row: any): Array<{ name: string; value: string }> {
    const source = row?.contactForms ?? row?.ContactForms;
    if (!Array.isArray(source)) {
      return [];
    }

    return source
      .map((x) => ({
        name: String(x?.name ?? x?.Name ?? '').trim(),
        value: String(x?.value ?? x?.Value ?? '').trim()
      }))
      .filter((x) => x.name !== '' && x.value !== '');
  }

  private isPhoneLabel(name: string): boolean {
    return this.phoneNames.has(name.trim().toLowerCase());
  }

  private buildSearchUrl(endpoint: string): string {
    const search = new URLSearchParams();

    Object.entries(this.parameters).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        search.set(key, String(value));
      }
    });

    const query = search.toString();
    return query ? `${endpoint}?${query}` : endpoint;
  }

  private resolveApiUrl(path: string): string {
    if (!path) {
      return path;
    }

    if (/^https?:\/\//i.test(path)) {
      return path;
    }

    if (!this.apiBaseUrl) {
      return path;
    }

    const base = this.apiBaseUrl.endsWith('/') ? this.apiBaseUrl.slice(0, -1) : this.apiBaseUrl;
    const suffix = path.startsWith('/') ? path : `/${path}`;
    return `${base}${suffix}`;
  }

  private buildFilterRequest(): Record<string, unknown> {
    const where = Object.entries(this.parameters)
      .filter(([, value]) => value !== null && value !== undefined && value !== '')
      .map(([key, value]) => ({
        // Backend filtra sobre propiedades del DTO en C# (PascalCase).
        field: this.toPascalCase(key),
        operator: 'equal',
        value,
        ignoreCase: true,
        ignoreAccent: true,
        condition: 'and'
      }));

    return {
      skip: 0,
      take: 200,
      requiresCounts: true,
      sorted: [],
      search: [],
      where,
      aggregates: []
    };
  }

  private toPascalCase(value: string): string {
    if (!value) {
      return value;
    }

    return `${value[0].toUpperCase()}${value.slice(1)}`;
  }
}
