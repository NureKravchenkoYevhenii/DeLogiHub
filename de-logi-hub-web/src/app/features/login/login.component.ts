import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../core/api/services';
import { StorageService } from '../../core/services/storage.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
	loginForm: FormGroup;
	loading = false;

	constructor(
		private fb: FormBuilder,
		private toastr: ToastrService,
		private authService: AuthService,
		private storageService: StorageService
	) {
		this.loginForm = this.fb.group({
			login: ['', [Validators.required]],
			password: ['', [Validators.required, Validators.minLength(6)]],
		});
	}

	get f() {
		return this.loginForm.controls;
	}

	onSubmit(): void {
		if (this.loginForm.invalid) {
			return;
		}

		this.loading = true;
		const credentials = { ...this.loginForm.value };

		this.authService.login({body: credentials }).subscribe({
			next: (token) => {
				this.storageService.saveToken(token);
				this.toastr.success('Вхід виконано успішно!', 'Успіх');
				this.loading = false;
			},
			error: (error) => {
				this.toastr.error(error.error.message || 'Помилка входу!', 'Помилка');
				this.loading = false;
			},
		});
	}
}
