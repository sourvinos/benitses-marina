import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../shared/classes/list-resolved'
import { ReservationHttpService } from '../services/reservation-http.service'

@Injectable({ providedIn: 'root' })

export class ReservationListResolver {

    constructor(private reservationHttpService: ReservationHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.reservationHttpService.getAll().pipe(
            map((reservationList) => new ListResolved(reservationList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
