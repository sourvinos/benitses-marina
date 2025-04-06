import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SaleListCriteriaVM } from '../view-models/criteria/sale-list-criteria-vm'
import { SaleListVM } from '../view-models/list/sale-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class SaleHttpDataService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/sales')
    }

    public getForList(criteria: SaleListCriteriaVM): Observable<SaleListVM[]> {
        return this.http.request<SaleListVM[]>('post', environment.apiUrl + '/sales/getForPeriod', { body: criteria })
    }

    public validateCustomerData(customerId: number): Observable<any> {
        return this.http.get(environment.apiUrl + '/customers/getByIdForValidation/' + customerId)
    }

    public override save(formData: any): Observable<any> {
        return formData.invoiceId == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
    }

    public submitDataUp(invoiceId: string): Observable<any> {
        return this.http.get(environment.apiUrl + '/salesdataup/' + invoiceId)
    }

}

