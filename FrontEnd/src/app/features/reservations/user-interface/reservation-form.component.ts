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
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationHttpService } from '../classes/services/reservation-http.service'
import { ReservationLeaseWriteDto } from '../classes/dtos/reservationLease-write-dto'
import { ReservationOwnerWriteDto } from '../classes/dtos/reservationOwner-write-dto'
import { ReservationReadDto } from '../classes/dtos/reservation-read-dto'
import { ReservationWriteDto } from '../classes/dtos/reservation-write-dto'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'reservation-form',
    templateUrl: './reservation-form.component.html',
    styleUrls: ['../../../../assets/styles/custom/forms.css', './reservation-form.component.css']
})

export class ReservationFormComponent {

    //#region common

    private reservation: ReservationReadDto
    private reservationId: string
    public feature = 'reservationForm'
    public featureIcon = 'reservations'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/reservations'

    //#endregion

    //#region autocompletes

    public dropdownPaymentStatuses: Observable<SimpleEntity[]>
    public isAutoCompleteDisabled = true

    //#endregion

    //#region berths

    public berthsArray: string[] = []

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private reservationHttpService: ReservationHttpService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.populateBerths()
        this.setLocale()
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

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getNewOrEditHeader(): string {
        return this.form.value.reservationId == '' ? 'headerNew' : 'headerEdit'
    }

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
    }

    public getFinancialRemarksLength(): any {
        return this.form.value.financialRemarks != null ? this.form.value.financialRemarks.length : 0
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
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
                this.reservationHttpService.delete(this.form.value.reservationId).subscribe({
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

    private flattenForm(): ReservationWriteDto {
        return {
            reservationId: this.form.value.reservationId != '' ? this.form.value.reservationId : null,
            boatName: this.form.value.boatName,
            loa: this.form.value.loa,
            beam: this.form.value.beam,
            draft: this.form.value.draft,
            fromDate: this.form.value.fromDate,
            toDate: this.form.value.toDate,
            berths: this.form.value.berths,
            email: this.form.value.email,
            remarks: this.form.value.remarks,
            financialRemarks: this.form.value.financialRemarks,
            paymentStatusId: this.form.value.paymentStatus.id,
            reservationOwner: this.mapReservationOwner(this.form),
            reservationLease: this.mapReservationLease(this.form),
            isDocked: this.form.value.isDocked,
            isLongTerm: this.form.value.isLongTerm,
            isAthenian: this.form.value.isAthenian,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.reservationId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['reservationForm']
                if (formResolved.error == null) {
                    this.reservation = formResolved.record.body
                    resolve(this.reservation)
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
            reservationId: '',
            boatName: ['', [Validators.required]],
            loa: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            beam: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            draft: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
            berths: this.formBuilder.array([]),
            remarks: ['', Validators.maxLength(2048)],
            financialRemarks: ['', Validators.maxLength(2048)],
            paymentStatus: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            insuranceCompany: '',
            policyNo: '',
            policyEnds: '',
            flag: '',
            registryPort: '',
            registryNo: '',
            boatType: '',
            boatUsage: '',
            netAmount: 0,
            vatAmount: 0,
            grossAmount: 0,
            isDocked: false,
            isLongTerm: false,
            isAthenian: false,
            owner: '',
            address: '',
            taxNo: '',
            taxOffice: '',
            passportNo: '',
            phones: '',
            email: '',
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private mapReservationOwner(form: any): ReservationOwnerWriteDto {
        const x: ReservationOwnerWriteDto = {
            reservationId: form.value.reservationId,
            owner: form.value.owner,
            address: form.value.address,
            taxNo: form.value.taxNo,
            taxOffice: form.value.taxOffice,
            passportNo: form.value.passportNo,
            phones: form.value.phones,
            email: form.value.email
        }
        return x
    }

    private mapReservationLease(form: any): ReservationLeaseWriteDto {
        const x: ReservationLeaseWriteDto = {
            reservationId: form.value.reservationId,
            insuranceCompany: form.value.insuranceCompany,
            policyNo: form.value.policyNo,
            policyEnds: form.value.policyEnds,
            flag: form.value.flag,
            registryPort: form.value.registryPort,
            registryNo: form.value.registryNo,
            boatType: form.value.boatType,
            boatUsage: form.value.boatUsage,
            netAmount: form.value.netAmount,
            vatAmount: form.value.vatAmount,
            grossAmount: form.value.grossAmount
        }
        return x
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.reservationId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('paymentStatuses', 'dropdownPaymentStatuses', 'paymentStatus', 'description', 'description')
    }

    private populateFields(): void {
        if (this.reservation != undefined) {
            this.form.setValue({
                reservationId: this.reservation.reservationId,
                boatName: this.reservation.boatName,
                loa: this.reservation.loa,
                beam: this.reservation.beam,
                draft: this.reservation.draft,
                fromDate: this.reservation.fromDate,
                toDate: this.reservation.toDate,
                berths: [],
                remarks: this.reservation.remarks,
                financialRemarks: this.reservation.financialRemarks,
                isDocked: this.reservation.isDocked,
                isLongTerm: this.reservation.isLongTerm,
                isAthenian: this.reservation.isAthenian,
                paymentStatus: { 'id': this.reservation.paymentStatus.id, 'description': this.reservation.paymentStatus.description },
                insuranceCompany: this.reservation.reservationLease.insuranceCompany,
                policyNo: this.reservation.reservationLease.policyNo,
                policyEnds: this.reservation.reservationLease.policyEnds,
                flag: this.reservation.reservationLease.flag,
                registryPort: this.reservation.reservationLease.registryPort,
                registryNo: this.reservation.reservationLease.registryNo,
                boatType: this.reservation.reservationLease.boatType,
                boatUsage: this.reservation.reservationLease.boatUsage,
                netAmount: this.reservation.reservationLease.netAmount,
                vatAmount: this.reservation.reservationLease.vatAmount,
                grossAmount: this.reservation.reservationLease.grossAmount,
                owner: this.reservation.reservationOwner.owner,
                address: this.reservation.reservationOwner.address,
                taxNo: this.reservation.reservationOwner.taxNo,
                taxOffice: this.reservation.reservationOwner.taxOffice,
                passportNo: this.reservation.reservationOwner.passportNo,
                phones: this.reservation.reservationOwner.phones,
                email: this.reservation.reservationOwner.email,
                postAt: this.reservation.postAt,
                postUser: this.reservation.postUser,
                putAt: this.reservation.putAt,
                putUser: this.reservation.putUser
            })
        }
    }

    private populateBerths(): void {
        if (this.reservation) {
            if (this.reservation.berths.length >= 1) {
                this.reservation.berths.forEach(berth => {
                    const control = <FormArray>this.form.get('berths')
                    const newGroup = this.formBuilder.group({
                        reservationId: berth.reservationId,
                        description: berth.description
                    })
                    control.push(newGroup)
                    this.berthsArray.push(this.form.controls.berths.value)
                })
            } else {
                this.onAddBerthTextBox()
            }
        } else {
            this.onAddBerthTextBox()
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(reservation: ReservationWriteDto): void {
        this.reservationHttpService.saveReservation(reservation).subscribe({
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
            this.reservationId = x.id
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

    get vatAmount(): AbstractControl {
        return this.form.get('vatAmount')
    }

    get grossAmount(): AbstractControl {
        return this.form.get('grossAmount')
    }

    get owner(): AbstractControl {
        return this.form.get('owner')
    }

    get address(): AbstractControl {
        return this.form.get('address')
    }

    get taxNo(): AbstractControl {
        return this.form.get('taxNo')
    }

    get taxOffice(): AbstractControl {
        return this.form.get('taxOffice')
    }

    get passportNo(): AbstractControl {
        return this.form.get('passportNo')
    }

    get phones(): AbstractControl {
        return this.form.get('phones')
    }

    get email(): AbstractControl {
        return this.form.get('email')
    }

    //#endregion

}
