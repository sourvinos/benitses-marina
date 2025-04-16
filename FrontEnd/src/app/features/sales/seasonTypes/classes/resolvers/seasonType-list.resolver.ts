import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { SeasonTypeHttpService } from '../services/seasonType-http.service'

@Injectable({ providedIn: 'root' })

export class SeasonTypeListResolver {

    constructor(private seasonTypeHttpService: SeasonTypeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.seasonTypeHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
