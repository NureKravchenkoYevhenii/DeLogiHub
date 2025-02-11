import { Injectable } from '@angular/core';
import { Token } from '../api/models';
import { JwtHelperService } from '@auth0/angular-jwt';

const USER_TOKEN = 'auth-user';

@Injectable({
	providedIn: 'root',
})
export class StorageService {
	constructor(
		private jwtHelper:  JwtHelperService
	) {}

	public clear(): void {
		window.sessionStorage.clear();
	}

	public saveToken(data: Token): void {
		window.sessionStorage.removeItem(USER_TOKEN);
		window.sessionStorage.setItem(USER_TOKEN, JSON.stringify(data));
	}

	public getToken(): Token {
		const data = window.sessionStorage.getItem(USER_TOKEN);
		if (data) {
			return JSON.parse(data) as Token;
		}

		return {
			accessToken: '',
			refreshToken: '',
		} as Token;
	}

	public isLoggedIn(): boolean {
		var user = window.sessionStorage.getItem(USER_TOKEN);
		if (user) {
			return true;
		}

		return false;
	}

	public getUserId(): string {
		const token = window.sessionStorage.getItem(USER_TOKEN);
		if (token) {
			const decode = this.jwtHelper.decodeToken(token);
			const userId = decode['id'];
			return userId;
		}
		return '';
	}

	public getUserRole(): string {
		var user = window.sessionStorage.getItem(USER_TOKEN);
        var token;
        if (user != null) {
            token = JSON.parse(user)
        }

        var accessToken = token['accessToken'];
        var decode = this.jwtHelper.decodeToken(accessToken);
        var currentUserRole = decode['role'];

        return currentUserRole;
	}
}

export function getAccessToken(): string {
	const data = window.sessionStorage.getItem(USER_TOKEN);
	if (data) {
		return (JSON.parse(data) as Token).accessToken ?? '';
	}
	return '';
}
