import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GridColumn } from '../../../shared/grid-shared/grid-column.model';
import { GridSharedComponent } from '../../../shared/grid-shared/grid-shared.component';

@Component({
  selector: 'app-parameters',
  standalone: true,
  imports: [CommonModule, GridSharedComponent],
  templateUrl: './parameters.component.html',
  styleUrl: './parameters.component.scss'
})
export class ParametersComponent {
  title = 'Parametros';

  columns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, textAlign: 'Right', type: 'number' },
    { field: 'code', headerText: 'Codigo', width: 150 },
    { field: 'name', headerText: 'Nombre', width: 220 },
    { field: 'scope', headerText: 'Ambito', width: 140 },
    { field: 'status', headerText: 'Estado', width: 120 }
  ];
}
