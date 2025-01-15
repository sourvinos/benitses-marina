import { Component, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
// Custom
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'ledger-modal-dialog',
    templateUrl: './ledger-modal-dialog.component.html',
    styleUrls: ['./ledger-modal-dialog.component.css']
})

export class LedgerModalDialogComponent {

    //#region variables

    private feature = 'ledger-modal-dialog'
    public content: any
    public iconStyle: any

    //#endregion

    constructor(@Inject(MAT_DIALOG_DATA) public data: any, private dialogRef: MatDialogRef<LedgerModalDialogComponent>, private messageLabelService: MessageLabelService) {
        this.iconStyle = data.iconStyle
    }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.content = this.data.message
    }

    //#endregion

    //#region public methods

    public getLabel(id: string): any {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onClose(): void {
        this.dialogRef.close()
    }

    //#endregion

}
