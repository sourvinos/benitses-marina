import { ActivatedRoute, Router } from '@angular/router'
import { Component } from '@angular/core'
import { DateAdapter } from '@angular/material/core'
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms'
// Custom
import { DialogService } from '../../../../../shared/services/modal-dialog.service'
import { FormResolved } from 'src/app/shared/classes/form-resolved'
import { HelperService } from 'src/app/shared/services/helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageInputHintService } from 'src/app/shared/services/message-input-hint.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { PriceHttpService } from '../../classes/services/price-http.service'
import { PriceReadDto } from '../../classes/dtos/price-read-dto'
import { PriceWriteDto } from '../../classes/dtos/price-write-dto'
import { SimpleEntity } from '../../../../../shared/classes/simple-entity'

@Component({
    selector: 'price-form',
    templateUrl: './price-form.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './price-form.component.css']
})

export class PriceFormComponent {

    //#region common

    private record: PriceReadDto
    private recordId: number
    public feature = 'priceForm'
    public featureIcon = 'prices'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/prices'

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private dateAdapter: DateAdapter<any>, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private interactionService: InteractionService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageHintService: MessageInputHintService, private messageLabelService: MessageLabelService, private priceService: PriceHttpService, private router: Router) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.setRecordId()
        this.getRecord()
        this.populateFields()
        this.subscribeToInteractionService()
        this.setLocale()
    }

    ngAfterViewInit(): void {
        this.focusOnField()
    }

    //#endregion

    //#region public methods

    public calculateGrossAmountBasedOnNetAmount(fieldName: string, digits: number): void {
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

    public formatPriceField(fieldName: string, digits: number): void {
        this.patchNumericFieldsWithZeroIfNullOrEmpty(fieldName, digits)
        this.form.patchValue({
            [fieldName]: parseFloat(this.form.value[fieldName]).toFixed(digits)
        })
    }

    public getHint(id: string, minmax = 0): string {
        return this.messageHintService.getDescription(id, minmax)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onDelete(): void {
        this.dialogService.open(this.messageDialogService.confirmDelete(), 'question', ['abort', 'ok']).subscribe(response => {
            if (response) {
                this.priceService.delete(this.form.value.id).subscribe({
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

    //#endregion

    //#region private methods

    private flattenForm(): PriceWriteDto {
        return {
            id: this.form.value.id != '' ? this.form.value.id : null,
            code: this.form.value.code,
            description: this.form.value.description,
            englishDescription: this.form.value.englishDescription,
            netAmount: this.form.value.netAmount,
            vatPercent: this.form.value.vatPercent,
            putAt: this.form.value.putAt
        }
    }

    private focusOnField(): void {
        this.helperService.focusOnField()
    }

    private getRecord(): Promise<any> {
        if (this.recordId != undefined) {
            return new Promise((resolve) => {
                const formResolved: FormResolved = this.activatedRoute.snapshot.data['priceForm']
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
            id: '',
            code: ['', [Validators.required]],
            description: ['', [Validators.required]],
            englishDescription: ['', [Validators.required]],
            netAmount: [0, [Validators.required, Validators.min(0), Validators.max(99999.99)]],
            vatPercent: [0, [Validators.required, Validators.min(0), Validators.max(999.99)]],
            vatAmount: [0],
            grossAmount: [0],
            postAt: [''],
            postUser: [''],
            putAt: [''],
            putUser: ['']
        })
    }

    private patchNumericFieldsWithZeroIfNullOrEmpty(fieldName: string, digits: number): void {
        if (this.form.controls[fieldName].value == null || this.form.controls[fieldName].value == '') {
            this.form.patchValue({ [fieldName]: parseInt('0').toFixed(digits) })
        }
    }

    private populateFields(): void {
        if (this.record != undefined) {
            this.form.setValue({
                id: this.record.id,
                code: this.record.code,
                description: this.record.description,
                englishDescription: this.record.englishDescription,
                netAmount: this.record.netAmount.toFixed(2),
                vatPercent: this.record.vatPercent.toFixed(2),
                vatAmount: this.record.vatAmount.toFixed(2),
                grossAmount: this.record.grossAmount.toFixed(2),
                postAt: this.record.postAt,
                postUser: this.record.postUser,
                putAt: this.record.putAt,
                putUser: this.record.putUser
            })
        }
    }

    private resetForm(): void {
        this.form.reset()
    }

    private saveRecord(price: PriceWriteDto): void {
        this.priceService.save(price).subscribe({
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
            this.recordId = x.id
        })
    }

    private subscribeToInteractionService(): void {
        this.interactionService.refreshDateAdapter.subscribe(() => {
            this.setLocale()
        })
    }

    //#endregion

    //#region getters

    get code(): AbstractControl {
        return this.form.get('code')
    }

    get description(): AbstractControl {
        return this.form.get('description')
    }

    get englishDescription(): AbstractControl {
        return this.form.get('englishDescription')
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
