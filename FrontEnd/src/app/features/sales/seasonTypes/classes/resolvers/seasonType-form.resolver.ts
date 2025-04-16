import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { SeasonTypeHttpService } from '../services/seasonType-http.service'

@Injectable({ providedIn: 'root' })

export class SeasonTypeFormResolver {

    constructor(private seasonTypeHttpService: SeasonTypeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.seasonTypeHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
