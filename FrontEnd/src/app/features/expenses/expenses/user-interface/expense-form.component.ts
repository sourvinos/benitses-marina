import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl, FormArray } from '@angular/forms'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { map, Observable, startWith } from 'rxjs'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { ExpenseHttpService } from '../classes/services/expense-http.service'
import { ExpenseReadDto } from '../classes/dtos/expense-read-dto'
import { ExpenseWriteDto } from '../classes/dtos/expense-write-dto'
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
    selector: 'expense-form',
    templateUrl: './expense-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './expense-form.component.css']
})

export class ExpenseFormComponent {

    //#region common

    private expense: ExpenseReadDto
    private expenseId: string
    public feature = 'expenseForm'
    public featureIcon = 'expenses'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/expenses'
    public imgIsLoaded = false

    //#endregion

    //#region autocompletes

    public dropdownBoatTypes: Observable<SimpleEntity[]>
    public dropdownBoatUsages: Observable<SimpleEntity[]>
    public dropdownPaymentStatuses: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    //#region berths

    public berthsArray: string[] = []

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private expenseHttpService: ExpenseHttpService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.setLocale()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
        this.leftAlignLastTab()
    }

    //#endregion

    //#region public methods

    public autocompleteFields(fieldName: any, object: any): any {
        return object ? object[fieldName] : undefined
    }

    public calculateVatAmountAndGrossAmountBasedOnNetAmount(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        const netAmount = parseFloat(this.form.value.netAmount)
        const vatPercent = this.form.value.vatPercent
        const vatAmount = netAmount * (vatPercent / 100)
        const grossAmount = netAmount + vatAmount
        this.form.patchValue(
            {
                netAmount: netAmount.toFixed(2),
                vatAmount: vatAmount.toFixed(2),
                grossAmount: grossAmount.toFixed(2)
            })
    }

    public calculateNetAndGrossAmountBasedOnVatPercent(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        this.calculateVatAmountAndGrossAmountBasedOnNetAmount(fieldName, digits)
    }

    public calculateNetAmountBasedOnGrossAmount(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        const netAmount = this.form.value.grossAmount / (1 + (this.form.value.vatPercent / 100))
        const vatPercent = this.form.value.vatPercent
        const vatAmount = netAmount * (vatPercent / 100)
        const grossAmount = parseFloat(this.form.value.grossAmount)
        this.form.patchValue(
            {
                netAmount: netAmount.toFixed(2),
                vatAmount: vatAmount.toFixed(2),
                grossAmount: grossAmount.toFixed(2)
            })
    }

    public copyOwnerToBilling(): void {
        this.form.patchValue({
            billingName: this.form.value.ownerName,
            billingAddress: this.form.value.ownerAddress,
            billingTaxNo: this.form.value.ownerTaxNo,
            billingTaxOffice: this.form.value.ownerTaxOffice,
            billingPassportNo: this.form.value.ownerPassportNo,
            billingPhones: this.form.value.ownerPhones,
            billingEmail: this.form.value.ownerEmail
        })
    }

    public checkForEmptyAutoComplete(event: { target: { value: any } }): void {
        if (event.target.value == '') this.isAutoCompleteDisabled = true
    }

    public enableOrDisableAutoComplete(event: any): void {
        this.isAutoCompleteDisabled = this.helperService.enableOrDisableAutoComplete(event)
    }

    public getEmoji(emoji: string): string {
        return this.emojiService.getEmoji(emoji)
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
        return this.form.value.expenseId == '' ? 'headerNew' : 'headerEdit'
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public getFinancialRemarksLength(): any {
        return this.form.value.financialRemarks != null ? this.form.value.financialRemarks.length : 0
    }

    public imageIsLoading(): any {
        return this.imgIsLoaded ? '' : 'skeleton'
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public isNotNewRecord(): string {
        return this.form.value.expenseId
    }

    public loadImage(): void {
        this.imgIsLoaded = true
    }

    public onAddBerthTextBox(): void {
        const control = <FormArray>this.form.get('berths')
        const newGroup = this.formBuilder.group({
            description: ''
        })
        control.push(newGroup)
        this.berthsArray.push(this.form.controls.berths.value)
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.expenseHttpService.delete(this.form.value.expenseId).subscribe({
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

    public onRemoveBerth(berthIndex: number): void {
        const berths = <FormArray>this.form.get('berths')
        berths.removeAt(berthIndex)
        this.berthsArray.splice(berthIndex, 1)
    }

    public onSave(): void {
        this.saveRecord(this.flattenForm())
    }

    public openOrCloseAutoComplete(trigger: MatAutocompleteTrigger, element: any): void {
        this.helperService.openOrCloseAutocomplete(this.form, element, trigger)
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

    private flattenForm(): ExpenseWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            supplierId: this.form.value.supplier.id,
            documentTypeId: this.form.value.documentType.id,
            paymentMethodId: this.form.value.paymentMethod.id,
            date: this.dateHelperService.formatDateToIso(new Date(this.form.value.date)),
            invoiceNo: this.form.value.invoiceNo,
            amount: this.form.value.amount,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.expenseId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['expenseForm']
                if (formResolved.error == null) {
                    this.expense = formResolved.record.body
                    resolve(this.expense)
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
            supplier: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            documentType: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            paymentMethod: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            date: ['', [Validators.required]],
            invoiceNo: ['', [Validators.required]],
            amount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private leftAlignLastTab(): void {
        this.helperService.leftAlignLastTab()
    }

    private patchNumericFieldsWithZeroIfNullOrEmpty(fieldName: string, digits: number): void {
        if (this.form.controls[fieldName].value == null || this.form.controls[fieldName].value == '') {
            this.form.patchValue({ [fieldName]: parseInt('0').toFixed(digits) })
        }
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.expenseId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('boatTypes', 'dropdownBoatTypes', 'boatType', 'description', 'description')
        this.populateDropdownFromDexieDB('boatUsages', 'dropdownBoatUsages', 'boatUsage', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentStatuses', 'dropdownPaymentStatuses', 'paymentStatus', 'description', 'description')
    }

    private populateFields(): void {
        if (this.expense != undefined) {
            this.form.setValue({
                id: this.expense.id,
                date: this.expense.date,
                invoiceNo: this.expense.invoiceNo,
                supplier: { 'id': this.expense.supplier.id, 'description': this.expense.supplier.description },
                documentType: { 'id': this.expense.documentType.id, 'description': this.expense.documentType.description },
                paymentMethod: { 'id': this.expense.paymentMethod.id, 'description': this.expense.paymentMethod.description },
                amount: this.expense.amount,
                postAt: this.expense.postAt,
                postUser: this.expense.postUser,
                putAt: this.expense.putAt,
                putUser: this.expense.putUser
            })
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(expense: ExpenseWriteDto): void {
        this.expenseHttpService.saveExpense(expense).subscribe({
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
            this.expenseId = x.id
        })
    }

    //#endregion

    //#region getters

    get boatName(): AbstractControl {
        return this.form.get('boatName')
    }

    get loa(): AbstractControl {
        return this.form.get('loa')
    }

    get beam(): AbstractControl {
        return this.form.get('beam')
    }

    get draft(): AbstractControl {
        return this.form.get('draft')
    }

    get fromDate(): AbstractControl {
        return this.form.get('fromDate')
    }

    get toDate(): AbstractControl {
        return this.form.get('toDate')
    }

    get paymentStatus(): AbstractControl {
        return this.form.get('paymentStatus')
    }

    get remarks(): AbstractControl {
        return this.form.get('remarks')
    }

    get financialRemarks(): AbstractControl {
        return this.form.get('financialRemarks')
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

    get insuranceCompany(): AbstractControl {
        return this.form.get('insuranceCompany')
    }

    get policyNo(): AbstractControl {
        return this.form.get('policyNo')
    }

    get policyEnds(): AbstractControl {
        return this.form.get('policyEnds')
    }

    get flag(): AbstractControl {
        return this.form.get('flag')
    }

    get registryPort(): AbstractControl {
        return this.form.get('registryPort')
    }

    get registryNo(): AbstractControl {
        return this.form.get('registryNo')
    }

    get boatType(): AbstractControl {
        return this.form.get('boatType')
    }

    get boatUsage(): AbstractControl {
        return this.form.get('boatUsage')
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

    get ownerName(): AbstractControl {
        return this.form.get('ownerName')
    }

    get ownwerAddress(): AbstractControl {
        return this.form.get('ownerAddress')
    }

    get ownerTaxNo(): AbstractControl {
        return this.form.get('ownerTaxNo')
    }

    get ownerTaxOffice(): AbstractControl {
        return this.form.get('ownerTaxOffice')
    }

    get ownerPassportNo(): AbstractControl {
        return this.form.get('ownerPassportNo')
    }

    get ownerPhones(): AbstractControl {
        return this.form.get('ownerPhones')
    }

    get ownerEmail(): AbstractControl {
        return this.form.get('ownerEmail')
    }

    get billingName(): AbstractControl {
        return this.form.get('billingName')
    }

    get billingAddress(): AbstractControl {
        return this.form.get('billingAddress')
    }

    get billingTaxNo(): AbstractControl {
        return this.form.get('billingTaxNo')
    }

    get billingTaxOffice(): AbstractControl {
        return this.form.get('billingTaxOffice')
    }

    get billingPassportNo(): AbstractControl {
        return this.form.get('billingPassportNo')
    }

    get billingPhones(): AbstractControl {
        return this.form.get('billingPhones')
    }

    get billingEmail(): AbstractControl {
        return this.form.get('billingEmail')
    }

    //#endregion

}
