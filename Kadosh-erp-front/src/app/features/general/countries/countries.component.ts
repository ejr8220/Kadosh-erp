import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GridSharedComponent } from '../../../shared/grid-shared/grid-shared.component';
import { GridColumn } from '../../../shared/grid-shared/grid-column.model';

@Component({
  selector: 'app-countries',
  standalone: true,
  imports: [CommonModule, GridSharedComponent],
  templateUrl: './countries.component.html',
  styleUrl: './countries.component.scss'
})
export class CountriesComponent {
  title = 'Paises';

  columns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, textAlign: 'Right', type: 'number', hidden: true },
    { field: 'name', headerText: 'Nombre', width: 260 },
    { field: 'isoCode', headerText: 'Iso', width: 120 },
    { field: 'status', headerText: 'Estado', width: 120 }
  ];
}
