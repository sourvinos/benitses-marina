import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { HullTypeHttpService } from '../services/hullType-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class HullTypeFormResolver {

    constructor(private hullTypeHttpService: HullTypeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.hullTypeHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
