import { Component, Input, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
import { formatNumber } from '@angular/common'
// Custom
import { HelperService } from 'src/app/shared/services/helper.service'
import { LocalStorageService } from 'src/app/shared/services/local-storage.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { StatisticsCriteriaVM } from '../../classes/view-models/criteria/statisticss-criteria-vm'
import { StatisticsVM } from '../../classes/view-models/list/statistics-vm'

@Component({
    selector: 'statistics-company-table',
    templateUrl: './statistics-company-table.component.html',
    styleUrls: ['../../../../../../assets/styles/custom/lists.css']
})

export class StatisticsCompanyTableComponent {

    //#region variables

    @ViewChild('table') table: Table

    @Input() records: StatisticsVM[]
    @Input() criteria: StatisticsCriteriaVM

    public feature = 'statistics'
    public totals: StatisticsVM
    public selectedRecords: StatisticsVM[] = []

    //#endregion

    constructor(private helperService: HelperService, private localStorageService: LocalStorageService, private messageLabelService: MessageLabelService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.adjustTableHeight()
    }

    ngOnChanges(): void {
        this.calculateTotals()
    }

    //#endregion

    //#region public methods

    public formatNumberToLocale(number: number, decimals = true): string {
        return formatNumber(number, this.localStorageService.getItem('language'), decimals ? '1.2' : '1.0')
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public invertSign(number: number): number {
        return (number < 0 ? number * -1 : -number)
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
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

    private calculateTotals(): void {
        this.totals = {
            supplier: { id: 0, description: '', isActive: true },
            previousBalance: this.records.reduce((sum: number, array: { previousBalance: number }) => sum + array.previousBalance, 0),
            debit: this.records.reduce((sum: number, array: { debit: number }) => sum + array.debit, 0),
            credit: this.records.reduce((sum: number, array: { credit: number }) => sum + array.credit, 0),
            periodBalance: this.records.reduce((sum: number, array: { periodBalance: number }) => sum + array.periodBalance, 0),
            actualBalance: this.records.reduce((sum: number, array: { actualBalance: number }) => sum + array.actualBalance, 0),
        }
    }

    //#endregion
}
