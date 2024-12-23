import { HttpClient } from '@angular/common/http'
import { Injectable, NgZone } from '@angular/core'
import { Observable } from 'rxjs'
import { Router } from '@angular/router'
import { map } from 'rxjs/operators'
// Custom
import { BankHttpService } from 'src/app/features/expenses/banks/classes/services/bank-http.service'
import { BerthHttpService } from 'src/app/features/reservations/berths/classes/services/berth-http.service'
import { BoatTypeHttpService } from 'src/app/features/reservations/boatTypes/classes/services/boatType-http.service'
import { BoatUsageHttpService } from 'src/app/features/reservations/boatUsages/classes/services/boatUsage-http.service'
import { ChangePasswordViewModel } from 'src/app/features/users/classes/view-models/change-password-view-model'
import { CryptoService } from './crypto.service'
import { DexieService } from './dexie.service'
import { DocumentTypeHttpService } from 'src/app/features/expenses/documentTypes/classes/services/documentType-http.service'
import { DotNetVersion } from '../classes/dotnet-version'
import { HttpDataService } from './http-data.service'
import { PaymentMethodHttpService } from 'src/app/features/expenses/paymentMethods/classes/services/paymentMethod-http.service'
import { PaymentStatusHttpService } from 'src/app/features/reservations/paymentStatuses/classes/services/paymentStatus-http.service'
import { ResetPasswordViewModel } from 'src/app/features/users/classes/view-models/reset-password-view-model'
import { SessionStorageService } from './session-storage.service'
import { SupplierHttpService } from 'src/app/features/expenses/suppliers/classes/services/supplier-http.service'
import { TokenRequest } from '../classes/token-request'
import { environment } from '../../../environments/environment'

@Injectable({ providedIn: 'root' })

export class AccountService extends HttpDataService {

    //#region variables

    private apiUrl = environment.apiUrl
    private urlForgotPassword = this.apiUrl + '/account/forgotPassword'
    private urlResetPassword = this.apiUrl + '/account/resetPassword'
    private urlToken = this.apiUrl + '/auth/auth'

    //#endregion

    constructor(httpClient: HttpClient, private bankHttpService: BankHttpService, private berthHttpService: BerthHttpService, private boatTypeHttpService: BoatTypeHttpService, private boatUsageHttpService: BoatUsageHttpService, private cryptoService: CryptoService, private dexieService: DexieService, private documentTypeHttpService: DocumentTypeHttpService, private ngZone: NgZone, private paymentMethodHttpService: PaymentMethodHttpService, private paymentStatusHttpService: PaymentStatusHttpService, private router: Router, private sessionStorageService: SessionStorageService, private supplierHttpService: SupplierHttpService) {
        super(httpClient, environment.apiUrl)
    }

    //#region public methods

    public changePassword(formData: ChangePasswordViewModel): Observable<any> {
        return this.http.post<any>(environment.apiUrl + '/account/changePassword/', formData)
    }

    public clearSessionStorage(): void {
        this.sessionStorageService.deleteItems([
            { 'item': 'displayName', 'when': 'always' },
            { 'item': 'expiration', 'when': 'always' },
            { 'item': 'isAdmin', 'when': 'always' },
            { 'item': 'jwt', 'when': 'always' },
            { 'item': 'now', 'when': 'always' },
            { 'item': 'refreshToken', 'when': 'always' },
            { 'item': 'returnUrl', 'when': 'always' },
            { 'item': 'userId', 'when': 'always' },
            { 'item': 'isFirstFieldFocused', 'when': 'always' }
        ])
    }

    public forgotPassword(formData: any): Observable<any> {
        return this.http.post<any>(this.urlForgotPassword, formData)
    }

    public getNewRefreshToken(): Observable<any> {
        const token: TokenRequest = {
            userId: this.cryptoService.decrypt(this.sessionStorageService.getItem('userId')),
            password: null,
            grantType: 'refresh_token',
            refreshToken: sessionStorage.getItem('refreshToken')
        }
        return this.http.post<any>(this.urlToken, token).pipe(
            map(response => {
                if (response.token) {
                    this.setAuthSettings(response)
                }
                return <any>response
            })
        )
    }

    public login(userName: string, password: string): Observable<void> {
        const grantType = 'password'
        return this.http.post<any>(this.urlToken, { userName, password, grantType }).pipe(map(response => {
            this.setUserData(response)
            this.setDotNetVersion(response)
            this.setAuthSettings(response)
            this.populateDexieFromAPI()
            this.setSelectedYear()
            this.setUpcomingLeaseEndDays()
            this.clearConsole()
        }))
    }

    public logout(): void {
        this.clearUserTokens()
        this.navigateToLogin()
        this.clearSessionStorage()
    }

    public resetPassword(vm: ResetPasswordViewModel): Observable<any> {
        return this.http.post<any>(this.urlResetPassword, vm)
    }

    //#endregion

    //#region private methods

    private clearConsole(): void {
        console.clear()
    }

    private clearUserTokens(): void {
        const userId = this.cryptoService.decrypt(this.sessionStorageService.getItem('userId'))
        this.http.post<any>(this.apiUrl + '/auth/logout', { 'userId': userId }).subscribe(() => {
            // Nothing left to do
        })
    }

    private navigateToLogin(): void {
        this.ngZone.run(() => {
            this.router.navigate(['/'])
        })
    }

    private setAuthSettings(response: any): void {
        sessionStorage.setItem('expiration', response.expiration)
        sessionStorage.setItem('jwt', response.token)
        sessionStorage.setItem('refreshToken', response.refreshToken)
    }

    private populateDexieFromAPI(): void {
        // Expenses
        this.dexieService.populateTable('banks', this.bankHttpService)
        this.dexieService.populateTable('documentTypes', this.documentTypeHttpService)
        this.dexieService.populateTable('paymentMethods', this.paymentMethodHttpService)
        this.dexieService.populateTable('suppliers', this.supplierHttpService)
        this.dexieService.populateCriteria('suppliersCriteria', this.supplierHttpService)
        // Reservations
        this.dexieService.populateTable('berths', this.berthHttpService)
        this.dexieService.populateTable('boatTypes', this.boatTypeHttpService)
        this.dexieService.populateTable('boatUsages', this.boatUsageHttpService)
        this.dexieService.populateTable('paymentStatuses', this.paymentStatusHttpService)
    }

    private setDotNetVersion(response: any): void {
        DotNetVersion.version = response.dotNetVersion
    }

    private setSelectedYear(): void {
        this.sessionStorageService.saveItem('selectedYear', new Date().getFullYear().toString())
    }

    private setUpcomingLeaseEndDays(): void {
        const days = this.sessionStorageService.getItem('lease-days') != '' ? this.sessionStorageService.getItem('lease-days') : '30'
        this.sessionStorageService.saveItem('lease-days', days)
    }

    private setUserData(response: any): void {
        this.sessionStorageService.saveItem('userId', this.cryptoService.encrypt(response.userId))
        this.sessionStorageService.saveItem('displayName', this.cryptoService.encrypt(response.displayname))
        this.sessionStorageService.saveItem('isAdmin', this.cryptoService.encrypt(response.isAdmin))
        this.sessionStorageService.saveItem('isFirstFieldFocused', response.isFirstFieldFocused)
    }

    //#endregion

    //#region  getters

    get isLoggedIn(): boolean {
        if (this.sessionStorageService.getItem('userId')) {
            return true
        }
        return false
    }

    //#endregion

}