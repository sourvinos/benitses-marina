import { ActivatedRoute, Router } from '@angular/router'
import { Component, HostListener } from '@angular/core'
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
import { EmailQueueDto } from 'src/app/shared/classes/email-queue-dto';
import { EmailQueueHttpService } from 'src/app/shared/services/email-queue-http.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { LeasePdfHttpService } from '../../leases/classes/services/lease-pdf-http.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { ReservationBoatWriteDto } from '../classes/dtos/reservation-boat-write-dto'
import { ReservationFeeDto } from '../classes/dtos/reservation-fee-dto'
import { ReservationHelperService } from '../classes/services/reservation-helper.service';
import { ReservationHttpService } from '../classes/services/reservation-http.service'
import { ReservationInsuranceDto } from '../classes/dtos/reservation-insurance-dto'
import { ReservationPersonDto } from '../classes/dtos/reservation-person-dto'
import { ReservationReadDto } from '../classes/dtos/reservation-read-dto'
import { ReservationStorage } from '../classes/storage/reservation-storage'
import { ReservationWriteDto } from '../classes/dtos/reservation-write-dto'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { SimpleEntity } from 'src/app/shared/classes/simple-entity'
import { ValidationService } from 'src/app/shared/services/validation.service'
import { environment } from 'src/environments/environment'
import { ReservationFishingLicenceDto } from '../classes/dtos/reservation-fishing-licence-dto'
import FileSaver from 'file-saver'

@Component({
    selector: 'reservation-form',
    templateUrl: './reservation-form.component.html',
    styleUrls: ['../../../../../assets/styles/custom/forms.css', './reservation-form.component.css']
})

export class ReservationFormComponent {

    //#region common

    private reservation: ReservationReadDto
    private storedReservation: ReservationStorage
    private reservationId: string
    public feature = 'reservationForm'
    public featureIcon = 'reservations'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/reservations'
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

    //#region documents

    private documents = []
    public selectedDocuments = []
    private renameDocumentForm: FormGroup

    // #endregion

    //#region hostlisteners

    @HostListener('window:keydown.escape', ['$event']) onEscKeyDown(event: KeyboardEvent): void {
        this.router.navigate([this.parentUrl])
    }

    @HostListener('window:keydown.control.s', ['$event']) onShiftKeyDown(event: KeyboardEvent): void {
        this.attemptToSaveRecord(event, true)
    }

    @HostListener('window:keydown.control.shift.s', ['$event']) onCtrlShiftKeyDown(event: KeyboardEvent): void {
        this.attemptToSaveRecord(event, false)
    }

    //#endregion

