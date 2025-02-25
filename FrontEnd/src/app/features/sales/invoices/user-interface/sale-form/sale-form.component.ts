import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Component, ElementRef, Renderer2 } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { CustomerAutoCompleteVM } from '../../../customers/classes/view-models/customer-autocomplete-vm'
import { DebugDialogService } from 'src/app/shared/services/debug-dialog.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeHttpService } from '../../../documentTypes/classes/services/documentType-http.service'
import { DocumentTypeListVM } from '../../../documentTypes/classes/view-models/documentType-list-vm'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LineItemVM } from '../../classes/view-models/form/line-item-vm'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PriceHttpService } from '../../../prices/classes/services/price-http.service'
import { SaleHelperService } from '../../classes/services/sale.helper.service'
import { SaleHttpDataService } from '../../classes/services/sale-http-data.service'
import { SaleReadDto } from '../../classes/dtos/form/sale-read-dto'
import { SaleTotalsVM } from '../../classes/view-models/form/sale-totals-vm'
import { SaleWriteDto } from '../../classes/dtos/form/sale-write-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'sale-form',
    templateUrl: './sale-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './sale-form.component.css']
})

export class SaleFormComponent {

    //#region variables

    private record: SaleReadDto
    private invoiceId: string
    public feature = 'saleForm'
    public featureIcon = 'sales'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public isNewRecord: boolean
    public itemsArray: string[] = []
    public parentUrl = '/sales'
    public saleTotals: SaleTotalsVM

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<CustomerAutoCompleteVM[]>
    public dropdownDocumentTypes: Observable<DocumentTypeListVM[]>
    public dropdownPaymentMethods: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateAdapter: DateAdapter<any>, private debugDialogService: DebugDialogService, private dexieService: DexieService, private dialogService: DialogService, private documentTypeHttpService: DocumentTypeHttpService, private elementRef: ElementRef, private formBuilder: FormBuilder, private helperService: HelperService, private priceHttpService: PriceHttpService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private renderer: Renderer2, private router: Router, private saleHelperService: SaleHelperService, private saleHttpInvoice: SaleHttpDataService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.populateDropdowns()
        this.getRecord()
        this.populateFields()
        this.setLocale()
        this.addItem()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public getDocumentTypeDescription(): string[] {
        const x = []
        x.push(this.form.value.documentTypeDescription + ' ')
        x.push(this.form.value.documentTypeDescription ? 'ΣΕΙΡΑ ' + this.form.value.batch + ' ' : '')
        x.push(this.form.value.documentTypeDescription ? 'NO ' + this.form.value.invoiceNo : '')
        return x
    }

    public getItemDescription(itemIndex: number) {
        const item = (<FormArray>this.form.get("items")).at(itemIndex);
        return item.value.code != '' && item.value.description != '' ? item.value.description : '·'
    }

    public onCalculateLine(index: number): void {
        const x = this.calculateLine(index)
        const items = this.form.get('items') as FormArray
        const item = items.at(index)
        item.patchValue({
            subTotal: x.subTotal,
            vatAmount: x.vatAmount,
            grossAmount: x.grossAmount,
        })
        this.calculateTotals()
    }

    public onAddItem(): void {
        const control = <FormArray>this.form.get('items')
        const newGroup = this.formBuilder.group({
            code: ['', Validators.required],
            description: '',
            englishDescription: '',
            quantity: [0, Validators.required],
            itemPrice: [0, Validators.required],
            subTotal: 0,
            vatPercent: [this.form.value.customer.vatPercent || 0, Validators.required],
            vatAmount: 0,
            grossAmount: [0, Validators.required],
        })
        control.push(newGroup)
        this.itemsArray.push(this.form.controls.items.value)
        this.helperService.addTabIndexToInput(this.elementRef, this.renderer)
    }

    public onRemoveItem(itemIndex: number): void {
        this.removeItem(itemIndex)
        this.addTabIndexToInput()
        this.calculateTotals()
    }

