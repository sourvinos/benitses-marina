import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { UpcomingLeaseTerminationListVM } from '../view-models/upcoming-lease-termination-list-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class UpcomingLeaseTerminationHttpService extends HttpDataService {

    constructor(httpClient: HttpClient, private sessionStorageService: SessionStorageService) {
        super(httpClient, environment.apiUrl)
    }

    public getUpcoming(): Observable<UpcomingLeaseTerminationListVM[]> {
        const days = this.sessionStorageService.getItem('lease-days') != '' ? this.sessionStorageService.getItem('lease-days') : '30'
        return this.http.get<UpcomingLeaseTerminationListVM[]>(environment.apiUrl + '/leases/' + days)
    }

}
