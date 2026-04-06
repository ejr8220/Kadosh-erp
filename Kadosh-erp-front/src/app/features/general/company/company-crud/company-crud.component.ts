import { CommonModule, DOCUMENT } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
import { UploaderModule } from '@syncfusion/ej2-angular-inputs';
import { TabModule } from '@syncfusion/ej2-angular-navigations';
import { SelectedEventArgs } from '@syncfusion/ej2-inputs';
import { catchError, firstValueFrom, of } from 'rxjs';
import { FormSharedComponent } from '../../../../shared/form-shared/form-shared.component';
import { GridColumn } from '../../../../shared/grid-shared/grid-column.model';
import { BranchInlineRow, GridInlineComponent } from '../../../../shared/grid-inline/grid-inline.component';
import { InputSearchSharedComponent } from '../../../../shared/input-search-shared/input-search-shared.component';

interface OptionItem {
  id: string;
  code: string;
  name: string;
}

@Component({
  selector: 'app-company-crud',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormSharedComponent,
    ButtonModule,
    DropDownListModule,
    UploaderModule,
    TabModule,
    InputSearchSharedComponent,
    GridInlineComponent
  ],
  templateUrl: './company-crud.component.html',
  styleUrl: './company-crud.component.scss'
})
export class CompanyCrudComponent implements OnInit, OnDestroy {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly http = inject(HttpClient);
  private readonly document = inject(DOCUMENT);

  readonly apiBaseUrl = 'http://localhost:5086';

  mode: 'add' | 'edit' = 'add';
  companyId: number | null = null;
  footerTooltip = 'Seleccione un campo para ver su descripcion.';
  logoPreview = '';
  validatingMainIdentification = false;
  validatingLegalRepresentative = false;
  validatingAccountant = false;
  mainValidationState: 'unknown' | 'valid' | 'invalid' = 'unknown';
  legalValidationState: 'unknown' | 'valid' | 'invalid' = 'unknown';
  accountantValidationState: 'unknown' | 'valid' | 'invalid' = 'unknown';
  private toastTimer: ReturnType<typeof setTimeout> | null = null;
  private toastElement: HTMLDivElement | null = null;

  identificationTypes: OptionItem[] = [];
  companyTypes: OptionItem[] = [];
  draftBranches: BranchInlineRow[] = [];

