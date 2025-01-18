import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class InvoiceHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/expenses')
    }

    public saveExpense(formData: any): Observable<any> {
        return formData.expenseId == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
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
