import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { BalanceSheetCriteriaDialogComponent } from '../criteria/balanceSheet-criteria.component'
import { BalanceSheetCriteriaVM } from '../../classes/view-models/criteria/balanceSheet-criteria-vm'
import { BalanceSheetHttpService } from '../../classes/services/balanceSheet-http.service'
import { BalanceSheetVM } from '../../classes/view-models/list/balanceSheet-vm'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'

@Component({
    selector: 'balanceSheet',
    templateUrl: './balanceSheet-parent.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './balanceSheet-parent.component.css']
})

export class BalanceSheetParentComponent {

    //#region variables

    public criteria: BalanceSheetCriteriaVM
    public feature = 'balanceSheetParent'
    public featureIcon = 'balanceSheet'
    public parentUrl = '/home'
    public records: BalanceSheetVM[] = []
    public filteredRecords: BalanceSheetVM[] = []
    public showZeroBalanceRow: boolean = true

    //#endregion

    constructor(private balanceSheetHttpService: BalanceSheetHttpService, private dateHelperService: DateHelperService, private helperService: HelperService, private messageLabelService: MessageLabelService, public dialog: MatDialog) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.setTabTitle()
        this.setListHeight()
    }

    //#endregion

    //#region public methods

    public getCriteria(): string {
        return this.criteria ? this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate) : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(BalanceSheetCriteriaDialogComponent, {
            data: 'balanceSheetCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.initVariables()
                this.buildCriteriaVM(criteria)
                this.loadRecords(this.criteria)
            }
        })
    }

    public onToggleZeroBalanceRows(): void {
        this.toggleZeroBalanceRecords()
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: BalanceSheetCriteriaVM): void {
        this.criteria = {
            companyId: event.companyId,
            fromDate: event.fromDate,
            toDate: event.toDate,
        }
    }

    private loadRecords(criteria: BalanceSheetCriteriaVM): void {
        const x: BalanceSheetCriteriaVM = {
            companyId: criteria.companyId,
            fromDate: criteria.fromDate,
            toDate: criteria.toDate
        }
        this.balanceSheetHttpService.get(x).subscribe(response => {
            this.records = response
            this.filteredRecords = response
        })
    }

    private initVariables(): void {
        this.showZeroBalanceRow = true
    }

    private setListHeight(): void {
        setTimeout(() => {
            document.getElementById('content').style.height = document.getElementById('list-wrapper').offsetHeight - 64 + 'px'
        }, 100)
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private toggleZeroBalanceRecords(): void {
        if (this.showZeroBalanceRow) {
            this.filteredRecords = this.records
        } else {
            this.filteredRecords = this.records.filter(x => x.actualBalance != 0)
        }
    }

    //#endregion

}