  readonly countryColumns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, type: 'number', hidden: true },
    { field: 'name', headerText: 'Pais', width: 200 },
    { field: 'isoCode', headerText: 'Iso', width: 120 }
  ];

  readonly provinceColumns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, type: 'number', hidden: true },
    { field: 'name', headerText: 'Provincia', width: 220 }
  ];

  readonly cityColumns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, type: 'number', hidden: true },
    { field: 'name', headerText: 'Ciudad', width: 220 }
  ];

  readonly branchColumns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, type: 'number' },
    { field: 'code', headerText: 'Codigo', width: 130 },
    { field: 'name', headerText: 'Nombre', width: 220 },
    { field: 'email', headerText: 'Email', width: 230 },
    { field: 'status', headerText: 'Estado', width: 120 }
  ];

  readonly form = this.fb.nonNullable.group({
    identificationTypeId: ['', [Validators.required]],
    identificationNumber: ['', [Validators.required]],
    businessName: [''],
    tradeName: [''],
    firstNames: [''],
    lastNames: [''],
    countryId: [''],
    countryName: [''],
    provinceId: [''],
    provinceName: [''],
    cityId: [''],
    cityName: [''],
    companyTypeId: ['', [Validators.required]],
    address: [''],
    legalRepresentativeIdentification: [''],
    legalRepresentativeName: [''],
    accountantIdentification: [''],
    accountantName: ['']
  });

  get title(): string {
    return this.mode === 'add' ? 'Crear empresa' : 'Editar empresa';
  }

  get isRuc(): boolean {
    const selectedId = this.form.controls.identificationTypeId.value;
    const selected = this.identificationTypes.find((x) => String(x.id) === String(selectedId));
    const token = `${selected?.code ?? ''} ${selected?.name ?? ''}`.toLowerCase();
    return token.includes('ruc');
  }

  ngOnInit(): void {
    this.mode = this.route.snapshot.data['mode'] === 'edit' ? 'edit' : 'add';
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.companyId = Number.isFinite(id) && id > 0 ? id : null;

    this.loadIdentificationTypes();
    this.loadCompanyTypes();
    if (this.mode !== 'edit') {
      this.form.controls.identificationTypeId.enable({ emitEvent: false });
    }
    this.form.controls.identificationNumber.setValidators([
      Validators.required,
      Validators.maxLength(20)
    ]);

    if (this.mode === 'edit' && this.companyId) {
      this.loadCompany(this.companyId);
      this.loadBranches(this.companyId);
    }
  }

  ngOnDestroy(): void {
    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
      this.toastTimer = null;
    }

    if (this.toastElement) {
      this.toastElement.remove();
      this.toastElement = null;
    }
  }

  onBack(): void {
    this.router.navigateByUrl('/admin/general/company');
  }

  async onSave(): Promise<void> {
    try {
      this.footerTooltip = 'Validando informacion para grabar...';

      if (this.form.invalid) {
        this.form.markAllAsTouched();
        this.footerTooltip = 'Complete los campos obligatorios antes de grabar.';
        this.showValidationToast(this.footerTooltip, 'error');
        return;
      }

      const isMainValid = await this.validateMainIdentification({ silentSuccessMessage: true });
      if (!isMainValid) {
        this.showValidationToast(this.footerTooltip || 'Validacion de identificacion principal fallida.', 'error');
        return;
      }

      const isLegalValid = await this.validateSupportIdentification('legalRepresentativeIdentification', {
        loadingSetter: (value) => this.validatingLegalRepresentative = value,
        silentSuccessMessage: true,
        fieldName: 'identificacion de representante legal'
      });
      if (!isLegalValid) {
        this.showValidationToast(this.footerTooltip || 'Validacion de representante legal fallida.', 'error');
        return;
      }

      const isAccountantValid = await this.validateSupportIdentification('accountantIdentification', {
        loadingSetter: (value) => this.validatingAccountant = value,
        silentSuccessMessage: true,
        fieldName: 'identificacion de contador'
      });
      if (!isAccountantValid) {
        this.showValidationToast(this.footerTooltip || 'Validacion de contador fallida.', 'error');
        return;
      }

      const payload = this.buildPayload();

      const request$ = this.mode === 'edit' && this.companyId
        ? this.http.put(`${this.apiBaseUrl}/api/general/company/${this.companyId}`, payload)
        : this.http.post(`${this.apiBaseUrl}/api/general/company`, payload);

      request$.subscribe({
        next: () => {
          this.showValidationToast('Empresa grabada correctamente.', 'success');
          setTimeout(() => this.onBack(), 1500);
        },
        error: (error) => {
          this.footerTooltip = this.extractApiErrorMessage(error, 'No fue posible grabar la informacion.');
          this.showValidationToast(this.footerTooltip, 'error');
        }
      });
    } catch {
      this.footerTooltip = 'Se produjo un error al intentar grabar la informacion.';
      this.showValidationToast(this.footerTooltip, 'error');
    }
  }

  onPrint(): void {
    window.print();
  }

  onFieldFocus(message: string): void {
    this.footerTooltip = message;
  }

  onIdentificationInputFocus(kind: 'main' | 'legal' | 'accountant', message: string): void {
    this.onFieldFocus(message);

    if (kind === 'main') {
      this.mainValidationState = 'unknown';
      return;
    }

    if (kind === 'legal') {
      this.legalValidationState = 'unknown';
      return;
    }

    this.accountantValidationState = 'unknown';
  }

  onIdentificationTypeChanged(): void {
    if (this.isRuc) {
      this.form.patchValue({ firstNames: '', lastNames: '' });
      return;
    }

    this.form.patchValue({ businessName: '', tradeName: '' });
  }

  async onValidateMainIdentification(event?: Event): Promise<void> {
    event?.preventDefault();
    event?.stopPropagation();
    this.footerTooltip = 'Validando identificacion principal...';
    this.showValidationToast('Validando identificacion principal...', 'success');
    const isValid = await this.validateMainIdentification({ silentSuccessMessage: false });
    this.mainValidationState = isValid ? 'valid' : 'invalid';
    this.showValidationToast(isValid ? 'Validacion completada correctamente.' : this.footerTooltip, isValid ? 'success' : 'error');
  }

  async onValidateLegalRepresentative(event?: Event): Promise<void> {
    event?.preventDefault();
    event?.stopPropagation();
    this.footerTooltip = 'Validando identificacion de representante legal...';
    this.showValidationToast('Validando identificacion de representante legal...', 'success');
    const isValid = await this.validateSupportIdentification('legalRepresentativeIdentification', {
      loadingSetter: (value) => this.validatingLegalRepresentative = value,
      silentSuccessMessage: false,
      fieldName: 'identificacion de representante legal'
    });
    this.legalValidationState = isValid ? 'valid' : 'invalid';
    this.showValidationToast(isValid ? 'Validacion completada correctamente.' : this.footerTooltip, isValid ? 'success' : 'error');
  }

  async onValidateAccountant(event?: Event): Promise<void> {
    event?.preventDefault();
    event?.stopPropagation();
    this.footerTooltip = 'Validando identificacion de contador...';
    this.showValidationToast('Validando identificacion de contador...', 'success');
    const isValid = await this.validateSupportIdentification('accountantIdentification', {
      loadingSetter: (value) => this.validatingAccountant = value,
      silentSuccessMessage: false,
      fieldName: 'identificacion de contador'
    });
    this.accountantValidationState = isValid ? 'valid' : 'invalid';
    this.showValidationToast(isValid ? 'Validacion completada correctamente.' : this.footerTooltip, isValid ? 'success' : 'error');
  }

  private showValidationToast(message: string, type: 'success' | 'error'): void {
    const normalized = String(message ?? '').trim();
    if (!normalized) {
      return;
    }

    if (this.toastElement) {
      this.toastElement.remove();
      this.toastElement = null;
    }

    const toast = this.document.createElement('div');
    const isSuccess = type === 'success';
    toast.textContent = normalized;
    toast.style.position = 'fixed';
    toast.style.top = '16px';
    toast.style.right = '16px';
    toast.style.zIndex = '2147483647';
    toast.style.maxWidth = '420px';
    toast.style.padding = '10px 14px';
    toast.style.borderRadius = '8px';
    toast.style.border = isSuccess ? '1px solid #37a061' : '1px solid #d64545';
    toast.style.background = isSuccess ? '#eaf8ef' : '#feecec';
    toast.style.color = isSuccess ? '#166534' : '#9f1239';
    toast.style.fontSize = '13px';
    toast.style.fontWeight = '600';
    toast.style.boxShadow = '0 8px 18px rgba(15, 23, 42, 0.18)';

    this.document.body.appendChild(toast);
    this.toastElement = toast;

    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
    }

    this.toastTimer = setTimeout(() => {
      if (this.toastElement) {
        this.toastElement.remove();
        this.toastElement = null;
      }
      this.toastTimer = null;
    }, 3000);
  }

  onCountrySelected(value: any): void {
    this.form.patchValue({
      countryId: String(value?.id ?? value?.Id ?? ''),
      countryName: String(value?.name ?? value?.Name ?? ''),
      provinceId: '',
      provinceName: '',
      cityId: '',
      cityName: ''
    });
  }

  onProvinceSelected(value: any): void {
    this.form.patchValue({
      provinceId: String(value?.id ?? value?.Id ?? ''),
      provinceName: String(value?.name ?? value?.Name ?? ''),
      cityId: '',
      cityName: ''
    });
  }

  onCitySelected(value: any): void {
    this.form.patchValue({
      cityId: String(value?.id ?? value?.Id ?? ''),
      cityName: String(value?.name ?? value?.Name ?? '')
    });
  }

  clearCountry(): void {
    this.form.patchValue({
      countryId: '',
      countryName: '',
      provinceId: '',
      provinceName: '',
      cityId: '',
      cityName: ''
    });
  }

  clearProvince(): void {
    this.form.patchValue({
      provinceId: '',
      provinceName: '',
      cityId: '',
      cityName: ''
    });
  }

  clearCity(): void {
    this.form.patchValue({ cityId: '', cityName: '' });
  }

  onLogoSelected(args: SelectedEventArgs): void {
    const file = args.filesData?.[0];
    if (!file) {
      return;
    }

    const extension = String(file.type ?? '').toLowerCase();
    const validTypes = ['png', 'jpg', 'jpeg'];

    if (!validTypes.includes(extension)) {
      args.cancel = true;
      this.footerTooltip = 'Solo se permiten imagenes PNG o JPG.';
      return;
    }

    const rawFile = file.rawFile as File | undefined;
    if (!rawFile) {
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      this.logoPreview = String(reader.result ?? '');
    };
    reader.readAsDataURL(rawFile);
  }

  private loadIdentificationTypes(): void {
    const filterRequest = {
      skip: 0,
      take: 200,
      requiresCounts: true,
      sorted: [],
      search: [],
      where: [],
      aggregates: []
    };

    this.http.post<any>(`${this.apiBaseUrl}/api/tax/identificationType/filter`, filterRequest).pipe(
      catchError(() => this.http.get<any[]>(`${this.apiBaseUrl}/api/tax/identificationType`)),
      catchError(() => of([]))
    ).subscribe((response) => {
      const source = Array.isArray(response) ? response : (response?.result ?? []);

      this.identificationTypes = (source ?? []).map((x: any) => ({
        id: String(x?.id ?? x?.Id ?? ''),
        code: String(x?.code ?? x?.Code ?? x?.name ?? x?.Name ?? ''),
        name: String(x?.name ?? x?.Name ?? x?.description ?? x?.Description ?? '')
      })).filter((x: OptionItem) => Number(x.id) > 0);

      if (!this.identificationTypes.length) {
        this.identificationTypes = this.getDefaultIdentificationTypes();
      }

      // En edición: si ya hay un valor cargado, forzar re-asignación numérica
      // para que el DropDownList lo tome como seleccionado tras poblar dataSource.
      const currentId = String(this.form.controls.identificationTypeId.value ?? '').trim();
      if (Number(currentId) > 0) {
        this.form.patchValue({ identificationTypeId: currentId });
        if (this.mode === 'edit') {
          this.form.controls.identificationTypeId.disable({ emitEvent: false });
        }
        return;
      }

      if (!this.companyId) {
        const rucOption = this.identificationTypes.find((x) => {
          const token = `${x.code} ${x.name}`.toLowerCase();
          return token.includes('ruc');
        }) ?? this.identificationTypes[0];

        if (rucOption) {
          this.form.patchValue({ identificationTypeId: String(rucOption.id) });
        }
      }
    });
  }

  private loadCompanyTypes(): void {
    const filterRequest = {
      skip: 0,
      take: 200,
      requiresCounts: true,
      sorted: [],
      search: [],
      where: [],
      aggregates: []
    };

    this.http.post<any>(`${this.apiBaseUrl}/api/tax/companyType/filter`, filterRequest).pipe(
      catchError(() => this.http.get<any[]>(`${this.apiBaseUrl}/api/tax/companyType`)),
      catchError(() => of([]))
    ).subscribe((response) => {
      const source = Array.isArray(response) ? response : (response?.result ?? []);

      this.companyTypes = (source ?? []).map((x: any) => ({
        id: String(x?.id ?? x?.Id ?? ''),
        code: String(x?.code ?? x?.Code ?? ''),
        name: String(x?.name ?? x?.Name ?? x?.description ?? x?.Description ?? '')
      }));
    });
  }

  private loadCompany(id: number): void {
    this.http.get<any>(`${this.apiBaseUrl}/api/general/company/${id}`).subscribe({
      next: (response) => {
        const company = response?.result ?? response;
        const identificationTypeId = String(company?.identificationTypeId ?? company?.IdentificationTypeId ?? '').trim();
        const razonSocial = String(company?.razonSocial ?? company?.RazonSocial ?? company?.personFirstName ?? company?.PersonFirstName ?? '');
        const nombreComercial = String(company?.nombreComercial ?? company?.NombreComercial ?? company?.personLastName ?? company?.PersonLastName ?? '');

        this.form.patchValue({
          identificationTypeId: identificationTypeId,
          identificationNumber: String(company?.identificacion ?? company?.Identificacion ?? ''),
          businessName: razonSocial,
          tradeName: nombreComercial,
          firstNames: razonSocial,
          lastNames: nombreComercial,
          countryId: String(company?.countryId ?? company?.CountryId ?? ''),
          countryName: String(company?.pais ?? company?.Pais ?? ''),
          provinceId: String(company?.provinceId ?? company?.ProvinceId ?? ''),
          provinceName: String(company?.provincia ?? company?.Provincia ?? ''),
          cityId: String(company?.cityId ?? company?.CityId ?? ''),
          cityName: String(company?.ciudad ?? company?.Ciudad ?? ''),
          companyTypeId: String(company?.companyTypeId ?? company?.CompanyTypeId ?? ''),
          address: String(company?.direccion ?? company?.Direccion ?? ''),
          legalRepresentativeIdentification: String(
            company?.legalpresentativeIdentification ??
            company?.LegalpresentativeIdentification ??
            ''
          ),
          legalRepresentativeName: String(
            company?.legalRepresentativeName ??
            company?.LegalRepresentativeName ??
            ''
          ),
          accountantIdentification: String(company?.accountantIdentification ?? company?.AccountantIdentification ?? ''),
          accountantName: String(company?.accountantName ?? company?.AccountantName ?? '')
        });

        const logoUrl = String(company?.logoUrl ?? company?.LogoUrl ?? '');
        if (logoUrl) {
          this.logoPreview = logoUrl;
        }

        if (this.mode === 'edit') {
          this.form.controls.identificationTypeId.disable({ emitEvent: false });
        }
      },
      error: () => {
        this.footerTooltip = 'No se pudo cargar la empresa para edicion.';
      }
    });
  }

  private loadBranches(companyId: number): void {
    const filterRequest = {
      skip: 0,
      take: 200,
      requiresCounts: true,
      sorted: [],
      search: [],
      where: [
        {
          field: 'CompanyId',
          operator: 'equal',
          value: companyId,
          condition: 'and',
          ignoreCase: false
        }
      ],
      aggregates: []
    };

    this.http.post<any>(`${this.apiBaseUrl}/api/general/branch/filter`, filterRequest).pipe(
      catchError(() => of({ result: [] }))
    ).subscribe((response) => {
      const source = Array.isArray(response) ? response : (response?.result ?? []);
      this.draftBranches = (source as any[]).map((x) => ({
        id: Number(x?.id ?? x?.Id ?? 0),
        code: String(x?.code ?? x?.Code ?? ''),
        name: String(x?.name ?? x?.Name ?? ''),
        email: String(x?.email ?? x?.Email ?? ''),
        phone: String(x?.phone ?? x?.Phone ?? ''),
        address: String(x?.address ?? x?.Address ?? ''),
        status: String(x?.status ?? x?.Status ?? 'Active')
      }));
    });
  }

  private loadPersonDetails(personId: number): void {
    this.http.get<any>(`${this.apiBaseUrl}/api/general/person/${personId}`).subscribe({
      next: (response) => {
        const person = response?.result ?? response;

        const countryId = person?.countryId ?? person?.CountryId;
        const provinceId = person?.provinceId ?? person?.ProvinceId;
        const cityId = person?.cityId ?? person?.CityId;

        this.form.patchValue({
          identificationTypeId: String(person?.identificationTypeId ?? person?.IdentificationTypeId ?? this.form.controls.identificationTypeId.value ?? ''),
          identificationNumber: String(person?.identificationNumber ?? person?.IdentificationNumber ?? this.form.controls.identificationNumber.value ?? ''),
          firstNames: String(person?.firstName ?? person?.FirstName ?? this.form.controls.firstNames.value ?? ''),
          lastNames: String(person?.lastName ?? person?.LastName ?? this.form.controls.lastNames.value ?? ''),
          countryId: String(countryId ?? this.form.controls.countryId.value ?? ''),
          provinceId: String(provinceId ?? this.form.controls.provinceId.value ?? ''),
          cityId: String(cityId ?? this.form.controls.cityId.value ?? ''),
          address: String(person?.address ?? person?.Address ?? this.form.controls.address.value ?? '')
        });

        this.loadLocationName('country', countryId, 'countryName');
        this.loadLocationName('province', provinceId, 'provinceName');
        this.loadLocationName('city', cityId, 'cityName');
      },
      error: () => {
        // Si falla la carga de Person, nos quedamos con lo que trajo Company.
      }
    });
  }

  private loadLocationName(
    resource: 'country' | 'province' | 'city',
    id: number | string | null | undefined,
    controlName: 'countryName' | 'provinceName' | 'cityName'
  ): void {
    const numericId = Number(id);
    if (!Number.isFinite(numericId) || numericId <= 0) {
      return;
    }

    this.http.get<any>(`${this.apiBaseUrl}/api/general/${resource}/${numericId}`).pipe(
      catchError(() => of(null))
    ).subscribe((response) => {
      const entity = response?.result ?? response;
      const name = String(entity?.name ?? entity?.Name ?? '').trim();
      if (name) {
        this.form.patchValue({ [controlName]: name } as any);
      }
    });
  }

  private buildPayload(): Record<string, unknown> {
    const value = this.form.getRawValue();

    return {
      identificationTypeId: value.identificationTypeId,
      identificacion: value.identificationNumber,
      razonSocial: this.isRuc ? value.businessName : '',
      nombreComercial: this.isRuc ? value.tradeName : '',
      nombres: this.isRuc ? '' : value.firstNames,
      apellidos: this.isRuc ? '' : value.lastNames,
      countryId: value.countryId || null,
      provinceId: value.provinceId || null,
      cityId: value.cityId || null,
      companyTypeId: value.companyTypeId || null,
      direccion: value.address || '',
      address: value.address || '',
      legalRepresentativeIdentification: value.legalRepresentativeIdentification,
      legalRepresentativeName: value.legalRepresentativeName,
      accountantIdentification: value.accountantIdentification,
      accountantName: value.accountantName,
      logoBase64: this.logoPreview || '',
      branches: this.draftBranches.map((x) => ({
        code: x.code,
        name: x.name,
        email: x.email || null,
        phone: x.phone || null,
        address: x.address || null
      }))
    };
  }

  onDraftBranchesChange(rows: BranchInlineRow[]): void {
    this.draftBranches = rows;
  }

  onEditBranchSaved(row: BranchInlineRow): void {
    // En modo crear: no permitir guardar sucursales individuales hasta guardar la empresa
    if (this.mode === 'add' && (!this.companyId || this.companyId <= 0)) {
      this.footerTooltip = 'Debe guardar la empresa primero antes de agregar sucursales.';
      return;
    }

    // En modo editar: validar que exista un companyId
    if (this.mode === 'edit' && (!this.companyId || this.companyId <= 0)) {
      this.footerTooltip = 'No se pudo determinar la empresa para esta sucursal.';
      return;
    }

    if (row.id < 0) {
      this.http.post<any>(`${this.apiBaseUrl}/api/general/branch`, {
        companyId: this.companyId,
        code: row.code,
        name: row.name,
        email: row.email || null,
        phone: row.phone || null,
        address: row.address || null
      }).subscribe({
        next: (response) => {
          const saved = response?.result ?? response;
          const newId = Number(saved?.id ?? saved?.Id ?? 0);
          if (newId > 0) {
            const idx = this.draftBranches.findIndex((x) => x.id === row.id);
            if (idx !== -1) {
              this.draftBranches = [
                ...this.draftBranches.slice(0, idx),
                { ...this.draftBranches[idx], id: newId },
                ...this.draftBranches.slice(idx + 1)
              ];
            }
          }
        },
        error: () => {
          this.footerTooltip = 'No se pudo guardar la nueva sucursal.';
        }
      });
    } else {
      this.http.put<any>(`${this.apiBaseUrl}/api/general/branch/${row.id}`, {
        companyId: this.companyId,
        code: row.code,
        name: row.name,
        email: row.email || null,
        phone: row.phone || null,
        address: row.address || null
      }).subscribe({
        error: () => {
          this.footerTooltip = 'No se pudo actualizar la sucursal.';
        }
      });
    }
  }

  onEditBranchDeleted(id: number): void {
    if (id > 0) {
      this.http.delete(`${this.apiBaseUrl}/api/general/branch/${id}`).subscribe({
        error: () => {
          this.footerTooltip = 'No se pudo eliminar la sucursal.';
        }
      });
    }
  }

  private async validateMainIdentification(options: { silentSuccessMessage: boolean }): Promise<boolean> {
    const control = this.form.controls.identificationNumber;
    const normalized = this.normalizeIdentification(control.value);

    if (!normalized) {
      control.setErrors({ required: true });
      this.footerTooltip = 'Ingrese la identificacion principal para validar.';
      return false;
    }

    this.validatingMainIdentification = true;
    try {
      control.setValue(normalized);

      if (this.containsLetters(normalized)) {
        this.setIdentificationTypeByCode('PAS');
        control.setErrors(null);
        if (!options.silentSuccessMessage) {
          this.footerTooltip = 'Se detecto Pasaporte. No requiere validacion ecuatoriana.';
        }
        return true;
      }

      if (!this.isOnlyDigits(normalized)) {
        control.setErrors({ invalidIdentification: true });
        this.footerTooltip = 'La identificacion contiene caracteres invalidos.';
        return false;
      }

      if (normalized.length === 10) {
        if (!this.isValidEcuadorCedula(normalized)) {
          control.setErrors({ invalidCedula: true });
          this.footerTooltip = 'Cedula invalida.';
          return false;
        }

        this.setIdentificationTypeByCode('CED');
        control.setErrors(null);
        if (!options.silentSuccessMessage) {
          this.footerTooltip = 'Identificacion valida como Cedula.';
        }
        return true;
      }

      if (normalized.length === 13) {
        if (!this.isValidEcuadorRuc(normalized)) {
          control.setErrors({ invalidRuc: true });
          this.footerTooltip = 'RUC invalido.';
          return false;
        }

        const isDuplicate = await this.existsCompanyByRuc(normalized);
        if (isDuplicate) {
          control.setErrors({ duplicatedRuc: true });
          this.footerTooltip = 'Ya existe una empresa con este RUC.';
          return false;
        }

        this.setIdentificationTypeByCode('RUC');
        control.setErrors(null);
        if (!options.silentSuccessMessage) {
          this.footerTooltip = 'Identificacion valida como RUC.';
        }
        return true;
      }

      control.setErrors({ invalidLength: true });
      this.footerTooltip = 'La identificacion debe tener 10 (Cedula) o 13 (RUC) digitos.';
      return false;
    } finally {
      this.validatingMainIdentification = false;
    }
  }

  private async validateSupportIdentification(
    controlName: 'legalRepresentativeIdentification' | 'accountantIdentification',
    options: {
      loadingSetter: (value: boolean) => void;
      silentSuccessMessage: boolean;
      fieldName: string;
    }
  ): Promise<boolean> {
    const control = this.form.controls[controlName];
    const normalized = this.normalizeIdentification(control.value);

    if (!normalized) {
      control.setErrors(null);
      return true;
    }

    options.loadingSetter(true);
    try {
      control.setValue(normalized);

      if (!this.isOnlyDigits(normalized)) {
        control.setErrors({ invalidIdentification: true });
        this.footerTooltip = `La ${options.fieldName} solo permite Cedula o RUC validos.`;
        return false;
      }

      const isCedula = normalized.length === 10 && this.isValidEcuadorCedula(normalized);
      const isRuc = normalized.length === 13 && this.isValidEcuadorRuc(normalized);

      if (!isCedula && !isRuc) {
        control.setErrors({ invalidIdentification: true });
        this.footerTooltip = `La ${options.fieldName} solo permite Cedula o RUC validos.`;
        return false;
      }

      control.setErrors(null);
      if (!options.silentSuccessMessage) {
        this.footerTooltip = 'Identificacion valida.';
      }
      return true;
    } finally {
      options.loadingSetter(false);
    }
  }

  private normalizeIdentification(value: string | null | undefined): string {
    return String(value ?? '')
      .trim()
      .replace(/[\s-]/g, '')
      .toUpperCase();
  }

  private containsLetters(value: string): boolean {
    return /[A-Z]/.test(value);
  }

  private isOnlyDigits(value: string): boolean {
    return /^\d+$/.test(value);
  }

  private setIdentificationTypeByCode(code: 'CED' | 'RUC' | 'PAS'): void {
    if (!this.identificationTypes.length) {
      this.identificationTypes = this.getDefaultIdentificationTypes();
    }

    const target = this.identificationTypes.find((x) => x.code.trim().toUpperCase() === code);
    if (!target) {
      return;
    }

    this.form.controls.identificationTypeId.setValue(target.id);
    this.form.controls.identificationTypeId.updateValueAndValidity({ emitEvent: false });
  }

  private getDefaultIdentificationTypes(): OptionItem[] {
    return [
      { id: '1', code: 'CED', name: 'Cedula' },
      { id: '2', code: 'RUC', name: 'Ruc' },
      { id: '3', code: 'PAS', name: 'Pasaporte' }
    ];
  }

  private isValidEcuadorCedula(cedula: string): boolean {
    if (!/^\d{10}$/.test(cedula)) {
      return false;
    }

    const province = Number(cedula.substring(0, 2));
    const thirdDigit = Number(cedula[2]);
    if (province < 1 || province > 24 || thirdDigit >= 6) {
      return false;
    }

    const digits = cedula.split('').map((x) => Number(x));
    let sum = 0;

    for (let i = 0; i < 9; i++) {
      let value = digits[i];
      if (i % 2 === 0) {
        value *= 2;
        if (value > 9) {
          value -= 9;
        }
      }
      sum += value;
    }

    const verifier = (10 - (sum % 10)) % 10;
    return verifier === digits[9];
  }

  private isValidEcuadorRuc(ruc: string): boolean {
    if (!/^\d{13}$/.test(ruc)) {
      return false;
    }

    const suffix = ruc.substring(10);
    if (suffix === '000') {
      return false;
    }

    const thirdDigit = Number(ruc[2]);
    const province = Number(ruc.substring(0, 2));
    if (province < 1 || province > 24) {
      return false;
    }

    if (thirdDigit <= 5) {
      return this.isValidEcuadorCedula(ruc.substring(0, 10));
    }

    if (thirdDigit === 6) {
      const factors = [3, 2, 7, 6, 5, 4, 3, 2];
      const digits = ruc.split('').map((x) => Number(x));
      const total = factors.reduce((acc, factor, index) => acc + (digits[index] * factor), 0);
      const verifier = 11 - (total % 11);
      const expected = verifier === 11 ? 0 : verifier;
      return expected === digits[8];
    }

    if (thirdDigit === 9) {
      const factors = [4, 3, 2, 7, 6, 5, 4, 3, 2];
      const digits = ruc.split('').map((x) => Number(x));
      const total = factors.reduce((acc, factor, index) => acc + (digits[index] * factor), 0);
      const verifier = 11 - (total % 11);
      const expected = verifier === 11 ? 0 : verifier;
      return expected === digits[9];
    }

    return false;
  }

  private async existsCompanyByRuc(ruc: string): Promise<boolean> {
    const request = {
      skip: 0,
      take: 20,
      requiresCounts: true,
      sorted: [],
      search: [],
      where: [
        {
          field: 'Identificacion',
          operator: 'equal',
          value: ruc,
          ignoreCase: false
        }
      ],
      aggregates: []
    };

    try {
      const response = await firstValueFrom(this.http.post<any>(`${this.apiBaseUrl}/api/general/company/filter`, request));
      const list = Array.isArray(response) ? response : (response?.result ?? []);
      return (list ?? []).some((x: any) => Number(x?.id ?? x?.Id ?? 0) !== (this.companyId ?? 0));
    } catch {
      this.footerTooltip = 'No se pudo validar duplicado de RUC. Intente nuevamente.';
      return true;
    }
  }

  private extractApiErrorMessage(error: any, fallback: string): string {
    const response = error?.error;

    const directMessage = String(response?.message ?? '').trim();
    if (directMessage) {
      const traceId = String(response?.traceId ?? '').trim();
      return traceId ? `${directMessage} (TraceId: ${traceId})` : directMessage;
    }

    const title = String(response?.title ?? '').trim();
    if (title) {
      const errors = response?.errors;
      if (errors && typeof errors === 'object') {
        const firstKey = Object.keys(errors)[0];
        const firstValue = firstKey ? errors[firstKey] : null;
        const firstError = Array.isArray(firstValue) ? String(firstValue[0] ?? '').trim() : String(firstValue ?? '').trim();
        if (firstError) {
          return `${title}: ${firstError}`;
        }
      }
      return title;
    }

    const statusText = String(error?.statusText ?? '').trim();
    if (statusText) {
      return `${fallback} (${statusText})`;
    }

    return fallback;
  }
}
