import { Injectable } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { Observable } from 'rxjs'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { LedgerCriteriaVM } from '../../../ledgers/classes/view-models/criteria/ledger-criteria-vm'
import { LedgerHttpService } from '../../../ledgers/classes/services/ledger-http.service'
import { LedgerModalDialogComponent } from '../../user-interface/ledger-modal-dialog/ledger-modal-dialog.component'
import { LedgerVM } from '../../../ledgers/classes/view-models/list/ledger-vm'
import { BalanceSheetCriteriaVM } from '../view-models/criteria/balanceSheet-criteria-vm'

@Injectable({ providedIn: 'root' })

export class LedgerDialogService {

    private response: any
    public records: LedgerVM[] = []

    constructor(public dialog: MatDialog, private dateHelperService: DateHelperService, private ledgerHttpService: LedgerHttpService) { }

    //#region public methods

    public open(criteria: LedgerCriteriaVM, iconStyle: string, actions: string[]): any {
        this.loadRecords(criteria).then(() => {
            this.openDialog(this.records, iconStyle, actions)
        })
    }

    //#endregion

    //#region private methods

    private loadRecords(criteria: LedgerCriteriaVM): Promise<void> {
        return new Promise<void>((resolve) => {
            const x: LedgerCriteriaVM = {
                companyId: criteria.companyId,
                supplierId: criteria.supplierId,
                fromDate: criteria.fromDate,
                toDate: criteria.toDate,
            }
            this.ledgerHttpService.get(x).subscribe(response => {
                this.records = response
                this.records.forEach(record => {
                    record.formattedDate = this.dateHelperService.formatISODateToLocale(record.date)
                })
                resolve()
            })
        })
    }

    private openDialog(records: object, iconStyle: string, actions: string[]): Observable<boolean> {
        this.response = this.dialog.open(LedgerModalDialogComponent, {
            height: '40rem',
            width: '60rem',
            data: {
                message: records,
                iconStyle: iconStyle,
                actions: actions
            },
            panelClass: 'dialog'
        })
        return this.response.afterClosed()
    }

    //#endregion

}
