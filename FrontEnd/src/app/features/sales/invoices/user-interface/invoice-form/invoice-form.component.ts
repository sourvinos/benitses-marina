import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { Observable, map, startWith } from 'rxjs'
// Custom
import { BillingCriteriaVM } from '../../classes/view-models/form/billing-criteria-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { DocumentTypeHttpService } from '../../../documentTypes/classes/services/documentType-http.service'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InvoiceHelperService } from '../../classes/services/invoice.helper.service'
import { InvoiceHttpPdfService } from '../../classes/services/invoice-http-pdf.service'
import { InvoiceHttpDataService } from '../../classes/services/invoice-http-data.service'
import { InvoiceReadDto } from '../../classes/dtos/form/invoice-read-dto'
import { InvoiceWriteDto } from '../../classes/dtos/form/invoice-write-dto'
import { InvoiceXmlHelperService } from '../../classes/services/invoice-xml-helper.service'
import { InvoiceHttpXmlService } from '../../classes/services/invoice-http-xml.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PriceHttpService } from '../../../prices/classes/services/price-http.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { MatDialog } from '@angular/material/dialog'
import { CustomerAutoCompleteVM } from '../../../customers/classes/view-models/customer-autocomplete-vm'
import { DocumentTypeListVM } from '../../../documentTypes/classes/view-models/documentType-list-vm'
import { DebugDialogService } from 'src/app/shared/services/debug-dialog.service'

@Component({
    selector: 'invoice-form',
    templateUrl: './invoice-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './invoice-form.component.css']
})

export class InvoiceFormComponent {

    //#region common

    private record: InvoiceReadDto
    private recordId: string
    public feature = 'invoiceForm'
    public featureIcon = 'invoices'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/invoices'

    //#endregion

    //#region specific

    public isNewRecord: boolean

    //#endregion

    //#region autocompletes

