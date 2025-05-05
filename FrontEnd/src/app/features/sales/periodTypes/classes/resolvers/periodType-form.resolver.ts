import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { PeriodTypeHttpService } from '../services/periodType-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class PeriodTypeFormResolver {

    constructor(private periodTypeHttpService: PeriodTypeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.periodTypeHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
