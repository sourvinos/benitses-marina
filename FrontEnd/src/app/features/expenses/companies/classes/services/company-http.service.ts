import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { CompanyAutoCompleteVM } from '../view-models/company-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class CompanyHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/companies')
    }

    //#region public methods

    public getForBrowser(): Observable<CompanyAutoCompleteVM[]> {
        return this.http.get<CompanyAutoCompleteVM[]>(environment.apiUrl + '/companies/getForBrowser')
    }

    //#endregion

}
