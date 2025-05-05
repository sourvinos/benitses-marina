import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { PeriodTypeBrowserStorageVM } from '../view-models/periodType-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PeriodTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/periodTypes')
    }

    public getForBrowser(): Observable<PeriodTypeBrowserStorageVM[]> {
        return this.http.get<PeriodTypeBrowserStorageVM[]>(environment.apiUrl + '/periodTypes/getForBrowser')
    }

}
