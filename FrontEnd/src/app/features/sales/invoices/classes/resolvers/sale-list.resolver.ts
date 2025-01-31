import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { ListResolved } from '../../../../../shared/classes/list-resolved'
import { SaleHttpDataService } from '../services/sale-http-data.service'

@Injectable({ providedIn: 'root' })

export class SaleListResolver {

    constructor(private saleHttpService: SaleHttpDataService) { }

    resolve(): Observable<ListResolved> {
        return this.saleHttpService.getAll()
            .pipe(
                map((saleList) => new ListResolved(saleList)),
                catchError((err: any) => of(new ListResolved(null, err)))
            )
    }

}
