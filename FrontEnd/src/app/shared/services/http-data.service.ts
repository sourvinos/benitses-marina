import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'

export class HttpDataService {

    constructor(public http: HttpClient, public url: string) { }

    public getAll(): Observable<any[]> {
        return this.http.get<any[]>(this.url)
    }

    public getSingle(id: string | number): Observable<any> {
        if (id != undefined)
            return this.http.get<any>(this.url + '/' + id)
    }

    public save(formData: any): Observable<any> {
        return formData.id == 0 || formData.id == null
            ? this.http.post<any>(this.url, formData)
            : this.http.put<any>(this.url, formData)
    }

    public softDelete(formData: any): Observable<any> {
        return this.http.put<any>(this.url, formData)
    }

    public delete(id: string | number): Observable<any> {
        if (id != undefined)
            return this.http.delete<any>(this.url + '/' + id)
    }

}
