import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class EmailQueueHttpService {

    constructor(public httpClient: HttpClient) { }

    public save(formData: any): Observable<any> {
        return this.httpClient.post<any>(environment.apiUrl + '/emailqueues', formData)
    }

}
