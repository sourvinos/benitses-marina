import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { TaxOfficeHttpService } from '../services/taxOffice-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class TaxOfficeFormResolver {

    constructor(private taxOfficeService: TaxOfficeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.taxOfficeService.getSingle(route.params.id).pipe(
            map((taxOfficeForm) => new FormResolved(taxOfficeForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
