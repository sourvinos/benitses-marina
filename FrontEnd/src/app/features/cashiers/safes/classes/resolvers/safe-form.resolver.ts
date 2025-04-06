import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { SafeHttpService } from '../services/safe-http.service'

@Injectable({ providedIn: 'root' })

export class SafeFormResolver {

    constructor(private safeHttpService: SafeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.safeHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
