import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { OffersService, OrdersService } from '../../core/api/services';
import { ToastrService } from 'ngx-toastr';
import { v4 as guid } from 'uuid';
import { StorageService } from '../../core/services/storage.service';
import { OfferModel, OfferStatus } from '../../core/api/models';

@Component({
	selector: 'app-order-modal',
	templateUrl: './order-modal.component.html',
	styleUrl: './order-modal.component.scss',
})
export class OrderModalComponent implements OnInit {
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
				customerId: this.storageService.getUserId()
			});
		}
	}

	loadOrder(): void {
		this.loading = true;
		this.ordersService.getOrder({ id: this.orderId })
			.subscribe({
				next: (order) => {
					this.orderForm.patchValue(order);
				},
				error: () => {
					this.toastr.error('Помилка завантаження замовлення', 'Помилка');
				},
				complete: () => {
					this.loading = false;	
				}
			});
	}

	saveOrder(): void {
		if (this.orderForm.invalid)
			return;

		const orderData = this.orderForm.value;
		this.loading = true;

		const request = this.isEdit
			? this.ordersService.updateOrder({ body: orderData })
			: this.ordersService.addOrder({ body: orderData });

		request.subscribe({
			next: () => {
				this.toastr.success('Замовлення збережено', 'Успіх');
				this.activeModal.close(true);
			},
			error: () => {
				this.toastr.error('Помилка збереження замовлення', 'Помилка');
			},
			complete: () => {
				this.loading = false;
			}
		});
	}

	dismiss(): void {
		this.activeModal.dismiss();
	}
}
