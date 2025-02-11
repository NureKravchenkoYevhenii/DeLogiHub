import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { OffersService, OrdersService } from '../../core/api/services';
import { StorageService } from '../../core/services/storage.service';
import { ToastrService } from 'ngx-toastr';
import { v4 as guid } from 'uuid';
import { OfferModel, OfferStatus } from '../../core/api/models';

@Component({
	selector: 'app-order-details',
	templateUrl: './order-details.component.html',
	styleUrl: './order-details.component.scss',
})
export class OrderDetailsComponent {
	@Input() orderId?: string;
	orderForm: FormGroup;
	loading = false;
	isEdit = false;

	constructor(
		public activeModal: NgbActiveModal,
		private fb: FormBuilder,
		private ordersService: OrdersService,
		private storageService: StorageService,
		private offersService: OffersService,
		private toastr: ToastrService
	) {
		this.orderForm = this.fb.group({
			id: ['', Validators.required],
			customerId: ['', Validators.required],
			details: ['', [Validators.required, Validators.minLength(5)]],
		});
	}

	ngOnInit(): void {
		if (this.orderId) {
			this.loadOrder();
			this.isEdit = true;
		} else {
			this.orderForm.patchValue({
				id: guid(),
				customerId: this.storageService.getUserId(),
			});
		}
	}

	loadOrder(): void {
		this.loading = true;
		this.ordersService.getOrder({ id: this.orderId }).subscribe({
			next: (order) => {
				this.orderForm.patchValue(order);
			},
			error: () => {
				this.toastr.error('Помилка завантаження замовлення', 'Помилка');
			},
			complete: () => {
				this.loading = false;
			},
		});
	}

	makeOffer(): void {
		this.loading = true;
		const offerModel: OfferModel = {
			id: guid(),
			carrierId: this.storageService.getUserId(),
			orderId: this.orderId,
			status: OfferStatus.Pending,
		};
		this.offersService.addOffer({ body: offerModel }).subscribe({
			next: () => {
				this.toastr.success('Пропозицію успішно надано', 'Успіх');
				this.activeModal.close(true);
			},
			error: (error) => {
				this.toastr.error(
					error.error.message || 'Помилка надання пропозиції',
					'Помилка'
				);
			},
			complete: () => {
				this.loading = false;
			},
		});
	}

	dismiss(): void {
		this.activeModal.dismiss();
	}
}
