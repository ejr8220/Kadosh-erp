import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { ToolbarModule } from '@syncfusion/ej2-angular-navigations';

@Component({
  selector: 'app-form-shared',
  standalone: true,
  imports: [CommonModule, ToolbarModule, ButtonModule],
  templateUrl: './form-shared.component.html',
  styleUrl: './form-shared.component.scss'
})
export class FormSharedComponent {
  @Input() title = 'Formulario';
  @Input() showBack = true;
  @Input() showSave = true;
  @Input() showPrint = true;
  @Input() footerTooltip = 'Seleccione un campo para ver ayuda contextual.';

  @Output() back = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();
  @Output() print = new EventEmitter<void>();

  onBack(): void {
    this.back.emit();
  }

  onSave(): void {
    this.save.emit();
  }

  onPrint(): void {
    this.print.emit();
  }
}
