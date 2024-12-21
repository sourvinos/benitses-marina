import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { InsurancePolicyHttpService } from '../services/insurance-policy-http.service'
import { ListResolved } from 'src/app/shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class InsurancePolicyListResolver {

    constructor(private insurancePolicyHttpService: InsurancePolicyHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.insurancePolicyHttpService.getAll().pipe(
            map((x) => new ListResolved(x)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
