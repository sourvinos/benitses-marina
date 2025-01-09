import { Component } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { HelperService } from '../../../../../shared/services/helper.service'
import { InteractionService } from 'src/app/shared/services/interaction.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticsCriteriaDialogComponent } from '../criteria/statistics-criteria-dialog.component'
import { StatisticsCriteriaVM } from '../../classes/view-models/criteria/statisticss-criteria-vm'
import { StatisticsFormCriteriaVM } from '../../classes/view-models/criteria/statisticss-form-criteria-vm'
import { StatisticsHttpService } from '../../classes/services/statistics-http.service'
import { StatisticsVM } from '../../classes/view-models/list/statistics-vm'

@Component({
    selector: 'statistics',
    templateUrl: './statistics-parent.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css', './statistics-parent.component.css']
})

export class StatisticsParentComponent {

    //#region variables

    public criteria: StatisticsCriteriaVM
    public criteriaForm: StatisticsFormCriteriaVM
    public feature = 'statisticsParent'
    public featureIcon = 'statistics'
    public parentUrl = '/home'
    public records: StatisticsVM[] = []
    public filteredRecords: StatisticsVM[] = []
    public showZeroBalanceRow: boolean = true

    //#endregion

    constructor(private statisticsHttpService: StatisticsHttpService, private dateHelperService: DateHelperService, private helperService: HelperService, private interactionService: InteractionService, private messageLabelService: MessageLabelService, public dialog: MatDialog) { }

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
            ? this.dateHelperService.formatISODateToLocale(this.criteria.fromDate) + ' - ' + this.dateHelperService.formatISODateToLocale(this.criteria.toDate)
            : ''
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public onSelectedTabChange(): void {
        setTimeout(() => {
            const x = document.getElementsByClassName('table-wrapper') as HTMLCollectionOf<HTMLInputElement>
            for (let i = 0; i < x.length; i++) {
                x[i].style.height = document.getElementById('content').offsetHeight - 150 + 'px'
            }
        }, 100)
    }

    public onShowCriteriaDialog(): void {
        const dialogRef = this.dialog.open(StatisticsCriteriaDialogComponent, {
            data: 'statisticsCriteria',
            height: '36.0625rem',
            panelClass: 'dialog',
            width: '32rem',
        })
        dialogRef.afterClosed().subscribe(criteria => {
            if (criteria !== undefined) {
                this.initVariables()
                this.buildCriteriaVM(criteria)
                this.loadRecords(this.criteria).then(() => {
                    this.records.forEach(record => {
                        record.periodBalance = record.debit - record.credit
                    });
                })
            }
        })
    }

    public onToggleZeroBalanceRows(): void {
        this.toggleZeroBalanceRecords()
    }

    //#endregion

    //#region private methods

    private buildCriteriaVM(event: StatisticsCriteriaVM): void {
        this.criteria = {
            companyId: event.companyId,
            balanceFilterId: event.companyId,
            fromDate: event.fromDate,
            toDate: event.toDate,
        }
    }

    private loadRecords(criteria: StatisticsCriteriaVM): Promise<any> {
        return new Promise((resolve) => {
            const x: StatisticsCriteriaVM = {
                companyId: criteria.companyId,
                balanceFilterId: criteria.balanceFilterId,
                fromDate: criteria.fromDate,
                toDate: criteria.toDate
            }
            this.statisticsHttpService.get(x).subscribe(response => {
                this.records = response
                this.filteredRecords = response
                resolve(this.records)
            })
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

    private subscribeToInteractionService(): void {
        this.interactionService.refreshTabTitle.subscribe(() => { this.setTabTitle() })
        this.interactionService.emitDateRange.subscribe((response) => {
            if (response) {
                this.criteria.fromDate = response.fromDate
                this.criteria.toDate = response.toDate
            }
        })
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
