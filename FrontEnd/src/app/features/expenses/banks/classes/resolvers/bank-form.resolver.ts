import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { BankHttpService } from '../services/bank-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class BankFormResolver {

    constructor(private bankHttpService: BankHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.bankHttpService.getSingle(route.params.id).pipe(
            map((form) => new FormResolved(form)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
