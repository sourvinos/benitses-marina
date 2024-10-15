import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BerthHttpService } from '../services/berth-http.service'
import { ListResolved } from '../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BerthListResolver {

    constructor(private berthHttpService: BerthHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.berthHttpService.getStatus().pipe(
            map((berthList) => new ListResolved(berthList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
