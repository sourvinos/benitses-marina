import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { DateHelperService } from '../../../../../shared/services/date-helper.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { LedgerCriteriaDialogComponent } from '../criteria/ledger-criteria.component'
import { LedgerCriteriaVM } from '../../classes/view-models/criteria/ledger-criteria-vm'
import { LedgerFormCriteriaVM } from '../../classes/view-models/criteria/ledger-form-criteria-vm'
import { LedgerHttpService } from '../../classes/services/ledger-http.service'
import { LedgerVM } from '../../classes/view-models/list/ledger-vm'
import { MessageLabelService } from '../../../../../shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'

@Component({
    selector: 'ledger',
    templateUrl: './ledger-parent.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './ledger-parent.component.css']
})

export class LedgerParentBillingComponent {

    //#region variables

    public criteria: LedgerCriteriaVM
    public criteriaForm: LedgerFormCriteriaVM
    public feature = 'ledgerParent'
    public featureIcon = 'ledgers'
    public parentUrl = '/home'
    public records: LedgerVM[] = []

    //#endregion

    constructor(private dateHelperService: DateHelperService, private helperService: HelperService, private ledgerHttpService: LedgerHttpService, private messageLabelService: MessageLabelService, private sessionStorageService: SessionStorageService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.setListHeight()
    }

    ngOnDestroy(): void {
        
    }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        const z = this.sessionStorageService.getItem('ledgerCriteria') ? JSON.parse(this.sessionStorageService.getItem('ledgerCriteria')) : ''
        return z ? z.company.description + ' - ' + z.supplier.description + ' - ' + this.dateHelperService.formatISODateToLocale(z.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(z.toDate) : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(LedgerCriteriaDialogComponent, {
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

    private buildCriteriaVM(event: LedgerCriteriaVM): void {
        this.criteria = {
            companyId: event.companyId,
            supplierId: event.supplierId,
            fromDate: event.fromDate,
            toDate: event.toDate,
        }
    }

    private loadRecords(criteria: LedgerCriteriaVM): void {
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
