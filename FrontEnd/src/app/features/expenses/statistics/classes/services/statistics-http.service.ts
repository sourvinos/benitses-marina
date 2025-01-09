import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { HttpDataService } from 'src/app/shared/services/http-data.service'
// Custom
import { StatisticsCriteriaVM } from '../view-models/criteria/statisticss-criteria-vm'
import { StatisticsVM } from '../view-models/list/statistics-vm'
import { environment } from 'src/environments/environment'

@Injectable({ providedIn: 'root' })

export class StatisticsHttpService extends HttpDataService {

    constructor(httpClient: HttpClient) {
        super(httpClient, environment.apiUrl + '/statistics')
    }

    get(criteria: StatisticsCriteriaVM): Observable<StatisticsVM[]> {
        return this.http.request<StatisticsVM[]>('post', this.url + '/buildStatistics', { body: criteria })
    }

}
