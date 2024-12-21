import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ExpenseHttpService } from '../services/expense-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class ExpenseListResolver {

    constructor(private expenseHttpService: ExpenseHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.expenseHttpService.getAll().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
