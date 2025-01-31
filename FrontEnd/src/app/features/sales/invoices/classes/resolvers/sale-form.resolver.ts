import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { SaleHttpDataService } from '../services/sale-http-data.service'

@Injectable({ providedIn: 'root' })

export class SaleFormResolver {

    constructor(private saleHttpService: SaleHttpDataService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.saleHttpService.getSingle(route.params.id).pipe(
            map((saleForm) => new FormResolved(saleForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
