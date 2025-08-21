import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
import { HttpEventType } from '@angular/common/http'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { map, Observable, startWith } from 'rxjs'
// Custom
import { CashierHttpService } from '../classes/services/cashier-http.service'
import { CashierReadDto } from '../classes/dtos/cashier-read-dto'
import { CashierWriteDto } from '../classes/dtos/cashier-write-dto'
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DebugDialogService } from 'src/app/shared/services/debug-dialog.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'cashier-form',
    templateUrl: './cashier-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './cashier-form.component.css']
})

export class CashierFormComponent {

    //#region common

    private cashier: CashierReadDto
    private cashierId: string
    public feature = 'cashierForm'
    public featureIcon = 'cashiers'
    public form: FormGroup
    public icon = 'arrow_back'
    public imgIsLoaded = false
    public input: InputTabStopDirective
    public parentUrl = '/cashiers'

    //#endregion

    //#region autocompletes

    public dropdownCompanies: Observable<SimpleEntity[]>
    public dropdownSafes: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    //#region documents

    private documents = []
    private renameDocumentForm: FormGroup

    // #endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private debugDialogService: DebugDialogService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private cashierHttpService: CashierHttpService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.initRenameFileForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.setLocale()
        this.getDocuments()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getIcon(filename: string): string {
        return environment.featuresIconDirectory + filename + '.svg'
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public getToday(): Date {
        return new Date()
    }

    public imageIsLoading(): any {
        return this.imgIsLoaded ? '' : 'skeleton'
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public isNewRecord(): boolean {
        return this.form.value.cashierId == ''
    }

    public loadImage(): void {
        this.imgIsLoaded = true
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.cashierHttpService.delete(this.form.value.cashierId).subscribe({
                    complete: () => {
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            }
        })
    }

    public onShowFormValue(): void {
        this.debugDialogService.open(this.form.value, '', ['ok'])
    }

    public onSoftDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.cashierHttpService.softDelete(this.patchFormWithSoftDelete(this.flattenForm())).subscribe({
                    complete: () => {
                        this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            }
        })
    }

    public onOpenDocument(filename: string): void {
        this.cashierHttpService.openDocument(filename).subscribe({
            next: (response) => {
                const blob = new Blob([response], { type: 'application/pdf' })
                const fileURL = URL.createObjectURL(blob)
                window.open(fileURL, '_blank')
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    public onDeleteDocument = (filename: string): any => {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                return new Promise<void>((resolve) => {
                    this.cashierHttpService.deleteDocument(filename).subscribe((x) => {
                        resolve(x)
                        this.getDocuments()
                    })
                })
            }
        })
    }

    public onSave(closeForm: boolean): void {
        this.saveRecord(this.flattenForm(), closeForm)
    }

    public onUploadAndRenameFile(file: File): void {
        this.uploadFile(file).then((x) => {
            this.renameFile(file).then(() => {
                this.getDocuments()
            })
        })
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public showDocuments(): string[] {
        return this.documents ? this.documents : []
    }

    //#endregion

    //#region private methods

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): CashierWriteDto {
        return {
            cashierId: this.form.value.cashierId != '' ? this.form.value.cashierId : null,
            date: this.dateHelperService.formatDateToIso(new Date(this.form.value.date)),
            companyId: this.form.value.company.id,
            safeId: this.form.value.safe.id,
            entry: this.form.value.entry,
            amount: this.form.value.amount,
            remarks: this.form.value.remarks,
            isDeleted: this.form.value.isDeleted,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getDocuments(): void {
        if (this.form.value.cashierId != '') {
            this.cashierHttpService.getDocuments(this.form.value.cashierId).subscribe((x) => {
                this.documents = Array.from(x.body)
            })
        }
    }

    private getRecord(): Promise<any> {
        if (this.cashierId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['cashierForm']
                if (formResolved.error == null) {
                    this.cashier = formResolved.record.body
                    resolve(this.cashier)
                } else {
                    this.dialogService.open(this.messageDialogService.filterResponse(formResolved.error), 'error', ['ok']).subscribe(() => {
                        this.resetForm()
                        this.goBack()
                    })
                }
            })
        }
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            cashierId: '',
            date: ['', [Validators.required]],
            company: ['', [Validators.required, ValidationService.requireAutocomplete]],
            safe: ['', [Validators.required, ValidationService.requireAutocomplete]],
            entry: ['', [Validators.maxLength(1), ValidationService.shouldBePlusOrMinus]],
            amount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            remarks: ['', [Validators.maxLength(2048)]],
            isDeleted: '',
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private initRenameFileForm(): void {
        this.renameDocumentForm = this.formBuilder.group({
            oldfilename: '',
            newfilename: ''
        })
    }

    private patchFormWithSoftDelete(flattenForm: CashierWriteDto): CashierWriteDto {
        flattenForm.isDeleted = true
        return flattenForm
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.form.value.id == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('companies', 'dropdownCompanies', 'company', 'description', 'description')
        this.populateDropdownFromDexieDB('safes', 'dropdownSafes', 'safe', 'description', 'description')
    }

    private populateFields(): void {
        if (this.cashier != undefined) {
            this.form.setValue({
                cashierId: this.cashier.cashierId,
                date: this.cashier.date,
                company: { 'id': this.cashier.company.id, 'description': this.cashier.company.description },
                safe: { 'id': this.cashier.safe.id, 'description': this.cashier.safe.description },
                entry: this.cashier.entry,
                amount: this.cashier.amount,
                remarks: this.cashier.remarks,
                isDeleted: this.cashier.isDeleted,
                postAt: this.cashier.postAt,
                postUser: this.cashier.postUser,
                putAt: this.cashier.putAt,
                putUser: this.cashier.putUser
            })
        }
    }

    private renameFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            this.renameDocumentForm.patchValue({
                oldfilename: file.name,
                newfilename: this.form.value.cashierId + ' ' + file.name
            })
            this.cashierHttpService.rename(this.renameDocumentForm.value).subscribe(x => {
                resolve()
            })
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(cashier: CashierWriteDto, closeForm: boolean): void {
        this.cashierHttpService.saveCashier(cashier).subscribe({
            next: (response) => {
                this.helperService.doPostSaveFormTasks(
                    response.code == 200 ? this.messageDialogService.success() : '',
                    response.code == 200 ? 'ok' : 'ok',
                    this.parentUrl,
                    closeForm)
                this.form.patchValue(
                    {
                        cashierId: response.body.cashierId,
                        postAt: response.body.postAt,
                        putAt: response.body.putAt
                    })
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setLocale(): void {
        this.dateAdapter.setLocale(this.localStorageService.getLanguage())
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.cashierId = x.id
        })
    }

    private uploadFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            const fileToUpload = <File>file
            const formData = new FormData()
            formData.append('x', fileToUpload, fileToUpload.name)
            this.cashierHttpService.upload(formData, { reportProgress: true, observe: 'events' }).subscribe((x) => {
                if (x.type == HttpEventType.Response) {
                    resolve(x)
                }
            })
        })
    }

    //#endregion

    //#region getters

    get date(): AbstractControl {
        return this.form.get('date')
    }

    get company(): AbstractControl {
        return this.form.get('company')
    }

    get safe(): AbstractControl {
        return this.form.get('safe')
    }

    get entry(): AbstractControl {
        return this.form.get('entry')
    }

    get amount(): AbstractControl {
        return this.form.get('amount')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get postAt(): AbstractControl {
        return this.form.get('postAt')
    }

    get postUser(): AbstractControl {
        return this.form.get('postUser')
    }

    get putAt(): AbstractControl {
        return this.form.get('putAt')
    }

    get putUser(): AbstractControl {
        return this.form.get('putUser')
    }

    //#endregion

}
