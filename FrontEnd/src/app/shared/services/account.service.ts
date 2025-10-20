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
import { CompanyHttpService } from 'src/app/features/expenses/companies/classes/services/company-http.service'
import { CryptoService } from './crypto.service'
import { CustomerHttpService } from 'src/app/features/sales/customers/classes/services/customer-http.service'
import { DexieService } from './dexie.service'
import { DocumentTypeHttpService } from 'src/app/features/sales/documentTypes/classes/services/documentType-http.service'
import { DotNetVersion } from '../classes/dotnet-version'
import { ExpensesDocumentTypeHttpService } from 'src/app/features/expenses/documentTypes/classes/services/documentType-http.service'
import { HttpDataService } from './http-data.service'
import { HullTypeHttpService } from 'src/app/features/sales/hullTypes/classes/services/hullType-http.service'
import { NationalityHttpService } from 'src/app/features/sales/nationalities/classes/services/nationality-http.service'
import { PaymentMethodHttpService } from 'src/app/features/expenses/paymentMethods/classes/services/paymentMethod-http.service'
import { PaymentStatusHttpService } from 'src/app/features/reservations/paymentStatuses/classes/services/paymentStatus-http.service'
import { PeriodTypeHttpService } from 'src/app/features/sales/periodTypes/classes/services/periodType-http.service'
import { PriceHttpService } from 'src/app/features/sales/prices/classes/services/price-http.service'
import { ResetPasswordViewModel } from 'src/app/features/users/classes/view-models/reset-password-view-model'
import { SafeHttpService } from 'src/app/features/cashiers/safes/classes/services/safe-http.service'
import { SeasonTypeHttpService } from 'src/app/features/sales/seasonTypes/classes/services/seasonType-http.service'
import { SessionStorageService } from './session-storage.service'
import { SupplierHttpService } from 'src/app/features/expenses/suppliers/classes/services/supplier-http.service'
import { TaxOfficeHttpService } from 'src/app/features/sales/taxOffices/classes/services/taxOffice-http.service'
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

    constructor(httpClient: HttpClient, private bankHttpService: BankHttpService, private berthHttpService: BerthHttpService, private boatTypeHttpService: BoatTypeHttpService, private boatUsageHttpService: BoatUsageHttpService, private companyHttpService: CompanyHttpService, private cryptoService: CryptoService, private customerHttpService: CustomerHttpService, private dexieService: DexieService, private expensesDocumentTypeHttpService: ExpensesDocumentTypeHttpService, private hullTypeHttpService: HullTypeHttpService, private nationalityHttpService: NationalityHttpService, private ngZone: NgZone, private paymentMethodHttpService: PaymentMethodHttpService, private paymentStatusHttpService: PaymentStatusHttpService, private periodTypeHttpService: PeriodTypeHttpService, private priceHttpService: PriceHttpService, private router: Router, private safeHttpService: SafeHttpService, private saleDocumentHttpService: DocumentTypeHttpService, private seasonTypeHttpService: SeasonTypeHttpService, private sessionStorageService: SessionStorageService, private supplierHttpService: SupplierHttpService, private taxOfficeService: TaxOfficeHttpService) {
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
            { 'item': 'isFirstFieldFocused', 'when': 'always' },
            { 'item': 'jwt', 'when': 'always' },
            { 'item': 'lease-days', 'when': 'always' },
            { 'item': 'now', 'when': 'always' },
            { 'item': 'refreshToken', 'when': 'always' },
            { 'item': 'returnUrl', 'when': 'always' },
            { 'item': 'userId', 'when': 'always' },
            // Lists and filters
            { 'item': 'balanceSheetCriteria', 'when': 'always' },
            { 'item': 'cards-active-tab', 'when': 'always' },
            { 'item': 'customerList-filters', 'when': 'always' },
            { 'item': 'customerList-id', 'when': 'always' },
            { 'item': 'customerList-scrollTop', 'when': 'always' },
            { 'item': 'expenseDocumentTypeList-filters', 'when': 'always' },
            { 'item': 'expenseDocumentTypeList-id', 'when': 'always' },
            { 'item': 'expenseDocumentTypeList-scrollTop', 'when': 'always' },
            { 'item': 'invoiceList-filters', 'when': 'always' },
            { 'item': 'invoicesListCriteria', 'when': 'always' },
            { 'item': 'invoiceList-id', 'when': 'always' },
            { 'item': 'invoiceList-scrollTop', 'when': 'always' },
            { 'item': 'ledgerCriteria', 'when': 'always' },
            { 'item': 'priceList-filters', 'when': 'always' },
            { 'item': 'priceList-id', 'when': 'always' },
            { 'item': 'priceList-scrollTop', 'when': 'always' },
            { 'item': 'reservationList-filters', 'when': 'always' },
            { 'item': 'reservationList-id', 'when': 'always' },
            { 'item': 'reservationList-scrollTop', 'when': 'always' },
            { 'item': 'safeList-filters', 'when': 'always' },
            { 'item': 'safeList-id', 'when': 'always' },
            { 'item': 'safeList-scrollTop', 'when': 'always' },
            { 'item': 'saleList-id', 'when': 'always' },
            { 'item': 'saleList-scrollTop', 'when': 'always' },
            { 'item': 'salesCriteria', 'when': 'always' },
            { 'item': 'statisticsCriteria', 'when': 'always' },
            { 'item': 'supplierList-filters', 'when': 'always' },
            { 'item': 'supplierList-id', 'when': 'always' },
            { 'item': 'supplierList-scrollTop', 'when': 'always' },
            { 'item': 'userList-filters', 'when': 'always' },
            { 'item': 'userList-id', 'when': 'always' },
            { 'item': 'userList-scrollTop', 'when': 'always' },
            { 'item': 'cashier-ledger-criteria', 'when': 'always' },
            { 'item': 'cashier-ledger-scrollTop', 'when': 'always' },
            { 'item': 'cashierList-id', 'when': 'always' },
            { 'item': 'cashierList-scrollTop', 'when': 'always' },
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
        this.dexieService.populateTable('companies', this.companyHttpService)
        this.dexieService.populateTable('expenseDocumentTypes', this.expensesDocumentTypeHttpService)
        this.dexieService.populateTable('paymentMethods', this.paymentMethodHttpService)
        this.dexieService.populateTable('suppliers', this.supplierHttpService)
        this.dexieService.populateCriteria('companiesCriteria', this.companyHttpService)
        this.dexieService.populateCriteria('suppliersCriteria', this.supplierHttpService)
        // Reservations
        this.dexieService.populateTable('berths', this.berthHttpService)
        this.dexieService.populateTable('boatTypes', this.boatTypeHttpService)
        this.dexieService.populateTable('boatUsages', this.boatUsageHttpService)
        this.dexieService.populateTable('paymentStatuses', this.paymentStatusHttpService)
        // Sales
        this.dexieService.populateTable('customers', this.customerHttpService)
        this.dexieService.populateTable('hullTypes', this.hullTypeHttpService)
        this.dexieService.populateTable('nationalities', this.nationalityHttpService)
        this.dexieService.populateTable('periodTypes', this.periodTypeHttpService)
        this.dexieService.populateTable('prices', this.priceHttpService)
        this.dexieService.populateTable('saleDocumentTypes', this.saleDocumentHttpService)
        this.dexieService.populateTable('seasonTypes', this.seasonTypeHttpService)
        this.dexieService.populateTable('taxOffices', this.taxOfficeService)
        // Safes
        this.dexieService.populateTable('safes', this.safeHttpService)
        this.dexieService.populateCriteria('safesCriteria', this.safeHttpService)
    }

    private setDotNetVersion(response: any): void {
        DotNetVersion.version = response.dotNetVersion
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