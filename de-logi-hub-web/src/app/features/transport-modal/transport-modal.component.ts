import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TransportsService } from '../../core/api/services';
import { ToastrService } from 'ngx-toastr';
import { v4 as guid } from 'uuid';
import { TransportModel } from '../../core/api/models';
import { StorageService } from '../../core/services/storage.service';
import { transportTypes } from '../../core/helpers/enum-helper';

@Component({
	selector: 'app-transport-modal',
	templateUrl: './transport-modal.component.html',
	styleUrl: './transport-modal.component.scss',
})
export class TransportModalComponent implements OnInit {
	@Input() transportId: string | undefined = undefined;
	transportForm: FormGroup;
	isEdit = false;
	loading = false;
	transportTypes = transportTypes

	constructor(
		public activeModal: NgbActiveModal,
		private fb: FormBuilder,
		private transportsService: TransportsService,
		private toastr: ToastrService,
		private storageService: StorageService
	) {
		this.transportForm = this.fb.group({
			id: ['', Validators.required],
			type: ['', Validators.required],
			licensePlate: [''],
			capacity: ['', [Validators.required, Validators.min(0.1)]],
			carrierId: ['', Validators.required]
		});
	}

	ngOnInit(): void {
		console.log(this.storageService.getUserId());
		if (this.transportId) {
			this.isEdit = true;
			this.loadTransportData();
		} else {
			this.transportForm.patchValue(
				{ 
					id: guid(),
					carrierId: this.storageService.getUserId()
				},
			)
		}
	}

	loadTransportData(): void {
		this.transportsService.getTransport({ id: this.transportId }).subscribe({
			next: (data) => {
				this.transportForm.patchValue(data);
			},
			error: () => {
				this.toastr.error('Помилка завантаження транспорту', 'Помилка');
				this.activeModal.dismiss();
			},
		});
	}

	onSubmit(): void {
		if (this.transportForm.invalid)
			return;

		this.loading = true;
		const transportData: TransportModel = this.transportForm.value;

		const request = this.isEdit
			? this.transportsService.updateTransport({ body: transportData })
			: this.transportsService.addTransport({ body: transportData });

		request.subscribe({
			next: () => {
				this.toastr.success(
					'Транспорт збережено',
					'Успіх'
				);
				this.activeModal.close(true);
			},
			error: () => {
				this.toastr.error(
					'Помилка збереження транспорту',
					'Помилка'
				);
			},
			complete: () => (this.loading = false),
		});
	}

	closeModal(): void {
		this.activeModal.dismiss();
	}
}
