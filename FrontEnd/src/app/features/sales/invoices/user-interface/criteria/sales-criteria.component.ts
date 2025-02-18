import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms'
import { Component, EventEmitter, NgZone, Output } from '@angular/core'
import { MatDialogRef } from '@angular/material/dialog'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { InputTabStopDirective } from 'src/app/shared/directives/input-tabstop.directive'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SaleListFormCriteriaVM } from '../../classes/view-models/criteria/sale-list-form-criteria-vm'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'sales-criteria',
    templateUrl: './sales-criteria.component.html',
    styleUrls: ['./sales-criteria.component.css']
})

export class SalesCriteriaDialogComponent {

    //#region variables

    @Output() outputSelected = new EventEmitter()

    public feature = 'salesCriteria'
    public featureIcon = 'sales'
    public form: FormGroup
    public icon = 'arrow_back'
    public input: InputTabStopDirective
    public parentUrl = '/home'

    //#endregion

    constructor(
        private dateHelperService: DateHelperService, 
        private dialogRef: MatDialogRef<SalesCriteriaDialogComponent>, 
        private formBuilder: FormBuilder,
        private messageLabelService: MessageLabelService,
        private ngZone: NgZone,
        private sessionStorageService: SessionStorageService
    ) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.populateFormFromStoredFields(this.getCriteriaFromStorage())
    }

    //#endregion

    //#region public methods

    public getDateRange(): any[] {
        const x = []
        x.push(this.form.value.fromDate)
        x.push(this.form.value.toDate)
        return x
    }

    // public getHint(id: string, minmax = 0): string {
    //     return this.messageHintService.getDescription(id, minmax)
    // }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
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

    private getCriteriaFromStorage(): any {
        return this.sessionStorageService.getItem(this.feature) ? JSON.parse(this.sessionStorageService.getItem(this.feature)) : ''
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            fromDate: ['', [Validators.required]],
            toDate: ['', [Validators.required]],
        })
    }

    private createCriteriaObject(criteria: SaleListFormCriteriaVM): SaleListFormCriteriaVM {
        return {
            fromDate: criteria.fromDate,
            toDate: criteria.toDate
        }
    }

    private populateFormFromStoredFields(object: any): void {
        this.form.patchValue({
            fromDate: object.fromDate,
            toDate: object.toDate,
        })
    }

    private updateFormControls(event: any): void {
        this.form.patchValue({
            fromDate: this.dateHelperService.formatDateToIso(new Date(event.value.fromDate)),
            toDate: this.dateHelperService.formatDateToIso(new Date(event.value.toDate))
        })
    }

    //#endregion

    //#region getters

    get fromDates(): AbstractControl {
        return this.form.get('fromDate')
    }

    get toDates(): AbstractControl {
        return this.form.get('toDate')
    }

    //#endregion

}
