import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { CashierLedgerHttpService } from '../services/cashier-ledger-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class CashierLedgerListResolver {

    constructor(private cashierLedgerHttpService: CashierLedgerHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.cashierLedgerHttpService.getAll()
            .pipe(
                map((x) => new ListResolved(x)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
