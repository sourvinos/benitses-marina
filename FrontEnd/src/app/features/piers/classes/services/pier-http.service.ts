import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'
import { PierAutoCompleteVM } from '../view-models/pier-autocomplete-vm'

@Injectable({ providedIn: 'root' })

export class PierHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/piers')
    }

    //#region public methods

    public getForBrowser(): Observable<PierAutoCompleteVM[]> {
        return this.http.get<PierAutoCompleteVM[]>(environment.apiUrl + '/piers/getForBrowser')
    }

    //#endregion

}
