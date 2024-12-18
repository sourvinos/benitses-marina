import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { SupplierHttpService } from '../services/supplier-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class SupplierFormResolver {

    constructor(private supplierHttpService: SupplierHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.supplierHttpService.getSingle(route.params.id).pipe(
            map((supplierForm) => new FormResolved(supplierForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
