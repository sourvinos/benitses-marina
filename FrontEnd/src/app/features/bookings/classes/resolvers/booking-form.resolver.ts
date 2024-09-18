import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { BookingHttpService } from '../services/booking-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class BookingFormResolver {

    constructor(private bookingHttpService: BookingHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.bookingHttpService.getSingle(route.params.id).pipe(
            map((bookingForm) => new FormResolved(bookingForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
