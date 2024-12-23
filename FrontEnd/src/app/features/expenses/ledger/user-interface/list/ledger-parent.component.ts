import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { DateHelperService } from '../../../../../shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { LedgerCriteriaDialogComponent } from '../criteria/ledger-criteria.component'
import { LedgerCriteriaVM } from '../../classes/view-models/criteria/ledger-criteria-vm'
import { LedgerHttpService } from '../../classes/services/ledger-http.service'
import { LedgerVM } from '../../classes/view-models/list/ledger-vm'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from '../../../../../shared/services/message-label.service'

@Component({
    selector: 'ledger',
    templateUrl: './ledger-parent.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './ledger-parent.component.css']
})

export class LedgerParentBillingComponent {

    //#region variables

    public criteria: LedgerCriteriaVM
    public feature = 'billingLedger'
    public featureIcon = 'ledgers'
    public parentUrl = '/home'
    public records: LedgerVM[] = []
    private selectedTabIndex = 0

    //#endregion

    constructor(private dateHelperService: DateHelperService, private dialogService: DialogService, private helperService: HelperService, private interactionService: InteractionService, private ledgerHttpService: LedgerHttpService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.subscribeToInteractionService()
        this.setListHeight()
    }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        return this.criteria
            ? this.criteria.supplier.description + ', ' + this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate)
            : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSelectedTabChange(event: number): void {
        this.selectedTabIndex = event
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
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
            fromDate: event.fromDate,
            toDate: event.toDate,
            supplier: event.supplier
        }
    }

    private loadRecords(criteria: LedgerCriteriaVM): void {
        const x: LedgerCriteriaVM = {
            fromDate: criteria.fromDate,
            toDate: criteria.toDate,
            supplier: criteria.supplier
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

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => {
            this.setTabTitle()
        })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria.fromDate = response.fromDate
                this.criteria.toDate = response.toDate
            }
        })
    }

    //#endregion

}
