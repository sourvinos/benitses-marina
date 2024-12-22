import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { DocumentTypeBrowserStorageVM } from '../view-models/documentType-browser-storage-vm'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class DocumentTypeHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/documentTypes')
    }

    public getForBrowser(): Observable<DocumentTypeBrowserStorageVM[]> {
        return this.http.get<DocumentTypeBrowserStorageVM[]>(environment.apiUrl + '/documentTypes/getForBrowser')
    }

}
