import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { SafeHttpService } from '../services/safe-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class SafeListResolver {

    constructor(private safeHttpService: SafeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.safeHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
