import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ReservationHttpService } from '../services/reservation-http.service'

@Injectable({ providedIn: 'root' })

export class ReservationFormResolver {

    constructor(private reservationHttpService: ReservationHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.reservationHttpService.getSingle(route.params.id).pipe(
            map((reservationForm) => new FormResolved(reservationForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
