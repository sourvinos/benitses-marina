import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
// Custom
import { HttpDataService } from 'src/app/shared/services/http-data.service'
import { environment } from 'src/environments/environment'
import { InsurancePolicyListVM } from '../view-models/insurance-policy-list-vm'

@Injectable({ providedIn: 'root' })

export class InsurancePolicyHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/insurancePolicies')
    }

}
