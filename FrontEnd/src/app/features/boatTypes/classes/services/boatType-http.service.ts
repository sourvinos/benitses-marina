import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BoatTypeAutoCompleteVM } from '../view-models/boatType-autocomplete-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BoatTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/boatTypes')
    }

    //#region public methods

    public getForBrowser(): Observable<BoatTypeAutoCompleteVM[]> {
        return this.http.get<BoatTypeAutoCompleteVM[]>(environment.apiUrl + '/boatTypes/getForBrowser')
    }

    //#endregion

}
