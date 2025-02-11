import { Component } from '@angular/core';
import { OffersService } from '../../core/api/services';
import { ToastrService } from 'ngx-toastr';
import { StorageService } from '../../core/services/storage.service';

@Component({
	selector: 'app-carrier-offers',
	templateUrl: './carrier-offers.component.html',
	styleUrl: './carrier-offers.component.scss',
})
export class CarrierOffersComponent {
	offers: any[] = [];
	loading = false;
	carrierId: string | undefined = undefined;

	constructor(
		private offersService: OffersService,
		private storageService: StorageService,
		private toastr: ToastrService
	) {}

	ngOnInit(): void {
		this.carrierId = this.storageService.getUserId();
		if (this.carrierId) {
			this.loadOffers();
		} else {
			this.toastr.error('Помилка отримання ID користувача', 'Помилка');
		}
	}

	loadOffers(): void {
		this.loading = true;
		this.offersService.getOffersByCarrier({ carrierId: this.carrierId })
			.subscribe({
				next: (offers) => {
					this.offers = offers;
				},
				error: () => {
					this.toastr.error('Помилка завантаження пропозицій', 'Помилка');
				},
				complete: () => {
					this.loading = false;
				}
			});
	}

	deleteOffer(offerId: string): void {
		this.offersService.deleteOffer({ id: offerId })
			.subscribe({
				next: () => {
					this.toastr.success('Пропозицію видалено', 'Успіх');
					this.loadOffers();
				},
				error: () => {
					this.toastr.error('Помилка видалення пропозиції', 'Помилка');
				},
			});
	}

	getOfferStatusDisplayValue(status: string): string {
		const statusMap: { [key: string]: string } = {
			Pending: 'Очікує',
			Accepted: 'Прийнято',
			Rejected: 'Відхилено',
		};
		return statusMap[status] || 'Невідомий';
	}
}
