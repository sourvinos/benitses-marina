import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BalanceFilterBrowserStorageVM } from '../view-models/balance-filter-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BalanceFilterHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/balanceFilters')
    }

    public getForBrowsers(): Observable<BalanceFilterBrowserStorageVM[]> {
        return this.http.get<BalanceFilterBrowserStorageVM[]>(environment.apiUrl + '/balanceFilters/getForBrowser')
    }

    public getForCriterias(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/balanceFilters/getForCriteria')
    }

}
