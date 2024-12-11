import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BoatUsageAutoCompleteVM } from '../view-models/boatUsage-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BoatUsageHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/boatUsages')
    }

    public getForBrowser(): Observable<BoatUsageAutoCompleteVM[]> {
        return this.http.get<BoatUsageAutoCompleteVM[]>(environment.apiUrl + '/boatUsages/getForBrowser')
    }

}
