import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { ExpenseHttpService } from '../services/expense-http.service'

@Injectable({ providedIn: 'root' })

export class ExpenseFormResolver {

    constructor(private expenseHttpService: ExpenseHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.expenseHttpService.getSingle(route.params.id).pipe(
            map((expenseForm) => new FormResolved(expenseForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
