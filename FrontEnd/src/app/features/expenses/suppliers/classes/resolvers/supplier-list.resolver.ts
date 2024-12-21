import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { SupplierHttpService } from '../services/supplier-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class SupplierListResolver {

    constructor(private supplierHttpService: SupplierHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.supplierHttpService.getAll().pipe(
            map((supplierList) => new ListResolved(supplierList)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
