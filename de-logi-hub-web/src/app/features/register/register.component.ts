import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsersService } from '../../core/api/services';
import { RegisterUserModel } from '../../core/api/models';
import { ToastrService } from 'ngx-toastr';
import { v4 as guid } from 'uuid';

@Component({
	selector: 'app-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
	registerForm!: FormGroup;
	loading = false;
	errorMessage = '';

	constructor(
		private fb: FormBuilder,
		private userService: UsersService,
		private toastr: ToastrService
	) {}

	ngOnInit(): void {
		this.registerForm = this.fb.group({
			login: ['', [Validators.required, Validators.minLength(3)]],
			password: ['', [Validators.required, Validators.minLength(6)]],
			firstName: ['', [Validators.required, Validators.minLength(2)]],
			lastName: ['', [Validators.required, Validators.minLength(2)]],
			address: ['', [Validators.required]],
			phoneNumber: [
				'',
				[Validators.required, Validators.pattern(/^\+?[0-9]{10,}$/)],
			],
			birthDate: ['', [Validators.required]],
			email: ['', [Validators.required, Validators.email]],
			role: ['Customer', [Validators.required]],
		});
	}

	get f() {
		return this.registerForm.controls;
	}

	onSubmit(): void {
		if (this.registerForm.invalid) {
			this.toastr.error('Заповніть всі обов’язкові поля', 'Помилка');
			return;
		}
		this.loading = true;
		const userData: RegisterUserModel = { ...this.registerForm.value };
		userData.userId = guid();
		this.userService.register({ body: userData }).subscribe({
			next: () => {
				this.toastr.success('Ви успішно зареєстровані!', 'Успіх');
				this.loading = false;
				this.registerForm.reset();
			},
			error: (error) => {
				console.log(error);
				this.toastr.error(
					error.error.message || 'Помилка реєстрації!',
					'Помилка'
				);
				this.loading = false;
			},
		});
	}
}
