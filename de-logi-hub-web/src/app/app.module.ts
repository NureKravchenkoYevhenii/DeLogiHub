import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { httpInterceptorProviders } from './core/interceptors/http-interceptor.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MainLayoutComponent } from './core/layout/main-layout/main-layout.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RegisterComponent } from './features/register/register.component';
import { LoginComponent } from './features/login/login.component';
import { TransportsComponent } from './features/transports/transports.component';
import { TransportModalComponent } from './features/transport-modal/transport-modal.component';
import { getAccessToken } from './core/services/storage.service';
import { ProfileComponent } from './features/profile/profile.component';
import { OrdersComponent } from './features/orders/orders.component';
import { OrderModalComponent } from './features/order-modal/order-modal.component';
import { CarrierOffersComponent } from './features/carrier-offers/carrier-offers.component';
import { OrderDetailsComponent } from './features/order-details/order-details.component';

@NgModule({
	declarations: [
		AppComponent,
		MainLayoutComponent,
		RegisterComponent,
		LoginComponent,
		TransportsComponent,
		TransportModalComponent,
		ProfileComponent,
		OrdersComponent,
		OrderModalComponent,
		CarrierOffersComponent,
		OrderDetailsComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		JwtModule,
		NgbModule,
		FormsModule,
		ReactiveFormsModule,
		BrowserAnimationsModule,
		ToastrModule.forRoot({
			positionClass: 'toast-bottom-right',
			closeButton: true,
			progressBar: true,
		}),
		JwtModule.forRoot({
			config: {
				tokenGetter: () => getAccessToken()
			},
		}),
	],
	providers: [
		httpInterceptorProviders,
		provideHttpClient(withInterceptorsFromDi()),
		JwtHelperService
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
