import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from 'src/app/shared/classes/list-resolved'
import { LeaseRenewalHttpService } from '../services/lease-renewal-http.service'

@Injectable({ providedIn: 'root' })

export class LeaseRenewalListResolver {

    constructor(private leaseRenewalHttpService: LeaseRenewalHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.leaseRenewalHttpService.getUpcoming().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
