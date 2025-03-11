import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { CashierHttpService } from '../services/cashier-http.service'

@Injectable({ providedIn: 'root' })

export class CashierFormResolver {

    constructor(private cashierHttpService: CashierHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.cashierHttpService.getSingle(route.params.id).pipe(
            map((cashierForm) => new FormResolved(cashierForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
