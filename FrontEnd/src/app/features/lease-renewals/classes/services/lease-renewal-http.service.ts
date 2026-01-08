import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { LeaseRenewalListVM } from '../view-models/lease-renewal-list-vm'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class LeaseRenewalHttpService extends HttpDataService {

    constructor(httpClient: HttpClient, private sessionStorageService: SessionStorageService) {
        super(httpClient, environment.apiUrl)
    }

    public getUpcoming(): Observable<LeaseRenewalListVM[]> {
        const days = this.sessionStorageService.getItem('lease-days') != '' ? this.sessionStorageService.getItem('lease-days') : '30'
        return this.http.get<LeaseRenewalListVM[]>(environment.apiUrl + '/leases/' + days)
    }

}
