import { ActivatedRouteSnapshot } from '@angular/router'
import { Injectable } from '@angular/core'
import { catchError, map, of } from 'rxjs'
// Custom
import { ExpensesDocumentTypeHttpService } from '../services/documentType-http.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'

@Injectable({ providedIn: 'root' })

export class DocumentTypeFormResolver {

    constructor(private documentTypeHttpService: ExpensesDocumentTypeHttpService) { }

    resolve(route: ActivatedRouteSnapshot): any {
        return this.documentTypeHttpService.getSingle(route.params.id).pipe(
            map((documentTypeForm) => new FormResolved(documentTypeForm)),
            catchError((err: any) => of(new FormResolved(null, err)))
        )
    }

}
