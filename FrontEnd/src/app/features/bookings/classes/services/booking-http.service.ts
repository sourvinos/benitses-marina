import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { BookingReadDto } from '../dtos/booking-read-dto'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class BookingHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/bookings')
    }

    public getForPeriod(): Observable<BookingReadDto[]> {
        return this.http.get<BookingReadDto[]>(environment.apiUrl + '/bookings')
    }

}
