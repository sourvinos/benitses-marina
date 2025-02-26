import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { BankHttpService } from '../services/bank-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class BankListResolver {

    constructor(private bankHttpService: BankHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.bankHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
