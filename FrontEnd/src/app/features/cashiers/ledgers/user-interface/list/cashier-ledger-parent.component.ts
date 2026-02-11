import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { CashierLedgerCriteriaDialogComponent } from '../criteria/cashier-ledger-criteria.component'
import { CashierLedgerCriteriaVM } from '../../classes/view-models/criteria/cashier-ledger-criteria-vm'
import { CashierLedgerFormCriteriaVM } from '../../classes/view-models/criteria/cashier-ledger-form-criteria-vm'
import { CashierLedgerHttpService } from '../../classes/services/cashier-ledger-http.service'
import { CashierLedgerVM } from '../../classes/view-models/list/cashier-ledger-vm'
import { DateHelperService } from '../../../../../shared/services/date-helper.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { MessageLabelService } from '../../../../../shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'cashier-ledger',
    templateUrl: './cashier-ledger-parent.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './cashier-ledger-parent.component.css']
})

export class CashierLedgerParentComponent {

    //#region variables

    public criteria: CashierLedgerCriteriaVM
    public criteriaForm: CashierLedgerFormCriteriaVM
    public feature = 'cashier-ledger-parent'
    public featureIcon = 'ledgers'
    public parentUrl = '/home'
    public records: CashierLedgerVM[] = []

    //#endregion

    constructor(private cashierLedgerHttpService: CashierLedgerHttpService, private dateHelperService: DateHelperService, private helperService: HelperService, private messageLabelService: MessageLabelService, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.setListHeight()
    }

    ngOnDestroy(): void { }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        const z = this.sessionStorageService.getItem('cashier-ledger-criteria') ? JSON.parse(this.sessionStorageService.getItem('cashier-ledger-criteria')) : ''
        return z ? z.company.description + ' - ' + z.safe.description + ' - ' + this.dateHelperService.formatISODateToLocale(z.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(z.toDate) : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(CashierLedgerCriteriaDialogComponent, {
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.buildCriteriaVM(criteria)
                this.loadRecords(this.criteria)
            }
        })
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: CashierLedgerCriteriaVM): void {
        this.criteria = {
            companyId: event.companyId,
            safeId: event.safeId,
            fromDate: event.fromDate,
            toDate: event.toDate,
        }
    }

    private loadRecords(criteria: CashierLedgerCriteriaVM): void {
        const x: CashierLedgerCriteriaVM = {
            companyId: criteria.companyId,
            safeId: criteria.safeId,
            fromDate: criteria.fromDate,
            toDate: criteria.toDate,
        }
        this.cashierLedgerHttpService.get(x).subscribe(response => {
            this.records = response
            this.records.forEach(record => {
                record.formattedDate = this.dateHelperService.formatISODateToLocale(record.date)
            })
        })
    }

    private setListHeight(): void {
        setTimeout(() => {
            document.getElementById('content').style.height = document.getElementById('list-wrapper').offsetHeight - 64 + 'px'
        }, 100)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    //#endregion

}
