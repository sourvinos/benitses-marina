import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { StatisticsHttpService } from '../services/statistics-http.service'

@Injectable({ providedIn: 'root' })

export class StatisticsResolver {

    constructor(private statisticsHttpService: StatisticsHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.statisticsHttpService.getAll()
            .pipe(
                map((balanceSheet) => new ListResolved(balanceSheet)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
