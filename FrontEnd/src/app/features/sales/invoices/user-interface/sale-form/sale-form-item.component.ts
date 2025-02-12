import { Component, Input } from '@angular/core'
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms'

@Component({
    selector: 'hello',
    templateUrl: './sale-form-item.component.html',
    styles: [`h1 { font-family: Lato; }`]
})

export class SaleFormItemComponent {

    @Input() form: FormGroup;

    constructor(private formGroup: FormBuilder) { }

    addOption(): void {
        this.list.push(this.buildListItem());
    }

    removeOption(i: number): void {
        this.list.removeAt(i);
    }

    private buildListItem(): FormGroup {
        return this.formGroup.group({
            item: '',
        });
    }

    get list(): FormArray { return this.form.get('list') as FormArray; }

}