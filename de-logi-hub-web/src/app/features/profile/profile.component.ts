import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserProfileInfo } from '../../core/api/models';
import { UsersService } from '../../core/api/services';
import { ToastrService } from 'ngx-toastr';
import { StorageService } from '../../core/services/storage.service';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
	profileForm: FormGroup;
	userProfile: UserProfileInfo | null = null;
	loading = false;
	imagePreview: string | null = null;

	constructor(
		private fb: FormBuilder,
		private usersService: UsersService,
		private toastr: ToastrService,
		private storageService: StorageService
	) {
		this.profileForm = this.fb.group({
			id: ['', Validators.required],
			firstName: ['', [Validators.required, Validators.minLength(2)]],
			lastName: ['', [Validators.required, Validators.minLength(2)]],
			email: ['', [Validators.required, Validators.email]],
			phoneNumber: [
				'',
				[Validators.required, Validators.pattern(/^\+380\d{9}$/)],
			],
			birthDate: [''],
			address: [''],
			profilePicture: [null],
		});
	}

	ngOnInit(): void {
		this.loadUserProfile();
	}

	loadUserProfile(): void {
		this.usersService.getUserProfileById({ id: this.storageService.getUserId() }).subscribe({
			next: (profile) => {
				this.userProfile = profile;
				this.profileForm.patchValue(profile);
				this.imagePreview = profile.profilePicture || null;
			},
			error: () => {
				this.toastr.error('Помилка завантаження профілю', 'Помилка');
			},
		});
	}
	
	onSubmit(): void {
		if (this.profileForm.invalid) return;

		this.loading = true;
		const updatedProfile: UserProfileInfo = this.profileForm.value;

		this.usersService.updateUserProfile({ body: updatedProfile }).subscribe({
			next: () => {
				this.toastr.success('Профіль оновлено', 'Успіх');
				this.loadUserProfile();
			},
			error: () => {
				this.toastr.error('Помилка оновлення профілю', 'Помилка');
			},
			complete: () => (this.loading = false),
		});
	}
}
