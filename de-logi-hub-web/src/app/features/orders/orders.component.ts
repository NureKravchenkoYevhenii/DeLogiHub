import { Component } from '@angular/core';
import { OrderModel } from '../../core/api/models';
import { OrdersService } from '../../core/api/services';
import { ToastrService } from 'ngx-toastr';
import { StorageService } from '../../core/services/storage.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrderModalComponent } from '../order-modal/order-modal.component';
import { OrderDetailsComponent } from '../order-details/order-details.component';

@Component({
	selector: 'app-orders',
	templateUrl: './orders.component.html',
	styleUrl: './orders.component.scss',
})
export class OrdersComponent {
	orders: OrderModel[] = [];
	userOrders: OrderModel[] = [];
	userRole: string | null = null;
	userId: string | null = null;
	loading = false;

	constructor(
		private ordersService: OrdersService,
		private storageService: StorageService,
		private toastr: ToastrService,
		private modalService: NgbModal
	) {}

	ngOnInit(): void {
		this.userRole = this.storageService.getUserRole();
		this.userId = this.storageService.getUserId();

		if (this.userRole === 'Customer') {
			this.loadUserOrders();
		} else {
			this.loadAllOrders();
		}
	}

	loadAllOrders(): void {
		this.loading = true;
		this.ordersService.getOrders().subscribe({
			next: (orders) => {
				this.orders = orders;
			},
			error: () => {
				this.toastr.error('Помилка завантаження замовлень', 'Помилка');
			},
			complete: () => {
				this.loading = false;
			}
		});
	}

	loadUserOrders(): void {
		if (!this.userId)
			return;

		this.loading = true;
		this.ordersService
			.getCustomerOrders({ customerId: this.userId })
			.subscribe({
				next: (orders) => {
					this.userOrders = orders;
				},
				error: () => {
					this.toastr.error(
						'Помилка завантаження ваших замовлень',
						'Помилка'
					);
				},
				complete: () => {
					this.loading = false;
				}
			});
	}

	deleteOrder(id: string | undefined): void {
		this.ordersService.deleteOrder({ id }).subscribe({
			next: () => {
				this.toastr.success('Замовлення успішно видалено', 'Успіх');
				this.loadUserOrders();
			},
			error: () => {
				this.toastr.error('Помилка видалення замовлення', 'Помилка');
			},
		});
	}

	openOrderModal(orderId?: string): void {
		const modalRef = this.modalService.open(OrderModalComponent, {
			size: 'lg',
			backdrop: 'static'
		});
		modalRef.componentInstance.orderId = orderId;
		modalRef.componentInstance.isViewMode = false;
		modalRef.result.then(
			(result) => {
				if (result) {
					this.loadUserOrders();
				}
			},
			() => {}
		);
	}

	viewOrder(orderId: string | undefined): void {
		const modalRef = this.modalService.open(OrderDetailsComponent, {
			size: 'lg',
			backdrop: 'static'
		});
		modalRef.componentInstance.orderId = orderId;
	}
}
