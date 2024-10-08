import { BehaviorSubject, Observable, throwError } from 'rxjs'
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { catchError, filter, finalize, switchMap, take, tap } from 'rxjs/operators'
// Custom
import { AccountService } from './account.service'
import { LoadingSpinnerService } from './loading-spinner.service'
import { SessionStorageService } from './session-storage.service'

@Injectable({ providedIn: 'root' })

export class InterceptorService {

    //#region variables

    private isTokenRefreshing = false
    private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null)

    //#endregion

    constructor(private accountService: AccountService, private loadingSpinnerService: LoadingSpinnerService, private sessionStorageService: SessionStorageService) { }

    //#region public methods

    public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (this.isUserLoggedIn()) {
            this.loadingSpinnerService.requestStarted()
            return next.handle(this.attachTokenToRequest(request)).pipe(
                tap((event: HttpEvent<any>) => {
                    if (event instanceof HttpResponse) {
                        this.loadingSpinnerService.requestEnded()
                        return
                    }
                }), catchError((err): Observable<any> => {
                    this.loadingSpinnerService.resetSpiner()
                    if (this.isUserLoggedIn()) {
                        return err.status == 401
                            ? this.refreshToken(request, next)
                            : this.trapError((<HttpErrorResponse>err).status)
                    }
                })
            )
        } else {
            return next.handle(request)
        }
    }

    //#endregion

    //#region private methods

    private attachTokenToRequest(request: HttpRequest<any>): HttpRequest<any> {
        const token = sessionStorage.getItem('jwt')
        return request.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        })
    }

    private refreshToken(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
        if (!this.isTokenRefreshing) {
            this.isTokenRefreshing = true
            this.tokenSubject.next(null)
            return this.accountService.getNewRefreshToken().pipe(
                switchMap((tokenresponse: any) => {
                    if (tokenresponse) {
                        this.tokenSubject.next(tokenresponse.token)
                        sessionStorage.setItem('jwt', tokenresponse.token)
                        sessionStorage.setItem('expiration', tokenresponse.expiration)
                        sessionStorage.setItem('refreshToken', tokenresponse.refreshToken)
                        return next.handle(this.attachTokenToRequest(request))
                    }
                    return <any>this.accountService.logout()
                }),
                catchError(error => {
                    return throwError(() => error.status)
                }),
                finalize(() => {
                    this.isTokenRefreshing = false
                })
            )
        } else {
            this.isTokenRefreshing = false
            return this.tokenSubject.pipe(filter(token => token != null), take(1), switchMap(() => next.handle(this.attachTokenToRequest(request))))
        }
    }

    private isUserLoggedIn(): boolean {
        return this.sessionStorageService.getItem('userId') ? true : false
    }

    private trapError(err: number): Observable<any> {
        switch (err) {
            case 400: return throwError(() => new Error('400')) // invalid model
            case 404: return throwError(() => new Error('404')) // not found
            case 412: return throwError(() => new Error('412')) // the password can't be changed because the current password is wrong
            case 415: return throwError(() => new Error('415')) // concurrency error
            case 491: return throwError(() => new Error('491')) // record can't be deleted because it's in use
            case 492: return throwError(() => new Error('492')) // unable to create user or update user, username and/or password not unique
            default:
                return throwError(() => new Error('500'))
        }
    }

    //#endregion

}
