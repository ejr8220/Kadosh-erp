import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { finalize } from 'rxjs';
import { AuthApiService } from '../../../core/services/auth-api.service';
import { SessionService } from '../../../core/services/session.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.nonNullable.group({
    user: ['', Validators.required],
    password: ['', Validators.required]
  });

  hidePassword = true;
  loading = false;
  errorMessage = '';

  constructor(
    private readonly router: Router,
    private readonly session: SessionService,
    private readonly authApi: AuthApiService
  ) {}

  login(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.errorMessage = '';
    this.loading = true;

    const value = this.form.getRawValue();
    this.authApi
      .login(value.user, value.password)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe({
        next: (state) => {
          this.session.startSession(state);
          this.router.navigateByUrl('/admin/general/parameters');
        },
        error: (error) => {
          this.errorMessage = error?.error?.message ?? 'No se pudo iniciar sesion.';
        }
      });
  }
}