    constructor(private emailQueueHttpService: EmailQueueHttpService, private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateAdapter: DateAdapter<any>, private dateHelperService: DateHelperService, private dexieService: DexieService, private dialogService: DialogService, private emojiService: EmojiService, private formBuilder: FormBuilder, private helperService: HelperService, private leasePdfHttpService: LeasePdfHttpService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private reservationHttpService: ReservationHttpService, private reservationHelperService: ReservationHelperService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.initRenameFileForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.populateDropdowns()
        this.populateBerths()
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

    public onCreateLeaseAndOpen(): void {
        const ids = []
        ids.push(this.form.value.reservationId)
        this.leasePdfHttpService.buildLeasePdf(ids).subscribe({
            next: (response) => {
                this.leasePdfHttpService.openLeasePdf(response.body[0]).subscribe({
                    next: (response) => {
                        const blob = new Blob([response], { type: 'application/pdf' })
                        const fileURL = URL.createObjectURL(blob)
                        FileSaver.saveAs(blob, this.helperService.convertStringToLowerCase(this.form.value.boatName) + " contract.pdf");
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

    public onCalculateFees(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        const netAmount = parseFloat(this.form.value.netAmount)
        const discountPercent = parseFloat(this.form.value.discountPercent)
        const discountAmount = netAmount * (discountPercent / 100)
        const netAmountAfterDiscount = netAmount - discountAmount
        const vatAmount = netAmountAfterDiscount * (this.form.value.vatPercent / 100)
        const grossAmount = netAmountAfterDiscount + vatAmount
        this.form.patchValue({
            netAmount: netAmount.toFixed(2),
            discountPercent: discountPercent.toFixed(2),
            discountAmount: discountAmount.toFixed(2),
            netAmountAfterDiscount: netAmountAfterDiscount.toFixed(2),
            vatAmount: vatAmount,
            grossAmount: grossAmount
        })

        // const discountPercent = this.form.value.discountPercent
        // const discountAmount = this.form.value.discountAmount
        // const netAmountAfterDiscount = parseFloat(this.form.value.netAmountAfterDiscount)
        // const vatAmount = netAmountAfterDiscount * (vatPercent / 100)
        // const grossAmount = netAmountAfterDiscount + vatAmount
        // this.form.patchValue({
        //     netAmount: netAmount.toFixed(2),
        //     discountPercent: discountPercent.toFixed(2),
        //     discountAmount: discountAmount.toFixed(2),
        //     netAmountAfterDiscount: netAmountAfterDiscount.toFixed(2),
        //     vatAmount: vatAmount.toFixed(2),
        //     grossAmount: grossAmount.toFixed(2)
        // })
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
        return this.form.value.reservationId == '' ? 'headerNew' : 'headerEdit'
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


    public isFishingBoat(): boolean {
        return this.form.value.isFishingBoat
    }

    public isNotNewRecord(): boolean {
        return this.form.value.reservationId == ''
    }


    public isNewRecord(): boolean {
        return this.form.value.reservationId != '' && this.form.pristine == true
    }


    public isNotNewRecordAndIsReservationNotInStorage(): boolean {
        return this.form.value.reservationId == '' && this.localStorageService.getItem('reservation').length != 0
    }


    public canSendDocuments(): boolean {
        return (this.form.value.reservationId != '' && this.form.pristine == true && this.selectedDocuments.length != 0)
    }


    public canSendInvalidInsurance(): boolean {
        return (this.form.value.reservationId != '' && this.form.pristine == true && this.form.value.isDocked && this.expireDateMustBeInThePast(this.form.value.policyEnds))
    }


    public canSendEndOfLease(): boolean {
        return (this.form.value.reservationId != '' && this.form.pristine == true && this.form.value.isDocked)
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

    public onCopyReservation(): void {
        this.localStorageService.saveItem('reservation', JSON.stringify(this.reservationHelperService.createCachedReservation(this.form.value)))
    }

    public onPasteReservation(): void {
        this.getCachedReservation()
        this.populateReservationFromStorage()
    }

    private getCachedReservation(): void {
        this.storedReservation = JSON.parse(this.localStorageService.getItem('reservation'))
    }

    private populateReservationFromStorage(): void {
        this.form.patchValue({
            reservationId: '',
            boatName: this.storedReservation.boatName,
            flag: this.storedReservation.flag,
            loa: this.storedReservation.loa,
            beam: this.storedReservation.beam,
            draft: this.storedReservation.draft,
            registryPort: this.storedReservation.registryPort,
            registryNo: this.storedReservation.registryNo,
            boatType: { 'id': this.storedReservation.boatType.id, 'description': this.storedReservation.boatType.description },
            boatUsage: { 'id': this.storedReservation.boatUsage.id, 'description': this.storedReservation.boatUsage.description },
            fromDate: this.storedReservation.fromDate,
            toDate: this.storedReservation.toDate,
            berths: this.storedReservation.berths,
            remarks: this.storedReservation.remarks,
            financialRemarks: this.storedReservation.financialRemarks,
            paymentStatus: { 'id': this.storedReservation.paymentStatus.id, 'description': this.storedReservation.paymentStatus.description },
            issuingAuthority: this.storedReservation.issuingAuthority,
            licenceNo: this.storedReservation.licenceNo,
            licenceEnds: this.storedReservation.licenceEnds,
            insuranceCompany: this.storedReservation.insuranceCompany,
            policyNo: this.storedReservation.policyNo,
            policyEnds: this.storedReservation.policyEnds,
            netAmount: this.storedReservation.netAmount,
            discountPercent: this.storedReservation.discountPercent,
            discountAmount: this.storedReservation.discountAmount,
            netAmountAfterDiscount: this.storedReservation.netAmountAfterDiscount,
            vatPercent: this.storedReservation.vatPercent,
            vatAmount: this.storedReservation.vatAmount,
            grossAmount: this.storedReservation.grossAmount,
            isAthenian: this.storedReservation.isAthenian,
            isDocked: this.storedReservation.isDocked,
            isDryDock: this.storedReservation.isDryDock,
            isFishingBoat: this.storedReservation.isFishingBoat,
            isRequest: this.storedReservation.isRequest,
            isCash: this.storedReservation.isCash,
            ownerName: this.storedReservation.ownerName,
            ownerAddress: this.storedReservation.ownerAddress,
            ownerTaxNo: this.storedReservation.ownerTaxNo,
            ownerTaxOffice: this.storedReservation.ownerTaxOffice,
            ownerPassportNo: this.storedReservation.ownerPassportNo,
            ownerPhones: this.storedReservation.ownerPhones,
            ownerEmail: this.storedReservation.ownerEmail,
            billingName: this.storedReservation.billingName,
            billingAddress: this.storedReservation.billingAddress,
            billingTaxNo: this.storedReservation.billingTaxNo,
            billingTaxOffice: this.storedReservation.billingTaxOffice,
            billingPassportNo: this.storedReservation.billingPassportNo,
            billingPhones: this.storedReservation.billingPhones,
            billingEmail: this.storedReservation.billingEmail,
            postAt: this.storedReservation.postAt,
            postUser: this.storedReservation.postUser,
            putAt: this.storedReservation.putAt,
            putUser: this.storedReservation.putUser
        })
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

    public onDeleteDocument = (filename: string, event: PointerEvent): any => {
        event.stopPropagation()
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                return new Promise<void>((resolve) => {
                    this.reservationHttpService.deleteDocument(filename).subscribe((x) => {
                        resolve(x)
                        this.getDocuments()
                    })
                })
            }
        })
    }

    public onOpenDocument(filename: string, event: PointerEvent): void {
        event.stopPropagation()
        this.reservationHttpService.openDocument(filename).subscribe({
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

    public onRemoveBerth(berthIndex: number): void {
        const berths = <FormArray>this.form.get('berths')
        berths.removeAt(berthIndex)
        this.berthsArray.splice(berthIndex, 1)
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

    public toggleSelectedDocuments(filename: string, event: any) {
        if (event.checked) {
            this.selectedDocuments.push(filename.substring(37))
        } else {
            let found = this.selectedDocuments.findIndex(z => {
                return z == filename.substring(37);
            });
            this.selectedDocuments.splice(found, 1)
        }
    }

    public trimFilename(filename: string): string {
        return filename.substring(36, filename.length - 4)
    }

    public showDocuments(): string[] {
        return this.documents ? this.documents : []
    }

    public onAddToEmailQueue(discriminator: string): void {
        if (discriminator == 'InvalidInsurance' || discriminator == 'EndOfLease') {
            const x: EmailQueueDto = {
                initiator: discriminator,
                entityId: this.form.value.reservationId,
                filenames: '',
                priority: 1,
                isSent: false
            }
            this.emailQueueHttpService.save(x).subscribe(() => {
                this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => { })
            })
        }
        if (discriminator == 'Reservation') {
            const x: EmailQueueDto = {
                initiator: discriminator,
                entityId: this.form.value.reservationId,
                filenames: this.selectedDocuments.join(),
                priority: 1,
                isSent: false
            }
            this.emailQueueHttpService.save(x).subscribe(() => {
                this.dialogService.open(this.messageDialogService.success(), 'ok', ['ok']).subscribe(() => { })
            })
        }
    }

    //#endregion

    //#region private methods

    private attemptToSaveRecord(event: KeyboardEvent, closeForm: boolean): void {
        this.form.valid ? ((): void => { this.onSave(closeForm); event.preventDefault() })() : ((): void => { event.preventDefault() })()
    }

    private filterAutocomplete(array: string, field: string, value: any): any[] {
        if (typeof value !== 'object') {
            const filtervalue = value.toLowerCase()
            return this[array].filter((element: { [x: string]: string; }) =>
                element[field].toLowerCase().startsWith(filtervalue))
        }
    }

    private flattenForm(): ReservationWriteDto {
        const x = {
            reservationId: this.form.value.reservationId != '' ? this.form.value.reservationId : null,
            fromDate: this.dateHelperService.formatDateToIso(new Date(this.form.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(this.form.value.toDate)),
            berths: this.form.value.berths,
            remarks: this.form.value.remarks,
            financialRemarks: this.form.value.financialRemarks,
            paymentStatusId: this.form.value.paymentStatus.id,
            boat: this.mapBoat(this.form),
            fishingLicence: this.mapFishingLicence(this.form),
            insurance: this.mapInsurance(this.form),
            owner: this.mapOwner(this.form),
            billing: this.mapBilling(this.form),
            fee: this.mapFee(this.form),
            isAthenian: this.form.value.isAthenian,
            isDocked: this.form.value.isDocked,
            isDryDock: this.form.value.isDryDock,
            isRequest: this.form.value.isRequest,
            putAt: this.form.value.putAt
        }
        return x
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getDocuments(): void {
        if (this.form.value.reservationId != '') {
            this.reservationHttpService.getDocuments(this.form.value.reservationId).subscribe((x) => {
                this.documents = Array.from(x.body)
            })
        }
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
            flag: '',
            loa: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            beam: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            draft: ['', [Validators.required, Validators.min(0), Validators.max(30)]],
            registryPort: ['', [Validators.required]],
            registryNo: ['', [Validators.required]],
            boatType: ['', [Validators.required, ValidationService.requireAutocomplete]],
            boatUsage: ['', [Validators.required, ValidationService.requireAutocomplete]],
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
            berths: this.formBuilder.array([]),
            remarks: ['', Validators.maxLength(2048)],
            financialRemarks: ['', Validators.maxLength(2048)],
            paymentStatus: ['', [Validators.required, ValidationService.requireAutocomplete]],
            issuingAuthority: '',
            licenceNo: '',
            licenceEnds: ['2199-12-31', [Validators.required]],
            insuranceCompany: '',
            policyNo: '',
            policyEnds: ['', [Validators.required]],
            netAmount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            discountPercent: [0, [Validators.required, Validators.min(0), Validators.max(999)]],
            discountAmount: [0, [Validators.required, Validators.min(0), Validators.max(99999)]],
            netAmountAfterDiscount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            vatPercent: ['', [Validators.required, Validators.min(0), Validators.max(99)]],
            vatAmount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            grossAmount: ['', [Validators.required, Validators.min(0), Validators.max(99999)]],
            isAthenian: false,
            isDocked: false,
            isDryDock: false,
            isFishingBoat: false,
            isRequest: false,
            isCash: false,
            ownerName: '',
            ownerAddress: '',
            ownerTaxNo: '',
            ownerTaxOffice: '',
            ownerPassportNo: '',
            ownerPhones: '',
            ownerEmail: '',
            billingName: '',
            billingAddress: '',
            billingTaxNo: '',
            billingTaxOffice: '',
            billingPassportNo: '',
            billingPhones: '',
            billingEmail: '',
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

    private mapBoat(form: any): ReservationBoatWriteDto {
        const x: ReservationBoatWriteDto = {
            reservationId: form.value.reservationId,
            name: form.value.boatName,
            flag: form.value.flag,
            loa: form.value.loa,
            beam: form.value.beam,
            draft: form.value.draft,
            registryPort: form.value.registryPort,
            registryNo: form.value.registryNo,
            isFishingBoat: form.value.isFishingBoat,
            typeId: form.value.boatType.id,
            usageId: form.value.boatUsage.id,
        }
        return x
    }

    private mapFishingLicence(form: any): ReservationFishingLicenceDto {
        const x: ReservationFishingLicenceDto = {
            reservationId: form.value.reservationId,
            issuingAuthority: form.value.issuingAuthority,
            licenceNo: form.value.licenceNo,
            licenceEnds: this.dateHelperService.formatDateToIso(new Date(this.form.value.licenceEnds))
        }
        return x
    }

    private mapInsurance(form: any): ReservationInsuranceDto {
        const x: ReservationInsuranceDto = {
            reservationId: form.value.reservationId,
            insuranceCompany: form.value.insuranceCompany,
            policyNo: form.value.policyNo,
            policyEnds: this.dateHelperService.formatDateToIso(new Date(this.form.value.policyEnds))
        }
        return x
    }

    private mapOwner(form: any): ReservationPersonDto {
        const x: ReservationPersonDto = {
            reservationId: form.value.reservationId,
            name: form.value.ownerName,
            address: form.value.ownerAddress,
            taxNo: form.value.ownerTaxNo,
            taxOffice: form.value.ownerTaxOffice,
            passportNo: form.value.ownerPassportNo,
            phones: form.value.ownerPhones,
            email: form.value.ownerEmail
        }
        return x
    }

    private mapBilling(form: any): ReservationPersonDto {
        const x: ReservationPersonDto = {
            reservationId: form.value.reservationId,
            name: form.value.billingName,
            address: form.value.billingAddress,
            taxNo: form.value.billingTaxNo,
            taxOffice: form.value.billingTaxOffice,
            passportNo: form.value.billingPassportNo,
            phones: form.value.billingPhones,
            email: form.value.billingEmail
        }
        return x
    }

    private mapFee(form: any): ReservationFeeDto {
        const x: ReservationFeeDto = {
            reservationId: form.value.reservationId,
            netAmount: form.value.netAmount,
            discountPercent: form.value.discountPercent,
            discountAmount: form.value.discountAmount,
            netAmountAfterDiscount: form.value.netAmountAfterDiscount,
            vatPercent: form.value.vatPercent,
            vatAmount: form.value.vatAmount,
            grossAmount: form.value.grossAmount,
            isCash: form.value.isCash
        }
        return x
    }

    private patchNumericFieldsWithZeroIfNullOrEmpty(fieldName: string, digits: number): void {
        if (this.form.controls[fieldName].value == null || this.form.controls[fieldName].value == '') {
            this.form.patchValue({ [fieldName]: parseInt('0').toFixed(digits) })
        }
    }

    private populateDropdownFromDexieDB(dexieTable: string, filteredTable: string, formField: string, modelProperty: string, orderBy: string): void {
        this.dexieService.table(dexieTable).orderBy(orderBy).toArray().then((response) => {
            this[dexieTable] = this.reservationId == undefined ? response.filter(x => x.isActive) : response
            this[filteredTable] = this.form.get(formField).valueChanges.pipe(startWith(''), map(value => this.filterAutocomplete(dexieTable, modelProperty, value)))
        })
    }

    private populateDropdowns(): void {
        this.populateDropdownFromDexieDB('boatTypes', 'dropdownBoatTypes', 'boatType', 'description', 'description')
        this.populateDropdownFromDexieDB('boatUsages', 'dropdownBoatUsages', 'boatUsage', 'description', 'description')
        this.populateDropdownFromDexieDB('paymentStatuses', 'dropdownPaymentStatuses', 'paymentStatus', 'description', 'description')
    }

    private populateFields(): void {
        if (this.reservation != undefined) {
            this.form.setValue({
                reservationId: this.reservation.reservationId,
                boatName: this.reservation.boat.name,
                flag: this.reservation.boat.flag,
                loa: this.reservation.boat.loa,
                beam: this.reservation.boat.beam,
                draft: this.reservation.boat.draft,
                registryPort: this.reservation.boat.registryPort,
                registryNo: this.reservation.boat.registryNo,
                fromDate: this.reservation.fromDate,
                toDate: this.reservation.toDate,
                berths: [],
                remarks: this.reservation.remarks,
                financialRemarks: this.reservation.financialRemarks,
                isDocked: this.reservation.isDocked,
                isDryDock: this.reservation.isDryDock,
                isAthenian: this.reservation.isAthenian,
                isFishingBoat: this.reservation.boat.isFishingBoat,
                isRequest: this.reservation.isRequest,
                isCash: this.reservation.fee.isCash,
                paymentStatus: { 'id': this.reservation.paymentStatus.id, 'description': this.reservation.paymentStatus.description },
                boatType: { 'id': this.reservation.boat.type.id, 'description': this.reservation.boat.type.description },
                boatUsage: { 'id': this.reservation.boat.usage.id, 'description': this.reservation.boat.usage.description },
                issuingAuthority: this.reservation.fishingLicence.issuingAuthority,
                licenceNo: this.reservation.fishingLicence.licenceNo,
                licenceEnds: this.reservation.fishingLicence.licenceEnds,
                insuranceCompany: this.reservation.insurance.insuranceCompany,
                policyNo: this.reservation.insurance.policyNo,
                policyEnds: this.reservation.insurance.policyEnds,
                netAmount: this.reservation.fee.netAmount,
                discountPercent: this.reservation.fee.discountPercent,
                discountAmount: this.reservation.fee.discountAmount,
                netAmountAfterDiscount: this.reservation.fee.netAmountAfterDiscount,
                vatPercent: this.reservation.fee.vatPercent,
                vatAmount: this.reservation.fee.vatAmount,
                grossAmount: this.reservation.fee.grossAmount,
                ownerName: this.reservation.owner.name,
                ownerAddress: this.reservation.owner.address,
                ownerTaxNo: this.reservation.owner.taxNo,
                ownerTaxOffice: this.reservation.owner.taxOffice,
                ownerPassportNo: this.reservation.owner.passportNo,
                ownerPhones: this.reservation.owner.phones,
                ownerEmail: this.reservation.owner.email,
                billingName: this.reservation.billing.name,
                billingAddress: this.reservation.billing.address,
                billingTaxNo: this.reservation.billing.taxNo,
                billingTaxOffice: this.reservation.billing.taxOffice,
                billingPassportNo: this.reservation.billing.passportNo,
                billingPhones: this.reservation.billing.phones,
                billingEmail: this.reservation.billing.email,
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

    private renameFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            this.renameDocumentForm.patchValue({
                oldfilename: file.name,
                newfilename: this.form.value.reservationId + ' ' + file.name
            })
            this.reservationHttpService.rename(this.renameDocumentForm.value).subscribe(x => {
                resolve()
            })
        })
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(reservation: ReservationWriteDto, closeForm: boolean): void {
        this.reservationHttpService.saveReservation(reservation).subscribe({
            next: (response) => {
                this.helperService.doPostSaveFormTasks(
                    response.code == 200 ? this.messageDialogService.success() : '',
                    response.code == 200 ? 'ok' : 'ok',
                    this.parentUrl,
                    closeForm)
                this.localStorageService.deleteItem('reservation')
                this.form.patchValue(
                    {
                        reservationId: response.body.reservationId,
                        postAt: response.body.postAt,
                        putAt: response.body.putAt
                    })
                this.form.markAsPristine()
                this.focusOnField()
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

    private uploadFile = (file: File): Promise<void> => {
        return new Promise<void>((resolve) => {
            const fileToUpload = <File>file
            const formData = new FormData()
            formData.append('x', fileToUpload, fileToUpload.name)
            this.reservationHttpService.upload(formData, { reportProgress: true, observe: 'events' }).subscribe((x) => {
                if (x.type == HttpEventType.Response) {
                    resolve(x)
                }
            })
        })
    }

    private expireDateMustBeInThePast(policyEnds: string): boolean {
        return this.dateHelperService.createDateFromString(policyEnds) > this.dateHelperService.createDateFromString(this.dateHelperService.formatDateToIso(new Date())) ? false : true
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


    get issuingAuthority(): AbstractControl {
        return this.form.get('issuingAuthority')
    }

    get licenceNo(): AbstractControl {
        return this.form.get('licenceNo')
    }

    get licenceEnds(): AbstractControl {
        return this.form.get('licenceEnds')
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


    get discountPercent(): AbstractControl {
        return this.form.get('discountPercent')
    }


    get discountAmount(): AbstractControl {
        return this.form.get('discountAmount')
    }


    get netAmountAfterDiscount(): AbstractControl {
        return this.form.get('netAmountAfterDiscount')
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
