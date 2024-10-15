import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BerthAutoCompleteVM } from '../view-models/berth-autocomplete-vm'
import { BerthListVM } from '../view-models/berth-list-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BerthHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/berths')
    }

    public getForBrowser(): Observable<BerthAutoCompleteVM[]> {
        return this.http.get<BerthAutoCompleteVM[]>(environment.apiUrl + '/berths/getForBrowser')
    }

    public getStatus(): Observable<BerthListVM[]> {
        return this.http.get<BerthListVM[]>(environment.apiUrl + '/berths/getState')
    }

}
