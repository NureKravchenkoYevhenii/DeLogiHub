import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { TransportsService } from '../../core/api/services';
import { TransportModel } from '../../core/api/models';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TransportModalComponent } from '../transport-modal/transport-modal.component';
import { StorageService } from '../../core/services/storage.service';
import { transportTypes } from '../../core/helpers/enum-helper';

@Component({
	selector: 'app-transports',
	templateUrl: './transports.component.html',
	styleUrls: ['./transports.component.scss'],
})
export class TransportsComponent implements OnInit {
	transports: TransportModel[] = [];

	constructor(
		private transportsService: TransportsService,
		private toastr: ToastrService,
		private modalService: NgbModal,
		private storageService: StorageService
	) {}

	ngOnInit(): void {
		this.loadTransports();
	}

	loadTransports(): void {
		this.transportsService
			.getTransportsByCarrier({ carrierId: this.storageService.getUserId() })
			.subscribe({
				next: (data) => {
					this.transports = data;
				},
				error: () => {
					this.toastr.error(
						'Помилка завантаження транспортних засобів',
						'Помилка'
					);
				},
		});
	}

	openTransportModal(id?: string | undefined): void {
		const modalRef = this.modalService.open(TransportModalComponent, {
			size: 'lg',
			backdrop: 'static',
		});
		modalRef.componentInstance.transportId = id;
		modalRef.result.then(
			(result) => {
				if (result) this.loadTransports();
			},
			() => {}
		);
	}

	deleteTransport(id: string | undefined): void {
		this.transportsService.deleteTransport({ id }).subscribe({
			next: () => {
				this.toastr.success('Транспорт успішно видалено', 'Успіх');
				this.loadTransports();
			},
			error: (error) => {
				this.toastr.error(
					error.error.message || 'Не вдалося видалити транспорт',
					'Помилка'
				);
			},
		});
	}

	getTransportTypeDisplayValue(value: string | undefined): string {
		return transportTypes.find(x => x.value === value)?.displayValue ?? ''
	}
}
