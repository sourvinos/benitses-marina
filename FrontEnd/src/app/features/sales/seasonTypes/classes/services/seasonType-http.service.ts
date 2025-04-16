import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SeasonTypeBrowserStorageVM } from '../view-models/seasonType-browser-storage-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class SeasonTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/seasonTypes')
    }

    public getForBrowser(): Observable<SeasonTypeBrowserStorageVM[]> {
        return this.http.get<SeasonTypeBrowserStorageVM[]>(environment.apiUrl + '/seasonTypes/getForBrowser')
    }

}
