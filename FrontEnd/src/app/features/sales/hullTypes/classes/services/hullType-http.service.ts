import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HullTypeBrowserStorageVM } from '../view-models/hullType-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class HullTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/hullTypes')
    }

    public getForBrowser(): Observable<HullTypeBrowserStorageVM[]> {
        return this.http.get<HullTypeBrowserStorageVM[]>(environment.apiUrl + '/hullTypes/getForBrowser')
    }

}
