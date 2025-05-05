import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { PeriodTypeHttpService } from '../services/periodType-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class PeriodTypeListResolver {

    constructor(private periodTypeHttpService: PeriodTypeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.periodTypeHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
