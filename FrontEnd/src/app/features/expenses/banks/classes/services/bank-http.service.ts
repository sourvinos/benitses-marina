import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BankBrowserStorageVM } from '../view-models/bank-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BankHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/banks')
    }

    public getForBrowser(): Observable<BankBrowserStorageVM[]> {
        return this.http.get<BankBrowserStorageVM[]>(environment.apiUrl + '/banks/getForBrowser')
    }

}
