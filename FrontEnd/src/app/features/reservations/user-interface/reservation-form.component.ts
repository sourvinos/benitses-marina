import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MatAutocompleteTrigger } from '@angular/material/autocomplete'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { Observable, map, startWith } from 'rxjs'
import { ReservationHttpService } from '../classes/services/reservation-http.service'
import { ReservationReadDto } from '../classes/dtos/reservation-read-dto'
import { ReservationWriteDto } from '../classes/dtos/reservation-write-dto'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { PierWriteDto } from '../classes/dtos/pier-write-dto'

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

    public isAutoCompleteDisabled = true
    public dropdownBoatTypes: Observable<SimpleEntity[]>

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private reservationHttpService: ReservationHttpService, private dexieService: DexieService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
    }

    ngAfterViewInit(): void {
        // this.focusOnField()
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

    public getRemarksLength(): any {
        return this.form.value.remarks != null ? this.form.value.remarks.length : 0
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
            boatTypeId: this.form.value.boatType.id,
            boatName: this.form.value.boatName,
            length: this.form.value.length,
            fromDate: this.form.value.fromDate,
            toDate: this.form.value.toDate,
            days: this.form.value.days,
            email: this.form.value.email,
            remarks: this.form.value.remarks,
            isConfirmed: this.form.value.isConfirmed,
            isDocked: this.form.value.isDocked,
            isPaid: this.form.value.isPaid,
            piers: this.mapPiers(this.form),
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
            boatType: ['', [Validators.required, ValidationService.RequireAutocomplete]],
            length: [0, [Validators.required, Validators.min(0), Validators.max(30)]],
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
            days: [0, [Validators.required]],
            email: ['', [Validators.maxLength(128), Validators.email]],
            remarks: ['', Validators.maxLength(128)],
            piers: [[]],
            isConfirmed: false,
            isDocked: false,
            isPaid: false,
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private mapPiers(form: any): PierWriteDto[] {
        const piers = []
        form.value.piers.forEach((pier: any) => {
            const x: PierWriteDto = {
                reservationId: form.value.reservationId,
                pierId: pier.pierId,
            }
            piers.push(x)
        })
        return piers
    }

    private patchNumericFieldsWithZeroIfNullOrEmpty(fieldName: string, digits: number): void {
        if (this.form.controls[fieldName].value == null || this.form.controls[fieldName].value == '') {
            this.form.patchValue({ [fieldName]: parseInt('0').toFixed(digits) })
        }
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('boatTypes', 'dropdownBoatTypes', 'boatType', 'description', 'description')
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.reservationId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateFields(): void {
        if (this.reservation != undefined) {
            this.form.setValue({
                reservationId: this.reservation.reservationId,
                boatType: { 'id': this.reservation.boatType.id, 'description': this.reservation.boatType.description },
                boatName: this.reservation.boatName,
                length: this.reservation.length,
                fromDate: this.reservation.fromDate,
                toDate: this.reservation.toDate,
                days: this.reservation.days,
                email: this.reservation.email,
                remarks: this.reservation.remarks,
                isConfirmed: this.reservation.isConfirmed,
                isDocked: this.reservation.isDocked,
                isPaid: this.reservation.isPaid,
                piers: this.reservation.piers,
                postAt: this.reservation.postAt,
                postUser: this.reservation.postUser,
                putAt: this.reservation.putAt,
                putUser: this.reservation.putUser
            })
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

    get boatType(): AbstractControl {
        return this.form.get('boatType')
    }

    get length(): AbstractControl {
        return this.form.get('length')
    }

    get fromDate(): AbstractControl {
        return this.form.get('fromDate')
    }

    get toDate(): AbstractControl {
        return this.form.get('toDate')
    }

    get days(): AbstractControl {
        return this.form.get('days')
    }

    get email(): AbstractControl {
        return this.form.get('email')
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
