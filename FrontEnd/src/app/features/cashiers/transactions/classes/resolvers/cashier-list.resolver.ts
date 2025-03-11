import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { CashierHttpService } from '../services/cashier-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class CashierListResolver {

    constructor(private cashierHttpService: CashierHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.cashierHttpService.getAll().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
