import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { PaymentStatusAutoCompleteVM } from '../view-models/paymentStatus-autocomplete-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class PaymentStatusHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/paymentStatuses')
    }

    //#region public methods

    public getForBrowser(): Observable<PaymentStatusAutoCompleteVM[]> {
        return this.http.get<PaymentStatusAutoCompleteVM[]>(environment.apiUrl + '/paymentStatuses/getForBrowser')
    }

    //#endregion

}
