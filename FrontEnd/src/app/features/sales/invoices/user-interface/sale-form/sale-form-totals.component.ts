import { AbstractControl, FormBuilder, FormGroup } from '@angular/forms'
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

    public form: FormGroup
    public feature = 'saleForm'

    @Input() saleTotals: SaleTotalsVM
    @Output() exportTotals = new EventEmitter()

    constructor(private messageLabelService: MessageLabelService, private formBuilder: FormBuilder) { }

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

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    get qty(): AbstractControl {
        return this.form.get('saleQty')
    }

    get subTotal(): AbstractControl {
        return this.form.get('saleSubTotal')
    }

    get totalVatAmount(): AbstractControl {
        return this.form.get('SaleVatAmount')
    }


}