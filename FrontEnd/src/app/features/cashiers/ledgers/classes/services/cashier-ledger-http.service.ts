import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CashierLedgerCriteriaVM } from '../view-models/criteria/cashier-ledger-criteria-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { CashierLedgerVM } from '../view-models/list/cashier-ledger-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CashierLedgerHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/cashierledgers')
    }

    get(criteria: CashierLedgerCriteriaVM): Observable<CashierLedgerVM[]> {
        return this.http.request<CashierLedgerVM[]>('post', this.url + '/buildLedger', { body: criteria })
    }

}
