import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'
import { PriceListBrowserStorageVM } from '../view-models/price-list-browser-storage-vm'
import { Observable } from 'rxjs'

@Injectable({ providedIn: 'root' })

export class PriceHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/prices')
    }

    public getForBrowser(): Observable<PriceListBrowserStorageVM[]> {
        return this.http.get<PriceListBrowserStorageVM[]>(environment.apiUrl + '/prices/getForBrowser')
    }

}
