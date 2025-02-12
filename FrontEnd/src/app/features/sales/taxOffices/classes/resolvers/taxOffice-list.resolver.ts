import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { TaxOfficeHttpService } from '../services/taxOffice-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class TaxOfficeListResolver {

    constructor(private taxOfficeService: TaxOfficeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.taxOfficeService.getAll().pipe(
            map((taxOfficeList) => new ListResolved(taxOfficeList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
