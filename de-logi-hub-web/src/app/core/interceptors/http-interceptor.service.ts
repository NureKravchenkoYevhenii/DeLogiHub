import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { StorageService } from "../services/storage.service";
import { Router } from "@angular/router";
import { Observable, catchError, switchMap, throwError } from "rxjs";
import { AuthService } from "../api/services";
import { Token } from "../api/models";

@Injectable({
	providedIn: 'root'
})

export class HttpRequestInterceptor implements HttpInterceptor {
	private isRefreshing = false;

	constructor(
		private storageService: StorageService,
		private authService: AuthService,
		private router: Router,
	) { }

	intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		var token: Token = this.storageService.getToken();
		if (!token) {
			return next.handle(req);
		}

		req = req.clone({
			headers: req.headers
				.set('Authorization', `Bearer ${token.accessToken}`)
				//.set('Accept-Language', this.storageService.getLocale() == undefined ? "uk-UA" : this.storageService.getLocale()!),
		});

		return next.handle(req).pipe(
			catchError((error) => {
				if (error instanceof HttpErrorResponse &&
					error.status === 401
				) {
					return this.handle401Error(req, next);
				}
				return throwError(() => error);
			})
		);
	}

	private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
		if (!this.isRefreshing) {
			this.isRefreshing = true;

			if (this.storageService.isLoggedIn()) {
				var token: Token = this.storageService.getToken();

				return this.authService.refresh({ refreshTokenString: token.refreshToken as string }).pipe(
					switchMap((result) => {
						this.isRefreshing = false;
						this.storageService.saveToken(result);

					return next.handle(request.clone({
						headers: request.headers.delete('Authorization')
							.set('Authorization', `Bearer ${result.accessToken}`),
					}));
				}),
				catchError((error) => {
					this.isRefreshing = false;

					if (error.status == '403') {
						this.storageService.clear();
						this.router.navigate(['login']);
					}

					return throwError(() => error);
				}));
			}
		}

		return next.handle(request);
	}
}

export const httpInterceptorProviders = [
	{
		provide: HTTP_INTERCEPTORS,
		useClass: HttpRequestInterceptor,
		multi: true
	},
];
