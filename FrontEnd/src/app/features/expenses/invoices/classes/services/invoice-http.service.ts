import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { InvoiceListCriteriaVM } from '../view-models/criteria/invoice-list-criteria-vm'
import { InvoiceListVM } from '../view-models/list/invoice-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/expenses')
    }

    public getForList(criteria: InvoiceListCriteriaVM): Observable<InvoiceListVM[]> {
        return this.http.request<InvoiceListVM[]>('post', environment.apiUrl + '/expenses/getForPeriod', { body: criteria })
    }

    public getForToday(): Observable<InvoiceListVM[]> {
        return this.http.request<InvoiceListVM[]>('post', environment.apiUrl + '/expenses/getForToday')
    }

    public saveExpense(formData: any): Observable<any> {
        return formData.expenseId == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
    }

    public patchExpense(expenseId: string, hasDocument: boolean): Observable<any> {
        return this.http.patch<any>(this.url + '/' + expenseId + '/' + hasDocument, null, { params: null })
    }

    public getDocuments(id: string): Observable<any> {
        return this.http.get(this.url + '/documents/' + id)
    }

    public upload(formData: any, options: any,): Observable<any> {
        return this.http.post(this.url + '/upload', formData, options)
    }

    public rename(formData: any): Observable<any> {
        return this.http.post<any>(this.url + '/rename', formData)
    }

    public deleteDocument(filename: string): Observable<any> {
        return this.http.delete<any>(this.url + '/deleteDocument/' + filename)
    }

    public openDocument(filename: string): Observable<any> {
        return this.http.get(this.url + '/openDocument/' + filename, { responseType: 'arraybuffer' })
    }

}