    public onSeekItem(event: Event, index: number): void {
        const x = (event.target as HTMLInputElement).value
        if (x != '') {
            this.priceHttpService.getByCode(x).subscribe({
                next: (response) => {
                    const items = (<FormArray>this.form.get("items")).at(index);
                    items.patchValue({
                        code: response.body.code,
                        description: response.body.description,
                        itemPrice: response.body.netAmount,
                        vatPercent: this.form.value.customer.vatPercent || 0
                    })
                },
                error: () => {
                    const items = (<FormArray>this.form.get("items")).at(index);
                    items.patchValue({
                        code: '',
                        description: '',
                        itemPrice: 0,
                        vatPercent: 0
                    })
                }
            })
        }
    }

    public onDoSubmitTasks(): void {
        // re-send to data-up
    }

    public onShowFormValue(): void {
        this.debugDialogService.open(this.form.value, '', ['ok'])
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isEditingNotAllowed(): boolean {
        return this.invoiceId == undefined
    }

    public isEmailSent(): boolean {
        return this.form.value.isEmailSent
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public updateFieldsAfterDocumentTypeSelection(value: DocumentTypeListVM): void {
        this.documentTypeHttpService.getSingle(value.id).subscribe({
            next: (response) => {
                const x: DocumentTypeReadDto = response.body
                this.form.patchValue({
                    documentTypeDescription: x.description,
                    batch: x.batch
                })
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
    }

    public async updateFieldsAfterCustomerSelection(value: SimpleEntity): Promise<void> {
        await this.dexieService.getById('customers', value.id).then(response => {
            this.form.patchValue({
                vatPercent: response.vatPercent
            })
            // this.calculateTotals()
        })
    }

    //#endregion

    //#region private methods

    private addItem(): void {
        if (this.record == undefined) {
            this.onAddItem()
        }
    }

    private addTabIndexToInput() {
        this.helperService.addTabIndexToInput(this.elementRef, this.renderer)
    }

    private calculateLine(index: number): LineItemVM {
        const quantity = parseFloat(this.form.value.items[index].quantity)
        const itemPrice = parseFloat(this.form.value.items[index].itemPrice)
        const subTotal = quantity * itemPrice
        const vatPercent = parseFloat(this.form.value.items[index].vatPercent) / 100
        const vatAmount = subTotal * vatPercent
        const grossAmount = subTotal + vatAmount
        const x: LineItemVM = {
            quantity: quantity,
            itemPrice: itemPrice,
            vatPercent: vatPercent,
            subTotal: subTotal,
            vatAmount: vatAmount,
            grossAmount: grossAmount
        }
        return x
    }

    private calculateTotals(): void {
        const x = this.form.get('items') as FormArray
        const items = x.value
        let saleNetAmount = 0
        let saleVatAmount = 0
        let saleGrossAmount = 0
        items.forEach(item => {
            saleNetAmount += parseFloat(item.subTotal)
            saleVatAmount += parseFloat(item.vatAmount)
            saleGrossAmount += item.subTotal + item.vatAmount
        })
        let saleTotals: SaleTotalsVM
        saleTotals = {
            netAmount: saleNetAmount,
            vatAmount: saleVatAmount,
            grossAmount: saleGrossAmount
        }
        this.form.patchValue({
            saleNetAmount: saleTotals.netAmount,
            saleVatAmount: saleTotals.vatAmount,
            saleGrossAmount: saleTotals.grossAmount
        })
    }

    private flattenForm(): SaleWriteDto {
        return this.saleHelperService.flattenForm(this.form.value)
    }

    private getRecord(): Promise<any> {
        if (this.invoiceId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['saleForm']
                if (formResolved.error == null) {
                    this.record = formResolved.record.body
                    resolve(this.record)
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
            invoiceId: '',
            date: [new Date(), [Validators.required]],
            invoiceNo: 0,
            customer: ['', [Validators.required, ValidationService.requireAutocomplete]],
            documentType: ['', [Validators.required, ValidationService.requireAutocomplete]],
            documentTypeDescription: '',
            batch: '',
            paymentMethod: ['', [Validators.required, ValidationService.requireAutocomplete]],
            saleNetAmount: [0],
            saleVatAmount: [0],
            saleGrossAmount: [0],
            remarks: ['', Validators.maxLength(128)],
            isEmailSent: false,
            isCancelled: false,
            items: this.formBuilder.array([]),
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private isCustomerDataValid(): Promise<any> {
        return new Promise((resolve) => {
            this.saleHttpInvoice.validateCustomerData(this.form.value.customer.id).subscribe({
                next: (response) => {
                    resolve(response.body.isValid)
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        })
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.invoiceId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description')
        this.populateDropdownFromDexieDB('saleDocumentTypes', 'dropdownDocumentTypes', 'documentType', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentMethods', 'dropdownPaymentMethods', 'paymentMethod', 'description', 'description')
        this.populateDropdownFromDexieDB('prices', 'dropdownItems', 'items.code', 'code', 'code')
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.patchValue({
                invoiceId: this.record.invoiceId,
                date: this.record.date,
                customer: { 'id': this.record.customer.id, 'description': this.record.customer.description, 'vatPercent': this.record.customer.vatPercent },
                documentType: { 'id': this.record.documentType.id, 'abbreviation': this.record.documentType.abbreviation },
                documentTypeDescription: this.record.documentType.description,
                invoiceNo: this.record.invoiceNo,
                batch: this.record.documentType.batch,
                paymentMethod: { 'id': this.record.paymentMethod.id, 'description': this.record.paymentMethod.description },
                remarks: this.record.remarks,
                isEmailSent: this.record.isEmailSent,
                isCancelled: this.record.isCancelled,
                saleNetAmount: this.record.netAmount,
                saleVatAmount: this.record.vatAmount,
                saleGrossAmount: this.record.grossAmount,
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser,
                items: this.populateItems(),
            })
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(sale: SaleWriteDto): void {
        this.saleHttpInvoice.save(sale).subscribe({
            next: () => {
                this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
            },
            error: (errorFromInterceptor: any) => {
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

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private populateItems(): void {
        if (this.record) {
            if (this.record.items.length >= 1) {
                this.record.items.forEach(item => {
                    const control = <FormArray>this.form.get('items')
                    const newGroup = this.formBuilder.group({
                        code: item.code,
                        description: item.description,
                        englishDescription: item.englishDescription,
                        quantity: item.quantity,
                        itemPrice: item.netAmount,
                        subTotal: item.quantity * item.netAmount,
                        vatPercent: item.vatPercent,
                        vatAmount: item.vatAmount,
                        grossAmount: item.grossAmount
                    })
                    control.push(newGroup)
                    this.itemsArray.push(this.form.controls.items.value)
                })
            } else {
                this.onAddItem()
            }
        } else {
            this.onAddItem()
        }
    }

    private removeItem(itemIndex: number) {
        const items = <FormArray>this.form.get('items')
        items.removeAt(itemIndex)
        this.itemsArray.splice(itemIndex, 1)
    }

    //#endregion

    //#region getters

    get date(): AbstractControl {
        return this.form.get('date')
    }

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get documentType(): AbstractControl {
        return this.form.get('documentType')
    }

    get paymentMethod(): AbstractControl {
        return this.form.get('paymentMethod')
    }

    get item(): AbstractControl {
        return this.form.get('item.code')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get saleNetAmount(): AbstractControl {
        return this.form.get('saleNetAmount')
    }

    get saleVatAmount(): AbstractControl {
        return this.form.get('saleVatAmount')
    }

    get saleGrossAmount(): AbstractControl {
        return this.form.get('saleGrossAmount')
    }

    //#endregion

}