    public isAutoCompleteDisabled = true
    public dropdownCustomers: Observable<CustomerAutoCompleteVM[]>
    public dropdownDocumentTypes: Observable<DocumentTypeListVM[]>
    public dropdownPaymentMethods: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateHelperService: DateHelperService, private debugDialogService: DebugDialogService, private dexieService: DexieService, private dialogService: DialogService, private documentTypeHttpService: DocumentTypeHttpService, private formBuilder: FormBuilder, private helperService: HelperService, private invoiceHelperService: InvoiceHelperService, private invoiceHttpPdfService: InvoiceHttpPdfService, private invoiceHttpService: InvoiceHttpDataService, private invoiceXmlHelperService: InvoiceXmlHelperService, private invoiceXmlHttpService: InvoiceHttpXmlService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private priceHttpService: PriceHttpService, private router: Router, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.populateDocumentTypesAfterLoadRecord()
        this.onDoPortCalculations()
    }

    ngAfterViewInit(): void {
        this.leftAlignLastTab()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public onComparePriceTotals(): boolean {
        const x = parseFloat(this.form.value.grossAmount)
        const z = parseFloat(this.form.value.portTotals.total_Amount)
        return x == z || z == 0
    }


    public onDoPortCalculations(): void {
        this.patchFormWithCalculations(
            this.invoiceHelperService.calculatePortA(this.form.value),
            this.invoiceHelperService.calculatePortB(this.form.value),
            this.invoiceHelperService.calculatePortTotals(this.form.value))
    }

    public onDoSummaryCalculations(): void {
        this.calculateSummary()
    }

    public onDoSubmitTasks(): void {
        this.doSubmitTasks()
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

    public isEditingAllowed(): boolean {
        return this.recordId == undefined
    }

    public isEmailSent(): boolean {
        return this.form.value.isEmailSent
    }

    public isCancelled(): boolean {
        return this.form.value.isCancelled
    }

    public isSubmitted(): boolean {
        return this.form.value.aade.mark != ''
    }

    public onSave(): void {
        this.isCustomerDataValid().then((response) => {
            response
                ? this.saveRecord(this.flattenForm())
                : this.dialogService.open(this.messageDialogService.customerDataIsInvalid(), 'error', ['ok'])
        })
    }

    public updateFieldsAfterDocumentTypeSelection(value: DocumentTypeListVM): void {
        this.documentTypeHttpService.getSingle(value.id).subscribe({
            next: (response) => {
                const x: DocumentTypeReadDto = response.body
                this.getLastDocumentTypeNo(value.id).subscribe(response => {
                    this.form.patchValue({
                        documentTypeDescription: x.description,
                        invoiceNo: response.body + 1,
                        batch: x.batch
                    })
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

    public onCreateAndOpenPdf(): void {
        this.invoiceHttpPdfService.buildPdf(new Array(this.form.value.invoiceId)).subscribe({
            next: (response) => {
                this.invoiceHttpPdfService.openPdf(response.body[0]).subscribe({
                    next: (response) => {
                        const blob = new Blob([response], { type: 'application/pdf' })
                        const fileURL = URL.createObjectURL(blob)
                        window.open(fileURL, '_blank')
                    },
                    error: (errorFromInterceptor) => {
                        this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                    }
                })
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    public onUpdateInvoiceWithOutputPort(port: any, portIndex: number): void {
        this.form.value.invoicesPorts[portIndex] = port
    }

    public updateFieldsAfterShipSelection(value: SimpleEntity): void {
        this.form.patchValue({
            documentType: '',
            documentTypeDescription: '',
            invoiceNo: 0,
            batch: ''
        })
        this.populateDocumentTypesAfterShipSelection('documentTypesInvoice', 'dropdownDocumentTypes', 'documentType', 'abbreviation', 'abbreviation', value.id)
    }

    public async updateFieldsAfterCustomerSelection(value: SimpleEntity): Promise<void> {
        await this.dexieService.getById('customers', value.id).then(response => {
            this.form.patchValue({
                vatPercent: response.vatPercent
            })
            this.calculateSummary()
        })
    }

    //#endregion

    //#region private methods

    private doSubmitTasks(): void {
        this.invoiceXmlHttpService.get(this.form.value.invoiceId).subscribe(response => {
            this.invoiceXmlHttpService.uploadInvoice(response.body).subscribe({
                next: (response) => {
                    this.invoiceHttpService.updateInvoiceAade(this.invoiceXmlHelperService.processInvoiceSuccessResponse(response)).subscribe({
                        next: () => {
                            this.helperService.doPostSaveFormTasks(this.messageDialogService.success(), 'ok', this.parentUrl, true)
                        },
                        error: (errorFromInterceptor) => {
                            this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                        }
                    })
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        })
    }

    private flattenForm(): InvoiceWriteDto {
        return this.invoiceHelperService.flattenForm(this.form.value)
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['invoiceForm']
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
            tripDate: [new Date(), [Validators.required]],
            customer: ['', [Validators.required, ValidationService.requireAutocomplete]],
            destination: ['', [Validators.required, ValidationService.requireAutocomplete]],
            documentType: ['', [Validators.required, ValidationService.requireAutocomplete]],
            documentTypeDescription: '',
            batch: '',
            invoiceNo: 0,
            paymentMethod: ['', [Validators.required, ValidationService.requireAutocomplete]],
            ship: ['', [Validators.required, ValidationService.requireAutocomplete]],
            netAmount: [0, ValidationService.isGreaterThanZero],
            vatPercent: [0],
            vatAmount: [0],
            grossAmount: [0, [Validators.required, Validators.min(1), Validators.max(99999)]],
            portA: this.formBuilder.group({
                id: 0,
                invoiceId: '',
                portId: 1,
                port_A_Description: 'CORFU PORT',
                adults_A_WithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_AmountWithTransfer: [0, [Validators.required]],
                adults_A_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_A_AmountWithoutTransfer: [0, [Validators.required]],
                kids_A_WithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_PriceWithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_AmountWithTransfer: [0, [Validators.required]],
                kids_A_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_PriceWithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_A_AmountWithoutTransfer: [0, [Validators.required]],
                free_A_WithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_A_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                total_A_Persons: [0],
                total_A_Amount: [0]
            }),
            portB: this.formBuilder.group({
                id: 0,
                invoiceId: '',
                portId: 2,
                port_B_Description: 'LEFKIMMI PORT',
                adults_B_WithTransfer: [0, [Validators.required, Validators.maxLength(3)]],
                adults_B_PriceWithTransfer: [0, [Validators.required, Validators.maxLength(6)]],
                adults_B_AmountWithTransfer: [0, [Validators.required]],
                adults_B_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                adults_B_PriceWithoutTransfer: [0, [Validators.required, Validators.maxLength(6)]],
                adults_B_AmountWithoutTransfer: [0, [Validators.required]],
                kids_B_WithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_PriceWithTransfer: [0, [Validators.required, Validators.maxLength(6)]],
                kids_B_AmountWithTransfer: [0, [Validators.required]],
                kids_B_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                kids_B_PriceWithoutTransfer: [0, [Validators.required, Validators.maxLength(6)]],
                kids_B_AmountWithoutTransfer: [0, [Validators.required]],
                free_B_WithTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                free_B_WithoutTransfer: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
                total_B_Persons: [0],
                total_B_Amount: [0]
            }),
            portTotals: this.formBuilder.group({
                port_Totals_Description: '',
                adults_Total_WithTransfer: 0,
                adults_TotalAmount_WithTransfer: 0,
                adults_Total_WithoutTransfer: 0,
                adults_TotalAmount_WithoutTransfer: 0,
                kids_Total_WithTransfer: 0,
                kids_TotalAmount_WithTransfer: 0,
                kids_Total_WithoutTransfer: 0,
                kids_TotalAmount_WithoutTransfer: 0,
                free_Total_WithTransfer: 0,
                free_Total_WithoutTransfer: 0,
                total_Persons: 0,
                total_Amount: 0
            }),
            aade: this.formBuilder.group({
                id: 0,
                uId: '',
                mark: '',
                markCancel: '',
                invoiceId: '',
                qrUrl: '',
            }),
            remarks: ['', Validators.maxLength(128)],
            isEmailSent: false,
            isCancelled: false,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: [''],
        })
    }

    private getLastDocumentTypeNo(id: number): Observable<any> {
        return this.documentTypeHttpService.getLastDocumentTypeNo(id)
    }

    private isCustomerDataValid(): Promise<any> {
        return new Promise((resolve) => {
            this.invoiceHttpService.validateCustomerData(this.form.value.customer.id).subscribe({
                next: (response) => {
                    resolve(response.body.isValid)
                },
                error: (errorFromInterceptor) => {
                    this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
                }
            })
        })
    }

    private patchFormWithCalculations(calculationsA: any, calculationsB: any, calculationTotals: any): void {
        this.form.patchValue({
            portA: {
                adults_A_AmountWithTransfer: calculationsA.adults_A_AmountWithTransfer,
                adults_A_AmountWithoutTransfer: calculationsA.adults_A_AmountWithoutTransfer,
                kids_A_AmountWithTransfer: calculationsA.kids_A_AmountWithTransfer,
                kids_A_AmountWithoutTransfer: calculationsA.kids_A_AmountWithoutTransfer,
                total_A_Persons: calculationsA.total_A_Persons,
                total_A_Amount: calculationsA.total_A_Amount
            },
            portB: {
                adults_B_AmountWithTransfer: calculationsB.adults_B_AmountWithTransfer,
                adults_B_AmountWithoutTransfer: calculationsB.adults_B_AmountWithoutTransfer,
                kids_B_AmountWithTransfer: calculationsB.kids_B_AmountWithTransfer,
                kids_B_AmountWithoutTransfer: calculationsB.kids_B_AmountWithoutTransfer,
                total_B_Persons: calculationsB.total_B_Persons,
                total_B_Amount: calculationsB.total_B_Amount
            },
            portTotals: {
                adults_Total_WithTransfer: calculationTotals.adultsWithTransfer,
                adults_TotalAmount_WithTransfer: calculationsA.adults_A_AmountWithTransfer + calculationsB.adults_B_AmountWithTransfer,
                adults_Total_WithoutTransfer: calculationTotals.adultsWithoutTransfer,
                adults_TotalAmount_WithoutTransfer: calculationsA.adults_A_AmountWithoutTransfer + calculationsB.adults_B_AmountWithoutTransfer,
                kids_Total_WithTransfer: calculationTotals.kidsWithTransfer,
                kids_TotalAmount_WithTransfer: calculationsA.kids_A_AmountWithTransfer + calculationsB.kids_B_AmountWithTransfer,
                kids_Total_WithoutTransfer: calculationTotals.kidsWithoutTransfer,
                kids_TotalAmount_WithoutTransfer: calculationsA.kids_A_AmountWithoutTransfer + calculationsB.kids_B_AmountWithoutTransfer,
                free_Total_WithTransfer: calculationTotals.freeWithTransfer,
                free_Total_WithoutTransfer: calculationTotals.freeWithoutTransfer,
                total_Persons: calculationTotals.totalPersons,
                total_Amount: calculationTotals.totalAmount
            }
        })
    }

    private populateDocumentTypesAfterLoadRecord(): void {
        if (this.record != undefined) {
            this.populateDocumentTypesAfterShipSelection('documentTypesInvoice', 'dropdownDocumentTypes', 'documentType', 'abbreviation', 'abbreviation', this.record.ship.id)
        }
    }

    private populateDocumentTypesAfterShipSelection(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string, shipId: number): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = response.filter(x => x.ship.id == shipId).filter(x => x.isActive)
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.recordId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('customers', 'dropdownCustomers', 'customer', 'description', 'description')
        this.populateDropdownFromDexieDB('destinations', 'dropdownDestinations', 'destination', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentMethods', 'dropdownPaymentMethods', 'paymentMethod', 'description', 'description')
        this.populateDropdownFromDexieDB('ships', 'dropdownShips', 'ship', 'description', 'description')
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.patchValue({
                invoiceId: this.record.invoiceId,
                date: this.record.date,
                tripDate: this.record.tripDate,
                customer: { 'id': this.record.customer.id, 'description': this.record.customer.description },
                destination: { 'id': this.record.destination.id, 'description': this.record.destination.description },
                documentType: { 'id': this.record.documentType.id, 'abbreviation': this.record.documentType.abbreviation },
                documentTypeDescription: this.record.documentType.description,
                invoiceNo: this.record.invoiceNo,
                batch: this.record.documentType.batch,
                paymentMethod: { 'id': this.record.paymentMethod.id, 'description': this.record.paymentMethod.description },
                ship: { 'id': this.record.ship.id, 'description': this.record.ship.description },
                adults: this.record.adults,
                kids: this.record.kids,
                free: this.record.free,
                totalPax: this.record.totalPax,
                remarks: this.record.remarks,
                isEmailSent: this.record.isEmailSent,
                isCancelled: this.record.isCancelled,
                netAmount: this.record.netAmount,
                vatPercent: this.record.vatPercent,
                vatAmount: this.record.vatAmount,
                grossAmount: this.record.grossAmount,
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser,
                aade: {
                    invoiceId: this.record.aade.invoiceId,
                    uId: this.record.aade.uId,
                    mark: this.record.aade.mark,
                    markCancel: this.record.aade.markCancel,
                    qrUrl: this.record.aade.qrUrl
                },
                portA: {
                    id: this.record.invoicesPorts[0].id,
                    invoiceId: this.record.invoicesPorts[0].invoiceId,
                    portId: this.record.invoicesPorts[0].port.id,
                    port_A_Description: this.record.invoicesPorts[0].port.description,
                    adults_A_WithTransfer: this.record.invoicesPorts[0].adultsWithTransfer,
                    adults_A_PriceWithTransfer: this.record.invoicesPorts[0].adultsPriceWithTransfer,
                    adults_A_WithoutTransfer: this.record.invoicesPorts[0].adultsWithoutTransfer,
                    adults_A_PriceWithoutTransfer: this.record.invoicesPorts[0].adultsPriceWithoutTransfer,
                    kids_A_WithTransfer: this.record.invoicesPorts[0].kidsWithTransfer,
                    kids_A_PriceWithTransfer: this.record.invoicesPorts[0].kidsPriceWithTransfer,
                    kids_A_WithoutTransfer: this.record.invoicesPorts[0].kidsWithoutTransfer,
                    kids_A_PriceWithoutTransfer: this.record.invoicesPorts[0].kidsPriceWithoutTransfer,
                    free_A_WithTransfer: this.record.invoicesPorts[0].freeWithTransfer,
                    free_A_WithoutTransfer: this.record.invoicesPorts[0].freeWithoutTransfer
                },
                portB: {
                    id: this.record.invoicesPorts[1].id,
                    invoiceId: this.record.invoicesPorts[1].invoiceId,
                    portId: this.record.invoicesPorts[1].port.id,
                    port_B_Description: this.record.invoicesPorts[1].port.description,
                    adults_B_WithTransfer: this.record.invoicesPorts[1].adultsWithTransfer,
                    adults_B_PriceWithTransfer: this.record.invoicesPorts[1].adultsPriceWithTransfer,
                    adults_B_WithoutTransfer: this.record.invoicesPorts[1].adultsWithoutTransfer,
                    adults_B_PriceWithoutTransfer: this.record.invoicesPorts[1].adultsPriceWithoutTransfer,
                    kids_B_WithTransfer: this.record.invoicesPorts[1].kidsWithTransfer,
                    kids_B_PriceWithTransfer: this.record.invoicesPorts[1].kidsPriceWithTransfer,
                    kids_B_WithoutTransfer: this.record.invoicesPorts[1].kidsWithoutTransfer,
                    kids_B_PriceWithoutTransfer: this.record.invoicesPorts[1].kidsPriceWithoutTransfer,
                    free_B_WithTransfer: this.record.invoicesPorts[1].freeWithTransfer,
                    free_B_WithoutTransfer: this.record.invoicesPorts[1].freeWithoutTransfer
                }
            })
        }
    }

    private calculateSummary(): void {
        const grossAmount = parseFloat(this.form.value.grossAmount)
        const vatPercent = parseFloat(this.form.value.vatPercent) / 100
        const netAmount = grossAmount / (1 + vatPercent)
        const vatAmount = netAmount * vatPercent
        this.form.patchValue({
            netAmount: netAmount.toFixed(2),
            vatAmount: vatAmount.toFixed(2),
            grossAmount: grossAmount.toFixed(2)
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(invoice: InvoiceWriteDto): void {
        this.invoiceHttpService.save(invoice).subscribe({
            next: (response) => {
                this.form.patchValue({
                    invoiceId: response.id
                })
                this.doSubmitTasks()
            },
            error: (errorFromInterceptor) => {
                this.dialogService.open(this.messageDialogService.filterResponse(errorFromInterceptor), 'error', ['ok'])
            }
        })
    }

    private setRecordId(): void {
        this.activatedRoute.params.subscribe(x => {
            this.recordId = x.id
        })
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private leftAlignLastTab(): void {
        this.helperService.leftAlignLastTab()
    }

    //#endregion

    //#region getters

    get date(): AbstractControl {
        return this.form.get('date')
    }

    get tripDate(): AbstractControl {
        return this.form.get('tripDate')
    }

    get documentType(): AbstractControl {
        return this.form.get('documentType')
    }

    get paymentMethod(): AbstractControl {
        return this.form.get('paymentMethod')
    }

    get customer(): AbstractControl {
        return this.form.get('customer')
    }

    get destination(): AbstractControl {
        return this.form.get('destination')
    }

    get ship(): AbstractControl {
        return this.form.get('ship')
    }

    get netAmount(): AbstractControl {
        return this.form.get('netAmount')
    }

    get vatPercent(): AbstractControl {
        return this.form.get('vatPercent')
    }

    get vatAmount(): AbstractControl {
        return this.form.get('vatAmount')
    }

    get grossAmount(): AbstractControl {
        return this.form.get('grossAmount')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get port_A_Description(): AbstractControl {
        return this.form.get('portA.portDescription')
    }

    get adults_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_WithTransfer')
    }

    get adults_A_PriceWithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_PriceWithTransfer')
    }

    get adults_A_AmountWithTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_AmountWithTransfer')
    }

    get adults_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_WithoutTransfer')
    }

    get adults_A_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_PriceWithoutTransfer')
    }

    get adults_A_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portA.adults_A_AmountWithoutTransfer')
    }

    get kids_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_WithTransfer')
    }

    get kids_A_PriceWithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_PriceWithTransfer')
    }

    get kids_A_AmountWithTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_AmountWithTransfer')
    }

    get kids_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_WithoutTransfer')
    }

    get kids_A_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_PriceWithoutTransfer')
    }

    get kids_A_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portA.kids_A_AmountWithoutTransfer')
    }

    get free_A_WithTransfer(): AbstractControl {
        return this.form.get('portA.free_A_WithTransfer')
    }

    get free_A_WithoutTransfer(): AbstractControl {
        return this.form.get('portA.free_A_WithoutTransfer')
    }

    get total_A_Persons(): AbstractControl {
        return this.form.get('total_A_Persons')
    }

    get total_A_Amount(): AbstractControl {
        return this.form.get('total_A_Amount')
    }

    get port_B_Description(): AbstractControl {
        return this.form.get('portB.portDescription')
    }

    get adults_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_WithTransfer')
    }

    get adults_B_PriceWithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_PriceWithTransfer')
    }

    get adults_B_AmountWithTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_AmountWithTransfer')
    }

    get adults_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_WithoutTransfer')
    }

    get adults_B_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_PriceWithoutTransfer')
    }

    get adults_B_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portB.adults_B_AmountWithoutTransfer')
    }

    get kids_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_WithTransfer')
    }

    get kids_B_PriceWithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_PriceWithTransfer')
    }

    get kids_B_AmountWithTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_AmountWithTransfer')
    }

    get kids_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_WithoutTransfer')
    }

    get kids_B_PriceWithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_PriceWithoutTransfer')
    }

    get kids_B_AmountWithoutTransfer(): AbstractControl {
        return this.form.get('portB.kids_B_AmountWithoutTransfer')
    }

    get free_B_WithTransfer(): AbstractControl {
        return this.form.get('portB.free_B_WithTransfer')
    }

    get free_B_WithoutTransfer(): AbstractControl {
        return this.form.get('portB.free_B_WithoutTransfer')
    }

    get total_B_Persons(): AbstractControl {
        return this.form.get('total_B_Persons')
    }

    get total_B_Amount(): AbstractControl {
        return this.form.get('total_B_Amount')
    }

    get adults_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_Total_WithTransfer')
    }

    get adults_TotalAmount_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_TotalAmount_WithTransfer')
    }

    get adults_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_Total_WithoutTransfer')
    }

    get adults_TotalAmount_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.adults_TotalAmount_WithoutTransfer')
    }

    get kids_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_Total_WithTransfer')
    }

    get kids_TotalAmount_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_TotalAmount_WithTransfer')
    }

    get kids_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_Total_WithoutTransfer')
    }

    get kids_TotalAmount_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.kids_TotalAmount_WithoutTransfer')
    }

    get free_Total_WithTransfer(): AbstractControl {
        return this.form.get('portTotals.free_Total_WithTransfer')
    }

    get free_Total_WithoutTransfer(): AbstractControl {
        return this.form.get('portTotals.free_Total_WithoutTransfer')
    }

    get total_Persons(): AbstractControl {
        return this.form.get('portTotals.total_Persons')
    }

    get total_Amount(): AbstractControl {
        return this.form.get('portTotals.total_Amount')
    }

    //#endregion

}
