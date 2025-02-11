import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './features/register/register.component';
import { LoginComponent } from './features/login/login.component';
import { TransportsComponent } from './features/transports/transports.component';
import { ProfileComponent } from './features/profile/profile.component';
import { OrdersComponent } from './features/orders/orders.component';
import { CarrierOffersComponent } from './features/carrier-offers/carrier-offers.component';

const routes: Routes = [
	{ path: 'register', component: RegisterComponent },
	{ path: 'login', component: LoginComponent },
	{ path: 'transports', component: TransportsComponent },
	{ path: 'profile', component: ProfileComponent },
	{ path: 'orders', component: OrdersComponent },
	{ path: 'carrier-offers', component: CarrierOffersComponent }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
