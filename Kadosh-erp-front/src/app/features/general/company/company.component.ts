import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GridColumn } from '../../../shared/grid-shared/grid-column.model';
import { GridSharedComponent } from '../../../shared/grid-shared/grid-shared.component';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [CommonModule, GridSharedComponent],
  templateUrl: './company.component.html',
  styleUrl: './company.component.scss'
})
export class CompanyComponent {
  title = 'Empresa';

  columns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, textAlign: 'Right', type: 'number', hidden: true },
    { field: 'identificacion', headerText: 'Identificacion', width: 170 },
    { field: 'razonSocial', headerText: 'Razon social', width: 220 },
    { field: 'nombreComercial', headerText: 'Nombre comercial', width: 220 },
    { field: 'pais', headerText: 'Pais', width: 150 },
    { field: 'direccion', headerText: 'Direccion', width: 220 },
    { field: 'contactForms', headerText: 'Formas de contacto', width: 320 },
    { field: 'status', headerText: 'Estado', width: 120 }
  ];
}
