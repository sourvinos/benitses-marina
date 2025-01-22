import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { DocumentTypeBrowserStorageVM } from '../view-models/documentType-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ExpensesDocumentTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/expenseDocumentTypes')
    }

    public getForBrowser(): Observable<DocumentTypeBrowserStorageVM[]> {
        return this.http.get<DocumentTypeBrowserStorageVM[]>(environment.apiUrl + '/expenseDocumentTypes/getForBrowser')
    }

}
