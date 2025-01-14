import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl, FormArray } from '@angular/forms'
import { HttpEventType } from '@angular/common/http'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { map, Observable, startWith } from 'rxjs'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InvoiceHttpService } from '../classes/services/invoice-http.service'
import { InvoiceReadDto } from '../classes/dtos/invoice-read-dto'
import { InvoiceWriteDto } from '../classes/dtos/invoice-write-dto'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'

@Component({
    selector: 'invoice-form',
    templateUrl: './invoice-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './invoice-form.component.css']
})

export class InvoiceFormComponent {

    //#region common

    private invoice: InvoiceReadDto
    private invoiceId: string
    public feature = 'invoiceForm'
    public featureIcon = 'invoices'
    public form: FormGroup
    public icon = 'arrow_back'
    public imgIsLoaded = false
    public input: InputTabStopDirective
    public parentUrl = '/invoices'

    //#endregion

    //#region autocompletes

    public dropdownCompanies: Observable<SimpleEntity[]>
    public dropdownDocumentTypes: Observable<SimpleEntity[]>
    public dropdownPaymentMethods: Observable<SimpleEntity[]>
    public dropdownSuppliers: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion upload

    //#region documents

    private documents = []
    private renameDocumentForm: FormGroup

    // #endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private invoiceHttpService: InvoiceHttpService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

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

    public getNewOrEditHeader(): string {
        return this.form.value.id == '' ? 'headerNew' : 'headerEdit'
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public imageIsLoading(): any {
        return this.imgIsLoaded ? '' : 'skeleton'
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public isNewRecord(): boolean {
        return this.form.value.id == ''
    }

    public loadImage(): void {
        this.imgIsLoaded = true
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.invoiceHttpService.delete(this.form.value.id).subscribe({
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

    public onSoftDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.invoiceHttpService.softDelete(this.patchFormWithSoftDelete(this.flattenForm())).subscribe({
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
        this.invoiceHttpService.openDocument(filename).subscribe({
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
                    this.invoiceHttpService.deleteDocument(filename).subscribe((x) => {
                        resolve(x)
                        this.getDocuments()
                    })
                })
            }
        })
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
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

    private flattenForm(): InvoiceWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            companyId: this.form.value.company.id,
            supplierId: this.form.value.supplier.id,
            documentTypeId: this.form.value.documentType.id,
            paymentMethodId: this.form.value.paymentMethod.id,
            date: this.dateHelperService.formatDateToIso(new Date(this.form.value.date)),
            documentNo: this.form.value.documentNo,
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
        if (this.invoiceId != undefined) {
            this.invoiceHttpService.getDocuments(this.invoiceId).subscribe((x) => {
                this.documents = Array.from(x.body)
            })
        }
    }

    private getRecord(): Promise<any> {
        if (this.invoiceId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['invoiceForm']
                if (formResolved.error == null) {
                    this.invoice = formResolved.record.body
                    resolve(this.invoice)
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
            id: '',
            date: ['', [Validators.required]],
            company: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            supplier: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentType: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            paymentMethod: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentNo: ['', [Validators.required, Validators.maxLength(128)]],
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

    private patchFormWithSoftDelete(flattenForm: InvoiceWriteDto): InvoiceWriteDto {
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
        this.populateDropdownFromDexieDB('documentTypes', 'dropdownDocumentTypes', 'documentType', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentMethods', 'dropdownPaymentMethods', 'paymentMethod', 'description', 'description')
        this.populateDropdownFromDexieDB('suppliers', 'dropdownSuppliers', 'supplier', 'description', 'description')
    }

    private populateFields(): void {
        if (this.invoice != undefined) {
            this.form.setValue({
                id: this.invoice.id,
                date: this.invoice.date,
                company: { 'id': this.invoice.company.id, 'description': this.invoice.company.description },
                supplier: { 'id': this.invoice.supplier.id, 'description': this.invoice.supplier.description },
                documentType: { 'id': this.invoice.documentType.id, 'description': this.invoice.documentType.description },
                paymentMethod: { 'id': this.invoice.paymentMethod.id, 'description': this.invoice.paymentMethod.description },
                documentNo: this.invoice.documentNo,
                amount: this.invoice.amount,
                remarks: this.invoice.remarks,
                isDeleted: this.invoice.isDeleted,
                postAt: this.invoice.postAt,
                postUser: this.invoice.postUser,
                putAt: this.invoice.putAt,
                putUser: this.invoice.putUser
            })
        }
    }

    private renameFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            this.renameDocumentForm.patchValue({
                oldfilename: file.name,
                newfilename: this.form.value.id + ' ' + file.name
            })
            this.invoiceHttpService.rename(this.renameDocumentForm.value).subscribe(x => {
                resolve()
            })
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(invoice: InvoiceWriteDto): void {
        this.invoiceHttpService.saveInvoice(invoice).subscribe({
            next: (response) => {
                this.helperService.doPostSaveFormTasks(
                    response.code == 200 ? this.messageDialogService.success() : '',
                    response.code == 200 ? 'ok' : 'ok',
                    this.parentUrl,
                    true)
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
            this.invoiceId = x.id
        })
    }

    private uploadFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            const fileToUpload = <File>file
            const formData = new FormData()
            formData.append('x', fileToUpload, fileToUpload.name)
            this.invoiceHttpService.upload(formData, { reportProgress: true, observe: 'events' }).subscribe((x) => {
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

    get documentType(): AbstractControl {
        return this.form.get('documentType')
    }

    get documentNo(): AbstractControl {
        return this.form.get('documentNo')
    }

    get paymentMethod(): AbstractControl {
        return this.form.get('paymentMethod')
    }

    get company(): AbstractControl {
        return this.form.get('company')
    }

    get supplier(): AbstractControl {
        return this.form.get('supplier')
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
