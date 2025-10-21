import { Component, Input } from '@angular/core'
import { HelperService } from 'src/app/shared/services/helper.service'
import { formatNumber } from '@angular/common'
// Custom
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { EmojiService } from 'src/app/shared/services/emoji.service'
import { InvoiceHttpService } from '../../../invoices/classes/services/invoice-http.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { LedgerVM } from '../../../ledgers/classes/view-models/list/ledger-vm'
import { LedgerCriteriaVM } from '../../../ledgers/classes/view-models/criteria/ledger-criteria-vm'

@Component({
    selector: 'ledger-table',
    templateUrl: 'ledger-table.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './ledger-table.component.css']
})

export class LedgerFromBalanceSheetComponent {

    //#region variables

    @Input() records: LedgerVM[] = []
    @Input() criteria: LedgerCriteriaVM

    public feature = 'ledger'

    //#endregion

    constructor(private dialogService: DialogService, private emojiService: EmojiService, private helperService: HelperService, private invoiceHttpService: InvoiceHttpService, private localStorageService: LocalStorageService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.adjustTableHeight()
    }

    //#endregion

    //#region public methods

    public formatNumberToLocale(number: number, decimals = true): string {
        return formatNumber(number, this.localStorageService.getItem('language'), decimals ? '1.2' : '1.0')
    }

    public getEmoji(anything: any): string {
        return typeof anything == 'string'
            ? this.emojiService.getEmoji(anything)
            : anything ? this.emojiService.getEmoji('green-box') : this.emojiService.getEmoji('empty')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public isNumberNegative(number: number): string {
        return this.helperService.isNumberNegative(number)
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onOpenDocument(filename: string): void {
        if (filename) {
            this.invoiceHttpService.openDocument(filename).subscribe({
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
    }

    //#endregion

    //#region private methods

    private adjustTableHeight(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    //#endregion

}
