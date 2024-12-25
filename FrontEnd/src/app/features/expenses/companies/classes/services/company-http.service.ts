import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CompanyBrowserStorageVM } from '../view-models/company-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SimpleCriteriaEntity } from 'src/app/shared/classes/simple-criteria-entity'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CompanyHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/companies')
    }

    public getForBrowser(): Observable<CompanyBrowserStorageVM[]> {
        return this.http.get<CompanyBrowserStorageVM[]>(environment.apiUrl + '/companies/getForBrowser')
    }

    public getForCriteria(): Observable<SimpleCriteriaEntity[]> {
        return this.http.get<SimpleCriteriaEntity[]>(environment.apiUrl + '/companies/getForCriteria')
    }

}
