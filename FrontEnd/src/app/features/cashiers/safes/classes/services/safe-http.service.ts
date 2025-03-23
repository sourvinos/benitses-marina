import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SafeBrowserStorageVM } from '../view-models/safe-browser-storage-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class SafeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/safes')
    }

    public getForBrowser(): Observable<SafeBrowserStorageVM[]> {
        return this.http.get<SafeBrowserStorageVM[]>(environment.apiUrl + '/safes/getForBrowser')
    }

}
