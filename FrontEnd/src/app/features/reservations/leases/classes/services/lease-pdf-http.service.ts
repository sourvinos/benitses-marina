import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class LeasePdfHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl)
    }

    public buildLeasePdf(reservationIds: string[]): Observable<any> {
        return this.http.post<any>(this.url + '/leases', reservationIds)
    }

    public openLeasePdf(filename: string): Observable<any> {
        return this.http.get(this.url + '/leases/openLeasePdf/' + filename, { responseType: 'arraybuffer' })
    }

}

