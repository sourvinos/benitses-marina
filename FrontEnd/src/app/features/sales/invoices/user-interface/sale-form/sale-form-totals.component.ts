import { FormBuilder, FormGroup } from '@angular/forms'
import { Component, EventEmitter, Input, Output } from '@angular/core'
// Custom
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SaleTotalsVM } from '../../classes/view-models/form/sale-totals-vm'

@Component({
    selector: 'sale-form-totals',
    templateUrl: './sale-form-totals.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/forms.css', './sale-form-totals.component.css']
})

export class SaleFormTotalsComponent {

    //#region variables

    public form: FormGroup
    public feature = 'saleForm'

    @Input() saleTotals: SaleTotalsVM
    @Output() exportTotals = new EventEmitter()

    //#endregion

    constructor(private formBuilder: FormBuilder, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.form = this.formBuilder.group({
            qty: this.saleTotals.qty || 0,
            subTotal: this.saleTotals.subTotal || 0,
            vatAmount: this.saleTotals.vatAmount || 0,
            grossAmount: this.saleTotals.grossAmount || 0
        })
    }

    ngOnChanges(): void {
        this.exportTotals.emit()
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    //#endregion
 
}