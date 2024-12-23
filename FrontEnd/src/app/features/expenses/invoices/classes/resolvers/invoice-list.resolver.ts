import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { InvoiceHttpService } from '../services/invoice-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class InvoiceListResolver {

    constructor(private invoiceHttpService: InvoiceHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.invoiceHttpService.getAll().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
