import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { firstValueFrom } from 'rxjs'
// Custom
import { LocalStorageService } from './local-storage.service'

@Injectable({ providedIn: 'root' })

export class MessageDialogService {

    //#region variables

    private messages: any = []
    private feature = 'snackbarMessages'

    //#endregion

    constructor(private httpClient: HttpClient, private localStorageService: LocalStorageService) {
        this.getMessages()
    }

    //#region public methods

    public getDescription(feature: string, id: string, stringLength = 0): string {
        let returnValue = ''
        if (this.messages != undefined) {
            this.messages.filter((f: { feature: string; labels: any[] }) => {
                if (f.feature === feature) {
                    f.labels.filter(l => {
                        if (l.id == id) {
                            returnValue = l.message.replace('xx', stringLength.toString())
                        }
                    })
                }
            })
        }
        return returnValue
    }

    public getMessages(): Promise<any> {
        const promise = new Promise((resolve) => {
            firstValueFrom(this.httpClient.get('assets/languages/dialogs.' + this.localStorageService.getLanguage() + '.json')).then(response => {
                this.messages = response
                resolve(this.messages)
            })
        })
        return promise
    }

    public authenticationFailed(): string { return this.getDescription(this.feature, 'authenticationFailed') }
    public emailSent(): string { return this.getDescription(this.feature, 'emailSent') }
    public formIsDirty(): string { return this.getDescription(this.feature, 'formIsDirty') }
    public emailNotSent(): string { return this.getDescription(this.feature, 'emailNotSent') }
    public unableToResetPassword(): string { return this.getDescription(this.feature, 'unableToResetPassword') }
    public passwordCanBeChangedOnlyByAccountOwner(): string { return this.getDescription(this.feature, 'passwordCanBeChangedOnlyByAccountOwner') }
    public noContactWithServer(): string { return this.getDescription(this.feature, 'noContactWithServer') }
    public success(): string { return this.getDescription(this.feature, 'success') }
    public confirmDelete(): string { return this.getDescription(this.feature, 'warning') }
    public confirmDeleteDexieDB(): string { return this.getDescription(this.feature, 'confirmDeleteDexieDB') }
    public confirmLogout(): string { return this.getDescription(this.feature, 'confirmLogout') }
    public supplierVatNumberIsDuplicate(): string { return this.getDescription(this.feature, 'supplierVatNumberIsDuplicate') }
    public customerAadeDoesNotExist(): string { return this.getDescription(this.feature, 'customerAadeDoesNotExist') }
    public customerVatNumberIsDuplicate(): string { return this.getDescription(this.feature, 'customerVatNumberIsDuplicate') }
    public noRecordsSelected(): string { return this.getDescription(this.feature, 'noRecordsSelected') }
    public policyEndDateMustBeInThePast(): string { return this.getDescription(this.feature, 'policyEndDateMustBeInThePast') }

    public filterResponse(error: any, feature = 'snackbarMessages'): string {
        let returnValue = ''
        this.messages.filter((f: { feature: string; labels: any[] }) => {
            if (f.feature === feature) {
                f.labels.filter(l => {
                    if (l.error == parseInt(error.message)) {
                        returnValue = l.message
                    }
                })
            }
        })
        return returnValue
    }

}
