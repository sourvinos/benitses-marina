import { Injectable } from '@angular/core'
import { Observable, of } from 'rxjs'
import { catchError, map } from 'rxjs/operators'
// Custom
import { HullTypeHttpService } from '../services/hullType-http.service'
import { ListResolved } from '../../../../../shared/classes/list-resolved'

@Injectable({ providedIn: 'root' })

export class HullTypeListResolver {

    constructor(private hullTypeHttpService: HullTypeHttpService) { }

    resolve(): Observable<ListResolved> {
        return this.hullTypeHttpService.getAll().pipe(
            map((list) => new ListResolved(list)),
            catchError((err: any) => of(new ListResolved(null, err)))
        )
    }

}
