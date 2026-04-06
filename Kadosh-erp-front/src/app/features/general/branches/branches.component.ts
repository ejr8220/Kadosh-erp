import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { GridColumn } from '../../../shared/grid-shared/grid-column.model';
import { GridSharedComponent } from '../../../shared/grid-shared/grid-shared.component';

@Component({
  selector: 'app-branches',
  standalone: true,
  imports: [CommonModule, GridSharedComponent],
  templateUrl: './branches.component.html',
  styleUrl: './branches.component.scss'
})
export class BranchesComponent {
  title = 'Sucursales';

  columns: GridColumn[] = [
    { field: 'id', headerText: 'Id', width: 90, textAlign: 'Right', type: 'number' },
    { field: 'code', headerText: 'Codigo', width: 140 },
    { field: 'name', headerText: 'Nombre', width: 220 },
    { field: 'email', headerText: 'Email', width: 220 },
    { field: 'status', headerText: 'Estado', width: 120 }
  ];
}
