import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { PaymentMethodBrowserStorageVM } from '../view-models/paymentMethod-browser-storage-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PaymentMethodHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/paymentMethods')
    }

    public getForBrowser(): Observable<PaymentMethodBrowserStorageVM[]> {
        return this.http.get<PaymentMethodBrowserStorageVM[]>(environment.apiUrl + '/paymentMethods/getForBrowser')
    }

}
