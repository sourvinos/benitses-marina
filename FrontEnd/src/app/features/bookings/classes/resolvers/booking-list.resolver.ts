import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BookingHttpService } from '../services/booking-http.service'
import { ListResolved } from '../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BookingListResolver {

    constructor(private bookingHttpService: BookingHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.bookingHttpService.getAll().pipe(
            map((bookingList) => new ListResolved(bookingList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
