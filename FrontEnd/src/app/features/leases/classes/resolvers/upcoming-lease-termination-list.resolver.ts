import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { UpcomingLeaseTerminationHttpService } from '../services/upcoming-lease-termination-http.service'

@Injectable({ providedIn: 'root' })

export class UpcomingLeaseTerminationListResolver {

    constructor(private upcomingLeaseTerminationHttpService: UpcomingLeaseTerminationHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.upcomingLeaseTerminationHttpService.getUpcoming().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
