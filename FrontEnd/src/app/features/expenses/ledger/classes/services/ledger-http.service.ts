import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { LedgerCriteriaVM } from '../view-models/criteria/ledger-criteria-vm'
import { LedgerVM } from '../view-models/list/ledger-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class LedgerHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/ledgers')
    }

    get(criteria: LedgerCriteriaVM): Observable<LedgerVM[]> {
        return this.http.request<LedgerVM[]>('post', this.url + '/buildLedger', { body: criteria })
    }

}
