import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { ReservationReadDto } from '../dtos/reservation-read-dto'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class ReservationHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/reservations')
    }

    public getForPeriod(): Observable<ReservationReadDto[]> {
        return this.http.get<ReservationReadDto[]>(environment.apiUrl + '/reservations')
    }

}
