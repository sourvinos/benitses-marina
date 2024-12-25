import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { SupplierBrowserStorageVM } from '../view-models/supplier-browser-storage-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class SupplierHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/suppliers')
    }

    public getForBrowser(): Observable<SupplierBrowserStorageVM[]> {
        return this.http.get<SupplierBrowserStorageVM[]>(environment.apiUrl + '/suppliers/getForBrowser')
    }

    public getForCriteria(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/suppliers/getForCriteria')
    }

}
