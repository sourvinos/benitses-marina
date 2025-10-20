import { FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, ElementRef, NgZone, Renderer2 } from '@angular/core'
import { MatDialogRef } from '@angular/material/dialog'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { InvoiceListFormCriteriaVM } from '../../classes/view-models/criteria/invoice-list-form-criteria-vm'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { ValidationService } from 'src/app/shared/services/validation.service'

@Component({
    selector: 'sales-criteria',
    templateUrl: './invoice-criteria.component.html',
    styleUrls: ['./invoice-criteria.component.css']
})

export class InvoiceCriteriaDialogComponent {

    //#region variables

    public feature = 'invoicesListCriteria'
    public form: FormGroup

    //#endregion

    constructor(private elementRef: ElementRef, private dateHelperService: DateHelperService, private dialogRef: MatDialogRef<InvoiceCriteriaDialogComponent>, private formBuilder: FormBuilder, private helperService: HelperService, private messageLabelService: MessageLabelService, private ngZone: NgZone, private renderer: Renderer2, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateFormFromStoredFields(this.getCriteriaFromStorage())
        this.addTabIndexToInput()
    }

    //#endregion

    //#region public methods

    public getDateRange(): any[] {
        const x = []
        x.push(this.form.value.fromDate)
        x.push(this.form.value.toDate)
        return x
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isNotValidDatePeriod(): boolean {
        const days = this.dateHelperService.calculateDays(this.dateHelperService.createDateFromString(this.form.value.fromDate), this.dateHelperService.createDateFromString(this.form.value.toDate))
        return days < 0 || isNaN(days)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    public onSearch(): void {
        this.ngZone.run(() => {
            this.sessionStorageService.saveItem(this.feature, JSON.stringify(this.form.value))
            this.dialogRef.close(this.createCriteriaObject(this.form.value))
        })
    }

    public patchFormWithSelectedDateRange(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    //#endregion

    //#region private methods

    private addTabIndexToInput() {
        this.helperService.addTabIndexToInput(this.elementRef, this.renderer)
    }

    private createCriteriaObject(criteria: InvoiceListFormCriteriaVM): InvoiceListFormCriteriaVM {
        return {
            fromDate: criteria.fromDate,
            toDate: criteria.toDate
        }
    }

    private getCriteriaFromStorage(): any {
        return this.sessionStorageService.getItem(this.feature) ? JSON.parse(this.sessionStorageService.getItem(this.feature)) : ''
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
        }, {
            validator: ValidationService.validDatePeriod
        })
    }

    private populateFormFromStoredFields(object: any): void {
        this.form.patchValue({
            fromDate: object.fromDate,
            toDate: object.toDate,
        })
    }

    //#endregion

}
